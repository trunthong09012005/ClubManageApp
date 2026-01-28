using System;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class NotificationEditForm : Form
    {
        public NotificationData NotificationData { get; set; }

        public NotificationEditForm()
        {
            InitializeComponent();
        }

        public NotificationEditForm(NotificationData data) : this()
        {
            NotificationData = data;
        }

        private void NotificationEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (cboType != null && cboType.Items.Count == 0)
                    cboType.Items.AddRange(new object[] { "Thông báo chung", "Khẩn cấp", "Sự kiện", "Nhắc nhở" });

                if (cboStatus != null && cboStatus.Items.Count == 0)
                    cboStatus.Items.AddRange(new object[] { "Đã gửi", "Nháp" });

                if (NotificationData != null)
                {
                    if (txtTitle != null) txtTitle.Text = NotificationData.TieuDe ?? string.Empty;
                    if (txtContent != null) txtContent.Text = NotificationData.NoiDung ?? string.Empty;
                    if (cboType != null) cboType.Text = NotificationData.LoaiThongBao ?? string.Empty;
                    if (txtRecipient != null) txtRecipient.Text = NotificationData.DoiTuong ?? string.Empty;
                    if (cboStatus != null) cboStatus.Text = NotificationData.TrangThai ?? string.Empty;
                    if (dtpNgayGui != null && NotificationData.NgayGui != DateTime.MinValue)
                        dtpNgayGui.Value = NotificationData.NgayGui;
                }
                else
                {
                    if (cboStatus != null && cboStatus.Items.Count > 0) cboStatus.SelectedIndex = 0;
                    if (dtpNgayGui != null) dtpNgayGui.Value = DateTime.Now;
                }
            }
            catch { }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtTitle == null)
                return;

            // Validation: Title
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Tiêu đề không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            if (txtTitle.Text.Trim().Length < 5)
            {
                MessageBox.Show("Tiêu đề phải có ít nhất 5 ký tự", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            if (txtTitle.Text.Trim().Length > 200)
            {
                MessageBox.Show("Tiêu đề không được vượt quá 200 ký tự", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            // Validation: Content
            if (txtContent != null && string.IsNullOrWhiteSpace(txtContent.Text))
            {
                MessageBox.Show("Nội dung không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContent.Focus();
                return;
            }

            if (txtContent != null && txtContent.Text.Trim().Length < 10)
            {
                MessageBox.Show("Nội dung phải có ít nhất 10 ký tự", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContent.Focus();
                return;
            }

            // Validation: Type
            if (cboType != null && string.IsNullOrWhiteSpace(cboType.Text))
            {
                MessageBox.Show("Vui lòng chọn loại thông báo", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboType.Focus();
                return;
            }

            // Validation: Recipient
            if (txtRecipient != null && string.IsNullOrWhiteSpace(txtRecipient.Text))
            {
                MessageBox.Show("Đối tượng không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRecipient.Focus();
                return;
            }

            // Validation: Status
            if (cboStatus != null && string.IsNullOrWhiteSpace(cboStatus.Text))
            {
                MessageBox.Show("Vui lòng chọn trạng thái", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboStatus.Focus();
                return;
            }

            // Validation: Date
            if (dtpNgayGui != null && dtpNgayGui.Value < DateTime.Now.AddDays(-1))
            {
                MessageBox.Show("Ngày gửi không được trong quá khứ quá xa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayGui.Focus();
                return;
            }

            if (NotificationData == null) NotificationData = new NotificationData();

            NotificationData.TieuDe = txtTitle.Text.Trim();
            if (txtContent != null) NotificationData.NoiDung = txtContent.Text.Trim();
            if (cboType != null) NotificationData.LoaiThongBao = cboType.Text.Trim();
            if (txtRecipient != null) NotificationData.DoiTuong = txtRecipient.Text.Trim();
            if (cboStatus != null) NotificationData.TrangThai = cboStatus.Text.Trim();
            if (dtpNgayGui != null) NotificationData.NgayGui = dtpNgayGui.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
