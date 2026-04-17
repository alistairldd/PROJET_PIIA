namespace TP8
{
    partial class TP8
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            panel1 = new Panel();
            button11 = new Button();
            button12 = new Button();
            button9 = new Button();
            button8 = new Button();
            button10 = new Button();
            button7 = new Button();
            button6 = new Button();
            button13 = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(21, 12);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "Deplacer";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(334, 12);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 1;
            button2.Text = "Rectangle";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(415, 12);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 2;
            button3.Text = "Disque";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(496, 12);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 3;
            button4.Text = "Droite";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(253, 12);
            button5.Name = "button5";
            button5.Size = new Size(75, 23);
            button5.TabIndex = 4;
            button5.Text = "Dessin";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(button11);
            panel1.Controls.Add(button12);
            panel1.Controls.Add(button9);
            panel1.Controls.Add(button8);
            panel1.Controls.Add(button10);
            panel1.Controls.Add(button7);
            panel1.Controls.Add(button6);
            panel1.Location = new Point(294, 41);
            panel1.Name = "panel1";
            panel1.Size = new Size(247, 37);
            panel1.TabIndex = 5;
            // 
            // button11
            // 
            button11.BackColor = Color.White;
            button11.Location = new Point(36, 7);
            button11.Name = "button11";
            button11.Size = new Size(30, 30);
            button11.TabIndex = 7;
            button11.UseVisualStyleBackColor = false;
            button11.Click += button11_Click;
            // 
            // button12
            // 
            button12.BackColor = Color.Black;
            button12.Location = new Point(0, 7);
            button12.Name = "button12";
            button12.Size = new Size(30, 30);
            button12.TabIndex = 8;
            button12.UseVisualStyleBackColor = false;
            button12.Click += button12_Click;
            // 
            // button9
            // 
            button9.BackColor = Color.FromArgb(192, 192, 0);
            button9.Location = new Point(180, 7);
            button9.Name = "button9";
            button9.Size = new Size(30, 30);
            button9.TabIndex = 3;
            button9.UseVisualStyleBackColor = false;
            button9.Click += button9_Click;
            // 
            // button8
            // 
            button8.BackColor = Color.Navy;
            button8.ForeColor = SystemColors.ActiveBorder;
            button8.Location = new Point(144, 7);
            button8.Name = "button8";
            button8.Size = new Size(30, 30);
            button8.TabIndex = 2;
            button8.UseVisualStyleBackColor = false;
            button8.Click += button8_Click;
            // 
            // button10
            // 
            button10.BackColor = Color.Teal;
            button10.Location = new Point(216, 7);
            button10.Name = "button10";
            button10.Size = new Size(30, 30);
            button10.TabIndex = 6;
            button10.UseVisualStyleBackColor = false;
            button10.Click += button10_Click;
            // 
            // button7
            // 
            button7.BackColor = Color.Green;
            button7.Location = new Point(108, 7);
            button7.Name = "button7";
            button7.Size = new Size(30, 30);
            button7.TabIndex = 1;
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // button6
            // 
            button6.BackColor = Color.FromArgb(192, 0, 0);
            button6.Location = new Point(72, 7);
            button6.Name = "button6";
            button6.Size = new Size(30, 30);
            button6.TabIndex = 0;
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // button13
            // 
            button13.Location = new Point(21, 41);
            button13.Name = "button13";
            button13.Size = new Size(75, 23);
            button13.TabIndex = 6;
            button13.Text = "Selection";
            button13.UseVisualStyleBackColor = true;
            button13.Click += button13_Click;
            // 
            // TP8
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button13);
            Controls.Add(panel1);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "TP8";
            Text = "Form1";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Panel panel1;
        private Button button8;
        private Button button7;
        private Button button6;
        private Button button11;
        private Button button12;
        private Button button9;
        private Button button10;
        private Button button13;
    }
}
