using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IME
{
    public partial class Form1 : Form
    {
        private OutPad outPadForm;
        private List<ButtonData>? BDL;
        private readonly List<Button> BL = [];


        public Form1()
        {
            InitializeComponent();

            GenerateBL();
            JsonToBDL();
            SetupValue0();

            outPadForm = new OutPad();
            outPadForm.Show();

        }

        private void GenerateBL()
        {
            int buttonWidth = 50; // ボタンの幅
            int buttonHeight = 50; // ボタンの高さ
            int startX = 10; // 配置開始位置（X座標）
            int startY = 40; // 配置開始位置（Y座標）


            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 5; x++) // 10個のボタンを生成
                {
                    Button btn = new()
                    {
                        Width = buttonWidth,
                        Height = buttonHeight,
                        Name = "B" + (y + 1) + (x + 1), // ボタンのテキスト
                        Left = startX + buttonWidth * x, // 配置位置（X）
                        Top = startY + buttonHeight * y, // 配置位置（Y）
                    };

                    btn.Text = btn.Name;
                    btn.Click += Button_Click;

                    // フォームに追加
                    this.Controls.Add(btn);
                    BL.Add(btn);
                }
            }
        }

        private void JsonToBDL()
        {
            string jsonFilePath = @"D:\source\repos\C#\IME\IME\ParetteData.json";
            if (!File.Exists(jsonFilePath))
            {
                MessageBox.Show("JSONファイルが見つかりません: " + jsonFilePath);
                return;
            }
            string json = File.ReadAllText(jsonFilePath);
            BDL = JsonSerializer.Deserialize<List<ButtonData>>(json);

            if (BDL == null)
            {
                MessageBox.Show("JSONデータが無効です" + json);
                return;
            }
            else
            {
                MessageBox.Show("BDL生成完了 BDL.Count" + BDL.Count);
            }

        }

        private void SetupValue0()
        {
            var matches = from bd in BDL
                          from b in BL
                          where bd.Id == b.Name
                          select new { bd, b };
            foreach (var match in matches)
            {
                if (match.bd.Value0[0] == null)
                {
                    MessageBox.Show($"{match.bd.Id}==null");
                    continue;
                }
                match.b.Text = match.bd.Value0[0];
                //MessageBox.Show($"{match.b.Name}に{match.bd.Value0[0]}を登録");
            }
            //MessageBox.Show("SetupEnd");


        }


        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            outPadForm.DisplayText(clickedButton.Text);
        }


    }

}
