namespace TipCalculator
{
    partial class TipCalc
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
            this.CheckLbl = new System.Windows.Forms.Label();
            this.checkBox = new System.Windows.Forms.TextBox();
            this.TipLbl = new System.Windows.Forms.Label();
            this.tipBox = new System.Windows.Forms.TextBox();
            this.calcBtn = new System.Windows.Forms.Button();
            this.TotalBox = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CheckLbl
            // 
            this.CheckLbl.AutoSize = true;
            this.CheckLbl.Location = new System.Drawing.Point(93, 126);
            this.CheckLbl.Name = "CheckLbl";
            this.CheckLbl.Size = new System.Drawing.Size(99, 17);
            this.CheckLbl.TabIndex = 0;
            this.CheckLbl.Text = "Check Amount";
            // 
            // checkBox
            // 
            this.checkBox.Location = new System.Drawing.Point(207, 126);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(100, 22);
            this.checkBox.TabIndex = 1;
            // 
            // TipLbl
            // 
            this.TipLbl.AutoSize = true;
            this.TipLbl.Location = new System.Drawing.Point(93, 167);
            this.TipLbl.Name = "TipLbl";
            this.TipLbl.Size = new System.Drawing.Size(44, 17);
            this.TipLbl.TabIndex = 2;
            this.TipLbl.Text = "Tip %";
            // 
            // tipBox
            // 
            this.tipBox.Location = new System.Drawing.Point(207, 167);
            this.tipBox.Name = "tipBox";
            this.tipBox.Size = new System.Drawing.Size(100, 22);
            this.tipBox.TabIndex = 3;
            // 
            // calcBtn
            // 
            this.calcBtn.Location = new System.Drawing.Point(177, 207);
            this.calcBtn.Name = "calcBtn";
            this.calcBtn.Size = new System.Drawing.Size(130, 33);
            this.calcBtn.TabIndex = 4;
            this.calcBtn.Text = "Calculate";
            this.calcBtn.UseVisualStyleBackColor = true;
            this.calcBtn.Click += new System.EventHandler(this.calcBtn_Click);
            this.calcBtn.MouseEnter += new System.EventHandler(this.calcBtn_MouseEnter);
            this.calcBtn.MouseLeave += new System.EventHandler(this.calcBtn_MouseLeave);
            // 
            // TotalBox
            // 
            this.TotalBox.AutoSize = true;
            this.TotalBox.Location = new System.Drawing.Point(93, 266);
            this.TotalBox.Name = "TotalBox";
            this.TotalBox.Size = new System.Drawing.Size(40, 17);
            this.TotalBox.TabIndex = 5;
            this.TotalBox.Text = "Total";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(207, 261);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 22);
            this.textBox3.TabIndex = 6;
            // 
            // TipCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 468);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.TotalBox);
            this.Controls.Add(this.calcBtn);
            this.Controls.Add(this.tipBox);
            this.Controls.Add(this.TipLbl);
            this.Controls.Add(this.checkBox);
            this.Controls.Add(this.CheckLbl);
            this.Name = "TipCalc";
            this.Text = "Tip Calculator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CheckLbl;
        private System.Windows.Forms.TextBox checkBox;
        private System.Windows.Forms.Label TipLbl;
        private System.Windows.Forms.TextBox tipBox;
        private System.Windows.Forms.Button calcBtn;
        private System.Windows.Forms.Label TotalBox;
        private System.Windows.Forms.TextBox textBox3;
    }
}

