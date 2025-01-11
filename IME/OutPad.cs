using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        bool crySet = false;
        bool showSourceMode = false;

        public bool CrySet { get => crySet; set => crySet = value; }

        public OutPad()
        {
            InitializeComponent();
            updateDisplay();
            OpenIme();
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
            MessageBox.Show("未実装ConfLongDelete");
            return;
        }

        public void UpdatePendingAndCurrent(string fpen, string current, string bpen)
        {

            pending[0] = fpen;
            this.current = current;
            pending[1] = bpen;

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

            if (CrySet)
            {
                colors[0] = (Color.Purple,Color.Orange);
                colors[1] = (Color.Purple, Color.Red);
                colors[2] = (Color.Purple, Color.Yellow);
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
    }
}
