using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ClubManageApp
{
    public class MemberChatPanel : UserControl
    {
        private string connectionString;
        private int currentUserMaTV;
        private string currentUsername;
        private int selectedMemberMaTV = 0;
        private string selectedMemberName = "";

        // UI Components
        private Panel headerPanel;
        private Label lblMemberName;
        private Label lblStatus;
        private Button btnClose;
        private TabControl tabControl;
        private TabPage tabMembers;
        private TabPage tabChat;
        private FlowLayoutPanel memberListFlow;
        private FlowLayoutPanel chatFlow;
        private TextBox txtMessage;
        private Button btnSend;
        private Timer refreshTimer;

        public MemberChatPanel(string connString, int maTV, string username)
        {
            this.connectionString = connString;
            this.currentUserMaTV = maTV;
            this.currentUsername = username;
            InitializeComponents();
            LoadMembers();
            StartAutoRefresh();
        }

        private void InitializeComponents()
        {
            this.Size = new Size(450, 632);
            this.BackColor = Color.White;

            // ========== HEADER ==========
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(44, 62, 80),
                Padding = new Padding(15)
            };

            // Icon và tên
            Label lblIcon = new Label
            {
                Text = "👤",
                Font = new Font("Segoe UI", 16),
                ForeColor = Color.White,
                Location = new Point(15, 10),
                AutoSize = true
            };

            lblMemberName = new Label
            {
                Text = "Thành viên CLB",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 12),
                AutoSize = true
            };

            lblStatus = new Label
            {
                Text = "● Trực tuyến",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(46, 204, 113),
                Location = new Point(50, 40),
                AutoSize = true
            };

            btnClose = new Button
            {
                Text = "✕",
                Size = new Size(35, 35),
                Location = new Point(395, 10),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Visible = false;

            headerPanel.Controls.Add(lblIcon);
            headerPanel.Controls.Add(lblMemberName);
            headerPanel.Controls.Add(lblStatus);
            headerPanel.Controls.Add(btnClose);
            this.Controls.Add(headerPanel);

            // ========== TAB CONTROL ==========
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10)
            };

            // Tab 1: Danh sách thành viên
            tabMembers = new TabPage
            {
                Text = "📋 Danh sách",
                BackColor = Color.FromArgb(236, 240, 241)
            };

            memberListFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(236, 240, 241)
            };

            tabMembers.Controls.Add(memberListFlow);

            // Tab 2: Chat
            tabChat = new TabPage
            {
                Text = "💬 Chat",
                BackColor = Color.White
            };

            Label lblChatPrompt = new Label
            {
                Text = "🔹 Bắt đầu chat với thành viên!",
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.Gray,
                BackColor = Color.FromArgb(250, 250, 250)
            };

            chatFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            // Input Panel
            Panel inputPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(10)
            };

            txtMessage = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11),
                Multiline = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtMessage.KeyDown += TxtMessage_KeyDown;

            btnSend = new Button
            {
                Text = "▶",
                Dock = DockStyle.Right,
                Width = 50,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Click += BtnSend_Click;

            inputPanel.Controls.Add(txtMessage);
            inputPanel.Controls.Add(btnSend);

            tabChat.Controls.Add(chatFlow);
            tabChat.Controls.Add(inputPanel);
            tabChat.Controls.Add(lblChatPrompt);

            tabControl.TabPages.Add(tabMembers);
            tabControl.TabPages.Add(tabChat);
            this.Controls.Add(tabControl);
        }

        // ========== LOAD DANH SÁCH THÀNH VIÊN ==========
        private void LoadMembers()
        {
            memberListFlow.Controls.Clear();

            string query = @"
                SELECT MaTV, HoTen, Email, SDT
                FROM ThanhVien 
                WHERE MaTV != @CurrentMaTV
                ORDER BY HoTen";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CurrentMaTV", currentUserMaTV);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int maTV = Convert.ToInt32(reader["MaTV"]);
                            string hoTen = reader["HoTen"].ToString();
                            string email = reader["Email"].ToString();
                            string sdt = reader["SDT"]?.ToString() ?? "";

                            AddMemberCard(maTV, hoTen, email, sdt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách thành viên:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddMemberCard(int maTV, string hoTen, string email, string sdt)
        {
            Panel card = new Panel
            {
                Width = memberListFlow.Width - 30,
                Height = 80,
                BackColor = Color.White,
                Margin = new Padding(5),
                Padding = new Padding(10),
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblName = new Label
            {
                Text = "👤 " + hoTen,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(10, 10),
                AutoSize = true
            };

            Label lblEmail = new Label
            {
                Text = "📧 " + email,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(10, 35),
                AutoSize = true
            };

            Label lblPhone = new Label
            {
                Text = "📱 " + (string.IsNullOrEmpty(sdt) ? "Chưa cập nhật" : sdt),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(10, 55),
                AutoSize = true
            };

            card.Controls.Add(lblName);
            card.Controls.Add(lblEmail);
            card.Controls.Add(lblPhone);

            // Hover effect
            card.MouseEnter += (s, e) => {
                card.BackColor = Color.FromArgb(240, 248, 255);
            };
            card.MouseLeave += (s, e) => {
                card.BackColor = Color.White;
            };

            // Click to chat
            card.Click += (s, e) => {
                SelectMember(maTV, hoTen);
            };

            memberListFlow.Controls.Add(card);
        }

        private void SelectMember(int maTV, string hoTen)
        {
            selectedMemberMaTV = maTV;
            selectedMemberName = hoTen;
            lblMemberName.Text = hoTen;
            lblStatus.Text = "● Trực tuyến";

            tabControl.SelectedTab = tabChat;
            LoadMessages();
        }

        // ========== GỬI TIN NHẮN ==========
        private void BtnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void TxtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                e.SuppressKeyPress = true;
                SendMessage();
            }
        }

        private void SendMessage()
        {
            if (selectedMemberMaTV == 0)
            {
                MessageBox.Show("Vui lòng chọn thành viên từ danh sách!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tabControl.SelectedTab = tabMembers;
                return;
            }

            string message = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;

            string query = @"
                INSERT INTO TinNhan (NguoiGui, NguoiNhan, NoiDung, ThoiGian, DaDoc)
                VALUES (@NguoiGui, @NguoiNhan, @NoiDung, GETDATE(), 0)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NguoiGui", currentUserMaTV);
                        cmd.Parameters.AddWithValue("@NguoiNhan", selectedMemberMaTV);
                        cmd.Parameters.AddWithValue("@NoiDung", message);
                        cmd.ExecuteNonQuery();
                    }
                }

                txtMessage.Clear();
                LoadMessages();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gửi tin nhắn:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== TẢI TIN NHẮN ==========
        private void LoadMessages()
        {
            if (selectedMemberMaTV == 0) return;

            chatFlow.Controls.Clear();

            string query = @"
                SELECT T.NoiDung, T.ThoiGian, T.NguoiGui, TV.HoTen
                FROM TinNhan T
                JOIN ThanhVien TV ON T.NguoiGui = TV.MaTV
                WHERE (T.NguoiGui = @CurrentUser AND T.NguoiNhan = @SelectedMember)
                   OR (T.NguoiGui = @SelectedMember AND T.NguoiNhan = @CurrentUser)
                ORDER BY T.ThoiGian ASC";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CurrentUser", currentUserMaTV);
                        cmd.Parameters.AddWithValue("@SelectedMember", selectedMemberMaTV);

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string content = reader["NoiDung"].ToString();
                            DateTime time = Convert.ToDateTime(reader["ThoiGian"]);
                            int nguoiGui = Convert.ToInt32(reader["NguoiGui"]);
                            string tenNguoiGui = reader["HoTen"].ToString();

                            bool isSentByMe = (nguoiGui == currentUserMaTV);
                            AddMessageBubble(content, time, tenNguoiGui, isSentByMe);
                        }
                    }
                }

                // Cuộn xuống tin nhắn mới nhất
                if (chatFlow.Controls.Count > 0)
                {
                    chatFlow.ScrollControlIntoView(chatFlow.Controls[chatFlow.Controls.Count - 1]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải tin nhắn:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddMessageBubble(string message, DateTime time, string sender, bool isSentByMe)
        {
            Panel bubblePanel = new Panel
            {
                Width = chatFlow.Width - 30,
                AutoSize = true,
                Padding = new Padding(5),
                Margin = new Padding(5, 2, 5, 2)
            };

            Label lblMessage = new Label
            {
                Text = message,
                AutoSize = true,
                MaximumSize = new Size(300, 0),
                Padding = new Padding(12),
                Font = new Font("Segoe UI", 10),
                BackColor = isSentByMe ? Color.FromArgb(52, 152, 219) : Color.FromArgb(236, 240, 241),
                ForeColor = isSentByMe ? Color.White : Color.Black
            };

            Label lblTime = new Label
            {
                Text = time.ToString("HH:mm dd/MM"),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.Gray,
                Padding = new Padding(12, 2, 12, 2)
            };

            if (isSentByMe)
            {
                lblMessage.Dock = DockStyle.Right;
                lblTime.Dock = DockStyle.Right;
            }
            else
            {
                Label lblSender = new Label
                {
                    Text = sender,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8, FontStyle.Bold),
                    ForeColor = Color.FromArgb(52, 152, 219),
                    Dock = DockStyle.Left,
                    Padding = new Padding(12, 2, 12, 2)
                };
                bubblePanel.Controls.Add(lblSender);

                lblMessage.Dock = DockStyle.Left;
                lblTime.Dock = DockStyle.Left;
            }

            bubblePanel.Controls.Add(lblMessage);
            bubblePanel.Controls.Add(lblTime);
            chatFlow.Controls.Add(bubblePanel);
        }

        // ========== TỰ ĐỘNG LÀM MỚI ==========
        private void StartAutoRefresh()
        {
            refreshTimer = new Timer
            {
                Interval = 3000 // 3 giây
            };
            refreshTimer.Tick += (s, e) =>
            {
                if (tabControl.SelectedTab == tabChat && selectedMemberMaTV > 0)
                {
                    LoadMessages();
                }
            };
            refreshTimer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                refreshTimer?.Stop();
                refreshTimer?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MemberChatPanel
            // 
            this.Name = "MemberChatPanel";
            this.Load += new System.EventHandler(this.MemberChatPanel_Load);
            this.ResumeLayout(false);

        }

        private void MemberChatPanel_Load(object sender, EventArgs e)
        {

        }
    }
}