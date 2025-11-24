using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ClubManageApp
{
    public partial class Activity : UserControl
    {
        private string connectionString = @"Data Source=21AK22-COM;Initial Catalog=QL_CLB_LSC;Persist Security Info=True;User ID=sa;Password=912005;Encrypt=True;TrustServerCertificate=True";
        private BindingList<ActivityData> activities = new BindingList<ActivityData>();
        private BindingList<ActivityData> filteredActivities = new BindingList<ActivityData>();

        private Timer footerTimer;
        private Label lblFooterTime;
        private Label lblFooterCount;

        public Activity()
        {
            InitializeComponent();
        }

        private void Activity_Load(object sender, EventArgs e)
        {
            try
            {
                // Wire realtime events
                if (txtSearch != null) txtSearch.TextChanged += txtSearch_TextChanged;
                if (cboStatus != null) cboStatus.SelectedIndexChanged += (s, ev) => ApplyFiltersAndSearch();
                if (cboSortBy != null) cboSortBy.SelectedIndexChanged += (s, ev) => ApplyFiltersAndSearch();

                LoadActivities();
                WireGrid();
                SetupFiltersAndSearch();

                // Footer timer for realtime time and count
                footerTimer = new Timer { Interval = 1000 };
                footerTimer.Tick += FooterTimer_Tick;
                footerTimer.Start();

                // Try to find footer labels if Designer added them
                try
                {
                    var pnl = this.Controls.Find("pnlMain", true).FirstOrDefault() as Panel;
                    if (pnl != null)
                    {
                        var footer = pnl.Controls.Find("pnlFooter", true).FirstOrDefault() as Panel;
                        if (footer != null)
                        {
                            lblFooterTime = footer.Controls.Find("lblTimeFooter", true).FirstOrDefault() as Label;
                            lblFooterCount = footer.Controls.Find("lblCountFooter", true).FirstOrDefault() as Label;
                        }
                    }
                }
                catch { }

                UpdateFooter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải hoạt động: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FooterTimer_Tick(object sender, EventArgs e)
        {
            UpdateFooter();
        }

        private void UpdateFooter()
        {
            try
            {
                if (lblFooterTime != null)
                {
                    lblFooterTime.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
                }

                if (lblFooterCount != null)
                {
                    int visibleRows = dgvActivities?.Rows.Count ?? 0;
                    lblFooterCount.Text = $"Đang hiển thị: {visibleRows} dòng";
                }
            }
            catch { }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // realtime filter
            ApplyFiltersAndSearch();
        }

        private void LoadActivities()
        {
            activities.Clear();

            string query = @"
                SELECT 
                    MaHD,
                    TenHD,
                    NgayToChuc,
                    GioBatDau,
                    GioKetThuc,
                    DiaDiem,
                    MoTa,
                    MaLoaiHD,
                    NguoiPhuTrach,
                    KinhPhiDuKien,
                    KinhPhiThucTe,
                    SoLuongToiDa,
                    TrangThai
                FROM HoatDong
                ORDER BY NgayToChuc DESC";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                activities.Add(new ActivityData
                                {
                                    MaHD = (int)reader["MaHD"],
                                    TenHD = reader["TenHD"]?.ToString() ?? "",
                                    NgayToChuc = reader["NgayToChuc"] != DBNull.Value ? (DateTime)reader["NgayToChuc"] : DateTime.MinValue,
                                    GioBatDau = reader["GioBatDau"] != DBNull.Value ? reader["GioBatDau"].ToString() : "",
                                    GioKetThuc = reader["GioKetThuc"] != DBNull.Value ? reader["GioKetThuc"].ToString() : "",
                                    DiaDiem = reader["DiaDiem"]?.ToString() ?? "",
                                    MoTa = reader["MoTa"]?.ToString() ?? "",
                                    MaLoaiHD = reader["MaLoaiHD"] != DBNull.Value ? (int)reader["MaLoaiHD"] : 0,
                                    NguoiPhuTrach = reader["NguoiPhuTrach"] != DBNull.Value ? (int)reader["NguoiPhuTrach"] : 0,
                                    KinhPhiDuKien = reader["KinhPhiDuKien"] != DBNull.Value ? (decimal)reader["KinhPhiDuKien"] : 0,
                                    KinhPhiThucTe = reader["KinhPhiThucTe"] != DBNull.Value ? (decimal)reader["KinhPhiThucTe"] : 0,
                                    SoLuongToiDa = reader["SoLuongToiDa"] != DBNull.Value ? (int)reader["SoLuongToiDa"] : 0,
                                    TrangThai = reader["TrangThai"]?.ToString() ?? ""
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Lỗi cơ sở dữ liệu: {sqlEx.Message}", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireGrid()
        {
            dgvActivities.DataSource = activities;
            dgvActivities.AutoGenerateColumns = false;
            dgvActivities.Columns.Clear();

            // STT column (index)
            var colStt = new DataGridViewTextBoxColumn()
            {
                Name = "STT",
                HeaderText = "STT",
                Width = 50,
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            dgvActivities.Columns.Add(colStt);

            // Define columns
            var colTenHD = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof(ActivityData.TenHD),
                Name = "TenHD",
                HeaderText = "Tên hoạt động",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            var colNgayToChuc = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof(ActivityData.NgayToChuc),
                Name = "NgayToChuc",
                HeaderText = "Ngày tổ chức",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            };

            var colDiaDiem = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof(ActivityData.DiaDiem),
                Name = "DiaDiem",
                HeaderText = "Địa điểm",
                Width = 150
            };

            var colGioBatDau = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof(ActivityData.GioBatDau),
                Name = "GioBatDau",
                HeaderText = "Giờ bắt đầu",
                Width = 80
            };

            var colTrangThai = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof(ActivityData.TrangThai),
                Name = "TrangThai",
                HeaderText = "Trạng thái",
                Width = 120
            };

            var colKinhPhi = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof(ActivityData.KinhPhiDuKien),
                Name = "KinhPhiDuKien",
                HeaderText = "Kinh phí dự kiến",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            };

            dgvActivities.Columns.Add(colTenHD);
            dgvActivities.Columns.Add(colNgayToChuc);
            dgvActivities.Columns.Add(colDiaDiem);
            dgvActivities.Columns.Add(colGioBatDau);
            dgvActivities.Columns.Add(colTrangThai);
            dgvActivities.Columns.Add(colKinhPhi);

            dgvActivities.AllowUserToAddRows = false;
            dgvActivities.ReadOnly = true;
            dgvActivities.RowHeadersVisible = false;
            dgvActivities.DefaultCellStyle.BackColor = Color.White;
            dgvActivities.DefaultCellStyle.ForeColor = Color.Black;
            dgvActivities.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 250);
            dgvActivities.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(94, 148, 255);
            dgvActivities.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvActivities.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            // Make grid fill columns to container
            dgvActivities.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Events
            dgvActivities.DataBindingComplete += DgvActivities_DataBindingComplete;
        }

        private void DgvActivities_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Update row numbers
            try
            {
                for (int i = 0; i < dgvActivities.Rows.Count; i++)
                {
                    dgvActivities.Rows[i].Cells["STT"].Value = (i + 1).ToString();
                }
                UpdateFooter();
            }
            catch { }
        }

        private void SetupFiltersAndSearch()
        {
            // This will be wired in the Designer
        }

        // Button Events
        private void btnAddActivity_Click(object sender, EventArgs e)
        {
            var form = new ActivityEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                InsertActivity(form.ActivityData);
                LoadActivities();
                WireGrid();
                ApplyFiltersAndSearch();
            }
        }

        private void btnEditActivity_Click(object sender, EventArgs e)
        {
            if (dgvActivities.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hoạt động để chỉnh sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selected = (ActivityData)dgvActivities.CurrentRow.DataBoundItem;
            var form = new ActivityEditForm(selected);
            if (form.ShowDialog() == DialogResult.OK)
            {
                UpdateActivity(form.ActivityData);
                LoadActivities();
                WireGrid();
                ApplyFiltersAndSearch();
            }
        }

        private void btnDeleteActivity_Click(object sender, EventArgs e)
        {
            if (dgvActivities.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hoạt động để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selected = (ActivityData)dgvActivities.CurrentRow.DataBoundItem;
            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa hoạt động '{selected.TenHD}'?", 
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                DeleteActivity(selected.MaHD);
                LoadActivities();
                WireGrid();
                ApplyFiltersAndSearch();
            }
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvActivities.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hoạt động để xem chi tiết", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selected = (ActivityData)dgvActivities.CurrentRow.DataBoundItem;
            ShowDetailForm(selected);
        }

        private void dgvActivities_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selected = (ActivityData)dgvActivities.Rows[e.RowIndex].DataBoundItem;
                ShowDetailForm(selected);
            }
        }

        private void ShowDetailForm(ActivityData activity)
        {
            var detailForm = new ActivityDetailForm(activity);
            detailForm.ShowDialog();
        }

        // Filter and Search
        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyFiltersAndSearch();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                ApplyFiltersAndSearch();
                e.Handled = true;
            }
        }

        private void ApplyFiltersAndSearch()
        {
            filteredActivities.Clear();

            // Get filter values
            string searchText = txtSearch?.Text?.Trim().ToLower() ?? "";
            string selectedStatus = cboStatus?.SelectedItem?.ToString() ?? "";
            string sortBy = cboSortBy?.SelectedItem?.ToString() ?? "Ngày tổ chức (Mới nhất)";

            var query = activities.AsEnumerable();

            // Search filter
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(a => a.TenHD.ToLower().Contains(searchText) || 
                                         a.DiaDiem.ToLower().Contains(searchText) ||
                                         a.MoTa.ToLower().Contains(searchText));
            }

            // Status filter
            if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "Tất cả")
            {
                query = query.Where(a => a.TrangThai == selectedStatus);
            }

            // Sort
            if (sortBy == "Ngày tổ chức (Cũ nhất)")
                query = query.OrderBy(a => a.NgayToChuc);
            else if (sortBy == "Tên (A-Z)")
                query = query.OrderBy(a => a.TenHD);
            else if (sortBy == "Tên (Z-A)")
                query = query.OrderByDescending(a => a.TenHD);
            else if (sortBy == "Kinh phí (Cao nhất)")
                query = query.OrderByDescending(a => a.KinhPhiDuKien);
            else if (sortBy == "Kinh phí (Thấp nhất)")
                query = query.OrderBy(a => a.KinhPhiDuKien);
            else
                query = query.OrderByDescending(a => a.NgayToChuc);

            foreach (var activity in query)
            {
                filteredActivities.Add(activity);
            }

            dgvActivities.DataSource = filteredActivities;

            UpdateFooter();
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            if (txtSearch != null) txtSearch.Text = "";
            if (cboStatus != null) cboStatus.SelectedIndex = 0;
            if (cboSortBy != null) cboSortBy.SelectedIndex = 0;
            dgvActivities.DataSource = activities;
            UpdateFooter();
        }

        // Database Operations
        private void InsertActivity(ActivityData activity)
        {
            string query = @"
                INSERT INTO HoatDong (TenHD, NgayToChuc, GioBatDau, GioKetThuc, DiaDiem, MoTa, 
                                      MaLoaiHD, NguoiPhuTrach, KinhPhiDuKien, KinhPhiThucTe, 
                                      SoLuongToiDa, TrangThai)
                VALUES (@TenHD, @NgayToChuc, @GioBatDau, @GioKetThuc, @DiaDiem, @MoTa, 
                        @MaLoaiHD, @NguoiPhuTrach, @KinhPhiDuKien, @KinhPhiThucTe, 
                        @SoLuongToiDa, @TrangThai)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TenHD", activity.TenHD);
                        cmd.Parameters.AddWithValue("@NgayToChuc", activity.NgayToChuc == DateTime.MinValue ? DBNull.Value : (object)activity.NgayToChuc);
                        // GioBatDau / GioKetThuc: pass as SqlDbType.Time
                        if (string.IsNullOrEmpty(activity.GioBatDau))
                        {
                            cmd.Parameters.Add("@GioBatDau", SqlDbType.Time).Value = DBNull.Value;
                        }
                        else
                        {
                            TimeSpan t;
                            if (TimeSpan.TryParse(activity.GioBatDau, out t))
                                cmd.Parameters.Add("@GioBatDau", SqlDbType.Time).Value = t;
                            else if (DateTime.TryParse(activity.GioBatDau, out DateTime dt))
                                cmd.Parameters.Add("@GioBatDau", SqlDbType.Time).Value = dt.TimeOfDay;
                            else
                                cmd.Parameters.Add("@GioBatDau", SqlDbType.Time).Value = DBNull.Value;
                        }

                        if (string.IsNullOrEmpty(activity.GioKetThuc))
                        {
                            cmd.Parameters.Add("@GioKetThuc", SqlDbType.Time).Value = DBNull.Value;
                        }
                        else
                        {
                            TimeSpan t2;
                            if (TimeSpan.TryParse(activity.GioKetThuc, out t2))
                                cmd.Parameters.Add("@GioKetThuc", SqlDbType.Time).Value = t2;
                            else if (DateTime.TryParse(activity.GioKetThuc, out DateTime dt2))
                                cmd.Parameters.Add("@GioKetThuc", SqlDbType.Time).Value = dt2.TimeOfDay;
                            else
                                cmd.Parameters.Add("@GioKetThuc", SqlDbType.Time).Value = DBNull.Value;
                        }
                        cmd.Parameters.AddWithValue("@DiaDiem", activity.DiaDiem);
                        cmd.Parameters.AddWithValue("@MoTa", activity.MoTa);
                        cmd.Parameters.AddWithValue("@MaLoaiHD", activity.MaLoaiHD == 0 ? DBNull.Value : (object)activity.MaLoaiHD);
                        cmd.Parameters.AddWithValue("@NguoiPhuTrach", activity.NguoiPhuTrach == 0 ? DBNull.Value : (object)activity.NguoiPhuTrach);
                        cmd.Parameters.AddWithValue("@KinhPhiDuKien", activity.KinhPhiDuKien == 0 ? DBNull.Value : (object)activity.KinhPhiDuKien);
                        cmd.Parameters.AddWithValue("@KinhPhiThucTe", activity.KinhPhiThucTe == 0 ? DBNull.Value : (object)activity.KinhPhiThucTe);
                        cmd.Parameters.AddWithValue("@SoLuongToiDa", activity.SoLuongToiDa == 0 ? DBNull.Value : (object)activity.SoLuongToiDa);
                        cmd.Parameters.AddWithValue("@TrangThai", activity.TrangThai);

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Thêm hoạt động thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Lỗi cơ sở dữ liệu: {sqlEx.Message}", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateActivity(ActivityData activity)
        {
            string query = @"
                UPDATE HoatDong 
                SET TenHD = @TenHD, NgayToChuc = @NgayToChuc, GioBatDau = @GioBatDau, 
                    GioKetThuc = @GioKetThuc, DiaDiem = @DiaDiem, MoTa = @MoTa, 
                    MaLoaiHD = @MaLoaiHD, NguoiPhuTrach = @NguoiPhuTrach, 
                    KinhPhiDuKien = @KinhPhiDuKien, KinhPhiThucTe = @KinhPhiThucTe, 
                    SoLuongToiDa = @SoLuongToiDa, TrangThai = @TrangThai
                WHERE MaHD = @MaHD";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHD", activity.MaHD);
                        cmd.Parameters.AddWithValue("@TenHD", activity.TenHD);
                        cmd.Parameters.AddWithValue("@NgayToChuc", activity.NgayToChuc == DateTime.MinValue ? DBNull.Value : (object)activity.NgayToChuc);
                        // GioBatDau / GioKetThuc as SqlDbType.Time
                        if (string.IsNullOrEmpty(activity.GioBatDau))
                            cmd.Parameters.Add("@GioBatDau", SqlDbType.Time).Value = DBNull.Value;
                        else if (TimeSpan.TryParse(activity.GioBatDau, out TimeSpan t3))
                            cmd.Parameters.Add("@GioBatDau", SqlDbType.Time).Value = t3;
                        else if (DateTime.TryParse(activity.GioBatDau, out DateTime dt3))
                            cmd.Parameters.Add("@GioBatDau", SqlDbType.Time).Value = dt3.TimeOfDay;
                        else
                            cmd.Parameters.Add("@GioBatDau", SqlDbType.Time).Value = DBNull.Value;

                        if (string.IsNullOrEmpty(activity.GioKetThuc))
                            cmd.Parameters.Add("@GioKetThuc", SqlDbType.Time).Value = DBNull.Value;
                        else if (TimeSpan.TryParse(activity.GioKetThuc, out TimeSpan t4))
                            cmd.Parameters.Add("@GioKetThuc", SqlDbType.Time).Value = t4;
                        else if (DateTime.TryParse(activity.GioKetThuc, out DateTime dt4))
                            cmd.Parameters.Add("@GioKetThuc", SqlDbType.Time).Value = dt4.TimeOfDay;
                        else
                            cmd.Parameters.Add("@GioKetThuc", SqlDbType.Time).Value = DBNull.Value;
                        cmd.Parameters.AddWithValue("@DiaDiem", activity.DiaDiem);
                        cmd.Parameters.AddWithValue("@MoTa", activity.MoTa);
                        cmd.Parameters.AddWithValue("@MaLoaiHD", activity.MaLoaiHD == 0 ? DBNull.Value : (object)activity.MaLoaiHD);
                        cmd.Parameters.AddWithValue("@NguoiPhuTrach", activity.NguoiPhuTrach == 0 ? DBNull.Value : (object)activity.NguoiPhuTrach);
                        cmd.Parameters.AddWithValue("@KinhPhiDuKien", activity.KinhPhiDuKien == 0 ? DBNull.Value : (object)activity.KinhPhiDuKien);
                        cmd.Parameters.AddWithValue("@KinhPhiThucTe", activity.KinhPhiThucTe == 0 ? DBNull.Value : (object)activity.KinhPhiThucTe);
                        cmd.Parameters.AddWithValue("@SoLuongToiDa", activity.SoLuongToiDa == 0 ? DBNull.Value : (object)activity.SoLuongToiDa);
                        cmd.Parameters.AddWithValue("@TrangThai", activity.TrangThai);

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Cập nhật hoạt động thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Lỗi cơ sở dữ liệu: {sqlEx.Message}", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteActivity(int maHD)
        {
            string query = "DELETE FROM HoatDong WHERE MaHD = @MaHD";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHD", maHD);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Xóa hoạt động thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Lỗi cơ sở dữ liệu: {sqlEx.Message}", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvActivities_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void dgvActivities_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblSubtitle_Click(object sender, EventArgs e)
        {

        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    // Data model for Activity
    public class ActivityData
    {
        public int MaHD { get; set; }
        public string TenHD { get; set; }
        public DateTime NgayToChuc { get; set; }
        public string GioBatDau { get; set; }
        public string GioKetThuc { get; set; }
        public string DiaDiem { get; set; }
        public string MoTa { get; set; }
        public int MaLoaiHD { get; set; }
        public int NguoiPhuTrach { get; set; }
        public decimal KinhPhiDuKien { get; set; }
        public decimal KinhPhiThucTe { get; set; }
        public int SoLuongToiDa { get; set; }
        public string TrangThai { get; set; }
    }
}
