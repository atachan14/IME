namespace IME
{
    partial class DebugForm
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
            debug1 = new Label();
            label1 = new Label();
            debug2 = new Label();
            SuspendLayout();
            // 
            // debugLabel
            // 
            debug1.AutoSize = true;
            debug1.Location = new Point(66, 51);
            debug1.Name = "debugLabel";
            debug1.Size = new Size(31, 15);
            debug1.TabIndex = 0;
            debug1.Text = "isare";
            debug1.Click += debugLabel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(38, 0);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 1;
            label1.Text = "label1";
            // 
            // debug2
            // 
            debug2.AutoSize = true;
            debug2.Location = new Point(44, 87);
            debug2.Name = "debug2";
            debug2.Size = new Size(38, 15);
            debug2.TabIndex = 2;
            debug2.Text = "label2";
            // 
            // DebugForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(184, 161);
            Controls.Add(debug2);
            Controls.Add(label1);
            Controls.Add(debug1);
            Name = "DebugForm";
            Text = "DebugLog";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label debug1;
        private Label label1;
        private Label debug2;
    }
}