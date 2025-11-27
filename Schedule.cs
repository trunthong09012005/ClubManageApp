using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;

namespace ClubManageApp
{
    public partial class ucSchedule : UserControl
    {
        // add connection string to enable direct DB loading
        private string connectionString = @"Data Source=DESKTOP-EJIGPN3;Initial Catalog=QL_APP_LSC;Persist Security Info=True;User ID=sa;Password=1234;Encrypt=True;TrustServerCertificate=True";

        private BindingList<Meeting> meetings = new BindingList<Meeting>();
        private BindingList<Participant> participants = new BindingList<Participant>();
        private BindingList<Minute> minutes = new BindingList<Minute>();
        private BindingList<KyLuat> kyLuats = new BindingList<KyLuat>();

        public ucSchedule()
        {
            InitializeComponent();
            InitializeData();
            WireGrids();

            // wire send all button
            this.button1.Click += Button1_Click;

            // live search when typing in txtFilter
            try
            {
                this.txtFilter.TextChanged += TxtFilter_TextChanged;
            }
            catch { }

            // set MinDate at runtime to avoid designer parsing issues
            try
            {
                this.monthCalendar1.MinDate = DateTime.Today;
            }
            catch { }
        }

        private void TxtFilter_TextChanged(object sender, EventArgs e)
        {
            var q = txtFilter.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(q))
            {
                dgvMeetings.DataSource = meetings;
            }
            else
            {
                var filtered = new BindingList<Meeting>(meetings.Where(m => (m.TieuDe ?? string.Empty).ToLower().Contains(q) || (m.DiaDiem ?? string.Empty).ToLower().Contains(q)).ToList());
                dgvMeetings.DataSource = filtered;
            }
        }

        private void InitializeData()
        {
            // Try to load participants from database; if fails, fall back to sample data
            try
            {
                LoadParticipantsFromDatabase();
            }
            catch
            {
                // ignore and use sample data below
            }

            if (participants.Count == 0)
            {
                AddSampleParticipants();
            }

            // Try load meetings from DB; if fails, fall back to sample data
            try
            {
                LoadMeetingsFromDatabase();
            }
            catch
            {
                // ignore
            }

            if (meetings.Count == 0)
            {
                // Meetings sample data (keep as-is for now)
                meetings.Add(new Meeting { Id = 1, TieuDe = "Họp tổng kết quý I/2025", NgayHop = DateTime.Parse("2025-04-15 14:00"), DiaDiem = "Phòng họp A101", NoiDung = "Tổng kết hoạt động quý I và lập kế hoạch quý II", NguoiChuTri = 2, TrangThai = "Hoàn thành" });
                meetings.Add(new Meeting { Id = 2, TieuDe = "Họp chuẩn bị Mùa hè xanh", NgayHop = DateTime.Parse("2025-06-20 15:00"), DiaDiem = "Phòng CLB", NoiDung = "Thảo luận kế hoạch chi tiết cho chiến dịch Mùa hè xanh", NguoiChuTri = 2, TrangThai = "Hoàn thành" });
                meetings.Add(new Meeting { Id = 3, TieuDe = "Họp Ban chủ nhiệm tháng 8", NgayHop = DateTime.Parse("2025-08-05 16:00"), DiaDiem = "Phòng họp B201", NoiDung = "Đánh giá hoạt động tháng 7 và kế hoạch tháng 8", NguoiChuTri = 2, TrangThai = "Hoàn thành" });
                meetings.Add(new Meeting { Id = 4, TieuDe = "Họp toàn thể CLB - Kế hoạch tuyển thành viên", NgayHop = DateTime.Parse("2025-09-15 14:00"), DiaDiem = "Hội trường lớn", NoiDung = "Công bố kế hoạch tuyển thành viên mới và phân công nhiệm vụ", NguoiChuTri = 2, TrangThai = "Hoàn thành" });
                meetings.Add(new Meeting { Id = 5, TieuDe = "Họp Ban chủ nhiệm tháng 11", NgayHop = DateTime.Parse("2025-11-18 15:00"), DiaDiem = "Phòng họp A101", NoiDung = "Thảo luận kế hoạch cuối năm và chuẩn bị đại hội", NguoiChuTri = 2, TrangThai = "Sắp diễn ra" });
            }

            // Note: kyLuats and minutes left empty
        }

