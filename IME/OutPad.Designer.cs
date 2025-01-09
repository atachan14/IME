namespace IME
{
    partial class OutPad
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            richTextBox1 = new RichTextBox();
            OpenIME = new Button();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(0, 0);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Vertical;
            richTextBox1.Size = new Size(274, 410);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // OpenIME
            // 
            OpenIME.Location = new Point(173, 416);
            OpenIME.Name = "OpenIME";
            OpenIME.Size = new Size(101, 42);
            OpenIME.TabIndex = 1;
            OpenIME.Text = "↑";
            OpenIME.UseVisualStyleBackColor = true;
            OpenIME.Click += OpenIME_Click;
            // 
            // OutPad
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(275, 461);
            Controls.Add(OpenIME);
            Controls.Add(richTextBox1);
            Name = "OutPad";
            Text = "OutPad";
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox1;
        private Button OpenIME;
    }
}