using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class DashBoard : Form
    {
        // 🔗 Chuỗi kết nối SQL Server
        private readonly string connectionString = @"Data Source=DESKTOP-HE7MI7F\SQLEXPRESS;Initial Catalog=QL_CLB_LSC;Integrated Security=True;TrustServerCertificate=True";

        private readonly string role;
        private readonly string username;
        private readonly int maTV;
        private bool slidebarExpanded = true;
        private ChatbotPanel2 chatPanel;

        // ✅ Constructor chính - bắt buộc các tham số
        public DashBoard(string role, string username, int maTV)
        {
            InitializeComponent();

            // Validate input
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role không được rỗng", nameof(role));
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username không được rỗng", nameof(username));
            if (maTV <= 0)
                throw new ArgumentException("MaTV phải lớn hơn 0", nameof(maTV));

            this.role = role;
            this.username = username;
            this.maTV = maTV;

            this.Load += DashBoard_Load;
            this.FormClosing += DashBoard_FormClosing;
        }

        // 🔵 Constructor mặc định (chỉ dùng cho Designer)
        public DashBoard()
        {
            InitializeComponent();

            // Giá trị mặc định cho test
            this.role = "Test";
            this.username = "TestUser";
            this.maTV = 1;

            this.Load += DashBoard_Load;
            this.FormClosing += DashBoard_FormClosing;
        }

        // ================================
        // 🎬 FORM LOAD
        // ================================
        private void DashBoard_Load(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị thông tin user trước
                DisplayUserInfo();

                // Load dữ liệu
                LoadStats();
                LoadTimeline();

                // Khởi tạo chatbot
                InitializeChatbot();
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show(
                    $"Lỗi kết nối cơ sở dữ liệu:\n{sqlEx.Message}",
                    "Lỗi Database",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi tải Dashboard:\n{ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ================================
        // 👤 HIỂN THỊ THÔNG TIN USER
        // ================================
        private void DisplayUserInfo()
        {
            if (lblUsername != null)
                lblUsername.Text = $"Người dùng: {username}";

            if (lblRole != null)
                lblRole.Text = $"Vai trò: {role}";
        }

        // ================================
        // 💬 KHỞI TẠO CHATBOT
        // ================================
        private void InitializeChatbot()
        {
            try
            {
                chatPanel = new ChatbotPanel2(connectionString, maTV, username);

                // Đặt vị trí góc dưới phải
                PositionChatbot();

                this.Controls.Add(chatPanel);
                chatPanel.BringToFront();

                // Cập nhật vị trí khi resize form
                this.Resize += DashBoard_Resize;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Không thể khởi tạo chatbot:\n{ex.Message}",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void DashBoard_Resize(object sender, EventArgs e)
        {
            if (chatPanel != null && !chatPanel.IsDisposed)
            {
                PositionChatbot();
                chatPanel.BringToFront();
            }
        }

        private void PositionChatbot()
        {
            if (chatPanel != null && !chatPanel.IsDisposed)
            {
                const int margin = 20;
                chatPanel.Location = new Point(
                    this.ClientSize.Width - chatPanel.Width - margin,
                    this.ClientSize.Height - chatPanel.Height - margin
                );
            }
        }

        // ================================
        // 📊 LOAD SỐ LIỆU THỐNG KÊ
        // ================================
        private void LoadStats()
        {
            // Cập nhật các label với null check
            if (lblMemberCount != null)
                lblMemberCount.Text = GetCount("SELECT COUNT(*) FROM ThanhVien").ToString();

            if (lblProjectCount != null)
                lblProjectCount.Text = GetCount("SELECT COUNT(*) FROM DuAn").ToString();

            if (lblPostCount != null)
                lblPostCount.Text = GetCount("SELECT COUNT(*) FROM ThongBao").ToString();

            if (lblEventCount != null)
                lblEventCount.Text = GetCount("SELECT COUNT(*) FROM HoatDong WHERE NgayToChuc >= CAST(GETDATE() AS DATE)").ToString();
        }

        private int GetCount(string query)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        return result != null && result != DBNull.Value
                            ? Convert.ToInt32(result)
                            : 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show(
                    $"Lỗi SQL khi lấy thống kê:\n{sqlEx.Message}",
                    "Lỗi Database",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi lấy thống kê:\n{ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return 0;
            }
        }

        // ================================
        // 📰 LOAD TIMELINE
        // ================================
        private void LoadTimeline()
        {
            if (flowTimeline == null) return;

            flowTimeline.SuspendLayout(); // Tạm dừng vẽ lại UI
            flowTimeline.Controls.Clear();

            string query = @"
                SELECT TOP 20
                    TieuDe AS Title,
                    NoiDung AS Content,
                    NgayDang AS DateEvent,
                    DoiTuong AS Recipient,
                    'ThongBao' AS TypeEvent
                FROM ThongBao
                
                UNION ALL
                
                SELECT TOP 20
                    N'Khen thưởng: ' + TV.HoTen AS Title,
                    LyDo AS Content,
                    NgayKT AS DateEvent,
                    TV.HoTen AS Recipient,
                    'KhenThuong' AS TypeEvent
                FROM KhenThuong KT
                INNER JOIN ThanhVien TV ON KT.MaTV = TV.MaTV
                
                ORDER BY DateEvent DESC";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string title = reader["Title"]?.ToString() ?? "Không có tiêu đề";
                            string content = reader["Content"]?.ToString() ?? "";
                            DateTime date = reader["DateEvent"] != DBNull.Value
                                ? Convert.ToDateTime(reader["DateEvent"])
                                : DateTime.Now;
                            string recipient = reader["Recipient"]?.ToString() ?? "Tất cả";
                            string typeEvent = reader["TypeEvent"]?.ToString() ?? "ThongBao";

                            AddTimelineItem(date, title, content, recipient, typeEvent);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show(
                    $"Lỗi SQL khi tải timeline:\n{sqlEx.Message}",
                    "Lỗi Database",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi tải timeline:\n{ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                flowTimeline.ResumeLayout(); // Tiếp tục vẽ lại UI
            }
        }

        private void AddTimelineItem(DateTime date, string title, string content, string recipient, string typeEvent)
        {
            // Tạo panel chính
            Panel postPanel = new Panel
            {
                Width = flowTimeline.Width - 40,
                AutoSize = true,
                MinimumSize = new Size(flowTimeline.Width - 40, 120),
                BackColor = typeEvent == "KhenThuong"
                    ? Color.FromArgb(255, 250, 205)
                    : Color.FromArgb(240, 248, 255),
                Margin = new Padding(5),
                Padding = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Tiêu đề
            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(0, 0, 0, 5)
            };

            // Ngày
            Label lblDate = new Label
            {
                Text = date.ToString("dd/MM/yyyy HH:mm"),
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.Gray,
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(0, 0, 0, 3)
            };

            // Người nhận
            Label lblRecipient = new Label
            {
                Text = $"🎯 {recipient}",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.DarkBlue,
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(0, 0, 0, 5)
            };

            // Nội dung
            Label lblContent = new Label
            {
                Text = content.Length > 150 ? content.Substring(0, 150) + "..." : content,
                Font = new Font("Segoe UI", 9),
                AutoSize = true,
                MaximumSize = new Size(postPanel.Width - 20, 0),
                Dock = DockStyle.Top
            };

            // Thêm controls theo thứ tự (từ dưới lên trên do Dock.Top)
            postPanel.Controls.Add(lblContent);
            postPanel.Controls.Add(lblRecipient);
            postPanel.Controls.Add(lblDate);
            postPanel.Controls.Add(lblTitle);

            flowTimeline.Controls.Add(postPanel);
        }

        // ================================
        // 🎞️ SLIDEBAR ANIMATION
        // ================================
        private void slidebarTransition_Tick(object sender, EventArgs e)
        {
            const int minWidth = 57;
            const int maxWidth = 206;
            const int step = 10;

            if (slidebarExpanded)
            {
                slidebar.Width -= step;
                if (slidebar.Width <= minWidth)
                {
                    slidebar.Width = minWidth;
                    slidebarExpanded = false;
                    slidebarTransition.Stop();
                }
            }
            else
            {
                slidebar.Width += step;
                if (slidebar.Width >= maxWidth)
                {
                    slidebar.Width = maxWidth;
                    slidebarExpanded = true;
                    slidebarTransition.Stop();
                }
            }
        }

        private void btnham_Click(object sender, EventArgs e)
        {
            if (!slidebarTransition.Enabled)
                slidebarTransition.Start();
        }

        // ================================
        // 🚪 ĐĂNG XUẤT
        // ================================
        private void btnDangxuat_Click(object sender, EventArgs e)
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

        // ================================
        // 🧹 CLEANUP
        // ================================
        private void DashBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Dọn dẹp chatbot
                if (chatPanel != null && !chatPanel.IsDisposed)
                {
                    chatPanel.Dispose();
                }

                // Hủy đăng ký sự kiện
                this.Resize -= DashBoard_Resize;
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không hiển thị cho user khi đóng form
                System.Diagnostics.Debug.WriteLine($"Error during cleanup: {ex.Message}");
            }
        }
    }
}