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
        string[] conf = ["", ""];
        string[] pending = ["", ""];
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

        public void UpdatePendingAndCurrent(string fpen, string current,string bpen)
        {

            pending[0]= fpen;
            this.current = current;
            pending[1]= bpen;
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
            switch (ftag1)
            {
                case "1":

                    if (conf[0] == "") return;
                    conf[1] = conf[0].Substring(conf[0].Length - 1) + conf[1];
                    conf[0] = conf[0].Substring(0, conf[0].Length - 1);

                    updateDisplay();
                    return;

                case "3":
                    if (conf[1] == "") return;
                    conf[0] += conf[1].Substring(0, 1); 
                    conf[1] = conf[1].Substring(1);
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
            richTextBox1.AppendText(conf[0]);

            int start = richTextBox1.Text.Length;
            richTextBox1.AppendText(pending[0]);
            richTextBox1.Select(start, pending[0].Length);
            richTextBox1.SelectionColor = Color.Blue;
            richTextBox1.SelectionBackColor = Color.LightCyan;

            start = richTextBox1.Text.Length;
            richTextBox1.AppendText(current);
            richTextBox1.Select(start, current.Length);
            richTextBox1.SelectionColor = Color.Blue;
            richTextBox1.SelectionBackColor = Color.LightBlue;

            //start = richTextBox1.Text.Length;
            //richTextBox1.AppendText("|");
            //richTextBox1.Select(start, "|".Length);
            //richTextBox1.SelectionColor = Color.Purple;
            //richTextBox1.SelectionBackColor = Color.White;

            start = richTextBox1.Text.Length;
            richTextBox1.AppendText(pending[1]);
            richTextBox1.Select(start, pending[1].Length);
            richTextBox1.SelectionColor = Color.Blue;
            richTextBox1.SelectionBackColor = Color.Yellow;

            start = richTextBox1.Text.Length;
            richTextBox1.AppendText(conf[1]);
            richTextBox1.Select(start, conf[1].Length);
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
            richTextBox1.SelectionBackColor = Color.LightGray;
        }

        private void OpenIME_Click(object sender, EventArgs e)
        {
            OpenIme();
        }
    }
}
