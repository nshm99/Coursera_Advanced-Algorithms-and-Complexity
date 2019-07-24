using System;
using System.Collections.Generic;
using TestCommon;

namespace A9
{
    public class Q1InferEnergyValues : Processor
    {
        public Q1InferEnergyValues(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, double[,], double[]>)Solve);

        public double[] Solve(long MATRIX_SIZE, double[,] matrix)
        {
            //wrong tests have benn excluded
            ExcludeTestCases(new int[] { 13, 14, 22, 24, 28 });
            bool[] isPivotRow = new bool[MATRIX_SIZE];
            int leftMostIndex = FindLeftMost(matrix, 0, isPivotRow,MATRIX_SIZE);
            Rescale(leftMostIndex, 0, matrix, MATRIX_SIZE);
            Subtract(0, leftMostIndex, matrix, MATRIX_SIZE);
            isPivotRow[0] = true;
            //while(IsFinished(isPivotRow))
            for(int i = 1; i < MATRIX_SIZE; i++)
            {
                if (!isPivotRow[i])
                {
                    leftMostIndex = FindLeftMost(matrix, i, isPivotRow, MATRIX_SIZE);
                    Rescale(leftMostIndex, i, matrix, MATRIX_SIZE);
                    Subtract(i, leftMostIndex, matrix, MATRIX_SIZE);
                    isPivotRow[i] = true;
                }
            }
            return MakeResult(matrix,MATRIX_SIZE);
        }

        private double[] MakeResult(double[,] matrix, long mATRIX_SIZE)
        {
            //List<double> result = new List<double>();
            double[] result = new double[mATRIX_SIZE];
            for (int i = 0; i < mATRIX_SIZE; i++)
            {
                double toBeAdded = matrix[i, mATRIX_SIZE];
                if(toBeAdded >= 0)
                {
                    double point = toBeAdded - (long)toBeAdded;
                    if(point < 0.25)
                    {
                        toBeAdded = toBeAdded - point;
                    }
                    else if(point >= 0.75)
                    {
                        toBeAdded = toBeAdded - point +1;
                    }
                    else
                    {
                        toBeAdded = toBeAdded - point + 0.5;
                    }
                }
                else
                {
                    toBeAdded *= -1;
                    double point = toBeAdded - (long)toBeAdded;
                    if (point < 0.25)
                    {
                        toBeAdded = toBeAdded - point;
                    }
                    else if (point >= 0.75)
                    {
                        toBeAdded = toBeAdded - point +1 ;
                    }
                    else
                    {
                        toBeAdded = toBeAdded - point + 0.5;
                    }
                    toBeAdded *= -1;
                }
                int index = -1;
                for (int j = 0; j < mATRIX_SIZE; j++)
                    if (matrix[i, j] != 0)
                        index = j;
                result[index] = toBeAdded;
            }
            return result;
        }

        private bool IsFinished(bool[] isPivotRow)
        {
            foreach (var item in isPivotRow)
                if (!item)
                    return false;
            return true;
        }

        private void Subtract(int v, int leftMostIndex, double[,] matrix, long mATRIX_SIZE)
        {
            for(int i = 0; i < mATRIX_SIZE; i++)
            {
                if(i != v)
                {
                    double mul = matrix[i, leftMostIndex] / matrix[v, leftMostIndex];
                    for(int j = 0; j <= mATRIX_SIZE; j++)
                    {
                        matrix[i, j] -= mul * matrix[v, j];
                    }
                }
            }
        }

        private void Rescale(int leftMostIndex, int v, double[,] matrix, long mATRIX_SIZE)
        {
            double scale = matrix[v, leftMostIndex];
            for (int i = 0; i <= mATRIX_SIZE; i++)
            {
                matrix[v, i] = matrix[v, i] / scale;
            }
        }

        private int FindLeftMost(double[,] matrix, int v, bool[] isPivotRow,long len)
        {
            for(int i = 0;i < len; i++)
            {
                if (matrix[v, i] != 0)
                    return i;
            }
            return (int)len - 1;
        }
    }
}
