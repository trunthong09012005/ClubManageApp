using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class ucAccountTest : UserControl
    {
        private string connectionString = ConnectionHelper.ConnectionString;
        private DataTable logTable;
        private string currentUserRole = "Admin"; // ✅ THÊM: Vai trò người dùng hiện tại (có thể truyền từ form chính)

        // ✅ PHÂN TRANG: Các biến quản lý phân trang
        private int currentPage = 1;
        private int pageSize = 20;
        private int totalRecords = 0;
        private int totalPages = 0;
        private string currentFilter = null;

        public ucAccountTest()
        {
            InitializeComponent();
            InitializeLogTable();
            InitializePagination();
            
            // Add event handler to txtMaTV to display member info when MaTV is entered
            txtMaTV.TextChanged += TxtMaTV_TextChanged;
        }

        // ✅ THÊM: Constructor nhận role từ form chính
        public ucAccountTest(string userRole) : this()
        {
            this.currentUserRole = userRole;
        }

        // ✅ PHÂN TRANG: Khởi tạo pagination
        private void InitializePagination()
        {
            cboPageSize.SelectedItem = "20";
            UpdatePaginationControls();
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
                currentFilter = filter;
                dgvTaiKhoan.DataSource = null;

                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // ✅ PHÂN TRANG: Đếm tổng số bản ghi
                    string countSql = @"SELECT COUNT(*) 
                                       FROM TaiKhoan TK
                                       LEFT JOIN ThanhVien TV ON TK.MaTV = TV.MaTV";
                    
                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        countSql += " WHERE TK.TenDN LIKE @f OR TV.HoTen LIKE @f OR TK.QuyenHan LIKE @f OR TK.TrangThai LIKE @f";
                    }

                    using (var countCmd = new SqlCommand(countSql, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(filter))
                        {
                            countCmd.Parameters.AddWithValue("@f", "%" + filter + "%");
                        }
                        totalRecords = (int)countCmd.ExecuteScalar();
                    }

                    // ✅ PHÂN TRANG: Tính tổng số trang
                    totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                    if (totalPages == 0) totalPages = 1;
                    
                    // ✅ PHÂN TRANG: Đảm bảo currentPage hợp lệ
                    if (currentPage > totalPages) currentPage = totalPages;
                    if (currentPage < 1) currentPage = 1;

                    // ✅ PHÂN TRANG: Lấy dữ liệu với OFFSET và FETCH
                    string sql = @"SELECT TK.MaTK, TK.TenDN, TK.MatKhau, TK.MaTV, TV.HoTen, TV.SDT,
                                   TK.QuyenHan, TK.NgayTao, TK.LanDangNhapCuoi, TK.TrangThai 
                                   FROM TaiKhoan TK
                                   LEFT JOIN ThanhVien TV ON TK.MaTV = TV.MaTV";

                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        sql += " WHERE TK.TenDN LIKE @f OR TV.HoTen LIKE @f OR TK.QuyenHan LIKE @f OR TK.TrangThai LIKE @f";
                    }

                    sql += " ORDER BY TK.MaTK OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(filter))
                        {
                            cmd.Parameters.AddWithValue("@f", "%" + filter + "%");
                        }
                        
                        int offset = (currentPage - 1) * pageSize;
                        cmd.Parameters.AddWithValue("@offset", offset);
                        cmd.Parameters.AddWithValue("@pageSize", pageSize);

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
                }

                // ✅ PHÂN TRANG: Cập nhật UI phân trang
                UpdatePaginationControls();
                UpdateStatistics();
                AddLog($"Tải danh sách tài khoản - Trang {currentPage}/{totalPages}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddLog("LỖI: Tải danh sách tài khoản - " + ex.Message);
            }
        }

        // ✅ PHÂN TRANG: Cập nhật trạng thái các nút phân trang
        private void UpdatePaginationControls()
        {
            lblPageInfo.Text = $"Trang {currentPage} / {totalPages} (Tổng: {totalRecords} tài khoản)";
            
            btnPreviousPage.Enabled = currentPage > 1;
            btnNextPage.Enabled = currentPage < totalPages;
            
            // Đổi màu nút khi disabled
            btnPreviousPage.BackColor = btnPreviousPage.Enabled 
                ? System.Drawing.Color.FromArgb(52, 152, 219) 
                : System.Drawing.Color.FromArgb(189, 195, 199);
            
            btnNextPage.BackColor = btnNextPage.Enabled 
                ? System.Drawing.Color.FromArgb(52, 152, 219) 
                : System.Drawing.Color.FromArgb(189, 195, 199);
        }

        // ✅ PHÂN TRANG: Nút trang trước
        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadTaiKhoan(currentFilter);
                AddLog($"Chuyển sang trang {currentPage}");
            }
        }

        // ✅ PHÂN TRANG: Nút trang sau
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadTaiKhoan(currentFilter);
                AddLog($"Chuyển sang trang {currentPage}");
            }
        }

        // ✅ PHÂN TRANG: Thay đổi số bản ghi mỗi trang
        private void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(cboPageSize.SelectedItem?.ToString(), out int newPageSize))
            {
                pageSize = newPageSize;
                currentPage = 1; // Reset về trang đầu
                LoadTaiKhoan(currentFilter);
                AddLog($"Đổi số bản ghi mỗi trang: {pageSize}");
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

                // ✅ VALIDATE: Tên đăng nhập không được để trống
                if (string.IsNullOrEmpty(tenDN))
                {
                    MessageBox.Show("❌ Tên đăng nhập không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenDN.Focus();
                    return;
                }

                // ✅ VALIDATE: Tên đăng nhập chi tiết
                if (!ValidateTenDangNhap(tenDN))
                {
                    return;
                }

                // ✅ VALIDATE: Mật khẩu không được để trống
                if (string.IsNullOrEmpty(matKhau))
                {
                    MessageBox.Show("❌ Mật khẩu không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return;
                }

                // ✅ VALIDATE: Mật khẩu chi tiết
                if (!ValidateMatKhau(matKhau))
                {
                    return;
                }

                // ✅ VALIDATE: Mã thành viên không được để trống
                if (string.IsNullOrEmpty(maTVText))
                {
                    MessageBox.Show("❌ Mã thành viên không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaTV.Focus();
                    return;
                }

                if (!int.TryParse(maTVText, out int maTV))
                {
                    MessageBox.Show("❌ Mã thành viên phải là số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                // Default values
                if (string.IsNullOrEmpty(quyenHan))
                {
                    quyenHan = "Thành viên";
                }

                if (string.IsNullOrEmpty(trangThai))
                {
                    trangThai = "Chờ kích hoạt";
                }

                // ✅ VALIDATE: Kiểm tra quyền hạn Admin
                if (string.Equals(quyenHan, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    // Kiểm tra quyền của người dùng hiện tại
                    if (!string.Equals(currentUserRole, "Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show(
                            "⛔ KHÔNG CÓ QUYỀN!\n\n" +
                            "Chỉ có Admin mới được phép đặt quyền Admin cho tài khoản khác.\n\n" +
                            $"Vai trò hiện tại của bạn: {currentUserRole}",
                            "Không có quyền",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        cboQuyenHan.SelectedItem = "Thành viên";
                        AddLog($"LỖI: Cố tạo Admin khi không có quyền - User: {currentUserRole}");
                        return;
                    }

                    // Kiểm tra số lượng Admin hiện tại
                    int adminCount = GetAdminCount();
                    if (adminCount >= 1)
                    {
                        MessageBox.Show(
                            "❌ KHÔNG THỂ TẠO ADMIN MỚI!\n\n" +
                            "Hệ thống chỉ cho phép TỐI ĐA MỘT ADMIN duy nhất.\n" +
                            $"Hiện tại đã có {adminCount} Admin trong hệ thống.\n\n" +
                            "Để tạo Admin mới, bạn cần:\n" +
                            "1. Đổi quyền hạn của Admin hiện tại sang quyền khác\n" +
                            "2. Sau đó mới có thể tạo Admin mới",
                            "Vi phạm quy tắc hệ thống",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        cboQuyenHan.SelectedItem = "Thành viên";
                        AddLog($"LỖI: Cố tạo Admin thứ {adminCount + 1}");
                        return;
                    }

                    // Xác nhận tạo Admin
                    DialogResult confirmAdmin = MessageBox.Show(
                        "⚠️ XÁC NHẬN TẠO ADMIN!\n\n" +
                        $"Bạn đang tạo tài khoản Admin cho:\n" +
                        $"• Họ tên: {memberInfo.HoTen}\n" +
                        $"• Tài khoản: {tenDN}\n\n" +
                        "Admin sẽ có toàn quyền trong hệ thống.\n\n" +
                        "Bạn có CHẮC CHẮN muốn tiếp tục?",
                        "Xác nhận tạo Admin",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmAdmin != DialogResult.Yes)
                    {
                        AddLog("HỦY: Tạo tài khoản Admin");
                        return;
                    }
                }

                // ✅ INSERT vào bảng TaiKhoan
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(@"
                    INSERT INTO TaiKhoan(TenDN, MatKhau, MaTV, QuyenHan, TrangThai, NgayTao) 
                    VALUES(@tendn, @matkhau, @matv, @quyenhan, @trangthai, GETDATE())", conn))
                {
                    cmd.Parameters.AddWithValue("@tendn", tenDN);
                    cmd.Parameters.AddWithValue("@matkhau", matKhau); // Plain text (theo yêu cầu không mã hóa)
                    cmd.Parameters.AddWithValue("@matv", maTV);
                    cmd.Parameters.AddWithValue("@quyenhan", quyenHan);
                    cmd.Parameters.AddWithValue("@trangthai", trangThai);

                    conn.Open();
                    try
                    {
                        int r = cmd.ExecuteNonQuery();
                        if (r > 0)
                        {
                            string successMessage = "✅ Thêm tài khoản thành công!\n\n" +
                                $"👤 Họ tên: {memberInfo.HoTen}\n" +
                                $"📱 SĐT: {memberInfo.SDT}\n" +
                                $"🔑 Tài khoản: {tenDN}\n" +
                                $"⚡ Quyền hạn: {quyenHan}\n" +
                                $"📊 Trạng thái: {trangThai}";

                            if (string.Equals(quyenHan, "Admin", StringComparison.OrdinalIgnoreCase))
                            {
                                successMessage += "\n\n👑 VAI TRÒ ADMIN ĐÃ ĐƯỢC GÁN!";
                            }

                            MessageBox.Show(successMessage, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AddLog($"Thêm tài khoản: {tenDN} - {quyenHan} cho {memberInfo.HoTen}");
                            ClearFields();
                            currentPage = 1; // ✅ PHÂN TRANG: Reset về trang đầu khi thêm mới
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
                            MessageBox.Show("❌ Lỗi khi thêm tài khoản: " + sex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        AddLog($"LỖI: Thêm tài khoản {tenDN} - {sex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi thêm tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddLog("LỖI: Thêm tài khoản - " + ex.Message);
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            currentPage = 1; // ✅ PHÂN TRANG: Reset về trang đầu
            LoadTaiKhoan();
            txtSearch.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            currentPage = 1; // ✅ PHÂN TRANG: Reset về trang đầu khi tìm kiếm
            LoadTaiKhoan(searchText);
            AddLog($"Tìm kiếm: {searchText}");
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

        // ✅ THÊM: Helper methods cho validation
        
        /// <summary>
        /// Đếm số lượng Admin trong hệ thống
        /// </summary>
        private int GetAdminCount()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM TaiKhoan WHERE QuyenHan = N'Admin'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Lấy quyền hạn của tài khoản theo MaTK
        /// </summary>
        private string GetQuyenHanByMaTK(int maTK)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT QuyenHan FROM TaiKhoan WHERE MaTK = @MaTK";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTK", maTK);
                        return cmd.ExecuteScalar()?.ToString() ?? "";
                    }
                }
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Kiểm tra tên đăng nhập đã tồn tại (phân biệt hoa thường)
        /// </summary>
        private bool CheckTenDangNhapExists(string tenDN, int? excludeMaTK = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = excludeMaTK.HasValue
                        ? "SELECT COUNT(*) FROM TaiKhoan WHERE TenDN COLLATE Latin1_General_CS_AS = @TenDN AND MaTK != @MaTK"
                        : "SELECT COUNT(*) FROM TaiKhoan WHERE TenDN COLLATE Latin1_General_CS_AS = @TenDN";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenDN", tenDN);
                        if (excludeMaTK.HasValue)
                            cmd.Parameters.AddWithValue("@MaTK", excludeMaTK.Value);
                        
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

        /// <summary>
        /// Validate tên đăng nhập
        /// </summary>
        private bool ValidateTenDangNhap(string tenDN, int? excludeMaTK = null)
        {
            // Kiểm tra độ dài
            if (tenDN.Length < 3)
            {
                MessageBox.Show(
                    "❌ Tên đăng nhập phải có ít nhất 3 ký tự!\n\n" +
                    $"Độ dài hiện tại: {tenDN.Length} ký tự", 
                    "Tên đăng nhập không hợp lệ",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                txtTenDN.Focus();
                return false;
            }

            if (tenDN.Length > 50)
            {
                MessageBox.Show(
                    "❌ Tên đăng nhập không được vượt quá 50 ký tự!\n\n" +
                    $"Độ dài hiện tại: {tenDN.Length} ký tự", 
                    "Tên đăng nhập quá dài",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                txtTenDN.Focus();
                return false;
            }

            // Kiểm tra ký tự hợp lệ (chỉ chữ, số, _, -)
            if (!Regex.IsMatch(tenDN, @"^[a-zA-Z0-9_-]+$"))
            {
                MessageBox.Show(
                    "❌ Tên đăng nhập chỉ được chứa:\n\n" +
                    "• Chữ cái (a-z, A-Z)\n" +
                    "• Số (0-9)\n" +
                    "• Dấu gạch dưới (_)\n" +
                    "• Dấu gạch ngang (-)\n\n" +
                    "Không được chứa khoảng trắng hoặc ký tự đặc biệt khác!",
                    "Tên đăng nhập không hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtTenDN.Focus();
                return false;
            }

            // Không được bắt đầu bằng số
            if (char.IsDigit(tenDN[0]))
            {
                MessageBox.Show(
                    "❌ Tên đăng nhập không được bắt đầu bằng số!\n\n" +
                    $"Ký tự đầu tiên: '{tenDN[0]}'", 
                    "Tên đăng nhập không hợp lệ",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                txtTenDN.Focus();
                return false;
            }

            // Kiểm tra tên đăng nhập trùng (phân biệt hoa thường)
            if (CheckTenDangNhapExists(tenDN, excludeMaTK))
            {
                MessageBox.Show(
                    $"❌ Tên đăng nhập '{tenDN}' đã được sử dụng!\n\n" +
                    "Lưu ý: Tên đăng nhập PHÂN BIỆT HOA THƯỜNG.\n" +
                    "Ví dụ: 'Admin' khác với 'admin'\n\n" +
                    "Vui lòng chọn tên đăng nhập khác.",
                    "Tên đăng nhập đã tồn tại",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtTenDN.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate mật khẩu
        /// </summary>
        private bool ValidateMatKhau(string matKhau)
        {
            // Kiểm tra độ dài tối thiểu
            if (matKhau.Length < 6)
            {
                MessageBox.Show(
                    "❌ Mật khẩu phải có ít nhất 6 ký tự!\n\n" +
                    $"Độ dài hiện tại: {matKhau.Length} ký tự\n\n" +
                    "Đề xuất mật khẩu mạnh:\n" +
                    "• Ít nhất 8 ký tự\n" +
                    "• Có chữ hoa và chữ thường\n" +
                    "• Có số\n" +
                    "• Có ký tự đặc biệt (!@#$%^&*)",
                    "Mật khẩu quá ngắn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            // Không chứa khoảng trắng
            if (matKhau.Contains(" "))
            {
                MessageBox.Show(
                    "❌ Mật khẩu không được chứa khoảng trắng!",
                    "Mật khẩu không hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            // Kiểm tra độ mạnh mật khẩu (cảnh báo, không bắt buộc)
            bool hasUpper = matKhau.Any(char.IsUpper);
            bool hasLower = matKhau.Any(char.IsLower);
            bool hasDigit = matKhau.Any(char.IsDigit);
            bool hasSpecial = matKhau.Any(c => "!@#$%^&*()_+-=[]{}|;:,.<>?".Contains(c));

            int strength = 0;
            if (hasUpper) strength++;
            if (hasLower) strength++;
            if (hasDigit) strength++;
            if (hasSpecial) strength++;

            if (strength < 3 && matKhau.Length < 8)
            {
                DialogResult result = MessageBox.Show(
                    "⚠️ MẬT KHẨU YẾU!\n\n" +
                    "Mật khẩu của bạn không đủ mạnh.\n\n" +
                    "Đề xuất:\n" +
                    "• Có chữ hoa (A-Z)\n" +
                    "• Có chữ thường (a-z)\n" +
                    "• Có số (0-9)\n" +
                    "• Có ký tự đặc biệt (!@#$%^&*)\n\n" +
                    "Bạn có muốn tiếp tục với mật khẩu này không?",
                    "Cảnh báo mật khẩu yếu",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                
                if (result != DialogResult.Yes)
                {
                    txtMatKhau.Focus();
                    return false;
                }
            }

            return true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaTK.Text))
                {
                    MessageBox.Show("❌ Chọn một tài khoản để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maTK = int.Parse(txtMaTK.Text);
                string tenDN = txtTenDN.Text.Trim();
                string matKhau = txtMatKhau.Text.Trim();
                string maTVText = txtMaTV.Text.Trim();
                string quyenHan = cboQuyenHan.SelectedItem?.ToString();
                string trangThai = cboTrangThai.SelectedItem?.ToString();

                // ✅ VALIDATE: Tên đăng nhập
                if (string.IsNullOrEmpty(tenDN))
                {
                    MessageBox.Show("❌ Tên đăng nhập không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenDN.Focus();
                    return;
                }

                if (!ValidateTenDangNhap(tenDN, maTK))
                {
                    return;
                }

                // ✅ VALIDATE: Mật khẩu
                if (string.IsNullOrEmpty(matKhau))
                {
                    MessageBox.Show("❌ Mật khẩu không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return;
                }

                if (!ValidateMatKhau(matKhau))
                {
                    return;
                }

                // ✅ VALIDATE: Mã thành viên
                if (string.IsNullOrEmpty(maTVText))
                {
                    MessageBox.Show("❌ Mã thành viên không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaTV.Focus();
                    return;
                }

                if (!int.TryParse(maTVText, out int maTV))
                {
                    MessageBox.Show("❌ Mã thành viên phải là số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaTV.Focus();
                    return;
                }

                // ✅ VALIDATE: Check if MaTV exists
                var memberInfo = GetMemberInfo(maTV);
                if (memberInfo == null)
                {
                    MessageBox.Show(
                        $"❌ Mã thành viên {maTV} không tồn tại trong hệ thống!\n\n" +
                        "Vui lòng kiểm tra lại mã thành viên.",
                        "Lỗi", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                    txtMaTV.Focus();
                    return;
                }

                // ✅ VALIDATE: Kiểm tra quyền hạn Admin
                string oldQuyenHan = GetQuyenHanByMaTK(maTK);
                bool isChangingToAdmin = string.Equals(quyenHan, "Admin", StringComparison.OrdinalIgnoreCase);
                bool wasAdmin = string.Equals(oldQuyenHan, "Admin", StringComparison.OrdinalIgnoreCase);

                // Nếu đang cố đổi thành Admin (và không phải Admin trước đó)
                if (isChangingToAdmin && !wasAdmin)
                {
                    // Kiểm tra quyền
                    if (!string.Equals(currentUserRole, "Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show(
                            "⛔ KHÔNG CÓ QUYỀN!\n\n" +
                            "Chỉ có Admin mới được phép đặt quyền Admin.",
                            "Không có quyền",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        cboQuyenHan.SelectedItem = oldQuyenHan;
                        return;
                    }

                    // Kiểm tra số Admin hiện tại
                    int adminCount = GetAdminCount();
                    if (adminCount >= 1)
                    {
                        MessageBox.Show(
                            "❌ KHÔNG THỂ ĐỔI THÀNH ADMIN!\n\n" +
                            "Hệ thống chỉ cho phép một Admin duy nhất.",
                            "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        cboQuyenHan.SelectedItem = oldQuyenHan;
                        return;
                    }
                }

                // Nếu đang đổi Admin sang quyền khác
                if (wasAdmin && !isChangingToAdmin)
                {
                    int adminCount = GetAdminCount();
                    if (adminCount <= 1)
                    {
                        DialogResult result = MessageBox.Show(
                            "⚠️ CẢNH BÁO QUAN TRỌNG!\n\n" +
                            "Bạn đang đổi quyền hạn của Admin DUY NHẤT trong hệ thống.\n" +
                            "Sau khi đổi, hệ thống sẽ KHÔNG CÒN ADMIN nào!\n\n" +
                            "Hậu quả:\n" +
                            "• Mất toàn bộ quyền quản trị\n" +
                            "• Không thể quản lý tài khoản, thành viên\n" +
                            "• Có thể phải can thiệp trực tiếp vào database\n\n" +
                            "Bạn có THỰC SỰ CHẮC CHẮN muốn tiếp tục?",
                            "Cảnh báo nghiêm trọng",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);
                        
                        if (result != DialogResult.Yes)
                        {
                            cboQuyenHan.SelectedItem = oldQuyenHan;
                            AddLog("HỦY: Đổi quyền Admin duy nhất");
                            return;
                        }
                    }
                }

                // ✅ UPDATE tài khoản
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(@"UPDATE TaiKhoan 
                                                 SET TenDN=@tendn, MatKhau=@matkhau, MaTV=@matv, 
                                                     QuyenHan=@quyenhan, TrangThai=@trangthai 
                                                 WHERE MaTK=@matk", conn))
                {
                    cmd.Parameters.AddWithValue("@matk", maTK);
                    cmd.Parameters.AddWithValue("@tendn", tenDN);
                    cmd.Parameters.AddWithValue("@matkhau", matKhau); // Plain text
                    cmd.Parameters.AddWithValue("@matv", maTV);
                    cmd.Parameters.AddWithValue("@quyenhan", string.IsNullOrEmpty(quyenHan) ? (object)DBNull.Value : quyenHan);
                    cmd.Parameters.AddWithValue("@trangthai", string.IsNullOrEmpty(trangThai) ? (object)DBNull.Value : trangThai);

                    conn.Open();
                    try
                    {
                        int r = cmd.ExecuteNonQuery();
                        if (r > 0)
                        {
                            string successMessage = "✅ Cập nhật tài khoản thành công!\n\n" +
                                $"Họ tên: {memberInfo.HoTen}\n" +
                                $"SĐT: {memberInfo.SDT}\n" +
                                $"Quyền hạn: {quyenHan}";

                            if (isChangingToAdmin && !wasAdmin)
                            {
                                successMessage += "\n\n👑 VAI TRÒ ADMIN ĐÃ ĐƯỢC GÁN!";
                            }

                            MessageBox.Show(successMessage, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AddLog($"Cập nhật tài khoản: {tenDN} - {quyenHan} cho {memberInfo.HoTen}");
                            ClearFields();
                            LoadTaiKhoan();
                        }
                    }
                    catch (SqlException sex)
                    {
                        if (sex.Number == 2627 || sex.Number == 2601)
                        {
                            MessageBox.Show("❌ Tên đăng nhập hoặc Mã thành viên đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (sex.Number == 547)
                        {
                            MessageBox.Show("❌ Mã thành viên không tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("❌ Lỗi khi cập nhật: " + sex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        AddLog($"LỖI: Cập nhật tài khoản {tenDN} - {sex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddLog("LỖI: Cập nhật tài khoản - " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaTK.Text))
                {
                    MessageBox.Show("❌ Chọn một tài khoản để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maTK = int.Parse(txtMaTK.Text);
                string tenDN = txtTenDN.Text.Trim();

                // ✅ VALIDATE: Lấy quyền hạn của tài khoản sắp xóa
                string quyenHan = GetQuyenHanByMaTK(maTK);

                // ✅ VALIDATE: Không được xóa Admin duy nhất
                if (string.Equals(quyenHan, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    int adminCount = GetAdminCount();
                    if (adminCount <= 1)
                    {
                        MessageBox.Show(
                            "🚫 KHÔNG THỂ XÓA ADMIN DUY NHẤT!\n\n" +
                            "Đây là Admin duy nhất trong hệ thống.\n" +
                            "Việc xóa Admin này sẽ khiến hệ thống mất quyền quản trị.\n\n" +
                            "Để xóa tài khoản này, bạn cần:\n" +
                            "1. Tạo một Admin mới trước\n" +
                            "2. Hoặc chuyển tài khoản này sang quyền khác\n" +
                            "3. Sau đó mới có thể xóa",
                            "Không thể xóa",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        AddLog($"LỖI: Cố xóa Admin duy nhất - {tenDN}");
                        return;
                    }

                    // Kiểm tra quyền
                    if (!string.Equals(currentUserRole, "Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show(
                            "⛔ KHÔNG CÓ QUYỀN!\n\n" +
                            "Chỉ có Admin mới được phép xóa tài khoản Admin khác.\n" +
                            $"Vai trò hiện tại của bạn: {currentUserRole}",
                            "Không có quyền",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        AddLog($"LỖI: Cố xóa Admin khi không có quyền - User: {currentUserRole}");
                        return;
                    }

                    // Cảnh báo nghiêm trọng khi xóa Admin
                    DialogResult confirmAdmin = MessageBox.Show(
                        $"⚠️ CẢNH BÁO: BẠN ĐANG XÓA MỘT ADMIN!\n\n" +
                        $"Tài khoản: {tenDN}\n" +
                        $"Quyền hạn: {quyenHan}\n\n" +
                        "Đây là hành động nghiêm trọng và KHÔNG THỂ HOÀN TÁC!\n\n" +
                        "Bạn có CHẮC CHẮN muốn xóa Admin này không?",
                        "Cảnh báo xóa Admin",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    
                    if (confirmAdmin != DialogResult.Yes)
                    {
                        AddLog("HỦY: Xóa tài khoản Admin");
                        return;
                    }
                }

                // ✅ XÁC NHẬN XÓA thông thường
                var res = MessageBox.Show(
                    $"Bạn có chắc muốn xóa tài khoản '{tenDN}'?\n\n" +
                    $"Quyền hạn: {quyenHan}\n\n" +
                    "Hành động này KHÔNG THỂ HOÀN TÁC!", 
                    "Xác nhận", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);
                
                if (res != DialogResult.Yes) 
                {
                    AddLog("HỦY: Xóa tài khoản");
                    return;
                }

                // ✅ Thực hiện xóa với Transaction
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Double-check lại trước khi xóa
                            string checkQuery = "SELECT QuyenHan FROM TaiKhoan WHERE MaTK = @MaTK";
                            using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn, transaction))
                            {
                                checkCmd.Parameters.AddWithValue("@MaTK", maTK);
                                string currentRole = checkCmd.ExecuteScalar()?.ToString() ?? "";
                                
                                if (string.Equals(currentRole, "Admin", StringComparison.OrdinalIgnoreCase))
                                {
                                    string countQuery = "SELECT COUNT(*) FROM TaiKhoan WHERE QuyenHan = N'Admin'";
                                    using (SqlCommand countCmd = new SqlCommand(countQuery, conn, transaction))
                                    {
                                        int count = (int)countCmd.ExecuteScalar();
                                        if (count <= 1)
                                        {
                                            transaction.Rollback();
                                            MessageBox.Show(
                                                "🚫 Không thể xóa Admin duy nhất trong hệ thống!",
                                                "Lỗi",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Error);
                                            return;
                                        }
                                    }
                                }
                            }
                            
                            // Thực hiện xóa
                            string deleteQuery = "DELETE FROM TaiKhoan WHERE MaTK = @MaTK";
                            using (SqlCommand cmd = new SqlCommand(deleteQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@MaTK", maTK);
                                int rowsAffected = cmd.ExecuteNonQuery();
                                
                                if (rowsAffected > 0)
                                {
                                    transaction.Commit();
                                    MessageBox.Show("✅ Xóa tài khoản thành công!", "Thành công", 
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    AddLog($"Xóa tài khoản: {tenDN} - {quyenHan}");
                                    ClearFields();
                                    LoadTaiKhoan();
                                }
                                else
                                {
                                    transaction.Rollback();
                                    MessageBox.Show("❌ Không tìm thấy tài khoản để xóa!", "Lỗi", 
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 547) // Foreign key constraint
                {
                    MessageBox.Show(
                        "❌ Không thể xóa tài khoản này!\n\n" +
                        "Tài khoản đang có dữ liệu liên quan trong hệ thống:\n" +
                        "• Lịch sử đăng nhập\n" +
                        "• Hoạt động đã tham gia\n" +
                        "• Hoặc các dữ liệu khác\n\n" +
                        "Vui lòng xóa các dữ liệu liên quan trước hoặc đổi trạng thái thành 'Khóa'.",
                        "Không thể xóa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    AddLog($"LỖI: Không thể xóa do Foreign Key - {sqlEx.Message}");
                }
                else
                {
                    MessageBox.Show($"❌ Lỗi SQL khi xóa: {sqlEx.Message}", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddLog($"LỖI: Xóa tài khoản - {sqlEx.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddLog("LỖI: Xóa tài khoản - " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            AddLog("Làm mới form nhập liệu");
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
    }

    // Helper class to hold member information for Account module
    internal class AccountMemberInfo
    {
        public string HoTen { get; set; }
        public string SDT { get; set; }
    }
}
