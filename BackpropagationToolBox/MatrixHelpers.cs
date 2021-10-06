using System;
using System.IO;


namespace BackpropagationToolBox
{
    public class MatrixHelpers
    {
        /// <summary>
        /// Singleton Pattern Implementation
        /// </summary>
        private static MatrixHelpers instance = null;
        private static readonly object padlock = new object();
        public static MatrixHelpers Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new MatrixHelpers();
                        }
                    }
                }
                return instance;
            }
        }



        public void ReplaceColumnOfAMatrix(ref float[,] replacedMatrix, int replacedColumn, float[,] replaceWithMatrix)
        {

            int col1 = replacedMatrix.GetLength(0);
            int col2 = replaceWithMatrix.GetLength(0);
            if (col1 == col2)
            {
                float length = col1;

                for (int i = 0; i < length; i++)
                {
                    replacedMatrix[i, replacedColumn] = replaceWithMatrix[i, 0];
                }

            }
            else
            {
                Console.WriteLine("ReplaceColumnOfAMatrix ERROR");
            }


        }

        public void SaveMatrix(float[,] matrix, string fileName)
        {
            string path = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(path + fileName);

            if (File.Exists(filePath))

                File.Delete(filePath);

            foreach (var item in matrix)
            {
                using (StreamWriter writetext = new StreamWriter(filePath, true))
                {
                    writetext.WriteLine(item.ToString().Replace(",", "."));
                }
            }
        }


        public void PrintVector(float[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                Console.WriteLine(vector[i]);
            }

        }

        public void PrintMatrix(float[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public float[] Ones(int length)
        {
            if (length < 0) return null;
            float[] array = new float[length];
            for (int i = 0; i < length; i++)
                array[i] = 1;

            return array;
        }

        public float[,] Ones(int row, int col)
        {

            float[,] matrix = new float[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    matrix[i, j] = 1;
                }
            }

            return matrix;
        }

        public float[] Zeros(int length)
        {
            if (length < 0) return null;
            float[] array = new float[length];
            for (int i = 0; i < length; i++)
                array[i] = 0;

            return array;
        }

        public float[,] Zeros(int row, int col)
        {

            float[,] matrix = new float[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    matrix[i, j] = 0;
                }
            }

            return matrix;
        }

        public float[] CreateIncrementalVector(float start, float inc, float end)
        {
            float[] array;
            if (end > start)
            {
                int length = (int)((end - start) / inc) + 1;
                array = new float[length];
                for (int i = 0; i < length; i++)
                    array[i] = i * inc + start;
            }
            else
            {
                return null;
            }

            return array;
        }

        public float[,] CombineColumnVectors2Matrix(float[] firstVector, float[] secondVector)
        {
            int firstVectorLength = firstVector.Length;
            int secondVectorLength = secondVector.Length;

            float[,] matrix;

            if (firstVectorLength == secondVectorLength)
            {
                int length = firstVectorLength;
                matrix = new float[2, length];

                for (int i = 0; i < length; i++)
                {
                    matrix[0, i] = firstVector[i];

                }
                for (int j = 0; j < length; j++)
                {
                    matrix[1, j] = secondVector[j];
                }

                return matrix;

            }
            else
            {
                return null;
            }

        }

        public float[] MultiplyVectorWithScalar(float scalar, float[] vector)
        {
            float[] result = new float[vector.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = scalar * vector[i];
            }

            return result;

        }

        public float[,] MultiplyMatrixWithScalar(float scalar, float[,] matrix)
        {
            float[,] result = new float[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = scalar * matrix[i, j];
                }
            }

            return result;

        }

        public float[,] SumMatrices(float[,] matrix1, float[,] matrix2)
        {

            float[,] result = new float[matrix1.GetLength(0), matrix2.GetLength(1)];

            if (matrix1.GetLength(0) == matrix2.GetLength(0) && matrix1.GetLength(1) == matrix2.GetLength(1))
                for (int i = 0; i < matrix1.GetLength(0); i++)
                    for (int j = 0; j < matrix1.GetLength(1); j++)
                        result[i, j] = matrix1[i, j] + matrix2[i, j];


            return result;

        }



        public float Randomize(Random random, float min, float max)
        {

            return (float)random.NextDouble() * (max - min) + min;

        }

        public float[,] CreateRandomMatrix(float min, float max, int row, int col)
        {
            float[,] matrix = new float[row, col];
            Random random = new Random();
            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                    matrix[i, j] = Randomize(random, min, max);

            return matrix;
        }

        public float[,] CreateSineOutput(float[] vector)
        {
            float[,] result = new float[1, vector.Length];
            for (int i = 0; i < result.Length; i++)
                result[0, i] = (float)Math.Sin(2 * Math.PI * 1 * vector[i]);


            return result;

        }


        public float Sum(float[] vector)
        {
            float sum = 0.0f;
            for (int i = 0; i < vector.Length; i++)

                sum += vector[i];

            return sum;

        }

        public float Sum(float[,] matrix)
        {
            float sum = 0.0f;
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    sum += matrix[i, j];

            return sum;

        }
    }
}
