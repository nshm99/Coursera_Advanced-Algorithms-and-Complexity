using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3
{
    public class Q2CleaningApartment : Processor
    {
        public Q2CleaningApartment(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, long[,], string[]>)Solve);

        public long max = long.MinValue;
        public string[] Solve(int V, int E, long[,] matrix)
        {
            List<List<long>> clauses = new List<List<long>>();
            List<long>[] adjArray = new List<long>[V];
            for (int i = 0; i < V; i++)//VERTEXES
            {
                adjArray[i] = new List<long>();
                ExactlyOneVertex(clauses, i, V);
                
            }
            for (int i = 1; i <= V; i++)//POSITIONS
            {
                ExactlyOnePosition(clauses, i, V);
            }
            
            for (int i = 0; i < E; i++)
            {
                adjArray[matrix[i, 0]-1].Add(matrix[i, 1]-1);
                adjArray[matrix[i, 1]-1].Add(matrix[i, 0]-1);
            }
            for(int i = 1; i < V; i++)
            {
                for(int j = 0; j < V; j++)
                {
                    for(int k = 0; k < V; k++)
                    {
                        if(!adjArray[j].Contains(k) && k != j)
                        {
                            clauses.Add(new List<long>() { -1 * (V * j + i), -1 * ((V *k) + i + 1) });
                        }
                    }
                    
                }
            }
            string[] s = MakeResult(clauses, V, E);
            return s;
        }

        private void ExactlyOneVertex(List<List<long>> clauses, int index, int v)
        {
            List<long> literals = new List<long>();
            for (int i = 1; i <= v; i++)
            {
                if (v * (index) + i > max)
                    max = v * (index) + i;
                literals.Add(v * (index) + i);
            }
            clauses.Add(literals);
            for (int i = 1; i <= v; i++)
            {
                for (int j = i + 1; j <= v; j++)
                {
                    literals = new List<long>();
                    if ((v * (index) + i) > max)
                        max = v * (index) + i;
                    literals.Add(-1 * (v * (index) + i));
                    if ((v * (index) + j) > max)
                        max = (v * (index) + j);
                    literals.Add(-1 * (v * (index) + j));
                    clauses.Add(literals);
                }
            }
        }

        private void ExactlyOnePosition(List<List<long>> clauses, int index, int v)
        {
            List<long> literals = new List<long>();
            for (int i = 0; i < v; i++)
            {
                if (v * (i) + index > max)
                    max = v * (i) + index;
                literals.Add(v * (i) + index);
            }
            clauses.Add(literals);
            for (int i = 0; i < v; i++)
            {
                for (int j = i + 1; j < v; j++)
                {
                    literals = new List<long>();
                    if (v * (i) + index > max)
                        max = v * (i) + index;
                    literals.Add(-1 * (v * (i) + index));
                    if (v * (j) + index > max)
                        max = v * (j) + index;
                    literals.Add(-1 * (v * (j) + index));
                    clauses.Add(literals);
                }
            }
        }

        private string[] MakeResult(List<List<long>> clauses, int v, int e)
        {

            List<string> result = new List<string>();
            result.Add($"{clauses.Count} {v * v}");

            for (int i = 0; i < clauses.Count; i++)
            {
                string toBeAdded = clauses[i][0].ToString();
                for (int j = 1; j < clauses[i].Count; j++)
                {
                    toBeAdded += " ";
                    toBeAdded += clauses[i][j].ToString();
                    //result.Add(clauses[i][j].ToString());
                }
                result.Add(toBeAdded + " 0");

            }

            return result.ToArray();
        }

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;
    }
}
