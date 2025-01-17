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
            width = new Button();
            pen = new Button();
            bg = new Button();
            eraser = new Button();
            bucket = new Button();
            c0 = new Button();
            c1 = new Button();
            c2 = new Button();
            c3 = new Button();
            c4 = new Button();
            c5 = new Button();
            save = new Button();
            cansel = new Button();
            trackBar = new TrackBar();
            pictureBoxColorMap = new PictureBox();
            New = new Button();
            ((System.ComponentModel.ISupportInitialize)trackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxColorMap).BeginInit();
            SuspendLayout();
            // 
            // color
            // 
            color.BackColor = Color.Black;
            color.FlatStyle = FlatStyle.Flat;
            color.Location = new Point(3, 4);
            color.Name = "color";
            color.Size = new Size(30, 30);
            color.TabIndex = 0;
            color.UseVisualStyleBackColor = false;
            color.Click += color_Click;
            // 
            // width
            // 
            width.Location = new Point(3, 37);
            width.Name = "width";
            width.Size = new Size(30, 30);
            width.TabIndex = 1;
            width.Text = "16";
            width.UseVisualStyleBackColor = true;
            width.Click += size_Click;
            // 
            // pen
            // 
            pen.Location = new Point(3, 195);
            pen.Name = "pen";
            pen.Size = new Size(25, 25);
            pen.TabIndex = 2;
            pen.Text = "描";
            pen.UseVisualStyleBackColor = true;
            pen.Click += pen_Click;
            // 
            // bg
            // 
            bg.Location = new Point(237, 7);
            bg.Name = "bg";
            bg.Size = new Size(25, 25);
            bg.TabIndex = 3;
            bg.Text = "背";
            bg.UseVisualStyleBackColor = true;
            bg.Click += bg_Click;
            // 
            // eraser
            // 
            eraser.Location = new Point(65, 195);
            eraser.Name = "eraser";
            eraser.Size = new Size(25, 25);
            eraser.TabIndex = 4;
            eraser.Text = "消";
            eraser.UseVisualStyleBackColor = true;
            eraser.Click += eraser_Click;
            // 
            // bucket
            // 
            bucket.Location = new Point(34, 195);
            bucket.Name = "bucket";
            bucket.Size = new Size(25, 25);
            bucket.TabIndex = 5;
            bucket.Text = "塗";
            bucket.UseVisualStyleBackColor = true;
            bucket.Click += bucket_Click;
            // 
            // c0
            // 
            c0.BackColor = SystemColors.MenuHighlight;
            c0.FlatAppearance.BorderSize = 0;
            c0.FlatStyle = FlatStyle.Flat;
            c0.Location = new Point(39, 7);
            c0.Name = "c0";
            c0.Size = new Size(25, 25);
            c0.TabIndex = 6;
            c0.UseVisualStyleBackColor = false;
            c0.MouseDown += cb_MouseDown;
            c0.MouseUp += cb_MouseUp;
            // 
            // c1
            // 
            c1.BackColor = SystemColors.MenuHighlight;
            c1.FlatAppearance.BorderSize = 0;
            c1.FlatStyle = FlatStyle.Flat;
            c1.Location = new Point(70, 7);
            c1.Name = "c1";
            c1.Size = new Size(25, 25);
            c1.TabIndex = 7;
            c1.UseVisualStyleBackColor = false;
            c1.MouseDown += cb_MouseDown;
            c1.MouseUp += cb_MouseUp;
            // 
            // c2
            // 
            c2.BackColor = SystemColors.MenuHighlight;
            c2.FlatAppearance.BorderSize = 0;
            c2.FlatStyle = FlatStyle.Flat;
            c2.Location = new Point(101, 7);
            c2.Name = "c2";
            c2.Size = new Size(25, 25);
            c2.TabIndex = 8;
            c2.UseVisualStyleBackColor = false;
            c2.MouseDown += cb_MouseDown;
            c2.MouseUp += cb_MouseUp;
            // 
            // c3
            // 
            c3.BackColor = SystemColors.MenuHighlight;
            c3.FlatAppearance.BorderSize = 0;
            c3.FlatStyle = FlatStyle.Flat;
            c3.Location = new Point(132, 7);
            c3.Name = "c3";
            c3.Size = new Size(25, 25);
            c3.TabIndex = 9;
            c3.UseVisualStyleBackColor = false;
            c3.MouseDown += cb_MouseDown;
            c3.MouseUp += cb_MouseUp;
            // 
            // c4
            // 
            c4.BackColor = SystemColors.MenuHighlight;
            c4.FlatAppearance.BorderSize = 0;
            c4.FlatStyle = FlatStyle.Flat;
            c4.Location = new Point(163, 7);
            c4.Name = "c4";
            c4.Size = new Size(25, 25);
            c4.TabIndex = 10;
            c4.UseVisualStyleBackColor = false;
            c4.MouseDown += cb_MouseDown;
            c4.MouseUp += cb_MouseUp;
            // 
            // c5
            // 
            c5.BackColor = SystemColors.MenuHighlight;
            c5.FlatAppearance.BorderSize = 0;
            c5.FlatStyle = FlatStyle.Flat;
            c5.Location = new Point(194, 6);
            c5.Name = "c5";
            c5.Size = new Size(25, 25);
            c5.TabIndex = 11;
            c5.UseVisualStyleBackColor = false;
            c5.MouseDown += cb_MouseDown;
            c5.MouseUp += cb_MouseUp;
            // 
            // save
            // 
            save.Location = new Point(237, 195);
            save.Name = "save";
            save.Size = new Size(25, 25);
            save.TabIndex = 12;
            save.Text = "出";
            save.UseVisualStyleBackColor = true;
            save.Click += save_Click;
            // 
            // cansel
            // 
            cansel.Location = new Point(3, 73);
            cansel.Name = "cansel";
            cansel.Size = new Size(25, 25);
            cansel.TabIndex = 13;
            cansel.Text = "戻";
            cansel.UseVisualStyleBackColor = true;
            cansel.Click += cansel_Click;
            // 
            // trackBar
            // 
            trackBar.Location = new Point(34, 32);
            trackBar.Maximum = 40;
            trackBar.Minimum = 1;
            trackBar.Name = "trackBar";
            trackBar.Size = new Size(237, 45);
            trackBar.TabIndex = 14;
            trackBar.Value = 20;
            trackBar.ValueChanged += trackBar_ValueChanged;
            // 
            // pictureBoxColorMap
            // 
            pictureBoxColorMap.Location = new Point(39, 38);
            pictureBoxColorMap.Name = "pictureBoxColorMap";
            pictureBoxColorMap.Size = new Size(223, 85);
            pictureBoxColorMap.TabIndex = 15;
            pictureBoxColorMap.TabStop = false;
            pictureBoxColorMap.MouseClick += pictureBoxColorMap_MouseClick;
            // 
            // New
            // 
            New.Location = new Point(194, 195);
            New.Name = "New";
            New.Size = new Size(25, 25);
            New.TabIndex = 16;
            New.Text = "新";
            New.UseVisualStyleBackColor = true;
            New.Click += New_Click;
            // 
            // PaintForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(274, 221);
            Controls.Add(New);
            Controls.Add(pictureBoxColorMap);
            Controls.Add(trackBar);
            Controls.Add(cansel);
            Controls.Add(save);
            Controls.Add(c5);
            Controls.Add(c4);
            Controls.Add(c3);
            Controls.Add(c2);
            Controls.Add(c1);
            Controls.Add(c0);
            Controls.Add(bucket);
            Controls.Add(eraser);
            Controls.Add(bg);
            Controls.Add(pen);
            Controls.Add(width);
            Controls.Add(color);
            Name = "PaintForm";
            Text = "PaintForm";
            Load += PaintForm_Load;
            ((System.ComponentModel.ISupportInitialize)trackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxColorMap).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button color;
        private Button width;
        private Button pen;
        private Button bg;
        private Button eraser;
        private Button bucket;
        private Button c0;
        private Button c1;
        private Button c2;
        private Button c3;
        private Button c4;
        private Button c5;
        private Button save;
        private Button cansel;
        private TrackBar trackBar;
        private PictureBox pictureBoxColorMap;
        private Button New;
    }
}