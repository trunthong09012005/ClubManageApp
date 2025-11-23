    namespace ClubManageApp
    {
        partial class Login
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
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.lbdangky = new System.Windows.Forms.Label();
            this.pt2 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.cbghinho = new Guna.UI2.WinForms.Guna2CheckBox();
            this.btndangnhap = new Guna.UI2.WinForms.Guna2Button();
            this.txtmatkhau = new Guna.UI2.WinForms.Guna2TextBox();
            this.txttaikhoan = new Guna.UI2.WinForms.Guna2TextBox();
            this.ptlogo = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.lbtaikhoan = new System.Windows.Forms.Label();
            this.lbmatkhau = new System.Windows.Forms.Label();
            this.lbTenclb = new System.Windows.Forms.Label();
            this.lbdangnhap = new System.Windows.Forms.Label();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.ptLogo2 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pt2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptlogo)).BeginInit();
            this.guna2Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptLogo2)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(217)))), ((int)(((byte)(255)))));
            this.guna2Panel1.Controls.Add(this.lbdangky);
            this.guna2Panel1.Controls.Add(this.pt2);
            this.guna2Panel1.Controls.Add(this.cbghinho);
            this.guna2Panel1.Controls.Add(this.btndangnhap);
            this.guna2Panel1.Controls.Add(this.txtmatkhau);
            this.guna2Panel1.Controls.Add(this.txttaikhoan);
            this.guna2Panel1.Controls.Add(this.ptlogo);
            this.guna2Panel1.Controls.Add(this.lbtaikhoan);
            this.guna2Panel1.Controls.Add(this.lbmatkhau);
            this.guna2Panel1.Controls.Add(this.lbTenclb);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(300, 450);
            this.guna2Panel1.TabIndex = 0;
            // 
            // lbdangky
            // 
            this.lbdangky.AutoSize = true;
            this.lbdangky.ForeColor = System.Drawing.Color.DimGray;
            this.lbdangky.Location = new System.Drawing.Point(47, 383);
            this.lbdangky.Name = "lbdangky";
            this.lbdangky.Size = new System.Drawing.Size(205, 16);
            this.lbdangky.TabIndex = 1;
            this.lbdangky.Text = "Chưa có tài khoản? Đăng ký ngay";
            this.lbdangky.Click += new System.EventHandler(this.Lbdangky_Click);
            // 
            // pt2
            // 
            this.pt2.Image = global::ClubManageApp.Properties.Resources.logo1;
            this.pt2.ImageRotate = 0F;
            this.pt2.Location = new System.Drawing.Point(-42, 425);
            this.pt2.Name = "pt2";
            this.pt2.Size = new System.Drawing.Size(174, 25);
            this.pt2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pt2.TabIndex = 1;
            this.pt2.TabStop = false;
            // 
            // cbghinho
            // 
            this.cbghinho.AutoSize = true;
            this.cbghinho.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbghinho.CheckedState.BorderRadius = 0;
            this.cbghinho.CheckedState.BorderThickness = 0;
            this.cbghinho.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbghinho.ForeColor = System.Drawing.Color.Purple;
            this.cbghinho.Location = new System.Drawing.Point(15, 301);
            this.cbghinho.Name = "cbghinho";
            this.cbghinho.Size = new System.Drawing.Size(141, 20);
            this.cbghinho.TabIndex = 8;
            this.cbghinho.Text = "Ghi nhớ đăng nhập";
            this.cbghinho.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.cbghinho.UncheckedState.BorderRadius = 0;
            this.cbghinho.UncheckedState.BorderThickness = 0;
            this.cbghinho.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            // 
            // btndangnhap
            // 
            this.btndangnhap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btndangnhap.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btndangnhap.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btndangnhap.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btndangnhap.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btndangnhap.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btndangnhap.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btndangnhap.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btndangnhap.ForeColor = System.Drawing.Color.Purple;
            this.btndangnhap.Location = new System.Drawing.Point(89, 334);
            this.btndangnhap.Name = "btndangnhap";
            this.btndangnhap.Size = new System.Drawing.Size(125, 30);
            this.btndangnhap.TabIndex = 7;
            this.btndangnhap.Text = "Đăng nhập";
            this.btndangnhap.Click += new System.EventHandler(this.Btndangnhap_Click);
            // 
            // txtmatkhau
            // 
            this.txtmatkhau.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtmatkhau.DefaultText = "";
            this.txtmatkhau.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtmatkhau.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtmatkhau.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtmatkhau.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtmatkhau.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtmatkhau.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtmatkhau.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtmatkhau.Location = new System.Drawing.Point(89, 263);
            this.txtmatkhau.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtmatkhau.Name = "txtmatkhau";
            this.txtmatkhau.PlaceholderText = "Nhập mật khẩu...";
            this.txtmatkhau.SelectedText = "";
            this.txtmatkhau.Size = new System.Drawing.Size(177, 31);
            this.txtmatkhau.TabIndex = 6;
            this.txtmatkhau.UseSystemPasswordChar = true;
            // 
            // txttaikhoan
            // 
            this.txttaikhoan.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txttaikhoan.DefaultText = "";
            this.txttaikhoan.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txttaikhoan.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txttaikhoan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txttaikhoan.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txttaikhoan.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txttaikhoan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txttaikhoan.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txttaikhoan.Location = new System.Drawing.Point(89, 211);
            this.txttaikhoan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txttaikhoan.Name = "txttaikhoan";
            this.txttaikhoan.PlaceholderText = "Nhập tài khoản...";
            this.txttaikhoan.SelectedText = "";
            this.txttaikhoan.Size = new System.Drawing.Size(177, 31);
            this.txttaikhoan.TabIndex = 1;
            // 
            // ptlogo
            // 
            this.ptlogo.Image = global::ClubManageApp.Properties.Resources.Logo;
            this.ptlogo.ImageRotate = 0F;
            this.ptlogo.Location = new System.Drawing.Point(74, 18);
            this.ptlogo.Name = "ptlogo";
            this.ptlogo.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.ptlogo.Size = new System.Drawing.Size(140, 140);
            this.ptlogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptlogo.TabIndex = 1;
            this.ptlogo.TabStop = false;
            // 
            // lbtaikhoan
            // 
            this.lbtaikhoan.AutoSize = true;
            this.lbtaikhoan.ForeColor = System.Drawing.Color.Purple;
            this.lbtaikhoan.Location = new System.Drawing.Point(12, 219);
            this.lbtaikhoan.Name = "lbtaikhoan";
            this.lbtaikhoan.Size = new System.Drawing.Size(67, 16);
            this.lbtaikhoan.TabIndex = 5;
            this.lbtaikhoan.Text = "Tài khoản";
            // 
            // lbmatkhau
            // 
            this.lbmatkhau.AutoSize = true;
            this.lbmatkhau.ForeColor = System.Drawing.Color.Purple;
            this.lbmatkhau.Location = new System.Drawing.Point(13, 271);
            this.lbmatkhau.Name = "lbmatkhau";
            this.lbmatkhau.Size = new System.Drawing.Size(61, 16);
            this.lbmatkhau.TabIndex = 3;
            this.lbmatkhau.Text = "Mật khẩu";
            // 
            // lbTenclb
            // 
            this.lbTenclb.AutoSize = true;
            this.lbTenclb.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTenclb.ForeColor = System.Drawing.Color.Purple;
            this.lbTenclb.Location = new System.Drawing.Point(21, 161);
            this.lbTenclb.Name = "lbTenclb";
            this.lbTenclb.Size = new System.Drawing.Size(256, 31);
            this.lbTenclb.TabIndex = 2;
            this.lbTenclb.Text = "CÂU LẠC BỘ KỸ NĂNG";
            this.lbTenclb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbdangnhap
            // 
            this.lbdangnhap.AutoSize = true;
            this.lbdangnhap.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbdangnhap.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbdangnhap.ForeColor = System.Drawing.Color.Purple;
            this.lbdangnhap.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbdangnhap.Location = new System.Drawing.Point(0, 0);
            this.lbdangnhap.Name = "lbdangnhap";
            this.lbdangnhap.Size = new System.Drawing.Size(340, 38);
            this.lbdangnhap.TabIndex = 0;
            this.lbdangnhap.Text = "ĐĂNG NHẬP HỆ THỐNG";
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.guna2Panel2.Controls.Add(this.lbdangnhap);
            this.guna2Panel2.Controls.Add(this.ptLogo2);
            this.guna2Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel2.Location = new System.Drawing.Point(300, 0);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(500, 450);
            this.guna2Panel2.TabIndex = 1;
            // 
            // ptLogo2
            // 
            this.ptLogo2.Image = global::ClubManageApp.Properties.Resources._1;
            this.ptLogo2.ImageRotate = 0F;
            this.ptLogo2.Location = new System.Drawing.Point(-394, -127);
            this.ptLogo2.Name = "ptLogo2";
            this.ptLogo2.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.ptLogo2.Size = new System.Drawing.Size(1267, 619);
            this.ptLogo2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptLogo2.TabIndex = 1;
            this.ptLogo2.TabStop = false;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pt2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptlogo)).EndInit();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptLogo2)).EndInit();
            this.ResumeLayout(false);

            }

            #endregion

            private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
            private System.Windows.Forms.Label lbTenclb;
            private System.Windows.Forms.Label lbtaikhoan;
            private System.Windows.Forms.Label lbmatkhau;
            private Guna.UI2.WinForms.Guna2TextBox txtmatkhau;
            private Guna.UI2.WinForms.Guna2TextBox txttaikhoan;
            private Guna.UI2.WinForms.Guna2Button btndangnhap;
            private Guna.UI2.WinForms.Guna2CheckBox cbghinho;
            private Guna.UI2.WinForms.Guna2CirclePictureBox ptlogo;
            private Guna.UI2.WinForms.Guna2PictureBox pt2;
            private System.Windows.Forms.Label lbdangky;
            private System.Windows.Forms.Label lbdangnhap;
            private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
            private Guna.UI2.WinForms.Guna2CirclePictureBox ptLogo2;
        }
    }

