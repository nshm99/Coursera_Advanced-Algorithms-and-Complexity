using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A8
{
    public class Q3Stocks : Processor
    {
        public Q3Stocks(string testDataName) : base(testDataName)
        { }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<long, long, long[][], long>)Solve);

        public virtual long Solve(long stockCount, long pointCount, long[][] matrix)
        {
            
            long nodeCount = (2*stockCount) + 2;
            long[,] resGraph = new long[nodeCount, nodeCount];
            for (int i = 1; i <= stockCount; i++)
            {
                resGraph[0, i] = 1;
                resGraph[i + stockCount, nodeCount - 1] = 1;
                for (long j = 1; j <= stockCount; j++)
                {
                    if (IsLessThan(i-1,j-1,matrix,pointCount,stockCount))
                    {
                        resGraph[i, j+stockCount] = 1;
                        
                    }
                 }
            }

            long[] parent = new long[nodeCount];
            long flow = 0;
            while (BFS(resGraph, 0, nodeCount - 1, parent))
            {
                UpdateResidual(resGraph, parent, stockCount, stockCount, 0, nodeCount - 1);
                flow++;
            }
  
            return stockCount-flow;
        }

       
        private bool IsLessThan(int v1, long v2, long[][] matrix, long pointCount, long stockCount)
        {

            if (v1 == v2)
                return false;

            for(int i = 0; i<pointCount;i++)
            { 
                if (matrix[v1][i] >= matrix[v2][i])
                    return false;
                
            }
            return true;
        }
        private void UpdateResidual(long[,] resGraphAdjList, long[] parent
            , long flightCount, long crewCount, long src, long des)
        {
            for (long i = des; i != src; i = parent[i])
            {
                long curParent = parent[i];
                resGraphAdjList[curParent, i] = 0;
                resGraphAdjList[i, curParent] = 1;
               
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
