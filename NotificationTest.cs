using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class NotificationTest : UserControl
    {
        private string connectionString = ConnectionHelper.ConnectionString;
        private DataTable notificationsTable = null;
        private DataTable logTable = new DataTable();
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalPages = 1;

        public NotificationTest()
        {
            InitializeComponent();
        }

        private void NotificationTest_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeLogTable();
                LoadNotifications();
                LoadFilterOptions();
                
                // Make MaTB read-only; other top fields editable for inline edit
                if (txtMaTB != null) txtMaTB.ReadOnly = true;
                if (txtTieuDe != null) txtTieuDe.ReadOnly = false;
                if (txtDoiTuong != null) txtDoiTuong.ReadOnly = false;
                if (dtpNgayGui != null) dtpNgayGui.Enabled = true;
                if (cboFilterStatus != null) cboFilterStatus.Enabled = true;

                UpdateStats();
                SetupGridColumns();
                RefreshGrid();
                AddLog("🚀 Ứng dụng tải thành công");

                // wire double-click to open detail
                this.dgvThongBao.DoubleClick += DgvThongBao_DoubleClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeLogTable()
        {
            logTable.Columns.Add("Thời gian", typeof(DateTime));
            logTable.Columns.Add("Tác vụ", typeof(string));
            dgvLog.DataSource = logTable;
        }

        private void LoadNotifications()
        {
            // Load notifications into a DataTable from DB and keep for binding/filtering
            notificationsTable = new DataTable();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("SELECT MaTB, TieuDe, NoiDung, LoaiThongBao, NgayDang, NgayGui, NguoiDang, DoiTuong, TrangThai FROM ThongBao ORDER BY NgayGui DESC", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    conn.Open();
                    da.Fill(notificationsTable);
                }
            }
            catch (Exception ex)
            {
                AddLog($"❌ Lỗi tải: {ex.Message}");
                notificationsTable = new DataTable();
            }
        }

        private void LoadFilterOptions()
        {
            try
            {
                // Populate suggestion lists for type and status (do not use as filters)
                cboFilterType.Items.Clear();
                if (notificationsTable != null && notificationsTable.Rows.Count > 0)
                {
                    var types = notificationsTable.AsEnumerable()
                        .Select(r => r.Field<string>("LoaiThongBao"))
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Distinct()
                        .OrderBy(x => x);
                    foreach (var t in types) cboFilterType.Items.Add(t);
                }

                cboFilterStatus.Items.Clear();
                if (notificationsTable != null && notificationsTable.Rows.Count > 0)
                {
                    var statuses = notificationsTable.AsEnumerable()
                        .Select(r => r.Field<string>("TrangThai"))
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Distinct()
                        .OrderBy(x => x);
                    foreach (var s in statuses) cboFilterStatus.Items.Add(s);
                }
            }
            catch (Exception ex)
            {
                AddLog($"❌ Lỗi tải bộ gợi ý: {ex.Message}");
            }
        }

        private void RefreshGrid()
        {
            // Use notificationsTable as source. Apply only search then paging and bind
            List<DataRow> rows = new List<DataRow>();
            if (notificationsTable == null || notificationsTable.Rows.Count == 0)
            {
                dgvThongBao.DataSource = null;
            }
            else
            {
                string searchText = txtSearch.Text.Trim().ToLower();

                rows = notificationsTable.AsEnumerable()
                    .Where(r =>
                    {
                        bool matchSearch = string.IsNullOrEmpty(searchText) ||
                            (r.Field<string>("TieuDe") ?? string.Empty).ToLower().Contains(searchText) ||
                            (r.Field<string>("NoiDung") ?? string.Empty).ToLower().Contains(searchText);

                        return matchSearch;
                    })
                    .ToList();

                totalPages = Math.Max(1, (int)Math.Ceiling((double)rows.Count / pageSize));
                if (currentPage > totalPages) currentPage = totalPages;

                var paged = rows.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                DataTable bindTable;
                if (paged.Count > 0)
                    bindTable = paged.CopyToDataTable();
                else
                    bindTable = notificationsTable.Clone();

                dgvThongBao.DataSource = bindTable;
            }

            int totalFiltered = rows?.Count ?? 0;
            lblPageInfo.Text = $"Trang {currentPage} / {totalPages} (Tổng: {totalFiltered} thông báo)";
        }

        private void UpdateStats()
        {
            if (notificationsTable == null)
            {
                lblTongThongBao.Text = "0";
                lblNhap.Text = "Nháp: 0";
                lblDuyet.Text = "Đã gửi: 0";
                lblHuyBo.Text = "Không gửi: 0";
                return;
            }

            int total = notificationsTable.Rows.Count;
            int nhap = notificationsTable.AsEnumerable().Count(r => (r.Field<string>("TrangThai") ?? "") == "Nháp");
            int daGui = notificationsTable.AsEnumerable().Count(r => (r.Field<string>("TrangThai") ?? "") == "Đã gửi");
            int khongGui = notificationsTable.AsEnumerable().Count(r => (r.Field<string>("TrangThai") ?? "") == "Không gửi");

            lblTongThongBao.Text = total.ToString();
            lblNhap.Text = $"Nháp: {nhap}";
            lblDuyet.Text = $"Đã gửi: {daGui}";
            lblHuyBo.Text = $"Không gửi: {khongGui}";
        }

        private void AddLog(string action)
        {
            var row = logTable.NewRow();
            row["Thời gian"] = DateTime.Now;
            row["Tác vụ"] = action;
            logTable.Rows.InsertAt(row, 0);

            while (logTable.Rows.Count > 50)
                logTable.Rows.RemoveAt(logTable.Rows.Count - 1);
        }

        private void BtnAdd_OpenEditForm(object sender, EventArgs e)
        {
            using (var editForm = new NotificationEditForm())
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    var data = editForm.NotificationData;
                    try
                    {
                        // insert and get new identity
                        using (var conn = new SqlConnection(connectionString))
                        using (var cmd = new SqlCommand(@"INSERT INTO ThongBao (TieuDe, NoiDung, LoaiThongBao, NgayDang, NgayGui, NguoiDang, DoiTuong, TrangThai) "
                                                       + "VALUES (@TieuDe, @NoiDung, @Loai, GETDATE(), @NgayGui, NULL, @DoiTuong, @TrangThai); SELECT SCOPE_IDENTITY();", conn))
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@TieuDe", data.TieuDe ?? string.Empty);
                            cmd.Parameters.AddWithValue("@NoiDung", data.NoiDung ?? string.Empty);
                            cmd.Parameters.AddWithValue("@Loai", string.IsNullOrEmpty(data.LoaiThongBao) ? (object)DBNull.Value : data.LoaiThongBao);
                            cmd.Parameters.AddWithValue("@NgayGui", data.NgayGui == DateTime.MinValue ? (object)DBNull.Value : data.NgayGui);
                            cmd.Parameters.AddWithValue("@DoiTuong", string.IsNullOrEmpty(data.DoiTuong) ? (object)DBNull.Value : data.DoiTuong);
                            cmd.Parameters.AddWithValue("@TrangThai", string.IsNullOrEmpty(data.TrangThai) ? "Nháp" : data.TrangThai);

                            object idObj = cmd.ExecuteScalar();
                            int newId = 0;
                            if (idObj != null && int.TryParse(idObj.ToString(), out newId))
                            {
                                // add to in-memory table
                                if (notificationsTable == null) notificationsTable = new DataTable();
                                if (!notificationsTable.Columns.Contains("MaTB"))
                                {
                                    // ensure proper structure by reloading schema only
                                    LoadNotifications();
                                }

                                var row = notificationsTable.NewRow();
                                row["MaTB"] = newId;
                                row["TieuDe"] = data.TieuDe ?? string.Empty;
                                row["NoiDung"] = data.NoiDung ?? string.Empty;
                                row["LoaiThongBao"] = data.LoaiThongBao ?? string.Empty;
                                row["NgayDang"] = DateTime.Now;
                                row["NgayGui"] = data.NgayGui == DateTime.MinValue ? (object)DBNull.Value : data.NgayGui;
                                row["NguoiDang"] = DBNull.Value;
                                row["DoiTuong"] = data.DoiTuong ?? string.Empty;
                                row["TrangThai"] = data.TrangThai ?? "Nháp";

                                notificationsTable.Rows.InsertAt(row, 0);

                                // update UI only (do not reload from DB)
                                LoadFilterOptions();
                                RefreshGrid();
                                UpdateStats();
                            }
                            else
                            {
                                // fallback: reload from DB if identity couldn't be obtained
                                LoadNotifications();
                                LoadFilterOptions();
                                RefreshGrid();
                                UpdateStats();
                            }
                        }

                        AddLog($"✅ Thêm: {data.TieuDe}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi thêm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        AddLog($"❌ Lỗi thêm: {ex.Message}");
                    }
                }
            }
        }

        private void UpdateFromTopFields(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaTB.Text))
            {
                MessageBox.Show("Vui lòng chọn thông báo để sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (notificationsTable == null)
            {
                MessageBox.Show("Dữ liệu thông báo chưa được tải.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int ma;
            if (!int.TryParse(txtMaTB.Text, out ma))
            {
                MessageBox.Show("Mã thông báo không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var rowToUpdate = notificationsTable.AsEnumerable().FirstOrDefault(r => r.Field<int>("MaTB") == ma);
            if (rowToUpdate == null)
            {
                MessageBox.Show("Không tìm thấy thông báo đã chọn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Read values from top fields
            string tieude = txtTieuDe?.Text?.Trim() ?? string.Empty;
            string doituong = txtDoiTuong?.Text?.Trim() ?? string.Empty;
            DateTime ngayGui = dtpNgayGui?.Value ?? DateTime.Now;
            string loai = cboFilterType?.Text?.Trim() ?? string.Empty;
            string trangThai = cboFilterStatus?.Text?.Trim() ?? "Nháp";

            // ===== VALIDATION =====
            // Validation: Title
            if (string.IsNullOrWhiteSpace(tieude))
            {
                MessageBox.Show("Tiêu đề không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTieuDe?.Focus();
                return;
            }

            if (tieude.Length < 5)
            {
                MessageBox.Show("Tiêu đề phải có ít nhất 5 ký tự", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTieuDe?.Focus();
                return;
            }

            if (tieude.Length > 200)
            {
                MessageBox.Show("Tiêu đề không được vượt quá 200 ký tự", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTieuDe?.Focus();
                return;
            }

            // Validation: Recipient
            if (string.IsNullOrWhiteSpace(doituong))
            {
                MessageBox.Show("Đối tượng không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDoiTuong?.Focus();
                return;
            }

            // Validation: Type
            if (string.IsNullOrWhiteSpace(loai))
            {
                MessageBox.Show("Vui lòng chọn loại thông báo", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboFilterType?.Focus();
                return;
            }

            // Validation: Status
            if (string.IsNullOrWhiteSpace(trangThai))
            {
                MessageBox.Show("Vui lòng chọn trạng thái", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboFilterStatus?.Focus();
                return;
            }

            // Validation: Date
            if (ngayGui < DateTime.Now.AddDays(-1))
            {
                MessageBox.Show("Ngày gửi không được trong quá khứ quá xa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayGui?.Focus();
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(@"UPDATE ThongBao SET TieuDe=@TieuDe, LoaiThongBao=@Loai, NgayGui=@NgayGui, DoiTuong=@DoiTuong, TrangThai=@TrangThai WHERE MaTB=@MaTB", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@TieuDe", tieude);
                    cmd.Parameters.AddWithValue("@Loai", string.IsNullOrEmpty(loai) ? (object)DBNull.Value : loai);
                    cmd.Parameters.AddWithValue("@NgayGui", ngayGui == DateTime.MinValue ? (object)DBNull.Value : ngayGui);
                    cmd.Parameters.AddWithValue("@DoiTuong", string.IsNullOrEmpty(doituong) ? (object)DBNull.Value : doituong);
                    cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                    cmd.Parameters.AddWithValue("@MaTB", ma);
                    cmd.ExecuteNonQuery();
                }

                AddLog($"✏️ Sửa: {tieude}");

                // update notificationsTable (in-memory)
                rowToUpdate.SetField("TieuDe", tieude);
                rowToUpdate.SetField("LoaiThongBao", loai ?? string.Empty);
                rowToUpdate.SetField("NgayGui", ngayGui == DateTime.MinValue ? (object)DBNull.Value : (object)ngayGui);
                rowToUpdate.SetField("DoiTuong", doituong ?? string.Empty);
                rowToUpdate.SetField("TrangThai", trangThai ?? string.Empty);

                RefreshGrid();
                LoadFilterOptions();
                UpdateStats();
                
                MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddLog($"❌ Lỗi cập nhật: {ex.Message}");
            }
        }

        private void DgvThongBao_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (dgvThongBao.CurrentRow == null) return;
                var drv = dgvThongBao.CurrentRow.DataBoundItem as DataRowView;
                if (drv == null) return;

                int maTb = drv.Row["MaTB"] != DBNull.Value ? Convert.ToInt32(drv.Row["MaTB"]) : 0;
                string tieuDe = drv.Row["TieuDe"] != DBNull.Value ? drv.Row["TieuDe"].ToString() : string.Empty;
                string noiDung = drv.Row["NoiDung"] != DBNull.Value ? drv.Row["NoiDung"].ToString() : string.Empty;
                string loaiThongBao = drv.Row["LoaiThongBao"] != DBNull.Value ? drv.Row["LoaiThongBao"].ToString() : string.Empty;
                DateTime ngayGui = drv.Row["NgayGui"] != DBNull.Value ? Convert.ToDateTime(drv.Row["NgayGui"]) : DateTime.MinValue;
                string doiTuong = drv.Row["DoiTuong"] != DBNull.Value ? drv.Row["DoiTuong"].ToString() : string.Empty;
                string trangThai = drv.Row["TrangThai"] != DBNull.Value ? drv.Row["TrangThai"].ToString() : string.Empty;

                using (var dlg = new Form())
                {
                    dlg.Text = "Chi tiết thông báo";
                    dlg.Size = new Size(700, 600);
                    dlg.StartPosition = FormStartPosition.CenterParent;
                    dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                    dlg.MaximizeBox = false;
                    dlg.MinimizeBox = false;

                    var pnlTop = new Panel { Dock = DockStyle.Top, Height = 120, BackColor = Color.FromArgb(41, 128, 185) };
                    var lblTieuDe = new Label
                    {
                        Text = tieuDe,
                        Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                        ForeColor = Color.White,
                        AutoSize = false,
                        Dock = DockStyle.Top,
                        Padding = new Padding(10),
                        TextAlign = ContentAlignment.MiddleLeft
                    };
                    lblTieuDe.Height = 80; // reserve space for title
                    pnlTop.Controls.Add(lblTieuDe);

                    var lblInfo = new Label
                    {
                        Text = $"Loại: {loaiThongBao} | Trạng thái: {trangThai} | Ngày gửi: {(ngayGui != DateTime.MinValue ? ngayGui.ToString("dd/MM/yyyy HH:mm") : "N/A")}",
                        Font = new Font("Segoe UI", 9F),
                        ForeColor = Color.White,
                        AutoSize = false,
                        Dock = DockStyle.Bottom,
                        Height = 30,
                        Padding = new Padding(10),
                        TextAlign = ContentAlignment.MiddleLeft
                    };
                    pnlTop.Controls.Add(lblInfo);

                    var pnlContent = new Panel { Dock = DockStyle.Fill };
                    var txtContent = new TextBox 
                    { 
                        Multiline = true, 
                        ReadOnly = true, 
                        Text = noiDung, 
                        Dock = DockStyle.Fill, 
                        ScrollBars = ScrollBars.Vertical,
                        Font = new Font("Segoe UI", 9.75F),
                        BackColor = Color.White
                    };
                    pnlContent.Controls.Add(txtContent);

                    var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 50, BackColor = Color.FromArgb(236, 240, 241) };
                    var btnClose = new Button
                    {
                        Text = "Đóng",
                        DialogResult = DialogResult.OK,
                        Width = 100,
                        Height = 35,
                        Location = new Point(dlg.ClientSize.Width - 110, 8),
                        BackColor = Color.FromArgb(149, 165, 166),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                    };
                    btnClose.Click += (s, ev) => dlg.Close();

                    // Edit button: opens NotificationEditForm to edit this notification
                    var btnEdit = new Button
                    {
                        Text = "Sửa",
                        Width = 100,
                        Height = 35,
                        Location = new Point(dlg.ClientSize.Width - 220, 8),
                        BackColor = Color.FromArgb(52, 152, 219),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                    };
                    btnEdit.Click += (s, ev) =>
                    {
                        try
                        {
                            var editData = new NotificationData
                            {
                                MaTB = maTb,
                                TieuDe = tieuDe,
                                NoiDung = noiDung,
                                LoaiThongBao = loaiThongBao,
                                NgayGui = ngayGui,
                                DoiTuong = doiTuong,
                                TrangThai = trangThai
                            };

                            using (var editForm = new NotificationEditForm(editData))
                            {
                                if (editForm.ShowDialog() == DialogResult.OK)
                                {
                                    var newData = editForm.NotificationData;
                                    // persist to DB
                                    try
                                    {
                                        using (var conn = new SqlConnection(connectionString))
                                        using (var cmd = new SqlCommand(@"UPDATE ThongBao SET TieuDe=@TieuDe, NoiDung=@NoiDung, LoaiThongBao=@Loai, NgayGui=@NgayGui, DoiTuong=@DoiTuong, TrangThai=@TrangThai WHERE MaTB=@MaTB", conn))
                                        {
                                            conn.Open();
                                            cmd.Parameters.AddWithValue("@MaTB", newData.MaTB);
                                            cmd.Parameters.AddWithValue("@TieuDe", newData.TieuDe ?? string.Empty);
                                            cmd.Parameters.AddWithValue("@NoiDung", newData.NoiDung ?? string.Empty);
                                            cmd.Parameters.AddWithValue("@Loai", string.IsNullOrEmpty(newData.LoaiThongBao) ? (object)DBNull.Value : newData.LoaiThongBao);
                                            cmd.Parameters.AddWithValue("@NgayGui", newData.NgayGui == DateTime.MinValue ? (object)DBNull.Value : newData.NgayGui);
                                            cmd.Parameters.AddWithValue("@DoiTuong", string.IsNullOrEmpty(newData.DoiTuong) ? (object)DBNull.Value : newData.DoiTuong);
                                            cmd.Parameters.AddWithValue("@TrangThai", string.IsNullOrEmpty(newData.TrangThai) ? (object)DBNull.Value : newData.TrangThai);
                                            cmd.ExecuteNonQuery();
                                        }

                                        AddLog($"✏️ Sửa (chi tiết): {newData.TieuDe}");

                                        // update in-memory table if available
                                        if (notificationsTable != null)
                                        {
                                            var found = notificationsTable.AsEnumerable().FirstOrDefault(r => r.Field<int>("MaTB") == newData.MaTB);
                                            if (found != null)
                                            {
                                                found.SetField("TieuDe", newData.TieuDe ?? string.Empty);
                                                found.SetField("NoiDung", newData.NoiDung ?? string.Empty);
                                                found.SetField("LoaiThongBao", newData.LoaiThongBao ?? string.Empty);
                                                found.SetField("NgayGui", newData.NgayGui == DateTime.MinValue ? (object)DBNull.Value : (object)newData.NgayGui);
                                                found.SetField("DoiTuong", newData.DoiTuong ?? string.Empty);
                                                found.SetField("TrangThai", newData.TrangThai ?? string.Empty);
                                            }
                                        }

                                        RefreshGrid();
                                        LoadFilterOptions();
                                        UpdateStats();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Lỗi khi lưu chỉnh sửa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi mở form sửa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };

                    pnlBottom.Controls.Add(btnEdit);
                    pnlBottom.Controls.Add(btnClose);
 
                     dlg.Controls.Add(pnlContent);
                     dlg.Controls.Add(pnlTop);
                     dlg.Controls.Add(pnlBottom);

                     dlg.ShowDialog();
                 }
             }
             catch (Exception ex)
             {
                 AddLog($"❌ Lỗi khi mở chi tiết: {ex.Message}");
                 MessageBox.Show($"Không thể hiển thị chi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
             }
         }

        private void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(cboPageSize.SelectedItem?.ToString() ?? string.Empty, out int size))
            {
                pageSize = size;
                currentPage = 1;
                RefreshGrid();
            }
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                RefreshGrid();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                RefreshGrid();
            }
        }

        private void dgvThongBao_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // intentionally left blank - required by designer
        }

        private void SetupGridColumns()
        {
            if (dgvThongBao == null) return;

            dgvThongBao.AllowUserToAddRows = false;
            dgvThongBao.AutoGenerateColumns = false;
            dgvThongBao.Columns.Clear();

            var colMa = new DataGridViewTextBoxColumn
            {
                Name = "MaTB",
                HeaderText = "Mã TB",
                Width = 80,
                ReadOnly = true,
                DataPropertyName = "MaTB"
            };
            dgvThongBao.Columns.Add(colMa);

            var colTieuDe = new DataGridViewTextBoxColumn
            {
                Name = "TieuDe",
                HeaderText = "Tiêu đề",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DataPropertyName = "TieuDe"
            };
            dgvThongBao.Columns.Add(colTieuDe);

            var colLoai = new DataGridViewTextBoxColumn
            {
                Name = "LoaiThongBao",
                HeaderText = "Loại",
                Width = 140,
                ReadOnly = true,
                DataPropertyName = "LoaiThongBao"
            };
            dgvThongBao.Columns.Add(colLoai);

            var colNgay = new DataGridViewTextBoxColumn
            {
                Name = "NgayGui",
                HeaderText = "Ngày gửi",
                Width = 150,
                ReadOnly = true,
                DataPropertyName = "NgayGui"
            };
            dgvThongBao.Columns.Add(colNgay);

            var colDoiTuong = new DataGridViewTextBoxColumn
            {
                Name = "DoiTuong",
                HeaderText = "Đối tượng",
                Width = 150,
                ReadOnly = true,
                DataPropertyName = "DoiTuong"
            };
            dgvThongBao.Columns.Add(colDoiTuong);

            var colTrangThai = new DataGridViewTextBoxColumn
            {
                Name = "TrangThai",
                HeaderText = "Trạng thái",
                Width = 120,
                ReadOnly = true,
                DataPropertyName = "TrangThai"
            };
            dgvThongBao.Columns.Add(colTrangThai);

            dgvThongBao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThongBao.MultiSelect = false;
        }

        private void dgvThongBao_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvThongBao.Rows.Count)
            {
                var drv = dgvThongBao.Rows[e.RowIndex].DataBoundItem as DataRowView;
                if (drv != null)
                {
                    txtMaTB.Text = drv["MaTB"]?.ToString() ?? string.Empty;
                    txtTieuDe.Text = drv["TieuDe"]?.ToString() ?? string.Empty;
                    txtDoiTuong.Text = drv["DoiTuong"]?.ToString() ?? string.Empty;

                    DateTime dt;
                    if (DateTime.TryParse(drv["NgayGui"]?.ToString(), out dt))
                        dtpNgayGui.Value = dt;
                    else
                        dtpNgayGui.Value = DateTime.Now;

                    // Set type and status into the top comboboxes (act as fields, not filters)
                    cboFilterType.Text = drv["LoaiThongBao"]?.ToString() ?? string.Empty;
                    cboFilterStatus.Text = drv["TrangThai"]?.ToString() ?? string.Empty;
                }
            }
        }

        private void cboFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            RefreshGrid();
        }

        private void cboFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            RefreshGrid();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtMaTB.Clear();
            txtTieuDe.Clear();
            txtDoiTuong.Clear();
            if (dtpNgayGui != null) dtpNgayGui.Value = DateTime.Now;
            if (cboFilterStatus != null && cboFilterStatus.Items.Count > 0) cboFilterStatus.SelectedIndex = 0;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaTB.Text))
            {
                MessageBox.Show("Vui lòng chọn thông báo để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc muốn xóa '{txtTieuDe.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            try
            {
                int idToDelete = int.Parse(txtMaTB.Text);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM ThongBao WHERE MaTB=@MaTB", conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTB", idToDelete);
                        cmd.ExecuteNonQuery();
                    }
                }

                if (notificationsTable != null && notificationsTable.Rows.Count > 0)
                {
                    var found = notificationsTable.AsEnumerable().FirstOrDefault(r => r.Field<int>("MaTB") == idToDelete);
                    if (found != null) notificationsTable.Rows.Remove(found);
                }

                MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AddLog($"🗑️ Xóa: {txtTieuDe.Text}");

                LoadFilterOptions();
                RefreshGrid();
                UpdateStats();
                btnClear_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            UpdateFromTopFields(sender, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            BtnAdd_OpenEditForm(sender, e);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadNotifications();
            LoadFilterOptions();
            RefreshGrid();
            UpdateStats();
            AddLog("🔄 Làm mới dữ liệu");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            RefreshGrid();
            AddLog($"🔍 Tìm kiếm: {txtSearch.Text}");
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            logTable.Clear();
            AddLog("🗑️ Xóa log");
        }

        private void dtpNgayGui_ValueChanged(object sender, EventArgs e)
        {

        }
    }

    // Data class for notification
    public class NotificationTestData
    {
        public int MaTB { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public string LoaiThongBao { get; set; }
        public DateTime NgayDang { get; set; }
        public DateTime NgayGui { get; set; }
        public int NguoiDang { get; set; }
        public string DoiTuong { get; set; }
        public string TrangThai { get; set; }
    }
}
