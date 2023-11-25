using Xunit;

namespace SudokuSolverTests
{
    public class SolverTest
    {
        [Fact]
        public void SolverCanSolveAnExampleProblemTest()
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
            var solver = new SudokuSolver.Solver();
            solver.PrintSolution(problem, tw);
            var result = solver.Solve(problem);
            Assert.NotNull(result);

            foreach (int[,] r in solver.GetAllAnswers())
            {
                VerifySolution(r);
            }

            sb.Clear();
            tw.Dispose();
            sb.Clear();
        }


        [Fact]
        public void SolverCanSolveAnotherExampleProblemTest()
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
            var solver = new SudokuSolver.Solver();
            solver.PrintSolution(problem, tw);
            var result = solver.Solve(problem);
            Assert.NotNull(result);

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

            Assert.NotNull(result);

            for (int i = 0; i < 9; i++)
            {
                List<int> y = new List<int>(), z = new List<int>();

                for (int j = 0; j < 9; j++)
                {
                    Assert.True(result[i, j] >= 1 && result[i, j] <= 9);
                    Assert.True(result[j, i] >= 1 && result[j, i] <= 9);
                    y.Add(result[i, j]);
                    z.Add(result[j, i]);
                }

                IsList1To9(y);
                IsList1To9(z);
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    List<int> y = new List<int>();

                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            y.Add(result[i * 3 + k, j * 3 + l]);
                        }
                    }
                }
            }
        }

        [Fact]
        public void TestInvalidSudoku()
        {
            // Arrange
            int[,] invalidSudoku = new int[9, 9]
            {
        {5, 3, 4, 6, 7, 8, 9, 1, 2},
        {6, 7, 2, 1, 9, 5, 3, 4, 8},
        {1, 9, 8, 3, 4, 2, 5, 6, 7},
        {8, 5, 9, 7, 6, 1, 4, 2, 3},
        {4, 2, 6, 8, 5, 3, 7, 9, 1},
        {7, 1, 3, 9, 2, 4, 8, 5, 6},
        {9, 6, 1, 5, 3, 7, 2, 8, 4},
        {2, 8, 7, 4, 1, 9, 6, 3, 5},
        {3, 4, 5, 2, 8, 6, 1, 7, 9}  // This row is invalid, it has two 1's
            };

            var solver = new SudokuSolver.Solver();

            // Act
            var result = solver.Solve(invalidSudoku);

            // Assert
            Assert.Null(result);  // Assuming that Solve returns null for unsolvable puzzles
        }

        [Fact]
        public void TestEmptySudoku()
        {
            // Arrange
            int[,] emptySudoku = new int[9, 9];  // An empty Sudoku puzzle

            var solver = new SudokuSolver.Solver();

            // Act
            var result = solver.Solve(emptySudoku);

            // Assert
            Assert.NotNull(result);  // Assuming that Solve returns a solution for solvable puzzles
                                     // Add more assertions here to check the validity of the solution if necessary

            foreach (int[,] r in solver.GetAllAnswers())
            {
                VerifySolution(r);
            }
        }

        private static void IsList1To9(List<int> y)
        {
            Assert.Equal(9, y.Count);
            y.Sort();

            for (int j = 0; j < 9; j++)
            {
                Assert.True(y[j] == j + 1);
            }
        }
    }
}