using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Media;
using System.Linq;

namespace ClubManageApp
{
    public class ChatbotPanel : UserControl
    {
        private Button btnToggle;
        private Label lblBadge;
        private ChatbotForm chatForm;
        private Timer badgeTimer;
        private string connectionString;
        private int maTV;
        private string username;
        private int unreadCount = 0;

        public ChatbotPanel(string connString, int memberID, string user)
        {
            connectionString = connString;
            maTV = memberID;
            username = user;
            InitializeComponents();
            SetupBadgeTimer();
        }

        private void InitializeComponents()
        {
            this.Size = new Size(65, 65);
            this.BackColor = Color.Transparent;

            btnToggle = new Button()
            {
                Size = new Size(55, 55),
                Location = new Point(5, 5),
                Text = "💬",
                Font = new Font("Segoe UI Emoji", 18),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(79, 172, 254),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnToggle.FlatAppearance.BorderSize = 0;

            var path = new GraphicsPath();
            path.AddEllipse(0, 0, btnToggle.Width, btnToggle.Height);
            btnToggle.Region = new Region(path);

            btnToggle.Click += BtnToggle_Click;
            this.Controls.Add(btnToggle);

            lblBadge = new Label()
            {
                Size = new Size(22, 22),
                Location = new Point(40, 0),
                Text = "0",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(220, 53, 69),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };

            var badgePath = new GraphicsPath();
            badgePath.AddEllipse(0, 0, lblBadge.Width, lblBadge.Height);
            lblBadge.Region = new Region(badgePath);

            this.Controls.Add(lblBadge);
            lblBadge.BringToFront();
        }

        private void SetupBadgeTimer()
        {
            badgeTimer = new Timer() { Interval = 3000 };
            badgeTimer.Tick += (s, e) => CheckUnreadMessages();
            badgeTimer.Start();
            CheckUnreadMessages();
        }

        private void CheckUnreadMessages()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        @"SELECT COUNT(*) FROM TinNhan 
                          WHERE MaNguoiNhan = @maTV 
                            AND TrangThai = N'Chưa đọc'
                            AND NgayGui >= DATEADD(HOUR, -24, GETDATE())", conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count != unreadCount)
                        {
                            if (count > unreadCount && unreadCount >= 0)
                            {
                                SystemSounds.Asterisk.Play();
                            }
                            unreadCount = count;
                            UpdateBadge();
                        }
                    }
                }
            }
            catch { }
        }

        private void UpdateBadge()
        {
            if (unreadCount > 0)
            {
                lblBadge.Text = unreadCount > 99 ? "99+" : unreadCount.ToString();
                lblBadge.Visible = true;
            }
            else
            {
                lblBadge.Visible = false;
            }
        }

        private void BtnToggle_Click(object sender, EventArgs e)
        {
            if (chatForm == null || chatForm.IsDisposed)
            {
                chatForm = new ChatbotForm(connectionString, maTV, username);
                chatForm.FormClosed += (s, args) => {
                    btnToggle.Text = "💬";
                    btnToggle.BackColor = Color.FromArgb(79, 172, 254);
                };

                chatForm.VisibleChanged += (s, args) => {
                    try
                    {
                        if (!chatForm.Visible)
                        {
                            btnToggle.Text = "💬";
                            btnToggle.BackColor = Color.FromArgb(79, 172, 254);
                        }
                        else
                        {
                            btnToggle.Text = "✕";
                            btnToggle.BackColor = Color.FromArgb(220, 53, 69);
                        }
                    }
                    catch { }
                };

                chatForm.OnMessagesRead += () => {
                    unreadCount = 0;
                    UpdateBadge();
                };
            }

            if (chatForm.Visible)
            {
                chatForm.Hide();
                btnToggle.Text = "💬";
                btnToggle.BackColor = Color.FromArgb(79, 172, 254);
            }
            else
            {
                Point screenPos = this.PointToScreen(new Point(this.Width, this.Height));
                chatForm.Location = new Point(screenPos.X - chatForm.Width - 5, screenPos.Y - chatForm.Height - 5);
                chatForm.Show();
                chatForm.Activate();
                btnToggle.Text = "✕";
                btnToggle.BackColor = Color.FromArgb(220, 53, 69);
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            badgeTimer?.Stop();
            badgeTimer?.Dispose();
            base.OnHandleDestroyed(e);
        }
    }

    // ============ MEMBER INFO CLASS ============
    public class MemberInfo
    {
        public int MaTV { get; set; }
        public string HoTen { get; set; }
        public string ChucVu { get; set; }
        public int UnreadCount { get; set; }
        public bool IsOnline { get; set; }
    }

    // ============ CHAT MODE ENUM ============
    public enum ChatMode
    {
        Bot,
        Admin,
        Member
    }

    // ============ FORM CHATBOT ============
    public class ChatbotForm : Form
    {
        private FlowLayoutPanel flowMessages;
        private TextBox txtMessage;
        private Button btnSend, btnSwitchMode, btnMemberList;
        private Panel pnlHeader, pnlMemberList;
        private Label lblTitle, lblStatus, lblTyping;
        private ChatMode currentMode = ChatMode.Bot;
        private string connectionString;
        private int maTV;
        private string username;
        private int adminID;
        private string adminName;
        private int? selectedMemberID = null;
        private string selectedMemberName = "";
        private Timer refreshTimer;
        private int lastMessageID = 0;
        private Dictionary<string[], string> faqResponses;
        private HashSet<string> displayedMessageKeys = new HashSet<string>(StringComparer.Ordinal);

        public event Action OnMessagesRead;

        public ChatbotForm(string connString, int memberID, string user)
        {
            connectionString = connString;
            maTV = memberID;
            username = user;

            LoadAdminInfo();
            InitializeFAQ();
            InitializeForm();
            SetupRefreshTimer();

            AddBotMessage($"Xin chào {username}! 👋\n\n💡 Chọn chế độ chat:\n• 🤖 Chat Bot - Trả lời tự động\n• 👨‍💼 Chat Admin - Hỗ trợ trực tiếp\n• 👥 Chat Thành viên - Trò chuyện với bạn bè");
        }

        private void LoadAdminInfo()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT TOP 1 TV.MaTV, TV.HoTen 
                                     FROM ThanhVien TV 
                                     LEFT JOIN TaiKhoan TK ON TV.MaTV = TK.MaTV 
                                     WHERE TV.MaCV = 1
                                        OR TK.QuyenHan IN (N'Admin', N'Quản trị viên')
                                     ORDER BY 
                                        CASE 
                                            WHEN TV.MaCV = 1 THEN 1
                                            WHEN TK.QuyenHan = N'Admin' THEN 2
                                            ELSE 3
                                        END ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            adminID = Convert.ToInt32(reader["MaTV"]);
                            adminName = "Admin CLB";
                        }
                        else
                        {
                            adminID = 1;
                            adminName = "Admin CLB";
                        }
                    }
                }
            }
            catch
            {
                adminID = 1;
                adminName = "Admin CLB";
            }
        }

        private void InitializeFAQ()
        {
            faqResponses = new Dictionary<string[], string>
    {
        { new[] { "xin chào", "hello", "hi", "chào", "chao", "hey", "alo", "hế lô" },
            "👋 Xin chào! Tôi là trợ lý ảo của CLB.\n\n💡 Tôi có thể giúp bạn:\n• Thông tin hoạt động\n• Điểm rèn luyện\n• Kết nối Admin\n• Chat với thành viên\n\nBạn cần gì?" },

        { new[] { "hoạt động", "sự kiện", "event", "lịch", "offline", "workshop", "du lịch", "teambuilding", "sắp tới" },
            "📅 Hoạt động CLB:\n\n🔹 Offline định kỳ: T7, CN\n🔹 Workshop: Hàng tháng\n🔹 Du lịch: Mùa hè, Tết\n🔹 Teambuilding: Quý\n\n💬 Chat Admin để biết chi tiết!" },

        { new[] { "thành viên", "member", "bạn bè", "danh sách", "người", "kết nối", "chat với bạn" },
            "👥 Kết nối thành viên:\n\n• Nhấn '👥 Danh sách' để xem\n• Chat trực tiếp 1-1\n• Xem trạng thái online\n• Tin nhắn chưa đọc\n\n🤝 Giao lưu ngay!" },

        { new[] { "help", "giúp", "hướng dẫn", "trợ giúp", "hỗ trợ", "cách dùng", "sử dụng", "làm sao" },
            "🤖 Hướng dẫn sử dụng:\n\n🔵 Chat Bot: Trả lời tự động\n🟢 Chat Admin: Hỗ trợ trực tiếp\n🟡 Chat Thành viên: Trò chuyện\n\n💡 Chuyển đổi bằng nút phía trên!" },

        { new[] { "cảm ơn", "thanks", "thank you", "cam on", "cám ơn", "cảm ơn nhiều", "okela" },
            "😊 Không có gì!\n\nCần gì thêm cứ hỏi nhé! 💙" }
    };
        }

        private void SetupRefreshTimer()
        {
            refreshTimer = new Timer() { Interval = 2000 };
            refreshTimer.Tick += (s, e) => {
                if (currentMode == ChatMode.Admin || currentMode == ChatMode.Member)
                {
                    LoadNewMessages();
                }
            };
        }

        private void InitializeForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(360, 500);
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = Color.White;
            this.ShowInTaskbar = false;
            this.TopMost = true;

            this.Region = CreateRoundedRegion(this.Width, this.Height, 15);

            this.Paint += (s, e) => {
                using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 1))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(pen, CreateRoundedPath(0, 0, this.Width - 1, this.Height - 1, 15));
                }
            };

            CreateHeader();
            CreateMemberListPanel();
            CreateMessageArea();
            CreateInputArea();
        }

        private Region CreateRoundedRegion(int w, int h, int r)
        {
            GraphicsPath path = CreateRoundedPath(0, 0, w, h, r);
            return new Region(path);
        }

        private GraphicsPath CreateRoundedPath(int x, int y, int w, int h, int r)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, r * 2, r * 2, 180, 90);
            path.AddArc(w - r * 2, y, r * 2, r * 2, 270, 90);
            path.AddArc(w - r * 2, h - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(x, h - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void CreateHeader()
        {
            pnlHeader = new Panel()
            {
                Size = new Size(360, 100),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(79, 172, 254)
            };

            lblTitle = new Label()
            {
                Text = "🤖 Trợ lý CLB",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 10),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitle);

            lblStatus = new Label()
            {
                Text = "Bot tự động",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(220, 220, 220),
                Location = new Point(15, 32),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblStatus);

            // Button Switch Mode
            btnSwitchMode = new Button()
            {
                Text = "👨‍💼 Admin",
                Size = new Size(80, 28),
                Location = new Point(15, 60),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(52, 211, 153),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSwitchMode.FlatAppearance.BorderSize = 0;
            btnSwitchMode.Click += BtnSwitchMode_Click;
            pnlHeader.Controls.Add(btnSwitchMode);

            // Button Member List
            btnMemberList = new Button()
            {
                Text = "👥 Thành viên",
                Size = new Size(110, 28),
                Location = new Point(100, 60),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnMemberList.FlatAppearance.BorderSize = 0;
            btnMemberList.Click += BtnMemberList_Click;
            pnlHeader.Controls.Add(btnMemberList);

            Button btnClose = new Button()
            {
                Text = "✕",
                Size = new Size(30, 30),
                Location = new Point(320, 8),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Hide();
            pnlHeader.Controls.Add(btnClose);

            this.Controls.Add(pnlHeader);
        }

        private void CreateMemberListPanel()
        {
            pnlMemberList = new Panel()
            {
                Location = new Point(0, 100),
                Size = new Size(360, 400),
                BackColor = Color.White,
                Visible = false,
                AutoScroll = true
            };

            this.Controls.Add(pnlMemberList);
            pnlMemberList.BringToFront();
        }

        private void CreateMessageArea()
        {
            flowMessages = new FlowLayoutPanel()
            {
                Location = new Point(5, 105),
                Size = new Size(350, 330),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.FromArgb(245, 245, 245)
            };
            this.Controls.Add(flowMessages);

            lblTyping = new Label()
            {
                Text = "",
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(10, 438),
                AutoSize = true,
                Visible = false
            };
            this.Controls.Add(lblTyping);
        }

        private void CreateInputArea()
        {
            Panel pnlInput = new Panel()
            {
                Location = new Point(0, 450),
                Size = new Size(360, 50),
                BackColor = Color.White
            };

            txtMessage = new TextBox()
            {
                Location = new Point(12, 10),
                Size = new Size(280, 32),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtMessage.KeyPress += (s, e) => {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    SendMessage();
                }
            };
            pnlInput.Controls.Add(txtMessage);

            btnSend = new Button()
            {
                Text = "➤",
                Location = new Point(300, 8),
                Size = new Size(50, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(79, 172, 254),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14),
                Cursor = Cursors.Hand
            };
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Click += (s, e) => SendMessage();
            pnlInput.Controls.Add(btnSend);

            this.Controls.Add(pnlInput);
        }

        private void BtnSwitchMode_Click(object sender, EventArgs e)
        {
            if (currentMode == ChatMode.Bot)
            {
                SwitchToAdminChat();
            }
            else
            {
                SwitchToBotChat();
            }
        }

        private void BtnMemberList_Click(object sender, EventArgs e)
        {
            if (pnlMemberList.Visible)
            {
                pnlMemberList.Visible = false;
                flowMessages.Visible = true;
            }
            else
            {
                LoadMemberList();
                pnlMemberList.Visible = true;
                flowMessages.Visible = false;
            }
        }

        private void LoadMemberList()
        {
            pnlMemberList.Controls.Clear();

            // Search box
            TextBox txtSearch = new TextBox()
            {
                Location = new Point(10, 10),
                Size = new Size(340, 30),
                Font = new Font("Segoe UI", 10),
            };
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Text = "🔍 Tìm kiếm thành viên...";
            txtSearch.GotFocus += (s, e) => {
                if (txtSearch.Text == "🔍 Tìm kiếm thành viên...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };
            txtSearch.LostFocus += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "🔍 Tìm kiếm thành viên...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };
            pnlMemberList.Controls.Add(txtSearch);

            FlowLayoutPanel flowMembers = new FlowLayoutPanel()
            {
                Location = new Point(0, 50),
                Size = new Size(360, 350),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            pnlMemberList.Controls.Add(flowMembers);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT TV.MaTV, TV.HoTen, CV.TenCV, TV.TrangThai,
                                    (SELECT COUNT(*) FROM TinNhan 
                                     WHERE MaNguoiGui = TV.MaTV 
                                       AND MaNguoiNhan = @myID 
                                       AND TrangThai = N'Chưa đọc') as UnreadCount
                                    FROM ThanhVien TV
                                    LEFT JOIN ChucVu CV ON TV.MaCV = CV.MaCV
                                    WHERE TV.MaTV != @myID
                                    ORDER BY 
                                        CASE WHEN TV.TrangThai = N'Đang hoạt động' THEN 0 ELSE 1 END,
                                        UnreadCount DESC, 
                                        TV.HoTen";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@myID", maTV);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            MemberInfo member = new MemberInfo()
                            {
                                MaTV = Convert.ToInt32(reader["MaTV"]),
                                HoTen = reader["HoTen"].ToString(),
                                ChucVu = reader["TenCV"]?.ToString() ?? "Thành viên",
                                UnreadCount = Convert.ToInt32(reader["UnreadCount"]),
                                IsOnline = reader["TrangThai"].ToString() == "Đang hoạt động"
                            };

                            Panel memberItem = CreateMemberItem(member);
                            flowMembers.Controls.Add(memberItem);
                        }
                    }
                }

                txtSearch.TextChanged += (s, e) => {
                    string search = txtSearch.Text.ToLower();
                    foreach (Control ctrl in flowMembers.Controls)
                    {
                        if (ctrl is Panel panel)
                        {
                            string memberName = panel.Tag?.ToString() ?? "";
                            panel.Visible = memberName.ToLower().Contains(search);
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load danh sách: " + ex.Message);
            }
        }

        private Panel CreateMemberItem(MemberInfo member)
        {
            Panel panel = new Panel()
            {
                Size = new Size(350, 60),
                BackColor = Color.White,
                Margin = new Padding(5, 2, 5, 2),
                Cursor = Cursors.Hand,
                Tag = member.HoTen
            };

            panel.Paint += (s, e) => {
                e.Graphics.DrawLine(new Pen(Color.FromArgb(230, 230, 230)),
                    0, panel.Height - 1, panel.Width, panel.Height - 1);
            };

            // Avatar with status indicator
            Panel avatarContainer = new Panel()
            {
                Size = new Size(45, 45),
                Location = new Point(10, 8),
                BackColor = Color.Transparent
            };

            Label lblAvatar = new Label()
            {
                Text = member.HoTen.Substring(0, 1).ToUpper(),
                Size = new Size(45, 45),
                Location = new Point(0, 0),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = member.IsOnline ? Color.FromArgb(79, 172, 254) : Color.FromArgb(150, 150, 150),
                TextAlign = ContentAlignment.MiddleCenter
            };
            var avatarPath = new GraphicsPath();
            avatarPath.AddEllipse(0, 0, lblAvatar.Width, lblAvatar.Height);
            lblAvatar.Region = new Region(avatarPath);
            avatarContainer.Controls.Add(lblAvatar);

            // Online status indicator
            if (member.IsOnline)
            {
                Label lblOnline = new Label()
                {
                    Size = new Size(12, 12),
                    Location = new Point(33, 33),
                    BackColor = Color.FromArgb(52, 211, 153),
                    BorderStyle = BorderStyle.FixedSingle
                };
                var onlinePath = new GraphicsPath();
                onlinePath.AddEllipse(0, 0, lblOnline.Width, lblOnline.Height);
                lblOnline.Region = new Region(onlinePath);
                avatarContainer.Controls.Add(lblOnline);
                lblOnline.BringToFront();
            }

            panel.Controls.Add(avatarContainer);

            // Name
            Label lblName = new Label()
            {
                Text = member.HoTen,
                Location = new Point(65, 10),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50)
            };
            panel.Controls.Add(lblName);

            // Position with status
            Label lblPosition = new Label()
            {
                Text = member.ChucVu + (member.IsOnline ? "" : " (Không hoạt động)"),
                Location = new Point(65, 32),
                Size = new Size(230, 16),
                Font = new Font("Segoe UI", 8),
                ForeColor = member.IsOnline ? Color.Gray : Color.FromArgb(180, 180, 180)
            };
            panel.Controls.Add(lblPosition);

            // Unread badge
            if (member.UnreadCount > 0)
            {
                Label lblUnread = new Label()
                {
                    Text = member.UnreadCount > 9 ? "9+" : member.UnreadCount.ToString(),
                    Size = new Size(25, 25),
                    Location = new Point(310, 18),
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(220, 53, 69),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                var badgePath = new GraphicsPath();
                badgePath.AddEllipse(0, 0, lblUnread.Width, lblUnread.Height);
                lblUnread.Region = new Region(badgePath);
                panel.Controls.Add(lblUnread);
            }

            panel.Click += (s, e) => OpenMemberChat(member);
            foreach (Control ctrl in panel.Controls)
            {
                ctrl.Click += (s, e) => OpenMemberChat(member);
            }

            panel.MouseEnter += (s, e) => panel.BackColor = Color.FromArgb(245, 245, 245);
            panel.MouseLeave += (s, e) => panel.BackColor = Color.White;

            return panel;
        }

        private void OpenMemberChat(MemberInfo member)
        {
            selectedMemberID = member.MaTV;
            selectedMemberName = member.HoTen;
            currentMode = ChatMode.Member;

            pnlMemberList.Visible = false;
            flowMessages.Visible = true;
            flowMessages.Controls.Clear();
            displayedMessageKeys.Clear();

            lblTitle.Text = $"👤 {member.HoTen}";
            lblStatus.Text = "● Trực tuyến";
            lblStatus.ForeColor = Color.FromArgb(52, 211, 153);
            btnSwitchMode.Text = "🤖 Bot";
            btnSwitchMode.BackColor = Color.FromArgb(79, 172, 254);
            pnlHeader.BackColor = Color.FromArgb(156, 39, 176);

            int currentMaxID = GetCurrentMaxMessageID(member.MaTV);
            AddSystemMessage($"💬 Chat với {member.HoTen}");
            LoadRecentMessages(member.MaTV);
            lastMessageID = currentMaxID;
            refreshTimer.Start();

            txtMessage.Focus();
        }

        private void SwitchToAdminChat()
        {
            currentMode = ChatMode.Admin;
            selectedMemberID = null;
            flowMessages.Controls.Clear();
            displayedMessageKeys.Clear();

            lblTitle.Text = $"👨‍💼 {adminName}";
            lblStatus.Text = "● Trực tuyến";
            lblStatus.ForeColor = Color.FromArgb(52, 211, 153);
            btnSwitchMode.Text = "🤖 Bot";
            btnSwitchMode.BackColor = Color.FromArgb(79, 172, 254);
            pnlHeader.BackColor = Color.FromArgb(52, 73, 94);

            int currentMaxID = GetCurrentMaxMessageID(null);
            AddSystemMessage($"💬 Chat với {adminName}");
            LoadRecentMessagesAdmin();
            lastMessageID = currentMaxID;
            refreshTimer.Start();

            txtMessage.Focus();
        }

        private void SwitchToBotChat()
        {
            currentMode = ChatMode.Bot;
            selectedMemberID = null;
            flowMessages.Controls.Clear();

            lblTitle.Text = "🤖 Trợ lý CLB";
            lblStatus.Text = "Bot tự động";
            lblStatus.ForeColor = Color.FromArgb(220, 220, 220);
            btnSwitchMode.Text = "👨‍💼 Admin";
            btnSwitchMode.BackColor = Color.FromArgb(52, 211, 153);
            pnlHeader.BackColor = Color.FromArgb(79, 172, 254);

            refreshTimer.Stop();
            AddBotMessage("Chào mừng quay lại! 👋\n\nTôi có thể giúp gì?");

            txtMessage.Focus();
        }

        private int GetCurrentMaxMessageID(int? targetMemberID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query;

                    if (targetMemberID.HasValue)
                    {
                        query = @"SELECT ISNULL(MAX(MaTN), 0) FROM TinNhan 
                                 WHERE (MaNguoiGui = @maTV AND MaNguoiNhan = @target)
                                    OR (MaNguoiGui = @target AND MaNguoiNhan = @maTV)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@maTV", maTV);
                            cmd.Parameters.AddWithValue("@target", targetMemberID.Value);
                            return Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }
                    else
                    {
                        query = @"SELECT ISNULL(MAX(MaTN), 0) FROM TinNhan 
                                 WHERE MaNguoiNhan = @maTV OR MaNguoiGui = @maTV";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@maTV", maTV);
                            return Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        private void LoadRecentMessages(int targetMemberID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        @"SELECT TOP 20 MaTN, MaNguoiGui, NoiDung, NgayGui, TrangThai
                          FROM TinNhan
                          WHERE (MaNguoiGui = @maTV AND MaNguoiNhan = @target)
                             OR (MaNguoiGui = @target AND MaNguoiNhan = @maTV)
                          ORDER BY NgayGui DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        cmd.Parameters.AddWithValue("@target", targetMemberID);
                        SqlDataReader reader = cmd.ExecuteReader();

                        List<MessageData> messages = new List<MessageData>();
                        while (reader.Read())
                        {
                            messages.Add(new MessageData
                            {
                                MaTN = Convert.ToInt32(reader["MaTN"]),
                                MaNguoiGui = Convert.ToInt32(reader["MaNguoiGui"]),
                                NoiDung = reader["NoiDung"].ToString(),
                                NgayGui = Convert.ToDateTime(reader["NgayGui"]),
                                TrangThai = reader["TrangThai"].ToString()
                            });
                        }
                        reader.Close();

                        messages.Reverse();

                        foreach (var msg in messages)
                        {
                            string key = $"{msg.MaNguoiGui}|{msg.NoiDung?.Trim()}|{msg.NgayGui:s}";
                            if (displayedMessageKeys.Contains(key)) continue;
                            displayedMessageKeys.Add(key);

                            bool isMe = (msg.MaNguoiGui == maTV);
                            AddChatBubble(msg.NoiDung, isMe, msg.NgayGui, isMe ? msg.TrangThai : null);
                        }

                        if (messages.Count > 0)
                        {
                            AddSystemMessage($"📜 Đã load {messages.Count} tin nhắn");
                        }
                    }

                    MarkMessagesAsRead(conn, targetMemberID);
                    OnMessagesRead?.Invoke();
                }
            }
            catch (Exception ex)
            {
                AddSystemMessage("⚠️ Lỗi: " + ex.Message);
            }
        }

        private void LoadRecentMessagesAdmin()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        @"SELECT TOP 20 MaTN, MaNguoiGui, NoiDung, NgayGui, TrangThai
                          FROM TinNhan
                          WHERE (MaNguoiGui = @maTV AND MaNguoiNhan IN (
                                SELECT TV.MaTV FROM ThanhVien TV 
                                LEFT JOIN TaiKhoan TK ON TV.MaTV = TK.MaTV
                                WHERE TV.MaCV = 1 OR TK.QuyenHan IN (N'Admin', N'Quản trị viên')
                          ))
                          OR (MaNguoiNhan = @maTV AND MaNguoiGui IN (
                                SELECT TV.MaTV FROM ThanhVien TV 
                                LEFT JOIN TaiKhoan TK ON TV.MaTV = TK.MaTV
                                WHERE TV.MaCV = 1 OR TK.QuyenHan IN (N'Admin', N'Quản trị viên')
                          ))
                          ORDER BY NgayGui DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        SqlDataReader reader = cmd.ExecuteReader();

                        List<MessageData> messages = new List<MessageData>();
                        while (reader.Read())
                        {
                            messages.Add(new MessageData
                            {
                                MaTN = Convert.ToInt32(reader["MaTN"]),
                                MaNguoiGui = Convert.ToInt32(reader["MaNguoiGui"]),
                                NoiDung = reader["NoiDung"].ToString(),
                                NgayGui = Convert.ToDateTime(reader["NgayGui"]),
                                TrangThai = reader["TrangThai"].ToString()
                            });
                        }
                        reader.Close();

                        messages.Reverse();

                        HashSet<string> seen = new HashSet<string>(StringComparer.Ordinal);
                        foreach (var msg in messages)
                        {
                            string key = $"{msg.MaNguoiGui}|{msg.NoiDung?.Trim()}|{msg.NgayGui:s}";
                            if (seen.Contains(key)) continue;
                            seen.Add(key);

                            if (displayedMessageKeys.Contains(key)) continue;
                            displayedMessageKeys.Add(key);

                            bool isMe = (msg.MaNguoiGui == maTV);
                            AddChatBubble(msg.NoiDung, isMe, msg.NgayGui, isMe ? msg.TrangThai : null);
                        }

                        if (messages.Count > 0)
                        {
                            AddSystemMessage($"📜 Đã load {messages.Count} tin nhắn");
                        }
                    }

                    MarkMessagesAsReadAdmin(conn);
                    OnMessagesRead?.Invoke();
                }
            }
            catch (Exception ex)
            {
                AddSystemMessage("⚠️ Lỗi: " + ex.Message);
            }
        }

        private void MarkMessagesAsRead(SqlConnection conn, int targetMemberID)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    @"UPDATE TinNhan SET TrangThai = N'Đã đọc' 
                      WHERE MaNguoiGui = @target 
                        AND MaNguoiNhan = @maTV 
                        AND TrangThai = N'Chưa đọc'", conn))
                {
                    cmd.Parameters.AddWithValue("@maTV", maTV);
                    cmd.Parameters.AddWithValue("@target", targetMemberID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }

        private void MarkMessagesAsReadAdmin(SqlConnection conn)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    @"UPDATE TN SET TN.TrangThai = N'Đã đọc' 
                      FROM TinNhan TN
                      INNER JOIN ThanhVien TV ON TN.MaNguoiGui = TV.MaTV
                      LEFT JOIN TaiKhoan TK ON TN.MaNguoiGui = TK.MaTV
                      WHERE TN.MaNguoiNhan = @maTV 
                        AND (TV.MaCV = 1 OR TK.QuyenHan IN (N'Admin', N'Quản trị viên'))
                        AND TN.TrangThai = N'Chưa đọc'", conn))
                {
                    cmd.Parameters.AddWithValue("@maTV", maTV);
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }

        private void LoadNewMessages()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query;
                    if (currentMode == ChatMode.Member && selectedMemberID.HasValue)
                    {
                        query = lastMessageID == 0
                            ? @"SELECT MaTN, NoiDung, NgayGui, MaNguoiGui 
                                FROM TinNhan
                                WHERE ((MaNguoiGui = @target AND MaNguoiNhan = @maTV)
                                   OR (MaNguoiGui = @maTV AND MaNguoiNhan = @target))
                                  AND NgayGui >= DATEADD(SECOND, -5, GETDATE())
                                ORDER BY NgayGui ASC"
                            : @"SELECT MaTN, NoiDung, NgayGui, MaNguoiGui 
                                FROM TinNhan
                                WHERE MaTN > @lastID 
                                  AND ((MaNguoiGui = @target AND MaNguoiNhan = @maTV)
                                   OR (MaNguoiGui = @maTV AND MaNguoiNhan = @target))
                                ORDER BY NgayGui ASC";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            if (lastMessageID > 0)
                                cmd.Parameters.AddWithValue("@lastID", lastMessageID);
                            cmd.Parameters.AddWithValue("@maTV", maTV);
                            cmd.Parameters.AddWithValue("@target", selectedMemberID.Value);

                            SqlDataReader reader = cmd.ExecuteReader();
                            bool hasNew = false;

                            while (reader.Read())
                            {
                                hasNew = true;
                                int maNguoiGui = Convert.ToInt32(reader["MaNguoiGui"]);
                                DateTime ngay = Convert.ToDateTime(reader["NgayGui"]);
                                string content = reader["NoiDung"].ToString();

                                string key = $"{maNguoiGui}|{content?.Trim()}|{ngay:s}";
                                if (displayedMessageKeys.Contains(key)) continue;

                                lastMessageID = Convert.ToInt32(reader["MaTN"]);
                                displayedMessageKeys.Add(key);
                                AddChatBubble(content, maNguoiGui == maTV, ngay, null);
                                if (maNguoiGui != maTV)
                                    SystemSounds.Asterisk.Play();
                            }
                            reader.Close();

                            if (hasNew)
                            {
                                MarkMessagesAsRead(conn, selectedMemberID.Value);
                                OnMessagesRead?.Invoke();
                            }
                        }
                    }
                    else if (currentMode == ChatMode.Admin)
                    {
                        query = lastMessageID == 0
                            ? @"SELECT TN.MaTN, TN.NoiDung, TN.NgayGui, TN.MaNguoiGui 
                                FROM TinNhan TN
                                INNER JOIN ThanhVien TV ON TN.MaNguoiGui = TV.MaTV
                                LEFT JOIN TaiKhoan TK ON TN.MaNguoiGui = TK.MaTV
                                WHERE TN.MaNguoiNhan = @maTV
                                  AND (TV.MaCV = 1 OR TK.QuyenHan IN (N'Admin', N'Quản trị viên'))
                                  AND TN.NgayGui >= DATEADD(SECOND, -5, GETDATE())
                                ORDER BY TN.NgayGui ASC"
                            : @"SELECT TN.MaTN, TN.NoiDung, TN.NgayGui, TN.MaNguoiGui 
                                FROM TinNhan TN
                                INNER JOIN ThanhVien TV ON TN.MaNguoiGui = TV.MaTV
                                LEFT JOIN TaiKhoan TK ON TN.MaNguoiGui = TK.MaTV
                                WHERE TN.MaTN > @lastID 
                                  AND TN.MaNguoiNhan = @maTV
                                  AND (TV.MaCV = 1 OR TK.QuyenHan IN (N'Admin', N'Quản trị viên'))
                                ORDER BY TN.NgayGui ASC";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            if (lastMessageID > 0)
                                cmd.Parameters.AddWithValue("@lastID", lastMessageID);
                            cmd.Parameters.AddWithValue("@maTV", maTV);

                            SqlDataReader reader = cmd.ExecuteReader();
                            bool hasNew = false;

                            while (reader.Read())
                            {
                                hasNew = true;
                                int maNguoiGui = Convert.ToInt32(reader["MaNguoiGui"]);
                                DateTime ngay = Convert.ToDateTime(reader["NgayGui"]);
                                string content = reader["NoiDung"].ToString();

                                string key = $"{maNguoiGui}|{content?.Trim()}|{ngay:s}";
                                if (displayedMessageKeys.Contains(key)) continue;

                                lastMessageID = Convert.ToInt32(reader["MaTN"]);
                                displayedMessageKeys.Add(key);
                                AddChatBubble(content, false, ngay, null);
                                SystemSounds.Asterisk.Play();
                            }
                            reader.Close();

                            if (hasNew)
                            {
                                MarkMessagesAsReadAdmin(conn);
                                OnMessagesRead?.Invoke();
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void SendMessage()
        {
            string message = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;
            txtMessage.Clear();

            if (currentMode == ChatMode.Admin)
            {
                SendAdminMessage(message);
            }
            else if (currentMode == ChatMode.Member && selectedMemberID.HasValue)
            {
                SendMemberMessage(message, selectedMemberID.Value);
            }
            else
            {
                AddUserMessage(message);
                string response = GetBotResponse(message);

                lblTyping.Text = "Bot đang nhập...";
                lblTyping.Visible = true;

                Timer delay = new Timer() { Interval = 800 };
                delay.Tick += (s, e) => {
                    delay.Stop();
                    delay.Dispose();
                    lblTyping.Visible = false;
                    AddBotMessage(response);
                };
                delay.Start();
            }

            txtMessage.Focus();
        }

        private void SendMemberMessage(string message, int targetMemberID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        @"INSERT INTO TinNhan (MaNguoiGui, MaNguoiNhan, NoiDung, NgayGui, TrangThai)
                          VALUES (@nguoiGui, @nguoiNhan, @noiDung, GETDATE(), N'Chưa đọc');
                          SELECT SCOPE_IDENTITY();", conn))
                    {
                        cmd.Parameters.AddWithValue("@nguoiGui", maTV);
                        cmd.Parameters.AddWithValue("@nguoiNhan", targetMemberID);
                        cmd.Parameters.AddWithValue("@noiDung", message);
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            lastMessageID = Convert.ToInt32(result);
                        }

                        AddChatBubble(message, true, DateTime.Now, "Đã gửi");
                        string key = $"{maTV}|{message?.Trim()}|{DateTime.Now:s}";
                        displayedMessageKeys.Add(key);
                    }
                }
            }
            catch (Exception ex)
            {
                AddSystemMessage("❌ Lỗi: " + ex.Message);
            }
        }

        private void SendAdminMessage(string message)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        @"INSERT INTO TinNhan (MaNguoiGui, MaNguoiNhan, NoiDung, NgayGui, TrangThai)
                          SELECT @nguoiGui, TV.MaTV, @noiDung, GETDATE(), N'Chưa đọc'
                          FROM ThanhVien TV
                          LEFT JOIN TaiKhoan TK ON TV.MaTV = TK.MaTV
                          WHERE (TV.MaCV = 1 OR TK.QuyenHan IN (N'Admin', N'Quản trị viên'))
                            AND TV.MaTV != @nguoiGui;
                          SELECT SCOPE_IDENTITY();", conn))
                    {
                        cmd.Parameters.AddWithValue("@nguoiGui", maTV);
                        cmd.Parameters.AddWithValue("@noiDung", message);
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            lastMessageID = Convert.ToInt32(result);
                        }

                        AddChatBubble(message, true, DateTime.Now, "Đã gửi");
                        string key = $"{maTV}|{message?.Trim()}|{DateTime.Now:s}";
                        displayedMessageKeys.Add(key);
                    }
                }
            }
            catch (Exception ex)
            {
                AddSystemMessage("❌ Lỗi: " + ex.Message);
            }
        }

        private string GetBotResponse(string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
                return "🤔 Bạn chưa nhập gì cả! Hãy hỏi tôi nhé 💬";

            string normalized = NormalizeText(userMessage);

            // Tìm phản hồi FAQ có số lượng từ khóa khớp nhiều nhất
            string faqResponse = FindBestFAQMatch(normalized);
            if (faqResponse != null)
                return faqResponse;

            // Các câu hỏi đặc biệt (có thể mở rộng thêm)
            if (normalized.Contains("hoạt động sắp tới") ||
                normalized.Contains("lịch hoạt động") ||
                normalized.Contains("sự kiện sắp"))
            {
                return GetUpcomingActivities();
            }

            if (normalized.Contains("điểm của tôi") ||
                normalized.Contains("điểm rèn luyện") ||
                normalized.Contains("điểm hiện tại") ||
                normalized.Contains("xếp loại"))
            {
                return GetMemberPoints();
            }

            // Không hiểu → gợi ý chuyển chế độ
            return "🤔 Mình chưa hiểu rõ câu hỏi của bạn.\n\n" +
                   "💡 Bạn có thể:\n" +
                   "• Hỏi lại chi tiết hơn\n" +
                   "• Chuyển sang 👨‍💼 Chat Admin để được hỗ trợ trực tiếp\n" +
                   "• Hoặc 👥 Chat Thành viên để giao lưu!";
        }

        // Chuẩn hóa văn bản: lowercase, loại bỏ dấu câu thừa
        private string NormalizeText(string text)
        {
            return text.ToLowerInvariant()
                       .Replace("?", "")
                       .Replace("!", "")
                       .Replace(".", "")
                       .Replace(",", "")
                       .Replace("  ", " ")
                       .Trim();
        }

        // Tìm phản hồi FAQ tốt nhất dựa trên số lượng từ khóa khớp
        private string FindBestFAQMatch(string normalizedInput)
        {
            string bestResponse = null;
            int bestMatchCount = 0;

            foreach (var entry in faqResponses)
            {
                int matchCount = 0;
                foreach (string keyword in entry.Key)
                {
                    string normKeyword = NormalizeText(keyword);
                    if (normalizedInput.Contains(normKeyword))
                        matchCount++;
                }

                // Chỉ chấp nhận nếu có ít nhất 1 từ khóa khớp
                // Và ưu tiên cái có nhiều từ khóa khớp hơn
                if (matchCount > bestMatchCount)
                {
                    bestMatchCount = matchCount;
                    bestResponse = entry.Value;
                }
            }

            return bestMatchCount > 0 ? bestResponse : null;
        }

        private string GetUpcomingActivities()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        @"SELECT TOP 3 TenHD, NgayToChuc, DiaDiem FROM HoatDong 
                          WHERE NgayToChuc >= CAST(GETDATE() AS DATE) ORDER BY NgayToChuc", conn))
                    {
                        SqlDataReader r = cmd.ExecuteReader();
                        string result = "📅 Hoạt động sắp tới:\n";
                        int c = 0;
                        while (r.Read())
                        {
                            c++;
                            result += $"\n🔹 {r["TenHD"]}\n   📆 {Convert.ToDateTime(r["NgayToChuc"]):dd/MM/yyyy}\n   📍 {r["DiaDiem"]}\n";
                        }
                        return c == 0 ? "📭 Chưa có hoạt động sắp tới." : result;
                    }
                }
            }
            catch { return "❌ Lỗi tải thông tin."; }
        }

        private string GetMemberPoints()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        @"SELECT TOP 1 Diem, XepLoai, HocKy, NamHoc FROM DiemRenLuyen 
                          WHERE MaTV = @maTV ORDER BY NgayCapNhat DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        SqlDataReader r = cmd.ExecuteReader();
                        if (r.Read())
                            return $"🏆 Điểm: {r["Diem"]}/100\n⭐ Xếp loại: {r["XepLoai"]}\n📅 {r["HocKy"]} - {r["NamHoc"]}";
                        return "📊 Chưa có điểm rèn luyện.";
                    }
                }
            }
            catch { return "❌ Lỗi tải thông tin."; }
        }

        private void AddUserMessage(string msg) => AddChatBubble(msg, true, DateTime.Now, null);

        private void AddBotMessage(string msg)
        {
            Panel bubble = new Panel()
            {
                AutoSize = true,
                MaximumSize = new Size(260, 0),
                MinimumSize = new Size(60, 40),
                Padding = new Padding(12, 10, 12, 10),
                Margin = new Padding(5, 5, 80, 3),
                BackColor = Color.White
            };

            bubble.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(220, 220, 220), 1))
                {
                    e.Graphics.DrawRoundedRectangle(pen, 0, 0, bubble.Width - 1, bubble.Height - 1, 12);
                }
            };

            Label lbl = new Label()
            {
                Text = msg,
                AutoSize = true,
                MaximumSize = new Size(230, 0),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(50, 50, 50),
                Location = new Point(12, 10)
            };
            bubble.Controls.Add(lbl);

            Label lblTime = new Label()
            {
                Text = DateTime.Now.ToString("HH:mm"),
                AutoSize = true,
                Font = new Font("Segoe UI", 7),
                ForeColor = Color.Gray,
                Location = new Point(12, lbl.Bottom + 4)
            };
            bubble.Controls.Add(lblTime);
            bubble.Height = lblTime.Bottom + 10;

            flowMessages.Controls.Add(bubble);
            ScrollToBottom();
        }

        private void AddChatBubble(string text, bool isMe, DateTime time, string status)
        {
            Panel bubble = new Panel()
            {
                AutoSize = true,
                MaximumSize = new Size(260, 0),
                MinimumSize = new Size(60, 40),
                Padding = new Padding(12, 10, 12, 10),
                Margin = isMe ? new Padding(80, 5, 5, 3) : new Padding(5, 5, 80, 3),
                BackColor = isMe ? Color.FromArgb(79, 172, 254) : Color.White
            };

            bubble.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(isMe ? Color.FromArgb(79, 172, 254) : Color.FromArgb(220, 220, 220), 1))
                {
                    e.Graphics.DrawRoundedRectangle(pen, 0, 0, bubble.Width - 1, bubble.Height - 1, 12);
                }
            };

            Label lbl = new Label()
            {
                Text = text,
                AutoSize = true,
                MaximumSize = new Size(230, 0),
                Font = new Font("Segoe UI", 9),
                ForeColor = isMe ? Color.White : Color.FromArgb(50, 50, 50),
                Location = new Point(12, 10)
            };
            bubble.Controls.Add(lbl);

            string timeText = time.ToString("HH:mm");
            if (!string.IsNullOrEmpty(status) && isMe)
            {
                timeText += " • " + status;
            }

            Label lblTime = new Label()
            {
                Text = timeText,
                AutoSize = true,
                Font = new Font("Segoe UI", 7),
                ForeColor = isMe ? Color.FromArgb(220, 240, 255) : Color.Gray,
                Location = new Point(12, lbl.Bottom + 4)
            };
            bubble.Controls.Add(lblTime);
            bubble.Height = lblTime.Bottom + 10;

            flowMessages.Controls.Add(bubble);
            ScrollToBottom();
        }

        private void AddSystemMessage(string msg)
        {
            Label lbl = new Label()
            {
                Text = msg,
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.Gray,
                Padding = new Padding(0, 8, 0, 8),
                TextAlign = ContentAlignment.MiddleCenter,
                Width = 340
            };
            flowMessages.Controls.Add(lbl);
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            if (flowMessages.Controls.Count > 0)
            {
                flowMessages.ScrollControlIntoView(flowMessages.Controls[flowMessages.Controls.Count - 1]);
                flowMessages.VerticalScroll.Value = flowMessages.VerticalScroll.Maximum;
            }
        }

        private class MessageData
        {
            public int MaTN { get; set; }
            public int MaNguoiGui { get; set; }
            public string NoiDung { get; set; }
            public DateTime NgayGui { get; set; }
            public string TrangThai { get; set; }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            refreshTimer?.Stop();
            refreshTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }

    // Extension method để vẽ hình chữ nhật bo góc
    public static class GraphicsExtensions
    {
        public static void DrawRoundedRectangle(this Graphics g, Pen pen, float x, float y, float w, float h, float r)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(x, y, r * 2, r * 2, 180, 90);
                path.AddArc(x + w - r * 2, y, r * 2, r * 2, 270, 90);
                path.AddArc(x + w - r * 2, y + h - r * 2, r * 2, r * 2, 0, 90);
                path.AddArc(x, y + h - r * 2, r * 2, r * 2, 90, 90);
                path.CloseFigure();
                g.DrawPath(pen, path);
            }
        }
    }
}