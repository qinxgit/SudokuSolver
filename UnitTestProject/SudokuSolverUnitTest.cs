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
            int[,] problem = new int[9, 9];

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

            for(int i = 0; i< 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    problem[i, j] = x[i][j] - '0';
                }
            }

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

        private static void VerifySolution(int[,] result)
        {
            
            Assert.IsNotNull(result);

            for (int i = 0; i < 9; i++)
            {
                List<int> y = new List<int>();

                for (int j = 0; j < 9; j++)
                {
                    Assert.IsTrue(result[i, j] >= 1 && result[i, j] <= 9);
                    y.Add(result[i, j]);
                }

                y.Sort();

                for (int j = 0; j < 9; j++)
                {
                    Assert.IsTrue(y[j] == j + 1);
                }
            }

            for (int i = 0; i < 9; i++)
            {
                List<int> y = new List<int>();

                for (int j = 0; j < 9; j++)
                {
                    Assert.IsTrue(result[j, i] >= 1 && result[j, i] <= 9);
                    y.Add(result[j, i]);
                }

                y.Sort();

                for (int j = 0; j < 9; j++)
                {
                    Assert.IsTrue(y[j] == j + 1);
                }
            }
        }
    }
}
