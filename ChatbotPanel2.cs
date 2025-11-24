using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Media;

namespace ClubManageApp
{
    public class ChatbotPanel2 : UserControl
    {
        private Button btnToggle;
        private Label lblBadge;
        private ChatForm2 chatForm;
        private Timer badgeTimer;
        private Timer pulseTimer;
        private string connectionString;
        private int maTV;
        private string username;
        private int unreadCount = 0;
        private float pulseScale = 1.0f;
        private bool pulseGrowing = true;
        private ToolTip tooltip;

        // THÊM VÀO CLASS ChatbotPanel2 HIỆN TẠI CỦA BẠN

        // ===== THÊM METHOD NÀY VÀO CONSTRUCTOR =====
        public ChatbotPanel2(string connString, int memberID, string user)
        {
            connectionString = connString;
            maTV = memberID;
            username = user;
            InitializeComponents();
            SetupBadgeTimer();
            SetupPulseAnimation();

            // ===== THÊM DÒNG NÀY ĐỂ BO TRÒN TOÀN BỘ =====
            MakeRounded();
        }

        // ===== THÊM METHOD MỚI NÀY VÀO CLASS =====
        private void MakeRounded()
        {
            // Bo tròn toàn bộ ChatbotPanel2
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, this.Width, this.Height);
            this.Region = new Region(path);
        }

        // ===== THÊM OVERRIDE NÀY ĐỂ VẼ NỀN GRADIENT CHO PANEL =====
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(2, 2, this.Width - 4, this.Height - 4);

            // Gradient đẹp như cũ
            using (var brush = new LinearGradientBrush(rect,
                Color.FromArgb(99, 102, 241),   // Indigo
                Color.FromArgb(168, 85, 247),   // Purple
                45F))
            {
                e.Graphics.FillEllipse(brush, rect);
            }

            // Inner glow (ánh sáng bên trong)
            using (var glowPath = new GraphicsPath())
            {
                glowPath.AddEllipse(8, 8, this.Width - 16, this.Height - 16);
                using (var glowBrush = new PathGradientBrush(glowPath))
                {
                    glowBrush.CenterColor = Color.FromArgb(40, 255, 255, 255);
                    glowBrush.SurroundColors = new Color[] { Color.Transparent };
                    e.Graphics.FillPath(glowBrush, glowPath);
                }
            }

            // Shadow nhẹ
            using (var shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
            {
                e.Graphics.FillEllipse(shadowBrush, 4, this.Height - 8, this.Width - 8, 6);
            }
        }

        // ===== SỬA LẠI InitializeComponents() =====
        private void InitializeComponents()
        {
            this.Size = new Size(70, 70);
            this.BackColor = Color.Transparent; // QUAN TRỌNG
            this.DoubleBuffered = true;

            // Tooltip
            tooltip = new ToolTip();
            tooltip.SetToolTip(this, "Nhắn tin");

            // Main toggle button - ĐẶT NHỎ HƠN ĐỂ NẰM TRONG PANEL TRÒ  N
            btnToggle = new Button()
            {
                Size = new Size(60, 60),
                Location = new Point(5, 5), // Canh giữa trong panel 70x70
                Text = "💬",
                Font = new Font("Segoe UI Emoji", 20),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TabStop = false,
                BackColor = Color.Transparent // QUAN TRỌNG
            };
            btnToggle.FlatAppearance.BorderSize = 0;
            btnToggle.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnToggle.FlatAppearance.MouseDownBackColor = Color.Transparent;

            // Circular shape cho button
            var path = new GraphicsPath();
            path.AddEllipse(0, 0, btnToggle.Width, btnToggle.Height);
            btnToggle.Region = new Region(path);

            btnToggle.Paint += BtnToggle_Paint;
            btnToggle.MouseEnter += (s, e) => { pulseGrowing = true; btnToggle.Invalidate(); };
            btnToggle.MouseLeave += (s, e) => btnToggle.Invalidate();
            btnToggle.Click += BtnToggle_Click;

            this.Controls.Add(btnToggle);

            // Badge - ĐẶT Ở GÓC PHẢI TRÊN
            lblBadge = new Label()
            {
                Size = new Size(24, 24),
                Location = new Point(46, 0), // Góc phải trên của panel 70x70
                Text = "0",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };

            var badgePath = new GraphicsPath();
            badgePath.AddEllipse(0, 0, lblBadge.Width, lblBadge.Height);
            lblBadge.Region = new Region(badgePath);
            lblBadge.Paint += LblBadge_Paint;

            this.Controls.Add(lblBadge);
            lblBadge.BringToFront();
        }

