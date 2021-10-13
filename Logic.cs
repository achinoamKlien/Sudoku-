using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Suduko
{
    class Logic
    {
        private const int N = 9;//אורך ורוחב המטריצה
        private Point[,] _logicMat;//המטריצה הלוגית 
        private int [,,]sudokuTables;//לוחות סודוקו אפשריים
        private int [,] actualTable;//המטריצה הממשית
        public static Random rnd = new Random();//הגרלת מספר לבחירת המטריצה האפשרית
        public Logic(Point[,] logicMat)
        {
            _logicMat = logicMat;
            sudokuTables = new int[3, 9, 9]{{{8,2,0,0,7,0,0,0,0},
                                        {0,4,0,0,3,0,1,0,7}, 
                                        {0,6,0,0,5,9,8,2,0}, 
                                        {0,1,4,9,0,5,0,0,6},
                                        {0,8,0,0,4,0,5,0,0},
                                        {0,9,0,0,0,3,7,0,4},
                                        {0,3,0,0,0,0,2,0,8}, 
                                        {0,5,0,3,1,0,0,0,9},  
                                        {4,0,9,5,8,0,0,3,0}} ,

                                        {{3,9,0,0,6,0,0,1,0},
                                        {0,5,1,3,9,0,0,4,0}, 
                                        {0,0,0,8,0,0,9,0,0}, 
                                        {0,3,0,0,0,0,8,5,0}, 
                                        {0,8,0,4,0,0,7,0,0},
                                        {0,7,0,2,8,5,0,0,4},
                                        {6,4,3,0,1,7,2,0,0}, 
                                        {0,2,0,0,0,0,1,7,0},  
                                        {0,0,0,0,2,8,0,3,0}} ,

                                        {{0,6,0,9,0,0,0,0,0},
                                        {0,1,0,0,0,0,5,9,0}, 
                                        {0,0,4,1,0,0,8,6,0}, 
                                        {0,8,0,5,0,4,0,2,0}, 
                                        {3,0,0,0,9,0,4,0,0},
                                        {4,0,0,2,6,0,0,8,0},
                                        {9,0,0,0,2,1,6,3,0}, 
                                        {0,0,8,0,0,9,0,4,0},  
                                        {0,0,0,3,0,6,9,0,5}} };
            NewGame();
           
        }

        public void NewGame()//הפעולה מחזירה לוח סודוקו חדש
        {
            int multiDimIndex = rnd.Next(3);
            ScrumbleTable(multiDimIndex);

            for (int i = 0; i < N * N; i++)
            {
                _logicMat[i / N, i % N].X = actualTable[i / N, i % N];
                _logicMat[i / N, i % N].Y = 0;
            }
        }


        public void ScrumbleTable(int index)//הפעולה מייצרת לוח סודוקו חדש
        {
            int[,] tempH = new int[3, N];
            int[,] tempV = new int[N, 3];
            actualTable = new int[N, N];
            for (int i = 0; i < N * N; i++)
            {
                actualTable[i / N, i % N] = sudokuTables[index, i / N, i % N];
            }
            int recNum = rnd.Next(3);
            int otherRecNum;
            while ((otherRecNum = rnd.Next(3)) == recNum)
            {
                otherRecNum = rnd.Next(3);
            }
            for (int i = 0; i < 3 * N; i++)
            {
                tempH[i / N, i % N] = actualTable[recNum*3 + i / N, i % N];
            }

            for (int i = 0; i < 3 * N; i++)
            {
                actualTable[recNum * 3 + i / N, i % N] = actualTable[otherRecNum * 3 + i / N, i % N];
            }
            for (int i = 0; i < 3 * N; i++)
            {
                actualTable[otherRecNum * 3 + i / N, i % N] = tempH[i / N, i % N];
            }
            recNum = rnd.Next(3);
            otherRecNum=0;
            while ((otherRecNum = rnd.Next(3)) == recNum)
            {
                otherRecNum = rnd.Next(3);
            }

            for (int i = 0; i < N * 3; i++)
            {
                tempV[i / 3, i % 3] = actualTable[i / 3, recNum * 3 + i % 3];
            }
            for (int i = 0; i < N * 3; i++)
            {
                actualTable[i / 3, recNum * 3 + i % 3] = actualTable[i / 3, otherRecNum * 3 + i % 3];
            }
            for (int i = 0; i < N * 3; i++)
            {
                actualTable[i / 3, otherRecNum * 3 + i % 3] = tempV[i / 3, i % 3];
            }

        }

        public bool IsInBox(int index, int digit)//הפעולה מחזירה אמת אם המספר שהוצב נמצא בקופסה הפנימית אחרת שקר
        {
            int boxLine = index / 9 / 3;
            int boxCol = index % 9 / 3;

            int row=index/N;
            int col = index % N;
            
            int i;
            bool isInSide = false;

            for (i = 0; i < 9 && !isInSide; i++)
            {
                int xx = boxCol * 3 + i % 3;
                int yy = boxLine * 3 + i / 3;
                isInSide= (row!=yy|| col!=xx)&&(_logicMat[yy, xx].X == digit || _logicMat[yy, xx].Y == digit) ? true : isInSide;
            }
            Console.WriteLine();
            return isInSide;
        }
        public bool CheckRow(int row,int col,int digit)//הפעולה מחזירה אמת אם המספר נמצא באותה שורה אחרת שקר
        {
            bool isInRow = false;
            for (int i = 0; i < N && !isInRow; i++)
            {
                isInRow = (i!=col&&(_logicMat[row, i].X == digit || _logicMat[row, i].Y == digit)) ? true : isInRow;
            }
            return isInRow;
        }
        public bool CheckCol(int row, int col, int digit)//הפעולה מחזירה אמת אם המספר נמצא באותו טור אחרת שקר
        {
            bool isInCol = false;
            for (int i = 0; i < N && !isInCol; i++)
            {
                isInCol = (i!=row&&(_logicMat[i, col].X == digit || _logicMat[i, col].Y == digit)) ? true : isInCol;
            }
            return isInCol;
        }
    }
}
