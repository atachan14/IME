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
    public partial class CryForm : Form
    {
        Form1 person;
        public CryForm(Form1 person, List<string> cryValue)
        {
            InitializeComponent();
            this.person = person;

            SetupCryLabels(cryValue);
        }
        private void SetupCryLabels(List<string> cryValues)
        {
            // 画面の幅と高さを取得
            int formWidth = this.ClientSize.Width;   // フォームの幅
            int formHeight = this.ClientSize.Height; // フォームの高さ

            // 中心点を取得
            int centerX = formWidth / 2;
            int centerY = formHeight / 2;

            // 分割角度計算
            double angleStep = 2 * Math.PI / cryValues.Count; // 1セクターの角度

            int maxRadius = Math.Min(formWidth, formHeight) / 2; // 最大半径

            for (int i = 0; i < cryValues.Count; i++)
            {
                // セクターの開始角度と終了角度
                double startAngle = i * angleStep;
                double endAngle = startAngle + angleStep;

                // 半径方向に埋めるループ
                for (int radius = 20; radius < maxRadius; radius += 15) // 30ずつ外に配置
                {
                    // セクター内の角度で埋めるループ
                    for (double angle = startAngle; angle < endAngle; angle += 0.2) // 細かく配置
                    {
                        // ラベルの位置計算
                        int x = (int)(centerX + radius * Math.Cos(angle));
                        int y = (int)(centerY + radius * Math.Sin(angle));

                        // ラベル生成
                        Label label = new Label();
                        label.Text = cryValues[i % cryValues.Count]; // サイクルで文字を取る
                        label.AutoSize = true;
                        label.Location = new Point(x, y);

                        // Formに追加
                        this.Controls.Add(label);
                    }
                }
            }
        }

    }
}
