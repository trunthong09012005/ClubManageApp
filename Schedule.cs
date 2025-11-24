using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class ucSchedule : UserControl
    {
        private BindingList<Meeting> meetings = new BindingList<Meeting>();
        private BindingList<Participant> participants = new BindingList<Participant>();
        private BindingList<Minute> minutes = new BindingList<Minute>();
        private BindingList<KyLuat> kyLuats = new BindingList<KyLuat>();

        public ucSchedule()
        {
            InitializeComponent();
            InitializeData();
            WireGrids();
        }

        private void InitializeData()
        {
            // Meetings from provided SQL inserts (LichHop)
            meetings.Add(new Meeting { Id = 1, TieuDe = "Họp tổng kết quý I/2025", NgayHop = DateTime.Parse("2025-04-15 14:00"), DiaDiem = "Phòng họp A101", NoiDung = "Tổng kết hoạt động quý I và lập kế hoạch quý II", NguoiChuTri = 2, TrangThai = "Hoàn thành" });
            meetings.Add(new Meeting { Id = 2, TieuDe = "Họp chuẩn bị Mùa hè xanh", NgayHop = DateTime.Parse("2025-06-20 15:00"), DiaDiem = "Phòng CLB", NoiDung = "Thảo luận kế hoạch chi tiết cho chiến dịch Mùa hè xanh", NguoiChuTri = 2, TrangThai = "Hoàn thành" });
            meetings.Add(new Meeting { Id = 3, TieuDe = "Họp Ban chủ nhiệm tháng 8", NgayHop = DateTime.Parse("2025-08-05 16:00"), DiaDiem = "Phòng họp B201", NoiDung = "Đánh giá hoạt động tháng 7 và kế hoạch tháng 8", NguoiChuTri = 2, TrangThai = "Hoàn thành" });
            meetings.Add(new Meeting { Id = 4, TieuDe = "Họp toàn thể CLB - Kế hoạch tuyển thành viên", NgayHop = DateTime.Parse("2025-09-15 14:00"), DiaDiem = "Hội trường lớn", NoiDung = "Công bố kế hoạch tuyển thành viên mới và phân công nhiệm vụ", NguoiChuTri = 2, TrangThai = "Hoàn thành" });
            meetings.Add(new Meeting { Id = 5, TieuDe = "Họp Ban chủ nhiệm tháng 11", NgayHop = DateTime.Parse("2025-11-18 15:00"), DiaDiem = "Phòng họp A101", NoiDung = "Thảo luận kế hoạch cuối năm và chuẩn bị đại hội", NguoiChuTri = 2, TrangThai = "Sắp diễn ra" });

            // Participants from provided ThanhVien inserts
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

            // Sample minutes (unchanged)
            //            minutes.Add(new Minute { Id = 1, MeetingId = 1, Title = "Minutes - Monthly Committee", Content = "Discussed budget and events." });
            //            minutes.Add(new Minute { Id = 2, MeetingId = 2, Title = "Minutes - Project Kickoff", Content = "Defined scope and owners." });
            // biên bản mẫu đã được thay bằng dữ liệu Kỷ luật

            // KyLuat (disciplinary records) from provided INSERTs
            // INSERT INTO KyLuat (MaTV, LyDo, HinhThuc, ThoiGianKyLuat, NgayKL, NguoiLap) VALUES
            // (10, N'Vắng mặt không phép 3 buổi họp liên tiếp', N'Cảnh cáo', NULL, '2025-10-05', 2),
            // (11, N'Không hoàn thành nhiệm vụ đúng thời hạn', N'Khiển trách', NULL, '2025-08-25', 2);
            kyLuats.Add(new KyLuat { Id = 1, MaTV = 10, LyDo = "Vắng mặt không phép 3 buổi họp liên tiếp", HinhThuc = "Cảnh cáo", ThoiGianKyLuat = null, NgayKL = DateTime.Parse("2025-10-05"), NguoiLap = 2 });
            kyLuats.Add(new KyLuat { Id = 2, MaTV = 11, LyDo = "Không hoàn thành nhiệm vụ đúng thời hạn", HinhThuc = "Khiển trách", ThoiGianKyLuat = null, NgayKL = DateTime.Parse("2025-08-25"), NguoiLap = 2 });
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

            // Configure dgvMinutes to show kỷ luật records (KyLuat)
            dgvMinutes.AutoGenerateColumns = false;
            dgvMinutes.Columns.Clear();

            var colLyDo = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof(KyLuat.LyDo),
                Name = nameof(KyLuat.LyDo),
                HeaderText = "Lý do",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            var colHinhThuc = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof(KyLuat.HinhThuc),
                Name = nameof(KyLuat.HinhThuc),
                HeaderText = "Hình thức",
                Width = 120
            };
            var colNgayKL = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof(KyLuat.NgayKL),
                Name = nameof(KyLuat.NgayKL),
                HeaderText = "Ngày KL",
                Width = 120
            };
            var colNguoiLap = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = nameof(KyLuat.NguoiLap),
                Name = nameof(KyLuat.NguoiLap),
                HeaderText = "Người lập",
                Width = 80
            };

            dgvMinutes.Columns.Add(colLyDo);
            dgvMinutes.Columns.Add(colHinhThuc);
            dgvMinutes.Columns.Add(colNgayKL);
            dgvMinutes.Columns.Add(colNguoiLap);

            dgvMinutes.DataSource = null;
            dgvMinutes.DataSource = kyLuats;
            dgvMinutes.AllowUserToAddRows = false;
            dgvMinutes.ReadOnly = true;
            dgvMinutes.RowHeadersVisible = false;
            dgvMinutes.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvMinutes.DefaultCellStyle.ForeColor = Color.Black;
            dgvMinutes.DefaultCellStyle.BackColor = Color.White;
            dgvMinutes.AutoResizeColumns();
            dgvMinutes.Refresh();
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

        private void btnFilter_Click(object sender, EventArgs e)
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
            // Simulate sending
            MessageBox.Show($"Gửi thông báo đến {p.HoTen} <{p.Email}>", "Notification");
        }

        // Minutes actions
        private void btnViewMinute_Click(object sender, EventArgs e)
        {
            if (dgvMinutes.CurrentRow == null) return;
            var item = dgvMinutes.CurrentRow.DataBoundItem;
            if (item is KyLuat k)
            {
                MessageBox.Show($"Mã TV: {k.MaTV}\nLý do: {k.LyDo}\nHình thức: {k.HinhThuc}\nNgày KL: {k.NgayKL:d}\nNgười lập: {k.NguoiLap}", "Chi tiết kỷ luật");
            }
            else if (item is Minute m)
            {
                MessageBox.Show($"{m.Title}\n\n{m.Content}", "Chi tiết");
            }
        }

        private void ucSchedule_Load(object sender, EventArgs e)
        {

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

    public class Participant
    {
        public int Id { get; set; }

        // Columns from ThanhVien table
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string Lop { get; set; }
        public string Khoa { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string VaiTro { get; set; }
        public int MaCV { get; set; }
        public int? MaBan { get; set; }

        // Backwards-compatible alias
        public string Name { get => HoTen; set => HoTen = value; }
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
            this.Size = new Size(480, 320);
            txtTitle = new TextBox() { Left = 20, Top = 20, Width = 420 };
            dtpDate = new DateTimePicker() { Left = 20, Top = 60, Width = 250, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm" };
            txtLocation = new TextBox() { Left = 20, Top = 100, Width = 420 };
            txtNoiDung = new TextBox() { Left = 20, Top = 140, Width = 420, Height = 40, Multiline = true };
            txtTrangThai = new TextBox() { Left = 20, Top = 190, Width = 200 };
            nudNguoiChuTri = new NumericUpDown() { Left = 240, Top = 190, Width = 80, Minimum = 0, Maximum = 9999 };
            btnOk = new Button() { Left = 260, Top = 230, Width = 80, Text = "OK" };
            btnCancel = new Button() { Left = 350, Top = 230, Width = 80, Text = "Cancel" };
            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            this.Controls.AddRange(new Control[] { txtTitle, dtpDate, txtLocation, txtNoiDung, txtTrangThai, nudNguoiChuTri, btnOk, btnCancel });
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
}
