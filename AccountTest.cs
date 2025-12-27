using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class ucAccountTest : UserControl
    {
        private string connectionString = ConnectionHelper.ConnectionString;
        private DataTable logTable;

        public ucAccountTest()
        {
            InitializeComponent();
            InitializeLogTable();
            
            // Add event handler to txtMaTV to display member info when MaTV is entered
            txtMaTV.TextChanged += TxtMaTV_TextChanged;
        }

        private void TxtMaTV_TextChanged(object sender, EventArgs e)
        {
            string maTVText = txtMaTV.Text.Trim();
            
            // Clear textboxes if empty
            if (string.IsNullOrEmpty(maTVText))
            {
                txtHoTen.Clear();
                txtSDT.Clear();
                txtHoTen.ForeColor = System.Drawing.Color.Black;
                txtSDT.ForeColor = System.Drawing.Color.Black;
                return;
            }

            // Try to parse MaTV
            if (int.TryParse(maTVText, out int maTV))
            {
                // Get member info from database
                var memberInfo = GetMemberInfo(maTV);
                if (memberInfo != null)
                {
                    // Member exists - auto-fill information
                    txtHoTen.Text = memberInfo.HoTen;
                    txtSDT.Text = memberInfo.SDT;
                    txtHoTen.ForeColor = System.Drawing.Color.DarkGreen;
                    txtSDT.ForeColor = System.Drawing.Color.DarkBlue;
                }
                else
                {
                    // Member doesn't exist - clear fields for manual input
                    txtHoTen.Clear();
                    txtSDT.Clear();
                    txtHoTen.ForeColor = System.Drawing.Color.Black;
                    txtSDT.ForeColor = System.Drawing.Color.Black;
                }
            }
            else
            {
                txtHoTen.Clear();
                txtSDT.Clear();
                txtHoTen.ForeColor = System.Drawing.Color.Black;
                txtSDT.ForeColor = System.Drawing.Color.Black;
            }
        }

        private AccountMemberInfo GetMemberInfo(int maTV)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("SELECT HoTen, SDT FROM ThanhVien WHERE MaTV = @MaTV", conn))
                {
                    cmd.Parameters.AddWithValue("@MaTV", maTV);
                    conn.Open();
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new AccountMemberInfo
                            {
                                HoTen = reader["HoTen"]?.ToString() ?? "",
                                SDT = reader["SDT"]?.ToString() ?? ""
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting member info: {ex.Message}");
            }
            
            return null;
        }

        private void InitializeLogTable()
        {
            logTable = new DataTable();
            logTable.Columns.Add("Thời gian", typeof(string));
            logTable.Columns.Add("Tác vụ", typeof(string));
            dgvLog.DataSource = logTable;
        }

        private void AddLog(string action)
        {
            DataRow row = logTable.NewRow();
            row["Thời gian"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            row["Tác vụ"] = action;
            logTable.Rows.InsertAt(row, 0); // Insert at top
            
            // Limit log to 100 entries
            while (logTable.Rows.Count > 100)
            {
                logTable.Rows.RemoveAt(logTable.Rows.Count - 1);
            }
        }

        private T GetControl<T>(string name) where T : Control
        {
            var arr = this.Controls.Find(name, true);
            if (arr != null && arr.Length > 0) return arr[0] as T;
            return null;
        }

        private void ucAccountTest_Load(object sender, EventArgs e)
        {
            PopulateQuyenHan();
            PopulateTrangThai();
            LoadTaiKhoan();
            UpdateStatistics();
            AddLog("Khởi động module quản lý tài khoản");
        }

        private void PopulateQuyenHan()
        {
            cboQuyenHan.Items.Clear();
            cboQuyenHan.Items.Add("Admin");
            cboQuyenHan.Items.Add("Quản trị viên");
            cboQuyenHan.Items.Add("Thành viên");
        }

        private void PopulateTrangThai()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Hoạt động");
            cboTrangThai.Items.Add("Khóa");
            cboTrangThai.Items.Add("Chờ kích hoạt");
        }

        private void LoadTaiKhoan(string filter = null)
        {
            try
            {
                dgvTaiKhoan.DataSource = null;

                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;

                    string sql = @"SELECT TK.MaTK, TK.TenDN, TK.MatKhau, TK.MaTV, TV.HoTen, TV.SDT,
                                   TK.QuyenHan, TK.NgayTao, TK.LanDangNhapCuoi, TK.TrangThai 
                                   FROM TaiKhoan TK
                                   LEFT JOIN ThanhVien TV ON TK.MaTV = TV.MaTV";

                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        sql += " WHERE TK.TenDN LIKE @f OR TV.HoTen LIKE @f OR TK.QuyenHan LIKE @f OR TK.TrangThai LIKE @f";
                        cmd.Parameters.AddWithValue("@f", "%" + filter + "%");
                    }

                    cmd.CommandText = sql;
                    conn.Open();

                    var da = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        dgvTaiKhoan.DataSource = dt;
                        
                        // Customize column headers
                        if (dgvTaiKhoan.Columns.Contains("MaTK")) dgvTaiKhoan.Columns["MaTK"].HeaderText = "Mã TK";
                        if (dgvTaiKhoan.Columns.Contains("TenDN")) dgvTaiKhoan.Columns["TenDN"].HeaderText = "Tên đăng nhập";
                        if (dgvTaiKhoan.Columns.Contains("MatKhau"))
                        {
                            dgvTaiKhoan.Columns["MatKhau"].HeaderText = "Mật khẩu";
                            dgvTaiKhoan.Columns["MatKhau"].Visible = false; // Hide password column
                        }
                        if (dgvTaiKhoan.Columns.Contains("MaTV")) dgvTaiKhoan.Columns["MaTV"].HeaderText = "Mã TV";
                        if (dgvTaiKhoan.Columns.Contains("HoTen")) dgvTaiKhoan.Columns["HoTen"].HeaderText = "Họ tên";
                        if (dgvTaiKhoan.Columns.Contains("SDT")) dgvTaiKhoan.Columns["SDT"].HeaderText = "Số điện thoại";
                        if (dgvTaiKhoan.Columns.Contains("QuyenHan")) dgvTaiKhoan.Columns["QuyenHan"].HeaderText = "Quyền hạn";
                        if (dgvTaiKhoan.Columns.Contains("NgayTao")) 
                        {
                            dgvTaiKhoan.Columns["NgayTao"].HeaderText = "Ngày tạo";
                            dgvTaiKhoan.Columns["NgayTao"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                        }
                        if (dgvTaiKhoan.Columns.Contains("LanDangNhapCuoi"))
                        {
                            dgvTaiKhoan.Columns["LanDangNhapCuoi"].HeaderText = "Lần đăng nhập cuối";
                            dgvTaiKhoan.Columns["LanDangNhapCuoi"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                        }
                        if (dgvTaiKhoan.Columns.Contains("TrangThai")) dgvTaiKhoan.Columns["TrangThai"].HeaderText = "Trạng thái";
                        
                        dgvTaiKhoan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                    else
                    {
                        dgvTaiKhoan.DataSource = dt;
                    }
                }

                UpdateStatistics();
                AddLog("Tải danh sách tài khoản");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddLog("LỖI: Tải danh sách tài khoản - " + ex.Message);
            }
        }

        private void UpdateStatistics()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Tổng tài khoản
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM TaiKhoan", conn))
                    {
                        lblTongTaiKhoan.Text = cmd.ExecuteScalar().ToString();
                    }

                    // Quyền hạn
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM TaiKhoan WHERE QuyenHan = N'Admin'", conn))
                    {
                        lblAdmin.Text = "Admin: " + cmd.ExecuteScalar().ToString();
                    }

                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM TaiKhoan WHERE QuyenHan = N'Quản trị viên'", conn))
                    {
                        lblQuanTriVien.Text = "Quản trị viên: " + cmd.ExecuteScalar().ToString();
                    }

                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM TaiKhoan WHERE QuyenHan = N'Thành viên'", conn))
                    {
                        lblThanhVien.Text = "Thành viên: " + cmd.ExecuteScalar().ToString();
                    }

                    // Trạng thái
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM TaiKhoan WHERE TrangThai = N'Hoạt động'", conn))
                    {
                        lblHoatDong.Text = "Hoạt động: " + cmd.ExecuteScalar().ToString();
                    }

                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM TaiKhoan WHERE TrangThai = N'Khóa'", conn))
                    {
                        lblKhoa.Text = "Khóa: " + cmd.ExecuteScalar().ToString();
                    }

                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM TaiKhoan WHERE TrangThai = N'Chờ kích hoạt'", conn))
                    {
                        lblChoKichHoat.Text = "Chờ kích hoạt: " + cmd.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string tenDN = txtTenDN.Text.Trim();
                string matKhau = txtMatKhau.Text.Trim();
                string maTVText = txtMaTV.Text.Trim();
                string quyenHan = cboQuyenHan.SelectedItem?.ToString();
                string trangThai = cboTrangThai.SelectedItem?.ToString();

                // Validate
                if (string.IsNullOrEmpty(tenDN))
                {
                    MessageBox.Show("Tên đăng nhập không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenDN.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(matKhau))
                {
                    MessageBox.Show("Mật khẩu không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(maTVText))
                {
                    MessageBox.Show("Mã thành viên không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaTV.Focus();
                    return;
                }

                if (!int.TryParse(maTVText, out int maTV))
                {
                    MessageBox.Show("Mã thành viên phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaTV.Focus();
                    return;
                }

                // ✅ VALIDATE: Check if MaTV exists in ThanhVien table
                var memberInfo = GetMemberInfo(maTV);
                if (memberInfo == null)
                {
                    MessageBox.Show(
                        $"❌ Mã thành viên {maTV} không tồn tại trong hệ thống!\n\n" +
                        $"Vui lòng:\n" +
                        $"1. Kiểm tra lại mã thành viên\n" +
                        $"2. Hoặc tạo thành viên mới trước trong module 'Quản lý thành viên'",
                        "Lỗi - Thành viên không tồn tại", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                    txtMaTV.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(quyenHan))
                {
                    quyenHan = "Thành viên";
                }

                if (string.IsNullOrEmpty(trangThai))
                {
                    trangThai = "Chờ kích hoạt";
                }

                // ✅ INSERT vào bảng TaiKhoan
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(@"
                    INSERT INTO TaiKhoan(TenDN, MatKhau, MaTV, QuyenHan, TrangThai, NgayTao) 
                    VALUES(@tendn, @matkhau, @matv, @quyenhan, @trangthai, GETDATE())", conn))
                {
                    cmd.Parameters.AddWithValue("@tendn", tenDN);
                    cmd.Parameters.AddWithValue("@matkhau", matKhau); // In production, should hash password
                    cmd.Parameters.AddWithValue("@matv", maTV);
                    cmd.Parameters.AddWithValue("@quyenhan", quyenHan);
                    cmd.Parameters.AddWithValue("@trangthai", trangThai);

                    conn.Open();
                    try
                    {
                        int r = cmd.ExecuteNonQuery();
                        if (r > 0)
                        {
                            MessageBox.Show(
                                $"✅ Thêm tài khoản thành công!\n\n" +
                                $"👤 Họ tên: {memberInfo.HoTen}\n" +
                                $"📱 SĐT: {memberInfo.SDT}\n" +
                                $"🔑 Tài khoản: {tenDN}\n" +
                                $"⚡ Quyền hạn: {quyenHan}\n" +
                                $"📊 Trạng thái: {trangThai}", 
                                "Thành công", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);
                            AddLog($"Thêm tài khoản: {tenDN} - {quyenHan} cho {memberInfo.HoTen}");
                            ClearFields();
                            LoadTaiKhoan();
                        }
                    }
                    catch (SqlException sex)
                    {
                        if (sex.Number == 2627 || sex.Number == 2601) // Unique constraint violation
                        {
                            MessageBox.Show(
                                $"❌ Tên đăng nhập '{tenDN}' hoặc Mã thành viên {maTV} đã được sử dụng!\n\n" +
                                "Vui lòng:\n" +
                                "• Chọn tên đăng nhập khác\n" +
                                "• Hoặc kiểm tra mã thành viên", 
                                "Lỗi - Trùng lặp dữ liệu", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                        }
                        else if (sex.Number == 547) // Foreign key constraint violation
                        {
                            MessageBox.Show(
                                $"❌ Mã thành viên {maTV} không tồn tại trong hệ thống!\n\n" +
                                "Vui lòng tạo thành viên trước trong module 'Quản lý thành viên'", 
                                "Lỗi - Foreign Key", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Lỗi khi thêm tài khoản: " + sex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        AddLog($"LỖI: Thêm tài khoản {tenDN} - {sex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddLog("LỎI: Thêm tài khoản - " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaTK.Text))
                {
                    MessageBox.Show("Chọn một tài khoản để sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maTK = int.Parse(txtMaTK.Text);
                string tenDN = txtTenDN.Text.Trim();
                string matKhau = txtMatKhau.Text.Trim();
                string maTVText = txtMaTV.Text.Trim();
                string quyenHan = cboQuyenHan.SelectedItem?.ToString();
                string trangThai = cboTrangThai.SelectedItem?.ToString();

                // Validate
                if (string.IsNullOrEmpty(tenDN))
                {
                    MessageBox.Show("Tên đăng nhập không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenDN.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(matKhau))
                {
                    MessageBox.Show("Mật khẩu không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(maTVText))
                {
                    MessageBox.Show("Mã thành viên không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaTV.Focus();
                    return;
                }

                if (!int.TryParse(maTVText, out int maTV))
                {
                    MessageBox.Show("Mã thành viên phải là số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaTV.Focus();
                    return;
                }

                // ✅ VALIDATE: Check if MaTV exists in ThanhVien table
                var memberInfo = GetMemberInfo(maTV);
                if (memberInfo == null)
                {
                    MessageBox.Show(
                        $"Mã thành viên {maTV} không tồn tại trong hệ thống!\n\n" +
                        "Vui lòng kiểm tra lại mã thành viên.",
                        "Lỗi", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                    txtMaTV.Focus();
                    return;
                }

                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(@"UPDATE TaiKhoan 
                                                 SET TenDN=@tendn, MatKhau=@matkhau, MaTV=@matv, 
                                                     QuyenHan=@quyenhan, TrangThai=@trangthai 
                                                 WHERE MaTK=@matk", conn))
                {
                    cmd.Parameters.AddWithValue("@matk", maTK);
                    cmd.Parameters.AddWithValue("@tendn", tenDN);
                    cmd.Parameters.AddWithValue("@matkhau", matKhau);
                    cmd.Parameters.AddWithValue("@matv", maTV);
                    cmd.Parameters.AddWithValue("@quyenhan", string.IsNullOrEmpty(quyenHan) ? (object)DBNull.Value : quyenHan);
                    cmd.Parameters.AddWithValue("@trangthai", string.IsNullOrEmpty(trangThai) ? (object)DBNull.Value : trangThai);

                    conn.Open();
                    try
                    {
                        int r = cmd.ExecuteNonQuery();
                        if (r > 0)
                        {
                            MessageBox.Show(
                                $"Cập nhật tài khoản thành công!\n\n" +
                                $"Họ tên: {memberInfo.HoTen}\n" +
                                $"SĐT: {memberInfo.SDT}",
                                "Thành công", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);
                            AddLog($"Cập nhật tài khoản: {tenDN} - {quyenHan} cho {memberInfo.HoTen}");
                            ClearFields();
                            LoadTaiKhoan();
                        }
                    }
                    catch (SqlException sex)
                    {
                        if (sex.Number == 2627 || sex.Number == 2601)
                        {
                            MessageBox.Show("Tên đăng nhập hoặc Mã thành viên đã tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (sex.Number == 547)
                        {
                            MessageBox.Show("Mã thành viên không tồn tại trong hệ thống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Lỗi khi cập nhật: " + sex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        AddLog($"LỖI: Cập nhật tài khoản {tenDN} - {sex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddLog("LỎI: Cập nhật tài khoản - " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaTK.Text))
                {
                    MessageBox.Show("Chọn một tài khoản để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maTK = int.Parse(txtMaTK.Text);
                string tenDN = txtTenDN.Text.Trim();

                var res = MessageBox.Show($"Bạn có chắc muốn xóa tài khoản '{tenDN}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res != DialogResult.Yes) return;

                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("DELETE FROM TaiKhoan WHERE MaTK=@matk", conn))
                {
                    cmd.Parameters.AddWithValue("@matk", maTK);
                    conn.Open();
                    int r = cmd.ExecuteNonQuery();
                    if (r > 0)
                    {
                        MessageBox.Show("Xóa tài khoản thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AddLog($"Xóa tài khoản: {tenDN}");
                        ClearFields();
                        LoadTaiKhoan();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddLog("LỖI: Xóa tài khoản - " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            AddLog("Làm mới form nhập liệu");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTaiKhoan();
            txtSearch.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            LoadTaiKhoan(searchText);
            AddLog($"Tìm kiếm: {searchText}");
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("Bạn có chắc muốn xóa tất cả log?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                logTable.Clear();
                AddLog("Đã xóa log");
            }
        }

        private void dgvTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            
            try
            {
                var row = dgvTaiKhoan.Rows[e.RowIndex];

                txtMaTK.Text = row.Cells["MaTK"].Value?.ToString();
                txtTenDN.Text = row.Cells["TenDN"].Value?.ToString();
                txtMatKhau.Text = row.Cells["MatKhau"].Value?.ToString();
                txtMaTV.Text = row.Cells["MaTV"].Value?.ToString();

                // Set HoTen and SDT from grid
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString() ?? "";
                txtSDT.Text = row.Cells["SDT"].Value?.ToString() ?? "";

                var quyenHan = row.Cells["QuyenHan"].Value?.ToString();
                if (!string.IsNullOrEmpty(quyenHan) && cboQuyenHan.Items.Contains(quyenHan))
                    cboQuyenHan.SelectedItem = quyenHan;
                else
                    cboQuyenHan.SelectedIndex = -1;

                var trangThai = row.Cells["TrangThai"].Value?.ToString();
                if (!string.IsNullOrEmpty(trangThai) && cboTrangThai.Items.Contains(trangThai))
                    cboTrangThai.SelectedItem = trangThai;
                else
                    cboTrangThai.SelectedIndex = -1;

                // Set date times
                if (row.Cells["NgayTao"].Value != null && row.Cells["NgayTao"].Value != DBNull.Value)
                {
                    dtpNgayTao.Value = Convert.ToDateTime(row.Cells["NgayTao"].Value);
                }

                if (row.Cells["LanDangNhapCuoi"].Value != null && row.Cells["LanDangNhapCuoi"].Value != DBNull.Value)
                {
                    dtpLanDangNhapCuoi.Value = Convert.ToDateTime(row.Cells["LanDangNhapCuoi"].Value);
                }
                else
                {
                    dtpLanDangNhapCuoi.Value = DateTime.Now;
                }

                AddLog($"Chọn tài khoản: {txtTenDN.Text}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtMaTK.Clear();
            txtTenDN.Clear();
            txtMatKhau.Clear();
            txtMaTV.Clear();
            txtHoTen.Clear();
            txtSDT.Clear();
            cboQuyenHan.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = -1;
            dtpNgayTao.Value = DateTime.Now;
            dtpLanDangNhapCuoi.Value = DateTime.Now;
            txtSearch.Clear();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }

    // Helper class to hold member information for Account module
    internal class AccountMemberInfo
    {
        public string HoTen { get; set; }
        public string SDT { get; set; }
    }
}
