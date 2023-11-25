using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationSudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] problem = new int[9, 9];
            List<string> input = new List<string>();
            Console.WriteLine("Welcome to sodoku resolver(9 x 9)");
            enter:
            Console.WriteLine("Please enter the problem one row at a time. Use 0 on blank cells");


            for (int i = 0; i < 9; i++)
            {
            startloop:
                Console.Write($"\nLine {i + 1}: ");
                var userInput = Console.ReadLine();
                if (userInput.Trim().Length != 9)
                {
                    Console.WriteLine("\nThe input length is not 9. Please try again");
                    goto startloop;
                }
                userInput = userInput.Trim();
                if (userInput.Any(c => c < '0' || c > '9'))
                {
                    Console.WriteLine("\nThe input can only be numbers from 0 to 9. Please try again");
                    goto startloop;
                }
                input.Add(userInput);
            }

            string[] x = input.ToArray();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    problem[i, j] = x[i][j] - '0';
                }
            }

            var solver = new SudokuSolver.Solver();

            Console.WriteLine("\nYour sudoku problem is entered like this:");
            solver.PrintSolution(problem, Console.Out);
            confirm:
            Console.WriteLine("\nIs this correct? (y/n)");
            var k = Console.ReadKey().KeyChar.ToString().ToLower();
            if(k == "n" )
            {
                goto enter;
            }
            else if(k != "y")
            {
                goto confirm;
            }
            var result = solver.Solve(problem);
            if (result == null)
                Console.WriteLine("\nThere is no solution to this sudoku. It might be a wrong problem.");
            else
            {
                Console.WriteLine("\nThe solution is:");
                solver.PrintSolutionV2(result, problem, Console.Out);
            }
        }
    }
}
