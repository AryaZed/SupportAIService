using Microsoft.ML;
using Microsoft.ML.Data;
using SupportAI.ML.Models;

namespace SupportAI.ML.Training
{
    public class SentimentAnalysisModel
    {
        public static void TrainModel(string dataPath, string modelPath)
        {
            // Create an MLContext
            var mlContext = new MLContext(seed: 1);

            // Load data
            IDataView dataView = mlContext.Data.LoadFromTextFile<SentimentData>(
                dataPath, hasHeader: true, separatorChar: ',');

            // Split data into train and test sets
            var splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

            // Build a data processing pipeline
            var pipeline = mlContext.Transforms.Text.FeaturizeText(
                                outputColumnName: "Features",
                                inputColumnName: nameof(SentimentData.Text))
                           .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                                labelColumnName: nameof(SentimentData.Sentiment),
                                featureColumnName: "Features"));

            // Train the model
            var model = pipeline.Fit(splitData.TrainSet);

            // Evaluate the model (optional)
            var predictions = model.Transform(splitData.TestSet);
            var metrics = mlContext.BinaryClassification.Evaluate(predictions, labelColumnName: nameof(SentimentData.Sentiment));
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");

            // Save the model
            mlContext.Model.Save(model, splitData.TrainSet.Schema, modelPath);
        }
    }
}
