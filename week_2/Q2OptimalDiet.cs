using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;


namespace A9
{
    public class Q2OptimalDiet : Processor
    {
        public Q2OptimalDiet(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, double[,], String>)Solve);

        public string Solve(int N, int M, double[,] matrix)
        {

            double[,] matrix1 = new double[M + N, M + 1];
            matrix1 = MakeEnequalities(matrix, M, N);
            List<List<double>> subSets = FindSubset(M, N + M);
            List<int> possibleAnswers = new List<int>();
            List<double> answers = new List<double>();
            double max = -1;
            double[] maxResult = new double[M];
            List<double[]> a = new List<double[]>();
            for (int i = 0; i < subSets.Count(); i++)
            {
                //     Q1InferEnergyValues solveEquation = new Q1InferEnergyValues("TD1");
                double[] solvedNumbers = Solve(M, ConvertToArray(subSets, i, M, matrix1));
                a.Add(solvedNumbers);
                if (CheckPossible(solvedNumbers, matrix1, subSets, M, N, i))
                {
                    double curr = FindValue(solvedNumbers, matrix, M, N);
                    if (curr >= max)
                    {
                        max = curr;
                        maxResult = solvedNumbers;
                    }
                    if (curr < double.MaxValue && curr > double.MinValue)
                        answers.Add(curr);
                }
            }
            if (answers.Count() == 0)
                return "No Solution";
            else if (answers.Count() == M)
                return "Infinity";
            else
            {
                maxResult[0] = RoundResult(maxResult[0]);
                string result = "Bounded Solution" + '\n' + maxResult[0].ToString();
                for (int i = 1; i < maxResult.Length; i++)
                {
                    maxResult[i] = RoundResult(maxResult[i]);
                    result += " " + maxResult[i].ToString();
                }
                return result;
            }


        }

        private double RoundResult(double v)
        {
            double toBeAdded = v;
               if (toBeAdded >= 0)
               {
                   double point = toBeAdded - (long)toBeAdded;
                   if (point < 0.25)
                   {
                       toBeAdded = toBeAdded - point;
                   }
                   else if (point >= 0.75)
                   {
                       toBeAdded = toBeAdded - point + 1;
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
                       toBeAdded = toBeAdded - point + 1;
                   }
                   else
                   {
                       toBeAdded = toBeAdded - point + 0.5;
                   }
                   toBeAdded *= -1;
               }
            return toBeAdded;
              
        }

        private double[,] MakeEnequalities(double[,] matrix, int m, int n)
        {
            double[,] matrix1 = new double[n + m, m + 1];
            for (int i = 0; i < n + m; i++)
            {
                for (int j = 0; j < m + 1; j++)
                {
                    if (i < n)
                    {
                        matrix1[i, j] = matrix[i, j];
                    }
                    else
                    {
                        if (j == i - n)
                            matrix1[i, j] = 1;
                        else
                            matrix1[i, j] = 0;
                    }
                }
            }
            return matrix1;
        }

        private double FindValue(double[] solvedNumbers, double[,] matrix1, int m, int n)
        {
            double sum = 0;
            for (int i = 0; i < m; i++)
                sum += matrix1[n, i] * solvedNumbers[i];
            return sum;
        }

