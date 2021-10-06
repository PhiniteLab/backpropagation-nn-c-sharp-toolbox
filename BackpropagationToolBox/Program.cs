using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BackpropagationToolBox
{
    class Program
    {
        /// <summary>
        /// Implementation of Backpropagation Algorithm in C#      
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            // Create the time series with 0.01 step size
            float[] t = MatrixHelpers.Instance.CreateIncrementalVector(0, 0.01f, 1);
            int trainingNumber = t.Length;

            // Input vector
            float[] x_0 = MatrixHelpers.Instance.Ones(trainingNumber);
            float[] x_1 = t;
            float[,] xGeneralInput = MatrixHelpers.Instance.CombineColumnVectors2Matrix(x_0, x_1);


            // Output vector
            float[,] yGeneralTarget = MatrixHelpers.Instance.CreateSineOutput(x_1);

            // NN Parameters
            int number_of_input_layer_node = 2;
            int number_of_hidden_layer_node = 8;
            int number_of_output_layer_node = 1;

            int I = number_of_input_layer_node;
            int H = number_of_hidden_layer_node;
            int K = number_of_output_layer_node;

            // boundary conditions
            float errorMax = 0.00001f;
            int iterationMax = 50000;

            // internal training parameters
            float learningRate = 0.01f;

            float[,] errorNow = MatrixHelpers.Instance.Ones(trainingNumber, K);
            float[,] errorPre = MatrixHelpers.Instance.Ones(trainingNumber, K);

            float errorNowValue = MatrixHelpers.Instance.Sum(errorNow) / trainingNumber;
            float errorPreValue = MatrixHelpers.Instance.Sum(errorPre) / trainingNumber;


            // NN structure initialization
            float[,] W = MatrixHelpers.Instance.CreateRandomMatrix(-1, 1, H, I);
            float[,] W_pre = MatrixHelpers.Instance.CreateRandomMatrix(-1, 1, H, I);

            float[,] V = MatrixHelpers.Instance.CreateRandomMatrix(-1, 1, K, H + 1);
            float[,] V_pre = MatrixHelpers.Instance.CreateRandomMatrix(-1, 1, K, H + 1);


            float[,] dW = MatrixHelpers.Instance.Zeros(H, I);
            float[,] dV = MatrixHelpers.Instance.Zeros(K, H + 1);


            float[,] y = MatrixHelpers.Instance.Zeros(K, trainingNumber);
            float[,] z = MatrixHelpers.Instance.Zeros(H + 1, trainingNumber);

            // Training Process
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int iteration = 0;

            while ((errorNowValue > errorMax) && (iteration < iterationMax))
            {
                iteration = iteration + 1;
                errorPre = errorNow;

                dW = MatrixHelpers.Instance.Zeros(H, I);
                dV = MatrixHelpers.Instance.Zeros(K, H + 1);

                // Implementing Parallel For for performance improvement
                Parallel.For(0, trainingNumber, i =>
                {

                    MatrixHelpers.Instance.ReplaceColumnOfAMatrix(ref z, i, PhiNNParameters.Instance.ActivationFunctionCalculation(xGeneralInput, W, H, I, i));

                    y[0, i] = PhiNNParameters.Instance.OutputFunctionCalculation(z, V, K, H, i)[0, 0];

                    dV = PhiNNParameters.Instance.UpdateGradientOfV(y, yGeneralTarget, z, dV, learningRate, K, H, i);

                    dW = PhiNNParameters.Instance.UpdateGradientOfW(y, yGeneralTarget, z, xGeneralInput, V, dW, learningRate, K, H, I, i);

                    for (int k = 0; k < K; k++)
                    {
                        errorNow[i, k] = PhiNNParameters.Instance.CalculateCostFunction(y, yGeneralTarget, K, i);
                    }

                    errorNowValue = (float)MatrixHelpers.Instance.Sum(errorNow) / trainingNumber;

                    V = MatrixHelpers.Instance.SumMatrices(V, MatrixHelpers.Instance.MultiplyMatrixWithScalar((1.0f / trainingNumber), dV));
                    W = MatrixHelpers.Instance.SumMatrices(W, MatrixHelpers.Instance.MultiplyMatrixWithScalar((1.0f / trainingNumber), dW));

                });

                if (iteration % 100 == 0)
                {
                    Console.WriteLine("Error: " + errorNowValue + " at iteration " + iteration);
                }


            }

            Console.WriteLine("Total elapsed Time: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();


            // Test Process with trained Weights
            float[,] y_test = MatrixHelpers.Instance.Zeros(K, trainingNumber);
            float[,] z_test = MatrixHelpers.Instance.Zeros(H + 1, trainingNumber);

            for (int i = 0; i < trainingNumber; i++)
            {
                MatrixHelpers.Instance.ReplaceColumnOfAMatrix(ref z_test, i, PhiNNParameters.Instance.ActivationFunctionCalculation(xGeneralInput, W, H, I, i));
                y_test[0, i] = PhiNNParameters.Instance.OutputFunctionCalculation(z_test, V, K, H, i)[0, 0];

            }


            // Create csv files for plotting results. Saved to the Application Current Directory
            MatrixHelpers.Instance.SaveMatrix(y_test, "\\output.csv");
            MatrixHelpers.Instance.SaveMatrix(yGeneralTarget, "\\input.csv");

            Console.ReadLine();
        }






    }
}
