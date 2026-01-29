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
        private bool isDrawingSidebar = false;
        private string role;
        private string username;
        private int maTV;

        // 🔗 Chuỗi kết nối SQL Server
        //private string connectionString = @"Data Source=21AK22-COM;Initial Catalog=QL_CLB_LSC;Persist Security Info=True;User ID=sa;Password=912005;Encrypt=True;TrustServerCertificate=True";
        private string connectionString = ConnectionHelper.ConnectionString;
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

            // maximize and cover screen area
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Bounds = Screen.PrimaryScreen.Bounds;

            this.Load += MemberDashboard_Load;
            this.Resize += MemberDashboard_Resize;
            btnham.Click += btnham_Click;
            slidebarTransition.Tick += slidebarTransition_Tick;
            RegisterMenuEvents();




        }
        // Thêm vào phương thức MemberDashboard_Load


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

                // ✨ TỰ ĐỘNG CLICK VÀO DASHBOARD KHI FORM LOAD
                HighlightButton(btnMemberDashBoard);
                ShowDashboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi khởi tạo form: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Cập nhật phương thức HighlightButton để có border màu đẹp hơn

        private void HighlightButton(object selectedButton)
        {
            Color menuDefaultFill = Color.Transparent;
            Color menuDefaultFore = Color.Black;
            Color menuSelectedFill = Color.FromArgb(94, 148, 255);
            Color menuSelectedFore = Color.White;
            Color menuSelectedBorder = Color.FromArgb(59, 130, 246); // 🎨 Màu border xanh dương đẹp

            object[] buttons = { btnMemberDashBoard, btnLichhop, btnDangXuat };

            foreach (var btn in buttons)
            {
                if (btn is Guna.UI2.WinForms.Guna2Button gunaBtn)
                {
                    try
                    {
                        // Reset về trạng thái mặc định
                        gunaBtn.FillColor = menuDefaultFill;
                        gunaBtn.ForeColor = menuDefaultFore;
                        gunaBtn.Font = new Font(gunaBtn.Font.FontFamily, gunaBtn.Font.Size, FontStyle.Regular);

                        // Tắt border và shadow
                        try { gunaBtn.BorderColor = Color.Transparent; } catch { }
                        try { gunaBtn.CustomBorderColor = Color.Transparent; } catch { }
                        try { gunaBtn.BorderThickness = 0; } catch { }
                        try { if (gunaBtn.ShadowDecoration != null) gunaBtn.ShadowDecoration.Enabled = false; } catch { }

                        // Reset hover state
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

            // Highlight button được chọn với border màu đẹp
            if (selectedButton is Guna.UI2.WinForms.Guna2Button selectedGunaBtn)
            {
                try
                {
                    selectedGunaBtn.FillColor = menuSelectedFill;
                    selectedGunaBtn.ForeColor = menuSelectedFore;
                    selectedGunaBtn.Font = new Font(selectedGunaBtn.Font.FontFamily, selectedGunaBtn.Font.Size, FontStyle.Bold);

                    // ✨ THÊM BORDER MÀU ĐẸP
                    try
                    {
                        selectedGunaBtn.BorderColor = menuSelectedBorder;
                        selectedGunaBtn.BorderThickness = 3; // Border dày 3px
                        selectedGunaBtn.BorderRadius = 8; // Bo góc đẹp
                    }
                    catch { }

                    try
                    {
                        selectedGunaBtn.CustomBorderColor = menuSelectedBorder;
                        selectedGunaBtn.CustomBorderThickness = new Padding(0, 0, 4, 0); // Border bên phải
                    }
                    catch { }

                    // ✨ THÊM SHADOW ĐẸP
                    try
                    {
                        if (selectedGunaBtn.ShadowDecoration != null)
                        {
                            selectedGunaBtn.ShadowDecoration.Enabled = true;
                            selectedGunaBtn.ShadowDecoration.Color = menuSelectedBorder;
                            selectedGunaBtn.ShadowDecoration.Depth = 10;
                            selectedGunaBtn.ShadowDecoration.Shadow = new Padding(0, 0, 5, 5);
                        }
                    }
                    catch { }

                    // Hover state cho button được chọn
                    try { selectedGunaBtn.HoverState.FillColor = Color.FromArgb(80, 130, 230); } catch { }
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

                    // Border cho Button thường
                    try
                    {
                        selectedNormalBtn.FlatAppearance.BorderSize = 3;
                        selectedNormalBtn.FlatAppearance.BorderColor = menuSelectedBorder;
                    }
                    catch { }
                }
                catch { }
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
            // Keep chatbot anchored
            if (chatbot != null)
            {
                chatbot.Location = new Point(
                    this.ClientSize.Width - chatbot.Width - 20,
                    this.ClientSize.Height - chatbot.Height - 20
                );
                chatbot.BringToFront();
            }

            // Update dynamic layout so dashboard & calendar scale with window
            AdjustLayout();
            // Re-render calendar grid so cells scale to new size
            try { if (pnlCalendarContainer != null) RenderCalendar(); } catch { }
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

        // ===== THAY THẾ phương thức AddTimelineCard =====

        private void AddTimelineCard(string title, string content, DateTime eventDate, string eventType, string category)
        {
            Color leftBorderColor = Color.FromArgb(99, 102, 241);
            Color bgColor = Color.White;

            switch (category)
            {
                case "award": leftBorderColor = Color.FromArgb(52, 211, 153); break;
                case "activity": leftBorderColor = Color.FromArgb(251, 191, 36); break;
                case "project": leftBorderColor = Color.FromArgb(244, 63, 94); break;
                case "notification": leftBorderColor = Color.FromArgb(99, 102, 241); break;
            }

            Panel cardPanel = new Panel()
            {
                Width = flowTimeline.Width - 35,
                Height = 140,
                BackColor = bgColor,
                Margin = new Padding(5, 5, 5, 10),
                Cursor = Cursors.Hand, // 👆 Thay đổi con trỏ thành tay
                Tag = new { Title = title, Content = content, Date = eventDate, Type = eventType, Category = category }
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

            // ✨ Thêm nhãn "Xem chi tiết"
            Label lblViewDetail = new Label()
            {
                Text = "👁️ Xem chi tiết →",
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = leftBorderColor,
                Location = new Point(cardPanel.Width - 120, cardPanel.Height - 25),
                AutoSize = true,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            cardPanel.Controls.Add(lblViewDetail);

            int totalHeight = lblCardContent.Bottom + 15;
            cardPanel.Height = Math.Max(totalHeight, 100);

            // ✨ Sự kiện hover
            cardPanel.MouseEnter += (s, e) =>
            {
                cardPanel.BackColor = Color.FromArgb(249, 250, 251);
                lblViewDetail.ForeColor = Color.FromArgb(59, 130, 246);
            };
            cardPanel.MouseLeave += (s, e) =>
            {
                cardPanel.BackColor = bgColor;
                lblViewDetail.ForeColor = leftBorderColor;
            };

            // ✨ Sự kiện click - Hiển thị chi tiết
            cardPanel.Click += (s, e) => ShowActivityDetail(title, content, eventDate, eventType, category);
            lblViewDetail.Click += (s, e) => ShowActivityDetail(title, content, eventDate, eventType, category);

            flowTimeline.Controls.Add(cardPanel);
        }

        // ===== THÊM phương thức MỚI để hiển thị chi tiết =====

        private void ShowActivityDetail(string title, string content, DateTime eventDate, string eventType, string category)
        {
            // Tạo form chi tiết
            Form detailForm = new Form()
            {
                Text = "Chi tiết " + eventType,
                Size = new Size(700, 600),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(240, 242, 245)
            };

            // Panel chính
            Panel mainPanel = new Panel()
            {
                Location = new Point(20, 20),
                Size = new Size(640, 520),
                BackColor = Color.White,
                AutoScroll = true
            };
            detailForm.Controls.Add(mainPanel);

            mainPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = CreateRoundedRectPath(0, 0, mainPanel.Width - 1, mainPanel.Height - 1, 12))
                {
                    using (var pen = new Pen(Color.FromArgb(229, 231, 235), 2))
                        e.Graphics.DrawPath(pen, path);
                }
            };

            // Màu sắc theo loại
            Color accentColor = Color.FromArgb(99, 102, 241);
            string icon = "📋";

            switch (category)
            {
                case "award":
                    accentColor = Color.FromArgb(52, 211, 153);
                    icon = "🎉";
                    break;
                case "activity":
                    accentColor = Color.FromArgb(251, 191, 36);
                    icon = "📅";
                    break;
                case "project":
                    accentColor = Color.FromArgb(244, 63, 94);
                    icon = "📋";
                    break;
                case "notification":
                    accentColor = Color.FromArgb(99, 102, 241);
                    icon = "🔔";
                    break;
            }

            // Header với gradient
            Panel headerPanel = new Panel()
            {
                Location = new Point(0, 0),
                Size = new Size(640, 80),
                BackColor = accentColor
            };
            mainPanel.Controls.Add(headerPanel);

            headerPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new LinearGradientBrush(
                    new Rectangle(0, 0, headerPanel.Width, headerPanel.Height),
                    accentColor,
                    Color.FromArgb(Math.Max(0, accentColor.R - 30),
                                  Math.Max(0, accentColor.G - 30),
                                  Math.Max(0, accentColor.B - 30)),
                    LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, 0, 0, headerPanel.Width, headerPanel.Height);
                }
            };

            // Icon lớn
            Label lblIcon = new Label()
            {
                Text = icon,
                Font = new Font("Segoe UI", 32),
                Location = new Point(20, 15),
                Size = new Size(60, 60),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblIcon);

            // Tiêu đề
            Label lblDetailTitle = new Label()
            {
                Text = title.Length > 60 ? title.Substring(0, 60) + "..." : title,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(90, 15),
                Size = new Size(530, 30),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblDetailTitle);

            // Loại sự kiện
            Label lblDetailType = new Label()
            {
                Text = "🏷️ " + eventType,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                Location = new Point(90, 48),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblDetailType);

            int yPos = 100;

            // Thời gian
            AddDetailRow(mainPanel, ref yPos, "📅 Thời gian:", eventDate.ToString("dddd, dd/MM/yyyy HH:mm"), accentColor);

            // Trạng thái
            string status = eventDate > DateTime.Now ? "⏳ Sắp diễn ra" : "✅ Đã diễn ra";
            AddDetailRow(mainPanel, ref yPos, "📊 Trạng thái:", status, accentColor);

            // Nội dung chi tiết
            yPos += 10;
            Label lblContentTitle = new Label()
            {
                Text = "📝 Nội dung chi tiết:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = accentColor,
                Location = new Point(20, yPos),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            mainPanel.Controls.Add(lblContentTitle);

            yPos += 35;
            Panel contentBox = new Panel()
            {
                Location = new Point(20, yPos),
                Size = new Size(590, 200),
                BackColor = Color.FromArgb(249, 250, 251),
                AutoScroll = true
            };
            mainPanel.Controls.Add(contentBox);

            contentBox.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = CreateRoundedRectPath(0, 0, contentBox.Width - 1, contentBox.Height - 1, 8))
                {
                    using (var pen = new Pen(Color.FromArgb(229, 231, 235), 1))
                        e.Graphics.DrawPath(pen, path);
                }
            };

            Label lblContent = new Label()
            {
                Text = string.IsNullOrEmpty(content) ? "Không có mô tả chi tiết." : content,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(55, 65, 81),
                Location = new Point(15, 15),
                Size = new Size(560, 170),
                BackColor = Color.Transparent
            };
            contentBox.Controls.Add(lblContent);

            yPos += 220;

            // Load thêm thông tin từ database theo category
            LoadAdditionalDetails(mainPanel, ref yPos, category, title, accentColor);

            // Button đóng
            Button btnClose = new Button()
            {
                Text = "✖ Đóng",
                Location = new Point(520, yPos + 20),
                Size = new Size(100, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => detailForm.Close();
            mainPanel.Controls.Add(btnClose);

            detailForm.ShowDialog();
        }

        // ===== Phương thức helper để thêm dòng thông tin =====

        private void AddDetailRow(Panel parent, ref int yPos, string label, string value, Color accentColor)
        {
            Label lblLabel = new Label()
            {
                Text = label,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = accentColor,
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                BackColor = Color.Transparent
            };
            parent.Controls.Add(lblLabel);

            Label lblValue = new Label()
            {
                Text = value,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(55, 65, 81),
                Location = new Point(180, yPos),
                Size = new Size(440, 25),
                BackColor = Color.Transparent
            };
            parent.Controls.Add(lblValue);

            yPos += 35;
        }

        // ===== Load thông tin bổ sung từ database =====

        private void LoadAdditionalDetails(Panel parent, ref int yPos, string category, string title, Color accentColor)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    if (category == "activity")
                    {
                        // Load thông tin hoạt động
                        string query = @"
                    SELECT HD.DiaDiem, HD.SoLuongThamGia, HD.TrangThai, 
                           TG.DiemDanh, TG.GhiChu
                    FROM HoatDong HD
                    LEFT JOIN ThamGia TG ON HD.MaHD = TG.MaHD AND TG.MaTV = @maTV
                    WHERE HD.TenHD = @tenHD";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@maTV", maTV);
                            cmd.Parameters.AddWithValue("@tenHD", title.Replace("📅 ", ""));

                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                AddDetailRow(parent, ref yPos, "📍 Địa điểm:",
                                    reader["DiaDiem"] != DBNull.Value ? reader["DiaDiem"].ToString() : "Chưa cập nhật",
                                    accentColor);

                                AddDetailRow(parent, ref yPos, "👥 Số lượng:",
                                    reader["SoLuongThamGia"] != DBNull.Value ? reader["SoLuongThamGia"].ToString() + " người" : "Chưa cập nhật",
                                    accentColor);

                                if (reader["DiemDanh"] != DBNull.Value && Convert.ToBoolean(reader["DiemDanh"]))
                                {
                                    AddDetailRow(parent, ref yPos, "✅ Điểm danh:", "Đã điểm danh", accentColor);
                                }

                                if (reader["GhiChu"] != DBNull.Value && !string.IsNullOrEmpty(reader["GhiChu"].ToString()))
                                {
                                    AddDetailRow(parent, ref yPos, "📌 Ghi chú:", reader["GhiChu"].ToString(), accentColor);
                                }
                            }
                        }
                    }
                    else if (category == "project")
                    {
                        // Load thông tin dự án
                        string query = @"
                    SELECT DA.TrangThai, DA.NgayKetThuc, PC.VaiTro
                    FROM DuAn DA
                    INNER JOIN PhanCong PC ON DA.MaDA = PC.MaDA
                    WHERE PC.MaTV = @maTV AND DA.TenDuAn = @tenDA";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@maTV", maTV);
                            cmd.Parameters.AddWithValue("@tenDA", title.Replace("📋 Dự án: ", ""));

                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                AddDetailRow(parent, ref yPos, "📊 Trạng thái:",
                                    reader["TrangThai"] != DBNull.Value ? reader["TrangThai"].ToString() : "Chưa cập nhật",
                                    accentColor);

                                if (reader["NgayKetThuc"] != DBNull.Value)
                                {
                                    DateTime ngayKetThuc = Convert.ToDateTime(reader["NgayKetThuc"]);
                                    AddDetailRow(parent, ref yPos, "📅 Ngày kết thúc:", ngayKetThuc.ToString("dd/MM/yyyy"), accentColor);
                                }

                                AddDetailRow(parent, ref yPos, "👤 Vai trò:",
                                    reader["VaiTro"] != DBNull.Value ? reader["VaiTro"].ToString() : "Thành viên",
                                    accentColor);
                            }
                        }
                    }
                    else if (category == "award")
                    {
                        // Load thông tin khen thưởng
                        string query = @"
                    SELECT HinhThuc, GiaiThuong, NguoiKy
                    FROM KhenThuong
                    WHERE MaTV = @maTV AND LyDo = @lyDo";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@maTV", maTV);
                            cmd.Parameters.AddWithValue("@lyDo", title.Replace("🎉 Khen thưởng: ", ""));

                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                AddDetailRow(parent, ref yPos, "🏆 Giải thưởng:",
                                    reader["GiaiThuong"] != DBNull.Value ? reader["GiaiThuong"].ToString() : "Không có",
                                    accentColor);

                                AddDetailRow(parent, ref yPos, "✍️ Người ký:",
                                    reader["NguoiKy"] != DBNull.Value ? reader["NguoiKy"].ToString() : "Chưa cập nhật",
                                    accentColor);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, chỉ hiển thị thông tin cơ bản
                Label lblError = new Label()
                {
                    Text = "⚠️ Không thể load thêm thông tin chi tiết",
                    Font = new Font("Segoe UI", 9, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Location = new Point(20, yPos),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                parent.Controls.Add(lblError);
                yPos += 30;
            }
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
            btnMemberDashBoard.Click += (s, e) =>
            {
                HighlightButton(btnMemberDashBoard);
                ShowDashboard();
            };

            ;

            btnLichhop.Click += (s, e) =>
            {
                HighlightButton(btnLichhop);
                ShowMeetingsPage();
            };

            btnDangXuat.Click += BtnDangXuat_Click;


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

            // ensure controls expand to new size
            AdjustLayout();
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
            // Tạo overlay effect
            using (LogoutEffectOverlay overlay = new LogoutEffectOverlay())
            {
                overlay.ShowOverlay(this);

                // ShowDialog sẽ tự động show form
                DialogResult result = overlay.ShowDialog(this);

                if (result == DialogResult.OK && overlay.LogoutConfirmed)
                {
                    SelectMenuButton(null);
                    this.Hide();

                    Login loginForm = new Login();
                    loginForm.FormClosed += (s, args) => this.Close();
                    loginForm.Show();
                }
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

            // create panel but don't hardcode absolute size; AdjustLayout will size it
            pnlLichHop = new Panel()
            {
                Location = new Point(250, 80),
                Size = new Size(1030, 620),
                BackColor = Color.FromArgb(240, 242, 245),
                Visible = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
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

            // size everything relative to current form size
            AdjustLayout();
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

            pnlControls.Paint += (s, e) =>
            {
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
            btnPrevMonth.Click += (s, e) =>
            {
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
            btnNextMonth.Click += (s, e) =>
            {
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
            btnToday.Click += (s, e) =>
            {
                currentViewMonth = DateTime.Now;
                LoadMonthMeetings();
            };
            pnlControls.Controls.Add(btnToday);
        }

        private void CreateCalendarView()
        {
            // create container; actual size set by AdjustLayout
            pnlCalendarContainer = new Panel()
            {
                Location = new Point(20, 140),
                Size = new Size(720, 470),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            pnlLichHop.Controls.Add(pnlCalendarContainer);

            pnlCalendarContainer.Paint += (s, e) =>
            {
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
            if (pnlCalendarContainer == null) return;
            pnlCalendarContainer.Controls.Clear();

            int headerHeight = 35;
            int startX = 10;
            int startY = 10;

            // compute dynamic cell sizes based on container
            int availableWidth = Math.Max(700, pnlCalendarContainer.ClientSize.Width - startX * 2);
            int availableHeight = Math.Max(300, pnlCalendarContainer.ClientSize.Height - headerHeight - startY * 2);
            int cellWidth = Math.Max(80, availableWidth / 7);
            int cellHeight = Math.Max(60, availableHeight / 6);

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

            cell.Paint += (s, e) =>
            {
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
                dot.Paint += (s, e) =>
                {
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

                card.Paint += (s, e) =>
                {
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
        private TextBox txtUsername, txtPassword, txtConfirmPassword;
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
                    Size = new Size(990, 700),  // ✅ Tăng chiều cao lên 700px
                    BackColor = Color.White,
                    AutoScroll = true  // ✅ Thêm scroll nếu nội dung quá dài
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

                // --- Tên đăng nhập ---
                AddFormField(rightPanel, ref yPos, "Tên đăng nhập:", "txtUsername", out txtUsername);

                // --- Mật khẩu mới ---
                Label lblPwd = new Label()
                {
                    Text = "Mật khẩu mới:",
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    ForeColor = Color.FromArgb(31, 41, 55),
                    Location = new Point(0, yPos),
                    Size = new Size(150, 25)
                };
                rightPanel.Controls.Add(lblPwd);

                txtPassword = new TextBox()
                {
                    Name = "txtPassword",
                    Location = new Point(160, yPos - 3),
                    Size = new Size(430, 30),
                    Font = new Font("Segoe UI", 11),
                    BorderStyle = BorderStyle.FixedSingle,
                    UseSystemPasswordChar = true
                };
                rightPanel.Controls.Add(txtPassword);
                yPos += 60;

                // --- Xác nhận mật khẩu ---
                Label lblConfirm = new Label()
                {
                    Text = "Xác nhận mật khẩu:",
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    ForeColor = Color.FromArgb(31, 41, 55),
                    Location = new Point(0, yPos),
                    Size = new Size(150, 25)
                };
                rightPanel.Controls.Add(lblConfirm);

                txtConfirmPassword = new TextBox()
                {
                    Name = "txtConfirmPassword",
                    Location = new Point(160, yPos - 3),
                    Size = new Size(430, 30),
                    Font = new Font("Segoe UI", 11),
                    BorderStyle = BorderStyle.FixedSingle,
                    UseSystemPasswordChar = true
                };
                rightPanel.Controls.Add(txtConfirmPassword);
                yPos += 60;

                // ✅ CHỈ THÊM MỘT LẦN
                AddFormField(rightPanel, ref yPos, "Lớp:", "txtLop", out txtLop);
                AddFormField(rightPanel, ref yPos, "Khoa:", "txtKhoa", out txtKhoa);

                // Load current profile values
                // Load current profile values
                LoadCurrentProfileData();

                // ✅✅✅ THÊM CODE TẠO NÚT LƯU VÀ HỦY ✅✅✅
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
                    try
                    {
                        if (pnlEditProfile != null)
                        {
                            this.Controls.Remove(pnlEditProfile);
                            pnlEditProfile.Dispose();
                            pnlEditProfile = null;
                        }
                    }
                    catch { }
                    ShowDashboard();
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

            yPos += 40z;
        }

        private void MemberDashboard_Load_2(object sender, EventArgs e)
        {

        }

        private void btnDangXuat_Click_1(object sender, EventArgs e)
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
                SELECT tv.HoTen, tv.Email, tv.SDT, tv.Lop, tv.Khoa, tv.AnhDaiDien,
                       tk.TenDN as TaiKhoan
                FROM ThanhVien tv
                LEFT JOIN TaiKhoan tk ON tv.MaTV = tk.MaTV
                WHERE tv.MaTV = @maTV";

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

                                // ✅ LOAD USERNAME
                                if (txtUsername != null && reader["TaiKhoan"] != DBNull.Value)
                                    txtUsername.Text = reader["TaiKhoan"].ToString();

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

            // Username validation
            if (txtUsername != null && string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            // Password validation
            string passwordHash = null;
            if (txtPassword != null && (!string.IsNullOrWhiteSpace(txtPassword.Text) || !string.IsNullOrWhiteSpace(txtConfirmPassword.Text)))
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Text) || string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ mật khẩu và xác nhận mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                passwordHash = HashPassword(txtPassword.Text);
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
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // ✅ CHECK USERNAME UNIQUENESS
                            if (txtUsername != null && !string.IsNullOrWhiteSpace(txtUsername.Text))
                            {
                                using (SqlCommand chk = new SqlCommand("SELECT COUNT(*) FROM TaiKhoan WHERE TenDN = @tenDN AND MaTV <> @maTV", conn, transaction))
                                {
                                    chk.Parameters.AddWithValue("@tenDN", txtUsername.Text.Trim());
                                    chk.Parameters.AddWithValue("@maTV", maTV);
                                    int count = Convert.ToInt32(chk.ExecuteScalar());
                                    if (count > 0)
                                    {
                                        MessageBox.Show("Tên đăng nhập đã được sử dụng. Vui lòng chọn tên khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        txtUsername.Focus();
                                        return;
                                    }
                                }
                            }

                            // ✅ UPDATE ThanhVien
                            string queryTV = @"
                        UPDATE ThanhVien 
                        SET HoTen = @hoTen,
                            Email = @email,
                            SDT = @sdt,
                            Lop = @lop,
                            Khoa = @khoa,
                            AnhDaiDien = @anhDaiDien
                        WHERE MaTV = @maTV";

                            using (SqlCommand cmd = new SqlCommand(queryTV, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@maTV", maTV);
                                cmd.Parameters.AddWithValue("@hoTen", txtHoTen.Text.Trim());
                                cmd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(txtEmail.Text) ? (object)DBNull.Value : txtEmail.Text.Trim());
                                cmd.Parameters.AddWithValue("@sdt", string.IsNullOrWhiteSpace(txtSDT.Text) ? (object)DBNull.Value : txtSDT.Text.Trim());
                                cmd.Parameters.AddWithValue("@lop", string.IsNullOrWhiteSpace(txtLop.Text) ? (object)DBNull.Value : txtLop.Text.Trim());
                                cmd.Parameters.AddWithValue("@khoa", string.IsNullOrWhiteSpace(txtKhoa.Text) ? (object)DBNull.Value : txtKhoa.Text.Trim());
                                cmd.Parameters.AddWithValue("@anhDaiDien", string.IsNullOrEmpty(finalAvatarPath) ? (object)DBNull.Value : finalAvatarPath);

                                cmd.ExecuteNonQuery();
                            }

                            // ✅ UPDATE TaiKhoan
                            if (txtUsername != null && !string.IsNullOrWhiteSpace(txtUsername.Text))
                            {
                                string queryTK = "UPDATE TaiKhoan SET TenDN = @tenDN";

                                if (!string.IsNullOrEmpty(passwordHash))
                                    queryTK += ", MatKhau = @matKhau";

                                queryTK += " WHERE MaTV = @maTV";

                                using (SqlCommand cmd = new SqlCommand(queryTK, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@maTV", maTV);
                                    cmd.Parameters.AddWithValue("@tenDN", txtUsername.Text.Trim());

                                    if (!string.IsNullOrEmpty(passwordHash))
                                        cmd.Parameters.AddWithValue("@matKhau", passwordHash);

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();

                            MessageBox.Show("✅ Cập nhật hồ sơ thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadMemberProfile();

                            if (pnlEditProfile != null)
                            {
                                this.Controls.Remove(pnlEditProfile);
                                pnlEditProfile.Dispose();
                                pnlEditProfile = null;
                            }

                            ShowDashboard();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
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
        // Hash password using SHA256
        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return null;
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(bytes);
                var sb = new System.Text.StringBuilder();
                foreach (var b in hash) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
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

        // Add this method to your MemberDashboard class
        private void SelectMenuButton(object selectedButton)
        {
            // If you want to visually deselect all menu buttons, you can call HighlightButton with null
            HighlightButton(selectedButton);
        }

        // New helper to adjust dashboard/calendar sizes dynamically
        private void AdjustLayout()
        {
            int left = (slidebar != null) ? slidebar.Width : SIDEBAR_MAX;
            int topOffset = 60; // header/menu area
            int margin = 20;
            int contentWidth = Math.Max(600, this.ClientSize.Width - left - (margin * 2));

            // Stats container: full width area under header
            if (pnlStatsContainer != null)
            {
                pnlStatsContainer.Location = new Point(left + margin, topOffset);
                pnlStatsContainer.Size = new Size(contentWidth, pnlStatsContainer.Height);
            }

            // Compute contentTop so profile sits BELOW the stats container (prevents overlap)
            int contentTop = topOffset + 80;
            if (pnlStatsContainer != null)
                contentTop = pnlStatsContainer.Bottom + 20; // 20px gap under stats

            int contentHeight = Math.Max(300, this.ClientSize.Height - contentTop - 40);

            // Move profile panel to the left under stats
            if (pnlProfileSection != null)
            {
                int profileW = Math.Min(420, Math.Max(300, pnlProfileSection.Width)); // reasonable width
                int profileH = contentHeight;
                int profileX = left + margin; // left-aligned relative to slidebar
                pnlProfileSection.Location = new Point(profileX, contentTop);
                pnlProfileSection.Size = new Size(profileW, profileH);
            }

            // Place timeline to the right of profile panel and let it take remaining width
            if (pnlTimelineSection != null)
            {
                int gap = 20;
                int timelineX = (pnlProfileSection != null) ? pnlProfileSection.Right + gap : left + margin;
                int timelineW = Math.Max(360, left + margin + contentWidth - (timelineX - left));
                pnlTimelineSection.Location = new Point(timelineX, contentTop);
                pnlTimelineSection.Size = new Size(Math.Min(timelineW, this.ClientSize.Width - timelineX - margin), contentHeight);
            }

            // Resize meeting panel and internal calendar container if visible
            if (pnlLichHop != null)
            {
                pnlLichHop.Location = new Point(left, topOffset);
                pnlLichHop.Size = new Size(this.ClientSize.Width - left, this.ClientSize.Height - topOffset);

                if (pnlCalendarContainer != null)
                {
                    pnlCalendarContainer.Location = new Point(20, 140);
                    pnlCalendarContainer.Size = new Size(Math.Max(300, pnlLichHop.ClientSize.Width - 40), Math.Max(240, pnlLichHop.ClientSize.Height - 160));
                }
            }
        }
    }
}