        // ===== SỬA LẠI BtnToggle_Paint ĐỂ CHỈ VẼ ICON =====
        private void BtnToggle_Paint(object sender, PaintEventArgs e)
        {
            var btn = sender as Button;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.Transparent); // Trong suốt, dùng nền của panel

            // VẼ NỀN TRÒN MÀU TÍM (gradient)
            Rectangle rect = new Rectangle(0, 0, btn.Width, btn.Height);
            using (var brush = new LinearGradientBrush(rect,
                Color.FromArgb(99, 102, 241),
                Color.FromArgb(168, 85, 247),
                45F))
            {
                e.Graphics.FillEllipse(brush, rect);
            }

            // Ánh sáng bên trong nhẹ - tạo path rồi dùng PathGradientBrush
            using (var gp = new GraphicsPath())
            {
                gp.AddEllipse(6, 6, btn.Width - 12, btn.Height - 12);
                using (var inner = new PathGradientBrush(gp))
                {
                    inner.CenterColor = Color.FromArgb(40, 255, 255, 255);
                    inner.SurroundColors = new Color[] { Color.Transparent };
                    e.Graphics.FillPath(inner, gp);
                }
            }

            // VẼ viền mỏng trắng
            using (var pen = new Pen(Color.FromArgb(200, Color.White), 2))
            {
                e.Graphics.DrawEllipse(pen, 1, 1, btn.Width - 3, btn.Height - 3);
            }

            // VẼ chữ (icon) ở giữa
            TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font,
                rect, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        // LblBadge_Paint giữ nguyên như cũ
        private void LblBadge_Paint(object sender, PaintEventArgs e)
        {
            var lbl = sender as Label;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.Transparent);

            // Red gradient for badge
            Rectangle rect = new Rectangle(0, 0, lbl.Width, lbl.Height);
            using (var brush = new LinearGradientBrush(rect,
                Color.FromArgb(248, 113, 113),
                Color.FromArgb(220, 38, 38),
                90F))
            {
                e.Graphics.FillEllipse(brush, rect);
            }

            // Border
            using (var pen = new Pen(Color.White, 2))
            {
                e.Graphics.DrawEllipse(pen, 1, 1, lbl.Width - 2, lbl.Height - 2);
            }

            // Text
            TextRenderer.DrawText(e.Graphics, lbl.Text, lbl.Font,
                rect, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void SetupPulseAnimation()
        {
            pulseTimer = new Timer() { Interval = 50 };
            pulseTimer.Tick += (s, e) =>
            {
                if (unreadCount > 0)
                {
                    if (pulseGrowing)
                    {
                        pulseScale += 0.02f;
                        if (pulseScale >= 1.1f) pulseGrowing = false;
                    }
                    else
                    {
                        pulseScale -= 0.02f;
                        if (pulseScale <= 1.0f) pulseGrowing = true;
                    }
                    lblBadge.Invalidate();
                }
            };
            pulseTimer.Start();
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
                            AND MaNguoiGui != @maTV
                            AND NgayGui >= DATEADD(HOUR, -1, GETDATE())", conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count != unreadCount)
                        {
                            if (count > unreadCount && unreadCount >= 0)
                                SystemSounds.Asterisk.Play();

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
                chatForm = new ChatForm2(connectionString, maTV, username);
                // ensure toggle resets when form gets hidden or closed
                chatForm.VisibleChanged += (s, args) =>
                {
                    if (!chatForm.Visible)
                    {
                        btnToggle.Text = "💬";
                        btnToggle.Invalidate();
                    }
                };
                chatForm.FormClosed += (s, args) =>
                {
                    btnToggle.Text = "💬";
                    btnToggle.Invalidate();
                };
                chatForm.OnMessagesRead += () =>
                {
                    unreadCount = 0;
                    UpdateBadge();
                };
            }

            if (chatForm.Visible)
            {
                chatForm.Hide();
                btnToggle.Text = "💬";
            }
            else
            {
                Point screenPos = this.PointToScreen(new Point(this.Width, this.Height));
                chatForm.Location = new Point(screenPos.X - chatForm.Width - 10, screenPos.Y - chatForm.Height - 10);
                chatForm.Show();
                chatForm.Activate();
                btnToggle.Text = "✕";
            }
            btnToggle.Invalidate();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            badgeTimer?.Stop();
            badgeTimer?.Dispose();
            pulseTimer?.Stop();
            pulseTimer?.Dispose();
            base.OnHandleDestroyed(e);
        }
    }

