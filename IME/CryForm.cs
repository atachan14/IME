using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace IME
{
    public partial class CryForm : Form
    {
        private Form1 person;
        private OutPad outPad;
        private List<string> cryValues;

        private bool isDragging = false;
        private List<(float startAngle, float endAngle)> areas;
        private List<(RectangleF bounds, string text)> textRegions = new();
        private HashSet<RectangleF> processedBounds = new();

        private bool brutalMode = true;

        public bool BrutalMode { get => brutalMode; set => brutalMode = value; }

        public CryForm(Form1 person, OutPad outPad, List<string> cryValues)
        {
            InitializeComponent();
            this.person = person;
            this.outPad = outPad;
            this.cryValues = cryValues;
            this.areas = new();

            this.MouseDown += CryForm_MouseDown;
            this.MouseUp += CryForm_MouseUp;
            this.MouseMove += CryForm_MouseMove;

            SetupCryField();
        }


        private void SetupCryField()
        {
            GenerateEreas();
            this.Invalidate();
        }
        void GenerateEreas()
        {
            int areaCount = cryValues.Count; // エリア数 = 文字列の数
            float angleStep = 360f / areaCount; // 各エリアの角度

            for (int i = 0; i < areaCount; i++)
            {
                float startAngle = i * angleStep;
                float endAngle = startAngle + angleStep;
                areas.Add((startAngle, endAngle));
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            GenerateGraphics(e.Graphics);
        }


        private void GenerateGraphics(Graphics g)
        {
            Random rand = new Random();

            int gridSize = 20; // グリッドのサイズ (縦横10マス)
            int cellSize = 20; // 1つのグリッドセルの基本サイズ
            int centerX = this.Width / 2;
            int centerY = this.Height / 2;

            for (int i = 0; i < areas.Count; i++) // 各エリアについて
            {
                (float startAngle, float endAngle) area = areas[i];
                string text = cryValues[i % cryValues.Count]; // ケーキカット方式で文字列を選択

                for (int row = 0; row < gridSize; row++)
                {
                    for (int col = 0; col < gridSize; col++)
                    {
                        float baseX = centerX + (col - gridSize / 2) * cellSize;
                        float baseY = centerY + (row - gridSize / 2) * cellSize;

                        float offsetX = (float)(rand.NextDouble() - 0.5) * cellSize * 0.7f;
                        float offsetY = (float)(rand.NextDouble() - 0.5) * cellSize * 0.7f;

                        float x = baseX + offsetX;
                        float y = baseY + offsetY;

                        // ランダムなフォントサイズ
                        int fontSize = rand.Next(9, 20);
                        System.Drawing.Font font = new System.Drawing.Font("Arial", fontSize);

                        // 描画する位置がエリア内かチェック（ケーキカット範囲）
                        if (BrutalMode || IsInsideArea(x, y, area))
                        {
                            SizeF textSize = g.MeasureString(text, font);
                            RectangleF bounds = new RectangleF(x, y, textSize.Width * 0.5f, textSize.Height * 0.5f);
                            textRegions.Add((bounds, text));

                            g.DrawString(text, font, Brushes.Black, x, y);
                        }
                    }
                }
            }
        }

        //エリア内角度判定用関数
        private bool IsInsideArea(float x, float y, (float startAngle, float endAngle) area)
        {
            // 中心点を基準にエリアの角度範囲と距離をチェック
            float dx = x - this.Width / 2;
            float dy = y - this.Height / 2;
            float angle = (float)(Math.Atan2(dy, dx) * 180 / Math.PI);
            if (angle < 0) angle += 360; // 負の角度を正に修正

            // 境界を少し緩くする
            return angle >= area.startAngle && angle <= area.endAngle; // 境界値を少し広げた
        }

        public void BrutalChange()
        {
            if (brutalMode)
            {
                brutalMode = false;
            }
            else
            {
                brutalMode |= true;
            }
            this.Invalidate();
        }

        private void CryForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                foreach (var (bounds, text) in textRegions)
                {
                    if (bounds.Contains(e.Location))
                    {
                        if (!processedBounds.Contains(bounds))
                        {
                            outPad.WriteCryValue(text);  // 文字入力処理
                            processedBounds.Add(bounds); // 処理済みとして記憶
                        }
                    }
                    else
                    {
                        // 出たらリセット
                        processedBounds.Remove(bounds);
                    }
                }
            }
        }
        private void CryForm_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
        }

        private void CryForm_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

    }

}

