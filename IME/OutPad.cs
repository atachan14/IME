using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.DataFormats;

namespace IME
{
    public partial class OutPad : Form
    {
        private Form1 form1;
        string[] conf = ["", ""];
        string[] pending = ["", ""];
        string current = "";

        string outMode = "none";
        bool showSourceMode = false;

        string kanjiJsonPath = @"..\..\..\kanjiDict.json";
        Dictionary<string, List<string>> kanjiDict;

        public string OutMode { get => outMode; set => outMode = value; }

        public OutPad()
        {
            InitializeComponent();
            updateDisplay();
            OpenIme();

            GenerateKanjiDict();

        }

        void ToJson(string jsonContent)
        {


            // JSONをDictionaryに変換
            var originalDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonContent);

            // 変換後のDictionary
            var normalizedDict = new Dictionary<string, List<string>>();

            // カタカナをひらがなに変換するための正規表現
            Regex katakanaRegex = new Regex("[ァ-ン]", RegexOptions.Compiled);

            foreach (var entry in originalDict)
            {
                // 読み仮名をひらがなに変換
                string normalizedKey = katakanaRegex.Replace(entry.Key, m => ((char)(m.Value[0] - 'ァ' + 'ぁ')).ToString());

                // 「、」や「-」で区切られた複数の読み仮名を単一のキーにまとめる
                string[] keys = normalizedKey.Split('、', '-', '・');
                foreach (var key in keys)
                {
                    string trimmedKey = key.Trim(); // 空白を除去

                    if (!normalizedDict.ContainsKey(trimmedKey))
                    {
                        normalizedDict[trimmedKey] = new List<string>();
                    }

                    // 漢字をマージ
                    normalizedDict[trimmedKey].AddRange(entry.Value);
                }
            }

            // 重複を削除
            foreach (var key in normalizedDict.Keys)
            {
                normalizedDict[key] = new HashSet<string>(normalizedDict[key]).ToList();
            }

            // JSON形式に変換して出力
            string resultJson = System.Text.Json.JsonSerializer.Serialize(normalizedDict, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(resultJson);
            File.WriteAllText(kanjiJsonPath, resultJson);

            Console.WriteLine("ファイルが保存されました: " + kanjiJsonPath);
        }

        void OpenIme()
        {
            form1 = new Form1(this);
            form1.TopLevel = false;  // 子ウィンドウとして扱う
            form1.FormBorderStyle = FormBorderStyle.None;  // 枠を消す
            form1.Dock = DockStyle.Bottom;  // 下部に配置

            this.Controls.Add(form1);
            form1.Show();
            form1.BringToFront();
        }

        public void ChangeOutMode(string mode)
        {
            outMode = mode;
            switch (mode)
            {
                case "edit":
                    Confirmed();
                    editSetup();
                    return;
                default:
                    return;
            }
        }

        void editSetup()
        {

        }

        void GenerateKanjiDict()
        {


            // ファイルが存在するか確認
            if (File.Exists(kanjiJsonPath))
            {
                // JSONファイルを文字列として読み込む
                string jsonContent = File.ReadAllText(kanjiJsonPath);
                //ToJson(jsonContent);
                // JSONをDictionaryにデシリアライズ
                kanjiDict = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonContent);

            }
        }

        public void ConfShortDelete(string direction)
        {
            switch (direction)
            {
                case "1":
                    if (conf[0] == "") return;
                    conf[0] = conf[0].Substring(0, conf[0].Length - 1);
                    updateDisplay();
                    return;

                case "3":
                    if (conf[1] == "") return;
                    conf[1] = conf[1].Substring(1);
                    updateDisplay();
                    return;
            }
        }

        public void ConfLongDelete(string direction)
        {
            switch (direction)
            {
                case "2":
                    if (conf[0] == "") return;
                    conf[0] = "";
                    updateDisplay();
                    return;

                case "4":
                    if (conf[1] == "") return;
                    conf[1] = "";
                    updateDisplay();
                    return;
            }
            MessageBox.Show("未実装ConfLongDelete");
            return;
        }

