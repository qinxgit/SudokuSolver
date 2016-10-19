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
        private List<int[,]> answers = new List<int[,]>();
        private int[,] x;
        List<int>[] row = new List<int>[9];
        List<int>[] col = new List<int>[9];

        private void InitializeData()
        {
            int[] points = new int[9], temp;
            for(int i = 0; i < 9; i++)
            {
                points[i] = i + 1;
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
            SolveInternal(row, col, problem);

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
                    writer.Write("{0}\t", problem[i, j]);
                }
            }

            writer.WriteLine();
        }

        public IReadOnlyCollection<int[,]> GetAllAnswers()
        {
            return new ReadOnlyCollection<int[,]>(answers);
        }

        private int[,] SolveInternal(List<int>[] r, List<int>[] c, int[,] problem)
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j< 9; j++)
                {
                    if(problem[i, j] == 0)
                    {
                        var choices = r[i].Intersect(c[j]).ToList();

                        if(choices.Any())
                        {
                            var nr = new List<int>[9];
                            var nc = new List<int>[9];

                            for (int l = 0; l < 9; l++)
                            {
                                nr[l] = new List<int>();
                                nr[l].AddRange(r[l]);
                            }

                            for (int l = 0; l < 9; l++)
                            {
                                nc[l] = new List<int>();
                                nc[l].AddRange(c[l]);
                            }

                            var p = problem.Clone() as int[,];

                            foreach(int k in choices)
                            {
                                nr[i].Remove(k);
                                nc[j].Remove(k);
                                p[i, j] = k;

                                var sub = SolveInternal(nr, nc, p);

                                if(sub != null)
                                {
                                    answers.Add(sub);
                                }

                                nr[i].Add(k);
                                nc[j].Add(k);
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
