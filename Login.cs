    using System;
    using System.Data.SqlClient;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows.Forms;
    using System.Diagnostics;

    namespace ClubManageApp
    {
        public partial class Login : Form
        {
        private string connectionString = @"Data Source=DESKTOP-B7F3HIU;Initial Catalog=QL_APP_LSC;Integrated Security=True;TrustServerCertificate=True";

        public Login()
            {
                InitializeComponent();

                txttaikhoan.KeyDown += TxtBox_KeyDown;
                txtmatkhau.KeyDown += TxtBox_KeyDown;

                // Ghi nhớ tài khoản
                if (!string.IsNullOrEmpty(Properties.Settings.Default.SavedUser))
                {
                    txttaikhoan.Text = Properties.Settings.Default.SavedUser;

                    // Nếu SavedPass trông giống hash (hex, 64 ký tự) thì không điền lại vào textbox
                    var savedPass = Properties.Settings.Default.SavedPass;
                    if (!string.IsNullOrEmpty(savedPass) && !IsHex(savedPass))
                    {
                        txtmatkhau.Text = savedPass;
                        cbghinho.Checked = true;
                    }
                    else
                    {
                        // tránh trường hợp lưu nhầm hash và gây double-hash khi đăng nhập
                        txtmatkhau.Text = "";
                        cbghinho.Checked = false;
                    }
                }
            }

            private void TxtBox_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (sender == txttaikhoan)
                        txtmatkhau.Focus();
                    else
                        btndangnhap.PerformClick();

                    e.SuppressKeyPress = true;
                }
            }
         

            private void Btndangnhap_Click(object sender, EventArgs e)
            {
                string username = txttaikhoan.Text.Trim();
                string password = txtmatkhau.Text; // do not trim password to preserve spaces if any

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        // Lấy thông tin user theo TenDN, sau đó so sánh hash trong C# để biết chính xác lỗi
                        string query = @"
                            SELECT MatKhau, QuyenHan, TrangThai, MaTV
                            FROM TaiKhoan
                            WHERE TenDN = @user";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@user", username);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    // Username không tồn tại
                                    MessageBox.Show("Tên đăng nhập không tồn tại hoặc sai!", "Lỗi",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                string dbHash = reader["MatKhau"]?.ToString().Trim() ?? "";
                                string role = reader["QuyenHan"]?.ToString().Trim() ?? "";
                                string status = reader["TrangThai"]?.ToString().Trim() ?? "";
                                int maTV = Convert.ToInt32(reader["MaTV"]);

                                string hashedPassword = HashPassword(password);
                     
                        
                         

                                // Nếu hash trong DB khác với hash input -> sai mật khẩu
                                if (!string.Equals(dbHash, hashedPassword, StringComparison.OrdinalIgnoreCase))
                                {
                                    // Nếu DB hash bị cắt ngắn (ví dụ cột quá nhỏ) thì cho gợi ý debug
                                    if (dbHash.Length < 64)
                                    {
                                        MessageBox.Show("Sai mật khẩu. (Debug: mật khẩu lưu trên DB có độ dài khác 64 — kiểm tra cột `MatKhau`)", "Lỗi",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    return;
                                }

                                if (!status.Equals("Hoạt động", StringComparison.OrdinalIgnoreCase))
                                {
                                    MessageBox.Show("Tài khoản của bạn đang bị khóa!", "Thông báo",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                // Ghi nhớ đăng nhập (lưu mật khẩu thô để autofill; tránh lưu hash vào textbox)
                                if (cbghinho.Checked)
                                {
                                    Properties.Settings.Default.SavedUser = username;
                                    Properties.Settings.Default.SavedPass = password;
                                    Properties.Settings.Default.Save();
                                }
                                else
                                {
                                    Properties.Settings.Default.SavedUser = "";
                                    Properties.Settings.Default.SavedPass = "";
                                    Properties.Settings.Default.Save();
                                }

                                MessageBox.Show("Đăng nhập thành công!", "Thành công",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                this.Hide();

                                // PHÂN QUYỀN
                                if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                                {
                                    DashBoard main = new DashBoard(role, username, maTV);
                                    main.FormClosed += (s, args) => Application.Exit();
                                    main.Show();
                                }
                                else if (role.Equals("Thành viên", StringComparison.OrdinalIgnoreCase) ||
                                         role.Equals("Quản trị viên", StringComparison.OrdinalIgnoreCase))
                                {
                                    MemberDashboard memberForm = new MemberDashboard(role, username, maTV);
                                    memberForm.FormClosed += (s, args) => Application.Exit();
                                    memberForm.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Quyền hạn không hợp lệ!", "Lỗi",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Show();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
          

    

            private static string HashPassword(string password)
            {
                using (var sha = SHA256.Create())
                {
                    var bytes = Encoding.UTF8.GetBytes(password ?? "");
                    var hash = sha.ComputeHash(bytes);
                    var sb = new StringBuilder();
                    foreach (var b in hash)
                        sb.Append(b.ToString("x2"));
                    return sb.ToString();
                }
            }
                
            // helper: kiểm tra chuỗi có phải hex SHA256 (64 ký tự) hay không
            private bool IsHex(string text)
            {
                if (string.IsNullOrEmpty(text) || text.Length != 64)
                    return false;
                foreach (char c in text)
                {
                    bool ok = (c >= '0' && c <= '9') ||
                              (c >= 'a' && c <= 'f') ||

                              (c >= 'A' && c <= 'F');
                    if (!ok) return false;
                }
                return true;
            }

            private void Lbdangky_Click(object sender, EventArgs e)
            {
                this.Hide();
            FormSignUp formDangKy = new FormSignUp();
                formDangKy.FormClosed += (s, args) => this.Show();
                formDangKy.Show();
            }

            protected override void OnFormClosing(FormClosingEventArgs e)
            {
                base.OnFormClosing(e);
                Application.Exit();
            }

        private void ptLogo2_Click(object sender, EventArgs e)
        {

        }
    }
    }
