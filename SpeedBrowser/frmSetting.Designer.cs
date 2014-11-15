namespace SpeedBrowser
{
    partial class frmSetting
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
            this.txtIndex = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.trSpeed = new System.Windows.Forms.TrackBar();
            this.btnon = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCurpage = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboIE = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.trSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "主页";
            // 
            // txtIndex
            // 
            this.txtIndex.Location = new System.Drawing.Point(207, 27);
            this.txtIndex.Name = "txtIndex";
            this.txtIndex.Size = new System.Drawing.Size(259, 25);
            this.txtIndex.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "速度";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Location = new System.Drawing.Point(98, 119);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(96, 25);
            this.txtSpeed.TabIndex = 3;
            // 
            // trSpeed
            // 
            this.trSpeed.Location = new System.Drawing.Point(200, 113);
            this.trSpeed.Maximum = 100;
            this.trSpeed.Minimum = -99;
            this.trSpeed.Name = "trSpeed";
            this.trSpeed.Size = new System.Drawing.Size(259, 56);
            this.trSpeed.TabIndex = 4;
            this.trSpeed.Scroll += new System.EventHandler(this.trSpeed_Scroll);
            // 
            // btnon
            // 
            this.btnon.Location = new System.Drawing.Point(200, 196);
            this.btnon.Name = "btnon";
            this.btnon.Size = new System.Drawing.Size(118, 39);
            this.btnon.TabIndex = 5;
            this.btnon.Text = "确定";
            this.btnon.UseVisualStyleBackColor = true;
            this.btnon.Click += new System.EventHandler(this.btnon_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(341, 196);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(118, 39);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCurpage
            // 
            this.btnCurpage.Location = new System.Drawing.Point(98, 27);
            this.btnCurpage.Name = "btnCurpage";
            this.btnCurpage.Size = new System.Drawing.Size(102, 25);
            this.btnCurpage.TabIndex = 7;
            this.btnCurpage.Text = "当前页";
            this.btnCurpage.UseVisualStyleBackColor = true;
            this.btnCurpage.Click += new System.EventHandler(this.btnCurpage_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "IE版本";
            // 
            // comboIE
            // 
            this.comboIE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboIE.FormattingEnabled = true;
            this.comboIE.Items.AddRange(new object[] {
            "默认",
            "Internet Explorer 7",
            "Internet Explorer 8",
            "Internet Explorer 8 强制",
            "Internet Explorer 9",
            "Internet Explorer 9 强制",
            "Internet Explorer 10",
            "Internet Explorer 10 强制",
            "Internet Explorer 11",
            "Internet Explorer 11 强制"});
            this.comboIE.Location = new System.Drawing.Point(98, 75);
            this.comboIE.Name = "comboIE";
            this.comboIE.Size = new System.Drawing.Size(371, 23);
            this.comboIE.TabIndex = 9;
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 265);
            this.Controls.Add(this.comboIE);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCurpage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnon);
            this.Controls.Add(this.trSpeed);
            this.Controls.Add(this.txtSpeed);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIndex);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置";
            this.Load += new System.EventHandler(this.frmSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIndex;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.TrackBar trSpeed;
        private System.Windows.Forms.Button btnon;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCurpage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboIE;
    }
}