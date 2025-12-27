namespace ClubManageApp
{
    partial class ucUserTest
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlThanhVien = new System.Windows.Forms.Panel();
            this.pnlThongKe = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlThanhVien
            // 
            this.pnlThanhVien.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlThanhVien.BackColor = System.Drawing.Color.White;
            this.pnlThanhVien.Location = new System.Drawing.Point(26, 27);
            this.pnlThanhVien.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlThanhVien.Name = "pnlThanhVien";
            this.pnlThanhVien.Size = new System.Drawing.Size(1004, 812);
            this.pnlThanhVien.TabIndex = 4;
            // 
            // pnlThongKe
            // 
            this.pnlThongKe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlThongKe.BackColor = System.Drawing.Color.White;
            this.pnlThongKe.Location = new System.Drawing.Point(1045, 27);
            this.pnlThongKe.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlThongKe.Name = "pnlThongKe";
            this.pnlThongKe.Size = new System.Drawing.Size(423, 812);
            this.pnlThongKe.TabIndex = 5;
            // 
            // ucUserTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(244)))), ((int)(((byte)(247)))));
            this.Controls.Add(this.pnlThongKe);
            this.Controls.Add(this.pnlThanhVien);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ucUserTest";
            this.Size = new System.Drawing.Size(1490, 872);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlThanhVien;
        private System.Windows.Forms.Panel pnlThongKe;
    }
}
