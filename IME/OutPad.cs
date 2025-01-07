using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IME
{
    public partial class OutPad : Form
    {
        public OutPad()
        {
            InitializeComponent();
        }
        public void DisplayText(string value)
        {
            richTextBox1.AppendText(value);  // 改行して追加
        }
    }
}
