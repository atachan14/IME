using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace IME
{
    public partial class OutPad : Form
    {
        private Form1 form1;
        string[] conf = "";
        string[] pending = "";
        string current = "";


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

        public void NextCurrent(string value)
        {
            pending[0] += current;
            current = value;
            updateDisplay();
        }

        public void TransCurrent(string value)
        {
            current = value;
            updateDisplay();
        }

        public void TransPending(string value)
        {
            pending[0] = value;
            current = "";
            updateDisplay();
        }

        public void Confirmed()
        {
            pending[0] += current;
            frontConf += pending[0];
            backConf += pending[1];
            pending[0] = "";
            current = "";
            pending[1] = "";
            updateDisplay();
        }

        public void CursorMove(string ftag1)
        {
            string[] target;
            if (current != "")
            {
                target = [pending[0], backPending];
            }
            else
            {
                target = [frontConf, backConf];
            }
            switch (ftag1)
            {
                case "1":
                    
                    if (target[0] == "") return;

                    target[1] = target[0].Substring(target[0].Length - 1) + target[1];
                    target[0] = target[0].Substring(0, target[0].Length - 1);
                    updateDisplay();
                    return;

                case "3":
                    if (target[1] == "") return;
                    target[0] += target[1].Substring(0, 1);  // backPending の最初の文字を frontPending の末尾に追加
                    target[1] = target[1].Substring(1);
                    updateDisplay();
                    return;

            }
        }

        public void BackSpace()
        {

        }


        public void updateDisplay()
        {
            richTextBox1.Clear();

            richTextBox1.AppendText(frontConf);

            int start = richTextBox1.Text.Length;
            richTextBox1.AppendText(frontPending);
            richTextBox1.Select(start, frontPending.Length);
            richTextBox1.SelectionColor = Color.Blue;
            richTextBox1.SelectionBackColor = Color.LightCyan;

            start = richTextBox1.Text.Length;
            richTextBox1.AppendText(current);
            richTextBox1.Select(start, current.Length);
            richTextBox1.SelectionColor = Color.Blue;
            richTextBox1.SelectionBackColor = Color.LightBlue;

            start = richTextBox1.Text.Length;
            richTextBox1.AppendText("|");
            richTextBox1.Select(start, "|".Length);
            richTextBox1.SelectionColor = Color.Purple;
            richTextBox1.SelectionBackColor = Color.White;

            start = richTextBox1.Text.Length;
            richTextBox1.AppendText(backPending);
            richTextBox1.Select(start, backPending.Length);
            richTextBox1.SelectionColor = Color.Blue;
            richTextBox1.SelectionBackColor = Color.LightCyan;



            start = richTextBox1.Text.Length;
            richTextBox1.AppendText(backConf);
            richTextBox1.Select(start, backConf.Length);
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
            richTextBox1.SelectionBackColor = richTextBox1.BackColor;


        }

        private void OpenIME_Click(object sender, EventArgs e)
        {
            OpenIme();
        }
    }
}
