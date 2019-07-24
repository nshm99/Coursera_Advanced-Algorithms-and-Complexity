using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3
{
    public class Q3AdBudgetAllocation : Processor
    {
        public Q3AdBudgetAllocation(string testDataName)
            : base(testDataName)
        { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long[], string[]>)Solve);

        public string[] Solve(long eqCount, long varCount, long[][] A, long[] b)
        {
            List<long> nonZeroes = new List<long>();
            List<List<long>> subsets = new List<List<long>>();
            List<List<long>> clauses = new List<List<long>>();
            List<long> literals = new List<long>();
            for (int j = 0; j < varCount; j++)
            {
                literals.Add(j + 1);
            }
            clauses.Add(literals);
            for (int i = 0; i < eqCount; i++)
            {
                nonZeroes = new List<long>();
                for (int j = 0; j < varCount; j++)
                {
                    if (A[i][j] != 0)
                        nonZeroes.Add(j);
                }

                //List<long> currSet = new List<long>();
                //for (int m = 0; m <= nonZeroes.Count(); m++)
                subsets = FindSubset(1, nonZeroes.Count());
                for (int j = 0; j < subsets.Count(); j++)
                {
                    literals = new List<long>();
                    long currSum = 0;
                    for (int k = 0; k < subsets[j].Count; k++)
                    {
                        currSum += A[i][subsets[j][k]];
                    }
                    if (currSum > b[i])
                    {
                        for (int a = 0; a < subsets[j].Count; a++)
                        {
                            if (nonZeroes.Contains(subsets[j][a]))
                                literals.Add(-1 * (subsets[j][a] + 1));
                            else
                                literals.Add(subsets[j][a] + 1);
                        }
                        if (literals.Count() != 0)
                            clauses.Add(literals);
                        /*literals = new List<long>();
                        for (int a = 0; a < nonZeroes.Count(); a++)
                        {
                            if (!subsets[j].Contains(a))
                                literals.Add(a + 1);
                        }
                        if (literals.Count() != 0)
                            clauses.Add(literals);
                            */
                    }

                }

            }
            //if (clauses.Count() == 0)
            //return new string[] { "1 1", "-1 1 0"};
            string[] s = MakeResult(clauses, varCount);
            return s;
        }

        private string[] MakeResult(List<List<long>> clauses, long v)
        {

            List<string> result = new List<string>();
            result.Add($"{clauses.Count} {v * v}");

            for (int i = 0; i < clauses.Count; i++)
            {
                string toBeAdded = "";
                if (clauses[i].Count() != 0)
                    toBeAdded = clauses[i][0].ToString();
                for (int j = 1; j < clauses[i].Count; j++)
                {
                    toBeAdded += " ";
                    if (clauses[i].Count() != 0)
                        toBeAdded += clauses[i][j].ToString();
                    //result.Add(clauses[i][j].ToString());
                }
                result.Add(toBeAdded + " 0");

            }

            return result.ToArray();
        }

        private List<List<long>> FindSubset(int m, int n)
        {
            List<List<long>> result = new List<List<long>>();
            for (int i = 1; i <= n; i++)
            {
                int[] data = new int[n];
                List<long> midle = new List<long>();
                Combination(n, i, 0, data, 0, midle, result);
            }
            return result;
        }

        private void Combination(int n, int m, int index, int[] data, int i, List<long> midle, List<List<long>> result)
        {
            if (index == m)
            {
                midle = new List<long>();
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

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;
    }
}
