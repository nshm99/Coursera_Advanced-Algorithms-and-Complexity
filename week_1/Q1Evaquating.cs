using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A8
{
    public class Q1Evaquating : Processor
    {
        public Q1Evaquating(string testDataName) : base(testDataName)
        {
            //this.ExcludeTestCaseRangeInclusive(1, 1);
            //this.ExcludeTestCaseRangeInclusive(11, 100);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long>)Solve);

        public virtual long Solve(long nodeCount, long edgeCount, long[][] edges)
        {
            if (edgeCount == 0)
                return 0;
            long f = 0;
            long[,] graph = new long[nodeCount, nodeCount];
            long[,] resGraph = new long[nodeCount, nodeCount];
            
            long[] parent = new long[nodeCount];

            for (int i = 0; i < edges.Length; i++)
            {
                graph[edges[i][0] - 1, edges[i][1] - 1] += edges[i][2];
                resGraph[edges[i][0] - 1, edges[i][1] - 1] += edges[i][2];
            }

            while (BFS(resGraph, 0, nodeCount - 1, parent))
            {
                //if (BFS(resGraph, 0, nodeCount - 1,parent))
                {
                    long minX = long.MaxValue;
                    for (long i = nodeCount - 1; i != 0; i = parent[i])
                    {
                        long curParent = parent[i];
                        if (resGraph[curParent, i] < minX)
                            minX = resGraph[curParent, i];
                    }

                    UpdateResidual(resGraph, parent, minX, 0, nodeCount - 1);

                    f += minX;

                }
                //  else
                //    return f;

            }
            return f;
        }




        private void UpdateResidual(long[,] resGraphAdjList, long[] parent
            , long minX, long src, long des)
        {
            for (long i = des; i != src; i = parent[i])
            {
                long curParent = parent[i];
                
                    resGraphAdjList[curParent, i] -= minX;
                    resGraphAdjList[i, curParent] += minX;
                
            }
        }

        private bool BFS(long[,] resGraphAdjList, int src, long des, long[] parent)
        {
            Queue<long> nodes = new Queue<long>();
            bool[] visited = new bool[parent.Length];
            nodes.Enqueue(src);
            visited[src] = true;
            parent[src] = -1;
            while (nodes.Count != 0)
            {
                long item = nodes.Dequeue();
                for (int i = 0; i < parent.Length; i++)
                {
                    
                        if (visited[i] != true && resGraphAdjList[item, i] > 0)
                        {
                            nodes.Enqueue(i);
                            parent[i] = item;
                            visited[i] = true;
                            
                        }

                }
            }
            if (visited[(int)des] == true)
                return true;
            else
                return false;

        }
    }
}
