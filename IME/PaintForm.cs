using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

        private string drawMode = "pen";

        private string savePictureDirectory = @"..\..\..\picture";
        private string saveOptionPath;

        private bool isPress = false;
        private System.Windows.Forms.Timer pressTimer;
        private Stopwatch stopwatch = new Stopwatch();
        private Button targetCB;

        public PaintForm(int index)
        {
            InitializeComponent();
            InitializeCanvas();
            drawPen = new Pen(color.BackColor, int.Parse(width.Text));

            CreateOptionJsonPath(index);
            CreateTimer();
            CreateColorMap();
            SetupButtons();
            LoadSettings();
        }

        void CreateOptionJsonPath(int index)
        {
            string directory = @"..\..\..\optionJSON\pf";
            string back = index + ".json";
            saveOptionPath = System.IO.Path.Combine(directory, back);
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

            

        }
        void CreateTimer()
        {
            pressTimer = new();
            pressTimer.Interval = 100;  // 100msごとにTickイベント発生
            pressTimer.Tick += PressTimer_Tick;
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
            pictureBoxColorMap.Visible = false;

        }

        private void LoadSettings()
        {
            if (!File.Exists(saveOptionPath)) return; // ファイルが存在しない場合は何もしない

            // JSONを読み込み、デシリアライズ
            string json = File.ReadAllText(saveOptionPath, Encoding.UTF8);
            pfData settings = JsonConvert.DeserializeObject<pfData>(json);

            // 3. データを画面に反映
            for (int i = 0; i < cbl.Length; i++)
            {
                cbl[i].BackColor = ColorTranslator.FromHtml(settings.PaletteColors[i]);
            }

            color.BackColor = ColorTranslator.FromHtml(settings.CurrentColor);
            width.Text = settings.PenWidth;

            bgBitmap = Base64ToBitmap(settings.BackgroundImage);
            drawBitmap = Base64ToBitmap(settings.DrawingImage);

            // Graphicsも再作成
            bgGraphics = Graphics.FromImage(bgBitmap);
            drawGraphics = Graphics.FromImage(drawBitmap);

            pictureBox.Invalidate(); // 再描画
        }

        // Base64からBitmapに復元する関数
        private Bitmap Base64ToBitmap(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return new Bitmap(ms);
            }
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {

            isDrawing = true;
            previousPoint = e.Location;

            if (drawMode == "bucket") BucketPaint(e);
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                switch (drawMode)
                {
                    case "pen":
                        PenWrite(e);
                        return;

                    case "bucket":
                        return;

                    case "eraser":
                        EraserDelete(e);
                        return;

                    default:
                        MessageBox.Show("謎モード");
                        return;
                }
            }
        }

        void PenWrite(MouseEventArgs e)
        {
            drawGraphics.DrawLine(drawPen, previousPoint, e.Location);
            previousPoint = e.Location;

            pictureBox.Invalidate(); // PictureBoxを再描画
        }

        void BucketPaint(MouseEventArgs e)
        {
            Color targetColor = bgBitmap.GetPixel(e.X, e.Y);

            if (targetColor == drawPen.Color) return;

            Stack<Point> pixels = new Stack<Point>();
            pixels.Push(previousPoint);

            while (pixels.Count > 0)
            {
                Point p = pixels.Pop();

                // 範囲外チェック
                if (p.X < 0 || p.X >= drawBitmap.Width || p.Y < 0 || p.Y >= drawBitmap.Height)
                    continue;

                // 現在の色をチェック
                if (drawBitmap.GetPixel(p.X, p.Y) != targetColor)
                    continue;

                // 色を塗り替え
                drawBitmap.SetPixel(p.X, p.Y, drawPen.Color);

                // 隣接ピクセルを追加
                pixels.Push(new Point(p.X + 1, p.Y));
                pixels.Push(new Point(p.X - 1, p.Y));
                pixels.Push(new Point(p.X, p.Y + 1));
                pixels.Push(new Point(p.X, p.Y - 1));
            }
        }

        void EraserDelete(MouseEventArgs e)
        {
            int eraserRadius = (int)drawPen.Width;

            // Bitmapの範囲内で安全に消す処理
            for (int x = Math.Max(0, e.X - eraserRadius); x < Math.Min(drawBitmap.Width, e.X + eraserRadius); x++)
            {
                for (int y = Math.Max(0, e.Y - eraserRadius); y < Math.Min(drawBitmap.Height, e.Y + eraserRadius); y++)
                {
                    // 消しゴムの円形範囲内かどうかチェック
                    if (Math.Pow(x - e.X, 2) + Math.Pow(y - e.Y, 2) <= Math.Pow(eraserRadius, 2))
                    {
                        drawBitmap.SetPixel(x, y, Color.Transparent); // ピクセルを透明に
                    }
                }
            }

            pictureBox.Invalidate(); // 
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

        }
        private void cb_MouseDown(object sender, MouseEventArgs e)
        {
            isPress = true;
            pressTimer.Start();
            stopwatch.Restart();
            targetCB = sender as Button;
        }

        private void cb_MouseUp(object sender, MouseEventArgs e)
        {
            if (isPress)
            {

                isPress = false;
                pressTimer.Stop();
                stopwatch.Stop();

                Button cb = sender as Button;
                ChangeColor(cb.BackColor);
                CblVisible();
                pictureBoxColorMap.Visible = false;
            }
        }

        void PressTimer_Tick(object? sender, EventArgs e)
        {
            if (isPress)
            {
                if (stopwatch.ElapsedMilliseconds >= 1000)
                {

                    isPress = false;
                    pressTimer.Stop();
                    stopwatch.Stop();

                    pictureBoxColorMap.Visible = true;
                }
            }
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



        private void pen_Click(object sender, EventArgs e)
        {
            drawMode = "pen";
        }

        private void bucket_Click(object sender, EventArgs e)
        {
            drawMode = "bucket";
        }
        private void eraser_Click(object sender, EventArgs e)
        {
            drawMode = "eraser";
        }



        void SaveAsPngWithFixedPath()
        {
            // 保存先ディレクトリを指定（存在しない場合は作成する）
            // 好きなフォルダパスに変更OK！
            if (!System.IO.Directory.Exists(savePictureDirectory))
            {
                System.IO.Directory.CreateDirectory(savePictureDirectory); // ディレクトリがなければ作成
            }

            // ファイル名を動的に生成（例：20250116_152334.png）
            string fileName = $"Drawing_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            string fullPath = System.IO.Path.Combine(savePictureDirectory, fileName);

            // 保存用Bitmapを作成
            Bitmap saveBitmap = new Bitmap(bgBitmap.Width, bgBitmap.Height);
            using (Graphics g = Graphics.FromImage(saveBitmap))
            {
                g.DrawImage(bgBitmap, 0, 0);    // 背景を描画
                g.DrawImage(drawBitmap, 0, 0); // 描画部分を重ねる
            }

            // 保存
            saveBitmap.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);
            saveBitmap.Dispose();

            // 完了メッセージ
            MessageBox.Show($"画像を保存しました！\n{fullPath}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void save_Click(object sender, EventArgs e)
        {
            SaveAsPngWithFixedPath();
        }

        private void cansel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void CreateColorMap()
        {
            int width = pictureBoxColorMap.Width;
            int height = pictureBoxColorMap.Height;
            Bitmap colorMap = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(colorMap))
            {
                // 上から下にかけて色を変える横グラデーション
                for (int x = 0; x < width; x++)
                {
                    // 色をHSLベースで生成
                    float hue = (float)x / width * 360; // 色相を計算 (0～360度)
                    Color color = FromHSL(hue, 1.0f, 0.5f); // 彩度:1.0、明度:0.5
                    using (Pen pen = new Pen(color))
                    {
                        g.DrawLine(pen, x, 0, x, height); // 縦ラインを描画
                    }
                }
            }

            pictureBoxColorMap.Image = colorMap;
        }

        // HSLからColorを生成するヘルパー関数
        private Color FromHSL(float hue, float saturation, float lightness)
        {
            // System.Drawing.ColorではHSLを直接扱えないのでHSBに変換
            float chroma = (1 - Math.Abs(2 * lightness - 1)) * saturation;
            float x = chroma * (1 - Math.Abs((hue / 60) % 2 - 1));
            float m = lightness - chroma / 2;

            float r = 0, g = 0, b = 0;

            if (0 <= hue && hue < 60) { r = chroma; g = x; b = 0; }
            else if (60 <= hue && hue < 120) { r = x; g = chroma; b = 0; }
            else if (120 <= hue && hue < 180) { r = 0; g = chroma; b = x; }
            else if (180 <= hue && hue < 240) { r = 0; g = x; b = chroma; }
            else if (240 <= hue && hue < 300) { r = x; g = 0; b = chroma; }
            else if (300 <= hue && hue <= 360) { r = chroma; g = 0; b = x; }

            return Color.FromArgb(
                (int)((r + m) * 255),
                (int)((g + m) * 255),
                (int)((b + m) * 255)
            );
        }

        private void pictureBoxColorMap_MouseClick(object sender, MouseEventArgs e)
        {
            Bitmap colorMap = pictureBoxColorMap.Image as Bitmap;

            if (colorMap != null && e.X >= 0 && e.Y >= 0 && e.X < colorMap.Width && e.Y < colorMap.Height)
            {
                Color clickedColor = colorMap.GetPixel(e.X, e.Y);
                //MessageBox.Show($"選択した色: {clickedColor}", "カラーマップ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                targetCB.BackColor = clickedColor;
            }
        }

        private void NewCanvas()
        {
            bgGraphics.Clear(Color.Transparent);
            drawGraphics.Clear(Color.Transparent);
            pictureBox.Invalidate();
        }

        private void New_Click(object sender, EventArgs e)
        {
            NewCanvas();
        }

        private void SaveSettings()
        {
            // Step 1: データを準備
            var settings = new
            {
                paletteColors = cbl.Select(button => ColorTranslator.ToHtml(button.BackColor)).ToList(),
                currentColor = ColorTranslator.ToHtml(color.BackColor),
                penWidth = width.Text,
                backgroundImage = BitmapToBase64(bgBitmap),
                drawingImage = BitmapToBase64(drawBitmap)
            };

            // Step 2: JSONに変換してファイル保存
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(saveOptionPath, json, Encoding.UTF8);
        }

        private string BitmapToBase64(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // PNG形式で保存
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        private void saveOption_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}