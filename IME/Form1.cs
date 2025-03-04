﻿//using Newtonsoft.Json;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Windows.Forms.AxHost;

namespace IME
{
    public partial class Form1 : Form
    {
        string startParettePath = @"..\..\..\json\StartParette.json";
        string ABDataPath = @"..\..\..\json\ActionButtonData.json";
       
        private OutPad outPad;
        private CryForm? cryForm;
        //private DebugForm df;

        private List<ButtonData>? BDL;
        private List<ButtonData>? outerBDL;
        private List<ButtonData> innerBDL;
        private readonly List<Button> BL = [];

        private ButtonData selectBd;

        private bool onUI = false;
        private Label[] lbls = new Label[5];

        private bool isPressing = false;
        private System.Windows.Forms.Timer pressTimer;

        private Point dragStartPoint;
        private Point lastMouseLocation;
        private object lastSender;
        private Stopwatch stopwatch = new Stopwatch();

        private int threshold = 10;
        private int longPressThreshold = 800;

        private List<(string[] values, int index)> frontPT = new();
        private List<(string[] values, int index)> backPT = new();

        private string mode = "none";
        private List<(string[] values, int index)> cryPT = new();

        private bool exeMode;
        private List<(string[] values, int index)> targetPT = new();
        private List<(string[] values, int index)> newValuePT = new();
        private string newValue = "";
        private (ButtonData bd, string pos, int trans) lastBdInfo = new();

        private PaintForm[] pfList = new PaintForm[4];



        private int debugCount = 0;
        public bool IsPressing { get => isPressing; set => isPressing = value; }
        public int DebugCount { get => debugCount; set => debugCount = value; }

        public Form1(OutPad outPad)
        {
            InitializeComponent();

            //KanjiTo();

            this.outPad = outPad;
            GenerateBL();

            JsonToBDL();
            updateParette();
            SetupButtonText();

            this.MouseMove += Form_MouseMove;

            pressTimer = new();
            pressTimer.Interval = 100;  // 100msごとにTickイベント発生
            pressTimer.Tick += PressTimer_Tick;

            targetPT = frontPT;

            //df = new DebugForm(this);
            //df.Show();

        }

        
        //void KanjiTo()
        //{
        //    string inputFilePath = @"..\..\..\joyo2010.txt";
        //    // 出力するJSONファイルのパス
        //    string outputFilePath = @"..\..\..\kanjiDict.json";

        //    // ファイルが存在するか確認
        //    if (File.Exists(inputFilePath))
        //    {
        //        // テキストデータを行ごとに読み込む
        //        string[] lines = File.ReadAllLines(inputFilePath);

        //        // Dictionaryを作成
        //        Dictionary<string, List<string>> kanjiDict = new Dictionary<string, List<string>>();

        //        foreach (string line in lines)
        //        {
        //            // 空行をスキップ
        //            if (string.IsNullOrWhiteSpace(line)) continue;

        //            // タブ区切りで分割
        //            string[] parts = line.Split('\t');

        //            if (parts.Length >= 6)
        //            {
        //                string kanji = parts[0]; // 漢字
        //                string kana = parts[5];  // 読み（ヨミ）

        //                // 読みをキーに漢字を追加
        //                if (!kanjiDict.ContainsKey(kana))
        //                {
        //                    kanjiDict[kana] = new List<string>();
        //                }
        //                kanjiDict[kana].Add(kanji);
        //            }
        //        }

        //        // JSONに変換して保存
        //        string outputJson = JsonConvert.SerializeObject(kanjiDict, Formatting.Indented);
        //        File.WriteAllText(outputFilePath, outputJson);

        //        MessageBox.Show($"変換が完了しました！JSONファイル: {outputFilePath}");
        //    }
        //    else
        //    {
        //        MessageBox.Show("テキストファイルが見つかりませんでした。");
        //    }
        //}

