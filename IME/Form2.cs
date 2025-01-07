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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            GenerateButtons();
        }

        private void GenerateButtons()
        {
            int buttonWidth = 50; // ボタンの幅
            int buttonHeight = 30; // ボタンの高さ
            int startX = 10; // 配置開始位置（X座標）
            int startY = 10; // 配置開始位置（Y座標）

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 5; x++) // 10個のボタンを生成
                {
                    Button btn = new Button();
                    btn.Width = buttonWidth;
                    btn.Height = buttonHeight;
                    btn.Name = "B" + (y + 1) + (x + 1); // ボタンのテキスト
                    btn.Text = btn.Name;
                    btn.Left = startX + (buttonWidth) * x; // 配置位置（X）
                    btn.Top = startY + (buttonHeight) * y; // 配置位置（Y）

                    // ボタンクリック時のイベントを登録
                    btn.Click += Button_Click;

                    // フォームに追加
                    this.Controls.Add(btn);
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                MessageBox.Show($"You clicked: {clickedButton.Text}");
            }
        }
    }

}
