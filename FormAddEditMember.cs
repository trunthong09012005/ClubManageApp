using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ClubManageApp
{
    public class FormAddEditMember : Form
    {
        #region Fields

        private string connectionString;
        private int? maTV; // null for add, value for edit
        private string currentUserRole; // Role của người đăng nhập
        private string originalRole; // ✅ THÊM: Vai trò gốc của thành viên (khi edit)
        
        private TextBox txtHoTen, txtEmail, txtSDT, txtDiaChi, txtLop, txtKhoa;
        private DateTimePicker dtpNgaySinh;
        private ComboBox cboGioiTinh, cboVaiTro, cboTrangThai, cboChucVu, cboBan;
        private Button btnSave, btnCancel;

        #endregion

        #region Constructor

        public FormAddEditMember(string connString, string userRole, int? memberID = null)
        {
            this.connectionString = connString;
            this.currentUserRole = userRole;
            this.maTV = memberID;
            InitializeForm();
            
            if (maTV.HasValue)
                LoadMemberData();
        }

        #endregion

        #region Form Initialization

        private void InitializeForm()
        {
            this.Text = maTV.HasValue ? "Sửa thông tin thành viên" : "Thêm thành viên mới";
            this.Size = new Size(600, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(245, 247, 250);

            int yPos = 20;

            AddFormField("Họ tên (*)", ref txtHoTen, ref yPos);
            AddFormField("Email (*)", ref txtEmail, ref yPos);
            AddFormField("SĐT (*)", ref txtSDT, ref yPos);
            
            Label lblNgaySinh = new Label 
            { 
                Text = "Ngày sinh (*)", 
                Location = new Point(20, yPos), 
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            dtpNgaySinh = new DateTimePicker 
            { 
                Location = new Point(150, yPos), 
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Short,
                MaxDate = DateTime.Now.AddYears(-18), // Ít nhất 18 tuổi
                MinDate = DateTime.Now.AddYears(-30)  // Tối đa 30 tuổi
            };
            this.Controls.Add(lblNgaySinh);
            this.Controls.Add(dtpNgaySinh);
            yPos += 40;

            AddComboField("Giới tính (*)", ref cboGioiTinh, new[] { "Nam", "Nữ", "Khác" }, ref yPos);
            AddFormField("Lớp (*)", ref txtLop, ref yPos);
            AddFormField("Khoa (*)", ref txtKhoa, ref yPos);
            AddFormField("Địa chỉ (*)", ref txtDiaChi, ref yPos);
            
            // ✅ Load vai trò - LOẠI TRỪ Admin
            LoadAndAddComboField("Vai trò (*)", ref cboVaiTro, "SELECT DISTINCT VaiTro FROM ThanhVien WHERE VaiTro IS NOT NULL AND VaiTro != N'Admin'", ref yPos);
            
            AddComboField("Trạng thái (*)", ref cboTrangThai, new[] { "Hoạt động", "Nghỉ" }, ref yPos);
            LoadAndAddComboField("Chức vụ", ref cboChucVu, "SELECT MaCV, TenCV FROM ChucVu", ref yPos, true);
            LoadAndAddComboField("Ban", ref cboBan, "SELECT MaBan, TenBan FROM BanChuyenMon", ref yPos, true);

            // Buttons
            Panel pnlButtons = new Panel
            {
                Location = new Point(0, yPos + 10),
                Size = new Size(600, 60),
                BackColor = Color.White
            };

            btnSave = new Button
            {
                Text = "💾 Lưu",  
                Location = new Point(170, 10),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "❌ Hủy",
                Location = new Point(310, 10),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            pnlButtons.Controls.Add(btnSave);
            pnlButtons.Controls.Add(btnCancel);
            this.Controls.Add(pnlButtons);

            // Add validation event handlers
            txtHoTen.Leave += TxtHoTen_Leave;
            txtEmail.Leave += TxtEmail_Leave;
            txtSDT.Leave += TxtSDT_Leave;
            txtSDT.KeyPress += TxtSDT_KeyPress;
            
            // Reset màu nền khi người dùng bắt đầu nhập
            txtHoTen.TextChanged += (s, e) => { if (txtHoTen.BackColor != Color.White) txtHoTen.BackColor = Color.White; };
            txtEmail.TextChanged += (s, e) => { if (txtEmail.BackColor != Color.White) txtEmail.BackColor = Color.White; };
            txtSDT.TextChanged += (s, e) => { if (txtSDT.BackColor != Color.White) txtSDT.BackColor = Color.White; };
            txtLop.TextChanged += (s, e) => { if (txtLop.BackColor != Color.White) txtLop.BackColor = Color.White; };
            txtKhoa.TextChanged += (s, e) => { if (txtKhoa.BackColor != Color.White) txtKhoa.BackColor = Color.White; };
            txtDiaChi.TextChanged += (s, e) => { if (txtDiaChi.BackColor != Color.White) txtDiaChi.BackColor = Color.White; };
            cboGioiTinh.SelectedIndexChanged += (s, e) => { if (cboGioiTinh.BackColor != Color.White) cboGioiTinh.BackColor = Color.White; };
            cboVaiTro.SelectedIndexChanged += (s, e) => { if (cboVaiTro.BackColor != Color.White) cboVaiTro.BackColor = Color.White; };
            cboTrangThai.SelectedIndexChanged += (s, e) => { if (cboTrangThai.BackColor != Color.White) cboTrangThai.BackColor = Color.White; };
        }

        private void AddFormField(string label, ref TextBox textBox, ref int yPos)
        {
            Label lbl = new Label 
            { 
                Text = label, 
                Location = new Point(20, yPos), 
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            textBox = new TextBox 
            { 
                Location = new Point(150, yPos), 
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lbl);
            this.Controls.Add(textBox);
            yPos += 40;
        }

        private void AddComboField(string label, ref ComboBox combo, string[] items, ref int yPos)
        {
            Label lbl = new Label 
            { 
                Text = label, 
                Location = new Point(20, yPos), 
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            combo = new ComboBox 
            { 
                Location = new Point(150, yPos), 
                Size = new Size(400, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            combo.Items.AddRange(items);
            this.Controls.Add(lbl);
            this.Controls.Add(combo);
            yPos += 40;
        }

        private void LoadAndAddComboField(string label, ref ComboBox combo, string query, ref int yPos, bool hasID = false)
        {
            Label lbl = new Label 
            { 
                Text = label, 
                Location = new Point(20, yPos), 
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            combo = new ComboBox 
            { 
                Location = new Point(150, yPos), 
                Size = new Size(400, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (hasID)
                            {
                                combo.Items.Add(new ComboBoxItem
                                {
                                    Value = reader.GetInt32(0),
                                    Text = reader.GetString(1)
                                });
                            }
                            else
                            {
                                combo.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu ComboBox: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (hasID)
            {
                combo.DisplayMember = "Text";
                combo.ValueMember = "Value";
            }

            this.Controls.Add(lbl);
            this.Controls.Add(combo);
            yPos += 40;
        }

        #endregion

        #region Data Loading

        private void LoadMemberData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM ThanhVien WHERE MaTV = @MaTV";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTV", maTV.Value);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtHoTen.Text = reader["HoTen"].ToString();
                                txtEmail.Text = reader["Email"].ToString();
                                txtSDT.Text = reader["SDT"]?.ToString() ?? "";
                                txtDiaChi.Text = reader["DiaChi"]?.ToString() ?? "";
                                txtLop.Text = reader["Lop"]?.ToString() ?? "";
                                txtKhoa.Text = reader["Khoa"]?.ToString() ?? "";
                                
                                if (reader["NgaySinh"] != DBNull.Value)
                                {
                                    DateTime ngaySinh = Convert.ToDateTime(reader["NgaySinh"]);
                                    if (ngaySinh <= dtpNgaySinh.MaxDate && ngaySinh >= dtpNgaySinh.MinDate)
                                        dtpNgaySinh.Value = ngaySinh;
                                }
                                
                                cboGioiTinh.SelectedItem = reader["GioiTinh"]?.ToString();
                                
                                // ✅ LUU VAI TRÒ GỐC
                                originalRole = reader["VaiTro"]?.ToString();
                                
                                // Load vai trò
                                if (!string.IsNullOrEmpty(originalRole))
                                {
                                    if (string.Equals(originalRole, "Admin", StringComparison.OrdinalIgnoreCase))
                                    {
                                        // ✅ Nếu là Admin, thêm vào dropdown và disable
                                        if (!cboVaiTro.Items.Contains(originalRole))
                                            cboVaiTro.Items.Add(originalRole);
                                        cboVaiTro.SelectedItem = originalRole;
                                        cboVaiTro.Enabled = false;
                                        
                                        Label lblWarning = new Label
                                        {
                                            Text = "⚠️ Không thể thay đổi vai trò Admin",
                                            Location = new Point(150, cboVaiTro.Location.Y + 30),
                                            AutoSize = true,
                                            ForeColor = Color.Red,
                                            Font = new Font("Segoe UI", 9, FontStyle.Italic)
                                        };
                                        this.Controls.Add(lblWarning);
                                    }
                                    else
                                    {
                                        cboVaiTro.SelectedItem = originalRole;
                                    }
                                }
                                
                                cboTrangThai.SelectedItem = reader["TrangThai"]?.ToString();
                                
                                if (reader["MaCV"] != DBNull.Value)
                                {
                                    int maCV = Convert.ToInt32(reader["MaCV"]);
                                    foreach (ComboBoxItem item in cboChucVu.Items)
                                    {
                                        if (item.Value == maCV)
                                        {
                                            cboChucVu.SelectedItem = item;
                                            break;
                                        }
                                    }
                                }
                                
                                if (reader["MaBan"] != DBNull.Value)
                                {
                                    int maBan = Convert.ToInt32(reader["MaBan"]);
                                    foreach (ComboBoxItem item in cboBan.Items)
                                    {
                                        if (item.Value == maBan)
                                        {
                                            cboBan.SelectedItem = item;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Validation Event Handlers

        private void TxtHoTen_Leave(object sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            if (string.IsNullOrWhiteSpace(hoTen))
            {
                txtHoTen.BackColor = Color.FromArgb(255, 230, 230);
                return;
            }

            // Kiểm tra tên chỉ chứa chữ cái và khoảng trắng
            if (!Regex.IsMatch(hoTen, @"^[\p{L}\s]+$"))
            {
                txtHoTen.BackColor = Color.FromArgb(255, 230, 230);
                MessageBox.Show("Họ tên chỉ được chứa chữ cái và khoảng trống!", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            // Kiểm tra độ dài
            if (hoTen.Length < 2 || hoTen.Length > 150)
            {
                txtHoTen.BackColor = Color.FromArgb(255, 230, 230);
                MessageBox.Show("Họ tên phải có độ dài từ 2 đến 150 ký tự!", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            txtHoTen.BackColor = Color.White;
        }

        private void TxtEmail_Leave(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                txtEmail.BackColor = Color.FromArgb(255, 230, 230);
                return;
            }

            if (!IsValidEmail(email))
            {
                txtEmail.BackColor = Color.FromArgb(255, 230, 230);
                MessageBox.Show("Email không hợp lệ! Vui lòng nhập đúng định dạng email.", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            // Kiểm tra email đã tồn tại (nếu là thêm mới hoặc email khác với email cũ)
            if (IsEmailExists(email))
            {
                txtEmail.BackColor = Color.FromArgb(255, 230, 230);
                MessageBox.Show("Email này đã được sử dụng bởi thành viên khác!", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            txtEmail.BackColor = Color.White;
        }

        private void TxtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số và các phím điều khiển
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtSDT_Leave(object sender, EventArgs e)
        {
            string sdt = txtSDT.Text.Trim();
            if (string.IsNullOrWhiteSpace(sdt))
            {
                txtSDT.BackColor = Color.White;
                return;
            }

            // Kiểm tra định dạng số điện thoại Việt Nam
            if (!Regex.IsMatch(sdt, @"^(0|\+84)[0-9]{9,10}$"))
            {
                txtSDT.BackColor = Color.FromArgb(255, 230, 230);
                MessageBox.Show("Số điện thoại không hợp lệ!\nĐịnh dạng: 0xxxxxxxxx hoặc +84xxxxxxxxx", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            txtSDT.BackColor = Color.White;
        }

        #endregion

        #region Event Handlers

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // ✅ KIỂM TRA TOÀN DIỆN
            var validationResult = ValidateAllFields();
            if (!validationResult.IsValid)
            {
                MessageBox.Show(validationResult.ErrorMessage, "Lỗi Validation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedRole = cboVaiTro.SelectedItem?.ToString();

            // ✅ Thực hiện lưu
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    string query;
                    
                    if (maTV.HasValue)
                    {
                        query = @"UPDATE ThanhVien SET 
                            HoTen = @HoTen, Email = @Email, SDT = @SDT, DiaChi = @DiaChi,
                            NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, Lop = @Lop, Khoa = @Khoa,
                            VaiTro = @VaiTro, TrangThai = @TrangThai, MaCV = @MaCV, MaBan = @MaBan
                            WHERE MaTV = @MaTV";
                    }
                    else
                    {
                        query = @"INSERT INTO ThanhVien 
                            (HoTen, Email, SDT, DiaChi, NgaySinh, GioiTinh, Lop, Khoa, VaiTro, TrangThai, MaCV, MaBan, NgayThamGia)
                            VALUES 
                            (@HoTen, @Email, @SDT, @DiaChi, @NgaySinh, @GioiTinh, @Lop, @Khoa, @VaiTro, @TrangThai, @MaCV, @MaBan, GETDATE())";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (maTV.HasValue)
                            cmd.Parameters.AddWithValue("@MaTV", maTV.Value);

                        cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim().ToLower());
                        cmd.Parameters.AddWithValue("@SDT", string.IsNullOrWhiteSpace(txtSDT.Text) ? (object)DBNull.Value : txtSDT.Text.Trim());
                        cmd.Parameters.AddWithValue("@DiaChi", string.IsNullOrWhiteSpace(txtDiaChi.Text) ? (object)DBNull.Value : txtDiaChi.Text.Trim());
                        cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value);
                        cmd.Parameters.AddWithValue("@GioiTinh", cboGioiTinh.SelectedItem?.ToString() ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Lop", string.IsNullOrWhiteSpace(txtLop.Text) ? (object)DBNull.Value : txtLop.Text.Trim());
                        cmd.Parameters.AddWithValue("@Khoa", string.IsNullOrWhiteSpace(txtKhoa.Text) ? (object)DBNull.Value : txtKhoa.Text.Trim());
                        cmd.Parameters.AddWithValue("@VaiTro", string.IsNullOrWhiteSpace(selectedRole) ? (object)DBNull.Value : selectedRole);
                        cmd.Parameters.AddWithValue("@TrangThai", cboTrangThai.SelectedItem?.ToString() ?? "Hoạt động");
                        cmd.Parameters.AddWithValue("@MaCV", cboChucVu.SelectedItem != null ? ((ComboBoxItem)cboChucVu.SelectedItem).Value : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaBan", cboBan.SelectedItem != null ? ((ComboBoxItem)cboBan.SelectedItem).Value : (object)DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }

                string successMessage = maTV.HasValue ? "✅ Cập nhật thành công!" : "✅ Thêm thành viên thành công!";
                
                MessageBox.Show(successMessage, "Thành công", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                {
                    MessageBox.Show("❌ Email đã tồn tại trong hệ thống!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (sqlEx.Number == 547)
                {
                    MessageBox.Show("❌ Dữ liệu tham chiếu không hợp lệ! Vui lòng kiểm tra lại.", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"❌ Lỗi SQL: {sqlEx.Message}", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi khi lưu: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Validation Methods

        private ValidationResult ValidateAllFields()
        {
            // Reset màu nền tất cả các trường trước khi validate
            ResetFieldColors();
            
            System.Text.StringBuilder errors = new System.Text.StringBuilder();
            bool hasError = false;
            
            // Validate Họ tên
            string hoTen = txtHoTen.Text.Trim();
            if (string.IsNullOrWhiteSpace(hoTen))
            {
                txtHoTen.BackColor = Color.FromArgb(255, 230, 230);
                errors.AppendLine("• Họ tên không được để trống");
                hasError = true;
            }
            else if (!Regex.IsMatch(hoTen, @"^[\p{L}\s]+$"))
            {
                txtHoTen.BackColor = Color.FromArgb(255, 230, 230);
                errors.AppendLine("• Họ tên chỉ được chứa chữ cái và khoảng trắng");
                hasError = true;
            }
            else if (hoTen.Length < 2 || hoTen.Length > 150)
            {
                txtHoTen.BackColor = Color.FromArgb(255, 230, 230);
                errors.AppendLine("• Họ tên phải có độ dài từ 2 đến 150 ký tự");
                hasError = true;
            }

            // Validate Email
            string email = txtEmail.Text.Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                txtEmail.BackColor = Color.FromArgb(255, 230, 230);
                errors.AppendLine("• Email không được để trống");
                hasError = true;
            }
            else if (!IsValidEmail(email))
            {
                txtEmail.BackColor = Color.FromArgb(255, 230, 230);
                errors.AppendLine("• Email không hợp lệ");
                hasError = true;
            }
            else if (IsEmailExists(email))
            {
                txtEmail.BackColor = Color.FromArgb(255, 230, 230);
                errors.AppendLine("• Email đã được sử dụng bởi thành viên khác");
                hasError = true;
            }

            // Validate SĐT (bắt buộc)
            string sdt = txtSDT.Text.Trim();
            if (string.IsNullOrWhiteSpace(sdt))
            {
                txtSDT.BackColor = Color.FromArgb(255, 230, 230);
                errors.AppendLine("• Số điện thoại không được để trống");
                hasError = true;
            }
            else if (!Regex.IsMatch(sdt, @"^(0|\+84)[0-9]{9,10}$"))
            {
                txtSDT.BackColor = Color.FromArgb(255, 230, 230);
                errors.AppendLine("• Số điện thoại không hợp lệ (định dạng: 0xxxxxxxxx hoặc +84xxxxxxxxx)");
                hasError = true;
            }
            else if (IsPhoneExists(sdt))
            {
                txtSDT.BackColor = Color.FromArgb(255, 230, 230);
                errors.AppendLine("• Số điện thoại đã được sử dụng bởi thành viên khác");
                hasError = true;
            }
            
            // Validate Lớp (bắt buộc)
            string lop = txtLop.Text.Trim();
            if (string.IsNullOrWhiteSpace(lop))
            {
                txtLop.BackColor = Color.FromArgb(255, 230, 230);
                errors.AppendLine("• Lớp không được để trống");
                hasError = true;
            }
            
            // Validate Khoa (bắt buộc)
            string khoa = txtKhoa.Text.Trim();
            if (string.IsNullOrWhiteSpace(khoa))
            {
                txtKhoa.BackColor = Color.FromArgb(255, 230, 230);
                errors.AppendLine("• Khoa không được để trống");
                hasError = true;
            }
            
            // Validate Địa chỉ (bắt buộc)
            string diaChi = txtDiaChi.Text.Trim();
            if (string.IsNullOrWhiteSpace(diaChi))
            {
                txtDiaChi.BackColor = Color.FromArgb(255, 230, 230);
                errors.AppendLine("• Địa chỉ không được để trống");
                hasError = true;
            }

            // Validate Ngày sinh
            int age = DateTime.Now.Year - dtpNgaySinh.Value.Year;
            if (DateTime.Now.DayOfYear < dtpNgaySinh.Value.DayOfYear)
                age--;
                
            if (age < 18)
            {
                errors.AppendLine("• Thành viên phải ít nhất 18 tuổi");
                hasError = true;
            }
            else if (age > 30)
            {
                errors.AppendLine("• Thành viên không được quá 30 tuổi");
                hasError = true;
            }
            
            // Validate giới tính
            if (cboGioiTinh.SelectedIndex < 0)
            {
                cboGioiTinh.BackColor = Color.FromArgb(255, 230, 230);
                errors.AppendLine("• Vui lòng chọn giới tính");
                hasError = true;
            }
            
            // Validate vai trò
            if (cboVaiTro.SelectedIndex < 0)
            {
                cboVaiTro.BackColor = Color.FromArgb(255, 230, 230);
                errors.AppendLine("• Vui lòng chọn vai trò");
                hasError = true;
            }
            
            // Validate trạng thái
            if (cboTrangThai.SelectedIndex < 0)
            {
                cboTrangThai.BackColor = Color.FromArgb(255, 230, 230);
                errors.AppendLine("• Vui lòng chọn trạng thái");
                hasError = true;
            }

            if (hasError)
            {
                string errorMessage = "❌ Vui lòng điền đầy đủ và chính xác các thông tin:\n\n" + errors.ToString();
                return new ValidationResult(false, errorMessage);
            }

            return new ValidationResult(true, "");
        }
        
        private void ResetFieldColors()
        {
            txtHoTen.BackColor = Color.White;
            txtEmail.BackColor = Color.White;
            txtSDT.BackColor = Color.White;
            txtLop.BackColor = Color.White;
            txtKhoa.BackColor = Color.White;
            txtDiaChi.BackColor = Color.White;
            cboGioiTinh.BackColor = Color.White;
            cboVaiTro.BackColor = Color.White;
            cboTrangThai.BackColor = Color.White;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            }
            catch
            {
                return false;
            }
        }

        private bool IsEmailExists(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = maTV.HasValue 
                        ? "SELECT COUNT(*) FROM ThanhVien WHERE Email = @Email AND MaTV != @MaTV"
                        : "SELECT COUNT(*) FROM ThanhVien WHERE Email = @Email";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email.ToLower());
                        if (maTV.HasValue)
                            cmd.Parameters.AddWithValue("@MaTV", maTV.Value);

                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        
        // ✅ THÊM: Kiểm tra SĐT đã tồn tại
        private bool IsPhoneExists(string phone)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = maTV.HasValue 
                        ? "SELECT COUNT(*) FROM ThanhVien WHERE SDT = @SDT AND MaTV != @MaTV"
                        : "SELECT COUNT(*) FROM ThanhVien WHERE SDT = @SDT";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SDT", phone);
                        if (maTV.HasValue)
                            cmd.Parameters.AddWithValue("@MaTV", maTV.Value);

                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Helper Classes

        public class ComboBoxItem
        {
            public int Value { get; set; }
            public string Text { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private class ValidationResult
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }

            public ValidationResult(bool isValid, string errorMessage)
            {
                IsValid = isValid;
                ErrorMessage = errorMessage;
            }
        }

        #endregion
    }
}
