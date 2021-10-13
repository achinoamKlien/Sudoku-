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
    public partial class InstructionForm : Form
    {
        private Image backGround;
        private Rectangle rec;

        public InstructionForm()
        {
            InitializeComponent();
            try
            {
                backGround = Bitmap.FromFile(@"images\instru.png");
                rec = new Rectangle(61, 662, 113, 163);
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("not found" + e);

            }
        }

        private void FormMouseDown(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(e.X + " " + e.Y);
            if (rec.Contains(e.X, e.Y))
            {
                StartMenu sm = new StartMenu();
                this.Hide();
                sm.ShowDialog();
                this.Close();
            }
        }

        private void FormPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TranslateTransform(0, 0);
            g.DrawImage(backGround, 0, 0, 880, 880);
        }
    }
}
