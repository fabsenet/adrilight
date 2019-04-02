using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NighlightDetectionModelGenerator
{

    public enum NightLightStatusEnum
    {
        Unknown,
        On,
        Off,
    }

    /*
BitConverter.ToString(offBytes)
"02-00-00-00-C4-D1-F8-EF-3F-4D-D3-01-00-00-00-00-43-42-01-00-C6-14-8A-F7-E2-FF-FE-A7-D3-E9-01-00"
BitConverter.ToString(data) [off nach on?!]
"02-00-00-00-79-BB-29-F3-89-4E-D3-01-00-00-00-00-43-42-01-00-D0-0A-02-C6-14-93-B3-A6-99-9F-D1-D3-E9-01-00"
BitConverter.ToString(onBytes)
"02-00-00-00-D1-1D-11-A6-89-4E-D3-01-00-00-00-00-43-42-01-00-10-00-D0-0A-02-C6-14-8E-A2-C4-B0-9A-D1-D3-E9-01-00"
     */
    public class NightLightStatusWatcher
    {
        private const string DataFile = "data.csv";

        private List<byte[]> onByteArrays = new List<byte[]> {
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 186, 228, 235, 226, 5, 42, 43, 14, 21, 67, 66, 1, 0, 16, 0, 208, 10, 2, 198, 20, 136, 210, 174, 251, 161, 197, 175, 234, 1, 0, 0, 0, 0 },
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 183, 229, 235, 226, 5, 42, 43, 14, 21, 67, 66, 1, 0, 16, 0, 208, 10, 2, 198, 20, 165, 131, 183, 207, 166, 197, 175, 234, 1, 0, 0, 0, 0 },
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 167, 230, 235, 226, 5, 42, 43, 14, 21, 67, 66, 1, 0, 16, 0, 208, 10, 2, 198, 20, 211, 142, 154, 231, 170, 197, 175, 234, 1, 0, 0, 0, 0 },
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 131, 231, 235, 226, 5, 42, 43, 14, 21, 67, 66, 1, 0, 16, 0, 208, 10, 2, 198, 20, 184, 212, 197, 154, 174, 197, 175, 234, 1, 0, 0, 0, 0},
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 184, 232, 235, 226, 5, 42, 43, 14, 21, 67, 66, 1, 0, 16, 0, 208, 10, 2, 198, 20, 141, 160, 133, 249, 180, 197, 175, 234, 1, 0, 0, 0, 0 },
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 137, 235, 235, 226, 5, 42, 43, 14, 21, 67, 66, 1, 0, 16, 0, 208, 10, 2, 198, 20, 198, 235, 156, 193, 193, 197, 175, 234, 1, 0, 0, 0, 0 },
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 212, 238, 235, 226, 5, 42, 43, 14, 21, 67, 66, 1, 0, 16, 0, 208, 10, 2, 198, 20, 176, 252, 194, 165, 201, 197, 175, 234, 1, 0, 0, 0, 0 },
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 252, 249, 235, 226, 5, 42, 43, 14, 21, 67, 66, 1, 0, 16, 0, 208, 10, 2, 198, 20, 168, 227, 135, 200, 136, 198, 175, 234, 1, 0, 0, 0, 0 },
        };

        private List<byte[]> offByteArrays = new List<byte[]> {
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 152, 185, 233, 226, 5, 42, 43, 14, 16, 67, 66, 1, 0, 198, 20, 174, 217, 223, 251, 142, 186, 175, 234, 1, 0, 0, 0, 0 },
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 179, 229, 235, 226, 5, 42, 43, 14, 19, 67, 66, 1, 0, 208, 10, 2, 198, 20, 220, 195, 248, 189, 166, 197, 175, 234, 1, 0, 0, 0, 0 },
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 186, 229, 235, 226, 5, 42, 43, 14, 19, 67, 66, 1, 0, 208, 10, 2, 198, 20, 225, 151, 191, 223, 166, 197, 175, 234, 1, 0, 0, 0, 0 },
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 198, 230, 235, 226, 5, 42, 43, 14, 19, 67, 66, 1, 0, 208, 10, 2, 198, 20, 144, 200, 165, 249, 171, 197, 175, 234, 1, 0, 0, 0, 0 },
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 164, 231, 235, 226, 5, 42, 43, 14, 19, 67, 66, 1, 0, 208, 10, 2, 198, 20, 151, 133, 202, 186, 175, 197, 175, 234, 1, 0, 0, 0, 0 },
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 189, 232, 235, 226, 5, 42, 43, 14, 19, 67, 66, 1, 0, 208, 10, 2, 198, 20, 231, 218, 214, 145, 181, 197, 175, 234, 1, 0, 0, 0, 0 },
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 143, 235, 235, 226, 5, 42, 43, 14, 19, 67, 66, 1, 0, 208, 10, 2, 198, 20, 223, 208, 186, 222, 193, 197, 175, 234, 1, 0, 0, 0, 0 },
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 181, 239, 235, 226, 5, 42, 43, 14, 19, 67, 66, 1, 0, 208, 10, 2, 198, 20, 213, 128, 176, 138, 202, 197, 175, 234, 1, 0, 0, 0, 0 },
            new byte[]{ 67, 66, 1, 0, 10, 2, 1, 0, 42, 6, 131, 129, 236, 226, 5, 42, 43, 14, 19, 67, 66, 1, 0, 208, 10, 2, 198, 20, 252, 148, 129, 151, 170, 198, 175, 234, 1, 0, 0, 0, 0 },
        };

        class NightLightDataRow
        {
            public bool IsActive { get; }

          //  [Microsoft.ML.Data.VectorType(43)]
            public byte[] Data { get; }

            [Microsoft.ML.Data.VectorType(38)]
            public float[] Data2
            {get;
            }

            public NightLightDataRow()
            {

            }

            public NightLightDataRow(byte[] data, bool isActive)
            {
                Data = data;
                Data2 = Data.Select(d => (float)d).Take(38).ToArray();
                IsActive = isActive;
            }
        }

        class NightLightState
        {
            public float Probability { get; set; }
            public bool PredictedLabel { get; set; }

        }

        private PredictionEngine<NightLightDataRow, NightLightState> _predictor;
        internal void TrainAndGuess()
        {
            MLContext mlContext = new MLContext();

            var data = mlContext.Data.LoadFromEnumerable(
                onByteArrays.Select(arr => new NightLightDataRow(arr, true))
                .Concat(offByteArrays.Select(arr => new NightLightDataRow(arr, false))).Reverse()
                .ToList());

            var trainingPipeLine = mlContext.Transforms.Conversion.MapValueToKey(nameof(NightLightDataRow.IsActive))
                .AppendCacheCheckpoint(mlContext)
                .Append(
                        mlContext.BinaryClassification.Trainers.StochasticDualCoordinateAscent(
                            labelColumnName: nameof(NightLightDataRow.IsActive)
                            , featureColumnName: nameof(NightLightDataRow.Data2)
                            )
                )
                //      .Append(mlContext.Transforms.Conversion.MapKeyToValue(nameof(NightLightState.PredictedLabel)))
                ;

            var p = trainingPipeLine.Preview(data);

            //var trainingPipeLine = 
            //    mlContext.Transforms.Conversion.ConvertType("Data")
            //    .AppendCacheCheckpoint(mlContext)
            //    .Append(
            //    mlContext.BinaryClassification.Trainers.StochasticDualCoordinateAscent(
            //                labelColumnName: nameof(NightLightDataRow.IsActive)
            //                , featureColumnName: nameof(NightLightDataRow.Data)
            //                ));

            var sw = Stopwatch.StartNew();
            // Evaluate the model using cross-validation.
            // Cross-validation splits our dataset into 'folds', trains a model on some folds and 
            // evaluates it on the remaining fold. We are using 5 folds so we get back 5 sets of scores.
            // Let's compute the average AUC, which should be between 0.5 and 1 (higher is better).


            //Console.WriteLine("=============== Cross-validating to get model's accuracy metrics ===============");
            //var crossValidationResults = mlContext.BinaryClassification.CrossValidate(
            //    data: data, estimator: trainingPipeLine, numFolds: 5, labelColumn: nameof(NightLightDataRow.IsActive));
            //var aucs = crossValidationResults.Select(r => r.Metrics.Auc);
            //Console.WriteLine("The AUC is {0}", aucs.Average());

            // Now let's train a model on the full dataset to help us get better results
            var model = trainingPipeLine.Fit(data);
            Console.WriteLine($"training took {sw.Elapsed}");

            using (var f = File.Open("NightLightDetectionModel.zip", FileMode.Create))
            {
                model.SaveTo(mlContext, f);
                f.Close();
            }
            Console.WriteLine($"saved model");
            // Create a PredictionFunction from our model 
            _predictor = model.CreatePredictionEngine<NightLightDataRow, NightLightState>(mlContext);

            Predict();
        }

        private void Predict()
        {
            if (_predictor == null)
            {
                Console.WriteLine("Predictor not trained yet!");
                return;
            }

            Console.WriteLine("=============== Predictions for below data===============");

            var predicted = _predictor.Predict(new NightLightDataRow(this._lastData, true));
            Console.WriteLine($"predicted1: {predicted.PredictedLabel}, {predicted.Probability:0.00000}");
            predicted = _predictor.Predict(new NightLightDataRow(this._lastData, !true));
            Console.WriteLine($"predicted2: {predicted.PredictedLabel}, {predicted.Probability:0.00000}");

            Console.WriteLine("=============== End of process, hit any key to finish =============== ");
        }
        public void AddLast(bool isOn)
        {
            byte[] lastData = _lastData;
            if (lastData == null) return;

            var list = isOn ? onByteArrays : offByteArrays;

            if(list.All(bs => !bs.SequenceEqual(lastData)))
            {
                Console.WriteLine($"added as {(isOn?"ON":"OFF")}.");
                list.Add(lastData);
            }
        }

        public void LoadFromFile()
        {
            if (!File.Exists(DataFile)) return;

            var ons = new List<byte[]>();
            var offs = new List<byte[]>();
            var sr = new StreamReader(DataFile);

            string line = sr.ReadLine();//skip the header

            while((line = sr.ReadLine()) != null)
            {
                var bytes = line.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(s => Convert.ToByte(s)).ToArray();
                var isOn = line.StartsWith("on");
                (isOn ? ons : offs).Add(bytes);
            }
            sr.Close();

            onByteArrays = ons;
            offByteArrays = offs;
        }
        public void WriteToFile()
        {
            var sw = new StreamWriter(DataFile);
            var maxLength = onByteArrays.Concat(offByteArrays).Select(arr => arr.Length).Max();

            //header
            sw.Write("state;");
            for (int i = 0; i < maxLength; i++)
            {
                sw.Write($"d{i + 1};");
            }
            sw.WriteLine();

            //data
            foreach (var data in onByteArrays)
            {
                sw.WriteLine($"on;{data.Select(b => b.ToString()).Aggregate((s1, s2) => s1 + ";" + s2)};");
            }
            foreach (var data in offByteArrays)
            {
                sw.WriteLine($"off;{data.Select(b => b.ToString()).Aggregate((s1, s2) => s1 + ";" + s2)};");
            }
            sw.Close();

            Console.WriteLine("File written.");
        }

        public NightLightStatusWatcher(CancellationToken token)
        {
            LoadFromFile();

            AnalyzeBytes("+", onByteArrays);
            Console.WriteLine();
            AnalyzeBytes("-", offByteArrays);


            _token = token;
            _watchTask = WatchStatusInternal();
        }

        private void AnalyzeBytes(string indicator, List<byte[]> bytesArray)
        {
            foreach(var bytes in bytesArray)
            Console.WriteLine($"{indicator} {bytes.Length}");
        }

        private RegistryKey _stateKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\CloudStore\Store\DefaultAccount\Current\default$windows.data.bluelightreduction.bluelightreductionstate\windows.data.bluelightreduction.bluelightreductionstate", false);

        private TimeSpan _waitDelay = TimeSpan.FromSeconds(1);

        private async Task WatchStatusInternal()
        {
            while (!_token.IsCancellationRequested)
            {
                WatchOnce();
                await Task.Delay(_waitDelay, _token);
            }
        }

        private byte[] _lastData;

        private void WatchOnce()
        {
            var data = (byte[])_stateKey.GetValue("Data");

            if (_lastData != null && _lastData.SequenceEqual(data)) return;
            _lastData = data;

            var status = ConvertDataToStatusEnum(data);

            if (status != NightLightStatusEnum.Unknown)
            {
                Console.WriteLine($"{status}");
            }
            else
            {
                Console.WriteLine($"{status}; {data.Select(b => b.ToString()).Aggregate((s1, s2) => s1 + "," + s2)}");
            }
            Predict();

        }

        private NightLightStatusEnum ConvertDataToStatusEnum(byte[] data)
        {
            if (onByteArrays.Any(onByteArray => onByteArray.SequenceEqual(data))) return NightLightStatusEnum.On;
            if (offByteArrays.Any(offByteArray => offByteArray.SequenceEqual(data))) return NightLightStatusEnum.Off;

            return NightLightStatusEnum.Unknown;
        }

        private Task _watchTask;

        private CancellationToken _token;
    }
}
