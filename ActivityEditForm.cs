using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace ClubManageApp
{
    public partial class ActivityEditForm : Form
    {
        // ✅ SỬ DỤNG ConnectionHelper thay vì hard-code
        private string connectionString = ConnectionHelper.ConnectionString;

        public ActivityData ActivityData { get; set; }

        public ActivityEditForm()
        {
            InitializeComponent();
            ActivityData = new ActivityData { TrangThai = "Đang chuẩn bị" };

            // load lookup data
            LoadLoaiHoatDong();
        }

        public ActivityEditForm(ActivityData data) : this()
        {
            ActivityData = data ?? new ActivityData { TrangThai = "Đang chuẩn bị" };
            LoadFormData();
        }

        private void LoadLoaiHoatDong()
        {
            try
            {
                var dt = new DataTable();
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("SELECT MaLoaiHD, TenLoaiHD FROM LoaiHoatDong ORDER BY TenLoaiHD", conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    conn.Open();
                    da.Fill(dt);
                }

                cboLoaiHD.DisplayMember = "TenLoaiHD";
                cboLoaiHD.ValueMember = "MaLoaiHD";
                cboLoaiHD.DataSource = dt;

                // add a default "Tất cả"/empty option? Not necessary for edit form.
            }
            catch (Exception ex)
            {
                // ignore but allow form to work
                MessageBox.Show($"Không thể tải danh sách loại hoạt động: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadFormData()
        {
            if (ActivityData == null) return;

            txtTenHD.Text = ActivityData.TenHD;
            dtpNgayToChuc.Value = ActivityData.NgayToChuc == DateTime.MinValue ? DateTime.Today : ActivityData.NgayToChuc;

            // parse time strings safely, default to 00:00 if parse fails
            DateTime tmp;
            if (DateTime.TryParse(ActivityData.GioBatDau, out tmp))
            {
                dtpGioBatDau.Value = DateTime.Today.Date + tmp.TimeOfDay;
            }
            else if (TimeSpan.TryParse(ActivityData.GioBatDau, out var ts1))
            {
                dtpGioBatDau.Value = DateTime.Today.Date + ts1;
            }
            else
            {
                dtpGioBatDau.Value = DateTime.Today.Date.AddHours(8);
            }

            if (DateTime.TryParse(ActivityData.GioKetThuc, out tmp))
            {
                dtpGioKetThuc.Value = DateTime.Today.Date + tmp.TimeOfDay;
            }
            else if (TimeSpan.TryParse(ActivityData.GioKetThuc, out var ts2))
            {
                dtpGioKetThuc.Value = DateTime.Today.Date + ts2;
            }
            else
            {
                dtpGioKetThuc.Value = DateTime.Today.Date.AddHours(17);
            }

            txtDiaDiem.Text = ActivityData.DiaDiem;
            txtMoTa.Text = ActivityData.MoTa;

            // clamp numeric values to allowed ranges
            try { nudKinhPhiDuKien.Value = Math.Min(nudKinhPhiDuKien.Maximum, Math.Max(nudKinhPhiDuKien.Minimum, ActivityData.KinhPhiDuKien)); } catch { }
            try { nudSoLuongToiDa.Value = Math.Min(nudSoLuongToiDa.Maximum, Math.Max(nudSoLuongToiDa.Minimum, ActivityData.SoLuongToiDa)); } catch { }

            cboTrangThai.SelectedItem = ActivityData.TrangThai;

            try { nudNguoiPhuTrach.Value = Math.Min(nudNguoiPhuTrach.Maximum, Math.Max(nudNguoiPhuTrach.Minimum, ActivityData.NguoiPhuTrach)); } catch { }

            // select LoaiHD by MaLoaiHD if available
            if (ActivityData.MaLoaiHD > 0 && cboLoaiHD.DataSource != null)
            {
                try
                {
                    cboLoaiHD.SelectedValue = ActivityData.MaLoaiHD;
                }
                catch { }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenHD.Text))
            {
                MessageBox.Show("Vui lòng nhập tên hoạt động", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // validate times: ensure end > start
            var startTime = dtpGioBatDau.Value.TimeOfDay;
            var endTime = dtpGioKetThuc.Value.TimeOfDay;
            if (endTime <= startTime)
            {
                MessageBox.Show("Giờ kết thúc phải lớn hơn giờ bắt đầu", "Lỗi thời gian", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ActivityData.TenHD = txtTenHD.Text.Trim();
            ActivityData.NgayToChuc = dtpNgayToChuc.Value.Date;

            // store times as HH:mm:ss for DB TIME compatibility
            ActivityData.GioBatDau = dtpGioBatDau.Value.ToString("HH:mm:ss");
            ActivityData.GioKetThuc = dtpGioKetThuc.Value.ToString("HH:mm:ss");

            ActivityData.DiaDiem = txtDiaDiem.Text.Trim();
            ActivityData.MoTa = txtMoTa.Text.Trim();
            ActivityData.KinhPhiDuKien = nudKinhPhiDuKien.Value;
            ActivityData.SoLuongToiDa = (int)nudSoLuongToiDa.Value;
            ActivityData.TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Đang chuẩn bị";
            ActivityData.NguoiPhuTrach = (int)nudNguoiPhuTrach.Value;

            // Map selected LoaiHD name to MaLoaiHD
            if (cboLoaiHD.SelectedValue != null && int.TryParse(cboLoaiHD.SelectedValue.ToString(), out int maLoai))
            {
                ActivityData.MaLoaiHD = maLoai;
            }
            else
            {
                ActivityData.MaLoaiHD = 0;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void lblKinhPhi_Click(object sender, EventArgs e)
        {

        }

        private void dtpNgayToChuc_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nudSoLuongToiDa_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nudNguoiPhuTrach_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nudKinhPhiDuKien_ValueChanged(object sender, EventArgs e)
        {

        }

        private void lblTrangThai_Click(object sender, EventArgs e)
        {

        }

        private void ActivityEditForm_Load(object sender, EventArgs e)
        {

        }
    }
}
