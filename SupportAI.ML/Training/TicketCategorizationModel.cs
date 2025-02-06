using Microsoft.ML;
using Microsoft.ML.Data;
using SupportAI.ML.Models;

namespace SupportAI.ML.Training;

public class TicketCategorizationModel
{
    private static readonly MLContext _mlContext = new();
    private static ITransformer? _model;

    public static void TrainModel()
    {
        var dataPath = "SupportAI.ML/Data/tickets.csv"; // Training Dataset
        IDataView dataView = _mlContext.Data.LoadFromTextFile<TicketData>(dataPath, separatorChar: ',', hasHeader: true);

        var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(TicketData.IssueDescription))
            .Append(_mlContext.Transforms.Conversion.MapValueToKey(nameof(TicketData.Category)))
            .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Category", "Features"))
            .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

        _model = pipeline.Fit(dataView);

        _mlContext.Model.Save(_model, dataView.Schema, "SupportAI.ML/Models/TicketCategorizationModel.zip");
    }
}
