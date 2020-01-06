namespace WebScraper
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.yearTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.semesterBox = new System.Windows.Forms.ComboBox();
            this.submit1 = new System.Windows.Forms.Button();
            this.save1 = new System.Windows.Forms.Button();
            this.status1 = new System.Windows.Forms.Label();
            this.amountBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.courseBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(85, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Year:";
            // 
            // yearTextBox
            // 
            this.yearTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yearTextBox.Location = new System.Drawing.Point(87, 168);
            this.yearTextBox.Name = "yearTextBox";
            this.yearTextBox.Size = new System.Drawing.Size(248, 23);
            this.yearTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(84, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Semester:";
            // 
            // semesterBox
            // 
            this.semesterBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.semesterBox.FormattingEnabled = true;
            this.semesterBox.Items.AddRange(new object[] {
            "Spring",
            "Summer",
            "Fall"});
            this.semesterBox.Location = new System.Drawing.Point(87, 227);
            this.semesterBox.Name = "semesterBox";
            this.semesterBox.Size = new System.Drawing.Size(248, 24);
            this.semesterBox.TabIndex = 2;
            // 
            // submit1
            // 
            this.submit1.BackColor = System.Drawing.SystemColors.Highlight;
            this.submit1.FlatAppearance.BorderSize = 0;
            this.submit1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.submit1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.submit1.Location = new System.Drawing.Point(88, 423);
            this.submit1.Name = "submit1";
            this.submit1.Size = new System.Drawing.Size(80, 40);
            this.submit1.TabIndex = 5;
            this.submit1.Text = "Submit";
            this.submit1.UseVisualStyleBackColor = false;
            this.submit1.Click += new System.EventHandler(this.submit1_Click);
            // 
            // save1
            // 
            this.save1.BackColor = System.Drawing.Color.ForestGreen;
            this.save1.Enabled = false;
            this.save1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.save1.Location = new System.Drawing.Point(236, 423);
            this.save1.Name = "save1";
            this.save1.Size = new System.Drawing.Size(100, 40);
            this.save1.TabIndex = 6;
            this.save1.Text = "Save Data";
            this.save1.UseVisualStyleBackColor = false;
            this.save1.Click += new System.EventHandler(this.save1_Click);
            // 
            // status1
            // 
            this.status1.AutoSize = true;
            this.status1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status1.ForeColor = System.Drawing.Color.Red;
            this.status1.Location = new System.Drawing.Point(85, 71);
            this.status1.MaximumSize = new System.Drawing.Size(230, 0);
            this.status1.Name = "status1";
            this.status1.Size = new System.Drawing.Size(0, 18);
            this.status1.TabIndex = 6;
            // 
            // amountBox
            // 
            this.amountBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.amountBox.Location = new System.Drawing.Point(88, 293);
            this.amountBox.Name = "amountBox";
            this.amountBox.Size = new System.Drawing.Size(248, 23);
            this.amountBox.TabIndex = 3;
            this.amountBox.Text = "100";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(84, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Number of Courses:";
            // 
            // courseBox
            // 
            this.courseBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.courseBox.Location = new System.Drawing.Point(88, 360);
            this.courseBox.Multiline = true;
            this.courseBox.Name = "courseBox";
            this.courseBox.Size = new System.Drawing.Size(248, 57);
            this.courseBox.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(84, 324);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "Course Number(s):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(83, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(274, 22);
            this.label5.TabIndex = 11;
            this.label5.Text = "Web Scraper for U of U Courses";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(87, 345);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(196, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Enter a comma seperated list for multiple";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 526);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.courseBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.amountBox);
            this.Controls.Add(this.status1);
            this.Controls.Add(this.save1);
            this.Controls.Add(this.submit1);
            this.Controls.Add(this.semesterBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.yearTextBox);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Web Scraper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox yearTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox semesterBox;
        private System.Windows.Forms.Button submit1;
        private System.Windows.Forms.Button save1;
        private System.Windows.Forms.Label status1;
        private System.Windows.Forms.TextBox amountBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox courseBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

