using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ClubManageApp
{
    public partial class MemberDashboard : Form
    {
        private string role;
        private string username;
        private int maTV;

        // 🔗 Chuỗi kết nối SQL Server
        private string connectionString = @"Data Source=DESKTOP-EJIGPN3;Initial Catalog=QL_APP_LSC;Persist Security Info=True;User ID=sa;Password=1234;Encrypt=True;TrustServerCertificate=True";
        // Biến cho animation sidebar
        bool sidebarExpand = true;
        private const int SIDEBAR_MAX = 250;
        private const int SIDEBAR_MIN = 70;

        // ========== CHATBOT PANEL ==========
        private ChatbotPanel chatbot;

        // ========== LỊCH HỌP PANEL ==========
        private Panel pnlLichHop;
        private Panel pnlCalendarContainer;
        private Label lblCurrentMonth;
        private Button btnPrevMonth, btnNextMonth, btnToday;
        private DateTime currentViewMonth;
        private Dictionary<DateTime, List<MeetingInfo>> meetingsByDate;


        private class MeetingInfo
        {
            public int MaHD { get; set; }
            public string TenHD { get; set; }
            public string MoTa { get; set; }
            public DateTime NgayToChuc { get; set; }
            public string DiaDiem { get; set; }
            public string TrangThai { get; set; }
            public bool DiemDanh { get; set; }
            public string GhiChu { get; set; }
        }

        public MemberDashboard(string role, string username, int maTV)
        {
            InitializeComponent();
            this.role = role;
            this.username = username;
            this.maTV = maTV;

            this.Load += MemberDashboard_Load;
            this.Resize += MemberDashboard_Resize;
            btnham.Click += btnham_Click;
            slidebarTransition.Tick += slidebarTransition_Tick;

            RegisterMenuEvents();
        }

        private void MemberDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                lblUsername.Text = username;
                lblRole.Text = role;

                LoadMemberProfile();
                LoadStatisticsData();
                LoadActivityTimeline();

                InitializeChatbot();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi khởi tạo form: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeChatbot()
        {
            chatbot = new ChatbotPanel(connectionString, maTV, username);
            chatbot.Location = new Point(
                this.ClientSize.Width - chatbot.Width - 20,
                this.ClientSize.Height - chatbot.Height - 20
            );
            chatbot.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.Controls.Add(chatbot);
            chatbot.BringToFront();
        }

        private void MemberDashboard_Resize(object sender, EventArgs e)
        {
            if (chatbot != null)
            {
                chatbot.Location = new Point(
                    this.ClientSize.Width - chatbot.Width - 20,
                    this.ClientSize.Height - chatbot.Height - 20
                );
                chatbot.BringToFront();
            }
        }

        #region Load Member Profile

        private void LoadMemberProfile()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            TV.HoTen, TV.MaTV, TV.Email, TV.SDT, TV.NgayThamGia,
                            TV.VaiTro, CV.TenCV AS TenChucVu, TV.AnhDaiDien, TV.Lop, TV.Khoa
                        FROM ThanhVien TV
                        LEFT JOIN ChucVu CV ON TV.MaCV = CV.MaCV
                        WHERE TV.MaTV = @maTV";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            lblMemberName.Text = reader["HoTen"].ToString();
                            lblMemberID.Text = "ID: " + reader["MaTV"].ToString();

                            string chucVu = reader["TenChucVu"] != DBNull.Value
                                ? reader["TenChucVu"].ToString()
                                : (reader["VaiTro"] != DBNull.Value ? reader["VaiTro"].ToString() : "Thành viên");
                            lblMemberRole.Text = chucVu;

                            lblEmail.Text = "    " + (reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "Chưa cập nhật");
                            lblPhone.Text = "    " + (reader["SDT"] != DBNull.Value ? reader["SDT"].ToString() : "Chưa cập nhật");

                            if (reader["NgayThamGia"] != DBNull.Value)
                            {
                                DateTime ngayThamGia = Convert.ToDateTime(reader["NgayThamGia"]);
                                lblJoinDate.Text = "    Ngày tham gia: " + ngayThamGia.ToString("dd/MM/yyyy");
                            }

                            if (reader["AnhDaiDien"] != DBNull.Value)
                            {
                                string imagePath = reader["AnhDaiDien"].ToString();
                                if (System.IO.File.Exists(imagePath))
                                    picAvatar.Image = Image.FromFile(imagePath);
                                else
                                    SetDefaultAvatar();
                            }
                            else
                            {
                                SetDefaultAvatar();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin hồ sơ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetDefaultAvatar();
            }
        }

        private void SetDefaultAvatar()
        {
            Bitmap defaultAvatar = new Bitmap(150, 150);
            using (Graphics g = Graphics.FromImage(defaultAvatar))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new LinearGradientBrush(
                    new Rectangle(0, 0, 150, 150),
                    Color.FromArgb(99, 102, 241),
                    Color.FromArgb(168, 85, 247), 45F))
                {
                    g.FillEllipse(brush, 0, 0, 149, 149);
                }
                string initial = username.Length > 0 ? username.Substring(0, 1).ToUpper() : "?";
                using (Font font = new Font("Segoe UI", 48, FontStyle.Bold))
                {
                    SizeF textSize = g.MeasureString(initial, font);
                    float x = (150 - textSize.Width) / 2;
                    float y = (150 - textSize.Height) / 2;
                    g.DrawString(initial, font, Brushes.White, x, y);
                }
            }
            picAvatar.Image = defaultAvatar;
        }

        #endregion

        #region Load Statistics Data

        private void LoadStatisticsData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    int hoatDongCount = GetCount(conn, @"
                        SELECT COUNT(DISTINCT TG.MaHD) FROM ThamGia TG
                        INNER JOIN HoatDong HD ON TG.MaHD = HD.MaHD
                        WHERE TG.MaTV = @maTV AND TG.DiemDanh = 1");
                    lblActivityCount.Text = hoatDongCount.ToString();

                    int duAnCount = GetCount(conn, "SELECT COUNT(*) FROM PhanCong WHERE MaTV = @maTV");
                    lblProjectCount.Text = duAnCount.ToString();

                    object diemRL = GetScalar(conn, @"
                        SELECT TOP 1 Diem FROM DiemRenLuyen
                        WHERE MaTV = @maTV ORDER BY NgayCapNhat DESC");
                    lblPointCount.Text = diemRL != null ? diemRL.ToString() : "0";

                    int khenThuongCount = GetCount(conn, "SELECT COUNT(*) FROM KhenThuong WHERE MaTV = @maTV");
                    lblAwardCount.Text = khenThuongCount.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetCount(SqlConnection conn, string query)
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@maTV", maTV);
                object result = cmd.ExecuteScalar();
                if (result == DBNull.Value || result == null) return 0;
                if (int.TryParse(result.ToString(), out int count)) return count;
                return 0;
            }
        }

        private object GetScalar(SqlConnection conn, string query)
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@maTV", maTV);
                object result = cmd.ExecuteScalar();
                return result == DBNull.Value ? null : result;
            }
        }

        #endregion

        #region Load Activity Timeline

        private void LoadActivityTimeline()
        {
            try
            {
                flowTimeline.Controls.Clear();
                flowTimeline.AutoScroll = true;

                string query = @"
                    SELECT TOP 10 N'🔔 ' + TB.TieuDe AS Title, TB.NoiDung AS Content,
                        TB.NgayDang AS EventDate, TB.LoaiThongBao AS EventType, 'notification' AS Category
                    FROM ThongBao TB
                    WHERE TB.TrangThai = N'Đã gửi' AND (TB.DoiTuong = N'Tất cả' OR TB.DoiTuong LIKE '%' + CAST(@maTV AS NVARCHAR) + '%')
                    UNION ALL
                    SELECT TOP 10 N'📅 ' + HD.TenHD, ISNULL(HD.MoTa, N'Không có mô tả'),
                        HD.NgayToChuc, N'Hoạt động', 'activity'
                    FROM HoatDong HD
                    INNER JOIN ThamGia TG ON HD.MaHD = TG.MaHD WHERE TG.MaTV = @maTV
                    UNION ALL
                    SELECT TOP 5 N'🎉 Khen thưởng: ' + ISNULL(KT.HinhThuc, N''), KT.LyDo,
                        KT.NgayKT, N'Khen thưởng', 'award'
                    FROM KhenThuong KT WHERE KT.MaTV = @maTV
                    UNION ALL
                    SELECT TOP 5 N'📋 Dự án: ' + DA.TenDuAn, ISNULL(DA.MoTa, N'Không có mô tả'),
                        DA.NgayBatDau, N'Dự án', 'project'
                    FROM DuAn DA
                    INNER JOIN PhanCong PC ON DA.MaDA = PC.MaDA WHERE PC.MaTV = @maTV
                    ORDER BY EventDate DESC";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maTV", maTV);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string title = reader["Title"].ToString();
                        string content = reader["Content"] != DBNull.Value ? reader["Content"].ToString() : "";
                        DateTime eventDate = Convert.ToDateTime(reader["EventDate"]);
                        string eventType = reader["EventType"].ToString();
                        string category = reader["Category"].ToString();
                        AddTimelineCard(title, content, eventDate, eventType, category);
                    }
                }

                if (flowTimeline.Controls.Count == 0)
                    AddEmptyTimelineMessage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải timeline: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddTimelineCard(string title, string content, DateTime eventDate, string eventType, string category)
        {
            Color leftBorderColor = Color.FromArgb(99, 102, 241);
            Color bgColor = Color.White;

            switch (category)
            {
                case "award": leftBorderColor = Color.FromArgb(52, 211, 153); break;
                case "activity": leftBorderColor = Color.FromArgb(251, 191, 36); break;
                case "project": leftBorderColor = Color.FromArgb(244, 63, 94); break;
            }

            Panel cardPanel = new Panel()
            {
                Width = flowTimeline.Width - 35,
                Height = 140,
                BackColor = bgColor,
                Margin = new Padding(5, 5, 5, 10)
            };
            cardPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = CreateRoundedRectPath(0, 0, cardPanel.Width - 1, cardPanel.Height - 1, 12))
                {
                    using (var brush = new SolidBrush(bgColor))
                        e.Graphics.FillPath(brush, path);
                    using (var pen = new Pen(Color.FromArgb(229, 231, 235), 1))
                        e.Graphics.DrawPath(pen, path);
                }
                using (var brush = new SolidBrush(leftBorderColor))
                    e.Graphics.FillRectangle(brush, 0, 12, 5, cardPanel.Height - 24);
            };

            Label lblCardTitle = new Label()
            {
                Text = title,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(20, 15),
                AutoSize = true,
                MaximumSize = new Size(cardPanel.Width - 40, 0),
                BackColor = Color.Transparent
            };
            cardPanel.Controls.Add(lblCardTitle);

            Label lblCardDate = new Label()
            {
                Text = "📅 " + eventDate.ToString("dd/MM/yyyy HH:mm"),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(20, lblCardTitle.Bottom + 5),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            cardPanel.Controls.Add(lblCardDate);

            Label lblCardType = new Label()
            {
                Text = "🏷️ " + eventType,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = leftBorderColor,
                Location = new Point(20, lblCardDate.Bottom + 5),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            cardPanel.Controls.Add(lblCardType);

            string shortContent = content.Length > 100 ? content.Substring(0, 100) + "..." : content;
            Label lblCardContent = new Label()
            {
                Text = shortContent,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(75, 85, 99),
                Location = new Point(20, lblCardType.Bottom + 8),
                MaximumSize = new Size(cardPanel.Width - 40, 40),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            cardPanel.Controls.Add(lblCardContent);

            int totalHeight = lblCardContent.Bottom + 15;
            cardPanel.Height = Math.Max(totalHeight, 100);

            cardPanel.MouseEnter += (s, e) => cardPanel.BackColor = Color.FromArgb(249, 250, 251);
            cardPanel.MouseLeave += (s, e) => cardPanel.BackColor = bgColor;

            flowTimeline.Controls.Add(cardPanel);
        }

        private GraphicsPath CreateRoundedRectPath(int x, int y, int w, int h, int r)
        {
            GraphicsPath path = new GraphicsPath();
            int d = r * 2;
            path.AddArc(x, y, d, d, 180, 90);
            path.AddArc(x + w - d, y, d, d, 270, 90);
            path.AddArc(x + w - d, y + h - d, d, d, 0, 90);
            path.AddArc(x, y + h - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void AddEmptyTimelineMessage()
        {
            Label lblEmpty = new Label()
            {
                Text = "📭 Chưa có hoạt động nào\n\nHãy tham gia các hoạt động của CLB để xem timeline!",
                Font = new Font("Segoe UI", 11, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Width = flowTimeline.Width - 40,
                Height = 150,
                Margin = new Padding(20)
            };
            flowTimeline.Controls.Add(lblEmpty);
        }

        #endregion

        #region Sidebar Animation

        private void btnham_Click(object sender, EventArgs e)
        {
            slidebarTransition.Start();
        }

        private void slidebarTransition_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                slidebar.Width -= 15;
                if (slidebar.Width <= SIDEBAR_MIN)
                {
                    sidebarExpand = false;
                    slidebarTransition.Stop();
                    slidebar.Width = SIDEBAR_MIN;
                    HideButtonText();
                }
            }
            else
            {
                slidebar.Width += 15;
                if (slidebar.Width >= SIDEBAR_MAX)
                {
                    sidebarExpand = true;
                    slidebarTransition.Stop();
                    slidebar.Width = SIDEBAR_MAX;
                    ShowButtonText();
                }
            }
        }

        private void HideButtonText()
        {
            btnMemberDashBoard.Text = "";
            btnLichhop.Text = "";
            btnDangXuat.Text = "";
        }

        private void ShowButtonText()
        {
            btnMemberDashBoard.Text = "     Dashboard";
            btnLichhop.Text = "     Lịch họp";
            btnDangXuat.Text = "     Đăng xuất";
        }

        #endregion

        #region Menu Navigation

        private void RegisterMenuEvents()
        {
            btnMemberDashBoard.Click += (s, e) => {
                HighlightButton(btnMemberDashBoard);
                ShowDashboard();
            };

       ;

            btnLichhop.Click += (s, e) => {
                HighlightButton(btnLichhop);
                ShowMeetingsPage();
            };

            btnDangXuat.Click += BtnDangXuat_Click;

         
        }

        private void HighlightButton(object selectedButton)
        {
            Color menuDefaultFill = Color.Transparent;
            Color menuDefaultFore = Color.Black;
            Color menuSelectedFill = Color.FromArgb(94, 148, 255);
            Color menuSelectedFore = Color.White;

            object[] buttons = { btnMemberDashBoard, btnLichhop };

            foreach (var btn in buttons)
            {
                if (btn is Guna.UI2.WinForms.Guna2Button gunaBtn)
                {
                    try
                    {
                        gunaBtn.FillColor = menuDefaultFill;
                        gunaBtn.ForeColor = menuDefaultFore;
                        gunaBtn.Font = new Font(gunaBtn.Font.FontFamily, gunaBtn.Font.Size, FontStyle.Regular);

                        try { gunaBtn.BorderColor = Color.Transparent; } catch { }
                        try { gunaBtn.CustomBorderColor = Color.Transparent; } catch { }
                        try { gunaBtn.BorderThickness = 0; } catch { }
                        try { if (gunaBtn.ShadowDecoration != null) gunaBtn.ShadowDecoration.Enabled = false; } catch { }

                        try { gunaBtn.HoverState.FillColor = menuDefaultFill; } catch { }
                        try { gunaBtn.HoverState.ForeColor = menuDefaultFore; } catch { }
                        try { gunaBtn.CheckedState.FillColor = menuDefaultFill; } catch { }
                        try { gunaBtn.CheckedState.ForeColor = menuDefaultFore; } catch { }
                    }
                    catch { }
                }
                else if (btn is Button normalBtn)
                {
                    try
                    {
                        normalBtn.BackColor = menuDefaultFill;
                        normalBtn.ForeColor = menuDefaultFore;
                        normalBtn.Font = new Font(normalBtn.Font.FontFamily, normalBtn.Font.Size, FontStyle.Regular);
                        try { normalBtn.FlatAppearance.BorderSize = 0; } catch { }
                    }
                    catch { }
                }
            }

            if (selectedButton is Guna.UI2.WinForms.Guna2Button selectedGunaBtn)
            {
                try
                {
                    selectedGunaBtn.FillColor = menuSelectedFill;
                    selectedGunaBtn.ForeColor = menuSelectedFore;
                    selectedGunaBtn.Font = new Font(selectedGunaBtn.Font.FontFamily, selectedGunaBtn.Font.Size, FontStyle.Bold);

                    try { selectedGunaBtn.BorderColor = Color.FromArgb(60, 100, 200); } catch { }
                    try { selectedGunaBtn.CustomBorderColor = Color.FromArgb(60, 100, 200); } catch { }
                    try { selectedGunaBtn.BorderThickness = 1; } catch { }
                    try { if (selectedGunaBtn.ShadowDecoration != null) selectedGunaBtn.ShadowDecoration.Enabled = true; } catch { }

                    try { selectedGunaBtn.HoverState.FillColor = menuSelectedFill; } catch { }
                    try { selectedGunaBtn.HoverState.ForeColor = menuSelectedFore; } catch { }
                    try { selectedGunaBtn.CheckedState.FillColor = menuSelectedFill; } catch { }
                    try { selectedGunaBtn.CheckedState.ForeColor = menuSelectedFore; } catch { }
                }
                catch { }
            }
            else if (selectedButton is Button selectedNormalBtn)
            {
                try
                {
                    selectedNormalBtn.BackColor = menuSelectedFill;
                    selectedNormalBtn.ForeColor = menuSelectedFore;
                    selectedNormalBtn.Font = new Font(selectedNormalBtn.Font.FontFamily, selectedNormalBtn.Font.Size, FontStyle.Bold);
                    try { selectedNormalBtn.FlatAppearance.BorderSize = 0; } catch { }
                }
                catch { }
            }
        }

        private void ShowDashboard()
        {
            if (pnlLichHop != null)
                pnlLichHop.Visible = false;

            pnlStatsContainer.Visible = true;
            pnlProfileSection.Visible = true;
            pnlTimelineSection.Visible = true;

            LoadStatisticsData();
            LoadMemberProfile();
            LoadActivityTimeline();
        }


        private void ShowMeetingsPage()
        {
            pnlStatsContainer.Visible = false;
            pnlProfileSection.Visible = false;
            pnlTimelineSection.Visible = false;

            ShowLichHopPage();
        }

        private void BtnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                Login loginForm = new Login();
                loginForm.FormClosed += (s, args) => this.Close();
                loginForm.Show();
            }
        }

        #endregion

        #region Lịch Họp - Calendar View

        private void ShowLichHopPage()
        {
            if (pnlLichHop != null)
            {
                this.Controls.Remove(pnlLichHop);
                pnlLichHop.Dispose();
            }

            currentViewMonth = DateTime.Now;
            meetingsByDate = new Dictionary<DateTime, List<MeetingInfo>>();

            pnlLichHop = new Panel()
            {
                Location = new Point(250, 80),
                Size = new Size(1030, 620),
                BackColor = Color.FromArgb(240, 242, 245),
                Visible = true
            };
            this.Controls.Add(pnlLichHop);
            pnlLichHop.BringToFront();

            Label lblTitle = new Label()
            {
                Text = "📅 Lịch Họp CLB",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(20, 10),
                AutoSize = true
            };
            pnlLichHop.Controls.Add(lblTitle);

            Label lblSubtitle = new Label()
            {
                Text = $"Xem lịch họp và hoạt động - {username}",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(20, 45),
                AutoSize = true
            };
            pnlLichHop.Controls.Add(lblSubtitle);

            CreateCalendarControls();
            CreateCalendarView();
            LoadMonthMeetings();
        }

        private void CreateCalendarControls()
        {
            Panel pnlControls = new Panel()
            {
                Location = new Point(20, 80),
                Size = new Size(720, 50),
                BackColor = Color.White
            };
            pnlLichHop.Controls.Add(pnlControls);

            pnlControls.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = CreateRoundedRectPath(0, 0, pnlControls.Width - 1, pnlControls.Height - 1, 8))
                {
                    using (var pen = new Pen(Color.FromArgb(229, 231, 235), 1))
                        e.Graphics.DrawPath(pen, path);
                }
            };

            btnPrevMonth = new Button()
            {
                Text = "◀",
                Location = new Point(15, 10),
                Size = new Size(40, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(99, 102, 241),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnPrevMonth.FlatAppearance.BorderSize = 0;
            btnPrevMonth.Click += (s, e) => {
                currentViewMonth = currentViewMonth.AddMonths(-1);
                LoadMonthMeetings();
            };
            pnlControls.Controls.Add(btnPrevMonth);

            lblCurrentMonth = new Label()
            {
                Text = currentViewMonth.ToString("MMMM yyyy"),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(250, 12),
                Size = new Size(220, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlControls.Controls.Add(lblCurrentMonth);

            btnNextMonth = new Button()
            {
                Text = "▶",
                Location = new Point(480, 10),
                Size = new Size(40, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(99, 102, 241),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnNextMonth.FlatAppearance.BorderSize = 0;
            btnNextMonth.Click += (s, e) => {
                currentViewMonth = currentViewMonth.AddMonths(1);
                LoadMonthMeetings();
            };
            pnlControls.Controls.Add(btnNextMonth);

            btnToday = new Button()
            {
                Text = "📍 Hôm nay",
                Location = new Point(580, 10),
                Size = new Size(120, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(52, 211, 153),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnToday.FlatAppearance.BorderSize = 0;
            btnToday.Click += (s, e) => {
                currentViewMonth = DateTime.Now;
                LoadMonthMeetings();
            };
            pnlControls.Controls.Add(btnToday);
        }

        private void CreateCalendarView()
        {
            pnlCalendarContainer = new Panel()
            {
                Location = new Point(20, 140),
                Size = new Size(720, 470),
                BackColor = Color.White
            };
            pnlLichHop.Controls.Add(pnlCalendarContainer);

            pnlCalendarContainer.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = CreateRoundedRectPath(0, 0, pnlCalendarContainer.Width - 1, pnlCalendarContainer.Height - 1, 8))
                {
                    using (var pen = new Pen(Color.FromArgb(229, 231, 235), 1))
                        e.Graphics.DrawPath(pen, path);
                }
            };
        }

        private void LoadMonthMeetings()
        {
            lblCurrentMonth.Text = currentViewMonth.ToString("MMMM yyyy", new System.Globalization.CultureInfo("vi-VN"));

            meetingsByDate.Clear();

            try
            {
                DateTime firstDay = new DateTime(currentViewMonth.Year, currentViewMonth.Month, 1);
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            HD.MaHD, HD.TenHD, HD.MoTa, HD.NgayToChuc, HD.DiaDiem,
                            HD.TrangThai, TG.DiemDanh, TG.GhiChu
                        FROM HoatDong HD
                        LEFT JOIN ThamGia TG ON HD.MaHD = TG.MaHD AND TG.MaTV = @maTV
                        WHERE MONTH(HD.NgayToChuc) = @month AND YEAR(HD.NgayToChuc) = @year
                        ORDER BY HD.NgayToChuc";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        cmd.Parameters.AddWithValue("@month", currentViewMonth.Month);
                        cmd.Parameters.AddWithValue("@year", currentViewMonth.Year);

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            DateTime ngayToChuc = Convert.ToDateTime(reader["NgayToChuc"]);
                            DateTime dateKey = ngayToChuc.Date;

                            if (!meetingsByDate.ContainsKey(dateKey))
                                meetingsByDate[dateKey] = new List<MeetingInfo>();

                            meetingsByDate[dateKey].Add(new MeetingInfo
                            {
                                MaHD = Convert.ToInt32(reader["MaHD"]),
                                TenHD = reader["TenHD"].ToString(),
                                MoTa = reader["MoTa"] != DBNull.Value ? reader["MoTa"].ToString() : "",
                                NgayToChuc = ngayToChuc,
                                DiaDiem = reader["DiaDiem"] != DBNull.Value ? reader["DiaDiem"].ToString() : "",
                                TrangThai = reader["TrangThai"] != DBNull.Value ? reader["TrangThai"].ToString() : "",
                                DiemDanh = reader["DiemDanh"] != DBNull.Value && Convert.ToBoolean(reader["DiemDanh"]),
                                GhiChu = reader["GhiChu"] != DBNull.Value ? reader["GhiChu"].ToString() : ""
                            });
                        }
                    }
                }

                RenderCalendar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lịch họp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenderCalendar()
        {
            pnlCalendarContainer.Controls.Clear();

            int cellWidth = 100;
            int cellHeight = 65;
            int headerHeight = 35;
            int startX = 10;
            int startY = 10;

            string[] dayNames = { "CN", "T2", "T3", "T4", "T5", "T6", "T7" };
            for (int i = 0; i < 7; i++)
            {
                Label lblDay = new Label()
                {
                    Text = dayNames[i],
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(99, 102, 241),
                    Location = new Point(startX + i * cellWidth, startY),
                    Size = new Size(cellWidth, headerHeight),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(243, 244, 246)
                };
                pnlCalendarContainer.Controls.Add(lblDay);
            }

            DateTime firstDay = new DateTime(currentViewMonth.Year, currentViewMonth.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(currentViewMonth.Year, currentViewMonth.Month);
            int firstDayOfWeek = (int)firstDay.DayOfWeek;

            int currentDay = 1;
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    int cellIndex = row * 7 + col;

                    if (cellIndex >= firstDayOfWeek && currentDay <= daysInMonth)
                    {
                        DateTime cellDate = new DateTime(currentViewMonth.Year, currentViewMonth.Month, currentDay);
                        bool isToday = cellDate.Date == DateTime.Now.Date;
                        bool hasMeetings = meetingsByDate.ContainsKey(cellDate);

                        Panel dayCell = CreateDayCell(currentDay, cellDate, isToday, hasMeetings);
                        dayCell.Location = new Point(startX + col * cellWidth, startY + headerHeight + row * cellHeight);
                        dayCell.Size = new Size(cellWidth, cellHeight);
                        pnlCalendarContainer.Controls.Add(dayCell);

                        currentDay++;
                    }
                    else
                    {
                        Panel emptyCell = new Panel()
                        {
                            Location = new Point(startX + col * cellWidth, startY + headerHeight + row * cellHeight),
                            Size = new Size(cellWidth, cellHeight),
                            BackColor = Color.FromArgb(249, 250, 251)
                        };
                        pnlCalendarContainer.Controls.Add(emptyCell);
                    }
                }

                if (currentDay > daysInMonth) break;
            }
        }

        private Panel CreateDayCell(int day, DateTime date, bool isToday, bool hasMeetings)
        {
            Color bgColor = isToday ? Color.FromArgb(219, 234, 254) : Color.White;
            Color borderColor = isToday ? Color.FromArgb(59, 130, 246) : Color.FromArgb(229, 231, 235);

            Panel cell = new Panel()
            {
                BackColor = bgColor,
                Cursor = hasMeetings ? Cursors.Hand : Cursors.Default
            };

            cell.Paint += (s, e) => {
                using (var pen = new Pen(borderColor, isToday ? 2 : 1))
                    e.Graphics.DrawRectangle(pen, 0, 0, cell.Width - 1, cell.Height - 1);
            };

            Label lblDayNumber = new Label()
            {
                Text = day.ToString(),
                Font = new Font("Segoe UI", 11, isToday ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = isToday ? Color.FromArgb(59, 130, 246) : Color.FromArgb(31, 41, 55),
                Location = new Point(5, 3),
                Size = new Size(30, 20),
                BackColor = Color.Transparent
            };
            cell.Controls.Add(lblDayNumber);

            if (hasMeetings)
            {
                var meetings = meetingsByDate[date];
                int meetingCount = meetings.Count;

                Panel dot = new Panel()
                {
                    Size = new Size(8, 8),
                    Location = new Point(cell.Width - 13, 5),
                    BackColor = Color.FromArgb(239, 68, 68)
                };
                dot.Paint += (s, e) => {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(239, 68, 68)), 0, 0, 8, 8);
                };
                cell.Controls.Add(dot);

                if (meetingCount > 1)
                {
                    Label lblCount = new Label()
                    {
                        Text = meetingCount.ToString(),
                        Font = new Font("Segoe UI", 7, FontStyle.Bold),
                        ForeColor = Color.White,
                        BackColor = Color.FromArgb(239, 68, 68),
                        Location = new Point(cell.Width - 20, 15),
                        Size = new Size(15, 15),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    cell.Controls.Add(lblCount);
                }

                string firstMeeting = meetings[0].TenHD;
                if (firstMeeting.Length > 12)
                    firstMeeting = firstMeeting.Substring(0, 12) + "...";

                Label lblMeeting = new Label()
                {
                    Text = firstMeeting,
                    Font = new Font("Segoe UI", 7),
                    ForeColor = Color.FromArgb(107, 114, 128),
                    Location = new Point(3, 25),
                    Size = new Size(cell.Width - 6, 35),
                    BackColor = Color.Transparent
                };
                cell.Controls.Add(lblMeeting);

                cell.Click += (s, e) => ShowDayMeetings(date);
                cell.MouseEnter += (s, e) => cell.BackColor = Color.FromArgb(243, 244, 246);
                cell.MouseLeave += (s, e) => cell.BackColor = bgColor;
            }

            return cell;
        }

        private void ShowDayMeetings(DateTime date)
        {
            if (!meetingsByDate.ContainsKey(date)) return;

            var meetings = meetingsByDate[date];

            Form detailForm = new Form()
            {
                Text = $"Lịch họp - {date:dd/MM/yyyy}",
                Size = new Size(500, 600),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            FlowLayoutPanel flowPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10)
            };
            detailForm.Controls.Add(flowPanel);

            Label lblTitle = new Label()
            {
                Text = $"📅 Lịch họp ngày {date:dd/MM/yyyy}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 15)
            };
            flowPanel.Controls.Add(lblTitle);

            foreach (var meeting in meetings)
            {
                Panel card = new Panel()
                {
                    Width = flowPanel.Width - 40,
                    Height = 140,
                    BackColor = Color.White,
                    Margin = new Padding(0, 0, 0, 10)
                };

                card.Paint += (s, e) => {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var path = CreateRoundedRectPath(0, 0, card.Width - 1, card.Height - 1, 8))
                    {
                        using (var brush = new SolidBrush(Color.FromArgb(249, 250, 251)))
                            e.Graphics.FillPath(brush, path);
                        using (var pen = new Pen(Color.FromArgb(229, 231, 235), 1))
                            e.Graphics.DrawPath(pen, path);
                    }

                    Color statusColor = meeting.NgayToChuc < DateTime.Now
                        ? Color.FromArgb(156, 163, 175)
                        : Color.FromArgb(52, 211, 153);
                    using (var brush = new SolidBrush(statusColor))
                        e.Graphics.FillRectangle(brush, 0, 8, 4, card.Height - 16);
                };

                Label lblMeetingTitle = new Label()
                {
                    Text = meeting.TenHD,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    ForeColor = Color.FromArgb(31, 41, 55),
                    Location = new Point(15, 10),
                    MaximumSize = new Size(card.Width - 30, 0),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                card.Controls.Add(lblMeetingTitle);

                Label lblTime = new Label()
                {
                    Text = "⏰ " + meeting.NgayToChuc.ToString("HH:mm"),
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(107, 114, 128),
                    Location = new Point(15, lblMeetingTitle.Bottom + 5),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                card.Controls.Add(lblTime);

                Label lblLocation = new Label()
                {
                    Text = "📍 " + meeting.DiaDiem,
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(107, 114, 128),
                    Location = new Point(15, lblTime.Bottom + 5),
                    MaximumSize = new Size(card.Width - 30, 40),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                card.Controls.Add(lblLocation);

                if (meeting.DiemDanh)
                {
                    Label lblAttended = new Label()
                    {
                        Text = "✓ Đã điểm danh",
                        Font = new Font("Segoe UI", 8, FontStyle.Bold),
                        ForeColor = Color.FromArgb(52, 211, 153),
                        Location = new Point(15, card.Height - 25),
                        AutoSize = true,
                        BackColor = Color.Transparent
                    };
                    card.Controls.Add(lblAttended);
                }

                flowPanel.Controls.Add(card);
            }

            detailForm.ShowDialog();
        }


        #endregion

        #region Meeting Statistics

        private string GetTotalMeetings()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT COUNT(DISTINCT HD.MaHD) 
                        FROM HoatDong HD
                        LEFT JOIN ThamGia TG ON HD.MaHD = TG.MaHD AND TG.MaTV = @maTV";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        object result = cmd.ExecuteScalar();
                        return result != null ? result.ToString() : "0";
                    }
                }
            }
            catch { return "0"; }
        }

        private string GetAttendedMeetings()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM ThamGia WHERE MaTV = @maTV AND DiemDanh = 1";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        object result = cmd.ExecuteScalar();
                        return result != null ? result.ToString() : "0";
                    }
                }
            }
            catch { return "0"; }
        }

        private string GetUpcomingMeetings()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT COUNT(DISTINCT HD.MaHD) 
                        FROM HoatDong HD
                        LEFT JOIN ThamGia TG ON HD.MaHD = TG.MaHD AND TG.MaTV = @maTV
                        WHERE HD.NgayToChuc >= CAST(GETDATE() AS DATE)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        object result = cmd.ExecuteScalar();
                        return result != null ? result.ToString() : "0";
                    }
                }
            }
            catch { return "0"; }
        }

        #endregion

        #region Hồ Sơ Thành Viên - Member Profile

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            ShowEditProfilePage();
        }

        #region Chỉnh Sửa Hồ Sơ

        private Panel pnlEditProfile;
        private TextBox txtHoTen, txtEmail, txtSDT, txtLop, txtKhoa;
        private PictureBox picEditAvatar;
        private Button btnSaveProfile, btnCancelEdit, btnChangeAvatar;
        private string selectedAvatarPath = string.Empty;

        private void ShowEditProfilePage()
        {
            try
            {
                // Hide other main sections if available
                try { pnlStatsContainer.Visible = false; } catch { }
                try { pnlProfileSection.Visible = false; } catch { }
                try { pnlTimelineSection.Visible = false; } catch { }
                try { if (pnlLichHop != null) pnlLichHop.Visible = false; } catch { }

                // Remove existing edit panel
                if (pnlEditProfile != null)
                {
                    this.Controls.Remove(pnlEditProfile);
                    pnlEditProfile.Dispose();
                    pnlEditProfile = null;
                }

                pnlEditProfile = new Panel()
                {
                    Location = new Point(250, 80),
                    Size = new Size(1030, 620),
                    BackColor = Color.FromArgb(240, 242, 245),
                    Visible = true,
                    AutoScroll = true
                };
                this.Controls.Add(pnlEditProfile);
                pnlEditProfile.BringToFront();

                Label lblTitle = new Label()
                {
                    Text = "✏️ Chỉnh Sửa Hồ Sơ",
                    Font = new Font("Segoe UI", 20, FontStyle.Bold),
                    ForeColor = Color.FromArgb(31, 41, 55),
                    Location = new Point(20, 10),
                    AutoSize = true
                };
                pnlEditProfile.Controls.Add(lblTitle);

                Panel mainContainer = new Panel()
                {
                    Location = new Point(20, 90),
                    Size = new Size(990, 510),
                    BackColor = Color.White
                };
                pnlEditProfile.Controls.Add(mainContainer);

                mainContainer.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var path = CreateRoundedRectPath(0, 0, mainContainer.Width - 1, mainContainer.Height - 1, 12))
                    {
                        using (var pen = new Pen(Color.FromArgb(229, 231, 235), 1))
                            e.Graphics.DrawPath(pen, path);
                    }
                };

                // Left panel: avatar
                Panel leftPanel = new Panel()
                {
                    Location = new Point(30, 30),
                    Size = new Size(300, 450),
                    BackColor = Color.FromArgb(249, 250, 251)
                };
                mainContainer.Controls.Add(leftPanel);

                Label lblAvatar = new Label()
                {
                    Text = "Ảnh đại diện",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(31, 41, 55),
                    Location = new Point(20, 20),
                    AutoSize = true
                };
                leftPanel.Controls.Add(lblAvatar);

                picEditAvatar = new PictureBox()
                {
                    Location = new Point(75, 60),
                    Size = new Size(150, 150),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BorderStyle = BorderStyle.None,
                    Image = picAvatar?.Image
                };
                leftPanel.Controls.Add(picEditAvatar);

                picEditAvatar.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        path.AddEllipse(0, 0, picEditAvatar.Width - 1, picEditAvatar.Height - 1);
                        picEditAvatar.Region = new Region(path);
                    }
                };

                btnChangeAvatar = new Button()
                {
                    Text = "📷 Thay đổi ảnh",
                    Location = new Point(75, 230),
                    Size = new Size(150, 40),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(99, 102, 241),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnChangeAvatar.FlatAppearance.BorderSize = 0;
                btnChangeAvatar.Click += BtnChangeAvatar_Click;
                leftPanel.Controls.Add(btnChangeAvatar);

                Label lblNote = new Label()
                {
                    Text = "💡 Lưu ý:\n• Ảnh nên có kích thước vuông\n• Định dạng: JPG, PNG\n• Dung lượng tối đa: 5MB",
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(107, 114, 128),
                    Location = new Point(20, 290),
                    Size = new Size(260, 80),
                    BackColor = Color.Transparent
                };
                leftPanel.Controls.Add(lblNote);

                // Right panel: fields
                Panel rightPanel = new Panel()
                {
                    Location = new Point(350, 30),
                    Size = new Size(610, 450),
                    BackColor = Color.Transparent
                };
                mainContainer.Controls.Add(rightPanel);

                int yPos = 0;
                AddFormField(rightPanel, ref yPos, "Họ và tên:", "txtHoTen", out txtHoTen);
                AddFormField(rightPanel, ref yPos, "Email:", "txtEmail", out txtEmail);
                AddFormField(rightPanel, ref yPos, "Số điện thoại:", "txtSDT", out txtSDT);
                AddFormField(rightPanel, ref yPos, "Lớp:", "txtLop", out txtLop);
                AddFormField(rightPanel, ref yPos, "Khoa:", "txtKhoa", out txtKhoa);

                // Load current profile values
                LoadCurrentProfileData();

                yPos += 30;
                btnSaveProfile = new Button()
                {
                    Text = "💾 Lưu thay đổi",
                    Location = new Point(320, yPos),
                    Size = new Size(140, 45),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(52, 211, 153),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnSaveProfile.FlatAppearance.BorderSize = 0;
                btnSaveProfile.Click += BtnSaveProfile_Click;
                rightPanel.Controls.Add(btnSaveProfile);

                btnCancelEdit = new Button()
                {
                    Text = "❌ Hủy",
                    Location = new Point(470, yPos),
                    Size = new Size(120, 45),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(239, 68, 68),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnCancelEdit.FlatAppearance.BorderSize = 0;
                btnCancelEdit.Click += (s, e) =>
                {
                    // Close edit panel and return to dashboard
                    try { if (pnlEditProfile != null) { this.Controls.Remove(pnlEditProfile); pnlEditProfile.Dispose(); pnlEditProfile = null; } } catch { }
                    ShowDashboard();
                    HighlightButton(btnMemberDashBoard);
                };
                rightPanel.Controls.Add(btnCancelEdit);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở giao diện chỉnh sửa hồ sơ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddFormField(Panel parent, ref int yPos, string labelText, string fieldName, out TextBox textBox)
        {
            Label lbl = new Label()
            {
                Text = labelText,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(0, yPos),
                Size = new Size(150, 25)
            };
            parent.Controls.Add(lbl);

            textBox = new TextBox()
            {
                Name = fieldName,
                Location = new Point(160, yPos - 3),
                Size = new Size(430, 30),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };
            parent.Controls.Add(textBox);

            yPos += 60;
        }

        private void MemberDashboard_Load_2(object sender, EventArgs e)
        {

        }

        private void LoadCurrentProfileData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT HoTen, Email, SDT, Lop, Khoa, AnhDaiDien
                        FROM ThanhVien
                        WHERE MaTV = @maTV";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtHoTen.Text = reader["HoTen"] != DBNull.Value ? reader["HoTen"].ToString() : string.Empty;
                                txtEmail.Text = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : string.Empty;
                                txtSDT.Text = reader["SDT"] != DBNull.Value ? reader["SDT"].ToString() : string.Empty;
                                txtLop.Text = reader["Lop"] != DBNull.Value ? reader["Lop"].ToString() : string.Empty;
                                txtKhoa.Text = reader["Khoa"] != DBNull.Value ? reader["Khoa"].ToString() : string.Empty;

                                if (reader["AnhDaiDien"] != DBNull.Value)
                                {
                                    selectedAvatarPath = reader["AnhDaiDien"].ToString();
                                    try { if (System.IO.File.Exists(selectedAvatarPath)) picEditAvatar.Image = Image.FromFile(selectedAvatarPath); } catch { }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu hồ sơ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnChangeAvatar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Chọn ảnh đại diện";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var fi = new System.IO.FileInfo(ofd.FileName);
                        if (fi.Length > 5 * 1024 * 1024)
                        {
                            MessageBox.Show("Ảnh không được vượt quá 5MB!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        picEditAvatar.Image = Image.FromFile(ofd.FileName);
                        selectedAvatarPath = ofd.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi tải ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnSaveProfile_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtSDT.Text) && !IsValidPhoneNumber(txtSDT.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            try
            {
                string finalAvatarPath = selectedAvatarPath;

                if (!string.IsNullOrEmpty(selectedAvatarPath) && System.IO.File.Exists(selectedAvatarPath))
                {
                    string projectFolder = Application.StartupPath + "\\Avatars";
                    if (!System.IO.Directory.Exists(projectFolder)) System.IO.Directory.CreateDirectory(projectFolder);

                    string fileName = $"avatar_{maTV}_{DateTime.Now.Ticks}{System.IO.Path.GetExtension(selectedAvatarPath)}";
                    finalAvatarPath = System.IO.Path.Combine(projectFolder, fileName);
                    System.IO.File.Copy(selectedAvatarPath, finalAvatarPath, true);
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        UPDATE ThanhVien 
                        SET HoTen = @hoTen,
                            Email = @email,
                            SDT = @sdt,
                            Lop = @lop,
                            Khoa = @khoa,
                            AnhDaiDien = @anhDaiDien
                        WHERE MaTV = @maTV";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        cmd.Parameters.AddWithValue("@hoTen", txtHoTen.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(txtEmail.Text) ? (object)DBNull.Value : txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@sdt", string.IsNullOrWhiteSpace(txtSDT.Text) ? (object)DBNull.Value : txtSDT.Text.Trim());
                        cmd.Parameters.AddWithValue("@lop", string.IsNullOrWhiteSpace(txtLop.Text) ? (object)DBNull.Value : txtLop.Text.Trim());
                        cmd.Parameters.AddWithValue("@khoa", string.IsNullOrWhiteSpace(txtKhoa.Text) ? (object)DBNull.Value : txtKhoa.Text.Trim());
                        cmd.Parameters.AddWithValue("@anhDaiDien", string.IsNullOrEmpty(finalAvatarPath) ? (object)DBNull.Value : finalAvatarPath);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("✅ Cập nhật hồ sơ thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // refresh profile
                            LoadMemberProfile();

                            // close edit panel
                            try { if (pnlEditProfile != null) { this.Controls.Remove(pnlEditProfile); pnlEditProfile.Dispose(); pnlEditProfile = null; } } catch { }

                            ShowDashboard();
                            HighlightButton(btnMemberDashBoard);
                        }
                        else
                        {
                            MessageBox.Show("Không có thay đổi nào được lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch { return false; }
        }

        private bool IsValidPhoneNumber(string phone)
        {
            // Kiểm tra số điện thoại Việt Nam (10 chữ số, bắt đầu bằng 0)
            return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^0\d{9}$");
        }

        #endregion

        #endregion

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            chatbot?.Dispose();
            base.OnFormClosing(e);
        }

        private void flowTimeline_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MemberDashboard_Load_1(object sender, EventArgs e)
        {

        }
    }
}