    // ===================== FORM CHAT 1-1 - MODERN UI =====================
    public class ChatForm2 : Form
    {
        private FlowLayoutPanel flowMessages;
        private TextBox txtMessage;
        private Button btnSend;
        private ComboBox cmbMembers;
        private Panel headerPanel;
        private Label lblStatus;
        private string connectionString;
        private int maTV;
        private string username;
        private int lastMessageID = 0;
        private Timer refreshTimer;
        private Timer typingTimer;

        public event Action OnMessagesRead;

        public ChatForm2(string connString, int memberID, string user)
        {
            connectionString = connString;
            maTV = memberID;
            username = user;

            InitializeForm();
            LoadMembers();
            SetupRefreshTimer();
        }

        private void InitializeForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(390, 560);
            this.StartPosition = FormStartPosition.Manual;

            this.TopMost = true;
            this.DoubleBuffered = true;

            // Rounded form shape
            int radius = 24;
            GraphicsPath formPath = new GraphicsPath();
            Rectangle bounds = new Rectangle(0, 0, this.Width, this.Height);
            formPath.AddArc(bounds.X, bounds.Y, radius * 2, radius * 2, 180, 90);
            formPath.AddArc(bounds.Right - radius * 2, bounds.Y, radius * 2, radius * 2, 270, 90);
            formPath.AddArc(bounds.Right - radius * 2, bounds.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            formPath.AddArc(bounds.X, bounds.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            formPath.CloseFigure();
            this.Region = new Region(formPath);

            // Main container with rounded corners and shadow effect
            Panel mainContainer = new Panel()
            {
                Size = new Size(390, 560),
                Location = new Point(0, 0),
                BackColor = Color.White
            };
            mainContainer.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // Soft gradient background
                using (var brush = new LinearGradientBrush(
                    new Rectangle(0, 0, mainContainer.Width, mainContainer.Height),
                    Color.FromArgb(255, 255, 255),
                    Color.FromArgb(249, 250, 251),
                    90F))
                {
                    e.Graphics.FillRectangle(brush, mainContainer.ClientRectangle);
                }
            };
            this.Controls.Add(mainContainer);

            // Modern Header with gradient and rounded top corners
            headerPanel = new Panel()
            {
                Size = new Size(390, 80),
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };
            headerPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                int r = 24;
                using (var path = new GraphicsPath())
                {
                    path.AddArc(0, 0, r * 2, r * 2, 180, 90);
                    path.AddArc(headerPanel.Width - r * 2, 0, r * 2, r * 2, 270, 90);
                    path.AddLine(headerPanel.Width, r, headerPanel.Width, headerPanel.Height);
                    path.AddLine(headerPanel.Width, headerPanel.Height, 0, headerPanel.Height);
                    path.AddLine(0, headerPanel.Height, 0, r);
                    path.CloseFigure();

                    using (var brush = new LinearGradientBrush(
                        new Rectangle(0, 0, headerPanel.Width, headerPanel.Height),
                        Color.FromArgb(99, 102, 241),
                        Color.FromArgb(168, 85, 247),
                        45F))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
            };
            mainContainer.Controls.Add(headerPanel);

            // Avatar placeholder
            Panel avatar = new Panel()
            {
                Size = new Size(44, 44),
                Location = new Point(18, 18),
                BackColor = Color.Transparent
            };
            avatar.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // White circle background
                using (var brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
                {
                    e.Graphics.FillEllipse(brush, 0, 0, 43, 43);
                }
                // Gradient inner circle
                using (var brush = new LinearGradientBrush(
                    new Rectangle(4, 4, 36, 36),
                    Color.FromArgb(199, 210, 254),
                    Color.FromArgb(233, 213, 255),
                    45F))
                {
                    e.Graphics.FillEllipse(brush, 4, 4, 35, 35);
                }
                TextRenderer.DrawText(e.Graphics, "👤", new Font("Segoe UI Emoji", 14),
                    new Rectangle(0, 0, 44, 44), Color.FromArgb(99, 102, 241),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            var avatarPath = new GraphicsPath();
            avatarPath.AddEllipse(0, 0, 44, 44);
            avatar.Region = new Region(avatarPath);
            headerPanel.Controls.Add(avatar);

            // Member selector - styled with rounded corners
            cmbMembers = new ComboBox()
            {
                Location = new Point(72, 20),
                Width = 200,
                Height = 30,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI Semibold", 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(255, 255, 255)
            };
            cmbMembers.SelectedIndexChanged += (s, e) => LoadChatHistory();
            headerPanel.Controls.Add(cmbMembers);

            // Status label
            lblStatus = new Label()
            {
                Text = "● Trực tuyến",
                Location = new Point(72, 52),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(167, 243, 208),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblStatus);

            // Close button - modern floating style
            Button btnClose = new Button()
            {
                Text = "✕",
                Size = new Size(38, 38),
                Location = new Point(340, 20),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(40, 255, 255, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 255, 255, 255);
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 255, 255, 255);
            btnClose.Click += (s, e) => this.Hide();

            var closePath = new GraphicsPath();
            closePath.AddEllipse(0, 0, 38, 38);
            btnClose.Region = new Region(closePath);
            headerPanel.Controls.Add(btnClose);

            mainContainer.Controls.Add(headerPanel);

            // Messages area with soft rounded container
            Panel messagesContainer = new Panel()
            {
                Location = new Point(15, 90),
                Size = new Size(360, 385),
                BackColor = Color.Transparent
            };
            messagesContainer.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = CreateRoundedRectangle(0, 0, messagesContainer.Width - 1, messagesContainer.Height - 1, 18))
                {
                    // Soft inner shadow / background
                    using (var brush = new SolidBrush(Color.FromArgb(248, 250, 252)))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                    using (var pen = new Pen(Color.FromArgb(226, 232, 240), 1.5f))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };

            flowMessages = new FlowLayoutPanel()
            {
                Location = new Point(8, 8),
                Size = new Size(344, 369),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.FromArgb(248, 250, 252)
            };
            messagesContainer.Controls.Add(flowMessages);
            mainContainer.Controls.Add(messagesContainer);

            // Modern input area with rounded bottom
            Panel inputPanel = new Panel()
            {
                Location = new Point(0, 485),
                Size = new Size(390, 75),
                BackColor = Color.Transparent
            };
            inputPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                int r = 24;
                using (var path = new GraphicsPath())
                {
                    path.AddLine(0, 0, inputPanel.Width, 0);
                    path.AddLine(inputPanel.Width, 0, inputPanel.Width, inputPanel.Height - r);
                    path.AddArc(inputPanel.Width - r * 2, inputPanel.Height - r * 2, r * 2, r * 2, 0, 90);
                    path.AddArc(0, inputPanel.Height - r * 2, r * 2, r * 2, 90, 90);
                    path.AddLine(0, inputPanel.Height - r, 0, 0);
                    path.CloseFigure();

                    using (var brush = new SolidBrush(Color.White))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }

                // Rounded input field background
                using (var fieldPath = CreateRoundedRectangle(15, 15, 290, 45, 22))
                {
                    using (var brush = new SolidBrush(Color.FromArgb(241, 245, 249)))
                    {
                        e.Graphics.FillPath(brush, fieldPath);
                    }
                    using (var pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                    {
                        e.Graphics.DrawPath(pen, fieldPath);
                    }
                }
            };

            txtMessage = new TextBox()
            {
                Location = new Point(30, 26),
                Width = 250,
                Height = 28,
                Font = new Font("Segoe UI", 10.5f),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(241, 245, 249)
            };
            txtMessage.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    SendMessage();
                }
            };
            // Add placeholder behavior
            txtMessage.GotFocus += (s, e) => { if (txtMessage.ForeColor == Color.Gray) { txtMessage.Text = ""; txtMessage.ForeColor = Color.FromArgb(30, 41, 59); } };
            txtMessage.LostFocus += (s, e) => { if (string.IsNullOrEmpty(txtMessage.Text)) { txtMessage.Text = "Nhập tin nhắn..."; txtMessage.ForeColor = Color.Gray; } };
            txtMessage.Text = "Nhập tin nhắn...";
            txtMessage.ForeColor = Color.Gray;
            inputPanel.Controls.Add(txtMessage);

