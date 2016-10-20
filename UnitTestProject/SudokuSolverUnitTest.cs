using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class SudokuSolverUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            int[,] problem = null;
            string[] x = new string[] {
                "060200109",
                "308007000",
                "050903007",
                "903070610",
                "000806000",
                "046090503",
                "500301090",
                "000700204",
                "602009050",
            };

            problem = InitializeProblemFromString(x);

            var sb = new System.Text.StringBuilder();
            var tw = new System.IO.StringWriter(sb);
            var solver = new SolveSudoku.SudokuSolver();
            solver.PrintSolution(problem, tw);
            var result = solver.Solve(problem);
            Assert.IsNotNull(result);

            foreach(int[,] r in solver.GetAllAnswers())
            {
                VerifySolution(r);
            }
                
            sb.Clear();
            tw.Dispose();
            sb.Clear();
        }


        [TestMethod]
        public void TestMethod2()
        {
            int[,] problem = null;
            string[] x = new string[] {
                "006020100",
                "000617000",
                "710000095",
                "100352009",
                "403000502",
                "900784001",
                "650000023",
                "000269000",
                "009070400",
            };

            problem = InitializeProblemFromString(x);

            var sb = new System.Text.StringBuilder();
            var tw = new System.IO.StringWriter(sb);
            var solver = new SolveSudoku.SudokuSolver();
            solver.PrintSolution(problem, tw);
            var result = solver.Solve(problem);
            Assert.IsNotNull(result);

            foreach (int[,] r in solver.GetAllAnswers())
            {
                VerifySolution(r);
            }

            sb.Clear();
            tw.Dispose();
            sb.Clear();
        }

        private static int[,] InitializeProblemFromString(string[] x)
        {
            int[,] problem = new int[9, 9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    problem[i, j] = x[i][j] - '0';
                }
            }

            return problem;
        }

        private static void VerifySolution(int[,] result)
        {
            
            Assert.IsNotNull(result);

            for (int i = 0; i < 9; i++)
            {
                List<int> y = new List<int>(), z = new List<int>();

                for (int j = 0; j < 9; j++)
                {
                    Assert.IsTrue(result[i, j] >= 1 && result[i, j] <= 9);
                    Assert.IsTrue(result[j, i] >= 1 && result[j, i] <= 9);
                    y.Add(result[i, j]);
                    z.Add(result[j, i]);
                }

                IsList1To9(y);
                IsList1To9(z);
            }

            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    List<int> y = new List<int>();

                    for(int k = 0; k < 3; k++)
                    {
                        for(int l = 0; l < 3; l++)
                        {
                            y.Add(result[i * 3 + k, j * 3 + l]);
                        }
                    }
                }
            }
        }

        private static void IsList1To9(List<int> y)
        {
            Assert.AreEqual(9, y.Count);
            y.Sort();

            for (int j = 0; j < 9; j++)
            {
                Assert.IsTrue(y[j] == j + 1);
            }
        }
    }
}
