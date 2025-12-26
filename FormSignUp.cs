using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class FormSignUp : Form
    {
        // Chuỗi kết nối database
        private string connectionString = @"Data Source=21AK22-COM;Initial Catalog=QL_CLB_LSC;Persist Security Info=True;User ID=sa;Password=912005;Encrypt=True;TrustServerCertificate=True";

        public FormSignUp()
        {
            InitializeComponent();
            LoadGioiTinh();
            SetupFormValidation();
        }

        // Load giá trị cho ComboBox Giới tính
        private void LoadGioiTinh()
        {
            cboGioiTinh.Items.Clear();
            cboGioiTinh.Items.Add("Nam");
            cboGioiTinh.Items.Add("Nữ");
            cboGioiTinh.Items.Add("Khác");
            cboGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // Thiết lập validation cho form
        private void SetupFormValidation()
        {
            // Validation cho Email
            txtEmail.Leave += (s, e) => {
                if (!string.IsNullOrEmpty(txtEmail.Text) && !IsValidEmail(txtEmail.Text))
                {
                    MessageBox.Show("Email không hợp lệ! Vui lòng nhập đúng định dạng email.",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                }
            };

            // Validation cho SĐT - chỉ cho phép nhập số
            txtSDT.KeyPress += (s, e) => {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            // Giới hạn độ dài SĐT
            txtSDT.MaxLength = 15;
        }

        // Kiểm tra email hợp lệ
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && email.Contains("@") && email.Contains(".");
            }
            catch
            {
                return false;
            }
        }

        // Kiểm tra email đã tồn tại
        private bool CheckEmailExists(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM ThanhVien WHERE Email = @Email";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", email);

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra email: " + ex.Message);
                return true;
            }
        }

        // THÊM MỚI: Kiểm tra họ tên đã tồn tại (không phân biệt hoa thường)
        private bool CheckHoTenExists(string hoTen)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Sử dụng COLLATE để không phân biệt hoa thường khi so sánh
                    string query = "SELECT COUNT(*) FROM ThanhVien WHERE HoTen COLLATE Latin1_General_CI_AS = @HoTen COLLATE Latin1_General_CI_AS";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@HoTen", hoTen.Trim());

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra họ tên: " + ex.Message);
                return true;
            }
        }

        // THÊM MỚI: Kiểm tra tên đăng nhập đã tồn tại (PHÂN BIỆT hoa thường)
        private bool CheckTenDangNhapExists(string tenDangNhap)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Sử dụng COLLATE để PHÂN BIỆT hoa thường khi so sánh
                    string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap COLLATE Latin1_General_CS_AS = @TenDangNhap COLLATE Latin1_General_CS_AS";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap.Trim());

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra tên đăng nhập: " + ex.Message);
                return true;
            }
        }

        // Nút Đăng ký
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            // 1. Validate Họ tên
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            if (txtHoTen.Text.Trim().Length < 2)
            {
                MessageBox.Show("Họ tên phải có ít nhất 2 ký tự!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            // THÊM MỚI: Kiểm tra họ tên đã tồn tại
            if (CheckHoTenExists(txtHoTen.Text.Trim()))
            {
                MessageBox.Show("Họ tên này đã tồn tại trong hệ thống!\n\n" +
                    "Vui lòng sử dụng họ tên khác hoặc thêm thông tin phân biệt\n" +
                    "(Ví dụ: Nguyễn Văn A - K21, Nguyễn Văn A - CNTT)",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            // 2. Validate Ngày sinh
            if (dtpNgaySinh.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Ngày sinh không được lớn hơn ngày hiện tại!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgaySinh.Focus();
                return;
            }

            int tuoi = DateTime.Now.Year - dtpNgaySinh.Value.Year;
            if (tuoi < 16 || tuoi > 100)
            {
                MessageBox.Show("Tuổi không hợp lệ! (Từ 16 đến 100 tuổi)", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgaySinh.Focus();
                return;
            }

            // 3. Validate Giới tính
            if (cboGioiTinh.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giới tính!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboGioiTinh.Focus();
                return;
            }

            // 4. Validate Lớp
            if (string.IsNullOrWhiteSpace(txtLop.Text))
            {
                MessageBox.Show("Vui lòng nhập lớp!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLop.Focus();
                return;
            }

            // 5. Validate Khoa
            if (string.IsNullOrWhiteSpace(txtKhoa.Text))
            {
                MessageBox.Show("Vui lòng nhập khoa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKhoa.Focus();
                return;
            }

            // 6. Validate SĐT
            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            if (txtSDT.Text.Length < 9 || txtSDT.Text.Length > 15)
            {
                MessageBox.Show("Số điện thoại phải có từ 9 đến 15 chữ số!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            // 7. Validate Email
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ! Vui lòng nhập đúng định dạng email.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            // 8. Kiểm tra email đã tồn tại
            if (CheckEmailExists(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Email này đã được đăng ký! Vui lòng sử dụng email khác.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            // Thực hiện đăng ký
            RegisterMember();
        }

        // Đăng ký thành viên
        private void RegisterMember()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Thêm thành viên mới và lấy MaTV vừa tạo
                    string insertQuery = @"INSERT INTO ThanhVien 
                        (HoTen, NgaySinh, GioiTinh, Lop, Khoa, SDT, Email, VaiTro, TrangThai, NgayThamGia)
                        VALUES 
                        (@HoTen, @NgaySinh, @GioiTinh, @Lop, @Khoa, @SDT, @Email, N'Thành viên', N'Hoạt động', GETDATE());
                        SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(insertQuery, conn);

                    cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                    cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value.Date);
                    cmd.Parameters.AddWithValue("@GioiTinh", cboGioiTinh.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Lop", txtLop.Text.Trim());
                    cmd.Parameters.AddWithValue("@Khoa", txtKhoa.Text.Trim());
                    cmd.Parameters.AddWithValue("@SDT", txtSDT.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                    // Lấy MaTV vừa tạo
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int maTV = Convert.ToInt32(result);
                        string hoTen = txtHoTen.Text.Trim();
                        string email = txtEmail.Text.Trim();

                        MessageBox.Show(
                            "Đăng ký thông tin thành công!\n\n" +
                            "Bước tiếp theo: Tạo tài khoản đăng nhập.",
                            "Thành công",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // Ẩn form hiện tại
                        this.Hide();

                        // Mở form SignIn để tạo tài khoản
                        SignUp formSignIn = new SignUp(maTV, hoTen, email);

                        if (formSignIn.ShowDialog() == DialogResult.OK)
                        {
                            // Nếu tạo tài khoản thành công
                            MessageBox.Show(
                                "Hoàn tất đăng ký!\n\n" +
                                "Bạn có thể đăng nhập vào hệ thống.",
                                "Thành công",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            // Nếu hủy tạo tài khoản
                            this.Show();

                            DialogResult retry = MessageBox.Show(
                                "Bạn đã hủy tạo tài khoản.\n\n" +
                                "Thông tin cá nhân đã được lưu với Mã TV: " + maTV + "\n" +
                                "Bạn có muốn thử tạo tài khoản lại không?",
                                "Thông báo",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            if (retry == DialogResult.Yes)
                            {
                                this.Hide();
                                SignUp retryForm = new SignUp(maTV, hoTen, email);

                                if (retryForm.ShowDialog() == DialogResult.OK)
                                {
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                                else
                                {
                                    this.Show();
                                }
                            }
                            else
                            {
                                ClearForm();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Đăng ký thất bại! Vui lòng thử lại.",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 2627 || sqlEx.Number == 2601) // Lỗi duplicate key
                {
                    MessageBox.Show("Dữ liệu này đã tồn tại trong hệ thống!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Lỗi SQL: " + sqlEx.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng ký: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa dữ liệu form
        private void ClearForm()
        {
            txtHoTen.Clear();
            dtpNgaySinh.Value = DateTime.Now.AddYears(-18);
            cboGioiTinh.SelectedIndex = -1;
            txtLop.Clear();
            txtKhoa.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            txtHoTen.Focus();
        }

        // Nút Hủy hoặc đóng form
        private void btnHuy_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn hủy đăng ký?\nDữ liệu đã nhập sẽ không được lưu.",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        // Xử lý khi form đóng
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Nếu chưa lưu dữ liệu và có dữ liệu trong form
            if (this.DialogResult != DialogResult.OK &&
                !string.IsNullOrEmpty(txtHoTen.Text))
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn thoát?\nDữ liệu đã nhập sẽ không được lưu.",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void FormSignUp_Load(object sender, EventArgs e)
        {

        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            try
            {
                // If there's an owner form set, return to it.
                if (this.Owner != null)
                {
                    this.Owner.Show();
                    this.Close();
                    return;
                }

                // Otherwise try to find any other open form and show it.
                foreach (Form openForm in Application.OpenForms)
                {
                    if (openForm != this)
                    {
                        openForm.Show();
                        openForm.BringToFront();
                        this.Close();
                        return;
                    }
                }

                // If no other form is available, just close this form.
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi quay lại: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}