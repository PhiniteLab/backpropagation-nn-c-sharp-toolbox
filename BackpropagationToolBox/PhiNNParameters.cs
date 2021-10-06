using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackpropagationToolBox
{
    public class PhiNNParameters
    {
        /// <summary>
        /// Singleton Pattern Implementation
        /// </summary>
        private static PhiNNParameters instance = null;
        private static readonly object padlock = new object();
        public static PhiNNParameters Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new PhiNNParameters();
                        }
                    }
                }
                return instance;
            }
        }


        public float[,] ActivationFunctionCalculation(float[,] inputs, float[,] weights, int hiddenLayerNumber, int inputLayerNumber, int index)
        {

            float[,] output = MatrixHelpers.Instance.Zeros(hiddenLayerNumber + 1, 1);
            float sum;
            for (int i = 0; i < hiddenLayerNumber + 1; i++)
            {
                if (i == (hiddenLayerNumber))
                {
                    output[i, 0] = 1.0f;
                }
                else
                {
                    sum = 0.0f;

                    for (int j = 0; j < inputLayerNumber; j++)
                    {
                        sum += weights[i, j] * inputs[j, index];

                    }
                    output[i, 0] = 1.00f / (1.00f + (float)(Math.Exp(-sum)));
                }
            }

            return output;

        }


        public float[,] OutputFunctionCalculation(float[,] inputs, float[,] weights, int outputLayerNumber, int hiddenLayerNumber, int index)
        {
            float[,] output = MatrixHelpers.Instance.Zeros(outputLayerNumber, 1);
            float sum;
            for (int i = 0; i < outputLayerNumber; i++)
            {
                sum = 0.0f;
                for (int j = 0; j < hiddenLayerNumber + 1; j++)
                {
                    sum += weights[i, j] * inputs[j, index];
                }

                output[i, 0] = sum;
            }

            return output;
        }



        public float[,] UpdateGradientOfV(float[,] yAct, float[,] yTrain, float[,] zAct, float[,] weights, float learningRate, int outputLayerNumber, int hiddenLayerNumber, int index)
        {
            float[,] output = weights;

            for (int i = 0; i < outputLayerNumber; i++)
            {
                for (int j = 0; j < hiddenLayerNumber + 1; j++)
                {
                    output[i, j] = output[i, j] + learningRate * (yTrain[i, index] - yAct[i, index]) * zAct[j, index];

                }

            }
            return output;
        }



        public float[,] UpdateGradientOfW(float[,] yAct, float[,] yTrain, float[,] zAct, float[,] xInput, float[,] Vweights, float[,] dWweights,
                                           float learningRate, int outputLayerNumber, int hiddenLayerNumber, int inputLayerNumber, int index)
        {
            float[,] dW = dWweights;
            float sum;
            for (int i = 0; i < hiddenLayerNumber; i++)
            {
                for (int j = 0; j < inputLayerNumber; j++)
                {
                    sum = 0;
                    for (int k = 0; k < outputLayerNumber; k++)
                    {
                        sum += (yTrain[k, index] - yAct[k, index]) * Vweights[k, i];
                    }
                    dW[i, j] = dW[i, j] + learningRate * sum * zAct[i, index] * (1 - zAct[i, index]) * xInput[j, index];
                }

            }
            return dW;
        }

        public float CalculateCostFunction(float[,] yTrain, float[,] yTarget, int outputLayerNumber, int index)
        {
            float costValue = 0;

            for (int i = 0; i < outputLayerNumber; i++)
            {

                costValue += (0.5f * (yTarget[i, index] - yTrain[i, index]) * (yTarget[i, index] - yTrain[i, index]));
            }

            return costValue;
        }
    }
}
