using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;

namespace ClubManageApp
{
    public partial class ucMeeting : UserControl
    {
        private EventInfo selectedEvent = null; // Lưu sự kiện được chọn
        private DateTime currentMonth; // Tháng hiện tại đang hiển thị
        private CultureInfo culture = new CultureInfo("vi-VN"); // Culture tiếng Việt
        private const int PANEL_MARGIN = 2; // Margin giữa các panel
        private List<EventInfo> events; // Danh sách tất cả sự kiện
        private DateTime selectedDate; // Ngày được chọn
        private List<Participant> participants; // Danh sách thành viên
        private string connectionString = @"Data Source=DESKTOP-EJIGPN3;Initial Catalog=QL_APP_LSC;Persist Security Info=True;User ID=sa;Password=1234;Encrypt=True;TrustServerCertificate=True";

        public ucMeeting()
        {
            InitializeComponent();
            currentMonth = DateTime.Now; // Khởi tạo tháng hiện tại
            events = new List<EventInfo>(); // Khởi tạo danh sách sự kiện
            participants = new List<Participant>(); // Khởi tạo danh sách thành viên
            
            // Load participants from database
            LoadParticipants();
            
            // Thêm một số sự kiện mẫu
            AddSampleEvents();
            
            LoadCalendar();
            InitializeMonthYearSelector();
            ClearEventInfo();
        }

