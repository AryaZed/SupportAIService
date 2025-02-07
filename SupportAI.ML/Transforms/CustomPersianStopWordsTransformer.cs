using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SupportAI.ML.Models;
using SupportAI.ML.Utils;

namespace SupportAI.ML.Transforms
{
    // Custom Transformer to Remove Persian Stop Words
    public class CustomPersianStopWordsTransformer : IEstimator<CustomPersianStopWordsTransformer.ModelTransformer>
    {
        private readonly MLContext _mlContext;
        private readonly string _inputColumn;
        private readonly string _outputColumn;

        public CustomPersianStopWordsTransformer(MLContext mlContext, string inputColumn = "IssueDescription", string outputColumn = "ProcessedDescription")
        {
            _mlContext = mlContext;
            _inputColumn = inputColumn;
            _outputColumn = outputColumn;
        }

        public ModelTransformer Fit(IDataView input) => new ModelTransformer(_mlContext, _inputColumn, _outputColumn, input.Schema);

        public SchemaShape GetOutputSchema(SchemaShape inputSchema) => inputSchema;

        public class ModelTransformer : ITransformer, IRowToRowMapper
        {
            private readonly MLContext _mlContext;
            private readonly string _inputColumn;
            private readonly string _outputColumn;
            private readonly DataViewSchema _inputSchema;
            private readonly DataViewSchema _outputSchema;

            public ModelTransformer(MLContext mlContext, string inputColumn, string outputColumn, DataViewSchema inputSchema)
            {
                _mlContext = mlContext;
                _inputColumn = inputColumn;
                _outputColumn = outputColumn;
                _inputSchema = inputSchema;

                // Build output schema: include all input columns plus our new output column.
                var builder = new DataViewSchema.Builder();
                foreach (var col in _inputSchema)
                    builder.AddColumn(col.Name, col.Type);
                // Use TextDataViewType.Instance for string/text columns.
                builder.AddColumn(_outputColumn, TextDataViewType.Instance);
                _outputSchema = builder.ToSchema();
            }

            public bool IsRowToRowMapper => true;

            public DataViewSchema InputSchema => _inputSchema;

            public DataViewSchema OutputSchema => _outputSchema;

            public DataViewSchema GetOutputSchema(DataViewSchema inputSchema)
            {
                var builder = new DataViewSchema.Builder();
                foreach (var col in inputSchema)
                    builder.AddColumn(col.Name, col.Type);
                builder.AddColumn(_outputColumn, TextDataViewType.Instance);
                return builder.ToSchema();
            }

            public IRowToRowMapper GetRowToRowMapper(DataViewSchema inputSchema) => this;

            public IDataView Transform(IDataView input) => new TransformedDataView(input, this);

            public IEnumerable<DataViewSchema.Column> GetDependencies(IEnumerable<DataViewSchema.Column> dependingColumns)
            {
                // We depend on the input column that we need to process.
                return _inputSchema.Where(col => col.Name == _inputColumn);
            }

            public DataViewRow GetRow(DataViewRow input, IEnumerable<DataViewSchema.Column> activeColumns)
            {
                return new RowMapper(input, activeColumns, _inputColumn, _outputColumn);
            }

            public void Save(ModelSaveContext ctx) => throw new NotImplementedException();
        }

        private class RowMapper : DataViewRow
        {
            private readonly DataViewRow _inputRow;
            private readonly string _inputColumn;
            private readonly string _outputColumn;

            public RowMapper(DataViewRow inputRow, IEnumerable<DataViewSchema.Column> activeColumns, string inputColumn, string outputColumn)
            {
                _inputRow = inputRow;
                _inputColumn = inputColumn;
                _outputColumn = outputColumn;
            }

            public override DataViewSchema Schema => _inputRow.Schema;

            public override long Position => _inputRow.Position;

            public override long Batch => _inputRow.Batch;

            // NEW: Override IsColumnActive to delegate to the input row.
            public override bool IsColumnActive(DataViewSchema.Column column) => _inputRow.IsColumnActive(column);

            public override ValueGetter<TValue> GetGetter<TValue>(DataViewSchema.Column column)
            {
                if (column.Name == _outputColumn)
                {
                    var inputGetter = _inputRow.GetGetter<ReadOnlyMemory<char>>(_inputRow.Schema[_inputColumn]);
                    return (ref TValue value) =>
                    {
                        ReadOnlyMemory<char> inputText = default;
                        inputGetter(ref inputText);
                        string cleanedText = RemoveStopWords(inputText.ToString());
                        value = (TValue)(object)cleanedText.AsMemory();
                    };
                }
                return _inputRow.GetGetter<TValue>(column);
            }

            public override ValueGetter<DataViewRowId> GetIdGetter() => _inputRow.GetIdGetter();

            private static string RemoveStopWords(string text)
            {
                var stopWords = PersianStopWords.StopWords;
                var words = text.Split(' ').Where(word => !stopWords.Contains(word));
                return string.Join(" ", words);
            }
        }

        private class TransformedDataView : IDataView
        {
            private readonly IDataView _input;
            private readonly IRowToRowMapper _mapper;

            public TransformedDataView(IDataView input, IRowToRowMapper mapper)
            {
                _input = input;
                _mapper = mapper;
            }

            public bool CanShuffle => _input.CanShuffle;

            public DataViewSchema Schema => _mapper.OutputSchema;

            public long? GetRowCount() => _input.GetRowCount();

            public DataViewRowCursor GetRowCursor(IEnumerable<DataViewSchema.Column> columns, Random rand = null)
            {
                var inputCursor = _input.GetRowCursor(_mapper.GetDependencies(columns), rand);
                return new TransformedRowCursor(inputCursor, _mapper, columns);
            }

            public DataViewRowCursor[] GetRowCursorSet(IEnumerable<DataViewSchema.Column> columns, int n, Random rand = null)
            {
                var cursors = _input.GetRowCursorSet(_mapper.GetDependencies(columns), n, rand);
                return cursors.Select(c => new TransformedRowCursor(c, _mapper, columns)).ToArray();
            }
        }

        private class TransformedRowCursor : DataViewRowCursor
        {
            private readonly DataViewRowCursor _inputCursor;
            private readonly IRowToRowMapper _mapper;
            private readonly DataViewRow _mappedRow;

            public TransformedRowCursor(DataViewRowCursor inputCursor, IRowToRowMapper mapper, IEnumerable<DataViewSchema.Column> activeColumns)
            {
                _inputCursor = inputCursor;
                _mapper = mapper;
                _mappedRow = _mapper.GetRow(_inputCursor, activeColumns);
            }

            public override DataViewSchema Schema => _mappedRow.Schema;

            public override long Position => _inputCursor.Position;

            public override long Batch => _inputCursor.Batch;

            // NEW: Override IsColumnActive to delegate to the mapped row.
            public override bool IsColumnActive(DataViewSchema.Column column) => _mappedRow.IsColumnActive(column);

            public override bool MoveNext() => _inputCursor.MoveNext();

            public override ValueGetter<TValue> GetGetter<TValue>(DataViewSchema.Column column) => _mappedRow.GetGetter<TValue>(column);

            public override ValueGetter<DataViewRowId> GetIdGetter() => _mappedRow.GetIdGetter();
        }
    }
}
