using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace ClubManageApp
{
    public partial class DashBoard : Form
    {
        // currently selected sidebar button
        private Guna2Button selectedMenuButton;

        private readonly Color menuDefaultFill = Color.Transparent;
        private readonly Color menuDefaultFore = Color.Black;
        private readonly Color menuSelectedFill = Color.FromArgb(94, 148, 255);
        private readonly Color menuSelectedFore = Color.White;

        // mark given button as selected and reset others
        private void SelectMenuButton(Guna2Button btn)
        {
            // list of menu buttons to manage
            var buttons = new Guna2Button[] {
                btnDashBoard, btnTaiKhoan, btnHoatDong, btnThongbao,
                btnTaiChinh, btnDuAn, btnLichHop, btnThanhVien
            };

            foreach (var b in buttons)
            {
                if (b == null) continue;
                try
                {
                    // Restore base appearance for all buttons
                    b.FillColor = menuDefaultFill;
                    b.ForeColor = menuDefaultFore;
                    b.Font = new Font(b.Font.FontFamily, b.Font.Size, FontStyle.Bold);

                    // Reset border / shadow to neutral
                    try { b.BorderColor = Color.Transparent; } catch { }
                    try { b.CustomBorderColor = Color.Transparent; } catch { }
                    try { b.BorderThickness = 0; } catch { }
                    try { if (b.ShadowDecoration != null) b.ShadowDecoration.Enabled = false; } catch { }

                    // Reset hover and checked states
                    try { b.HoverState.FillColor = menuDefaultFill; } catch { }
                    try { b.HoverState.ForeColor = menuDefaultFore; } catch { }
                    try { b.CheckedState.FillColor = menuDefaultFill; } catch { }
                    try { b.CheckedState.ForeColor = menuDefaultFore; } catch { }
                }
                catch
                {
                    // ignore issues restoring a specific button
                }
            }

            if (btn != null)
            {
                try
                {
                    // Make selected button visually prominent (full fill + white text + subtle border + shadow)
                    btn.FillColor = menuSelectedFill;
                    btn.ForeColor = menuSelectedFore;
                    btn.Font = new Font(btn.Font.FontFamily, btn.Font.Size, FontStyle.Bold);

                    try { btn.BorderColor = Color.FromArgb(60, 100, 200); } catch { }
                    try { btn.CustomBorderColor = Color.FromArgb(60, 100, 200); } catch { }
                    try { btn.BorderThickness = 1; } catch { }
                    try { if (btn.ShadowDecoration != null) { btn.ShadowDecoration.Enabled = true; } } catch { }

                    // Make hover/checked states consistent with selected
                    try { btn.HoverState.FillColor = menuSelectedFill; } catch { }
                    try { btn.HoverState.ForeColor = menuSelectedFore; } catch { }
                    try { btn.CheckedState.FillColor = menuSelectedFill; } catch { }
                    try { btn.CheckedState.ForeColor = menuSelectedFore; } catch { }

                    selectedMenuButton = btn;
                }
                catch
                {
                    // ignore selection errors
                }
            }
            else
            {
                selectedMenuButton = null;
            }
        }

        // 🔗 Chuỗi kết nối SQL Server
        private string connectionString = @"Data Source=DESKTOP-B7F3HIU;Initial Catalog=QL_APP_LSC;Integrated Security=True;TrustServerCertificate=True";
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


        // Helper: show placeholder content in contentPanel for modules that are not implemented yet
        private void ShowModulePlaceholder(string moduleName)
        {
            if (this.contentPanel == null) return;
            try
            {
                // Hide top dashboard panels/labels when showing a module placeholder
                if (this.panelStats != null) this.panelStats.Visible = false;
                if (this.lblTimeline != null) this.lblTimeline.Visible = false;
                if (this.flowTimeline != null) this.flowTimeline.Visible = false;

                this.contentPanel.Controls.Clear();

                var lbl = new Label()
                {
                    Text = moduleName + " (coming soon)",
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    Dock = DockStyle.Top,
                    Height = 48,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                var info = new Label()
                {
                    Text = "Module '" + moduleName + "' will be implemented here.\nYou can replace this placeholder with the corresponding UserControl or Form.",
                    Font = new Font("Segoe UI", 10F),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                this.contentPanel.Controls.Add(info);
                this.contentPanel.Controls.Add(lbl);
            }
            catch
            {
                // ignore
            }
        }


        // ================================
        // Khôi làm sidebar từ đây
        // ================================
        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            SelectMenuButton(btnDashBoard);
             // Restore dashboard main content
             try
             {
                // Make sure the top stats and timeline label are visible
                if (this.panelStats != null) this.panelStats.Visible = true;
                if (this.lblTimeline != null) this.lblTimeline.Visible = true;

                // Clear contentPanel and ensure the flowTimeline control is present and visible
                contentPanel.Controls.Clear();

                if (this.flowTimeline != null)
                {
                    flowTimeline.Dock = DockStyle.Fill;
                    flowTimeline.Visible = true;
                    contentPanel.Controls.Add(flowTimeline);
                }

                LoadStats();
                LoadTimeline();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở Dashboard: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            SelectMenuButton(btnTaiKhoan);
             try
             {
                if (this.contentPanel == null) return;

                // Hide dashboard top stats/timeline when switching to account
                if (this.panelStats != null) this.panelStats.Visible = false;
                if (this.lblTimeline != null) this.lblTimeline.Visible = false;
                if (this.flowTimeline != null) this.flowTimeline.Visible = false;

                var acct = new ucAccount();
                acct.Dock = DockStyle.Fill;

                this.contentPanel.Controls.Clear();
                this.contentPanel.Controls.Add(acct);
                acct.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lõi khi mở Tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnHoatDong_Click(object sender, EventArgs e)
        {
            SelectMenuButton(btnHoatDong);
            try
            {
                if (this.contentPanel == null) return;

                // Hide dashboard top stats/timeline when switching to activity
                if (this.panelStats != null) this.panelStats.Visible = false;
                if (this.lblTimeline != null) this.lblTimeline.Visible = false;
                if (this.flowTimeline != null) this.flowTimeline.Visible = false;

                var activity = new Activity();
                activity.Dock = DockStyle.Fill;

                this.contentPanel.Controls.Clear();
                this.contentPanel.Controls.Add(activity);
                activity.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở Hoạt động: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
 
         private void btnThongbao_Click(object sender, EventArgs e)
         {
            SelectMenuButton(btnThongbao);
            try
            {
                if (this.contentPanel == null) return;

                // Hide dashboard top stats/timeline when switching to notifications
                if (this.panelStats != null) this.panelStats.Visible = false;
                if (this.lblTimeline != null) this.lblTimeline.Visible = false;
                if (this.flowTimeline != null) this.flowTimeline.Visible = false;

                var notificationControl = new Notification();

                // Make sure it fills the available content area precisely
                notificationControl.Dock = DockStyle.Fill;
                notificationControl.Margin = new Padding(0);
                notificationControl.Location = new Point(0, 0);
                // Do not set Size or Anchor manually; let Dock=Fill handle sizing

                // Remove padding inside contentPanel so child can occupy full area
                try { this.contentPanel.Padding = new Padding(0); } catch { }
                try { this.contentPanel.Margin = new Padding(0); } catch { }

                this.contentPanel.Controls.Clear();
                this.contentPanel.Controls.Add(notificationControl);
                notificationControl.BringToFront();

                // Ensure it resizes when contentPanel changes
                this.contentPanel.Resize -= ContentPanel_Resize_AdjustChild;
                this.contentPanel.Resize += ContentPanel_Resize_AdjustChild;

                // Force layout refresh
                this.contentPanel.PerformLayout();
                this.contentPanel.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở Thông báo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
         }

         private void ContentPanel_Resize_AdjustChild(object sender, EventArgs e)
         {
             try
             {
                 if (this.contentPanel == null) return;
                 if (this.contentPanel.Controls.Count == 0) return;
                 var ctl = this.contentPanel.Controls[0];
                 ctl.Location = new Point(0, 0);
                 ctl.Size = this.contentPanel.ClientSize;
             }
             catch { }
         }
 
         private void btnTaiChinh_Click(object sender, EventArgs e)
         {
            SelectMenuButton(btnTaiChinh);
             try
             {
                if (this.contentPanel == null) return;

                // Hide dashboard top stats/timeline when switching to finance
                if (this.panelStats != null) this.panelStats.Visible = false;
                if (this.lblTimeline != null) this.lblTimeline.Visible = false;
                if (this.flowTimeline != null) this.flowTimeline.Visible = false;

                var finance = new ucFinance();
                finance.Dock = DockStyle.Fill;

                this.contentPanel.Controls.Clear();
                this.contentPanel.Controls.Add(finance);
                finance.BringToFront();
             }
             catch (Exception ex)
             {
                 MessageBox.Show("Lỗi khi mở Tài Chính: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
         }
 
         private void btnDuAn_Click(object sender, EventArgs e)
         {
            SelectMenuButton(btnDuAn);
             try
             {
                if (this.contentPanel == null) return;

                // Hide dashboard top stats/timeline when switching to projects
                if (this.panelStats != null) this.panelStats.Visible = false;
                if (this.lblTimeline != null) this.lblTimeline.Visible = false;
                if (this.flowTimeline != null) this.flowTimeline.Visible = false;

                var proj = new ucProject();
                proj.Dock = DockStyle.Fill;

                this.contentPanel.Controls.Clear();
                this.contentPanel.Controls.Add(proj);
                proj.BringToFront();
             }
             catch (Exception ex)
             {
                 MessageBox.Show("Lỗi khi mở Dự án: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
         }
 
         private void btnLichHop_Click(object sender, EventArgs e)
         {
            SelectMenuButton(btnLichHop);
            try
            {
                if (this.contentPanel == null) return;

                // Hide dashboard top stats/timeline when switching to schedule
                if (this.panelStats != null) this.panelStats.Visible = false;
                if (this.lblTimeline != null) this.lblTimeline.Visible = false;
                if (this.flowTimeline != null) this.flowTimeline.Visible = false;

                var schedule = new ucSchedule();
                schedule.Dock = DockStyle.Fill;

                this.contentPanel.Controls.Clear();
                this.contentPanel.Controls.Add(schedule);
                schedule.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở Lịch họp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

         private void btnThanhVien_Click(object sender, EventArgs e)
         {
            SelectMenuButton(btnThanhVien);
            try
            {
                if (this.contentPanel == null) return;

                // Hide dashboard top stats/timeline when switching to members
                if (this.panelStats != null) this.panelStats.Visible = false;
                if (this.lblTimeline != null) this.lblTimeline.Visible = false;
                if (this.flowTimeline != null) this.flowTimeline.Visible = false;

                var userCtrl = new ucUser();
                userCtrl.Dock = DockStyle.Fill;

                this.contentPanel.Controls.Clear();
                this.contentPanel.Controls.Add(userCtrl);
                userCtrl.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở Thành viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                // reset selection on logout
                SelectMenuButton(null);
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

        private void flowTimeline_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}