using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IME
{
    public partial class Form1 : Form
    {
        public class ButtonData
        {
            public string Id { get; set; }
            public List<string> InnerValue0List { get; set; }
            public List<string> InnerValue1List { get; set; }
            public List<string> InnerValue2List { get; set; }
            public List<string> InnerValue3List { get; set; }
            public List<string> InnerValue4List { get; set; }
        }
        public class Root
        {
            public List<ButtonData> Buttons { get; set; }
        }
        public Form1()
        {
            InitializeComponent();

            string jsonFilePath = @"C:\Users\240925PM\Desktop\murota atari\C#\IME\IME\ParetteData.json";
            if (!File.Exists(jsonFilePath))
            {
                MessageBox.Show("JSONファイルが見つかりません: " + jsonFilePath);
                return;
            }
            string json = File.ReadAllText(jsonFilePath);
            Root buttonData = JsonSerializer.Deserialize<Root>(json);

            if (buttonData?.Buttons == null)
            {
                MessageBox.Show("JSONデータが無効です");
                return;
            }

            GenerateButtons();
        }

        private void GenerateButtons()
        {
            int buttonWidth = 55; // ボタンの幅
            int buttonHeight = 55; // ボタンの高さ
            int startX = 15; // 配置開始位置（X座標）
            int startY = 45; // 配置開始位置（Y座標）

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 5; x++) // 10個のボタンを生成
                {
                    Button btn = new Button();
                    btn.Width = buttonWidth;
                    btn.Height = buttonHeight;
                    btn.Name = "B" + (y + 1) + (x + 1); // ボタンのテキスト
                    btn.Text = btn.Name;
                    btn.Left = startX + (buttonWidth-2) * x; // 配置位置（X）
                    btn.Top = startY + (buttonHeight-2) * y; // 配置位置（Y）

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