        private void GenerateBL()
        {
            int buttonWidth = 55; // ボタンの幅
            int buttonHeight = 55; // ボタンの高さ
            int startX = 0; // 配置開始位置（X座標）
            int startY = 0; // 配置開始位置（Y座標）


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
                    btn.MouseMove += Form_MouseMove;

                    // フォームに追加
                    this.Controls.Add(btn);
                    BL.Add(btn);
                }
            }
        }

        private void JsonToBDL()
        {

            if (!File.Exists(startParettePath))
            {
                MessageBox.Show("JSONファイルが見つかりません: " + startParettePath);
                return;
            }
            outerBDL = new List<ButtonData>();
            string ABData = File.ReadAllText(ABDataPath);
            outerBDL = System.Text.Json.JsonSerializer.Deserialize<List<ButtonData>>(ABData);


            innerBDL = new List<ButtonData>();
            string startParette = File.ReadAllText(startParettePath);
            innerBDL = System.Text.Json.JsonSerializer.Deserialize<List<ButtonData>>(startParette);

        }

        void updateParette()
        {
            BDL = [.. outerBDL, .. innerBDL];
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
                if (match.bd.Text == null)
                {
                    match.b.Text = match.bd.Value0[0];
                }
                else
                {
                    match.b.Text = match.bd.Text;
                }
            }
        }

        private string[] ToFtag(string exeTags)
        {
            string[] ftag = new string[2];

            var match = Regex.Match(exeTags, @"^(current|trans|move|delete|parette|paint)");

            if (match.Success)
            {
                ftag[0] = match.Value;                     // "current" または "trans"
                ftag[1] = exeTags.Substring(match.Length);  // 残りの部分
            }
            else
            {
                ftag[0] = exeTags;  // 丸ごとftag[0]に入れる
                ftag[1] = "";     // ftag[1]は空文字
            }
            return ftag;
        }

        private void ButtonExe(string exeTags)
        {
            string[] ftag = ToFtag(exeTags);
            switch (ftag[0])
            {
                case "none":
                    return;

                case "current":
                    ExeCurrent(ftag[1]);
                    return;

                case "trans":
                    ExeTrans(ftag[1]);
                    return;

                case "move":
                    ExeMove(ftag[1]);
                    return;

                case "delete":
                    ExeDelete(ftag[1]);
                    return;

                case "indention":
                    ExeIndention(true);
                    return;

                case "insertLine":
                    ExeIndention(false);
                    return;

                case "cry":
                    ChangeCryMode();
                    return;

                case "return":
                    ExeEnter();
                    this.Dispose();
                    return;

                case "parette":
                    ExeParette(ftag[1]);
                    return;

                case "Enter":
                    ExeEnter();
                    return;

                case "edit":
                    ExeEdit();
                    return;

                case "paint":
                    ExePaint(ftag[1]);
                    return;

                case "Brutal":
                    ExeBrutal();
                    return;

                case "EditEnter":
                    MessageBox.Show("ExeEditEnterはなくなりました。");
                    return;

                case "UI":
                    onUI = !onUI;
                    return;

                case "kanji":
                    ExeKanji();
                    return;

                default:
                    MessageBox.Show("不明なボタン処理");
                    return;
            }
        }

        void ExeKanji()
        {
            outPad.transKanji();
            frontPT.Clear();
            backPT.Clear();
        }

        void ExeBrutal()
        {
            if (cryForm == null) return;

            cryForm.BrutalChange();
        }

        void ExePaint(string ftag1)
        {
            int index = int.Parse(ftag1);
            if (pfList[index] == null)
            {
                OpenPaintForm(index);
            }
            else
            {
                pfList[index].Visible = true;
            }

        }

        void OpenPaintForm(int index)
        {
            pfList[index] = new(index);
            pfList[index].TopLevel = false; // 子ウィンドウとして扱う
            pfList[index].FormBorderStyle = FormBorderStyle.None; // 枠を消す

            //paintForm.Dock = DockStyle.Fill; // 親フォーム内にぴったり埋め込む
            this.Controls.Add(pfList[index]); // 親フォームに追加
            pfList[index].BringToFront(); // 手前に持ってくる
            pfList[index].Show(); // 表示
        }


        void ExeParette(string ftag1)
        {
            ButtonData paretteBD = innerBDL.FirstOrDefault(b => b.Id == "Parette");
            if (paretteBD == null)
            {
                MessageBox.Show("paretteBDがnull");
                return;
            }

            string jsonPath = @"..\..\..\json\";
            jsonPath += paretteBD.Value0[int.Parse(ftag1)];
            innerBDL = new List<ButtonData>();
            string jsonText = File.ReadAllText(jsonPath);
            innerBDL = System.Text.Json.JsonSerializer.Deserialize<List<ButtonData>>(jsonText);

            updateParette();
            SetupButtonText();
        }



        private void ExeCurrent(string ftag1)
        {
            if (mode == "edit")
            {
                targetPT = newValuePT;
            }
            else
            {
                targetPT = frontPT;
            }
            switch (ftag1)
            {
                case "0":
                    targetPT.Add((selectBd.Value0, 0));
                    break;
                case "1":
                    targetPT.Add((selectBd.Value1, 0));
                    break;
                case "2":
                    targetPT.Add((selectBd.Value2, 0));
                    break;
                case "3":
                    targetPT.Add((selectBd.Value3, 0));
                    break;
                case "4":
                    targetPT.Add((selectBd.Value4, 0));
                    break;
                default:
                    MessageBox.Show("ftag1 error");
                    return;
            }
            if (mode != "edit") lastBdInfo = (selectBd, ftag1, 0);
            SendPendingAndCurrent();
        }

        private void ExeTrans(string ftag1)
        {
            if (targetPT == null) MessageBox.Show("taPTNull");
            if (lastBdInfo.bd == null) MessageBox.Show("lastBdNull");

            if (targetPT.Count == 0 || lastBdInfo.bd.Id == "B42")
            {
                Debug.WriteLine("targetPT||bd nullみたいな");
                ExeCurrent(ftag1);
                return;
            }

            int index = int.Parse(ftag1);
            int current = targetPT.Count - 1;
            if (targetPT.Last().index == index)
            {
                Debug.WriteLine("index==index");

                index = 0;
            }
            Debug.WriteLine($"代入前　targetPT.Count:{targetPT.Count}   targetPT[current]:{targetPT[current]}");

            targetPT[current] = (targetPT[current].values, index);
            Debug.WriteLine($"代入後　targetPT.Count:{targetPT.Count}   targetPT[current]:{targetPT[current]}");

            if (mode != "edit") lastBdInfo.trans = index;
            SendPendingAndCurrent();
            return;
        }

        void ExeDelete(string ftag1)
        {
            var match = Regex.Match(ftag1, @"^(Long|Short)");
            string type = match.Value;
            string direction = ftag1.Substring(match.Length);

            switch ((type, targetPT.Count != 0 || backPT.Count != 0))
            {
                case ("Short", true):
                    PendingShortDelete(direction);
                    return;
                case ("Long", true):
                    PendingLongDelete(direction);
                    return;
                case ("Short", false):
                    outPad.ConfShortDelete(direction);
                    return;
                case ("Long", false):
                    outPad.ConfLongDelete(direction);
                    return;
            }
        }
        void PendingShortDelete(string direction)
        {
            switch (direction)
            {
                case "1":
                    if (targetPT.Count == 0) return;
                    targetPT.RemoveAt(targetPT.Count - 1);
                    SendPendingAndCurrent();
                    return;
                case "3":
                    if (backPT.Count == 0) return;
                    backPT.RemoveAt(0);
                    SendPendingAndCurrent();
                    return;

                default:
                    MessageBox.Show("未実装PendingShortDelete direction:" + direction);
                    return;
            }
        }

        void PendingLongDelete(string direction)
        {
            switch (direction)
            {
                case "2":
                    if (frontPT.Count == 0) return;
                    frontPT.Clear();
                    SendPendingAndCurrent();
                    return;
                case "4":
                    if (backPT.Count == 0) return;
                    backPT.Clear();
                    SendPendingAndCurrent();
                    return;

                default:
                    MessageBox.Show("未実装PendingShortDelete direction:" + direction);
                    return;
            }
        }

        void ExeMove(string ftag1)
        {
            if (frontPT.Count > 0 || backPT.Count > 0)
            {
                PendingMove(ftag1);
            }
            else
            {
                outPad.ConfMove(ftag1);
            }
        }

        List<List<(string[] value, int index)>> GetFrontLines()
        {
            MessageBox.Show("getFrontLines start");
            List<List<(string[] value, int index)>> frontLines = new();
            foreach ((string[] value, int index) ft in frontPT)
            {
                if (ft.value[0] == "\n")
                {
                    frontLines.Add(new List<(string[] value, int index)>());
                }
                frontLines[frontLines.Count - 1].Add(ft);
            }
            MessageBox.Show("getFrontLines end");
            return frontLines;
        }
        void PendingMove(string ftag1)
        {
            switch (ftag1)
            {
                case "1":
                    if (frontPT.Count == 0) return;
                    backPT.Insert(0, frontPT.Last());
                    frontPT.RemoveAt(frontPT.Count - 1);
                    SendPendingAndCurrent();
                    return;

                case "2":
                    if (frontPT.Count == 0) return;

                    List<List<(string[] value, int index)>> frontLines = GetFrontLines();
                    int maxX = frontLines[frontLines.Count - 1].Count;

                    MessageBox.Show($"frontLines.Count:{frontLines.Count}");
                    if (frontLines.Count == 1)
                    {
                        MessageBox.Show("frontLines.count:1 ");
                        backPT.Insert(0, frontPT[0]);
                        MessageBox.Show("frontPT.count:1 ");
                        frontPT.Clear();
                        SendPendingAndCurrent();
                        return;
                    }

                    MessageBox.Show("frontPT.count > 1 ");


                    for (int i = 0; i < frontLines[frontLines.Count].Count; i++)
                    {
                        backPT.Insert(i, frontLines[frontLines.Count - 1][i]);
                    }
                    frontLines.RemoveAt(frontLines.Count - 1);

                    MessageBox.Show("");


                    if (maxX < frontLines[frontLines.Count - 1].Count)
                    {
                        for (int i = 0; i < maxX; i++)
                        {
                            backPT.Insert(i, frontLines[frontLines.Count - 1][i]);
                            frontLines[frontLines.Count - 1].RemoveAt(i);
                        }
                    }

                    frontPT.Clear();
                    for (int i = 0; i < frontLines.Count; i++)
                    {
                        for (int j = 0; j < frontLines[i].Count; j++)
                        {
                            frontPT.Add(frontLines[i][j]);
                        }
                    }
                    SendPendingAndCurrent();
                    return;

                case "3":
                    if (backPT.Count == 0) return;
                    frontPT.Add(backPT[0]);
                    backPT.RemoveAt(0);
                    SendPendingAndCurrent();
                    return;

                default:
                    MessageBox.Show("未実装:" + ftag1);
                    return;
            }
        }


        void ExeIndention(bool move)
        {
            if (!move) frontPT.Add((["\n"], 0));
            if (move) backPT.Insert(0, (["\n"], 0));
            SendPendingAndCurrent();
        }

        private void SendPendingAndCurrent()
        {
            string current = "";
            string fpen = "";
            string bpen = "";

            if (frontPT.Count > 0) current = frontPT.Last().values[frontPT.Last().index];
            if (frontPT.Count > 0) fpen = PendingTapleToString(frontPT, 1);
            if (backPT.Count > 0) bpen = PendingTapleToString(backPT, 0);

            if (mode == "edit") current = NewValuePTtoNewValue();


            outPad.UpdatePendingAndCurrent(fpen, current, bpen);
        }



        string PendingTapleToString(List<(string[] values, int index)> PT, int range)
        {
            string value = "";
            for (int i = 0; i < PT.Count - range; i++)
            {
                value += PT[i].values[PT[i].index];
            }
            return value;
        }
        string NewValuePTtoNewValue()
        {
            newValue = "";
            foreach ((string[] value, int index) taple in newValuePT)
            {
                newValue += taple.value[taple.index];
            }
            Debug.WriteLine("NewValuePTtoNewValue:" + newValue);
            return newValue;
        }
        void ChangeCryMode()
        {
            (mode, outPad.OutMode) = mode switch
            {
                "none" => ("set", "cry"),
                "set" or "write" => ("none", "none"),
                _ => (mode, outPad.OutMode)
            };

            if (mode == "none") CloseCryForm();

            SendPendingAndCurrent();
        }

        void Confirmed()
        {
            outPad.Confirmed();
            frontPT.Clear();
            backPT.Clear();
        }
        void ExeEnter()
        {
            switch (mode)
            {
                case "none":
                    Confirmed();
                    return;
                case "set":
                    mode = "write";
                    OpenCryForm();

                    frontPT.Clear();
                    backPT.Clear();
                    SendPendingAndCurrent();
                    return;
                case "write":
                    mode = "none";
                    CloseCryForm();
                    outPad.OutMode = "none";
                    outPad.Confirmed();
                    return;
                case "edit":
                    string newValue = NewValuePTtoNewValue();
                    EditButtonData(newValue);

                    mode = "none";
                    targetPT = frontPT;
                    outPad.OutMode = "none";
                    outPad.Confirmed();
                    SaveBDLtoJson();
                    newValuePT.Clear();
                    return;
            }
        }

        void EditButtonData(string newValue)
        {
            switch (lastBdInfo.pos)
            {
                case "0":
                    lastBdInfo.bd.Value0[lastBdInfo.trans] = newValue;
                    break;
                case "1":
                    lastBdInfo.bd.Value1[lastBdInfo.trans] = newValue;
                    break;
                case "2":
                    lastBdInfo.bd.Value2[lastBdInfo.trans] = newValue;
                    break;
                case "3":
                    lastBdInfo.bd.Value3[lastBdInfo.trans] = newValue;
                    break;
                case "4":
                    lastBdInfo.bd.Value4[lastBdInfo.trans] = newValue;
                    MessageBox.Show($"{lastBdInfo.bd.Value4[lastBdInfo.trans]}に{newValuePT}を登録");
                    break;
                default:
                    MessageBox.Show("ExeEnter.Edit error");
                    return;
            }
        }



        List<string> CatchCryValue()
        {
            List<string> cryValue = new List<string>();
            foreach ((string[] value, int index) taple in frontPT)
            {
                cryValue.Add(taple.value[taple.index]);
            }

            foreach ((string[] value, int index) taple in backPT)
            {
                cryValue.Add(taple.value[taple.index]);
            }

            return cryValue;
        }
        void OpenCryForm()
        {
            List<string> cryValue = CatchCryValue();

            cryForm = new CryForm(this, outPad, cryValue);
            cryForm.TopLevel = false;  // 子ウィンドウとして扱う
            cryForm.FormBorderStyle = FormBorderStyle.None;  // 枠を消す

            this.Controls.Add(cryForm);
            cryForm.BringToFront();
            cryForm.Show();
        }
        void CloseCryForm()
        {
            if (cryForm != null) { cryForm.Close(); }
        }
        void ExeEdit()
        {
            Debug.WriteLine("1" + frontPT.Count);

            frontPT.RemoveAt(frontPT.Count - 1);
            Debug.WriteLine("2" + frontPT.Count);
            SendPendingAndCurrent();
            Confirmed();

            mode = "edit";
            newValuePT.Clear();
            newValue = "";
            targetPT = newValuePT;
            outPad.ChangeOutMode("edit");


            SendPendingAndCurrent();
        }
        void SaveBDLtoJson()
        {
            string updatedJson = System.Text.Json.JsonSerializer.Serialize(innerBDL, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(startParettePath, updatedJson);

            Debug.Write("JSONファイルを保存しました！");
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
            pressTimer.Start();
            isPressing = true;
            lastSender = sender;

            dragStartPoint = e.Location;
            generateUI(sender, e);
        }
        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsPressing)
            {
                pressTimer.Stop();
                isPressing = false;
                string pos = CalcSwipePos(e.Location);
                selectBd = CatchSelectBd(sender);
                string exeTags = CatchExeTags(selectBd, "Short", pos);
                ButtonExe(exeTags);
                LblsDelete();
            }
        }
        public void PressTimer_Tick(object? sender, EventArgs e)
        {
            if (isPressing)
            {
                if (stopwatch.ElapsedMilliseconds >= longPressThreshold)
                {
                    pressTimer.Stop();
                    IsPressing = false;

                    string pos = CalcSwipePos(lastMouseLocation);
                    selectBd = CatchSelectBd(lastSender);
                    string exeTags = CatchExeTags(selectBd, "Long", pos);
                    ButtonExe(exeTags);
                    LblsDelete();
                }
            }
        }
        void Form_MouseMove(object sender, MouseEventArgs e)
        {
            stopwatch.Restart();
            lastMouseLocation = e.Location;
        }

        void LblsDelete()
        {

            foreach (Label lbl in lbls)
            {
                if (lbl == null) return;
                lbl.Dispose();
            }
        }
        void generateUI(object sender, MouseEventArgs e)
        {
            if (!onUI) return;

            selectBd = CatchSelectBd(sender);
            if (selectBd.ExeTags["Short"]["4"] != "current4") return;

            for (int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0:
                        lbls[i] = new Label()
                        {
                            Text = selectBd.Value0[0].Length < 3 ? selectBd.Value0[0] : selectBd.Value0[0].Substring(3) + "..",
                        };
                        break;
                    case 1:
                        lbls[i] = new()
                        {
                            Text = selectBd.Value1[0].Length < 3 ? selectBd.Value1[0] : selectBd.Value1[0].Substring(3) + "..",
                        };
                        break;
                    case 2:
                        lbls[i] = new()
                        {
                            Text = selectBd.Value2[0].Length < 3 ? selectBd.Value2[0] : selectBd.Value2[0].Substring(3) + "..",
                        };
                        break;
                    case 3:
                        lbls[i] = new()
                        {
                            Text = selectBd.Value3[0].Length < 3 ? selectBd.Value3[0] : selectBd.Value3[0].Substring(3) + "..",
                        };
                        break;
                    case 4:
                        lbls[i] = new()
                        {
                            Text = selectBd.Value4[0].Length < 3 ? selectBd.Value4[0] : selectBd.Value4[0].Substring(3) + "..",
                        };
                        break;
                    default:
                        break;
                }


                lbls[i].Left = (ClientSize.Width / 2) - lbls[i].PreferredWidth; // 配置位置（X）
                lbls[i].Top = (ClientSize.Height / 2) - 20; // 配置位置（Y）
                this.Controls.Add(lbls[i]);
                lbls[i].BackColor = Color.FromArgb(128, 255, 192, 203);
                lbls[i].Font = new Font(lbls[i].Font.FontFamily, 20);
                lbls[i].AutoSize = true;
                lbls[i].BringToFront();
            };
            lbls[1].Left = 0;
            lbls[3].Left = ClientSize.Width - lbls[4].PreferredWidth;
            lbls[2].Top -= 80;
            lbls[4].Top += 80;
        }

        //private System.Windows.Forms.Timer fadeTimer;
        //private double opacity = 1.0; // 初期の不透明度（完全に表示）

        //private void StartLblsFadeOut()
        //{
        //    foreach (Label lbl in lbls)
        //    {
        //        opacity = 1.0; // 最初は完全に表示されている
        //        lbl.Visible = true;

        //        fadeTimer = new System.Windows.Forms.Timer();
        //        fadeTimer.Interval = 50; // 毎50msごとに実行
        //        fadeTimer.Tick += FadeOut;
        //        fadeTimer.Start();
        //    }
        //}

        //private void FadeOut(object sender, EventArgs e)
        //{
        //    foreach (Label lbl in lbls)
        //    {
        //        opacity -= 0.05; // 徐々に透明度を下げる
        //        if (opacity <= 0)
        //        {
        //            fadeTimer.Stop();
        //            lbl.Visible = false;
        //            lbl.Dispose(); // 完全に消す
        //        }
        //        else
        //        {
        //            // alphaが0～255の範囲になるように修正
        //            lbl.ForeColor = Color.FromArgb((int)(opacity * 255), lbl.ForeColor.R, lbl.ForeColor.G, lbl.ForeColor.B);
        //        }
        //    }
        //}


        private void IME_Load(object sender, EventArgs e)
        {

        }
    }
}
