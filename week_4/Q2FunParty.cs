using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A11
{
    public class Q2FunParty : Processor
    {
        public Q2FunParty(string testDataName) : base(testDataName)
        {
            //ExcludeTestCases(50);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[], long[][], long>)Solve);

        public virtual long Solve(long n, long[] funFactors, long[][] hierarchy)
        {
            if (n == 1)
                return funFactors[0];
            List<long>[] adjList = new List<long>[n];
            long[] fun = new long[n];
            for (int i = 0; i < n; i++)
            {
                adjList[i] = new List<long>();
                fun[i] = -1;
            }
            for (int i = 0; i < hierarchy.Length; i++)
            {
                adjList[hierarchy[i][0] - 1].Add(hierarchy[i][1] - 1);
                adjList[hierarchy[i][1] - 1].Add(hierarchy[i][0] - 1);
            }
            long funn = TotalFun(0, adjList, fun, funFactors, -1);
            return funn;
        }

        private long TotalFun(long v, List<long>[] adjList, long[] fun, long[] funFactors,long parent)
        {
            long m1 = funFactors[v];
            long m2 = 0;
            if (fun[v] == -1)
            {
                        
                if (adjList[v].Count() == 1)
                {
                    return funFactors[v];
                }
                else
                {
                    m1 = funFactors[v];
                    for (int i = 0; i < adjList[v].Count(); i++)
                    {
                        long child = adjList[v][i];
                        if(child != parent)
                        for (int j = 0; j < adjList[child].Count(); j++)//grand childes
                        {
                            if(adjList[child][j] != v)
                                m1 += TotalFun(adjList[child][j], adjList, fun, funFactors,child);
                        }
                    }
                    m2 = 0;
                    for (int i = 0; i < adjList[v].Count(); i++)
                    {
                        if(adjList[v][i] != parent)
                            m2 += TotalFun(adjList[v][i], adjList, fun, funFactors,v);
                    }
                }
            }
            return Math.Max(m1, m2);
        }
    }
}
