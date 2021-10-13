using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace Suduko
{
    public partial class GameForm : Form
    {
        private Image backGround; //תמונת רקע
        private Image pencil;//תמונה של עיפרון האפשרויות
        private Rectangle backRec;//
        private SolidBrush brush;//
        private SolidBrush brush2;//
        private SolidBrush brush3;//
        private Pen pen;//
        private Pen penChosen;
        private int yOffset=4;
        private  int xOffset=160;
        private int rubricW=62;//
        private int rubricH=63;//הסט 
        private Rectangle digitRec;//
        private Rectangle eraseRec;
        private Rectangle initiateBoard;
        private Rectangle pencilRec;
        private Rectangle newGame;

        private bool digitIsChosen;//אם סומן מספר
        private bool erase;//מוחק את הספרה בלוח
        private bool pencilOn;//העיפרון שכותב את ההערות
        
        private const int N = 9;
        private Point[,] logicMat;
        private Font f= new System.Drawing.Font("Microsoft Sans Serif", 33F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));//גופן של המספרים המשתנים
        private Font f2 = new System.Drawing.Font("Microsoft Sans Serif", 41F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));//הגופן של המספרים הקבועים
        private Font f3 = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));//הגופן של המספרים בהערות
        private int digit=-1;

        private Logic logic;

        
        private List<int>[,] optionalMat;


        public GameForm()
        {
            InitializeComponent();
            try
            {
                backGround = Bitmap.FromFile(@"images\gameplay.png");
                backRec = new Rectangle(25, 760, 72, 99);
                digitRec = new Rectangle(196, 730, 404, 86);
                eraseRec = new Rectangle(605, 730, 72, 86);
                initiateBoard = new Rectangle(222, 49,128,68);
                newGame = new Rectangle(468,50, 200, 69);

                pencil = Bitmap.FromFile(@"images\pencil.png");
                pencilRec = new Rectangle(700, 740, 70, 70);
                pencilOn = false;

                optionalMat = new List<int>[N, N];
                initOptionalMat();

                brush = new SolidBrush(Color.Black);
                brush2 = new SolidBrush(Color.Black);
                brush3 = new SolidBrush(Color.Yellow);
               pen = new Pen(brush);
               penChosen = new Pen(brush3);
               penChosen.Width = 7;

                logicMat = new Point[N, N];
                logic = new Logic(logicMat);
               
                
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("not found" + e);

            }
        }

        private void initOptionalMat()
        {
            for (int i = 0; i < N * N; i++)
            {
                optionalMat[i / N, i % N] = new List<int>();
            }
        }

        private void FormMouseDown(object sender, MouseEventArgs e)
        {
            if (backRec.Contains(e.X, e.Y))
            {
                StartMenu sm = new StartMenu();
                this.Hide();
                sm.ShowDialog();
                this.Close();
            }
            else if (digitRec.Contains(e.X, e.Y))
            {
                digit = ((e.X - digitRec.X) / 45) + 1;
                digitIsChosen = true;
                Refresh();
            }
            else if (eraseRec.Contains(e.X, e.Y))
            {
                erase = true;
            }
            else if (initiateBoard.Contains(e.X, e.Y))
            {
                
                for (int i = 0; i < N * N; i++)
                {
                    optionalMat[i / N, i % N].Clear();
                }
                InitiateB();
                
            }
            else if (pencilRec.Contains(e.X, e.Y))
            {
                pencilOn = true;
            }
            else if (newGame.Contains(e.X, e.Y))
            {
                
                logic.NewGame();
                for (int i = 0; i < N * N; i++)
                {
                    optionalMat[i / N, i % N].Clear();
                }
                pictureBox1.Refresh();
            }
            
        }

        private void InitiateB()
        {
            for (int i = 0; i < N * N; i++)
            {
                logicMat[i / N, i % N].Y = 0;
            }
            pictureBox1.Refresh();

        }

        private void FormPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TranslateTransform(0, 0);
            g.DrawImage(backGround,0, 0, 880,880);
            g.DrawImage(pencil, 700, 740, 70, 70);
            if (digitIsChosen)
            {
                g.DrawRectangle(penChosen,((digit<7)? 9:21) + (digit - 1) * (digitRec.Height / 2) + digitRec.X, digitRec.Y, (digitRec.Height / 2)-5, (digitRec.Height-2) );//הצהוב
            }
            
           
        }

        private void PicMouseD(object sender, MouseEventArgs e)
        {
            
            int indexY = (e.Y - yOffset) / rubricH;
            int indexX = (e.X - xOffset) / rubricW;
            int indexYX= ((e.Y - yOffset) / rubricH) * 9 + (e.X - xOffset) / rubricW;
            if (digitIsChosen && pencilOn)
            {
                pencilOn = false;
     
                optionalMat[indexY, indexX].Add(digit);
                pictureBox1.Refresh();
            }
            else if (digitIsChosen)
            {
                if (logicMat[indexY, indexX].X == 0 && logicMat[indexY, indexX].Y == 0)
                {
                    while (optionalMat[indexY, indexX].Count > 0)
                    {
                        optionalMat[indexY, indexX].RemoveAt(optionalMat[indexY, indexX].Count - 1);
                    }
                    
                    logicMat[indexY, indexX].Y = digit;
                    
                    digitIsChosen = false;
                    pictureBox1.Refresh();
                    if (logic.IsInBox(indexYX, digit))
                    {
                        logicMat[indexY, indexX].Y = 0;
                        MessageBox.Show("already in that box!");
                    }

                    if (logic.CheckRow(indexY, indexX, digit))
                    {
                        logicMat[indexY, indexX].Y = 0;
                        MessageBox.Show("already in that row!");
                    }
                    if (logic.CheckCol(indexY,indexX, digit))
                    {
                        logicMat[indexY, indexX].Y = 0;
                        MessageBox.Show("already in that collum!");
                    }
                    pictureBox1.Refresh();
                }
            }
            else if (erase && logicMat[indexY, indexX].X == 0)
            {
                logicMat[indexY, indexX].Y = 0;
                pictureBox1.Refresh();
            }
        
            
           

        }

        private void PicPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int i = 0; i < N * N; i++)
            {
                if (logicMat[i / N, i % N].X != 0)
                    g.DrawString(logicMat[i / N, i % N].X.ToString(), f2, brush2, (i % N) * rubricW + xOffset+10, (i / N) * rubricH + yOffset+5);
                else if(logicMat[i / N, i % N].Y != 0)
                    g.DrawString(logicMat[i / N, i % N].Y.ToString(), f, brush, (i % N) * rubricW + xOffset + 10, (i / N) * rubricH + yOffset + 5);
            }
            for (int i = 0; i < N * N; i++)
            {
                int gap = 0;
               
                if (optionalMat[i / N, i % N].Count != 0)
                {
                    for (int j = 0; j < optionalMat[i / N, i % N].Count; j++,gap+=13)
                    {
                        g.DrawString(optionalMat[i / N, i % N].ElementAt(j).ToString(), f3, brush, (i % N) * rubricW + xOffset + gap, (i / N) * rubricH + yOffset + 5);
                    }
                    
                }
            }
            
            
        }
       
    }
}
