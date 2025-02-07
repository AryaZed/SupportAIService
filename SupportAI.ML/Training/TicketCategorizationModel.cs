using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
using SupportAI.ML.Models;
using SupportAI.ML.Transforms;

namespace SupportAI.ML.Training;

public class TicketCategorizationModel
{
    private static readonly MLContext _mlContext = new();
    private static ITransformer? _model;

    public static void TrainModel()
    {
        var dataPath = "SupportAI.ML/Data/tickets_fa.csv"; // Persian Dataset
        IDataView dataView = _mlContext.Data.LoadFromTextFile<TicketData>(dataPath, separatorChar: ',', hasHeader: true);

        var stopWordRemover = new CustomPersianStopWordsTransformer(_mlContext);
        IDataView transformedData = stopWordRemover.Fit(dataView).Transform(dataView);

        var pipeline = _mlContext.Transforms.Text.NormalizeText("NormalizedText", nameof(ProcessedTicketData.IssueDescription),
                TextNormalizingEstimator.CaseMode.Lower, keepDiacritics: false, keepNumbers: false, keepPunctuations: false)
            .Append(_mlContext.Transforms.Text.TokenizeIntoWords("Tokens", "NormalizedText"))
            .Append(_mlContext.Transforms.Text.FeaturizeText("Features", "Tokens"))
            .Append(_mlContext.Transforms.Conversion.MapValueToKey(nameof(ProcessedTicketData.Category)))
            .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Category", "Features"))
            .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

        _model = pipeline.Fit(transformedData);

        _mlContext.Model.Save(_model, transformedData.Schema, "SupportAI.ML/Models/TicketCategorizationModel.zip");
    }
}
