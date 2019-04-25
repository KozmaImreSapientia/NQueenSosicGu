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
        private static int[] mDiag;
        private static int[] sDiag;

        static void Main(string[] args)
        {
            //NQueenSusic2(18000);

            Console.WriteLine("N Queen problem solver:");
            Console.WriteLine("(Type ESC to exit)");

            for (;;){

                Console.Write("\n> Board size: ");
                string inStr = Console.ReadLine();
                int input;
                try
                {
                    bool A = inStr.Trim().ToLower().Equals("esc");
                    bool B = inStr.Trim().ToLower().Equals("exit");
                    bool C = inStr.Trim().ToLower().Equals("close");
                    bool D = inStr.Trim().ToLower().Equals("x");
                    if ( A || B || C || D)
                    {
                        break;
                    }
                    input = Math.Abs(Int32.Parse( inStr ));
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong input! Use natural numbers");
                    continue;
                }

                NQueenSusic2( input );

            }
            
            Console.WriteLine("\n\nPress any key to exit.");
            Console.ReadKey();

            /*
            int[] testQueen = new int[6] { 1,5,0,2,3,4 };
            int C = CalculateDiagonalCollisions(testQueen);


            int[] tests = new int[12] { 4, 6, 10, 25, 50, 100, 200, 300, 500, 1000, 10000, 18000 };

            foreach (int i in tests)
            {
                Console.Write(i + " : ");
                //NQueenSusic(i);
                Console.WriteLine("v2: ");
                NQueenSusic2(i);
            }

            Console.WriteLine("Done");
            while (true)
                Console.Read();
            */
        }

        /// <summary>
        /// Solves the N queen problem and writes to the console the solution and the time needed to reach it
        /// </summary>
        /// <param name="n">The problems dimension</param>
        private static void NQueenSusic(int n)
        {
            int noOfSwaps, noOfCollisions;
            int[] queen;
            Stopwatch sw;
            do {
                queen = InitializeWithRandomPermutation(n);
                //Console.WriteLine("Initial state:");
                //PrintQueens(queen);
                int attackScore;
                sw = new Stopwatch();
                sw.Start();
                noOfCollisions = CalculateCollisions(queen);
                //noOfCollisions = CalculateDiagonalCollisions(queen);
                do {
                    noOfSwaps = 0;
                    for (int i = 0; i < n; ++i)
                    {
                        for (int j = i + 1; j < n; ++j)
                        {
                            //Console.WriteLine("hgdszz");
                            if (IsAttacked(queen, i, out attackScore) || IsAttacked(queen, j, out attackScore))
                            {
                                int[] secondBoard = Swap(queen, i, j);
                                int noOfCol = CalculateCollisions(secondBoard);
                                //int noOfCol = CalculateDiagonalCollisions(secondBoard);
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
            Console.WriteLine("Solved state:");
            PrintQueens(queen);
        }

        private static void NQueenSusic2(int n)
        {
            int noOfSwaps, noOfCollisions;
            int[] queen;
            Stopwatch sw;
            do
            {
                queen = InitializeWithRandomPermutation(n);
                InitializeDiagonalArrays(queen);
                //Console.WriteLine("Initial state:");
                //PrintQueens(queen);
                sw = new Stopwatch();
                sw.Start();
                noOfCollisions = CalculateCollisions2(queen);
                do
                {
                    noOfSwaps = 0;
                    for (int i = 0; i < n; ++i)
                    {
                        for (int j = i + 1; j < n; ++j)
                        {
                            //Console.WriteLine("hgdszz");
                            if (IsAttacked2(queen, i) || IsAttacked2(queen, j))
                            {
                                Swap2(queen, i, j);
                                int noOfCol = CalculateCollisions2(queen);
                                if (noOfCol < noOfCollisions)
                                {
                                    noOfCollisions = noOfCol;
                                    noOfSwaps++;
                                    //RefreshDiagonalArrays(queen,i,j);
                                }
                                else
                                {//back to initial state
                                    Swap2(queen, j, i);
                                }
                            }
                        }
                    }
                } while (noOfSwaps != 0);
            } while (noOfCollisions != 0);
            sw.Stop();
            Console.WriteLine($"Time needed: {sw.Elapsed}");
            Console.WriteLine("Solved state:");
            PrintQueens(queen);
        }

        private static int[] InitializeWithRandomPermutation(int n)
        {
            int[] array = new int[n];

            for (int i = 0; i < n; i++)
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

        private static void Swap2(int[] queen, int i, int j)
        {
            //remove the collision for "source" queens
            mDiag[MDiagIndex(i, queen[i], queen.Length)]--;
            sDiag[i + queen[i]]--;
            mDiag[MDiagIndex(j, queen[j], queen.Length)]--;
            sDiag[j + queen[j]]--;

            int aux = queen[i];
            queen[i] = queen[j];
            queen[j] = aux;

            //refresh the collisions for "destination" queens
            mDiag[MDiagIndex(i, queen[i], queen.Length)]++;
            sDiag[i + queen[i]]++;
            mDiag[MDiagIndex(j, queen[j], queen.Length)]++;
            sDiag[j + queen[j]]++;
        }

        private static int MDiagIndex(int i, int j, int dimension)
        {
            return -(i - j) + dimension - 1;
        }

        private static bool IsAttacked2(int[] queen, int position)
        {
            if (mDiag[MDiagIndex(position, queen[position], queen.Length)] > 1)
            {
                return true;
            }
            if (sDiag[position + queen[position]] > 1)
            {
                return true;
            }
            return false;
        }

        private static void InitializeDiagonalArrays(int[] queen)
        {
            mDiag = new int[2 * queen.Length - 1];
            sDiag = new int[2 * queen.Length - 1];
            for (int position = 0; position < queen.Length; ++position)
            {
                mDiag[MDiagIndex(position, queen[position], queen.Length)]++;
                sDiag[position + queen[position]]++;
            }
        }

        private static int CalculateCollisions2(int[] queen)
        {
            int collisions = 0;
            for(int i = 0; i < mDiag.Length; ++i)
            {
                if (mDiag[i] > 1)
                {
                    collisions += mDiag[i] - 1;
                }
                if (sDiag[i] > 1)
                {
                    collisions += sDiag[i] - 1;
                }
            }
            return collisions;
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
            for (int i=position; i<queen.Length; ++i)
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

        private static int CalculateDiagonalCollisions(int[] queen)
        {
            // Calculate main and secondary diagonals:
            //int[] mDiag = AttackCounterOnMainDiagonal(queen);
            //int[] sDiag = AttackCounterOnSecondaryDiagonal(queen);

            // Calculate collision count:
            int counter = 0;

            foreach(int item in mDiag)
            {
                if( item > 1)
                {
                    counter++;
                }
            }

            foreach (int item in sDiag)
            {
                if (item > 1)
                {
                    counter++;
                }
            }

            return counter;
        }

        private static int[] AttackCounterOnMainDiagonal(int[] queen)
        {
            int n = queen.GetLength(0);
            bool isUsed = false;
            int[] mDiag = new int[ 2*n-1 ];
            int diagRowCount;

            /* 
             3    4 5 6 <=== mDiag array
             2 \   \ \ \
             1 \ \   \ \
             0 \ \ \   \ <-- upper part
               \ \ \ \ <-- lower part
            so: */

            // lower part
            diagRowCount = n - 1; // ! ! !

            for (int i = 0; i < n; ++i)
            {
                int x = i;
                for (int j = 0; j < n; ++j)
                {
                    int y = j;
                    while (x < n && y < n)
                    {
                        //Console.Write("[" + x + "," + y + "]");
                        if( queen[x] == y )
                        {
                            mDiag[diagRowCount]++;
                            //Console.Write("* ");
                        }  
                            
                        ++x;
                        ++y;
                        isUsed = false;
                    }
                    if (!isUsed)
                    {
                        isUsed = true;
                    }
                }
                //Console.WriteLine();
                --diagRowCount;
            }

            // upper part
            diagRowCount = n; // ! ! !

            isUsed = false;
            for (int k = 0; k < n; ++k)
            {
                int x = 0;
                for (int j = k + 1; j < n; ++j)
                {
                    int y = j;
                    while (x < n - k - 1 && y < n)
                    {
                        //Console.Write("[" + x + "," + y + "] ");
                        if (queen[x] == y)
                        {
                            mDiag[diagRowCount]++;
                            //Console.Write("* ");
                        }

                        ++x;
                        ++y;
                        isUsed = false;
                    }
                    if (!isUsed)
                    {
                        isUsed = true;
                    }
                }
                //Console.WriteLine();
                diagRowCount++;
            }

            return mDiag;
        }

        private static int[] AttackCounterOnSecondaryDiagonal(int[] queen)
        {
            int n = queen.GetLength(0);
            bool isUsed = false;
            int[] sDiag = new int[2 * n - 1];
            //int tokencount = 0;
            int diagRowCount;

            /*  mDiag array
              0 1 2 3       <-- upper part
             / / / /    4
             / / /    / 5
             / /    / / 6
             /    / / / 7       <-- lower part
            so: 
            */

            // upper part
            diagRowCount = 0;
            
            for (int i = 0; i < n; ++i)
            {
                int x = i;
                for (int j = 0; j < n; ++j)
                {
                    int y = j;
                    while (x >= 0 && y < n)
                    {
                        //Console.Write("[" + x + "," + y + "] ");
                        if (queen[x] == y)
                        {
                            sDiag[diagRowCount]++;
                        }

                        --x;
                        ++y;
                        isUsed = false;
                    }
                    if (!isUsed)
                    {
                        isUsed = true;
                    }
                }
                //Console.WriteLine();
                diagRowCount++;
            }

            // lower part
            for (int k = 1; k < n; ++k)
            {
                int x = n - 1;
                for (int j = k; j < n; ++j)
                {
                    int y = j;
                    while (x >= k - 1 && y < n)
                    {
                        //Console.Write("[" + x + "," + y + "] ");
                        if (queen[x] == y)
                        {
                            sDiag[diagRowCount]++;
                        }

                        --x;
                        ++y;
                        isUsed = false;
                    }
                    if (!isUsed)
                    {
                        isUsed = true;
                    }
                }
                //Console.WriteLine();
                diagRowCount++;
            }

            return sDiag;
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
