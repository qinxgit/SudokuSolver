using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace SolveSudoku
{
    public class SudokuSolver
    {
        readonly private List<int[,]> answers = new List<int[,]>();
        private int[,] x;
        private readonly List<int>[,] squares = new List<int>[3, 3];
        readonly List<int>[] row = new List<int>[9];
        readonly List<int>[] col = new List<int>[9];

        private void InitializeData()
        {
            int[] points = new int[9], temp;
            for(int i = 0; i < 9; i++)
            {
                points[i] = i + 1;
                squares[i / 3, i % 3] = new List<int>();
            }

            for (int j = 0; j < 9; j++)
            {
                squares[j / 3, j % 3].AddRange(points);
            }


            for(int i = 0; i< 9; i++)
            {
                row[i] = new List<int>();
                temp = points.Clone() as int[];

                for (int j = 0; j< 9; j++)
                {
                    if(x[i,j] != 0)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            if(temp[l] == x[i, j])
                            {
                                temp[l] = 0;
                            }
                        }

                        squares[i / 3, j / 3].Remove(x[i, j]);
                    }
                }
                
                for(int l = 0; l < 9; l ++)
                {
                    if(temp[l] != 0)
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
                    if (x[j, i] != 0)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            if (temp[l] == x[j, i])
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
            x = problem;
            InitializeData();
            SolveInternal(row, col, squares, problem);

            if(answers.Count > 0)
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
            for(int i = 0; i < 9; i++)
            {
                writer.WriteLine("\n");

                for(int j = 0; j < 9; j++)
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

            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    r[i, j] = new List<int>();
                    r[i, j].AddRange(sqs[i, j]);
                }
            }

            return r;
        }

        private int[,] SolveInternal(List<int>[] r, List<int>[] c, List<int>[,] sqs, int[,] problem)
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j< 9; j++)
                {
                    if(problem[i, j] == 0)
                    {
                        var choices = r[i].Intersect(c[j]).Intersect(sqs[i / 3, j / 3]).ToList();

                        if(choices.Any())
                        {
                            var nr = CloneArrays(r);
                            var nc = CloneArrays(c);
                            var nsqs = CloneSquares(sqs);
                            var p = problem.Clone() as int[,];

                            foreach(int k in choices)
                            {
                                nr[i].Remove(k);
                                nc[j].Remove(k);
                                nsqs[i / 3, j / 3].Remove(k);
                                p[i, j] = k;

                                var sub = SolveInternal(nr, nc, nsqs, p);

                                if(sub != null)
                                {
                                    answers.Add(sub);
                                }

                                nr[i].Add(k);
                                nc[j].Add(k);
                                nsqs[i / 3, j / 3].Add(k);
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
