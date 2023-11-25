using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace SudokuSolver
{
    public class Solver
    {
        private const int MAX_SOLUTION_NUMBER = 3;
        readonly private List<int[,]> answers = new List<int[,]>();
        private int[,]? inputData;

        // Numbers don't exist in this sub square
        private readonly List<int>[,] squares = new List<int>[3, 3];

        // Numbers don't exist in this row
        readonly List<int>[] row = new List<int>[9];

        // Numbers don't exist in this col
        readonly List<int>[] col = new List<int>[9];

        private void InitializeData()
        {
            int[]? points = new int[9], temp;
            for (int i = 0; i < 9; i++)
            {
                points[i] = i + 1;
                squares[i / 3, i % 3] = new List<int>();
            }

            for (int j = 0; j < 9; j++)
            {
                squares[j / 3, j % 3].AddRange(points);
            }


            for (int i = 0; i < 9; i++)
            {
                row[i] = new List<int>();
                temp = points.Clone() as int[];

                for (int j = 0; j < 9; j++)
                {
                    if (inputData[i, j] != 0)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            if (temp[l] == inputData[i, j])
                            {
                                temp[l] = 0;
                            }
                        }

                        squares[i / 3, j / 3].Remove(inputData[i, j]);
                    }
                }

                for (int l = 0; l < 9; l++)
                {
                    if (temp[l] != 0)
                    {
                        row[i].Add(temp[l]);
                    }
                }
            }

            for (int i = 0; i < 9; i++)
            {
                col[i] = new List<int>();
                temp = points.Clone() as int[];

                for (int j = 0; j < 9; j++)
                {
                    if (inputData[j, i] != 0)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            if (temp[l] == inputData[j, i])
                            {
                                temp[l] = 0;
                            }
                        }
                    }
                }

                for (int l = 0; l < 9; l++)
                {
                    if (temp[l] != 0)
                    {
                        col[i].Add(temp[l]);
                    }
                }
            }
        }

        public int[,] Solve(int[,] problem)
        {
            inputData = problem;
            InitializeData();
            SolveInternal(row, col, squares, problem);

            if (answers.Count > 0)
            {
                return answers[0];
            }
            else
            {
                return null;
            }
        }

        public void PrintSolution(int[,] problem, TextWriter writer)
        {
            for (int i = 0; i < 9; i++)
            {
                writer.WriteLine("\n");

                for (int j = 0; j < 9; j++)
                {
                    if (problem[i, j] == 0)
                        writer.Write(" \t");
                    else
                        writer.Write("{0}\t", problem[i, j]);
                }
            }

            writer.WriteLine();
        }

        public void PrintSolutionV2(int[,] result, int[,] problem, TextWriter writer)
        {
            for (int i = 0; i < 9; i++)
            {
                writer.WriteLine("\n");

                for (int j = 0; j < 9; j++)
                {
                    if (problem[i, j] == 0)
                        writer.Write("\"{0}\"\t", result[i, j]);
                    else
                        writer.Write("{0}\t", result[i, j]);
                }
            }

            writer.WriteLine();
        }

        public IReadOnlyCollection<int[,]> GetAllAnswers()
        {
            return new ReadOnlyCollection<int[,]>(answers);
        }

        List<int>[] CloneArrays(List<int>[] arr)
        {
            var r = new List<int>[arr.Length];

            for (int l = 0; l < 9; l++)
            {
                r[l] = new List<int>();
                r[l].AddRange(arr[l]);
            }

            return r;
        }

        List<int>[,] CloneSquares(List<int>[,] sqs)
        {
            var r = new List<int>[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    r[i, j] = new List<int>();
                    r[i, j].AddRange(sqs[i, j]);
                }
            }

            return r;
        }

        private int[,] SolveInternal(List<int>[] r, List<int>[] c, List<int>[,] sqs, int[,] problem)
        {
            if (answers.Count >= MAX_SOLUTION_NUMBER)
                return null;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (problem[i, j] == 0)
                    {
                        var choices = r[i].Intersect(c[j]).Intersect(sqs[i / 3, j / 3]).ToList();

                        if (choices.Any())
                        {

                            // Create a new sub problem and maintain the state
                            var nr = CloneArrays(r);
                            var nc = CloneArrays(c);
                            var nsqs = CloneSquares(sqs);
                            var p = problem.Clone() as int[,];

                            foreach (int k in choices)
                            {
                                // Made a choice to use k on problem[i, j]

                                // Update the state of row, col, sqrs and problem
                                nr[i].Remove(k);
                                nc[j].Remove(k);
                                nsqs[i / 3, j / 3].Remove(k);
                                p[i, j] = k;

                                // Solve this new problem
                                var sub = SolveInternal(nr, nc, nsqs, p);
                                if (answers.Count >= MAX_SOLUTION_NUMBER)
                                    return null;

                                if (sub != null)
                                {
                                    answers.Add(sub);
                                    if (answers.Count >= MAX_SOLUTION_NUMBER)
                                        return null;
                                }

                                // Store the status
                                nr[i].Add(k);
                                nc[j].Add(k);
                                nsqs[i / 3, j / 3].Add(k);

                                // Make another choice
                            }

                            return null;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }

            return problem;
        }
    }
}
