using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ClubManageApp
{
    public partial class ActivityDetailForm : Form
    {
        // Event fired when the activity is updated in this detail form
        public event EventHandler ActivityUpdated;

        private ActivityData activity;
        private bool isEditing = false;

        // editable controls as fields
        private TextBox txtTenHD;
        private DateTimePicker dtpNgayToChuc;
        private TextBox txtGioBatValue;
        private TextBox txtGioKetValue;
        private TextBox txtLocationValue;
        private ComboBox cboStatusValue;
        private NumericUpDown nudKinhPhiDuKienValue;
        private NumericUpDown nudKinhPhiThucTeValue;
        private NumericUpDown nudSoLuongValue;
        private TextBox txtNguoiPhuTrachValue;
        private TextBox txtDesc;
        private Button btnEditHeader;

        public ActivityDetailForm(ActivityData activity)
        {
            InitializeComponent();
            this.activity = activity;

            if (activity != null)
            {
                SetupUI();
                LoadActivityData();
            }
        }

        private void SetupUI()
        {
            this.Text = "Chi tiết hoạt động";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Main panel
            var pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // Header panel
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(94, 148, 255)
            };

            lblTitle = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Text = activity.TenHD
            };

            // right-aligned container for header buttons
            var pnlHeaderButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 300,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10),
                BackColor = Color.Transparent,
                WrapContents = false
            };
            pnlHeader.Controls.Add(pnlHeaderButtons);

            // header edit button (small) will be added into pnlHeaderButtons
            btnEditHeader = new Button
            {
                Text = "Sửa",
                Width = 80,
                Height = 30,
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };
            btnEditHeader.Click += BtnEditSave_Click;
            pnlHeaderButtons.Controls.Add(btnEditHeader);

            // add title after header buttons so Dock=Fill occupies remaining space
            pnlHeader.Controls.Add(lblTitle);

            // Ensure main panel leaves space for header so content not hidden beneath it
            // set top padding of pnlMain to header height + base padding
            pnlMain.Padding = new Padding(pnlMain.Padding.Left, pnlHeader.Height + 20, pnlMain.Padding.Right, pnlMain.Padding.Bottom);

            // Scroll content panel
            var pnlScroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10)
            };

            int yPos = 10;

            // Basic info section
            var lblBasicSection = new Label
            {
                Location = new Point(10, yPos),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Text = "Thông tin cơ bản",
                AutoSize = true
            };
            pnlScroll.Controls.Add(lblBasicSection);
            yPos += 35;

            // Date
            var lblDateLabel = new Label { Location = new Point(20, yPos), Text = "Ngày tổ chức:", Width = 120, Font = new Font("Segoe UI", 9) };
            dtpNgayToChuc = new DateTimePicker { Location = new Point(200, yPos), Width = 200, Format = DateTimePickerFormat.Short, Enabled = false };
            if (activity.NgayToChuc != DateTime.MinValue) dtpNgayToChuc.Value = activity.NgayToChuc;
            pnlScroll.Controls.Add(lblDateLabel);
            pnlScroll.Controls.Add(dtpNgayToChuc);
            yPos += 30;

            // Start time
            var lblStartLabel = new Label { Location = new Point(20, yPos), Text = "Giờ bắt đầu:", Width = 120, Font = new Font("Segoe UI", 9) };
            txtGioBatValue = new TextBox { Location = new Point(200, yPos), Text = activity.GioBatDau ?? string.Empty, Width = 200, Enabled = false };
            pnlScroll.Controls.Add(lblStartLabel);
            pnlScroll.Controls.Add(txtGioBatValue);
            yPos += 30;

            // End time
            var lblEndLabel = new Label { Location = new Point(20, yPos), Text = "Giờ kết thúc:", Width = 120, Font = new Font("Segoe UI", 9) };
            txtGioKetValue = new TextBox { Location = new Point(200, yPos), Text = activity.GioKetThuc ?? string.Empty, Width = 200, Enabled = false };
            pnlScroll.Controls.Add(lblEndLabel);
            pnlScroll.Controls.Add(txtGioKetValue);
            yPos += 30;

            // Location
            var lblLocationLabel = new Label { Location = new Point(20, yPos), Text = "Địa điểm:", Width = 120, Font = new Font("Segoe UI", 9) };
            txtLocationValue = new TextBox { Location = new Point(200, yPos), Text = activity.DiaDiem ?? string.Empty, Width = 200, Enabled = false };
            pnlScroll.Controls.Add(lblLocationLabel);
            pnlScroll.Controls.Add(txtLocationValue);
            yPos += 30;

            // Status
            var lblStatusLabel = new Label { Location = new Point(20, yPos), Text = "Trạng thái:", Width = 120, Font = new Font("Segoe UI", 9) };
            cboStatusValue = new ComboBox { Location = new Point(200, yPos), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, Enabled = false };
            cboStatusValue.Items.AddRange(new[] { "Đang chuẩn bị", "Đang diễn ra", "Hoàn thành", "Hủy bỏ" });
            if (!string.IsNullOrEmpty(activity.TrangThai) && cboStatusValue.Items.Contains(activity.TrangThai)) cboStatusValue.SelectedItem = activity.TrangThai;
            pnlScroll.Controls.Add(lblStatusLabel);
            pnlScroll.Controls.Add(cboStatusValue);
            yPos += 40;

            // Financial info section
            var lblFinanceSection = new Label
            {
                Location = new Point(10, yPos),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Text = "Thông tin tài chính",
                AutoSize = true
            };
            pnlScroll.Controls.Add(lblFinanceSection);
            yPos += 35;

            // Budget
            var lblBudgetLabel = new Label { Location = new Point(20, yPos), Text = "Kinh phí dự kiến:", Width = 150, Font = new Font("Segoe UI", 9) };
            nudKinhPhiDuKienValue = new NumericUpDown { Location = new Point(200, yPos), Width = 200, Maximum = decimal.MaxValue, DecimalPlaces = 0, ThousandsSeparator = true, Enabled = false };
            nudKinhPhiDuKienValue.Value = activity.KinhPhiDuKien;
            pnlScroll.Controls.Add(lblBudgetLabel);
            pnlScroll.Controls.Add(nudKinhPhiDuKienValue);
            yPos += 30;

            // Actual budget
            var lblActualLabel = new Label { Location = new Point(20, yPos), Text = "Kinh phí thực tế:", Width = 150, Font = new Font("Segoe UI", 9) };
            nudKinhPhiThucTeValue = new NumericUpDown { Location = new Point(200, yPos), Width = 200, Maximum = decimal.MaxValue, DecimalPlaces = 0, ThousandsSeparator = true, Enabled = false };
            nudKinhPhiThucTeValue.Value = activity.KinhPhiThucTe;
            pnlScroll.Controls.Add(lblActualLabel);
            pnlScroll.Controls.Add(nudKinhPhiThucTeValue);
            yPos += 40;

            // Participant info section
            var lblPartSection = new Label
            {
                Location = new Point(10, yPos),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Text = "Thông tin người tham gia",
                AutoSize = true
            };
            pnlScroll.Controls.Add(lblPartSection);
            yPos += 35;

            // Max participants
            var lblMaxLabel = new Label { Location = new Point(20, yPos), Text = "Số lượng tối đa:", Width = 150, Font = new Font("Segoe UI", 9) };
            nudSoLuongValue = new NumericUpDown { Location = new Point(200, yPos), Width = 200, Maximum = 1000000, Value = activity.SoLuongToiDa, Enabled = false };
            pnlScroll.Controls.Add(lblMaxLabel);
            pnlScroll.Controls.Add(nudSoLuongValue);
            yPos += 30;

            // Person in charge
            var lblChargeLabel = new Label { Location = new Point(20, yPos), Text = "Người phụ trách:", Width = 150, Font = new Font("Segoe UI", 9) };
            txtNguoiPhuTrachValue = new TextBox { Location = new Point(200, yPos), Width = 200, Text = activity.NguoiPhuTrach > 0 ? activity.NguoiPhuTrach.ToString() : string.Empty, Enabled = false };
            pnlScroll.Controls.Add(lblChargeLabel);
            pnlScroll.Controls.Add(txtNguoiPhuTrachValue);
            yPos += 40;

            // Description section
            var lblDescSection = new Label
            {
                Location = new Point(10, yPos),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Text = "Mô tả",
                AutoSize = true
            };
            pnlScroll.Controls.Add(lblDescSection);
            yPos += 35;

            txtDesc = new TextBox
            {
                Location = new Point(20, yPos),
                Width = 650,
                Height = 150,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Text = activity.MoTa ?? "Không có mô tả",
                Font = new Font("Segoe UI", 9)
            };
            pnlScroll.Controls.Add(txtDesc);

            // Footer buttons
            var pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(245, 248, 252),
                Visible = false // buttons moved to header
            };

            // only keep header edit button (btnEditHeader) in header buttons panel
            // header button text already set when created above

            pnlMain.Controls.Add(pnlScroll);
            pnlMain.Controls.Add(pnlFooter);

            // add panels to form: header first, then main. main has top padding so content starts below header
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlMain);
        }

        private void LoadActivityData()
        {
            // UI is already loaded in SetupUI
        }

        private Label lblTitle;

        private void txtDetails_TextChanged(object sender, EventArgs e)
        {

        }

        private void ActivityDetailForm_Load(object sender, EventArgs e)
        {

        }

        private void BtnEditSave_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                // enter edit mode
                isEditing = true;
                btnEditHeader.Text = "Lưu";
                // enable controls
                txtTenHD = txtTenHD ?? new TextBox(); // ensure not null
                txtTenHD.Text = activity.TenHD;
                txtTenHD.Location = lblTitle.Location; // not used visually
                txtTenHD.Visible = false;
                dtpNgayToChuc.Enabled = true;
                txtGioBatValue.Enabled = true;
                txtGioKetValue.Enabled = true;
                txtLocationValue.Enabled = true;
                cboStatusValue.Enabled = true;
                nudKinhPhiDuKienValue.Enabled = true;
                nudKinhPhiThucTeValue.Enabled = true;
                nudSoLuongValue.Enabled = true;
                txtNguoiPhuTrachValue.Enabled = true;
                txtDesc.ReadOnly = false;
            }
            else
            {
                // confirm save
                var res = MessageBox.Show("Bạn có chắc muốn lưu thay đổi?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res != DialogResult.Yes)
                {
                    // cancel edit mode
                    isEditing = false;
                    btnEditHeader.Text = "Sửa";
                    dtpNgayToChuc.Enabled = false;
                    txtGioBatValue.Enabled = false;
                    txtGioKetValue.Enabled = false;
                    txtLocationValue.Enabled = false;
                    cboStatusValue.Enabled = false;
                    nudKinhPhiDuKienValue.Enabled = false;
                    nudKinhPhiThucTeValue.Enabled = false;
                    nudSoLuongValue.Enabled = false;
                    txtNguoiPhuTrachValue.Enabled = false;
                    txtDesc.ReadOnly = true;
                    return;
                }

                // gather values
                string newTen = !string.IsNullOrEmpty(txtTenHD?.Text) ? txtTenHD.Text.Trim() : activity.TenHD;
                DateTime newNgay = dtpNgayToChuc.Value.Date;
                string newGioBat = txtGioBatValue.Text.Trim();
                string newGioKet = txtGioKetValue.Text.Trim();
                string newDia = txtLocationValue.Text.Trim();
                string newTrang = cboStatusValue.SelectedItem?.ToString() ?? activity.TrangThai;
                decimal newKinh = nudKinhPhiDuKienValue.Value;
                decimal newKinhThuc = nudKinhPhiThucTeValue.Value;
                int newSoLuong = (int)nudSoLuongValue.Value;
                int newNguoi = 0;
                int.TryParse(txtNguoiPhuTrachValue.Text, out newNguoi);
                string newMoTa = txtDesc.Text;

                // update DB
                try
                {
                    using (var conn = new SqlConnection(ConnectionHelper.ConnectionString))
                    using (var cmd = new SqlCommand(@"UPDATE HoatDong SET TenHD=@TenHD, NgayToChuc=@NgayToChuc, GioBatDau=@GioBatDau, GioKetThuc=@GioKetThuc, DiaDiem=@DiaDiem, MoTa=@MoTa, TrangThai=@TrangThai, KinhPhiDuKien=@KinhPhiDuKien, KinhPhiThucTe=@KinhPhiThucTe, SoLuongToiDa=@SoLuongToiDa, NguoiPhuTrach=@NguoiPhuTrach WHERE MaHD=@MaHD", conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@TenHD", newTen ?? string.Empty);
                        cmd.Parameters.AddWithValue("@NgayToChuc", newNgay == DateTime.MinValue ? (object)DBNull.Value : newNgay);
                        cmd.Parameters.AddWithValue("@GioBatDau", string.IsNullOrEmpty(newGioBat) ? (object)DBNull.Value : newGioBat);
                        cmd.Parameters.AddWithValue("@GioKetThuc", string.IsNullOrEmpty(newGioKet) ? (object)DBNull.Value : newGioKet);
                        cmd.Parameters.AddWithValue("@DiaDiem", newDia ?? string.Empty);
                        cmd.Parameters.AddWithValue("@MoTa", newMoTa ?? string.Empty);
                        cmd.Parameters.AddWithValue("@TrangThai", newTrang ?? string.Empty);
                        cmd.Parameters.AddWithValue("@KinhPhiDuKien", newKinh);
                        cmd.Parameters.AddWithValue("@KinhPhiThucTe", newKinhThuc);
                        cmd.Parameters.AddWithValue("@SoLuongToiDa", newSoLuong);
                        cmd.Parameters.AddWithValue("@NguoiPhuTrach", newNguoi);
                        cmd.Parameters.AddWithValue("@MaHD", activity.MaHD);
                        cmd.ExecuteNonQuery();
                    }

                    // update local object
                    activity.TenHD = newTen;
                    activity.NgayToChuc = newNgay;
                    activity.GioBatDau = newGioBat;
                    activity.GioKetThuc = newGioKet;
                    activity.DiaDiem = newDia;
                    activity.MoTa = newMoTa;
                    activity.TrangThai = newTrang;
                    activity.KinhPhiDuKien = newKinh;
                    activity.KinhPhiThucTe = newKinhThuc;
                    activity.SoLuongToiDa = newSoLuong;
                    activity.NguoiPhuTrach = newNguoi;

                    MessageBox.Show("Lưu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // notify parent to refresh list
                    ActivityUpdated?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // exit edit mode
                isEditing = false;
                btnEditHeader.Text = "Sửa";
                dtpNgayToChuc.Enabled = false;
                txtGioBatValue.Enabled = false;
                txtGioKetValue.Enabled = false;
                txtLocationValue.Enabled = false;
                cboStatusValue.Enabled = false;
                nudKinhPhiDuKienValue.Enabled = false;
                nudKinhPhiThucTeValue.Enabled = false;
                nudSoLuongValue.Enabled = false;
                txtNguoiPhuTrachValue.Enabled = false;
                txtDesc.ReadOnly = true;
                // update header label text
                lblTitle.Text = activity.TenHD;
            }
        }
    }
}
