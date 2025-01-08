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
    public partial class DebugForm : Form
    {

        private System.Windows.Forms.Timer debugTimer;
        private Form1 form1;

        public DebugForm(Form1 parentForm)
        {
            InitializeComponent();
            form1 = parentForm;
            debugTimer = new System.Windows.Forms.Timer
            {
                Interval = 100 // 100msごとにチェック
            };
            debugTimer.Tick += (s, e) =>
            {
                debug1.Text = $"isPressing: {form1.IsPressing}";
                debug2.Text = $"debugCount: {form1.DebugCount}";
            };
            debugTimer.Start();
        }

        private void debugLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