        // Load danh sách thành viên từ database
        private void LoadParticipants()
        {
            try
            {
                participants.Clear();
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
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
            catch (Exception ex)
            {
                // Nếu không kết nối được database, thêm dữ liệu mẫu
                AddSampleParticipants();
            }
        }

        // Thêm participant mẫu nếu không load được từ database
        private void AddSampleParticipants()
        {
            participants.Add(new Participant { Id = 1, HoTen = "Nguyễn Vương Khang", Email = "khang@student.hcmute.edu.vn", Lop = "DHKTPM17A", VaiTro = "Thành viên" });
            participants.Add(new Participant { Id = 2, HoTen = "Nguyễn Thị Lan", Email = "lan@student.hcmute.edu.vn", Lop = "DHKTPM17A", VaiTro = "Thành viên" });
            participants.Add(new Participant { Id = 3, HoTen = "Lê Quốc Bảo", Email = "bao@student.hcmute.edu.vn", Lop = "DHKTPM17B", VaiTro = "Thành viên" });
        }

        // Xử lý nút Gửi email
        private async void btnGuiEmail_Click(object sender, EventArgs e)
        {
            if (selectedEvent == null)
            {
                MessageBox.Show("Vui lòng chọn một sự kiện để gửi thông báo!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mở dialog để nhập thông tin email
            using (var dlg = new SendAllEventsForm(selectedEvent))
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
                    MessageBox.Show("Không có thành viên nào có email trong hệ thống!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var errors = new List<string>();
                var succeeded = 0;

                // Hiển thị progress
                btnGuiEmail.Enabled = false;
                btnGuiEmail.Text = "⏳ Đang gửi...";

                // Gửi email tuần tự
                await Task.Run(() =>
                {
                    foreach (var recipient in recipients)
                    {
                        try
                        {
                            using (var msg = new MailMessage())
                            {
                                msg.From = new MailAddress(from);
                                msg.To.Add(new MailAddress(recipient.Email));
                                msg.Subject = subject;
                                // Personalize body
                                msg.Body = bodyTemplate
                                    .Replace("{Name}", recipient.HoTen)
                                    .Replace("{EventTitle}", selectedEvent.Title)
                                    .Replace("{EventDate}", selectedEvent.Date.ToString("dd/MM/yyyy", culture))
                                    .Replace("{EventTime}", $"{selectedEvent.StartTime} - {selectedEvent.EndTime}")
                                    .Replace("{EventLocation}", selectedEvent.Location ?? "");
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
                            errors.Add($"{recipient.HoTen} <{recipient.Email}>: {ex.Message}");
                        }
                    }
                });

                btnGuiEmail.Enabled = true;
                btnGuiEmail.Text = "📧 Gửi email";

                var msgSum = $"Gửi xong!\n\nThành công: {succeeded}/{recipients.Count}\nThất bại: {errors.Count}";
                if (errors.Count > 0)
                {
                    msgSum += "\n\nChi tiết lỗi (tối đa 5):\n" + string.Join("\n", errors.Take(5));
                }
                MessageBox.Show(msgSum, "Kết quả gửi email", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Thêm sự kiện mẫu để demo
        private void AddSampleEvents()
        {
            events.Add(new EventInfo
            {
                Id = 1,
                Title = "Họp nhóm dự án",
                Date = DateTime.Now.Date,
                StartTime = "14:00",
                EndTime = "16:00",
                Location = "Phòng họp A101",
                Description = "Họp bàn về tiến độ dự án ClubManageApp.\n\nNội dung:\n- Review code hiện tại\n- Phân công công việc tuần tới\n- Thảo luận về giao diện mới"
            });

            events.Add(new EventInfo
            {
                Id = 2,
                Title = "Training C# căn bản",
                Date = DateTime.Now.Date,
                StartTime = "09:00",
                EndTime = "11:00",
                Location = "Phòng học B202",
                Description = "Khóa học C# cho thành viên mới."
            });
        }

        // Lấy danh sách sự kiện theo ngày
        private List<EventInfo> GetEventsByDate(DateTime date)
        {
            return events.Where(e => e.Date.Date == date.Date).OrderBy(e => e.StartTime).ToList();
        }

        // Khởi tạo chức năng chọn tháng/năm nhanh
        private void InitializeMonthYearSelector()
        {
            lblThangNam.Cursor = Cursors.Hand;
            lblThangNam.Click += LblThangNam_Click;
            
            // Thêm tooltip để người dùng biết có thể click
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(lblThangNam, "Click để chọn tháng/năm khác");
        }

        // Xử lý khi click vào label tháng năm
        private void LblThangNam_Click(object sender, EventArgs e)
        {
            // Tạo form tùy chỉnh để chọn tháng và năm
            Form monthYearForm = new Form
            {
                Text = "Chọn Tháng và Năm",
                Size = new Size(350, 220),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblMonth = new Label
            {
                Text = "Tháng:",
                Location = new Point(30, 30),
                Size = new Size(60, 25),
                Font = new Font("Segoe UI", 10F)
            };

            ComboBox cmbMonth = new ComboBox
            {
                Location = new Point(100, 28),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };

            // Thêm các tháng
            for (int i = 1; i <= 12; i++)
            {
                cmbMonth.Items.Add($"Tháng {i}");
            }
            cmbMonth.SelectedIndex = currentMonth.Month - 1;

            Label lblYear = new Label
            {
                Text = "Năm:",
                Location = new Point(30, 70),
                Size = new Size(60, 25),
                Font = new Font("Segoe UI", 10F)
            };

            NumericUpDown nudYear = new NumericUpDown
            {
                Location = new Point(100, 68),
                Size = new Size(200, 25),
                Minimum = 1900,
                Maximum = 2100,
                Value = currentMonth.Year,
                Font = new Font("Segoe UI", 10F)
            };

            Button btnOK = new Button
            {
                Text = "Xác nhận",
                Location = new Point(80, 120),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            btnOK.FlatAppearance.BorderSize = 0;

            Button btnCancel = new Button
            {
                Text = "Hủy",
                Location = new Point(190, 120),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            monthYearForm.Controls.Add(lblMonth);
            monthYearForm.Controls.Add(cmbMonth);
            monthYearForm.Controls.Add(lblYear);
            monthYearForm.Controls.Add(nudYear);
            monthYearForm.Controls.Add(btnOK);
            monthYearForm.Controls.Add(btnCancel);

            monthYearForm.AcceptButton = btnOK;
            monthYearForm.CancelButton = btnCancel;

            if (monthYearForm.ShowDialog() == DialogResult.OK)
            {
                int selectedMonth = cmbMonth.SelectedIndex + 1;
                int selectedYear = (int)nudYear.Value;
                
                // Cập nhật tháng hiện tại
                currentMonth = new DateTime(selectedYear, selectedMonth, 1);
                LoadCalendar();
                ClearEventInfo();
            }
        }

        // Tạo tiêu đề các thứ trong tuần
        private void InitializeDaysOfWeekHeader(int panelWidth)
        {
            string[] daysOfWeek = { "Chủ Nhật", "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy" };
            
            pnlDaysOfWeek.Controls.Clear();
            
            for (int i = 0; i < 7; i++)
            {
                Label lblDay = new Label
                {
                    Text = daysOfWeek[i],
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Width = panelWidth,
                    Height = 50,
                    Left = i * panelWidth,
                    Top = 0,
                    BorderStyle = BorderStyle.None
                };
                pnlDaysOfWeek.Controls.Add(lblDay);
            }
        }

        // Tải lịch cho tháng hiện tại
        private void LoadCalendar()
        {
            flowLayoutPanel1.Controls.Clear();
            UpdateMonthDisplay();

            // Lấy ngày đầu tiên của tháng
            DateTime firstDayOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            
            // Lấy số ngày trong tháng
            int daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
            
            // Lấy thứ của ngày đầu tiên (0 = Chủ Nhật, 1 = Thứ Hai, ...)
            int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            
            // Tính toán kích thước panel - phải khớp với header
            // Trừ đi padding của flowLayoutPanel
            int totalWidth = flowLayoutPanel1.Width - flowLayoutPanel1.Padding.Left - flowLayoutPanel1.Padding.Right;
            int panelWidth = totalWidth / 7;
            int panelHeight = 100;

            // Khởi tạo header với cùng width
            InitializeDaysOfWeekHeader(panelWidth);

            // Thêm các panel trống cho những ngày trước ngày 1
            for (int i = 0; i < startDayOfWeek; i++)
            {
                Panel emptyPanel = CreateEmptyDayPanel(panelWidth, panelHeight);
                flowLayoutPanel1.Controls.Add(emptyPanel);
            }

            // Thêm các panel cho các ngày trong tháng
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime currentDate = new DateTime(currentMonth.Year, currentMonth.Month, day);
                Panel dayPanel = CreateDayPanel(day, currentDate, panelWidth, panelHeight);
                flowLayoutPanel1.Controls.Add(dayPanel);
            }
        }

        // Tạo panel ngày trống
        private Panel CreateEmptyDayPanel(int width, int height)
        {
            Panel panel = new Panel
            {
                Width = width - 1, // Trừ 1 để tính border
                Height = height,
                BackColor = Color.FromArgb(245, 245, 245),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0)
            };
            return panel;
        }

        // Tạo panel cho một ngày
        private Panel CreateDayPanel(int day, DateTime date, int width, int height)
        {
            Panel panel = new Panel
            {
                Width = width - 1, // Trừ 1 để tính border
                Height = height,
                BackColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Margin = new Padding(0),
                Tag = date // Lưu ngày vào Tag
            };

            // Highlight ngày hiện tại
            if (date.Date == DateTime.Now.Date)
            {
                panel.BackColor = Color.FromArgb(255, 243, 205); // Màu vàng nhạt
            }

            // Label hiển thị số ngày
            Label lblDay = new Label
            {
                Text = day.ToString(),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                AutoSize = false,
                Width = 40,
                Height = 25,
                Location = new Point(5, 5),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Highlight ngày chủ nhật
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                lblDay.ForeColor = Color.Red;
            }

            panel.Controls.Add(lblDay);

            // Hiển thị số lượng sự kiện (nếu có)
            List<EventInfo> dayEvents = GetEventsByDate(date);
            if (dayEvents.Count > 0)
            {
                Label lblEventCount = new Label
                {
                    Text = $"🗓️ {dayEvents.Count} sự kiện",
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = Color.FromArgb(52, 152, 219),
                    AutoSize = false,
                    Width = width - 10,
                    Height = 20,
                    Location = new Point(5, height - 25),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                panel.Controls.Add(lblEventCount);
                lblEventCount.Click += (s, e) => DayPanel_Click(panel, e);
            }

            panel.Click += DayPanel_Click;
            lblDay.Click += (s, e) => DayPanel_Click(panel, e);

            return panel;
        }

        // Xử lý khi click vào panel ngày
        private void DayPanel_Click(object sender, EventArgs e)
        {
            Panel clickedPanel = sender as Panel;
            if (clickedPanel != null && clickedPanel.Tag != null)
            {
                selectedDate = (DateTime)clickedPanel.Tag;
                
                // Reset màu tất cả các panel
                foreach (Control control in flowLayoutPanel1.Controls)
                {
                    if (control is Panel panel && panel.Tag != null)
                    {
                        DateTime panelDate = (DateTime)panel.Tag;
                        if (panelDate.Date == DateTime.Now.Date)
                        {
                            panel.BackColor = Color.FromArgb(255, 243, 205);
                        }
                        else
                        {
                            panel.BackColor = Color.FromArgb(240, 240, 240);
                        }
                    }
                }

                // Highlight panel được chọn
                clickedPanel.BackColor = Color.FromArgb(52, 152, 219);
                
                // Hiển thị danh sách sự kiện của ngày được chọn
                DisplayEventsForDate(selectedDate);
            }
        }

        // Hiển thị danh sách sự kiện cho ngày được chọn
        private void DisplayEventsForDate(DateTime date)
        {
            List<EventInfo> dayEvents = GetEventsByDate(date);
            
            lblTieuDe.Text = $"Sự kiện ngày {date:dd/MM/yyyy}";
            lblNgay.Visible = false;
            lblThoiGian.Visible = false;
            lblMoTa.Visible = false;

            // Xóa và load lại ListBox
            lstEvents.Items.Clear();
            lstEvents.DisplayMember = "Title"; // Hiển thị Title trong ListBox
            
            if (dayEvents.Count > 0)
            {
                lblDanhSachSuKien.Text = $"Danh sách sự kiện: ({dayEvents.Count})";
                lblDanhSachSuKien.Visible = true;
                lstEvents.Visible = true;

                foreach (var evt in dayEvents)
                {
                    // Tạo item hiển thị với format đẹp
                    string displayText = $"{evt.StartTime} - {evt.Title}";
                    lstEvents.Items.Add(evt);
                    // Override ToString để hiển thị đúng format
                    int index = lstEvents.Items.Count - 1;
                }

                // Tự động chọn item đầu tiên
                if (lstEvents.Items.Count > 0)
                {
                    lstEvents.SelectedIndex = 0;
                }
                
                txtMoTa.Text = "Chọn một sự kiện từ danh sách phía trên để xem chi tiết.";
            }
            else
            {
                lblDanhSachSuKien.Text = "Danh sách sự kiện: (0)";
                lblDanhSachSuKien.Visible = true;
                lstEvents.Visible = false;
                txtMoTa.Text = "Chưa có sự kiện nào trong ngày này.\nClick nút 'Thêm sự kiện mới' để thêm sự kiện.";
                selectedEvent = null;
                btnSuaSuKien.Enabled = false;
                btnXoaSuKien.Enabled = false;
            }
        }

        // Xử lý khi chọn sự kiện trong ListBox
        private void lstEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstEvents.SelectedItem != null)
            {
                selectedEvent = (EventInfo)lstEvents.SelectedItem;
                
                // Hiển thị chi tiết sự kiện
                lblNgay.Text = $"Ngày: {selectedEvent.Date:dd/MM/yyyy}";
                lblNgay.Visible = true;
                
                lblThoiGian.Text = $"Thời gian: {selectedEvent.StartTime} - {selectedEvent.EndTime}";
                lblThoiGian.Visible = true;
                
                lblMoTa.Text = "Mô tả:";
                lblMoTa.Visible = true;

                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(selectedEvent.Location))
                    sb.AppendLine($"📍 Địa điểm: {selectedEvent.Location}\n");
                
                if (!string.IsNullOrEmpty(selectedEvent.Description))
                    sb.AppendLine(selectedEvent.Description);
                else
                    sb.AppendLine("(Không có mô tả)");

                txtMoTa.Text = sb.ToString();
                
                // Kích hoạt các nút chức năng
                btnSuaSuKien.Enabled = true;
                btnXoaSuKien.Enabled = true;
            }
            else
            {
                selectedEvent = null;
                btnSuaSuKien.Enabled = false;
                btnXoaSuKien.Enabled = false;
            }
        }

        // Cập nhật hiển thị tháng năm
        private void UpdateMonthDisplay()
        {
            lblThangNam.Text = $"Tháng {currentMonth.Month} - {currentMonth.Year}";
        }

        // Xóa thông tin hiển thị
        private void ClearEventInfo()
        {
            lblTieuDe.Text = "Chọn một ngày để xem sự kiện";
            lblNgay.Visible = false;
            lblThoiGian.Visible = false;
            lblMoTa.Visible = false;
            lblDanhSachSuKien.Visible = false;
            lstEvents.Visible = false;
            lstEvents.Items.Clear();
            txtMoTa.Text = "Click vào một ngày trong lịch để xem danh sách sự kiện...";
            
            btnSuaSuKien.Enabled = false;
            btnXoaSuKien.Enabled = false;
            selectedEvent = null;
            
            // Reset màu các panel
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control is Panel panel && panel.Tag != null)
                {
                    DateTime panelDate = (DateTime)panel.Tag;
                    if (panelDate.Date == DateTime.Now.Date)
                    {
                        panel.BackColor = Color.FromArgb(255, 243, 205);
                    }
                    else
                    {
                        panel.BackColor = Color.FromArgb(240, 240, 240);
                    }
                }
            }
        }

        // Xử lý nút Thêm sự kiện
        private void btnThemSuKien_Click(object sender, EventArgs e)
        {
            DateTime dateForNewEvent = lstEvents.Items.Count > 0 ? selectedDate : DateTime.Now;
            
            frmAddEditEvent frm = new frmAddEditEvent(dateForNewEvent);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Tạo ID mới
                frm.Event.Id = events.Count > 0 ? events.Max(ev => ev.Id) + 1 : 1;
                
                events.Add(frm.Event);
                LoadCalendar();
                
                // Hiển thị lại sự kiện của ngày đó
                if (lstEvents.Items.Count > 0 || dateForNewEvent == selectedDate)
                {
                    DisplayEventsForDate(frm.Event.Date);
                    selectedDate = frm.Event.Date;
                    
                    // Highlight lại panel của ngày vừa thêm
                    foreach (Control control in flowLayoutPanel1.Controls)
                    {
                        if (control is Panel panel && panel.Tag != null)
                        {
                            DateTime panelDate = (DateTime)panel.Tag;
                            if (panelDate.Date == frm.Event.Date.Date)
                            {
                                panel.BackColor = Color.FromArgb(52, 152, 219);
                            }
                        }
                    }
                }
                
                MessageBox.Show("Đã thêm sự kiện thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Xử lý nút Sửa sự kiện
        private void btnSuaSuKien_Click(object sender, EventArgs e)
        {
            if (selectedEvent != null)
            {
                frmAddEditEvent frm = new frmAddEditEvent(selectedEvent.Date, selectedEvent);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Cập nhật sự kiện trong danh sách
                    int index = events.FindIndex(ev => ev.Id == selectedEvent.Id);
                    if (index >= 0)
                    {
                        events[index] = frm.Event;
                        events[index].Id = selectedEvent.Id; // Giữ nguyên ID
                    }
                    
                    LoadCalendar();
                    DisplayEventsForDate(selectedDate);
                    
                    MessageBox.Show("Đã cập nhật sự kiện thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sự kiện để sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Xử lý nút Xóa sự kiện
        private void btnXoaSuKien_Click(object sender, EventArgs e)
        {
            if (selectedEvent != null)
            {
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa sự kiện:\n\n'{selectedEvent.Title}'\n\nTrong ngày {selectedEvent.Date:dd/MM/yyyy} lúc {selectedEvent.StartTime}?", 
                    "Xác nhận xóa", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning);
                
                if (result == DialogResult.Yes)
                {
                    events.RemoveAll(ev => ev.Id == selectedEvent.Id);
                    
                    LoadCalendar();
                    DisplayEventsForDate(selectedDate);
                    
                    MessageBox.Show("Đã xóa sự kiện thành công!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sự kiện để xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Xử lý nút Tháng trước
        private void btnThangTruoc_Click(object sender, EventArgs e)
        {
            // Chuyển sang tháng trước
            currentMonth = currentMonth.AddMonths(-1);
            LoadCalendar();
            ClearEventInfo();
        }

        // Xử lý nút Tháng sau
        private void btnThangSau_Click(object sender, EventArgs e)
        {
            // Chuyển sang tháng sau
            currentMonth = currentMonth.AddMonths(1);
            LoadCalendar();
            ClearEventInfo();
        }

        // Xử lý placeholder cho TextBox tìm kiếm
        private void txtTimKiem_Enter(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "Nhập tên sự kiện...")
            {
                txtTimKiem.Text = "";
                txtTimKiem.ForeColor = Color.Black;
            }
        }

        private void txtTimKiem_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
            {
                txtTimKiem.Text = "Nhập tên sự kiện...";
                txtTimKiem.ForeColor = Color.Gray;
            }
        }

        private void txtTimKiem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnTimKiem_Click(sender, e);
                e.Handled = true;
            }
        }

        // Xử lý nút Tìm kiếm
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(keyword) || keyword == "Nhập tên sự kiện...")
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTimKiem.Focus();
                return;
            }

            // Tìm kiếm sự kiện theo title
            List<EventInfo> searchResults = events.Where(evt => 
                evt.Title.ToLower().Contains(keyword.ToLower()) ||
                (evt.Description != null && evt.Description.ToLower().Contains(keyword.ToLower())) ||
                (evt.Location != null && evt.Location.ToLower().Contains(keyword.ToLower()))
            ).OrderBy(evt => evt.Date).ThenBy(evt => evt.StartTime).ToList();

            if (searchResults.Count == 0)
            {
                MessageBox.Show($"Không tìm thấy sự kiện nào với từ khóa '{keyword}'!", 
                    "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Hiển thị kết quả tìm kiếm
            ShowSearchResults(searchResults, keyword);
        }

        // Hiển thị kết quả tìm kiếm
        private void ShowSearchResults(List<EventInfo> results, string keyword)
        {
            Form searchForm = new Form
            {
                Text = $"Kết quả tìm kiếm: '{keyword}'",
                Size = new Size(700, 500),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.Sizable,
                MinimizeBox = false,
                MaximizeBox = true
            };

            Label lblResults = new Label
            {
                Text = $"Tìm thấy {results.Count} sự kiện:",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            ListBox lstResults = new ListBox
            {
                Location = new Point(20, 50),
                Size = new Size(640, 300),
                Font = new Font("Segoe UI", 10F)
            };

            foreach (var evt in results)
            {
                string item = $"{evt.Date:dd/MM/yyyy} - {evt.StartTime} - {evt.Title}";
                lstResults.Items.Add(evt);
            }

            TextBox txtDetail = new TextBox
            {
                Location = new Point(20, 360),
                Size = new Size(640, 60),
                Multiline = true,
                ReadOnly = true,
                Font = new Font("Segoe UI", 9F),
                ScrollBars = ScrollBars.Vertical
            };

            lstResults.SelectedIndexChanged += (s, e) =>
            {
                if (lstResults.SelectedItem != null)
                {
                    EventInfo selected = (EventInfo)lstResults.SelectedItem;
                    txtDetail.Text = $"Địa điểm: {selected.Location}\r\nMô tả: {selected.Description}";
                }
            };

            Button btnViewInCalendar = new Button
            {
                Text = "Xem trong lịch",
                Location = new Point(480, 430),
                Size = new Size(180, 35),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnViewInCalendar.FlatAppearance.BorderSize = 0;
            btnViewInCalendar.Click += (s, e) =>
            {
                if (lstResults.SelectedItem != null)
                {
                    EventInfo selected = (EventInfo)lstResults.SelectedItem;
                    currentMonth = new DateTime(selected.Date.Year, selected.Date.Month, 1);
                    LoadCalendar();
                    selectedDate = selected.Date;
                    DisplayEventsForDate(selected.Date);
                    
                    // Highlight panel
                    foreach (Control control in flowLayoutPanel1.Controls)
                    {
                        if (control is Panel panel && panel.Tag != null)
                        {
                            DateTime panelDate = (DateTime)panel.Tag;
                            if (panelDate.Date == selected.Date.Date)
                            {
                                panel.BackColor = Color.FromArgb(52, 152, 219);
                            }
                        }
                    }
                    
                    searchForm.Close();
                }
            };

            searchForm.Controls.Add(lblResults);
            searchForm.Controls.Add(lstResults);
            searchForm.Controls.Add(txtDetail);
            searchForm.Controls.Add(btnViewInCalendar);

            searchForm.ShowDialog();
        }

        // Xử lý nút Xuất file
        private void btnXuatFile_Click(object sender, EventArgs e)
        {
            if (events.Count == 0)
            {
                MessageBox.Show("Không có sự kiện nào để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt",
                    DefaultExt = "csv",
                    FileName = $"DanhSachSuKien_{DateTime.Now:yyyyMMdd_HHmmss}"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(saveDialog.FileName);
                    
                    DialogResult result = MessageBox.Show(
                        $"Đã xuất file thành công!\n\nĐường dẫn: {saveDialog.FileName}\n\nBạn có muốn mở file không?",
                        "Thành công",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất file: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xuất dữ liệu ra file CSV
        private void ExportToCSV(string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Header
                sw.WriteLine("STT,Tiêu đề,Ngày,Giờ bắt đầu,Giờ kết thúc,Địa điểm,Mô tả");

                // Data
                var sortedEvents = events.OrderBy(e => e.Date).ThenBy(e => e.StartTime).ToList();
                int stt = 1;

                foreach (var evt in sortedEvents)
                {
                    string line = $"{stt}," +
                                $"\"{evt.Title}\"," +
                                $"{evt.Date:dd/MM/yyyy}," +
                                $"{evt.StartTime}," +
                                $"{evt.EndTime}," +
                                $"\"{evt.Location ?? ""}\"," +
                                $"\"{evt.Description?.Replace("\"", "\"\"") ?? ""}\"";
                    
                    sw.WriteLine(line);
                    stt++;
                }
            }
        }

        // Form để nhập thông tin gửi email cho tất cả thành viên
        public class SendAllEventsForm : Form
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
            private EventInfo eventInfo;

            public string SenderEmail => txtSender.Text.Trim();
            public string Password => txtPassword.Text;
            public string SmtpHost => txtSmtp.Text.Trim();
            public int Port => (int)nudPort.Value;
            public bool EnableSsl => chkSsl.Checked;
            public string Subject => txtSubject.Text;
            public string Body => txtBody.Text;

            public SendAllEventsForm(EventInfo evt)
            {
                this.eventInfo = evt;
                InitializeForm();
            }

            private void InitializeForm()
            {
                this.Text = "Gửi thông báo sự kiện";
                this.ClientSize = new Size(560, 520);
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.StartPosition = FormStartPosition.CenterParent;
                this.MaximizeBox = false;
                this.MinimizeBox = false;

                var tl = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), ColumnCount = 2 };
                tl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
                tl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

                tl.RowCount = 8;
                for (int i = 0; i < 7; i++) tl.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
                tl.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                tl.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));

                tl.Controls.Add(new Label { Text = "Email gửi:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
                txtSender = new TextBox { Dock = DockStyle.Fill, Text = "khoingotuantu.305@gmail.com" }; // ← THAY ĐỔI EMAIL CỦA BẠN TẠI ĐÂY
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
                txtSubject = new TextBox { Dock = DockStyle.Fill, Text = $"Thông báo: {eventInfo?.Title ?? "Sự kiện"}" };
                tl.Controls.Add(txtSubject, 1, 5);

                tl.Controls.Add(new Label { Text = "Nội dung:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 6);
                txtBody = new TextBox 
                { 
                    Multiline = true, 
                    ScrollBars = ScrollBars.Vertical, 
                    Dock = DockStyle.Fill, 
                    Text = $"Kính gửi {{Name}},\n\nBạn nhận được thông báo về sự kiện:\n\n" +
                           $"📅 Sự kiện: {{EventTitle}}\n" +
                           $"📆 Ngày: {{EventDate}}\n" +
                           $"⏰ Thời gian: {{EventTime}}\n" +
                           $"📍 Địa điểm: {{EventLocation}}\n\n" +
                           $"Vui lòng sắp xếp thời gian tham dự.\n\n" +
                           $"Trân trọng,\nBan tổ chức" 
                };
                tl.Controls.Add(txtBody, 1, 6);

                var btnPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
                btnSend = new Button { Text = "Gửi tất cả", Width = 110 };
                btnCancel = new Button { Text = "Hủy", Width = 90 };
                btnSend.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
                btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
                btnPanel.Controls.Add(btnCancel);
                btnPanel.Controls.Add(btnSend);

                tl.Controls.Add(new Label { Text = "", AutoSize = true }, 0, 8);
                tl.Controls.Add(btnPanel, 1, 8);

                this.Controls.Add(tl);
            }
        }
    }
}
