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
    public partial class Notification : UserControl
    {
        private string connectionString = @"Data Source=DESKTOP-B7F3HIU;Initial Catalog=QL_APP_LSC;Integrated Security=True;TrustServerCertificate=True";

        private BindingList<NotificationData> notifications = new BindingList<NotificationData>();
        private BindingList<NotificationData> filtered = new BindingList<NotificationData>();

        private Timer footerTimer;

        public Notification()
        {
            InitializeComponent();

            // Do not lock MinimumSize/MaximumSize so the control can resize when docked in parent
            // Keep the designed size as default
            // this.MinimumSize = this.Size;
            // this.MaximumSize = this.Size;
        }

        private void Notification_Load(object sender, EventArgs e)
        {
            try
            {
                // Wire events
                if (txtSearch != null) txtSearch.TextChanged += (s, ev) => ApplyFiltersAndSearch();
                if (btnSearch != null) btnSearch.Click += (s, ev) => ApplyFiltersAndSearch();
                if (btnResetFilter != null) btnResetFilter.Click += (s, ev) => ResetFilters();
                if (btnAdd != null) btnAdd.Click += (s, ev) => AddNotification();
                if (btnEdit != null) btnEdit.Click += (s, ev) => EditSelected();
                if (btnDelete != null) btnDelete.Click += (s, ev) => DeleteSelected();
                if (btnView != null) btnView.Click += (s, ev) => ViewSelected();
                if (btnRefresh != null) btnRefresh.Click += (s, ev) => { LoadNotifications(); ApplyFiltersAndSearch(); };
                if (cboFilterType != null) cboFilterType.SelectedIndexChanged += (s, ev) => ApplyFiltersAndSearch();
                if (cboFilterStatus != null) cboFilterStatus.SelectedIndexChanged += (s, ev) => ApplyFiltersAndSearch();
                if (cboSortBy != null) cboSortBy.SelectedIndexChanged += (s, ev) => ApplyFiltersAndSearch();

                WireGrid();
                LoadNotifications();
                ApplyFiltersAndSearch();

                // Footer timer
                footerTimer = new Timer { Interval = 1000 };
                footerTimer.Tick += (s, ev) => UpdateFooter();
                footerTimer.Start();

                UpdateFooter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi khởi tạo Notification: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireGrid()
        {
            try
            {
                dgvNotifications.AutoGenerateColumns = false;
                dgvNotifications.Columns.Clear();

                var colId = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(NotificationData.MaTB),
                    Name = "MaTB",
                    HeaderText = "ID",
                    Width = 60
                };
                var colTitle = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(NotificationData.TieuDe),
                    Name = "TieuDe",
                    HeaderText = "Tiêu đề",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                };
                var colType = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(NotificationData.LoaiThongBao),
                    Name = "LoaiThongBao",
                    HeaderText = "Loại",
                    Width = 120
                };
                var colDate = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(NotificationData.NgayGui),
                    Name = "NgayGui",
                    HeaderText = "Ngày gửi",
                    Width = 140,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
                };
                var colRecipient = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(NotificationData.DoiTuong),
                    Name = "DoiTuong",
                    HeaderText = "Đối tượng",
                    Width = 120
                };
                var colStatus = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(NotificationData.TrangThai),
                    Name = "TrangThai",
                    HeaderText = "Trạng thái",
                    Width = 120
                };

                dgvNotifications.Columns.AddRange(new DataGridViewColumn[] { colId, colTitle, colType, colDate, colRecipient, colStatus });

                dgvNotifications.DataSource = filtered;
                dgvNotifications.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvNotifications.MultiSelect = false;
                dgvNotifications.ReadOnly = true;
                dgvNotifications.RowHeadersVisible = false;
                dgvNotifications.AllowUserToAddRows = false;
                dgvNotifications.AllowUserToDeleteRows = false;
                dgvNotifications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dgvNotifications.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) ViewSelected(); };
            }
            catch { }
        }

        private void LoadNotifications()
        {
            notifications.Clear();

            string query = @"SELECT MaTB, TieuDe, NoiDung, LoaiThongBao, NgayDang, NgayGui, NguoiDang, DoiTuong, TrangThai FROM ThongBao ORDER BY NgayGui DESC";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            notifications.Add(new NotificationData
                            {
                                MaTB = r["MaTB"] != DBNull.Value ? Convert.ToInt32(r["MaTB"]) : 0,
                                TieuDe = r["TieuDe"]?.ToString() ?? "",
                                NoiDung = r["NoiDung"]?.ToString() ?? "",
                                LoaiThongBao = r["LoaiThongBao"]?.ToString() ?? "",
                                NgayDang = r["NgayDang"] != DBNull.Value ? Convert.ToDateTime(r["NgayDang"]) : DateTime.MinValue,
                                NgayGui = r["NgayGui"] != DBNull.Value ? Convert.ToDateTime(r["NgayGui"]) : DateTime.MinValue,
                                NguoiDang = r["NguoiDang"] != DBNull.Value ? Convert.ToInt32(r["NguoiDang"]) : 0,
                                DoiTuong = r["DoiTuong"]?.ToString() ?? "",
                                TrangThai = r["TrangThai"]?.ToString() ?? ""
                            });
                        }
                    }
                }

                // populate filter comboboxes if empty
                if (cboFilterType != null && cboFilterType.Items.Count <= 1)
                {
                    cboFilterType.Items.Clear();
                    cboFilterType.Items.Add("Tất cả");
                    var types = notifications.Select(n => n.LoaiThongBao).Where(x => !string.IsNullOrEmpty(x)).Distinct().OrderBy(x => x);
                    foreach (var t in types) cboFilterType.Items.Add(t);
                    cboFilterType.SelectedIndex = 0;
                }

                if (cboFilterStatus != null && cboFilterStatus.Items.Count <= 1)
                {
                    cboFilterStatus.Items.Clear();
                    cboFilterStatus.Items.Add("Tất cả");
                    var stats = notifications.Select(n => n.TrangThai).Where(x => !string.IsNullOrEmpty(x)).Distinct().OrderBy(x => x);
                    foreach (var s in stats) cboFilterStatus.Items.Add(s);
                    cboFilterStatus.SelectedIndex = 0;
                }

                if (cboSortBy != null && cboSortBy.Items.Count == 0)
                {
                    cboSortBy.Items.AddRange(new object[] { "Ngày gửi (Mới nhất)", "Ngày gửi (Cũ nhất)", "Tiêu đề (A-Z)", "Tiêu đề (Z-A)" });
                    cboSortBy.SelectedIndex = 0;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Lỗi khi tải thông báo: {ex.Message}", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception) { }
        }

        private void ApplyFiltersAndSearch()
        {
            filtered.Clear();

            string text = txtSearch?.Text?.Trim().ToLower() ?? string.Empty;
            string type = cboFilterType?.SelectedItem?.ToString() ?? "Tất cả";
            string status = cboFilterStatus?.SelectedItem?.ToString() ?? "Tất cả";
            string sort = cboSortBy?.SelectedItem?.ToString() ?? "Ngày gửi (Mới nhất)";

            var q = notifications.AsEnumerable();

            if (!string.IsNullOrEmpty(text))
            {
                q = q.Where(n => (n.TieuDe ?? string.Empty).ToLower().Contains(text) || (n.NoiDung ?? string.Empty).ToLower().Contains(text));
            }
            if (!string.IsNullOrEmpty(type) && type != "Tất cả") q = q.Where(n => n.LoaiThongBao == type);
            if (!string.IsNullOrEmpty(status) && status != "Tất cả") q = q.Where(n => n.TrangThai == status);

            // sorting
            if (sort == "Ngày gửi (Cũ nhất)") q = q.OrderBy(n => n.NgayGui);
            else if (sort == "Tiêu đề (A-Z)") q = q.OrderBy(n => n.TieuDe);
            else if (sort == "Tiêu đề (Z-A)") q = q.OrderByDescending(n => n.TieuDe);
            else q = q.OrderByDescending(n => n.NgayGui);

            foreach (var item in q) filtered.Add(item);

            UpdateFooter();
        }

        private void ResetFilters()
        {
            if (txtSearch != null) txtSearch.Text = string.Empty;
            if (cboFilterType != null) cboFilterType.SelectedIndex = 0;
            if (cboFilterStatus != null) cboFilterStatus.SelectedIndex = 0;
            if (cboSortBy != null) cboSortBy.SelectedIndex = 0;
            ApplyFiltersAndSearch();
        }

        private NotificationData GetSelectedNotification()
        {
            if (dgvNotifications == null || dgvNotifications.CurrentRow == null) return null;
            return dgvNotifications.CurrentRow.DataBoundItem as NotificationData;
        }

        private void AddNotification()
        {
            using (var form = CreateEditDialog())
            {
                form.Text = "Tạo thông báo mới";
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var title = ((TextBox)form.Controls.Find("txtTitle", true)[0]).Text.Trim();
                    var content = ((TextBox)form.Controls.Find("txtContent", true)[0]).Text.Trim();
                    var type = ((ComboBox)form.Controls.Find("cboType", true)[0]).Text;
                    var recipient = ((TextBox)form.Controls.Find("txtRecipient", true)[0]).Text.Trim();
                    var status = ((ComboBox)form.Controls.Find("cboStatus", true)[0]).Text;
                    var ngayGui = ((DateTimePicker)form.Controls.Find("dtpNgayGui", true)[0]).Value;

                    InsertNotification(new NotificationData
                    {
                        TieuDe = title,
                        NoiDung = content,
                        LoaiThongBao = type,
                        DoiTuong = recipient,
                        TrangThai = status,
                        NgayGui = ngayGui
                    });

                    LoadNotifications(); ApplyFiltersAndSearch();
                }
            }
        }

        private void EditSelected()
        {
            var sel = GetSelectedNotification();
            if (sel == null)
            {
                MessageBox.Show("Vui lòng chọn thông báo để chỉnh sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var form = CreateEditDialog(sel))
            {
                form.Text = "Chỉnh sửa thông báo";
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var title = ((TextBox)form.Controls.Find("txtTitle", true)[0]).Text.Trim();
                    var content = ((TextBox)form.Controls.Find("txtContent", true)[0]).Text.Trim();
                    var type = ((ComboBox)form.Controls.Find("cboType", true)[0]).Text;
                    var recipient = ((TextBox)form.Controls.Find("txtRecipient", true)[0]).Text.Trim();
                    var status = ((ComboBox)form.Controls.Find("cboStatus", true)[0]).Text;
                    var ngayGui = ((DateTimePicker)form.Controls.Find("dtpNgayGui", true)[0]).Value;

                    sel.TieuDe = title;
                    sel.NoiDung = content;
                    sel.LoaiThongBao = type;
                    sel.DoiTuong = recipient;
                    sel.TrangThai = status;
                    sel.NgayGui = ngayGui;

                    UpdateNotification(sel);
                    LoadNotifications(); ApplyFiltersAndSearch();
                }
            }
        }

        private void DeleteSelected()
        {
            var sel = GetSelectedNotification();
            if (sel == null)
            {
                MessageBox.Show("Vui lòng chọn thông báo để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var r = MessageBox.Show($"Bạn có chắc muốn xóa thông báo '{sel.TieuDe}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (r != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM ThongBao WHERE MaTB = @MaTB", conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTB", sel.MaTB);
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadNotifications(); ApplyFiltersAndSearch();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewSelected()
        {
            var sel = GetSelectedNotification();
            if (sel == null)
            {
                MessageBox.Show("Vui lòng chọn thông báo để xem chi tiết", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new Form())
            {
                dlg.Text = "Chi tiết thông báo";
                dlg.Size = new Size(700, 500);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;

                var lblTitle = new Label { Text = sel.TieuDe, Font = new Font("Segoe UI", 12F, FontStyle.Bold), AutoSize = false, Dock = DockStyle.Top, Height = 40 };
                var txtContent = new TextBox { Multiline = true, ReadOnly = true, Text = sel.NoiDung, Dock = DockStyle.Fill, ScrollBars = ScrollBars.Vertical };
                var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 40 };
                var btnClose = new Button { Text = "Đóng", DialogResult = DialogResult.OK, Anchor = AnchorStyles.Right, Width = 100, Height = 30, Left = dlg.ClientSize.Width - 110, Top = 5 };
                btnClose.Click += (s, e) => dlg.Close();

                pnlBottom.Controls.Add(btnClose);
                dlg.Controls.Add(txtContent);
                dlg.Controls.Add(lblTitle);
                dlg.Controls.Add(pnlBottom);

                dlg.ShowDialog();
            }
        }

        private Form CreateEditDialog(NotificationData data = null)
        {
            var form = new Form();
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterParent;
            form.Size = new Size(700, 520);
            form.MaximizeBox = false;
            form.MinimizeBox = false;

            var lblTitle = new Label { Text = "Tiêu đề:", Location = new Point(12, 12), AutoSize = true };
            var txtTitle = new TextBox { Name = "txtTitle", Location = new Point(12, 34), Width = 660 };

            var lblType = new Label { Text = "Loại:", Location = new Point(12, 70), AutoSize = true };
            var cboType = new ComboBox { Name = "cboType", Location = new Point(12, 92), Width = 250, DropDownStyle = ComboBoxStyle.DropDown }; 
            cboType.Items.AddRange(new object[] { "Thông báo chung", "Khẩn cấp", "Sự kiện", "Nhắc nhở" });

            var lblRecipient = new Label { Text = "Đối tượng:", Location = new Point(280, 70), AutoSize = true };
            var txtRecipient = new TextBox { Name = "txtRecipient", Location = new Point(280, 92), Width = 392, Text = "Tất cả" };

            var lblStatus = new Label { Text = "Trạng thái:", Location = new Point(12, 130), AutoSize = true };
            var cboStatusLocal = new ComboBox { Name = "cboStatus", Location = new Point(12, 152), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cboStatusLocal.Items.AddRange(new object[] { "Đã gửi", "Nháp" });

            var lblNgayGui = new Label { Text = "Ngày gửi:", Location = new Point(230, 130), AutoSize = true };
            var dtpNgayGui = new DateTimePicker { Name = "dtpNgayGui", Location = new Point(230, 152), Width = 200, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm" };

            var lblContent = new Label { Text = "Nội dung:", Location = new Point(12, 190), AutoSize = true };
            var txtContent = new TextBox { Name = "txtContent", Location = new Point(12, 212), Width = 660, Height = 220, Multiline = true, ScrollBars = ScrollBars.Vertical };

            var btnOk = new Button { Text = "Lưu", DialogResult = DialogResult.OK, Width = 100, Height = 34, Left = 472, Top = 444 };
            var btnCancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Width = 100, Height = 34, Left = 582, Top = 444 };

            form.Controls.AddRange(new Control[] { lblTitle, txtTitle, lblType, cboType, lblRecipient, txtRecipient, lblStatus, cboStatusLocal, lblNgayGui, dtpNgayGui, lblContent, txtContent, btnOk, btnCancel });

            if (data != null)
            {
                txtTitle.Text = data.TieuDe;
                txtContent.Text = data.NoiDung;
                cboType.Text = data.LoaiThongBao;
                txtRecipient.Text = data.DoiTuong;
                cboStatusLocal.Text = data.TrangThai;
                dtpNgayGui.Value = data.NgayGui == DateTime.MinValue ? DateTime.Now : data.NgayGui;
            }
            else
            {
                cboStatusLocal.SelectedIndex = 0;
                dtpNgayGui.Value = DateTime.Now;
            }

            return form;
        }

        private void InsertNotification(NotificationData n)
        {
            string q = @"INSERT INTO ThongBao (TieuDe, NoiDung, LoaiThongBao, NgayDang, NgayGui, NguoiDang, DoiTuong, TrangThai) 
                         VALUES (@TieuDe, @NoiDung, @Loai, GETDATE(), @NgayGui, NULL, @DoiTuong, @TrangThai)";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    {
                        cmd.Parameters.AddWithValue("@TieuDe", n.TieuDe);
                        cmd.Parameters.AddWithValue("@NoiDung", n.NoiDung);
                        cmd.Parameters.AddWithValue("@Loai", string.IsNullOrEmpty(n.LoaiThongBao) ? (object)DBNull.Value : n.LoaiThongBao);
                        cmd.Parameters.AddWithValue("@NgayGui", n.NgayGui == DateTime.MinValue ? (object)DBNull.Value : n.NgayGui);
                        cmd.Parameters.AddWithValue("@DoiTuong", string.IsNullOrEmpty(n.DoiTuong) ? (object)DBNull.Value : n.DoiTuong);
                        cmd.Parameters.AddWithValue("@TrangThai", string.IsNullOrEmpty(n.TrangThai) ? (object)DBNull.Value : n.TrangThai);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Tạo thông báo thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo thông báo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateNotification(NotificationData n)
        {
            string q = @"UPDATE ThongBao SET TieuDe=@TieuDe, NoiDung=@NoiDung, LoaiThongBao=@Loai, NgayGui=@NgayGui, DoiTuong=@DoiTuong, TrangThai=@TrangThai WHERE MaTB=@MaTB";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTB", n.MaTB);
                        cmd.Parameters.AddWithValue("@TieuDe", n.TieuDe);
                        cmd.Parameters.AddWithValue("@NoiDung", n.NoiDung);
                        cmd.Parameters.AddWithValue("@Loai", string.IsNullOrEmpty(n.LoaiThongBao) ? (object)DBNull.Value : n.LoaiThongBao);
                        cmd.Parameters.AddWithValue("@NgayGui", n.NgayGui == DateTime.MinValue ? (object)DBNull.Value : n.NgayGui);
                        cmd.Parameters.AddWithValue("@DoiTuong", string.IsNullOrEmpty(n.DoiTuong) ? (object)DBNull.Value : n.DoiTuong);
                        cmd.Parameters.AddWithValue("@TrangThai", string.IsNullOrEmpty(n.TrangThai) ? (object)DBNull.Value : n.TrangThai);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Cập nhật thông báo thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thông báo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateFooter()
        {
            try
            {
                if (lblTimeFooter != null) lblTimeFooter.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
                if (lblCountFooter != null) lblCountFooter.Text = $"Đang hiển thị: {filtered.Count} thông báo";
            }
            catch { }
        }

        private void dgvNotifications_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                ApplyFiltersAndSearch();
                e.Handled = true;
            }
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            ResetFilters();
        }

        private void cboSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    public class NotificationData
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
