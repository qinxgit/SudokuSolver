using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationSudoku
{
    class Program
    {
        static void Main(string[] args)
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

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    problem[i, j] = x[i][j] - '0';
                }
            }

            var solver = new SolveSudoku.SudokuSolver();
            solver.PrintSolution(problem, Console.Out);
            var result = solver.Solve(problem);
            solver.PrintSolution(result, Console.Out);
        }
    }
}
