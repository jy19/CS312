namespace WindowsFormsApplication1
{
    partial class Simplex
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
            this.solveBtn = new System.Windows.Forms.Button();
            this.CopperText = new System.Windows.Forms.TextBox();
            this.SilverText = new System.Windows.Forms.TextBox();
            this.GoldText = new System.Windows.Forms.TextBox();
            this.PlatText = new System.Windows.Forms.TextBox();
            this.CopperLbl = new System.Windows.Forms.Label();
            this.GoldLbl = new System.Windows.Forms.Label();
            this.PlatLbl = new System.Windows.Forms.Label();
            this.SilverLbl = new System.Windows.Forms.Label();
            this.ResultTxt = new System.Windows.Forms.TextBox();
            this.ResultLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // solveBtn
            // 
            this.solveBtn.Location = new System.Drawing.Point(103, 244);
            this.solveBtn.Name = "solveBtn";
            this.solveBtn.Size = new System.Drawing.Size(75, 23);
            this.solveBtn.TabIndex = 0;
            this.solveBtn.Text = "Solve";
            this.solveBtn.UseVisualStyleBackColor = true;
            this.solveBtn.Click += new System.EventHandler(this.solveBtn_Click);
            // 
            // CopperText
            // 
            this.CopperText.Location = new System.Drawing.Point(121, 32);
            this.CopperText.Name = "CopperText";
            this.CopperText.Size = new System.Drawing.Size(104, 20);
            this.CopperText.TabIndex = 1;
            // 
            // SilverText
            // 
            this.SilverText.Location = new System.Drawing.Point(121, 84);
            this.SilverText.Name = "SilverText";
            this.SilverText.Size = new System.Drawing.Size(104, 20);
            this.SilverText.TabIndex = 2;
            // 
            // GoldText
            // 
            this.GoldText.Location = new System.Drawing.Point(121, 58);
            this.GoldText.Name = "GoldText";
            this.GoldText.Size = new System.Drawing.Size(104, 20);
            this.GoldText.TabIndex = 3;
            // 
            // PlatText
            // 
            this.PlatText.Location = new System.Drawing.Point(121, 110);
            this.PlatText.Name = "PlatText";
            this.PlatText.Size = new System.Drawing.Size(104, 20);
            this.PlatText.TabIndex = 4;
            // 
            // CopperLbl
            // 
            this.CopperLbl.AutoSize = true;
            this.CopperLbl.Location = new System.Drawing.Point(62, 39);
            this.CopperLbl.Name = "CopperLbl";
            this.CopperLbl.Size = new System.Drawing.Size(41, 13);
            this.CopperLbl.TabIndex = 5;
            this.CopperLbl.Text = "Copper";
            // 
            // GoldLbl
            // 
            this.GoldLbl.AutoSize = true;
            this.GoldLbl.Location = new System.Drawing.Point(62, 61);
            this.GoldLbl.Name = "GoldLbl";
            this.GoldLbl.Size = new System.Drawing.Size(29, 13);
            this.GoldLbl.TabIndex = 6;
            this.GoldLbl.Text = "Gold";
            this.GoldLbl.UseWaitCursor = true;
            // 
            // PlatLbl
            // 
            this.PlatLbl.AutoSize = true;
            this.PlatLbl.Location = new System.Drawing.Point(62, 113);
            this.PlatLbl.Name = "PlatLbl";
            this.PlatLbl.Size = new System.Drawing.Size(47, 13);
            this.PlatLbl.TabIndex = 7;
            this.PlatLbl.Text = "Platinum";
            // 
            // SilverLbl
            // 
            this.SilverLbl.AutoSize = true;
            this.SilverLbl.Location = new System.Drawing.Point(62, 87);
            this.SilverLbl.Name = "SilverLbl";
            this.SilverLbl.Size = new System.Drawing.Size(33, 13);
            this.SilverLbl.TabIndex = 8;
            this.SilverLbl.Text = "Silver";
            // 
            // ResultTxt
            // 
            this.ResultTxt.Location = new System.Drawing.Point(121, 136);
            this.ResultTxt.Name = "ResultTxt";
            this.ResultTxt.Size = new System.Drawing.Size(104, 20);
            this.ResultTxt.TabIndex = 9;
            // 
            // ResultLbl
            // 
            this.ResultLbl.AutoSize = true;
            this.ResultLbl.Location = new System.Drawing.Point(62, 139);
            this.ResultLbl.Name = "ResultLbl";
            this.ResultLbl.Size = new System.Drawing.Size(37, 13);
            this.ResultLbl.TabIndex = 10;
            this.ResultLbl.Text = "Result";
            // 
            // Simplex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 279);
            this.Controls.Add(this.ResultLbl);
            this.Controls.Add(this.ResultTxt);
            this.Controls.Add(this.SilverLbl);
            this.Controls.Add(this.PlatLbl);
            this.Controls.Add(this.GoldLbl);
            this.Controls.Add(this.CopperLbl);
            this.Controls.Add(this.PlatText);
            this.Controls.Add(this.GoldText);
            this.Controls.Add(this.SilverText);
            this.Controls.Add(this.CopperText);
            this.Controls.Add(this.solveBtn);
            this.Name = "Simplex";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button solveBtn;
        private System.Windows.Forms.TextBox CopperText;
        private System.Windows.Forms.TextBox SilverText;
        private System.Windows.Forms.TextBox GoldText;
        private System.Windows.Forms.TextBox PlatText;
        private System.Windows.Forms.Label CopperLbl;
        private System.Windows.Forms.Label GoldLbl;
        private System.Windows.Forms.Label PlatLbl;
        private System.Windows.Forms.Label SilverLbl;
        private System.Windows.Forms.TextBox ResultTxt;
        private System.Windows.Forms.Label ResultLbl;
    }
}

