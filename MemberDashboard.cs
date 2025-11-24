using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class MemberDashboard : Form
    {
        private string role;
        private string username;
        private int maTV;

        // 🔗 Chuỗi kết nối SQL Server
        private string connectionString = @"Data Source=DESKTOP-B7F3HIU;Initial Catalog=QL_APP_LSC;Integrated Security=True;TrustServerCertificate=True";
        // Biến cho animation sidebar
        bool sidebarExpand = true;
        private const int SIDEBAR_MAX = 250;
        private const int SIDEBAR_MIN = 70;

        // ========== CHATBOT PANEL ==========
        private ChatbotPanel chatbot;

        // ========== LỊCH HỌP PANEL ==========
        private Panel pnlLichHop;
        private FlowLayoutPanel flowMeetings;
        private ComboBox cboMeetingFilter;
        private DateTimePicker dtpMeetingDate;

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
            btnHoso.Text = "";
            btnLichhop.Text = "";
            btnDangXuat.Text = "";
        }

        private void ShowButtonText()
        {
            btnMemberDashBoard.Text = "     Dashboard";
            btnHoso.Text = "     Hồ sơ";
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

            btnHoso.Click += (s, e) => {
                HighlightButton(btnHoso);
                ShowProfilePage();
            };

            btnLichhop.Click += (s, e) => {
                HighlightButton(btnLichhop);
                ShowMeetingsPage();
            };

            btnDangXuat.Click += BtnDangXuat_Click;

            if (btnEditProfile != null)
                btnEditProfile.Click += (s, e) => ShowProfilePage();
        }

        // THAY THẾ hàm HighlightButton cũ bằng cái này:

        private void HighlightButton(object selectedButton)
        {
            Color menuDefaultFill = Color.Transparent;
            Color menuDefaultFore = Color.Black;
            Color menuSelectedFill = Color.FromArgb(94, 148, 255);
            Color menuSelectedFore = Color.White;

            // Danh sách các button (dùng object thay vì Button)
            object[] buttons = { btnMemberDashBoard, btnHoso, btnLichhop };

            foreach (var btn in buttons)
            {
                // Kiểm tra nếu là Guna2Button
                if (btn is Guna.UI2.WinForms.Guna2Button gunaBtn)
                {
                    try
                    {
                        gunaBtn.FillColor = menuDefaultFill;
                        gunaBtn.ForeColor = menuDefaultFore;
                        gunaBtn.Font = new Font(gunaBtn.Font.FontFamily, gunaBtn.Font.Size, FontStyle.Regular);

                        // Reset border / shadow
                        try { gunaBtn.BorderColor = Color.Transparent; } catch { }
                        try { gunaBtn.CustomBorderColor = Color.Transparent; } catch { }
                        try { gunaBtn.BorderThickness = 0; } catch { }
                        try { if (gunaBtn.ShadowDecoration != null) gunaBtn.ShadowDecoration.Enabled = false; } catch { }

                        // Reset hover/checked states
                        try { gunaBtn.HoverState.FillColor = menuDefaultFill; } catch { }
                        try { gunaBtn.HoverState.ForeColor = menuDefaultFore; } catch { }
                        try { gunaBtn.CheckedState.FillColor = menuDefaultFill; } catch { }
                        try { gunaBtn.CheckedState.ForeColor = menuDefaultFore; } catch { }
                    }
                    catch { }
                }
                // Kiểm tra nếu là Button thường
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

            // Highlight button được chọn
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
            // Ẩn lịch họp
            if (pnlLichHop != null)
                pnlLichHop.Visible = false;

            // Hiện dashboard
            pnlStatsContainer.Visible = true;
            pnlProfileSection.Visible = true;
            pnlTimelineSection.Visible = true;

            LoadStatisticsData();
            LoadMemberProfile();
            LoadActivityTimeline();
        }

        private void ShowProfilePage()
        {
            MessageBox.Show("Chức năng Hồ sơ đang được phát triển.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowMeetingsPage()
        {
            // Ẩn dashboard
            pnlStatsContainer.Visible = false;
            pnlProfileSection.Visible = false;
            pnlTimelineSection.Visible = false;

            // Hiển thị lịch họp
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

        #region Lịch Họp

        private void ShowLichHopPage()
        {
            if (pnlLichHop != null)
            {
                this.Controls.Remove(pnlLichHop);
                pnlLichHop.Dispose();
            }

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
                Text = "📅 Lịch họp CLB",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(20, 10),
                AutoSize = true
            };
            pnlLichHop.Controls.Add(lblTitle);

            Label lblSubtitle = new Label()
            {
                Text = $"Xem lịch họp và hoạt động của bạn - {username}",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(20, 45),
                AutoSize = true
            };
            pnlLichHop.Controls.Add(lblSubtitle);

            CreateMeetingFilterSection();
            CreateMeetingsListSection();
            CreateMeetingStatsSection();
            LoadMeetingsData();
        }

        private void CreateMeetingFilterSection()
        {
            Panel pnlFilter = new Panel()
            {
                Location = new Point(20, 80),
                Size = new Size(990, 50),
                BackColor = Color.White
            };
            pnlLichHop.Controls.Add(pnlFilter);

            Label lblFilter = new Label()
            {
                Text = "Lọc:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(15, 15),
                AutoSize = true
            };
            pnlFilter.Controls.Add(lblFilter);

            cboMeetingFilter = new ComboBox()
            {
                Location = new Point(60, 12),
                Size = new Size(150, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboMeetingFilter.Items.AddRange(new object[] {
                "Tất cả", "Sắp diễn ra", "Đã diễn ra", "Hôm nay", "Tuần này", "Tháng này"
            });
            cboMeetingFilter.SelectedIndex = 1;
            cboMeetingFilter.SelectedIndexChanged += (s, e) => LoadMeetingsData();
            pnlFilter.Controls.Add(cboMeetingFilter);

            Label lblDate = new Label()
            {
                Text = "Ngày:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(230, 15),
                AutoSize = true
            };
            pnlFilter.Controls.Add(lblDate);

            dtpMeetingDate = new DateTimePicker()
            {
                Location = new Point(285, 12),
                Size = new Size(180, 30),
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 10)
            };
            dtpMeetingDate.ValueChanged += (s, e) => {
                cboMeetingFilter.SelectedIndex = 0;
                LoadMeetingsData();
            };
            pnlFilter.Controls.Add(dtpMeetingDate);

            Button btnRefresh = new Button()
            {
                Text = "🔄 Làm mới",
                Location = new Point(860, 10),
                Size = new Size(110, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(99, 102, 241),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => {
                cboMeetingFilter.SelectedIndex = 1;
                dtpMeetingDate.Value = DateTime.Now;
                LoadMeetingsData();
            };
            pnlFilter.Controls.Add(btnRefresh);
        }

        private void CreateMeetingsListSection()
        {
            Panel pnlMeetingsContainer = new Panel()
            {
                Location = new Point(20, 140),
                Size = new Size(720, 470),
                BackColor = Color.White
            };
            pnlLichHop.Controls.Add(pnlMeetingsContainer);

            Label lblListTitle = new Label()
            {
                Text = "📋 Danh sách lịch họp",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(15, 15),
                AutoSize = true
            };
            pnlMeetingsContainer.Controls.Add(lblListTitle);

            flowMeetings = new FlowLayoutPanel()
            {
                Location = new Point(10, 50),
                Size = new Size(700, 410),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.White
            };
            pnlMeetingsContainer.Controls.Add(flowMeetings);
        }

        private void CreateMeetingStatsSection()
        {
            Panel pnlStats = new Panel()
            {
                Location = new Point(750, 140),
                Size = new Size(260, 470),
                BackColor = Color.White
            };
            pnlLichHop.Controls.Add(pnlStats);

            Label lblStatsTitle = new Label()
            {
                Text = "📊 Thống kê",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(15, 15),
                AutoSize = true
            };
            pnlStats.Controls.Add(lblStatsTitle);

            AddMeetingStatCard(pnlStats, 60, "Tổng lịch họp", GetTotalMeetings(), Color.FromArgb(99, 102, 241));
            AddMeetingStatCard(pnlStats, 150, "Đã tham gia", GetAttendedMeetings(), Color.FromArgb(52, 211, 153));
            AddMeetingStatCard(pnlStats, 240, "Sắp diễn ra", GetUpcomingMeetings(), Color.FromArgb(251, 191, 36));
            AddMeetingStatCard(pnlStats, 330, "Tỷ lệ tham dự", GetAttendanceRate(), Color.FromArgb(244, 63, 94));
        }
        private void AddMeetingStatCard(Panel parent, int yPos, string label, string value, Color color)
        {
            Panel card = new Panel()
            {
                Location = new Point(15, yPos),
                Size = new Size(230, 75),
                BackColor = Color.FromArgb(249, 250, 251)
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
                using (var brush = new SolidBrush(color))
                    e.Graphics.FillRectangle(brush, 0, 8, 4, card.Height - 16);
            };

            Label lblValue = new Label()
            {
                Text = value,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = color,
                Location = new Point(15, 10),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblValue);

            Label lblLabel = new Label()
            {
                Text = label,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(15, 40),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblLabel);

            parent.Controls.Add(card);
        }

        private void LoadMeetingsData()
        {
            if (flowMeetings == null) return;

            flowMeetings.Controls.Clear();

            string query = @"
                SELECT 
                    HD.MaHD, HD.TenHD, HD.MoTa, HD.NgayToChuc, HD.DiaDiem,
                    HD.TrangThai, TG.DiemDanh, TG.GhiChu
                FROM HoatDong HD
                LEFT JOIN ThamGia TG ON HD.MaHD = TG.MaHD AND TG.MaTV = @maTV
                WHERE 1=1";

            // Áp dụng bộ lọc
            string filterCondition = GetMeetingFilterCondition();
            if (!string.IsNullOrEmpty(filterCondition))
                query += " AND " + filterCondition;

            query += " ORDER BY HD.NgayToChuc DESC";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        SqlDataReader reader = cmd.ExecuteReader();

                        int count = 0;
                        while (reader.Read())
                        {
                            int maHD = Convert.ToInt32(reader["MaHD"]);
                            string tenHD = reader["TenHD"].ToString();
                            string moTa = reader["MoTa"] != DBNull.Value ? reader["MoTa"].ToString() : "Không có mô tả";
                            DateTime ngayToChuc = Convert.ToDateTime(reader["NgayToChuc"]);
                            string diaDiem = reader["DiaDiem"] != DBNull.Value ? reader["DiaDiem"].ToString() : "Chưa xác định";
                            string trangThai = reader["TrangThai"] != DBNull.Value ? reader["TrangThai"].ToString() : "Chưa xác định";
                            bool diemDanh = reader["DiemDanh"] != DBNull.Value && Convert.ToBoolean(reader["DiemDanh"]);
                            string ghiChu = reader["GhiChu"] != DBNull.Value ? reader["GhiChu"].ToString() : "";

                            AddMeetingCard(maHD, tenHD, moTa, ngayToChuc, diaDiem, trangThai, diemDanh, ghiChu);
                            count++;
                        }

                        if (count == 0)
                        {
                            AddEmptyMeetingMessage();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lịch họp: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetMeetingFilterCondition()
        {
            if (cboMeetingFilter == null) return "";

            string filter = cboMeetingFilter.SelectedItem?.ToString() ?? "Tất cả";
            DateTime today = DateTime.Now.Date;
            DateTime selectedDate = dtpMeetingDate.Value.Date;

            switch (filter)
            {
                case "Sắp diễn ra":
                    return "HD.NgayToChuc >= CAST(GETDATE() AS DATE)";

                case "Đã diễn ra":
                    return "HD.NgayToChuc < CAST(GETDATE() AS DATE)";

                case "Hôm nay":
                    return "CAST(HD.NgayToChuc AS DATE) = CAST(GETDATE() AS DATE)";

                case "Tuần này":
                    return "DATEPART(WEEK, HD.NgayToChuc) = DATEPART(WEEK, GETDATE()) AND DATEPART(YEAR, HD.NgayToChuc) = DATEPART(YEAR, GETDATE())";

                case "Tháng này":
                    return "DATEPART(MONTH, HD.NgayToChuc) = DATEPART(MONTH, GETDATE()) AND DATEPART(YEAR, HD.NgayToChuc) = DATEPART(YEAR, GETDATE())";

                default:
                    if (cboMeetingFilter.SelectedIndex == 0 && dtpMeetingDate.Value.Date != today)
                        return $"CAST(HD.NgayToChuc AS DATE) = '{selectedDate:yyyy-MM-dd}'";
                    return "";
            }
        }

        private void AddMeetingCard(int maHD, string tenHD, string moTa, DateTime ngayToChuc,
            string diaDiem, string trangThai, bool diemDanh, string ghiChu)
        {
            bool isPast = ngayToChuc < DateTime.Now;
            Color statusColor = isPast ? Color.FromArgb(156, 163, 175) : Color.FromArgb(52, 211, 153);

            Panel card = new Panel()
            {
                Width = flowMeetings.Width - 25,
                Height = 150,
                BackColor = Color.White,
                Margin = new Padding(5)
            };

            card.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = CreateRoundedRectPath(0, 0, card.Width - 1, card.Height - 1, 10))
                {
                    using (var brush = new SolidBrush(Color.White))
                        e.Graphics.FillPath(brush, path);
                    using (var pen = new Pen(Color.FromArgb(229, 231, 235), 2))
                        e.Graphics.DrawPath(pen, path);
                }
                using (var brush = new SolidBrush(statusColor))
                    e.Graphics.FillRectangle(brush, 0, 10, 5, card.Height - 20);
            };

            // Tên hoạt động
            Label lblTitle = new Label()
            {
                Text = tenHD,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(20, 15),
                MaximumSize = new Size(card.Width - 180, 0),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblTitle);

            // Trạng thái
            Label lblStatus = new Label()
            {
                Text = isPast ? "✓ Đã diễn ra" : "⏰ Sắp diễn ra",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = statusColor,
                Location = new Point(card.Width - 150, 15),
                Size = new Size(130, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(5)
            };
            card.Controls.Add(lblStatus);

            // Ngày tổ chức
            Label lblDate = new Label()
            {
                Text = "📅 " + ngayToChuc.ToString("dd/MM/yyyy HH:mm"),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(20, lblTitle.Bottom + 10),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblDate);

            // Địa điểm
            Label lblLocation = new Label()
            {
                Text = "📍 " + diaDiem,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(20, lblDate.Bottom + 5),
                MaximumSize = new Size(card.Width - 40, 40),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblLocation);

            // Mô tả ngắn
            string shortDesc = moTa.Length > 80 ? moTa.Substring(0, 80) + "..." : moTa;
            Label lblDesc = new Label()
            {
                Text = shortDesc,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(20, lblLocation.Bottom + 5),
                MaximumSize = new Size(card.Width - 40, 35),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblDesc);

            // Điểm danh status
            if (diemDanh)
            {
                Label lblAttended = new Label()
                {
                    Text = "✓ Đã điểm danh",
                    Font = new Font("Segoe UI", 8, FontStyle.Bold),
                    ForeColor = Color.FromArgb(52, 211, 153),
                    Location = new Point(20, card.Height - 25),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                card.Controls.Add(lblAttended);
            }

            card.Cursor = Cursors.Hand;
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(249, 250, 251);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;
            card.Click += (s, e) => ShowMeetingDetails(maHD, tenHD, moTa, ngayToChuc, diaDiem, trangThai, diemDanh, ghiChu);

            flowMeetings.Controls.Add(card);
        }

        private void ShowMeetingDetails(int maHD, string tenHD, string moTa, DateTime ngayToChuc,
            string diaDiem, string trangThai, bool diemDanh, string ghiChu)
        {
            string details = $"📋 CHI TIẾT LỊCH HỌP\n\n" +
                           $"Tên hoạt động: {tenHD}\n\n" +
                           $"Thời gian: {ngayToChuc:dd/MM/yyyy HH:mm}\n\n" +
                           $"Địa điểm: {diaDiem}\n\n" +
                           $"Trạng thái: {trangThai}\n\n" +
                           $"Mô tả: {moTa}\n\n" +
                           $"Điểm danh: {(diemDanh ? "Đã điểm danh ✓" : "Chưa điểm danh")}\n\n";

            if (!string.IsNullOrEmpty(ghiChu))
                details += $"Ghi chú: {ghiChu}";

            MessageBox.Show(details, "Thông tin chi tiết", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddEmptyMeetingMessage()
        {
            Label lblEmpty = new Label()
            {
                Text = "📭 Không có lịch họp nào\n\nKhông tìm thấy lịch họp phù hợp với bộ lọc hiện tại.",
                Font = new Font("Segoe UI", 11, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Width = flowMeetings.Width - 40,
                Height = 150,
                Margin = new Padding(20),
                BackColor = Color.Transparent
            };
            flowMeetings.Controls.Add(lblEmpty);
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
            catch
            {
                return "0";
            }
        }

        private string GetAttendedMeetings()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT COUNT(*) 
                        FROM ThamGia 
                        WHERE MaTV = @maTV AND DiemDanh = 1";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        object result = cmd.ExecuteScalar();
                        return result != null ? result.ToString() : "0";
                    }
                }
            }
            catch
            {
                return "0";
            }
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
            catch
            {
                return "0";
            }
        }

        private string GetAttendanceRate()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            COUNT(CASE WHEN TG.DiemDanh = 1 THEN 1 END) * 100.0 / NULLIF(COUNT(*), 0)
                        FROM ThamGia TG
                        INNER JOIN HoatDong HD ON TG.MaHD = HD.MaHD
                        WHERE TG.MaTV = @maTV AND HD.NgayToChuc < GETDATE()";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            double rate = Convert.ToDouble(result);
                            return rate.ToString("0.0") + "%";
                        }
                        return "0%";
                    }
                }
            }
            catch
            {
                return "0%";
            }
        }

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