        public void UpdatePendingAndCurrent(string fpen, string current, string bpen)
        {

            pending[0] = fpen;
            this.current = current;
            if (this.current == "") this.current = " ";
            pending[1] = bpen;

            updateDisplay();
        }

        public void WriteCryValue(string value)
        {
            pending[0] += value;
            updateDisplay();
        }

        public void Confirmed()
        {
            pending[0] += current;
            conf[0] += pending[0];
            conf[1] = pending[1] + conf[1];

            current = "";
            pending[0] = "";
            pending[1] = "";

            updateDisplay();
        }

        public void ConfMove(string ftag1)
        {
            string[] frontLines = Regex.Split(conf[0], @"(?<=\r\n|\n)");
            int maxX = frontLines[frontLines.Length - 1].Length;
            switch (ftag1)
            {
                case "1":
                    if (conf[0] == "") return;
                    conf[1] = conf[0].Substring(conf[0].Length - 1) + conf[1];
                    conf[0] = conf[0].Substring(0, conf[0].Length - 1);
                    updateDisplay();
                    return;

                case "2":
                    if (conf[0] == "") return;

                    if (frontLines.Length == 1)
                    {
                        conf[1] = conf[0] + conf[1];
                        conf[0] = "";
                        updateDisplay();
                        return;
                    }

                    conf[1] = frontLines[frontLines.Length - 1] + conf[1];
                    frontLines[frontLines.Length - 1] = "";

                    if (maxX < frontLines[frontLines.Length - 2].Length)
                    {
                        conf[1] = frontLines[frontLines.Length - 2].Substring(maxX) + conf[1];
                        frontLines[frontLines.Length - 2] = frontLines[frontLines.Length - 2].Substring(0, maxX);
                    }

                    conf[0] = string.Join("", frontLines);
                    updateDisplay();
                    return;
                case "3":
                    if (conf[1] == "") return;
                    conf[0] += conf[1].Substring(0, 1);
                    conf[1] = conf[1].Substring(1);
                    updateDisplay();
                    return;

                case "4":
                    if (conf[1] == "") return;
                    string[] backLines = Regex.Split(conf[1], @"(?<=\r\n|\n)");
                    if (backLines.Length == 1)
                    {
                        conf[0] += conf[1];
                        conf[1] = "";
                        updateDisplay();
                        return;
                    }

                    conf[0] += backLines[0];
                    backLines[0] = "";

                    if (maxX < backLines[1].Length)
                    {
                        conf[0] += backLines[1].Substring(0, maxX);
                        backLines[1] = backLines[1].Substring(maxX);
                    }

                    conf[1] = string.Join("", backLines);
                    updateDisplay();
                    return;
            }
        }


        public void updateDisplay()
        {
            if (showSourceMode)
            {
                updateShowSourceDisplay();
                return;
            }

            List<(Color font, Color bg)> colors = [
                (Color.Blue,Color.LightCyan),
                (Color.Blue,Color.LightBlue),
                (Color.Blue,Color.Yellow),
                (richTextBox1.ForeColor,Color.LightGray)
                ];

            if (OutMode == "cry")
            {
                colors[0] = (Color.Purple, Color.Orange);
                colors[1] = (Color.Purple, Color.Red);
                colors[2] = (Color.Purple, Color.Yellow);
            }
            if (OutMode == "edit")
            {
                colors[0] = (Color.Blue, Color.LightGreen);
                colors[1] = (Color.Blue, Color.Green);
                colors[2] = (Color.Blue, Color.Yellow);
            }

            richTextBox1.Clear();
            richTextBox1.AppendText(conf[0]);

            string fPen = pending[0].Replace("\n", " \n");
            int start = richTextBox1.Text.Length;
            richTextBox1.AppendText(fPen);
            richTextBox1.Select(start, fPen.Length);
            richTextBox1.SelectionColor = colors[0].font;
            richTextBox1.SelectionBackColor = colors[0].bg;

            string cur = current.Replace("\n", "\n ");
            start = richTextBox1.Text.Length;
            richTextBox1.AppendText(cur);
            richTextBox1.Select(start, cur.Length);
            richTextBox1.SelectionColor = colors[1].font;
            richTextBox1.SelectionBackColor = colors[1].bg;

            // 表示処理のあとに自動スクロール
            richTextBox1.SelectionStart = start; // currentの開始位置
            richTextBox1.ScrollToCaret();        // カーソルの位置までスクロール


            //start = richTextBox1.Text.Length;
            //richTextBox1.AppendText("|");
            //richTextBox1.Select(start, "|".Length);
            //richTextBox1.SelectionColor = Color.Purple;
            //richTextBox1.SelectionBackColor = Color.White;

            string bPen = pending[1].Replace("\n", " \n");
            start = richTextBox1.Text.Length;
            richTextBox1.AppendText(bPen);
            richTextBox1.Select(start, bPen.Length);
            richTextBox1.SelectionColor = colors[2].font;
            richTextBox1.SelectionBackColor = colors[2].bg;

            string bConf = conf[1].Replace("\n", " \n");
            start = richTextBox1.Text.Length;
            richTextBox1.AppendText(bConf);
            richTextBox1.Select(start, bConf.Length);
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
            richTextBox1.SelectionBackColor = Color.LightGray;
        }

