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
               
               
                noOfCollisions = CalculateCollisions(queen);
                do {
                    noOfSwaps = 0;
                    for (int i = 0; i < n; ++i)
                    {
                        for (int j = i + 1; j < n; ++j)
                        {
                            if (IsAttacked(queen, i) || IsAttacked(queen, j))
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

        private static bool IsAttacked(int[] queen, int position)
        {
            return true;
        }

        private static int CalculateCollisions(int[] queen)
        {
            //uses IsAttacked
            return 0;
        }
    }
}
