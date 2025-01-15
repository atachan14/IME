namespace IME
{
    partial class PaintForm
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
            color = new Button();
            size = new Button();
            pen = new Button();
            bg = new Button();
            eraser = new Button();
            bucket = new Button();
            SuspendLayout();
            // 
            // color
            // 
            color.Location = new Point(3, 1);
            color.Name = "color";
            color.Size = new Size(25, 25);
            color.TabIndex = 0;
            color.Text = "色";
            color.UseVisualStyleBackColor = true;
            color.Click += color_Click;
            // 
            // size
            // 
            size.Location = new Point(3, 30);
            size.Name = "size";
            size.Size = new Size(25, 25);
            size.TabIndex = 1;
            size.Text = "太";
            size.UseVisualStyleBackColor = true;
            size.Click += size_Click;
            // 
            // pen
            // 
            pen.Location = new Point(3, 195);
            pen.Name = "pen";
            pen.Size = new Size(25, 25);
            pen.TabIndex = 2;
            pen.Text = "書";
            pen.UseVisualStyleBackColor = true;
            // 
            // bg
            // 
            bg.Location = new Point(240, 1);
            bg.Name = "bg";
            bg.Size = new Size(25, 25);
            bg.TabIndex = 3;
            bg.Text = "背";
            bg.UseVisualStyleBackColor = true;
            // 
            // eraser
            // 
            eraser.Location = new Point(57, 195);
            eraser.Name = "eraser";
            eraser.Size = new Size(50, 25);
            eraser.TabIndex = 4;
            eraser.Text = "eraser";
            eraser.UseVisualStyleBackColor = true;
            // 
            // bucket
            // 
            bucket.Location = new Point(219, 195);
            bucket.Name = "bucket";
            bucket.Size = new Size(25, 25);
            bucket.TabIndex = 5;
            bucket.Text = "塗";
            bucket.UseVisualStyleBackColor = true;
            // 
            // PaintForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(274, 221);
            Controls.Add(bucket);
            Controls.Add(eraser);
            Controls.Add(bg);
            Controls.Add(pen);
            Controls.Add(size);
            Controls.Add(color);
            Name = "PaintForm";
            Text = "PaintForm";
            ResumeLayout(false);
        }

        #endregion

        private Button color;
        private Button size;
        private Button pen;
        private Button bg;
        private Button eraser;
        private Button bucket;
    }
}