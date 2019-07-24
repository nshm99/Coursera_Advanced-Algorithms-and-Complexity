using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A8
{
    public class Q2Airlines : Processor
    {
        public Q2Airlines(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long[]>)Solve);

        public virtual long[] Solve(long flightCount, long crewCount, long[][] info)
        {
            long nodeCount = flightCount + crewCount + 2;
            long[,] graph = new long[nodeCount, nodeCount];
            long[,] resGraph = new long[nodeCount, nodeCount];
            for (int i = 1; i <= flightCount; i++)
            {
                graph[0, i] = 1;
                resGraph[0, i] = 1;
                for (long j = flightCount + 1; j <= flightCount + crewCount; j++)
                {
                    graph[i, j] = info[i - 1][j - flightCount - 1];
                    graph[j, nodeCount - 1] = 1;
                    resGraph[i, j] = info[i - 1][j - flightCount - 1];
                    resGraph[j, nodeCount - 1] = 1;
                }
            }
            long[] parent = new long[nodeCount];
            long[] result = new long[flightCount];
            for(int i =0;i<flightCount;i++)
            {
                result[i] = -1;
            }
            while (BFS(resGraph, 0, nodeCount - 1, parent))
            {
                UpdateResidual(resGraph, parent,flightCount,crewCount, 0, nodeCount - 1,result);
    

            }

            return result;
        }
        
        private void UpdateResidual(long[,] resGraphAdjList, long[] parent
            , long flightCount,long crewCount, long src, long des,long[] result)
        {
            for (long i = des; i != src; i = parent[i])
            {
                long curParent = parent[i];

                resGraphAdjList[curParent, i] = 0;
                resGraphAdjList[i, curParent] = 1;
                if(i>=flightCount && i<= flightCount+crewCount )
                {
                    if (parent[i] > 0)
                        {
                            result[parent[i] - 1] = i - flightCount;   
                        }
                }
                
                

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
                        if (i == des)
                            return true;
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