            // Send button - beautiful circular gradient
            btnSend = new Button()
            {
                Size = new Size(48, 48),
                Location = new Point(325, 13),
                Text = "",
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnSend.FlatAppearance.MouseDownBackColor = Color.Transparent;

            var sendPath = new GraphicsPath();
            sendPath.AddEllipse(0, 0, 48, 48);
            btnSend.Region = new Region(sendPath);
            btnSend.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, 48, 48);

                // Gradient background
                using (var brush = new LinearGradientBrush(rect,
                    Color.FromArgb(99, 102, 241),
                    Color.FromArgb(168, 85, 247),
                    45F))
                {
                    e.Graphics.FillEllipse(brush, 2, 2, 44, 44);
                }

                // Send arrow icon
                using (var pen = new Pen(Color.White, 2.5f))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    pen.LineJoin = LineJoin.Round;

                    // Arrow shape
                    Point[] arrow = new Point[]
                    {
                        new Point(18, 24),
                        new Point(30, 24),
                    };
                    e.Graphics.DrawLines(pen, arrow);

                    // Arrow head
                    Point[] head = new Point[]
                    {
                        new Point(24, 18),
                        new Point(30, 24),
                        new Point(24, 30)
                    };
                    e.Graphics.DrawLines(pen, head);
                }
            };
            btnSend.Click += (s, e) => SendMessage();
            inputPanel.Controls.Add(btnSend);

            mainContainer.Controls.Add(inputPanel);
        }

        private GraphicsPath CreateRoundedRectangle(int x, int y, int width, int height, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(x, y, d, d, 180, 90);
            path.AddArc(x + width - d, y, d, d, 270, 90);
            path.AddArc(x + width - d, y + height - d, d, d, 0, 90);
            path.AddArc(x, y + height - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void SetupRefreshTimer()
        {
            refreshTimer = new Timer() { Interval = 1500 };
            refreshTimer.Tick += (s, e) => LoadNewMessages();
            refreshTimer.Start();
        }

        private void LoadMembers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT MaTV, HoTen FROM ThanhVien WHERE MaTV != @maTV ORDER BY HoTen", conn);
                    cmd.Parameters.AddWithValue("@maTV", maTV);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cmbMembers.Items.Add(new ComboBoxItem
                        {
                            Text = reader["HoTen"].ToString(),
                            Value = Convert.ToInt32(reader["MaTV"])
                        });
                    }
                    if (cmbMembers.Items.Count > 0) cmbMembers.SelectedIndex = 0;
                }
            }
            catch { }
        }

        private void LoadChatHistory()
        {
            if (cmbMembers.SelectedItem == null) return;
            int memberChatID = ((ComboBoxItem)cmbMembers.SelectedItem).Value;
            flowMessages.Controls.Clear();
            lastMessageID = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT MaTN, MaNguoiGui, NoiDung, NgayGui, TrangThai 
                          FROM TinNhan 
                          WHERE (MaNguoiGui = @me AND MaNguoiNhan = @them) 
                             OR (MaNguoiGui = @them AND MaNguoiNhan = @me)
                          ORDER BY NgayGui ASC", conn);
                    cmd.Parameters.AddWithValue("@me", maTV);
                    cmd.Parameters.AddWithValue("@them", memberChatID);

                    SqlDataReader r = cmd.ExecuteReader();
                    DateTime? lastDate = null;

                    while (r.Read())
                    {
                        DateTime msgDate = Convert.ToDateTime(r["NgayGui"]);

                        // Add date separator if new day
                        if (!lastDate.HasValue || lastDate.Value.Date != msgDate.Date)
                        {
                            AddDateSeparator(msgDate);
                            lastDate = msgDate;
                        }

                        bool isMe = Convert.ToInt32(r["MaNguoiGui"]) == maTV;
                        AddChatBubble(r["NoiDung"].ToString(), isMe, msgDate,
                            isMe ? r["TrangThai"].ToString() : null);
                        int msgID = Convert.ToInt32(r["MaTN"]);
                        if (msgID > lastMessageID) lastMessageID = msgID;
                    }
                }
            }
            catch { }
        }

        private void AddDateSeparator(DateTime date)
        {
            Panel separator = new Panel()
            {
                Size = new Size(330, 35),
                Margin = new Padding(0, 12, 0, 12),
                BackColor = Color.Transparent
            };
            separator.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                string text = date.Date == DateTime.Today ? "Hôm nay" :
                              date.Date == DateTime.Today.AddDays(-1) ? "Hôm qua" :
                              date.ToString("dd/MM/yyyy");

                // Dotted lines
                using (var pen = new Pen(Color.FromArgb(203, 213, 225), 1))
                {
                    pen.DashStyle = DashStyle.Dot;
                    e.Graphics.DrawLine(pen, 15, 17, 95, 17);
                    e.Graphics.DrawLine(pen, 235, 17, 315, 17);
                }

                // Rounded pill background for date
                using (var path = CreateRoundedRectangle(100, 5, 130, 24, 12))
                {
                    using (var brush = new SolidBrush(Color.FromArgb(241, 245, 249)))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }

                TextRenderer.DrawText(e.Graphics, text, new Font("Segoe UI", 8.5f),
                    new Rectangle(100, 5, 130, 24), Color.FromArgb(100, 116, 139),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            flowMessages.Controls.Add(separator);
        }

        private void LoadNewMessages()
        {
            if (cmbMembers.SelectedItem == null) return;
            int memberChatID = ((ComboBoxItem)cmbMembers.SelectedItem).Value;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT MaTN, MaNguoiGui, NoiDung, NgayGui 
                          FROM TinNhan 
                          WHERE MaTN > @lastID AND 
                                ((MaNguoiGui = @them AND MaNguoiNhan = @me) OR (MaNguoiGui = @me AND MaNguoiNhan = @them))
                          ORDER BY NgayGui ASC", conn);
                    cmd.Parameters.AddWithValue("@lastID", lastMessageID);
                    cmd.Parameters.AddWithValue("@me", maTV);
                    cmd.Parameters.AddWithValue("@them", memberChatID);

                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        bool isMe = Convert.ToInt32(r["MaNguoiGui"]) == maTV;
                        AddChatBubble(r["NoiDung"].ToString(), isMe,
                            Convert.ToDateTime(r["NgayGui"]), isMe ? "Đã gửi" : null);
                        lastMessageID = Convert.ToInt32(r["MaTN"]);
                    }
                }
            }
            catch { }
        }

        private void SendMessage()
        {
            if (cmbMembers.SelectedItem == null) return;
            string msg = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(msg)) return;
            txtMessage.Clear();

            int memberChatID = ((ComboBoxItem)cmbMembers.SelectedItem).Value;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        @"INSERT INTO TinNhan(MaNguoiGui, MaNguoiNhan, NoiDung, NgayGui, TrangThai)
                          VALUES(@me, @them, @noiDung, GETDATE(), N'Chưa đọc'); SELECT SCOPE_IDENTITY();", conn);
                    cmd.Parameters.AddWithValue("@me", maTV);
                    cmd.Parameters.AddWithValue("@them", memberChatID);
                    cmd.Parameters.AddWithValue("@noiDung", msg);
                    object id = cmd.ExecuteScalar();
                    lastMessageID = Convert.ToInt32(id);
                    AddChatBubble(msg, true, DateTime.Now, "Đã gửi");
                }
            }
            catch { }
        }

        private void AddChatBubble(string text, bool isMe, DateTime time, string status)
        {
            Panel container = new Panel()
            {
                AutoSize = true,
                Width = 330,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 3, 0, 3)
            };

            Panel bubble = new Panel()
            {
                AutoSize = true,
                MaximumSize = new Size(240, 0),
                MinimumSize = new Size(85, 50),
                Padding = new Padding(16, 12, 16, 12),
                BackColor = Color.Transparent
            };

            bubble.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Different corner radius for sender/receiver
                int r = 18;
                using (var path = new GraphicsPath())
                {
                    if (isMe)
                    {
                        // Rounded except bottom-right
                        path.AddArc(0, 0, r * 2, r * 2, 180, 90);
                        path.AddArc(bubble.Width - r * 2, 0, r * 2, r * 2, 270, 90);
                        path.AddLine(bubble.Width, r, bubble.Width, bubble.Height - 6);
                        path.AddArc(bubble.Width - 12, bubble.Height - 12, 12, 12, 0, 90);
                        path.AddArc(0, bubble.Height - r * 2, r * 2, r * 2, 90, 90);
                        path.CloseFigure();

                        using (var brush = new LinearGradientBrush(
                            new Rectangle(0, 0, bubble.Width, bubble.Height),
                            Color.FromArgb(99, 102, 241),
                            Color.FromArgb(139, 92, 246), 135F))
                        {
                            e.Graphics.FillPath(brush, path);
                        }
                    }
                    else
                    {
                        // Rounded except bottom-left
                        path.AddArc(0, 0, r * 2, r * 2, 180, 90);
                        path.AddArc(bubble.Width - r * 2, 0, r * 2, r * 2, 270, 90);
                        path.AddArc(bubble.Width - r * 2, bubble.Height - r * 2, r * 2, r * 2, 0, 90);
                        path.AddArc(0, bubble.Height - 12, 12, 12, 90, 90);
                        path.AddLine(0, bubble.Height - 6, 0, r);
                        path.CloseFigure();

                        using (var brush = new SolidBrush(Color.White))
                        {
                            e.Graphics.FillPath(brush, path);
                        }
                        using (var pen = new Pen(Color.FromArgb(226, 232, 240), 1))
                        {
                            e.Graphics.DrawPath(pen, path);
                        }
                    }
                }
            };

            Label lbl = new Label()
            {
                Text = text,
                AutoSize = true,
                MaximumSize = new Size(205, 0),
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = isMe ? Color.White : Color.FromArgb(30, 41, 59),
                Location = new Point(16, 12),
                BackColor = Color.Transparent
            };
            bubble.Controls.Add(lbl);

            string timeText = time.ToString("HH:mm");
            if (!string.IsNullOrEmpty(status) && isMe)
            {
                timeText += "  ✓";
            }

            Label lblTime = new Label()
            {
                Text = timeText,
                AutoSize = true,
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = isMe ? Color.FromArgb(200, 210, 255) : Color.FromArgb(148, 163, 184),
                Location = new Point(16, lbl.Bottom + 5),
                BackColor = Color.Transparent
            };
            bubble.Controls.Add(lblTime);
            bubble.Height = lblTime.Bottom + 12;

            bubble.Location = isMe ? new Point(330 - bubble.Width - 8, 0) : new Point(8, 0);
            container.Controls.Add(bubble);
            container.Height = bubble.Height + 6;

            flowMessages.Controls.Add(container);
            flowMessages.ScrollControlIntoView(container);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            refreshTimer?.Stop();
            refreshTimer?.Dispose();
            base.OnFormClosed(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Add subtle shadow around form
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var pen = new Pen(Color.FromArgb(20, 0, 0, 0), 1))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            }
        }

        public class ComboBoxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
            public override string ToString() => Text;
        }
    }
}