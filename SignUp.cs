using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class SignUp : Form
    {
        // Thông tin thành viên từ FormSignUp
        private int? maTV = null;
        private string hoTenThanhVien = "";
        private string emailThanhVien = "";

        // Constructor mặc định (khi đăng ký từ đầu)
        public SignUp()
        {
            InitializeComponent();
        }

        // Constructor nhận thông tin từ FormSignUp
        public SignUp(int maTV, string hoTen, string email)
        {
            InitializeComponent();
            this.maTV = maTV;
            this.hoTenThanhVien = hoTen;
            this.emailThanhVien = email;

            // Hiển thị thông tin thành viên
            ShowMemberInfo();
        }

        // Hiển thị thông tin thành viên đã đăng ký
        private void ShowMemberInfo()
        {
            if (maTV.HasValue)
            {
                // Thêm label để hiển thị thông tin (hoặc bạn có thể thêm vào Designer)
                Label lblInfo = new Label
                {
                    Text = $"Đang tạo tài khoản cho:\n{hoTenThanhVien}\n{emailThanhVien}\nMã TV: {maTV}",
                    AutoSize = true,
                    Location = new Point(15, 420),
                    ForeColor = Color.DarkGreen,
                    Font = new Font("Segoe UI", 8, FontStyle.Italic)
                };
                this.guna2Panel1.Controls.Add(lblInfo);
                lblInfo.BringToFront();
            }
        }

        private void InitializeComponent()
        {
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.txtNhapLai = new Guna.UI2.WinForms.Guna2TextBox();
            this.lbnhaplaimatkhau = new System.Windows.Forms.Label();
            this.lbdangnhap = new System.Windows.Forms.Label();
            this.pt2 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.btndangky = new Guna.UI2.WinForms.Guna2Button();
            this.txtmatkhau = new Guna.UI2.WinForms.Guna2TextBox();
            this.txttaikhoan = new Guna.UI2.WinForms.Guna2TextBox();
            this.ptlogo = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.lbtaikhoan = new System.Windows.Forms.Label();
            this.lbmatkhau = new System.Windows.Forms.Label();
            this.lbTenclb = new System.Windows.Forms.Label();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.lbdangky = new System.Windows.Forms.Label();
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
            this.guna2Panel1.Controls.Add(this.txtNhapLai);
            this.guna2Panel1.Controls.Add(this.lbnhaplaimatkhau);
            this.guna2Panel1.Controls.Add(this.lbdangnhap);
            this.guna2Panel1.Controls.Add(this.pt2);
            this.guna2Panel1.Controls.Add(this.btndangky);
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
            this.guna2Panel1.TabIndex = 1;
            // 
            // txtNhapLai
            // 
            this.txtNhapLai.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNhapLai.DefaultText = "";
            this.txtNhapLai.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtNhapLai.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtNhapLai.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNhapLai.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNhapLai.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNhapLai.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNhapLai.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNhapLai.Location = new System.Drawing.Point(89, 313);
            this.txtNhapLai.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNhapLai.Name = "txtNhapLai";
            this.txtNhapLai.PlaceholderText = "Nhập lại mật khẩu...";
            this.txtNhapLai.SelectedText = "";
            this.txtNhapLai.Size = new System.Drawing.Size(177, 31);
            this.txtNhapLai.TabIndex = 9;
            this.txtNhapLai.UseSystemPasswordChar = true;
            // 
            // lbnhaplaimatkhau
            // 
            this.lbnhaplaimatkhau.AutoSize = true;
            this.lbnhaplaimatkhau.ForeColor = System.Drawing.Color.Purple;
            this.lbnhaplaimatkhau.Location = new System.Drawing.Point(12, 319);
            this.lbnhaplaimatkhau.Name = "lbnhaplaimatkhau";
            this.lbnhaplaimatkhau.Size = new System.Drawing.Size(57, 16);
            this.lbnhaplaimatkhau.TabIndex = 8;
            this.lbnhaplaimatkhau.Text = "Nhập lại";
            // 
            // lbdangnhap
            // 
            this.lbdangnhap.AutoSize = true;
            this.lbdangnhap.ForeColor = System.Drawing.Color.DimGray;
            this.lbdangnhap.Location = new System.Drawing.Point(49, 396);
            this.lbdangnhap.Name = "lbdangnhap";
            this.lbdangnhap.Size = new System.Drawing.Size(207, 16);
            this.lbdangnhap.TabIndex = 1;
            this.lbdangnhap.Text = "Đã có tài khoản? Đăng nhập ngay";
            this.lbdangnhap.Click += new System.EventHandler(this.lbDangNhap_Click);
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
            // btndangky
            // 
            this.btndangky.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btndangky.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btndangky.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btndangky.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btndangky.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btndangky.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btndangky.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btndangky.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btndangky.ForeColor = System.Drawing.Color.Purple;
            this.btndangky.Location = new System.Drawing.Point(89, 356);
            this.btndangky.Name = "btndangky";
            this.btndangky.Size = new System.Drawing.Size(125, 30);
            this.btndangky.TabIndex = 7;
            this.btndangky.Text = "Đăng ký";
            this.btndangky.Click += new System.EventHandler(this.btnDangKy_Click);
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
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.guna2Panel2.Controls.Add(this.lbdangky);
            this.guna2Panel2.Controls.Add(this.ptLogo2);
            this.guna2Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel2.Location = new System.Drawing.Point(300, 0);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(500, 450);
            this.guna2Panel2.TabIndex = 2;
            // 
            // lbdangky
            // 
            this.lbdangky.AutoSize = true;
            this.lbdangky.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbdangky.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbdangky.ForeColor = System.Drawing.Color.Purple;
            this.lbdangky.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbdangky.Location = new System.Drawing.Point(0, 0);
            this.lbdangky.Name = "lbdangky";
            this.lbdangky.Size = new System.Drawing.Size(304, 38);
            this.lbdangky.TabIndex = 0;
            this.lbdangky.Text = "ĐĂNG KÝ TÀI KHOẢN";
            // 
            // ptLogo2
            // 
            this.ptLogo2.Image = global::ClubManageApp.Properties.Resources._1;
            this.ptLogo2.ImageRotate = 0F;
            this.ptLogo2.Location = new System.Drawing.Point(-389, -119);
            this.ptLogo2.Name = "ptLogo2";
            this.ptLogo2.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.ptLogo2.Size = new System.Drawing.Size(1267, 619);
            this.ptLogo2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptLogo2.TabIndex = 2;
            this.ptLogo2.TabStop = false;
            // 
            // SignUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SignUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SignUp";
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pt2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptlogo)).EndInit();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptLogo2)).EndInit();
            this.ResumeLayout(false);

        }

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Label lbdangnhap;
        private Guna.UI2.WinForms.Guna2PictureBox pt2;
        private Guna.UI2.WinForms.Guna2Button btndangky;
        private Guna.UI2.WinForms.Guna2TextBox txtmatkhau;
        private Guna.UI2.WinForms.Guna2TextBox txttaikhoan;
        private Guna.UI2.WinForms.Guna2CirclePictureBox ptlogo;
        private Label lbtaikhoan;
        private Label lbmatkhau;
        private Label lbTenclb;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Label lbdangky;
        private Guna.UI2.WinForms.Guna2CirclePictureBox ptLogo2;
        private Guna.UI2.WinForms.Guna2TextBox txtNhapLai;
        private Label lbnhaplaimatkhau;

        private string connectionString =
            @"Data Source=DESKTOP-EJIGPN3;Initial Catalog=QL_APP_LSC;User ID=sa;Password=1234;TrustServerCertificate=True";

        // 🔐 Hàm băm SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hash)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        // ✅ Sự kiện nút "Đăng ký"
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string username = txttaikhoan.Text.Trim();
            string password = txtmatkhau.Text.Trim();
            string confirm = txtNhapLai.Text.Trim();

            // Validation
            if (username == "" || password == "" || confirm == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirm)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra trùng tài khoản
                    string checkQuery = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDN = @user";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@user", username);

                    int exists = (int)checkCmd.ExecuteScalar();
                    if (exists > 0)
                    {
                        MessageBox.Show("Tên tài khoản đã tồn tại, vui lòng chọn tên khác!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Nếu có MaTV từ FormSignUp
                    if (maTV.HasValue)
                    {
                        // Kiểm tra MaTV đã có tài khoản chưa
                        string checkMaTVQuery = "SELECT COUNT(*) FROM TaiKhoan WHERE MaTV = @maTV";
                        SqlCommand checkMaTVCmd = new SqlCommand(checkMaTVQuery, conn);
                        checkMaTVCmd.Parameters.AddWithValue("@maTV", maTV.Value);

                        int maTVExists = (int)checkMaTVCmd.ExecuteScalar();
                        if (maTVExists > 0)
                        {
                            MessageBox.Show("Mã thành viên này đã có tài khoản!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Tạo tài khoản với MaTV có sẵn
                        string insertQuery = @"
                            INSERT INTO TaiKhoan (MaTV, TenDN, MatKhau, QuyenHan, TrangThai, NgayTao)
                            VALUES (@maTV, @user, @pass, N'Thành viên', N'Hoạt động', GETDATE())";

                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@maTV", maTV.Value);
                        insertCmd.Parameters.AddWithValue("@user", username);
                        insertCmd.Parameters.AddWithValue("@pass", HashPassword(password));
                        insertCmd.ExecuteNonQuery();

                        MessageBox.Show(
                            $"Tạo tài khoản thành công!\n\n" +
                            $"Họ tên: {hoTenThanhVien}\n" +
                            $"Email: {emailThanhVien}\n" +
                            $"Tài khoản: {username}\n\n" +
                            "Bạn có thể đăng nhập ngay bây giờ!",
                            "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        // Đăng ký không qua FormSignUp (tạo cả ThanhVien và TaiKhoan)
                        MessageBox.Show(
                            "Vui lòng nhập thông tin cá nhân trước!\n\n" +
                            "Bạn cần điền thông tin trong form 'Thông tin chi tiết' trước.",
                            "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng ký: " + ex.Message, "Lỗi hệ thống",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Nút trở lại trang đăng nhập
        private void lbDangNhap_Click(object sender, EventArgs e)
        {
            // Nếu đang trong quy trình tạo tài khoản từ FormSignUp
            if (maTV.HasValue)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn hủy tạo tài khoản? Thông tin cá nhân đã được lưu. Bạn sẽ trở về trang trước để có thể chỉnh sửa hoặc thử lại sau.",
                    "Xác nhận hủy",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Trả về cho FormSignUp biết là đã hủy (FormSignUp sẽ xử lý tiếp)
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
                // nếu chọn No thì vẫn ở lại form tạo tài khoản
            }
            else
            {
                // Trường hợp người dùng mở SignUp trực tiếp từ màn hình đăng nhập
                this.Hide();
                Login loginForm = new Login();
                // Khi Login đóng, đóng luôn form này để tránh cửa sổ ẩn còn tồn tại
                loginForm.FormClosed += (s, args) => this.Close();
                loginForm.Show();
            }
        }

    }
}