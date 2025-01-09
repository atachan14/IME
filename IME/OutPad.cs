﻿using System;
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
        string frontConf = "";
        string pending = "";
        string current = "";
        string backConf = "";

        
        public OutPad()
        {
            InitializeComponent();
            updateDisplay();
            ShowIME();
        }

        void ShowIME()
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
            pending += current;
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
            pending = value;
            current = "";
            updateDisplay();
        }

        public void Confirmed()
        {
            pending += current;
            frontConf += pending;
            current = "";
            pending = "";
            updateDisplay();
        }


        public void updateDisplay()
        {
            richTextBox1.Clear();

            richTextBox1.AppendText(frontConf);

            int start = richTextBox1.Text.Length;  // 現在のカーソル位置
            richTextBox1.AppendText(pending);  // pendingを追加
            richTextBox1.Select(start, pending.Length);
            richTextBox1.SelectionColor = Color.Blue;
            richTextBox1.SelectionBackColor = Color.LightCyan;

            start = richTextBox1.Text.Length; 
            richTextBox1.AppendText(current);  // current
            richTextBox1.Select(start, current.Length);
            richTextBox1.SelectionColor = Color.Blue;  
            richTextBox1.SelectionBackColor = Color.LightBlue;

            start = richTextBox1.Text.Length;
            richTextBox1.AppendText(backConf);
            richTextBox1.Select(start, backConf.Length);
            richTextBox1.SelectionColor = richTextBox1.ForeColor; 
            richTextBox1.SelectionBackColor = richTextBox1.BackColor;
        }
        
    }
}
