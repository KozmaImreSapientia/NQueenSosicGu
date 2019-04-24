using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NQueenSusicGu
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        private static void NQueenSusic(int n)
        {
            int noOfSwaps, noOfCollisions;
            do {
                int[] queen = InitializeWithRandomPermutation(n);
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
        }

        private static int[] InitializeWithRandomPermutation(int n)
        {
            return new int[1];
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
            for (int i=0; i<queen.GetLength(0); ++i)
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
    }
}
