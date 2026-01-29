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
    public partial class Activity : UserControl
    {
        private string connectionString = ConnectionHelper.ConnectionString;
        private BindingList<ActivityData> activities = new BindingList<ActivityData>();
        private DataTable activitiesTable = null;
        private DataTable logTable = new DataTable();
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalPages = 1;

        public Activity()
        {
            InitializeComponent();
        }

        private void Activity_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeLogTable();
                LoadActivities();
                LoadStatuses();
                // Make MaHD read-only; other top fields editable for inline edit
                if (txtMaHD != null) txtMaHD.ReadOnly = true;
                if (txtTenHD != null) txtTenHD.ReadOnly = false;
                if (txtDiaDiem != null) txtDiaDiem.ReadOnly = false;
                if (dtpNgayToChuc != null) dtpNgayToChuc.Enabled = true;
                if (cboStatus != null) cboStatus.Enabled = true;

                UpdateStats();
                SetupGridColumns();
                RefreshGrid();
                AddLog("🚀 Ứng dụng tải thành công");

                // wire double-click to open detail
                this.dgvHoatDong.DoubleClick += DgvHoatDong_DoubleClick;
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

        private void LoadActivities()
        {
            // Load activities into a DataTable from DB and keep for binding/filtering
            activitiesTable = new DataTable();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("SELECT MaHD, TenHD, NgayToChuc, GioBatDau, GioKetThuc, DiaDiem, MoTa, TrangThai, MaLoaiHD, NguoiPhuTrach, KinhPhiDuKien, KinhPhiThucTe, SoLuongToiDa FROM HoatDong ORDER BY NgayToChuc DESC", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    conn.Open();
                    da.Fill(activitiesTable);
                }
            }
            catch (Exception ex)
            {
                AddLog($"❌ Lỗi tải: {ex.Message}");
                activitiesTable = new DataTable();
            }
        }



        private void LoadStatuses()
        {
            cboStatus.Items.Clear();
            cboStatus.Items.AddRange(new[] { "Tất cả", "Đang chuẩn bị", "Đang diễn ra", "Hoàn thành", "Hủy bỏ" });
            cboStatus.SelectedIndex = 0;
        }

        private void RefreshGrid()
        {
            // Use activitiesTable as source. Apply filters then paging and bind
            List<DataRow> rows = new List<DataRow>();
            if (activitiesTable == null || activitiesTable.Rows.Count == 0)
            {
                dgvHoatDong.DataSource = null;
            }
            else
            {
                string rawSearch = txtSearch.Text.Trim();
                List<string> terms = new List<string>();
                if (!string.IsNullOrEmpty(rawSearch))
                {
                    // split by spaces, ignore empty
                    terms = rawSearch.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList();
                }

                rows = activitiesTable.AsEnumerable()
                    .Where(r =>
                    {
                        if (terms.Count == 0)
                            return true; // no search

                        // prepare row fields
                        string ten = (r.Field<string>("TenHD") ?? string.Empty).ToLower();
                        string dia = (r.Field<string>("DiaDiem") ?? string.Empty).ToLower();
                        string trang = (r.Field<string>("TrangThai") ?? string.Empty).ToLower();
                        string mota = (r.Field<string>("MoTa") ?? string.Empty).ToLower();
                        string loai = (r.Table.Columns.Contains("TenLoaiHD") ? (r.Field<string>("TenLoaiHD") ?? string.Empty) : string.Empty).ToLower();

                        string ngayStr = string.Empty;
                        if (r.Table.Columns.Contains("NgayToChuc") && r["NgayToChuc"] != DBNull.Value)
                        {
                            DateTime d = Convert.ToDateTime(r["NgayToChuc"]);
                            ngayStr = d.ToString("dd/MM/yyyy");
                        }

                        decimal kinh = 0m;
                        if (r.Table.Columns.Contains("KinhPhiDuKien") && r["KinhPhiDuKien"] != DBNull.Value)
                            kinh = Convert.ToDecimal(r["KinhPhiDuKien"]);

                        int ma = r.Table.Columns.Contains("MaHD") && r["MaHD"] != DBNull.Value ? Convert.ToInt32(r["MaHD"]) : 0;
                        int soLuong = r.Table.Columns.Contains("SoLuongToiDa") && r["SoLuongToiDa"] != DBNull.Value ? Convert.ToInt32(r["SoLuongToiDa"]) : 0;

                        // every term must match at least one field (AND semantics)
                        foreach (var termRaw in terms)
                        {
                            var term = termRaw.ToLower();

                            bool matched = false;

                            // exact/int match for MaHD or SoLuong
                            if (int.TryParse(term, out int n))
                            {
                                if (n == ma || n == soLuong)
                                {
                                    matched = true;
                                }
                            }

                            // try decimal match for kinh phi (allow formatted like 1.000.000 or 1000000)
                            if (!matched)
                            {
                                var norm = term.Replace(",", string.Empty).Replace(".", string.Empty).Replace("vnđ", string.Empty).Replace("vnd", string.Empty).Trim();
                                if (decimal.TryParse(norm, out decimal kd))
                                {
                                    if (kd == kinh)
                                        matched = true;
                                }
                            }

                            // try date match
                            if (!matched)
                            {
                                if (DateTime.TryParse(termRaw, out DateTime dtTerm))
                                {
                                    if (r.Table.Columns.Contains("NgayToChuc") && r["NgayToChuc"] != DBNull.Value)
                                    {
                                        DateTime rowDate = Convert.ToDateTime(r["NgayToChuc"]).Date;
                                        if (rowDate == dtTerm.Date) matched = true;
                                    }
                                }
                            }

                            // substring matches against textual fields
                            if (!matched)
                            {
                                if (ten.Contains(term) || dia.Contains(term) || trang.Contains(term) || mota.Contains(term) || loai.Contains(term) || ngayStr.Contains(term))
                                    matched = true;
                            }

                            if (!matched)
                                return false; // this term didn't match any field -> exclude row
                        }

                        return true;
                    })
                    .ToList();

                totalPages = Math.Max(1, (int)Math.Ceiling((double)rows.Count / pageSize));
                if (currentPage > totalPages) currentPage = totalPages;

                var paged = rows.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                DataTable bindTable;
                if (paged.Count > 0)
                    bindTable = paged.CopyToDataTable();
                else
                    bindTable = activitiesTable.Clone();

                dgvHoatDong.DataSource = bindTable;
            }

            int totalFiltered = rows?.Count ?? 0;
            lblPageInfo.Text = $"Trang {currentPage} / {totalPages} (Tổng: {totalFiltered} hoạt động)";
        }

        private void UpdateStats()
        {
            if (activitiesTable == null)
            {
                lblTongHoatDong.Text = "0";
                lblDangDienRa.Text = "Đang diễn ra: 0";
                lblHoanThanh.Text = "Hoàn thành: 0";
                lblHuyBo.Text = "Hủy bỏ: 0";
                return;
            }

            int total = activitiesTable.Rows.Count;
            int dangDienRa = activitiesTable.AsEnumerable().Count(r => (r.Field<string>("TrangThai") ?? "") == "Đang diễn ra");
            int hoanThanh = activitiesTable.AsEnumerable().Count(r => (r.Field<string>("TrangThai") ?? "") == "Hoàn thành");
            int huyBo = activitiesTable.AsEnumerable().Count(r => (r.Field<string>("TrangThai") ?? "") == "Hủy bỏ");

            lblTongHoatDong.Text = total.ToString();
            lblDangDienRa.Text = $"Đang diễn ra: {dangDienRa}";
            lblHoanThanh.Text = $"Hoàn thành: {hoanThanh}";
            lblHuyBo.Text = $"Hủy bỏ: {huyBo}";
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

        // Open edit form for creating new activity
        private void BtnAdd_OpenEditForm(object sender, EventArgs e)
        {
            var editForm = new ActivityEditForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                var data = editForm.ActivityData;
                // insert new record using returned ActivityData
                try
                {
                    using (var conn = new SqlConnection(connectionString))
                    using (var cmd = new SqlCommand(@"INSERT INTO HoatDong (TenHD, NgayToChuc, GioBatDau, GioKetThuc, DiaDiem, MoTa, MaLoaiHD, NguoiPhuTrach, KinhPhiDuKien, KinhPhiThucTe, SoLuongToiDa, TrangThai) 
                                                     VALUES (@TenHD,@NgayToChuc,@GioBatDau,@GioKetThuc,@DiaDiem,@MoTa,@MaLoaiHD,@NguoiPhuTrach,@KinhPhiDuKien,@KinhPhiThucTe,@SoLuongToiDa,@TrangThai)", conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@TenHD", data.TenHD ?? string.Empty);
                        cmd.Parameters.AddWithValue("@NgayToChuc", data.NgayToChuc == DateTime.MinValue ? (object)DBNull.Value : data.NgayToChuc);
                        cmd.Parameters.AddWithValue("@GioBatDau", string.IsNullOrEmpty(data.GioBatDau) ? (object)DBNull.Value : data.GioBatDau);
                        cmd.Parameters.AddWithValue("@GioKetThuc", string.IsNullOrEmpty(data.GioKetThuc) ? (object)DBNull.Value : data.GioKetThuc);
                        cmd.Parameters.AddWithValue("@DiaDiem", data.DiaDiem ?? string.Empty);
                        cmd.Parameters.AddWithValue("@MoTa", data.MoTa ?? string.Empty);
                        cmd.Parameters.AddWithValue("@MaLoaiHD", data.MaLoaiHD == 0 ? (object)DBNull.Value : data.MaLoaiHD);
                        cmd.Parameters.AddWithValue("@NguoiPhuTrach", data.NguoiPhuTrach == 0 ? (object)DBNull.Value : data.NguoiPhuTrach);
                        cmd.Parameters.AddWithValue("@KinhPhiDuKien", data.KinhPhiDuKien == 0 ? (object)DBNull.Value : data.KinhPhiDuKien);
                        cmd.Parameters.AddWithValue("@KinhPhiThucTe", data.KinhPhiThucTe == 0 ? (object)DBNull.Value : data.KinhPhiThucTe);
                        cmd.Parameters.AddWithValue("@SoLuongToiDa", data.SoLuongToiDa == 0 ? (object)DBNull.Value : data.SoLuongToiDa);
                        cmd.Parameters.AddWithValue("@TrangThai", data.TrangThai ?? "Đang chuẩn bị");
                        cmd.ExecuteNonQuery();
                    }
                    AddLog($"✅ Thêm: {data.TenHD}");
                    LoadActivities();
                    RefreshGrid();
                    UpdateStats();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddLog($"❌ Lỗi thêm: {ex.Message}");
                }
            }
        }

        // Update selected activity directly from top fields
        private void UpdateFromTopFields(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHD.Text))
            {
                MessageBox.Show("Vui lòng chọn hoạt động để sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int ma;
            if (!int.TryParse(txtMaHD.Text, out ma))
            {
                MessageBox.Show("Mã hoạt động không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // find the row in activitiesTable (grid is bound to this table)
            if (activitiesTable == null)
            {
                MessageBox.Show("Dữ liệu hoạt động chưa được tải.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var rowToUpdate = activitiesTable.AsEnumerable().FirstOrDefault(r => r.Field<int>("MaHD") == ma);
            if (rowToUpdate == null)
            {
                MessageBox.Show("Không tìm thấy hoạt động đã chọn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // read values from top fields
            string ten = txtTenHD.Text.Trim();
            DateTime ngay = dtpNgayToChuc.Value.Date;
            string dia = txtDiaDiem.Text.Trim();
            string trang = cboStatus.SelectedItem?.ToString() ?? cboStatus.Text;
            decimal kinhPhi = 0m;
            // try parse kinh phi from a top field if exists (nud or textbox named txtKinhPhi)
            if (this.Controls.Find("txtKinhPhi", true).FirstOrDefault() is TextBox tbKp)
            {
                decimal.TryParse(tbKp.Text, out kinhPhi);
            }
            else if (this.Controls.Find("nudKinhPhi", true).FirstOrDefault() is NumericUpDown nud)
            {
                kinhPhi = nud.Value;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(@"UPDATE HoatDong SET TenHD=@TenHD, NgayToChuc=@NgayToChuc, DiaDiem=@DiaDiem, TrangThai=@TrangThai, KinhPhiDuKien=@KinhPhiDuKien WHERE MaHD=@MaHD", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@TenHD", ten);
                    cmd.Parameters.AddWithValue("@NgayToChuc", ngay == DateTime.MinValue ? (object)DBNull.Value : ngay);
                    cmd.Parameters.AddWithValue("@DiaDiem", dia);
                    cmd.Parameters.AddWithValue("@TrangThai", trang ?? "Đang chuẩn bị");
                    cmd.Parameters.AddWithValue("@KinhPhiDuKien", kinhPhi);
                    cmd.Parameters.AddWithValue("@MaHD", ma);
                    cmd.ExecuteNonQuery();
                }

                AddLog($"✏️ Sửa: {ten}");
                // update activitiesTable (bound to grid)
                rowToUpdate.SetField("TenHD", ten);
                rowToUpdate.SetField("NgayToChuc", ngay);
                rowToUpdate.SetField("DiaDiem", dia);
                rowToUpdate.SetField("TrangThai", trang);
                if (activitiesTable.Columns.Contains("KinhPhiDuKien"))
                    rowToUpdate.SetField("KinhPhiDuKien", kinhPhi);

                // also update in-memory list if present
                var current = activities.FirstOrDefault(a => a.MaHD == ma);
                if (current != null)
                {
                    current.TenHD = ten;
                    current.NgayToChuc = ngay;
                    current.DiaDiem = dia;
                    current.TrangThai = trang;
                    current.KinhPhiDuKien = kinhPhi;
                }

                RefreshGrid();
                UpdateStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddLog($"❌ Lỗi cập nhật: {ex.Message}");
            }
        }

        // Open detail form on double-click
        private void DgvHoatDong_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (dgvHoatDong.CurrentRow == null) return;
                var drv = dgvHoatDong.CurrentRow.DataBoundItem as DataRowView;
                if (drv == null) return;

                // Safely read fields (handle DBNull and TIME -> TimeSpan)
                object objMa = drv.Row["MaHD"];
                object objTen = drv.Row["TenHD"];
                object objNgay = drv.Row["NgayToChuc"];
                object objGioBat = drv.Row["GioBatDau"];
                object objGioKet = drv.Row["GioKetThuc"];
                object objDia = drv.Row["DiaDiem"];
                object objMoTa = drv.Row["MoTa"];
                object objTrang = drv.Row["TrangThai"];
                object objMaLoai = drv.Row["MaLoaiHD"];
                object objNguoi = drv.Row["NguoiPhuTrach"];
                object objKinhDuKien = drv.Row["KinhPhiDuKien"];
                object objKinhThucTe = drv.Row["KinhPhiThucTe"];
                object objSoLuong = drv.Row["SoLuongToiDa"];

                int maHd = objMa != DBNull.Value ? Convert.ToInt32(objMa) : 0;
                string tenHd = objTen != DBNull.Value ? objTen.ToString() : string.Empty;

                DateTime ngayToChuc = DateTime.MinValue;
                if (objNgay != DBNull.Value)
                {
                    DateTime tmp;
                    if (DateTime.TryParse(objNgay.ToString(), out tmp)) ngayToChuc = tmp;
                }

                string gioBat = null;
                if (objGioBat != DBNull.Value && objGioBat != null)
                {
                    if (objGioBat is TimeSpan ts1) gioBat = ts1.ToString(@"hh\:mm\:ss");
                    else gioBat = objGioBat.ToString();
                }

                string gioKet = null;
                if (objGioKet != DBNull.Value && objGioKet != null)
                {
                    if (objGioKet is TimeSpan ts2) gioKet = ts2.ToString(@"hh\:mm\:ss");
                    else gioKet = objGioKet.ToString();
                }

                string diaDiem = objDia != DBNull.Value ? objDia.ToString() : string.Empty;
                string moTa = objMoTa != DBNull.Value ? objMoTa.ToString() : string.Empty;
                string trangThai = objTrang != DBNull.Value ? objTrang.ToString() : string.Empty;

                int maLoai = objMaLoai != DBNull.Value && objMaLoai != null ? Convert.ToInt32(objMaLoai) : 0;
                int nguoiPhuTrach = objNguoi != DBNull.Value && objNguoi != null ? Convert.ToInt32(objNguoi) : 0;
                decimal kinhDuKien = objKinhDuKien != DBNull.Value && objKinhDuKien != null ? Convert.ToDecimal(objKinhDuKien) : 0m;
                decimal kinhThucTe = objKinhThucTe != DBNull.Value && objKinhThucTe != null ? Convert.ToDecimal(objKinhThucTe) : 0m;
                int soLuong = objSoLuong != DBNull.Value && objSoLuong != null ? Convert.ToInt32(objSoLuong) : 0;

                var model = new ActivityData
                {
                    MaHD = maHd,
                    TenHD = tenHd,
                    NgayToChuc = ngayToChuc,
                    GioBatDau = gioBat,
                    GioKetThuc = gioKet,
                    DiaDiem = diaDiem,
                    MoTa = moTa,
                    TrangThai = trangThai,
                    MaLoaiHD = maLoai,
                    NguoiPhuTrach = nguoiPhuTrach,
                    KinhPhiDuKien = kinhDuKien,
                    KinhPhiThucTe = kinhThucTe,
                    SoLuongToiDa = soLuong
                };

                using (var detail = new ActivityDetailForm(model))
                {
                    // when detail form saves, refresh list in parent
                    detail.ActivityUpdated += (s2, e2) =>
                    {
                        LoadActivities();
                        RefreshGrid();
                        UpdateStats();
                    };

                    detail.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                AddLog($"❌ Lỗi khi mở chi tiết: {ex.Message}");
                MessageBox.Show($"Không thể hiển thị chi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // kept for designer compatibility - actual handler wired to BtnAdd_OpenEditForm
            BtnAdd_OpenEditForm(sender, e);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateFromTopFields(sender, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHD.Text))
            {
                MessageBox.Show("Vui lòng chọn hoạt động để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc muốn xóa '{txtTenHD.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM HoatDong WHERE MaHD=@MaHD", conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHD", int.Parse(txtMaHD.Text));
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AddLog($"🗑️ Xóa: {txtTenHD.Text}");
                LoadActivities();
                RefreshGrid();
                UpdateStats();
                btnClear_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtMaHD.Clear();
            txtTenHD.Clear();
            txtDiaDiem.Clear();
            dtpNgayToChuc.Value = DateTime.Now;
            cboStatus.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            RefreshGrid();
            AddLog($"🔍 Tìm kiếm: {txtSearch.Text}");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadActivities();
            RefreshGrid();
            UpdateStats();
            AddLog("🔄 Làm mới dữ liệu");
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            logTable.Clear();
            AddLog("🗑️ Xóa log");
        }

        private void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(cboPageSize.SelectedItem?.ToString() ?? "10", out int size))
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

        private void dgvHoatDong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvHoatDong.Rows.Count)
            {
                var drv = dgvHoatDong.Rows[e.RowIndex].DataBoundItem as DataRowView;
                if (drv != null)
                {
                    txtMaHD.Text = drv["MaHD"].ToString();
                    txtTenHD.Text = drv["TenHD"].ToString();
                    txtDiaDiem.Text = drv["DiaDiem"].ToString();
                    // set kinh phi control if present (NumericUpDown named nudKinhPhiDuKien or nudKinhPhi)
                    var ctrl = this.Controls.Find("nudKinhPhiDuKien", true).FirstOrDefault();
                    if (ctrl is NumericUpDown nudCtrl)
                    {
                        nudCtrl.Value = Convert.ToDecimal(drv["KinhPhiDuKien"] ?? 0);
                    }
                    else
                    {
                        var alt = this.Controls.Find("nudKinhPhi", true).FirstOrDefault();
                        if (alt is NumericUpDown nudAlt)
                            nudAlt.Value = Convert.ToDecimal(drv["KinhPhiDuKien"] ?? 0);
                        else
                        {
                            // try textbox
                            var tb = this.Controls.Find("txtKinhPhi", true).FirstOrDefault() as TextBox;
                            if (tb != null)
                                tb.Text = Convert.ToDecimal(drv["KinhPhiDuKien"] ?? 0).ToString("N0");
                        }
                    }

                    DateTime dt;
                    if (DateTime.TryParse(drv["NgayToChuc"]?.ToString(), out dt))
                        dtpNgayToChuc.Value = dt;
                    else
                        dtpNgayToChuc.Value = DateTime.Now;

                    var trang = drv["TrangThai"]?.ToString() ?? string.Empty;
                    for (int i = 0; i < cboStatus.Items.Count; i++)
                    {
                        if (cboStatus.Items[i]?.ToString() == trang)
                        {
                            cboStatus.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            BtnAdd_OpenEditForm(sender, e);
        }

        private void SetupGridColumns()
        {
            if (dgvHoatDong == null) return;

            dgvHoatDong.AllowUserToAddRows = false;
            dgvHoatDong.AutoGenerateColumns = false;
            dgvHoatDong.Columns.Clear();

            var colMa = new DataGridViewTextBoxColumn
            {
                Name = "MaHD",
                HeaderText = "Mã HĐ",
                Width = 80,
                ReadOnly = true
            };
            colMa.DataPropertyName = "MaHD";
            dgvHoatDong.Columns.Add(colMa);

            var colTen = new DataGridViewTextBoxColumn
            {
                Name = "TenHD",
                HeaderText = "Tên hoạt động",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            colTen.DataPropertyName = "TenHD";
            dgvHoatDong.Columns.Add(colTen);

            var colNgay = new DataGridViewTextBoxColumn
            {
                Name = "NgayToChuc",
                HeaderText = "Ngày tổ chức",
                Width = 120,
                ReadOnly = true
            };
            colNgay.DataPropertyName = "NgayToChuc";
            dgvHoatDong.Columns.Add(colNgay);

            var colDia = new DataGridViewTextBoxColumn
            {
                Name = "DiaDiem",
                HeaderText = "Địa điểm",
                Width = 220,
                ReadOnly = true
            };
            colDia.DataPropertyName = "DiaDiem";
            dgvHoatDong.Columns.Add(colDia);

            var colTrang = new DataGridViewTextBoxColumn
            {
                Name = "TrangThai",
                HeaderText = "Trạng thái",
                Width = 140,
                ReadOnly = true
            };
            colTrang.DataPropertyName = "TrangThai";
            dgvHoatDong.Columns.Add(colTrang);

            var colKinh = new DataGridViewTextBoxColumn
            {
                Name = "KinhPhiDuKien",
                HeaderText = "Kinh phí",
                Width = 120,
                ReadOnly = true
            };
            colKinh.DataPropertyName = "KinhPhiDuKien";
            // display with thousand separators (no currency symbol). Use "C0" to include currency symbol if desired.
            colKinh.DefaultCellStyle.Format = "N0";
            colKinh.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvHoatDong.Columns.Add(colKinh);

            dgvHoatDong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoatDong.MultiSelect = false;
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            // designer may wire this; forward to inline update
            UpdateFromTopFields(sender, e);
        }

        private void lblCategory_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblLog_Click(object sender, EventArgs e)
        {

        }
    }
}
