using Microsoft.VisualBasic.ApplicationServices;
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
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.AxHost;

namespace IME
{
    public partial class Form1 : Form
    {
        private OutPad outPadForm;
        private DebugForm df;
        private List<ButtonData>? BDL;
        private readonly List<Button> BL = [];

        private bool isPressing = false;
        private Point dragStartPoint;
        private DateTime pressStartTime;

        private int threshold = 10;
        private const int longPressThreshold = 1000;



        public Form1()
        {
            InitializeComponent();

            GenerateBL();
            MessageBox.Show("JsonToBDL befor");
            JsonToBDL();
            SetupButtonText();

            outPadForm = new OutPad();
            outPadForm.Show();

            df = new DebugForm();
            df.Show();

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
                    btn.MouseDown += Button_MouseDown;
                    btn.MouseUp += Button_MouseUp;
                    btn.MouseMove += Button_MouseMove;

                    // フォームに追加
                    this.Controls.Add(btn);
                    BL.Add(btn);
                }
            }
        }

        private void JsonToBDL()
        {
            string jsonFilePath = @"..\..\..\ParetteData.json";
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

        private void SetupButtonText()
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

        private void ButtonExe(string exeTags, ButtonData selectBd)
        {
            switch (exeTags)
            {
                case "none":
                    return;
                case "write0":
                    outPadForm.DisplayText(selectBd.Value0[0]);
                    return;
                case "write1":
                    outPadForm.DisplayText(selectBd.Value1[0]);
                    return;
                case "write2":
                    outPadForm.DisplayText(selectBd.Value2[0]);
                    return;
                case "write3":
                    outPadForm.DisplayText(selectBd.Value3[0]);
                    return;
                case "write4":
                    outPadForm.DisplayText(selectBd.Value4[0]);
                    return;
                case "temp0":
                    outPadForm.DisplayText("長押しできた？！");
                    return;

                default:
                    MessageBox.Show("不明なボタン処理");
                    return;
            }
        }

        private string CalcSwipePos(Point dragEndPoint)
        {
            int deltaX = dragEndPoint.X - dragStartPoint.X;
            int deltaY = dragEndPoint.Y - dragStartPoint.Y;

            // 差が閾値未満なら "0" を返す
            if (Math.Abs(deltaX) < threshold && Math.Abs(deltaY) < threshold)
                return "0";

            // X軸方向とY軸方向で長い方を判定
            return Math.Abs(deltaX) > Math.Abs(deltaY)
                ? (deltaX > 0 ? "2" : "4")  // X軸で右（2）か左（4）
                : (deltaY > 0 ? "3" : "1"); // Y軸で下（3）か上（1）
        }


        private ButtonData CatchSelectBd(object sender)
        {
            Button b = sender as Button;
            ButtonData selectBd = BDL.FirstOrDefault(bd => bd.Id == b.Name);
            if (selectBd == null)
            {
                MessageBox.Show("ButtonDataがnull");
            }
            return selectBd;
        }
        private string CatchExeTags(ButtonData selectBd, string type, string pos)
        {
            return selectBd.ExeTags[type][pos];
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {

            isPressing = true;
            dragStartPoint = e.Location;  // ドラッグ開始地点を記録
        }


        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            string pos = CalcSwipePos(e.Location);
            ButtonData selectBd = CatchSelectBd(sender);
            string exeTags = CatchExeTags(selectBd, "Short", pos);
            ButtonExe(exeTags, selectBd);
            isPressing = false;
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPressing)
            {
                df.Display("isPressing");
            }
            else
            {
                df.Display("notIsPressing");
            }
            if (isPressing)
            {

                // マウスが動いていないか、一定の移動距離がないかをチェック
                int deltaX = e.X - dragStartPoint.X;
                int deltaY = e.Y - dragStartPoint.Y;

                // 移動が少なければ長押し判定開始
                if (Math.Abs(deltaX) < 10 && Math.Abs(deltaY) < 10)
                {
                    if ((DateTime.Now - pressStartTime).TotalMilliseconds >= longPressThreshold)
                    {
                        string pos = CalcSwipePos(e.Location);
                        ButtonData selectBd = CatchSelectBd(sender);
                        string exeTags = CatchExeTags(selectBd, "Long", pos);

                        ButtonExe(exeTags, selectBd);

                        isPressing = false; // 長押し処理が完了したらフラグをリセット
                    }
                }
            }
        }
    }
}
