using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;
//using Microsoft.SolverFoundation.Solvers;

namespace A11
{
    public class Q1CircuitDesign : Processor
    {
        public Q1CircuitDesign(string testDataName) : base(testDataName)
        {
            ExcludeTestCases(10);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], Tuple<bool, long[]>>)Solve);

        public override Action<string, string> Verifier =>
            TestTools.SatAssignmentVerifier;

        public virtual Tuple<bool, long[]> Solve(long v, long c, long[][] cnf)
        {
            long[][] implicationGraph = new long[(2 * v) + 1][];
            long[] assignments = new long[(2 * v) + 1];
            for (int i = 0; i < implicationGraph.Length; i++)
                implicationGraph[i] = new long[(2 * v) + 1];
            ImplicationGraph(v, c, cnf, implicationGraph);
            long[] SCC = new long[(2 * v) + 1];
            long sccCount = FindSCC(SCC, (2 * v) , implicationGraph);
            for(int i = 1; i <= v; i++)
            {
                if (SCC[i] == SCC[NegationOf(i, v)])
                    return new Tuple<bool, long[]>(false, null);
            }
            for(long i = sccCount-1; i > 0; i--)
            {
                for(int j = 1; j <= 2*v; j++)
                {
                    if(SCC[j] == i)
                    {
                        if(assignments[j] == 0)
                        {
                            assignments[j] = 2;
                            if (j > v)
                                assignments[j - v] = 1;
                            else
                                assignments[j + v] = 1;
                        }
                    }
                }
            }
            
            return new Tuple<bool, long[]>(true, MakeResult(assignments,v));
        }

        private long[] MakeResult(long[] assignments,long size)
        {
            long[] result = new long[size];
            for (int i = 1; i <= size; i++)
            {
                if (assignments[i] == 1)
                    result[i - 1] = -1 * i;
                else
                    result[i - 1] = i;
            }
            return result;
        }

        

        private long FindSCC(long[] sCC, long nodeCount, long[][] edges)
        {
            List<long>[] adjList = new List<long>[nodeCount];
            List<long>[] revAdjList = new List<long>[nodeCount];


            for (int i = 0; i < nodeCount; i++)
            {
                adjList[i] = new List<long>();
                revAdjList[i] = new List<long>();
            }


            for (int i = 1; i < edges.Length; i++)
            {
                for (int j = 1; j < edges[i].Length; j++)
                {

                    if (edges[i][j] == 1)
                    {
                        adjList[i-1].Add(j-1);
                        revAdjList[j - 1].Add(i - 1);
                    }
                    //adjList[edges[i][0] - 1].Add(edges[i][1] - 1);
                    //revAdjList[edges[i][1] - 1].Add(edges[i][0] - 1);
                }
            }

            bool[] visited = new bool[nodeCount];
            Stack<long> order = new Stack<long>();

            for (int i = 0; i < adjList.Length; i++)
            {
                if (visited[i] == false)
                    explore(i, adjList, order, visited);
            }

            for (int i = 0; i < nodeCount; i++)
            {
                visited[i] = false;
            }
            long cc = 0;
            while (order.Count != 0)
            {
                long item = order.Pop();

                if (visited[item] == false)
                {
                    explore(item, revAdjList, visited,cc,sCC);
                    cc++;
                }

            }
            return cc;

        }



        private void explore(long i, List<long>[] adjList, bool[] visited, long cc,long[] SCC)
        {
            visited[i] = true;
            SCC[i + 1] = cc;

            foreach (var adj in adjList[i])
            {
                if (!visited[adj])
                {
                    explore(adj, adjList, visited,cc,SCC);
                }
            }
        }


        private void explore(long i, List<long>[] revAdjList, Stack<long> order, bool[] visited)
        {
            visited[i] = true;
            foreach (var adj in revAdjList[i])
            {
                if (!visited[adj])
                {
                    explore(adj, revAdjList, order, visited);
                }
            }
            order.Push(i);
        }


        private void ImplicationGraph(long v, long c, long[][] cnf, long[][] implicationGraph)
        {
            for (int i = 0; i < c; i++)
            {
                if (cnf[i].Length == 2)
                {
                    long index = cnf[i][1];
                    if (cnf[i][1] < 0)
                        index = -1 * cnf[i][1] + v;
                    implicationGraph[NegationOf(cnf[i][0], v)][index] = 1;
                    index = cnf[i][0];
                    if (cnf[i][0] < 0)
                        index = -1 * cnf[i][0] + v;
                    implicationGraph[NegationOf(cnf[i][1], v)][index] = 1;
                }
                else if (cnf[i].Length == 1)
                {
                    implicationGraph[NegationOf(cnf[i][0], v)][cnf[i][0]] = 1;
                }
            }
        }

        private long NegationOf(long variable,long size)
        {
                if(variable < 0)
                {
                    return (-1 * variable) ;
                }
                return variable + size;

        }
    }
}
