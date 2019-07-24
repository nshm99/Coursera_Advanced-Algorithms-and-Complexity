//using Microsoft.SolverFoundation.Solvers;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TestCommon;

namespace A3
{
    public class Q1FrequencyAssignment : Processor
    {
        public Q1FrequencyAssignment(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, long[,], string[]>)Solve);

        public string[] Solve(int V, int E, long[,] matrix)
        {
            List<List<long>> clauses = new List<List<long>>();
            for (int i = 1; i <= V; i++)
                ExactlyOneOf(i, matrix, V, clauses);
            for (int i = 0; i < E; i++)
                Adjancy(matrix, i, clauses);
            string[] s = MakeResult(clauses, V, E);
            return s;
        }

        private string[] MakeResult(List<List<long>> clauses, int v, int e)
        {

            List<string> result = new List<string>();
            result.Add($"{clauses.Count} {v * 3}");
            
            for(int i = 0; i < clauses.Count; i++)  
            {
                string toBeAdded = clauses[i][0].ToString();
                for (int j = 1; j< clauses[i].Count; j++)
                {
                    toBeAdded += " ";
                    toBeAdded += clauses[i][j].ToString();
                    //result.Add(clauses[i][j].ToString());
                }
                result.Add(toBeAdded+" 0");
                
            }
            
            return result.ToArray();
        }

        private void Adjancy(long[,] matrix, int index, List<List<long>> clauses)
        {
            for (int i = 1; i <= 3; i++)
            {
                List<long> adj = new List<long>();
                adj.Add(-1 * (3 * (matrix[index, 0] - 1) + i));
                adj.Add(-1 * (3 * (matrix[index, 1] - 1) + i));
                if(!clauses.Contains(adj))
                    clauses.Add(adj);
            }
        }

        private void ExactlyOneOf(int index, long[,] matrix, int v, List<List<long>> clauses)
        {
            List<long> literals = new List<long>();
            for (int i = 1; i <= 3; i++)
                literals.Add(3 * (index - 1) + i);  
            clauses.Add(literals);
            for(int i = 1; i <= 3; i++)
            {
                for(int j = i+1; j <= 3; j++)
                {
                    literals = new List<long>();
                    literals.Add(-1 * (3 * (index - 1) + i));
                    literals.Add(-1 * (3 * (index - 1) + j));
                    clauses.Add(literals);
                }
            }
            
        }

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;
    }   
}