        public void updateShowSourceDisplay()
        {
            richTextBox1.Clear();
            richTextBox1.AppendText(conf[0].Replace("\r\n", "\\r\\n").Replace("\n", "\\n"));

            int start = richTextBox1.Text.Length;
            richTextBox1.AppendText(pending[0].Replace("\r\n", "\\r\\n").Replace("\n", "\\n"));
            richTextBox1.Select(start, pending[0].Length);
            richTextBox1.SelectionColor = Color.Blue;
            richTextBox1.SelectionBackColor = Color.LightCyan;

            if (current != null)
            {
                start = richTextBox1.Text.Length;
                richTextBox1.AppendText(current.Replace("\r\n", "\\r\\n").Replace("\n", "\\n"));
                richTextBox1.Select(start, current.Length);
                richTextBox1.SelectionColor = Color.Blue;
                richTextBox1.SelectionBackColor = Color.LightBlue;
            }
            //start = richTextBox1.Text.Length;
            //richTextBox1.AppendText("|");
            //richTextBox1.Select(start, "|".Length);
            //richTextBox1.SelectionColor = Color.Purple;
            //richTextBox1.SelectionBackColor = Color.White;

            start = richTextBox1.Text.Length;
            richTextBox1.AppendText(pending[1].Replace("\r\n", "\\r\\n").Replace("\n", "\\n"));
            richTextBox1.Select(start, pending[1].Length);
            richTextBox1.SelectionColor = Color.Blue;
            richTextBox1.SelectionBackColor = Color.Yellow;

            start = richTextBox1.Text.Length;
            richTextBox1.AppendText(conf[1].Replace("\r\n", "\\r\\n").Replace("\n", "\\n"));
            richTextBox1.Select(start, conf[1].Length);
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
            richTextBox1.SelectionBackColor = Color.LightGray;
        }

        private void OpenIME_Click(object sender, EventArgs e)
        {
            OpenIme();
        }

        private void ShowSource_Click(object sender, EventArgs e)
        {
            if (showSourceMode) { showSourceMode = false; }
            else { showSourceMode = true; }
            updateDisplay();
        }

        public void transKanji()
        {
            Random random = new Random();
            List<string> kanjiList = new();

            string target = pending[0] + current;
            for (int i = target.Length; i > 0; i--)
            {
                if (kanjiDict.TryGetValue(target.Substring(0, i), out List<string> value))
                {
                    kanjiList = kanjiDict[target.Substring(0, i)];
                    string kanji = kanjiList[random.Next(kanjiList.Count)];

                    pending[0] = kanji + target.Substring(i);
                    current = "";
                    break;
                }
            }

            for (int i = 0; i > pending[1].Length; i++)
            {
                if (kanjiDict.TryGetValue(pending[1].Substring(1), out List<string> value))
                {
                    kanjiList = kanjiDict[pending[1].Substring(1)];
                    string kanji = kanjiList[random.Next(kanjiList.Count)];

                    pending[1] = kanji + pending[1].Substring(i);
                    break;
                }
            }
            Confirmed();
        }
    }
}
