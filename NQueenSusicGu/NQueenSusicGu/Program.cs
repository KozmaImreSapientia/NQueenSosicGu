using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NQueenSusicGu
{
    class Program
    {
        static void Main(string[] args)
        {
            NQueenSusic(4);
        }

        /// <summary>
        /// Solves the N queen problem and writes to the console the solution and the time needed to reach it
        /// </summary>
        /// <param name="n">The problems dimension</param>
        private static void NQueenSusic(int n)
        {
            int noOfSwaps, noOfCollisions;
            int[] queen;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            do {
                queen = InitializeWithRandomPermutation(n);
                Console.WriteLine("Initial state:");
                PrintQueens(queen);
                int attackScore;
               
                noOfCollisions = CalculateCollisions(queen);
                do {
                    noOfSwaps = 0;
                    for (int i = 0; i < n; ++i)
                    {
                        for (int j = i + 1; j < n; ++j)
                        {
                            if (IsAttacked(queen, i, out attackScore) || IsAttacked(queen, j, out attackScore))
                            {
                                int[] secondBoard = Swap(queen, i, j);
                                int noOfCol = CalculateCollisions(secondBoard);
                                if (noOfCol < noOfCollisions)
                                {
                                    queen = secondBoard;
                                    noOfCollisions = noOfCol;
                                    noOfSwaps++;
                                }
                            }
                        }
                    }
                } while (noOfSwaps != 0);
            } while (noOfCollisions != 0);
            sw.Stop();
            Console.WriteLine($"Time needed: {sw.Elapsed}");
            Console.WriteLine("Solved state");
            PrintQueens(queen);
        }

        private static int[] InitializeWithRandomPermutation(int n)
        {
            int[] array = new int[n];

            for(int i = 0; i < n; i++)
            {
                array[i] = i;
            }

            /*for(int i = 0; i < n; i++)
            {
                Console.Write(array[i]+" ");
            }*/

            FisherYatesShuffle(array);

            /*Console.WriteLine();
            for (int i = 0; i < n; i++)
            {
                Console.Write(array[i] + " ");
            }*/

            return array;
        }

        private static void FisherYatesShuffle(int[] array)
        {
            Random rand = new Random();
            int n = array.Count();
            for (int i = n - 1; i > 0; i--)
            {
                int j = rand.Next(0, i + 1);

                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        private static int[] Swap(int[] queen,int i, int j)
        {
            
            int size = queen.Length;

            int[] temp = new int [size];
            // quenn[0,1,i,3,j,5] swap i with j
             for(int q = 0; q < size; ++q)
            {
                
                if (q == i)
                {
                    temp[q] = queen[j];
                }
               else if  (q == j)
                {
                    temp[q] = queen[i];
                }
                else
                {
                    temp[q] = queen[q];
                }
                    
            }
           
         
            return temp;
        }

        private static bool IsAttacked(int[] queen, int position, out int attack_score)
        {
            //// no vertical attack possible because of 1D storing
            //
            //// horizontal attack:
            //int[] usedIndexes = new int[n];
            //for (int i = 0; i < n; ++i) usedIndexes[i] = 0;
            //for (int i = 0; i < n; ++i)
            //{
            //    if (usedIndexes[queen[i]] == 0 )
            //    {
            //        usedIndexes[ queen[i] ]++;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}

            attack_score = 0;
            bool isAttacked = false;

            // diagonal attack:
            int distance;
            bool A, B;
            for (int i=0; i<queen.Length; ++i)
            {
                if (i == position)
                {
                    continue;
                }

                distance = Math.Abs( position - i );
                A = queen[i] == queen[position] + distance;
                B = queen[i] == queen[position] - distance;

                if ( A || B)
                {
                    attack_score++;
                    isAttacked = true;
                }

            }
            return isAttacked;
        }

        private static int CalculateCollisions(int[] queen)
        {
            int n = queen.GetLength(0);
            int finalAttackScore = 0;
            int attackScore;

            //int[] mDiag = new int[n]; // only needed if we use 2D matrix
            //int[] sDiag = new int[n];
            
            for (int i = 0; i < n; ++i)
            {
                if( IsAttacked(queen, i, out attackScore) )
                {
                    finalAttackScore += attackScore;
                }
            }
            return finalAttackScore;
        }

        /// <summary>
        /// Prints the queens positions in a matrix shape for n less than 10 or in a list shape 
        /// </summary>
        /// <param name="board">The array containing the positions of the queens</param>
        private static void PrintQueens(int[] board)
        {
            if (board.Length <= 10)
            {
                for (int i = 0; i < board.Length; i++)
                {
                    for (int j = 0; j < board.Length; j++)
                    {
                        if (board[i] == j)
                        {
                            Console.Write($"{board[i]} ");
                        }
                        else
                        {
                            Console.Write("_ ");
                        }
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                for (int i = 0; i < board.Length; i++)
                {
                    Console.Write($"{board[i]}, ");
                }
                Console.WriteLine();
            }
        }
    }
}
