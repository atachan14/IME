using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IME
{
    public partial class PaintForm : Form
    {

        private PictureBox pictureBox;
        private Point previousPoint;
        private bool isDrawing = false;

        private Graphics bgGraphics;
        private Graphics drawGraphics;
        private Bitmap bgBitmap;
        private Bitmap drawBitmap;

        private Pen drawPen;
        private Button[] cbl = new Button[6];

        public PaintForm()
        {
            InitializeComponent();
            InitializeCanvas();
            drawPen = new Pen(Color.Black, 1f);
        }
        private void InitializeCanvas()
        {
            // PictureBoxを用意
            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            pictureBox.Paint += PictureBox_Paint;
            this.Controls.Add(pictureBox);

            //bitmap生成

            bgBitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            bgGraphics = Graphics.FromImage(bgBitmap);
            bgGraphics.Clear(Color.Transparent);

            drawBitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            drawGraphics = Graphics.FromImage(drawBitmap);
            drawGraphics.Clear(Color.Transparent);

            // マウスイベントの登録
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;

            SetupButtons();
        }



        void SetupButtons()
        {
            cbl = [c0, c1, c2, c3, c4, c5];
            string[] colorList = ["blue", "red", "green", "yellow", "white", "black"];
            for (int i = 0; i < cbl.Count(); i++)
            {
                cbl[i].Visible = false;
                cbl[i].BackColor = Color.FromName(colorList[i]);
            }

            trackBar.Visible = false;

        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {

            isDrawing = true;
            previousPoint = e.Location;
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {

                drawGraphics.DrawLine(drawPen, previousPoint, e.Location);
                previousPoint = e.Location;

                pictureBox.Invalidate(); // PictureBoxを再描画
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            // 背景用のBitmapを描画
            e.Graphics.DrawImage(bgBitmap, 0, 0);

            // 描画用のBitmapを描画
            e.Graphics.DrawImage(drawBitmap, 0, 0);
        }

        private void size_Click(object sender, EventArgs e)
        {
            trackBar.Visible = !trackBar.Visible;
        }

        private void color_Click(object sender, EventArgs e)
        {
            CblVisible();
        }

        private void bg_Click(object sender, EventArgs e)
        {
            bg.BackColor = bg.BackColor == color.BackColor ? Color.Transparent : color.BackColor;

            ChangeBG();
        }

        void CblVisible()
        {
            for (int i = 0; i < cbl.Count(); i++)
            {
                cbl[i].Visible = !cbl[i].Visible;
            }
        }
        private void Cb_Click(object sender, EventArgs e)
        {
            Button cb = sender as Button;
            ChangeColor(cb.BackColor);
            CblVisible();
        }
        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            TrackBar tb = sender as TrackBar;
            ChangeWidth(tb.Value);
        }


        void ChangeColor(Color c)
        {
            color.BackColor = c;
            ChangePen();
        }

        void ChangeWidth(int w)
        {
            width.Text = w.ToString();
            ChangePen();
        }
        void ChangePen()
        {
            float w = float.Parse(width.Text);
            Color c = color.BackColor;
            drawPen = new Pen(c, w);
        }



        void ChangeBG()
        {
            bgGraphics.Clear(bg.BackColor);
            pictureBox.Invalidate();
        }

        private void PaintForm_Load(object sender, EventArgs e)
        {

        }

        private void eraser_Click(object sender, EventArgs e)
        {

        }
    }
}