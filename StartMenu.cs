using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Suduko
{
    public partial class StartMenu : Form
    {
        private Image backGround;
        private const int N = 3;
        private Rectangle[] recArr;
        public StartMenu()
        {
            InitializeComponent();
            try
            {
                backGround = Bitmap.FromFile(@"images\menu.png");
                recArr = new Rectangle[N];
                recArr[0]= new Rectangle (279, 303, 262, 40);
                recArr[1] = new Rectangle(260, 386, 309, 40);
                recArr[2] = new Rectangle(368, 471, 101, 40);
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("not found" + e);

            }

        }

        private void FormMouseDown(object sender, MouseEventArgs e)
        {
           // MessageBox.Show(e.X + " " + e.Y);
            int i = 0;
            bool found = false;
            for (i = 0; i < N && !found; )
            {
                if (recArr[i].Contains(e.X, e.Y))
                    found = true;
                else
                    i++;
            }
            if (found)
            {
                switch (i)
                {
                    case 0:
                        GameForm gf = new GameForm();
                        this.Hide();
                        gf.ShowDialog();
                        this.Close();
                        //MessageBox.Show("start");
                        break;
                    case 1:
                        InstructionForm ins = new InstructionForm();
                        this.Hide();
                        ins.ShowDialog();
                        this.Close();
                        //MessageBox.Show("instructions");
                        break;
                    case 2:
                        if (MessageBox.Show("יציאה", "אתה בטוח שברצונך לצאת?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            Application.Exit();
                        //MessageBox.Show("exit");
                        break;
                }
            }
        }

        private void FormPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TranslateTransform(0,0);
            g.DrawImage(backGround, 0, 0, 800,910);
        }
    }
}
