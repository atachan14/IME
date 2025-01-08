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
            debugLabel = new Label();
            SuspendLayout();
            // 
            // debugLabel
            // 
            debugLabel.AutoSize = true;
            debugLabel.Location = new Point(47, 31);
            debugLabel.Name = "debugLabel";
            debugLabel.Size = new Size(31, 15);
            debugLabel.TabIndex = 0;
            debugLabel.Text = "isare";
            // 
            // DebugForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(184, 161);
            Controls.Add(debugLabel);
            Name = "DebugForm";
            Text = "DebugLog";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label debugLabel;
    }
}