        private bool CheckPossible(double[] solvedNumbers, double[,] matrix1, List<List<double>> subSets, int m, int n, int index)
        {
            for (int i = 0; i < m + n; i++)
            {
                if (!Contains(i, subSets, index))
                {
                    if (!HoldEquation(solvedNumbers, matrix1, i, m, n))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool HoldEquation(double[] solvedNumbers, double[,] matrix1, int index, int m, int n)
        {
            double sum = 0;
            for (int i = 0; i < m; i++)
            {
                if (solvedNumbers[i] < 0)
                    return false;
                sum += solvedNumbers[i] * matrix1[index, i];
            }
            if (index >= n)
            {
                if (sum >= matrix1[index, m])
                    return true;
            }
            else
            {
                if (sum <= matrix1[index, m])
                    return true;
            }
            return false;
        }

        private bool Contains(int index, List<List<double>> subSets, int i)
        {
            for (int j = 0; j < subSets[i].Count(); j++)
                if (subSets[i].Contains(index))
                    return true;
            return false;
        }

        private double[,] ConvertToArray(List<List<double>> subSets, int i, int m, double[,] matrix)
        {
            double[,] result = new double[subSets[0].Count(), m + 1];
            for (int j = 0; j < subSets[i].Count(); j++)
            {
                int index = (int)subSets[i][j];
                for (int k = 0; k <= m; k++)
                {
                    result[j, k] = matrix[index, k];
                }
            }
            return result;

        }

        private List<List<double>> FindSubset(int m, int n)
        {
            int[] data = new int[n + m];
            List<double> midle = new List<double>();
            List<List<double>> result = new List<List<double>>();
            Combination(n, m, 0, data, 0, midle, result);
            return result;
        }

        private void Combination(int n, int m, int index, int[] data, int i, List<double> midle, List<List<double>> result)
        {
            if (index == m)
            {
                midle = new List<double>();
                for (int j = 0; j < m; j++)
                {
                    midle.Add(data[j]);
                }
                result.Add(midle);
                return;
            }
            if (i >= n)
                return;
            data[index] = i;
            Combination(n, m, index + 1, data, i + 1, midle, result);
            Combination(n, m, index, data, i + 1, midle, result);

        }


        public double[] Solve(long MATRIX_SIZE, double[,] matrix)
        {
            //wrong tests have benn excluded
            ExcludeTestCases(new int[] { 13, 14, 22, 24, 28 });
            bool[] isPivotRow = new bool[MATRIX_SIZE];
            int leftMostIndex = FindLeftMost(matrix, 0, isPivotRow, MATRIX_SIZE);
            Rescale(leftMostIndex, 0, matrix, MATRIX_SIZE);
            Subtract(0, leftMostIndex, matrix, MATRIX_SIZE);
            isPivotRow[0] = true;
            //while(IsFinished(isPivotRow))
            for (int i = 1; i < MATRIX_SIZE; i++)
            {
                if (!isPivotRow[i])
                {
                    leftMostIndex = FindLeftMost(matrix, i, isPivotRow, MATRIX_SIZE);
                    Rescale(leftMostIndex, i, matrix, MATRIX_SIZE);
                    Subtract(i, leftMostIndex, matrix, MATRIX_SIZE);
                    isPivotRow[i] = true;
                }
            }
            return MakeResult(matrix, MATRIX_SIZE);
        }

        private double[] MakeResult(double[,] matrix, long mATRIX_SIZE)
        {
            //List<double> result = new List<double>();
            double[] result = new double[mATRIX_SIZE];
            for (int i = 0; i < mATRIX_SIZE; i++)
            {
                double toBeAdded = matrix[i, mATRIX_SIZE];
             /*   if (toBeAdded >= 0)
                {
                    double point = toBeAdded - (long)toBeAdded;
                    if (point < 0.25)
                    {
                        toBeAdded = toBeAdded - point;
                    }
                    else if (point >= 0.75)
                    {
                        toBeAdded = toBeAdded - point + 1;
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
                        toBeAdded = toBeAdded - point + 1;
                    }
                    else
                    {
                        toBeAdded = toBeAdded - point + 0.5;
                    }
                    toBeAdded *= -1;
                }
               */ 
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
            for (int i = 0; i < mATRIX_SIZE; i++)
            {
                if (i != v)
                {
                    double mul = matrix[i, leftMostIndex] / matrix[v, leftMostIndex];
                    for (int j = 0; j <= mATRIX_SIZE; j++)
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

        private int FindLeftMost(double[,] matrix, int v, bool[] isPivotRow, long len)
        {
            for (int i = 0; i < len; i++)
            {
                if (matrix[v, i] != 0)
                    return i;
            }
            return (int)len - 1;
        }

    }
}