        // Load participants directly from database
        private void LoadParticipantsFromDatabase()
        {
            participants.Clear();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Adjust column names to match your ThanhVien table
                var cmd = new SqlCommand(@"SELECT MaTV, HoTen, NgaySinh, GioiTinh, Lop, Khoa, SDT, Email, DiaChi, VaiTro, MaCV, MaBan FROM ThanhVien", conn);
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var p = new Participant();
                        p.Id = rdr["MaTV"] != DBNull.Value ? Convert.ToInt32(rdr["MaTV"]) : 0;
                        p.HoTen = rdr["HoTen"] != DBNull.Value ? rdr["HoTen"].ToString() : string.Empty;
                        p.NgaySinh = rdr["NgaySinh"] != DBNull.Value ? Convert.ToDateTime(rdr["NgaySinh"]) : default(DateTime);
                        p.GioiTinh = rdr["GioiTinh"] != DBNull.Value ? rdr["GioiTinh"].ToString() : string.Empty;
                        p.Lop = rdr["Lop"] != DBNull.Value ? rdr["Lop"].ToString() : string.Empty;
                        p.Khoa = rdr["Khoa"] != DBNull.Value ? rdr["Khoa"].ToString() : string.Empty;
                        p.SDT = rdr["SDT"] != DBNull.Value ? rdr["SDT"].ToString() : string.Empty;
                        p.Email = rdr["Email"] != DBNull.Value ? rdr["Email"].ToString() : string.Empty;
                        p.DiaChi = rdr["DiaChi"] != DBNull.Value ? rdr["DiaChi"].ToString() : string.Empty;
                        p.VaiTro = rdr["VaiTro"] != DBNull.Value ? rdr["VaiTro"].ToString() : string.Empty;
                        p.MaCV = rdr["MaCV"] != DBNull.Value ? Convert.ToInt32(rdr["MaCV"]) : 0;
                        p.MaBan = rdr["MaBan"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["MaBan"]) : null;
                        participants.Add(p);
                    }
                }
            }
        }

        // Load meetings from database
        private void LoadMeetingsFromDatabase()
        {
            meetings.Clear();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Adjust column names to match your LichHop table
                var cmd = new SqlCommand(@"SELECT Id, TieuDe, NgayHop, DiaDiem, NoiDung, NguoiChuTri, TrangThai FROM LichHop", conn);
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var m = new Meeting();
                        m.Id = rdr["Id"] != DBNull.Value ? Convert.ToInt32(rdr["Id"]) : 0;
                        m.TieuDe = rdr["TieuDe"] != DBNull.Value ? rdr["TieuDe"].ToString() : string.Empty;
                        m.NgayHop = rdr["NgayHop"] != DBNull.Value ? Convert.ToDateTime(rdr["NgayHop"]) : default(DateTime);
                        m.DiaDiem = rdr["DiaDiem"] != DBNull.Value ? rdr["DiaDiem"].ToString() : string.Empty;
                        m.NoiDung = rdr["NoiDung"] != DBNull.Value ? rdr["NoiDung"].ToString() : string.Empty;
                        m.NguoiChuTri = rdr["NguoiChuTri"] != DBNull.Value ? Convert.ToInt32(rdr["NguoiChuTri"]) : 0;
                        m.TrangThai = rdr["TrangThai"] != DBNull.Value ? rdr["TrangThai"].ToString() : string.Empty;
                        meetings.Add(m);
                    }
                }
            }
        }

        // If DB not available, add sample participants (existing hard-coded data)
        private void AddSampleParticipants()
        {
            participants.Add(new Participant { Id = 1, HoTen = "Nguyễn Vương Khang", NgaySinh = DateTime.Parse("2004-02-15"), GioiTinh = "Nam", Lop = "DHKTPM17A", Khoa = "Công nghệ thông tin", SDT = "0912345678", Email = "huytm@student.hcmute.edu.vn", DiaChi = "Quận 7, TP.HCM", VaiTro = "Chủ nhiệm", MaCV = 1, MaBan = null });
            participants.Add(new Participant { Id = 2, HoTen = "Nguyễn Thị Lan", NgaySinh = DateTime.Parse("2005-05-10"), GioiTinh = "Nữ", Lop = "DHKTPM17A", Khoa = "Công nghệ thông tin", SDT = "0987654321", Email = "lannt@student.hcmute.edu.vn", DiaChi = "Quận 5, TP.HCM", VaiTro = "Phó chủ nhiệm", MaCV = 2, MaBan = null });
            participants.Add(new Participant { Id = 3, HoTen = "Lê Quốc Bảo", NgaySinh = DateTime.Parse("2005-09-21"), GioiTinh = "Nam", Lop = "DHKTPM17B", Khoa = "Công nghệ thông tin", SDT = "0977112233", Email = "baolq@student.hcmute.edu.vn", DiaChi = "Quận 10, TP.HCM", VaiTro = "Trưởng ban", MaCV = 4, MaBan = 1 });
            participants.Add(new Participant { Id = 4, HoTen = "Phạm Thị Mai", NgaySinh = DateTime.Parse("2005-03-18"), GioiTinh = "Nữ", Lop = "DHKTPM17B", Khoa = "Công nghệ thông tin", SDT = "0965432109", Email = "maipt@student.hcmute.edu.vn", DiaChi = "Quận 3, TP.HCM", VaiTro = "Trưởng ban", MaCV = 4, MaBan = 2 });
            participants.Add(new Participant { Id = 5, HoTen = "Hoàng Văn Nam", NgaySinh = DateTime.Parse("2004-11-25"), GioiTinh = "Nam", Lop = "DHKTPM17C", Khoa = "Công nghệ thông tin", SDT = "0923456789", Email = "namhv@student.hcmute.edu.vn", DiaChi = "Quận 1, TP.HCM", VaiTro = "Trưởng ban", MaCV = 4, MaBan = 3 });
            participants.Add(new Participant { Id = 6, HoTen = "Võ Thị Hoa", NgaySinh = DateTime.Parse("2005-07-08"), GioiTinh = "Nữ", Lop = "DHKTPM17A", Khoa = "Công nghệ thông tin", SDT = "0934567890", Email = "hoavt@student.hcmute.edu.vn", DiaChi = "Quận 6, TP.HCM", VaiTro = "Thành viên", MaCV = 6, MaBan = 1 });
            participants.Add(new Participant { Id = 7, HoTen = "Đặng Minh Tuấn", NgaySinh = DateTime.Parse("2005-01-30"), GioiTinh = "Nam", Lop = "DHKTPM17B", Khoa = "Công nghệ thông tin", SDT = "0945678901", Email = "tuandm@student.hcmute.edu.vn", DiaChi = "Quận 8, TP.HCM", VaiTro = "Thành viên", MaCV = 6, MaBan = 2 });
            participants.Add(new Participant { Id = 8, HoTen = "Trương Thị Lan Anh", NgaySinh = DateTime.Parse("2004-12-12"), GioiTinh = "Nữ", Lop = "DHKTPM17C", Khoa = "Công nghệ thông tin", SDT = "0956789012", Email = "anhttl@student.hcmute.edu.vn", DiaChi = "Quận 12, TP.HCM", VaiTro = "Thành viên", MaCV = 6, MaBan = 3 });
            participants.Add(new Participant { Id = 9, HoTen = "Ngô Văn Đức", NgaySinh = DateTime.Parse("2005-04-20"), GioiTinh = "Nam", Lop = "DHKTPM17A", Khoa = "Công nghệ thông tin", SDT = "0967890123", Email = "ducnv@student.hcmute.edu.vn", DiaChi = "Thủ Đức, TP.HCM", VaiTro = "Thành viên", MaCV = 6, MaBan = 1 });
            participants.Add(new Participant { Id = 10, HoTen = "Bùi Thị Ngọc", NgaySinh = DateTime.Parse("2005-08-15"), GioiTinh = "Nữ", Lop = "DHKTPM17B", Khoa = "Công nghệ thông tin", SDT = "0978901234", Email = "ngocbt@student.hcmute.edu.vn", DiaChi = "Bình Thạnh, TP.HCM", VaiTro = "Thành viên", MaCV = 6, MaBan = 2 });
        }

        private void WireGrids()
        {
            dgvMeetings.DataSource = meetings;
            // show only selected meeting columns
            var meetingAllowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "TieuDe", "NgayHop", "DiaDiem", "NoiDung", "NguoiChuTri", "TrangThai" };
            foreach (DataGridViewColumn c in dgvMeetings.Columns)
            {
                if (!meetingAllowed.Contains(c.Name))
                    c.Visible = false;
                else
                {
                    // Set friendly headers
                    switch (c.Name)
                    {
                        case "TieuDe": c.HeaderText = "Tiêu đề"; break;
                        case "NgayHop": c.HeaderText = "Ngày họp"; break;
                        case "DiaDiem": c.HeaderText = "Địa điểm"; break;
                        case "NoiDung": c.HeaderText = "Nội dung"; break;
                        case "NguoiChuTri": c.HeaderText = "Người chủ trì"; break;
                        case "TrangThai": c.HeaderText = "Trạng thái"; break;
                    }
                }
            }

            dgvParticipants.DataSource = participants;
            // show only HoTen, Lop, Email
            var partAllowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "HoTen", "Lop", "Email" };
            foreach (DataGridViewColumn c in dgvParticipants.Columns)
            {
                if (!partAllowed.Contains(c.Name))
                    c.Visible = false;
                else
                {
                    switch (c.Name)
                    {
                        case "HoTen": c.HeaderText = "Họ tên"; break;
                        case "Lop": c.HeaderText = "Lớp"; break;
                        case "Email": c.HeaderText = "Email"; break;
                    }
                }
            }

            // populate meetings combobox
            try
            {
                comboBox1.DataSource = meetings;
                comboBox1.DisplayMember = "TieuDe";
                comboBox1.ValueMember = "Id";
            }
            catch
            {
                // ignore if designer-time
            }

            // Note: minutes / kyLuats UI removed per request; keep minutes collection available for future use

            dgvParticipants.AllowUserToAddRows = false;
            dgvParticipants.ReadOnly = true;
            dgvParticipants.RowHeadersVisible = false;
            dgvParticipants.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvParticipants.AutoResizeColumns();
            dgvParticipants.Refresh();

            dgvMeetings.AllowUserToAddRows = false;
            dgvMeetings.ReadOnly = true;
            dgvMeetings.RowHeadersVisible = false;
            dgvMeetings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMeetings.AutoResizeColumns();
            dgvMeetings.Refresh();
        }

        // Meeting actions
        private void btnNewMeeting_Click(object sender, EventArgs e)
        {
            var form = new MeetingEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                var nextId = (meetings.Count == 0) ? 1 : meetings.Max(m => m.Id) + 1;
                form.Meeting.Id = nextId;
                meetings.Add(form.Meeting);
            }
        }

        private void btnEditMeeting_Click(object sender, EventArgs e)
        {
            if (dgvMeetings.CurrentRow == null) return;
            var selected = (Meeting)dgvMeetings.CurrentRow.DataBoundItem;
            var form = new MeetingEditForm(selected);
            if (form.ShowDialog() == DialogResult.OK)
            {
                // update properties
                selected.TieuDe = form.Meeting.TieuDe;
                selected.NgayHop = form.Meeting.NgayHop;
                selected.DiaDiem = form.Meeting.DiaDiem;
                selected.NoiDung = form.Meeting.NoiDung;
                selected.NguoiChuTri = form.Meeting.NguoiChuTri;
                selected.TrangThai = form.Meeting.TrangThai;
                dgvMeetings.Refresh();
            }
        }

        private void btnDeleteMeeting_Click(object sender, EventArgs e)
        {
            if (dgvMeetings.CurrentRow == null) return;
            var selected = (Meeting)dgvMeetings.CurrentRow.DataBoundItem;
            var r = MessageBox.Show($"Xóa cuộc họp '{selected.TieuDe}'?", "Chấp nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (r == DialogResult.Yes)
            {
                meetings.Remove(selected);
            }
        }

        private void btnViewMeeting_Click(object sender, EventArgs e)
        {
            if (dgvMeetings.CurrentRow == null) return;
            var selected = (Meeting)dgvMeetings.CurrentRow.DataBoundItem;
            MessageBox.Show($"Tiêu đề: {selected.TieuDe}\nNgày họp: {selected.NgayHop}\nĐịa điểm: {selected.DiaDiem}\nNgười chủ trì: {selected.NguoiChuTri}\nTrạng thái: {selected.TrangThai}\nNội dung: {selected.NoiDung}", "Chi tiết cuộc họp");
        }

        // Participants actions
        private void dgvParticipants_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var selected = (Participant)dgvParticipants.Rows[e.RowIndex].DataBoundItem;
            // Show context menu with Send Notification
            var menu = new ContextMenu();
            var mi = new MenuItem("Gửi thông báo", (s, ev) => SendNotificationTo(selected));
            menu.MenuItems.Add(mi);
            var cellRect = dgvParticipants.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            menu.Show(dgvParticipants, new Point(cellRect.Left, cellRect.Bottom));
        }

        private void SendNotificationTo(Participant p)
        {
            using (var form = new SendMailForm(p))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // sending performed inside form
                    MessageBox.Show("Email đã gửi (hoặc đang được xử lý).", "Gửi thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Minutes actions
        private void btnViewMinute_Click(object sender, EventArgs e)
        {
            // This handler removed because minutes UI was removed in Designer
        }

        // Calendar date selected - open MeetingEditForm with selected date to create a new meeting
        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            // open the MeetingEditForm with the selected date prefilled
            var form = new MeetingEditForm(e.Start);
            if (form.ShowDialog() == DialogResult.OK)
            {
                var nextId = (meetings.Count == 0) ? 1 : meetings.Max(m => m.Id) + 1;
                form.Meeting.Id = nextId;
                meetings.Add(form.Meeting);
            }
        }

        private void ucSchedule_Load(object sender, EventArgs e)
        {

        }

        private void dgvMeetings_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // Send all button click handler
        private async void Button1_Click(object sender, EventArgs e)
        {
            // ensure a meeting is selected
            var meeting = comboBox1.SelectedItem as Meeting;
            if (meeting == null)
            {
                MessageBox.Show("Vui lòng chọn cuộc họp từ combobox.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // open dialog to collect sender credentials and message
            using (var dlg = new SendAllForm(meeting))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;

                var from = dlg.SenderEmail;
                var password = dlg.Password;
                var smtpHost = dlg.SmtpHost;
                var port = dlg.Port;
                var enableSsl = dlg.EnableSsl;
                var subject = dlg.Subject;
                var bodyTemplate = dlg.Body;

                var recipients = participants.Where(p => !string.IsNullOrWhiteSpace(p.Email)).ToList();
                if (recipients.Count == 0)
                {
                    MessageBox.Show("Không có thành viên nào có email.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var errors = new List<string>();
                var succeeded = 0;

                // send emails sequentially
                await Task.Run(() =>
                {
                    foreach (var r in recipients)
                    {
                        try
                        {
                            using (var msg = new MailMessage())
                            {
                                msg.From = new MailAddress(from);
                                msg.To.Add(new MailAddress(r.Email));
                                msg.Subject = subject;
                                // personalize body a little
                                msg.Body = bodyTemplate.Replace("{Name}", r.HoTen).Replace("{MeetingTitle}", meeting.TieuDe).Replace("{MeetingDate}", meeting.NgayHop.ToString());
                                msg.IsBodyHtml = false;

                                using (var client = new SmtpClient(smtpHost, port))
                                {
                                    client.EnableSsl = enableSsl;
                                    client.Credentials = new NetworkCredential(from, password);
                                    client.Send(msg);
                                }
                            }
                            succeeded++;
                        }
                        catch (Exception ex)
                        {
                            errors.Add($"{r.HoTen} <{r.Email}>: {ex.Message}");
                        }
                    }
                });

                var msgSum = $"Gửi xong. Thành công: {succeeded}. Thất bại: {errors.Count}.";
                if (errors.Count > 0)
                {
                    msgSum += "\n\nChi tiết lỗi:\n" + string.Join("\n", errors.Take(10));
                }
                MessageBox.Show(msgSum, "Kết quả gửi email", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Simple models for demo
        public class Meeting
        {
            public int Id { get; set; }

            // Columns from LichHop table
            public string TieuDe { get; set; }
            public DateTime NgayHop { get; set; }
            public string DiaDiem { get; set; }
            public string NoiDung { get; set; }
            public int NguoiChuTri { get; set; }
            public string TrangThai { get; set; }

            // Backwards-compatible aliases
            public string Title { get => TieuDe; set => TieuDe = value; }
            public DateTime Date { get => NgayHop; set => NgayHop = value; }
            public string Location { get => DiaDiem; set => DiaDiem = value; }
        }

        public class Minute
        {
            public int Id { get; set; }
            public int MeetingId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
        }

        public class KyLuat
        {
            public int Id { get; set; }
            public int MaTV { get; set; }
            public string LyDo { get; set; }
            public string HinhThuc { get; set; }
            public DateTime? ThoiGianKyLuat { get; set; }
            public DateTime NgayKL { get; set; }
            public int NguoiLap { get; set; }
        }

        // Simple meeting edit form used by New/Edit actions
        public class MeetingEditForm : Form
        {
            public Meeting Meeting { get; private set; }
            private TextBox txtTitle;
            private TextBox txtLocation;
            private DateTimePicker dtpDate;
            private TextBox txtNoiDung;
            private TextBox txtTrangThai;
            private NumericUpDown nudNguoiChuTri;
            private Button btnOk;
            private Button btnCancel;

            public MeetingEditForm()
            {
                InitializeForm();
                Meeting = new Meeting();
            }

            // New constructor to accept initial date for creating meeting from calendar
            public MeetingEditForm(DateTime initialDate) : this()
            {
                // ensure dtpDate exists and set to provided date (keep time at noon as default)
                try
                {
                    dtpDate.Value = initialDate.Date.AddHours(9); // default time 9:00 AM
                }
                catch { }
            }

            public MeetingEditForm(Meeting m) : this()
            {
                Meeting = new Meeting { Id = m.Id, TieuDe = m.TieuDe, NgayHop = m.NgayHop, DiaDiem = m.DiaDiem, NoiDung = m.NoiDung, NguoiChuTri = m.NguoiChuTri, TrangThai = m.TrangThai };
                txtTitle.Text = Meeting.TieuDe;
                txtLocation.Text = Meeting.DiaDiem;
                dtpDate.Value = Meeting.NgayHop == default(DateTime) ? DateTime.Today : Meeting.NgayHop;
                txtNoiDung.Text = Meeting.NoiDung;
                txtTrangThai.Text = Meeting.TrangThai;
                nudNguoiChuTri.Value = Meeting.NguoiChuTri;
            }

            private void InitializeForm()
            {
                this.Text = "Chỉnh sửa";
                this.ClientSize = new Size(620, 420);
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.StartPosition = FormStartPosition.CenterParent;

                // create controls
                txtTitle = new TextBox();
                dtpDate = new DateTimePicker() { Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm" };
                txtLocation = new TextBox();
                txtNoiDung = new TextBox() { Multiline = true, ScrollBars = ScrollBars.Vertical };
                txtTrangThai = new TextBox();
                nudNguoiChuTri = new NumericUpDown() { Minimum = 0, Maximum = 9999 };
                btnOk = new Button() { Text = "OK" };
                btnCancel = new Button() { Text = "Cancel" };

                btnOk.Click += BtnOk_Click;
                btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

                // main layout
                var tl = new TableLayoutPanel();
                tl.Dock = DockStyle.Fill;
                tl.Padding = new Padding(14);
                tl.ColumnCount = 2;
                tl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
                tl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                tl.RowCount = 6;
                tl.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // title
                tl.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // date
                tl.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // location
                tl.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // content
                tl.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F)); // status + owner
                tl.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F)); // buttons

                // Title
                tl.Controls.Add(new Label() { Text = "Tiêu đề:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
                txtTitle.Dock = DockStyle.Fill;
                tl.Controls.Add(txtTitle, 1, 0);

                // Date
                tl.Controls.Add(new Label() { Text = "Ngày giờ:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 1);
                dtpDate.Anchor = AnchorStyles.Left;
                dtpDate.Width = 260;
                tl.Controls.Add(dtpDate, 1, 1);

                // Location
                tl.Controls.Add(new Label() { Text = "Địa điểm:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 2);
                txtLocation.Dock = DockStyle.Fill;
                tl.Controls.Add(txtLocation, 1, 2);

                // Content
                tl.Controls.Add(new Label() { Text = "Nội dung:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 3);
                txtNoiDung.Dock = DockStyle.Fill;
                txtNoiDung.Height = 200;
                tl.Controls.Add(txtNoiDung, 1, 3);

                // Status and Owner inner layout
                var inner = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 3 };
                inner.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                inner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                inner.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                inner.RowCount = 1;
                inner.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

                inner.Controls.Add(new Label() { Text = "Trạng thái:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
                txtTrangThai.Dock = DockStyle.Fill;
                inner.Controls.Add(txtTrangThai, 1, 0);

                var ownerPanel = new FlowLayoutPanel() { FlowDirection = FlowDirection.LeftToRight, AutoSize = true, Anchor = AnchorStyles.Right };
                ownerPanel.Controls.Add(new Label() { Text = "Người chủ trì (ID):", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft });
                nudNguoiChuTri.Width = 80;
                nudNguoiChuTri.Margin = new Padding(6, 0, 0, 0);
                ownerPanel.Controls.Add(nudNguoiChuTri);
                inner.Controls.Add(ownerPanel, 2, 0);

                tl.Controls.Add(new Label() { Text = "", AutoSize = true }, 0, 4);
                tl.Controls.Add(inner, 1, 4);

                // Buttons
                var btnPanel = new FlowLayoutPanel() { FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill };
                btnOk.Width = 90; btnCancel.Width = 90;
                btnOk.Margin = new Padding(0, 12, 8, 0);
                btnCancel.Margin = new Padding(0, 12, 0, 0);
                btnPanel.Controls.Add(btnCancel);
                btnPanel.Controls.Add(btnOk);
                tl.Controls.Add(new Label() { Text = "", AutoSize = true }, 0, 5);
                tl.Controls.Add(btnPanel, 1, 5);

                this.Controls.Add(tl);
                this.AcceptButton = btnOk;
                this.CancelButton = btnCancel;
            }

            private void BtnOk_Click(object sender, EventArgs e)
            {
                Meeting.TieuDe = txtTitle.Text.Trim();
                Meeting.NgayHop = dtpDate.Value;
                Meeting.DiaDiem = txtLocation.Text.Trim();
                Meeting.NoiDung = txtNoiDung.Text.Trim();
                Meeting.TrangThai = txtTrangThai.Text.Trim();
                Meeting.NguoiChuTri = (int)nudNguoiChuTri.Value;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        // Form to collect settings for sending to all participants
        public class SendAllForm : Form
        {
            private TextBox txtSender;
            private TextBox txtPassword;
            private TextBox txtSmtp;
            private NumericUpDown nudPort;
            private CheckBox chkSsl;
            private TextBox txtSubject;
            private TextBox txtBody;
            private Button btnSend;
            private Button btnCancel;
            private Meeting meeting;

            public string SenderEmail => txtSender.Text.Trim();
            public string Password => txtPassword.Text;
            public string SmtpHost => txtSmtp.Text.Trim();
            public int Port => (int)nudPort.Value;
            public bool EnableSsl => chkSsl.Checked;
            public string Subject => txtSubject.Text;
            public string Body => txtBody.Text;

            public SendAllForm(Meeting meeting)
            {
                this.meeting = meeting;
                InitializeForm();
            }

            private void InitializeForm()
            {
                this.Text = "Gửi tất cả";
                this.ClientSize = new Size(560, 520);
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.StartPosition = FormStartPosition.CenterParent;

                var tl = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), ColumnCount = 2 };
                tl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
                tl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

                tl.RowCount = 8;
                for (int i = 0; i < tl.RowCount; i++) tl.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
                tl.RowStyles[6] = new RowStyle(SizeType.Percent, 100F);
                tl.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));

                tl.Controls.Add(new Label { Text = "Email gửi:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
                txtSender = new TextBox { Dock = DockStyle.Fill };
                tl.Controls.Add(txtSender, 1, 0);

                tl.Controls.Add(new Label { Text = "Mật khẩu:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 1);
                txtPassword = new TextBox { Dock = DockStyle.Fill, UseSystemPasswordChar = true };
                tl.Controls.Add(txtPassword, 1, 1);

                tl.Controls.Add(new Label { Text = "SMTP host:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 2);
                txtSmtp = new TextBox { Dock = DockStyle.Fill, Text = "smtp.gmail.com" };
                tl.Controls.Add(txtSmtp, 1, 2);

                tl.Controls.Add(new Label { Text = "Port:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 3);
                nudPort = new NumericUpDown { Minimum = 1, Maximum = 65535, Value = 587 };
                tl.Controls.Add(nudPort, 1, 3);

                tl.Controls.Add(new Label { Text = "SSL/TLS:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 4);
                chkSsl = new CheckBox { Checked = true, Anchor = AnchorStyles.Left };
                tl.Controls.Add(chkSsl, 1, 4);

                tl.Controls.Add(new Label { Text = "Tiêu đề:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 5);
                txtSubject = new TextBox { Dock = DockStyle.Fill, Text = $"Thông báo: {meeting?.TieuDe ?? "Cuộc họp"}" };
                tl.Controls.Add(txtSubject, 1, 5);

                tl.Controls.Add(new Label { Text = "Nội dung:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 6);
                txtBody = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Fill, Text = $"Kính gửi thành viên,\n\nXin mời tham gia cuộc họp: {meeting?.TieuDe}\nThời gian: {meeting?.NgayHop}\nĐịa điểm: {meeting?.DiaDiem}\n\nTrân trọng,\nBan tổ chức" };
                tl.Controls.Add(txtBody, 1, 6);

                var btnPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
                btnSend = new Button { Text = "Gửi", Width = 90 };
                btnCancel = new Button { Text = "Hủy", Width = 90 };
                btnSend.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
                btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
                btnPanel.Controls.Add(btnCancel);
                btnPanel.Controls.Add(btnSend);

                tl.Controls.Add(new Label { Text = "", AutoSize = true }, 0, 7);
                tl.Controls.Add(btnPanel, 1, 7);

                this.Controls.Add(tl);
            }
        }
    }
}
