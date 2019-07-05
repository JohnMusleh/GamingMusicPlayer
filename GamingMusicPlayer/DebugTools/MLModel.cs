using System;
using System.Windows.Forms;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace GamingMusicPlayer.DebugTools
{
    public class MLModel
    {
        private MLContext mlContext=new MLContext(seed: 0);
        private TransformerChain<RegressionPredictionTransformer<LinearRegressionModelParameters>> trainedModel=null;
        public MLModel()
        {
            Console.WriteLine("ATTEMPTING TO CREATE MODEL");
            // STEP 1: Common data loading configuration
            Console.WriteLine(Application.StartupPath);
            IDataView baseTrainingDataView = mlContext.Data.LoadFromTextFile<ModelDataType>(Application.StartupPath+"\\DebugTools\\learning_data.csv", hasHeader: true, separatorChar: ',');

            // STEP 2: Common data process configuration with pipeline data transformations
            var dataProcessPipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(ModelDataType.Score))
                .Append(mlContext.Transforms.NormalizeMinMax(outputColumnName: nameof(ModelDataType.BPM)))
                .Append(mlContext.Transforms.NormalizeMinMax(outputColumnName: nameof(ModelDataType.ZCR)))
                .Append(mlContext.Transforms.NormalizeMinMax(outputColumnName: nameof(ModelDataType.SpectIrr)))
                .Append(mlContext.Transforms.Concatenate("Features",nameof(ModelDataType.BPM), nameof(ModelDataType.ZCR), nameof(ModelDataType.SpectIrr)));


            //var trainer = mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");
            var trainer = mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");
            var trainingPipeline = dataProcessPipeline.Append(trainer);

            this.trainedModel = trainingPipeline.Fit(baseTrainingDataView);

            Console.WriteLine("CREATED MODEL");
        }

        public double predict(float bpm, float zcr, float spectirr)
        {
            var sample = new ModelDataType()
            {
                BPM = 0,
                ZCR = 0,
                SpectIrr = spectirr,
                Score = -1
            };
            // Create prediction engine related to the loaded trained model
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelDataType, ModelPrediction>(trainedModel);

            var resultprediction = predEngine.Predict(sample);
            return resultprediction.score;
        }
    }
}
