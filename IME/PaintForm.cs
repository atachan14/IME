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
        private Bitmap canvasBitmap; // 描画用のBitmap
        PictureBox pictureBox;
        private Point previousPoint; // 前回のマウス位置
        private bool isDrawing = false; // 描画中フラグ

        public PaintForm()
        {
            InitializeComponent();
            InitializeCanvas();
        }
        private void InitializeCanvas()
        {
            // PictureBoxを用意
            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            this.Controls.Add(pictureBox);

            //bitmap生成
            canvasBitmap = new Bitmap(pictureBox.Width, pictureBox.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(canvasBitmap))
            {
                g.Clear(Color.Transparent); // 背景を透明にする
            }

            pictureBox.Image = canvasBitmap;

            // マウスイベントの登録
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;
        }

        void GenerateButtons()
        {

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
                using (Graphics g = Graphics.FromImage(canvasBitmap))
                {
                    g.DrawLine(Pens.Black, previousPoint, e.Location);
                }
                previousPoint = e.Location;
                ((PictureBox)sender).Invalidate(); // 再描画
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
        }

        private void size_Click(object sender, EventArgs e)
        {

        }

        private void color_Click(object sender, EventArgs e)
        {

        }
    }
}