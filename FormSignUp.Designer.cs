namespace ClubManageApp
{
    partial class FormSignUp
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
            this.lblHoTen = new System.Windows.Forms.Label();
            this.txtHoTen = new System.Windows.Forms.TextBox();
            this.lblNgaySinh = new System.Windows.Forms.Label();
            this.dtpNgaySinh = new System.Windows.Forms.DateTimePicker();
            this.lblGioiTinh = new System.Windows.Forms.Label();
            this.lblLop = new System.Windows.Forms.Label();
            this.lblKhoa = new System.Windows.Forms.Label();
            this.lblSDT = new System.Windows.Forms.Label();
            this.cboGioiTinh = new System.Windows.Forms.ComboBox();
            this.txtLop = new System.Windows.Forms.TextBox();
            this.txtKhoa = new System.Windows.Forms.TextBox();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.btnSignUp = new Guna.UI2.WinForms.Guna2Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblHoTen
            // 
            this.lblHoTen.AutoSize = true;
            this.lblHoTen.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoTen.Location = new System.Drawing.Point(216, 112);
            this.lblHoTen.Name = "lblHoTen";
            this.lblHoTen.Size = new System.Drawing.Size(81, 30);
            this.lblHoTen.TabIndex = 0;
            this.lblHoTen.Text = "Họ tên:";
            // 
            // txtHoTen
            // 
            this.txtHoTen.Location = new System.Drawing.Point(350, 115);
            this.txtHoTen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.Size = new System.Drawing.Size(224, 26);
            this.txtHoTen.TabIndex = 1;
            // 
            // lblNgaySinh
            // 
            this.lblNgaySinh.AutoSize = true;
            this.lblNgaySinh.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lblNgaySinh.Location = new System.Drawing.Point(216, 164);
            this.lblNgaySinh.Name = "lblNgaySinh";
            this.lblNgaySinh.Size = new System.Drawing.Size(114, 30);
            this.lblNgaySinh.TabIndex = 2;
            this.lblNgaySinh.Text = "Ngày sinh:";
            // 
            // dtpNgaySinh
            // 
            this.dtpNgaySinh.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpNgaySinh.Location = new System.Drawing.Point(350, 165);
            this.dtpNgaySinh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpNgaySinh.Name = "dtpNgaySinh";
            this.dtpNgaySinh.Size = new System.Drawing.Size(224, 26);
            this.dtpNgaySinh.TabIndex = 3;
            // 
            // lblGioiTinh
            // 
            this.lblGioiTinh.AutoSize = true;
            this.lblGioiTinh.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lblGioiTinh.Location = new System.Drawing.Point(216, 218);
            this.lblGioiTinh.Name = "lblGioiTinh";
            this.lblGioiTinh.Size = new System.Drawing.Size(100, 30);
            this.lblGioiTinh.TabIndex = 4;
            this.lblGioiTinh.Text = "Giới tính:";
            // 
            // lblLop
            // 
            this.lblLop.AutoSize = true;
            this.lblLop.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lblLop.Location = new System.Drawing.Point(216, 266);
            this.lblLop.Name = "lblLop";
            this.lblLop.Size = new System.Drawing.Size(52, 30);
            this.lblLop.TabIndex = 5;
            this.lblLop.Text = "Lớp:";
            // 
            // lblKhoa
            // 
            this.lblKhoa.AutoSize = true;
            this.lblKhoa.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lblKhoa.Location = new System.Drawing.Point(216, 320);
            this.lblKhoa.Name = "lblKhoa";
            this.lblKhoa.Size = new System.Drawing.Size(67, 30);
            this.lblKhoa.TabIndex = 6;
            this.lblKhoa.Text = "Khoa:";
            // 
            // lblSDT
            // 
            this.lblSDT.AutoSize = true;
            this.lblSDT.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lblSDT.Location = new System.Drawing.Point(216, 376);
            this.lblSDT.Name = "lblSDT";
            this.lblSDT.Size = new System.Drawing.Size(56, 30);
            this.lblSDT.TabIndex = 7;
            this.lblSDT.Text = "SĐT:";
            // 
            // cboGioiTinh
            // 
            this.cboGioiTinh.FormattingEnabled = true;
            this.cboGioiTinh.Location = new System.Drawing.Point(350, 216);
            this.cboGioiTinh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboGioiTinh.Name = "cboGioiTinh";
            this.cboGioiTinh.Size = new System.Drawing.Size(224, 28);
            this.cboGioiTinh.TabIndex = 8;
            // 
            // txtLop
            // 
            this.txtLop.Location = new System.Drawing.Point(350, 269);
            this.txtLop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLop.Name = "txtLop";
            this.txtLop.Size = new System.Drawing.Size(224, 26);
            this.txtLop.TabIndex = 9;
            // 
            // txtKhoa
            // 
            this.txtKhoa.Location = new System.Drawing.Point(350, 320);
            this.txtKhoa.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtKhoa.Name = "txtKhoa";
            this.txtKhoa.Size = new System.Drawing.Size(224, 26);
            this.txtKhoa.TabIndex = 10;
            // 
            // txtSDT
            // 
            this.txtSDT.Location = new System.Drawing.Point(350, 378);
            this.txtSDT.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Size = new System.Drawing.Size(224, 26);
            this.txtSDT.TabIndex = 11;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lblEmail.Location = new System.Drawing.Point(216, 431);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(72, 30);
            this.lblEmail.TabIndex = 12;
            this.lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(350, 432);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(224, 26);
            this.txtEmail.TabIndex = 13;
            // 
            // btnSignUp
            // 
            this.btnSignUp.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSignUp.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSignUp.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSignUp.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSignUp.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSignUp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSignUp.ForeColor = System.Drawing.Color.White;
            this.btnSignUp.Location = new System.Drawing.Point(350, 494);
            this.btnSignUp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSignUp.Name = "btnSignUp";
            this.btnSignUp.Size = new System.Drawing.Size(143, 40);
            this.btnSignUp.TabIndex = 14;
            this.btnSignUp.Text = "Đăng ký";
            this.btnSignUp.Click += new System.EventHandler(this.btnDangKy_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(228, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(351, 45);
            this.label1.TabIndex = 15;
            this.label1.Text = "THÔNG TIN CHI TIẾT ";
            // 
            // FormSignUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(827, 562);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSignUp);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtSDT);
            this.Controls.Add(this.txtKhoa);
            this.Controls.Add(this.txtLop);
            this.Controls.Add(this.cboGioiTinh);
            this.Controls.Add(this.lblSDT);
            this.Controls.Add(this.lblKhoa);
            this.Controls.Add(this.lblLop);
            this.Controls.Add(this.lblGioiTinh);
            this.Controls.Add(this.dtpNgaySinh);
            this.Controls.Add(this.lblNgaySinh);
            this.Controls.Add(this.txtHoTen);
            this.Controls.Add(this.lblHoTen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormSignUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng ký thành viên";
            this.Load += new System.EventHandler(this.FormSignUp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHoTen;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.Label lblNgaySinh;
        private System.Windows.Forms.DateTimePicker dtpNgaySinh;
        private System.Windows.Forms.Label lblGioiTinh;
        private System.Windows.Forms.Label lblLop;
        private System.Windows.Forms.Label lblKhoa;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.ComboBox cboGioiTinh;
        private System.Windows.Forms.TextBox txtLop;
        private System.Windows.Forms.TextBox txtKhoa;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private Guna.UI2.WinForms.Guna2Button btnSignUp;
        private System.Windows.Forms.Label label1;
    }
}