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
        //private DebugForm df;

        private List<ButtonData>? BDL;
        private readonly List<Button> BL = [];

        private bool isPressing = false;
        private Point dragStartPoint;
        private DateTime pressStartTime;

        private int threshold = 10;
        private int longPressThreshold = 10000;

        private string[] currentValues;
        private int currentValuesIndex;
        private string pending;





        private int debugCount = 0;
        public bool IsPressing { get => isPressing; set => isPressing = value; }
        public int DebugCount { get => debugCount; set => debugCount = value; }

        public Form1()
        {
            InitializeComponent();

            GenerateBL();
            JsonToBDL();
            SetupButtonText();

            outPadForm = new OutPad();
            outPadForm.Show();

            //df = new DebugForm(this);
            //df.Show();

        }

        private void GenerateBL()
        {
            int buttonWidth = 55; // ボタンの幅
            int buttonHeight = 55; // ボタンの高さ
            int startX = 1; // 配置開始位置（X座標）
            int startY = 40; // 配置開始位置（Y座標）


            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    Button btn = new()
                    {
                        Width = buttonWidth,
                        Height = buttonHeight,
                        Name = "B" + (y + 1) + (x + 1),
                        Left = startX + buttonWidth * x, // 配置位置（X）
                        Top = startY + buttonHeight * y, // 配置位置（Y）
                    };

                    btn.Text = btn.Name;
                    btn.MouseDown += Button_MouseDown;
                    btn.MouseUp += Button_MouseUp;
                    //btn.MouseMove += Button_MouseMove;

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

        private string[] ToFtag(string exeTags)
        {
            // 結果を格納する配列
            string[] ftag = new string[2];

            // 正規表現で文頭の特定文字列を抽出 (例: "current" or "trans")
            var match = Regex.Match(exeTags, @"^(current|trans)");

            if (match.Success)
            {
                // マッチした場合
                ftag[0] = match.Value;                     // "current" または "trans"
                ftag[1] = exeTags.Substring(match.Length);  // 残りの部分
            }
            else
            {
                // マッチしなかった場合
                ftag[0] = exeTags;  // 丸ごとftag[0]に入れる
                ftag[1] = "";     // ftag[1]は空文字
            }

            return ftag;
        }

        private void ExeCurrent(string ftag1, ButtonData selectBd)
        {
            currentValues = selectBd.Value0;
        }
        private void ButtonExe(string exeTags, ButtonData selectBd)
        {
            
            switch (exeTags)
            {
                case "none":
                    return;

                case "current0":
                    outPadForm.NextCurrent(selectBd.Value0[0]);
                    currentValues = selectBd.Value0;
                    currentValuesIndex = 0;
                    return;
                case "current1":
                    outPadForm.NextCurrent(selectBd.Value1[0]);
                    currentValues = selectBd.Value1;
                    currentValuesIndex = 0;
                    return;
                case "current2":
                    outPadForm.NextCurrent(selectBd.Value2[0]);
                    currentValues = selectBd.Value2;
                    currentValuesIndex = 0;
                    return;
                case "current3":
                    outPadForm.NextCurrent(selectBd.Value3[0]);
                    currentValues = selectBd.Value3;
                    currentValuesIndex = 0;
                    return;
                case "current4":
                    outPadForm.NextCurrent(selectBd.Value4[0]);
                    currentValues = selectBd.Value4;
                    currentValuesIndex = 0;
                    return;

                case "trans1":
                    if (currentValuesIndex == 1)
                    {
                        outPadForm.TransCurrent(currentValues[0]);
                        currentValuesIndex = 0;
                        return;
                    }
                    outPadForm.TransCurrent(currentValues[1]);
                    currentValuesIndex = 1;
                    return;
                case "trans2":
                    if (currentValuesIndex == 2)
                    {
                        outPadForm.TransCurrent(currentValues[0]);
                        currentValuesIndex = 0;
                        return;
                    }
                    outPadForm.TransCurrent(currentValues[2]);
                    currentValuesIndex = 2;
                    return;
                case "trans3":
                    if (currentValuesIndex == 3)
                    {
                        outPadForm.TransCurrent(currentValues[0]);
                        currentValuesIndex = 0;
                        return;
                    }
                    outPadForm.TransCurrent(currentValues[3]);
                    currentValuesIndex = 3;
                    return;
                case "trans4":
                    if (currentValuesIndex == 4)
                    {
                        outPadForm.TransCurrent(currentValues[0]);
                        currentValuesIndex = 0;
                        return;
                    }
                    outPadForm.TransCurrent(currentValues[4]);
                    currentValuesIndex = 4;
                    return;
                case "trans5":
                    if (currentValuesIndex == 5)
                    {
                        outPadForm.TransCurrent(currentValues[0]);
                        currentValuesIndex = 0;
                        return;
                    }
                    outPadForm.TransCurrent(currentValues[5]);
                    currentValuesIndex = 5;
                    return;



                case "Enter":
                    outPadForm.Confirmed();
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
                ? (deltaX > 0 ? "3" : "1")  // X軸で右（2）か左（4）
                : (deltaY > 0 ? "4" : "2"); // Y軸で下（3）か上（1）
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

            IsPressing = true;
            dragStartPoint = e.Location;
        }


        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            string pos = CalcSwipePos(e.Location);
            ButtonData selectBd = CatchSelectBd(sender);
            string exeTags = CatchExeTags(selectBd, "Short", pos);
            ButtonExe(exeTags, selectBd);
            IsPressing = false;
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {

            if (IsPressing)
            {
                debugCount++;
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

                        IsPressing = false; // 長押し処理が完了したらフラグをリセット
                    }
                }
            }
        }
    }
}
