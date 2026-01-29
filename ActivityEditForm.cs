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
            ApplyFormStyling();
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

        // Validation methods
        private bool ValidateInput()
        {
            // Validate Activity Name
            if (string.IsNullOrWhiteSpace(txtTenHD.Text))
            {
                MessageBox.Show("Tên hoạt động không được để trống", "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenHD.Focus();
                return false;
            }

            if (txtTenHD.Text.Length > 255)
            {
                MessageBox.Show("Tên hoạt động không được vượt quá 255 ký tự", "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenHD.Focus();
                return false;
            }

            // Validate date - must be today or future (unless status is completed/cancelled)
            DateTime today = DateTime.Today;
            if (dtpNgayToChuc.Value.Date < today)
            {
                // Allow past dates only for completed or cancelled activities
                string selectedStatus = cboTrangThai.SelectedItem?.ToString() ?? "Đang chuẩn bị";
                if (selectedStatus != "Hoàn thành" && selectedStatus != "Hủy bỏ")
                {
                    MessageBox.Show("Ngày tổ chức không được là quá khứ (trừ hoạt động hoàn thành/hủy)", "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpNgayToChuc.Focus();
                    return false;
                }
            }

            // Validate times
            var startTime = dtpGioBatDau.Value.TimeOfDay;
            var endTime = dtpGioKetThuc.Value.TimeOfDay;

            if (endTime <= startTime)
            {
                MessageBox.Show("Giờ kết thúc phải lớn hơn giờ bắt đầu", "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpGioKetThuc.Focus();
                return false;
            }

            // Validate location
            if (string.IsNullOrWhiteSpace(txtDiaDiem.Text))
            {
                MessageBox.Show("Địa điểm không được để trống", "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaDiem.Focus();
                return false;
            }

            if (txtDiaDiem.Text.Length > 255)
            {
                MessageBox.Show("Địa điểm không được vượt quá 255 ký tự", "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaDiem.Focus();
                return false;
            }

            // Validate budget (must be positive and formatted as VND)
            if (nudKinhPhiDuKien.Value < 0)
            {
                MessageBox.Show("Kinh phí dự kiến không được âm", "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudKinhPhiDuKien.Focus();
                return false;
            }

            // Validate max participants
            if (nudSoLuongToiDa.Value <= 0)
            {
                MessageBox.Show("Số lượng tối đa phải lớn hơn 0", "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudSoLuongToiDa.Focus();
                return false;
            }

            // Validate status
            if (string.IsNullOrWhiteSpace(cboTrangThai.Text))
            {
                MessageBox.Show("Vui lòng chọn trạng thái", "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return false;
            }

            // ✅ Validate status consistency with date
            string status = cboTrangThai.SelectedItem?.ToString() ?? "Đang chuẩn bị";
            DateTime eventDate = dtpNgayToChuc.Value.Date;
            DateTime nowDate = DateTime.Today;

            if (eventDate < nowDate)
            {
                // Event is in the past
                if (status == "Đang chuẩn bị" || status == "Đang diễn ra")
                {
                    MessageBox.Show($"⚠️ Lưu ý: Hoạt động này được tổ chức ngày {eventDate:dd/MM/yyyy} (quá khứ), " +
                                    $"nhưng trạng thái là \"{status}\". " +
                                    $"Hãy đảm bảo đây là hoạt động có kế hoạch tái diễn hoặc cập nhật trạng thái thành \"Hoàn thành\" hoặc \"Hủy bỏ\".",
                                    "⚠️ Cảnh báo Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Don't block, just warn
                }
            }

            // Validate LoaiHoatDong selection
            if (cboLoaiHD.DataSource == null || cboLoaiHD.SelectedValue == null || !int.TryParse(cboLoaiHD.SelectedValue.ToString(), out int tmpLoai) || tmpLoai <= 0)
            {
                MessageBox.Show("Vui lòng chọn loại hoạt động", "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiHD.Focus();
                return false;
            }

            // Check duplicate activity (same name and same date) - skip when editing same MaHD
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("SELECT COUNT(1) FROM HoatDong WHERE TenHD=@TenHD AND NgayToChuc=@NgayToChuc" + (ActivityData.MaHD > 0 ? " AND MaHD<>@MaHD" : ""), conn))
                {
                    cmd.Parameters.AddWithValue("@TenHD", txtTenHD.Text.Trim());
                    cmd.Parameters.AddWithValue("@NgayToChuc", dtpNgayToChuc.Value.Date);
                    if (ActivityData.MaHD > 0) cmd.Parameters.AddWithValue("@MaHD", ActivityData.MaHD);
                    conn.Open();
                    var cnt = Convert.ToInt32(cmd.ExecuteScalar());
                    if (cnt > 0)
                    {
                        MessageBox.Show("Đã tồn tại hoạt động cùng tên vào ngày đã chọn.", "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTenHD.Focus();
                        return false;
                    }
                }
            }
            catch { }

            // Validate description
            if (txtMoTa.Text.Length > 1000)
            {
                MessageBox.Show("Mô tả không được vượt quá 1000 ký tự", "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMoTa.Focus();
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            ActivityData.TenHD = txtTenHD.Text.Trim();
            ActivityData.NgayToChuc = dtpNgayToChuc.Value.Date;

            // store times as HH:mm:ss for DB TIME compatibility
            ActivityData.GioBatDau = dtpGioBatDau.Value.ToString("HH:mm:ss");
            ActivityData.GioKetThuc = dtpGioKetThuc.Value.ToString("HH:mm:ss");

            ActivityData.DiaDiem = txtDiaDiem.Text.Trim();
            ActivityData.MoTa = txtMoTa.Text.Trim();
            
            // ✅ Store budget as decimal (VND)
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

        private void ApplyFormStyling()
        {
            // Apply consistent fonts/colors to existing controls without adding a header panel.
            try
            {
                Action<Control.ControlCollection> styleChildren = null;
                styleChildren = (cols) =>
                {
                    foreach (Control c in cols)
                    {
                        if (c is Label l)
                        {
                            // make section labels slightly larger/bolder if they look like headers
                            if (l.Font.Size < 12 && l.Text.Length < 40)
                                l.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                            else
                                l.Font = new Font("Segoe UI", l.Font.Size, l.Font.Style);
                        }
                        else if (c is TextBox tb)
                        {
                            tb.Font = new Font("Segoe UI", 9);
                        }
                        else if (c is ComboBox cb)
                        {
                            cb.Font = new Font("Segoe UI", 9);
                        }
                        else if (c is NumericUpDown nud)
                        {
                            nud.Font = new Font("Segoe UI", 9);
                            
                            // ✅ Format kinh phí as VND (thousand separator)
                            if (nud.Name.Contains("KinhPhi") || nud.Name.Contains("kinhphi") || nud.Name.Contains("Kinh"))
                            {
                                nud.DecimalPlaces = 0;
                                nud.ThousandsSeparator = true; // enable thousand separator for VND
                            }
                        }

                        if (c.HasChildren)
                            styleChildren(c.Controls);
                    }
                };

                styleChildren(this.Controls);
            }
            catch { }
        }
    }
}
