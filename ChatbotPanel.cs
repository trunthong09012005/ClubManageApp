using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Media;

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

            // Badge số tin nhắn chưa đọc
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
                    // Chỉ đếm tin nhắn chưa đọc từ Admin (không đếm tin cũ)
                    using (SqlCommand cmd = new SqlCommand(
                        @"SELECT COUNT(*) FROM TinNhan TN
                          INNER JOIN ThanhVien TV ON TN.MaNguoiGui = TV.MaTV
                          LEFT JOIN TaiKhoan TK ON TN.MaNguoiGui = TK.MaTV
                          WHERE TN.MaNguoiNhan = @maTV 
                            AND TN.TrangThai = N'Chưa đọc'
                            AND (TV.MaCV = 1 OR TK.QuyenHan IN (N'Admin', N'Quản trị viên'))
                            AND TN.NgayGui >= DATEADD(HOUR, -1, GETDATE())", conn))
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

    // ============ FORM CHATBOT ============
    public class ChatbotForm : Form
    {
        private FlowLayoutPanel flowMessages;
        private TextBox txtMessage;
        private Button btnSend, btnSwitchMode;
        private Panel pnlHeader;
        private Label lblTitle, lblStatus, lblTyping;
        private bool isAdminChatMode = false;
        private string connectionString;
        private int maTV;
        private string username;
        private int adminID;
        private string adminName;
        private Timer refreshTimer;
        private int lastMessageID = 0;
        private Dictionary<string[], string> faqResponses;

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

            AddBotMessage($"Xin chào {username}! 👋\n\n💡 Nhấn 'Chat Admin' để được hỗ trợ trực tiếp!");
        }

        private void LoadAdminInfo()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Chỉ lấy để hiển thị tên, tin nhắn sẽ broadcast tới tất cả Admin
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
                            adminName = "Admin CLB"; // Hiển thị tên chung
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
                { new[] { "xin chào", "hello", "hi", "chào" }, $"Xin chào {username}! 👋\n\n💡 Nhấn 'Chat Admin' để nói chuyện với {adminName}!" },
                { new[] { "hoạt động", "sự kiện" }, "📅 Vào menu 'Hoạt động' hoặc hỏi 'hoạt động sắp tới'" },
                { new[] { "điểm", "điểm rèn luyện" }, "📊 Hỏi tôi: 'điểm của tôi' để xem điểm RL" },
                { new[] { "admin", "quản lý", "liên hệ" }, $"📞 Nhấn 'Chat Admin' để chat với {adminName}!" },
                { new[] { "help", "giúp", "hướng dẫn" }, "🤖 Tôi có thể giúp:\n• Thông tin hoạt động CLB\n• Điểm rèn luyện\n• Chat với Admin" },
                { new[] { "cảm ơn", "thanks" }, "Không có gì! 😊" }
            };
        }

        private void SetupRefreshTimer()
        {
            refreshTimer = new Timer() { Interval = 1500 };
            refreshTimer.Tick += (s, e) => {
                if (isAdminChatMode) LoadNewMessages();
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
                Size = new Size(360, 75),
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

            btnSwitchMode = new Button()
            {
                Text = $"💬 Chat {adminName}",
                Size = new Size(140, 28),
                Location = new Point(15, 52),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(52, 211, 153),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSwitchMode.FlatAppearance.BorderSize = 0;
            btnSwitchMode.Click += BtnSwitchMode_Click;
            pnlHeader.Controls.Add(btnSwitchMode);

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

        private void CreateMessageArea()
        {
            flowMessages = new FlowLayoutPanel()
            {
                Location = new Point(5, 80),
                Size = new Size(350, 355),
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
            isAdminChatMode = !isAdminChatMode;
            flowMessages.Controls.Clear();

            if (isAdminChatMode)
            {
                lblTitle.Text = $"👤 {adminName}";
                lblStatus.Text = "● Trực tuyến";
                lblStatus.ForeColor = Color.FromArgb(52, 211, 153);
                btnSwitchMode.Text = "🤖 Chat Bot";
                btnSwitchMode.BackColor = Color.FromArgb(79, 172, 254);
                pnlHeader.BackColor = Color.FromArgb(52, 73, 94);

                // Lấy MAX ID hiện tại trước khi load tin cũ
                int currentMaxID = GetCurrentMaxMessageID();

                AddSystemMessage($"💬 Bắt đầu chat với {adminName}!");

                // Load 10 tin nhắn gần nhất
                LoadRecentMessages();

                // Set lastMessageID sau khi load
                lastMessageID = currentMaxID;

                // Bắt đầu refresh
                refreshTimer.Start();

                // Test ngay lập tức
                LoadNewMessages();
            }
            else
            {
                lblTitle.Text = "🤖 Trợ lý CLB";
                lblStatus.Text = "Bot tự động";
                lblStatus.ForeColor = Color.FromArgb(220, 220, 220);
                btnSwitchMode.Text = $"💬 Chat {adminName}";
                btnSwitchMode.BackColor = Color.FromArgb(52, 211, 153);
                pnlHeader.BackColor = Color.FromArgb(79, 172, 254);

                refreshTimer.Stop();
                AddBotMessage("Chào mừng quay lại! 👋\n\nTôi có thể giúp gì cho bạn?");
            }

            txtMessage.Focus();
        }

        // Load 10 tin nhắn gần nhất
        private void LoadRecentMessages()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        @"SELECT TOP 10 MaTN, MaNguoiGui, NoiDung, NgayGui, TrangThai
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

                        // Đảo ngược để hiển thị theo thứ tự thời gian
                        messages.Reverse();

                        foreach (var msg in messages)
                        {
                            bool isMe = (msg.MaNguoiGui == maTV);
                            AddChatBubble(msg.NoiDung, isMe, msg.NgayGui, isMe ? msg.TrangThai : null);
                        }

                        if (messages.Count > 0)
                        {
                            AddSystemMessage($"📜 Đã load {messages.Count} tin gần nhất");
                        }
                    }

                    MarkMessagesAsRead(conn);
                    OnMessagesRead?.Invoke();
                }
            }
            catch (Exception ex)
            {
                AddSystemMessage("⚠️ Lỗi load tin cũ: " + ex.Message);
            }
        }

        // Class để lưu trữ tin nhắn
        private class MessageData
        {
            public int MaTN { get; set; }
            public int MaNguoiGui { get; set; }
            public string NoiDung { get; set; }
            public DateTime NgayGui { get; set; }
            public string TrangThai { get; set; }
        }

        // Lấy ID tin nhắn mới nhất để làm baseline
        private int GetCurrentMaxMessageID()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        @"SELECT ISNULL(MAX(MaTN), 0) FROM TinNhan 
                          WHERE MaNguoiNhan = @maTV OR MaNguoiGui = @maTV", conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        private void LoadChatHistory()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Lấy tin nhắn giữa member và TẤT CẢ Admin
                    using (SqlCommand cmd = new SqlCommand(
                        @"SELECT TOP 50 TN.MaTN, TN.MaNguoiGui, TN.NoiDung, TN.NgayGui, TN.TrangThai 
                          FROM TinNhan TN
                          LEFT JOIN ThanhVien TV ON TN.MaNguoiGui = TV.MaTV
                          LEFT JOIN TaiKhoan TK ON TN.MaNguoiGui = TK.MaTV
                          WHERE (TN.MaNguoiGui = @maTV AND (TV.MaCV = 1 OR TK.QuyenHan IN (N'Admin', N'Quản trị viên')))
                             OR ((TV.MaCV = 1 OR TK.QuyenHan IN (N'Admin', N'Quản trị viên')) AND TN.MaNguoiNhan = @maTV)
                             OR (TN.MaNguoiGui = @maTV AND TN.MaNguoiNhan IN (
                                SELECT TV2.MaTV FROM ThanhVien TV2 
                                LEFT JOIN TaiKhoan TK2 ON TV2.MaTV = TK2.MaTV
                                WHERE TV2.MaCV = 1 OR TK2.QuyenHan IN (N'Admin', N'Quản trị viên')
                             ))
                          ORDER BY TN.NgayGui ASC", conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        SqlDataReader reader = cmd.ExecuteReader();

                        int count = 0;
                        while (reader.Read())
                        {
                            count++;
                            int msgID = Convert.ToInt32(reader["MaTN"]);
                            bool isMe = Convert.ToInt32(reader["MaNguoiGui"]) == maTV;
                            string status = reader["TrangThai"].ToString();
                            AddChatBubble(
                                reader["NoiDung"].ToString(),
                                isMe,
                                Convert.ToDateTime(reader["NgayGui"]),
                                isMe ? status : null
                            );
                            if (msgID > lastMessageID) lastMessageID = msgID;
                        }
                        reader.Close();

                        if (count == 0)
                        {
                            AddSystemMessage($"💬 Bắt đầu chat với {adminName}!");
                        }
                    }
                    MarkMessagesAsRead(conn);
                    OnMessagesRead?.Invoke();
                }
            }
            catch (Exception ex) { AddSystemMessage("❌ Lỗi: " + ex.Message); }
        }

        private void MarkMessagesAsRead(SqlConnection conn)
        {
            try
            {
                // Đánh dấu tất cả tin từ Admin đã đọc
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

                    // Chỉ lấy tin nhắn mới từ BẤT KỲ Admin nào (sau khi bắt đầu chat)
                    string query = lastMessageID == 0
                        ? @"SELECT TN.MaTN, TN.NoiDung, TN.NgayGui 
                            FROM TinNhan TN
                            INNER JOIN ThanhVien TV ON TN.MaNguoiGui = TV.MaTV
                            LEFT JOIN TaiKhoan TK ON TN.MaNguoiGui = TK.MaTV
                            WHERE TN.MaNguoiNhan = @maTV
                              AND (TV.MaCV = 1 OR TK.QuyenHan IN (N'Admin', N'Quản trị viên'))
                              AND TN.NgayGui >= DATEADD(SECOND, -5, GETDATE())
                            ORDER BY TN.NgayGui ASC"
                        : @"SELECT TN.MaTN, TN.NoiDung, TN.NgayGui 
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
                        {
                            cmd.Parameters.AddWithValue("@lastID", lastMessageID);
                        }
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        SqlDataReader reader = cmd.ExecuteReader();
                        bool hasNew = false;

                        while (reader.Read())
                        {
                            hasNew = true;
                            lastMessageID = Convert.ToInt32(reader["MaTN"]);
                            AddChatBubble(reader["NoiDung"].ToString(), false, Convert.ToDateTime(reader["NgayGui"]), null);
                            SystemSounds.Asterisk.Play();
                        }
                        reader.Close();

                        if (hasNew)
                        {
                            MarkMessagesAsRead(conn);
                            OnMessagesRead?.Invoke();
                        }
                    }
                }
            }
            catch { }
        }

        private void UpdateMessageStatus(SqlConnection conn)
        {
            try
            {
                // Kiểm tra trạng thái tin nhắn đã gửi
                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT TN.MaTN 
                      FROM TinNhan TN
                      INNER JOIN ThanhVien TV ON TN.MaNguoiNhan = TV.MaTV
                      LEFT JOIN TaiKhoan TK ON TN.MaNguoiNhan = TK.MaTV
                      WHERE TN.MaNguoiGui = @maTV 
                        AND (TV.MaCV = 1 OR TK.QuyenHan IN (N'Admin', N'Quản trị viên'))
                        AND TN.TrangThai = N'Đã đọc'", conn))
                {
                    cmd.Parameters.AddWithValue("@maTV", maTV);
                }
            }
            catch { }
        }

        private void SendMessage()
        {
            string message = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;
            txtMessage.Clear();

            if (isAdminChatMode)
            {
                SendAdminMessage(message);
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

        private void SendAdminMessage(string message)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // BROADCAST: Gửi tin nhắn tới TẤT CẢ Admin (trừ chính mình)
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
                    }
                }
            }
            catch (Exception ex) { AddSystemMessage("❌ Lỗi: " + ex.Message); }
        }

        private string GetBotResponse(string message)
        {
            string lower = message.ToLower();

            foreach (var faq in faqResponses)
                foreach (string kw in faq.Key)
                    if (lower.Contains(kw)) return faq.Value;

            if (lower.Contains("hoạt động sắp tới")) return GetUpcomingActivities();
            if (lower.Contains("điểm của tôi") || lower.Contains("điểm hiện tại")) return GetMemberPoints();

            return $"🤔 Tôi chưa hiểu câu hỏi.\n\n💡 Nhấn 'Chat {adminName}' để được hỗ trợ!";
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