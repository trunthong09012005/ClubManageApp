using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class MemberDashboard : Form
    {
        private string role;
        private string username;
        private int maTV;

        // 🔗 Chuỗi kết nối SQL Server
        private string connectionString =
            @"Data Source=21AK22-COM;Initial Catalog=QL_CLB_LSC;User ID=sa;Password=912005;TrustServerCertificate=True";

        // Biến cho animation sidebar
        bool sidebarExpand = true;
        private const int SIDEBAR_MAX = 206;
        private const int SIDEBAR_MIN = 60;

        public MemberDashboard(string role, string username, int maTV)
        {
            InitializeComponent();
            this.role = role;
            this.username = username;
            this.maTV = maTV;

            // Đăng ký sự kiện
            this.Load += MemberDashboard_Load;
            btnham.Click += btnham_Click;
            slidebarTransition.Tick += slidebarTransition_Tick;

            // Đăng ký sự kiện cho các nút menu
            RegisterMenuEvents();
        }

        private void MemberDashboard_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin người dùng
            lblUsername.Text = "Người dùng: " + username;
            lblRole.Text = "Vai trò: " + role;

            // Load dữ liệu dashboard
            LoadDashboardData();

            // Load danh sách hoạt động sắp tới
            LoadUpcomingActivities();

            // Highlight nút Dashboard
            HighlightButton(btnMemberDashBoard);
            LoadTimeline();
        }
        // ================================
        private void LoadTimeline()
        {
            flowTimeline.Controls.Clear();

            string query = @"
        -- Thông báo
        SELECT 
            TieuDe AS Title, 
            NoiDung AS Content, 
            NgayDang AS DateEvent,
            DoiTuong AS Recipient,
            'ThongBao' AS TypeEvent
        FROM ThongBao

        UNION ALL

        -- Khen thưởng
        SELECT 
            N'Khen thưởng' AS Title,
            LyDo AS Content,
            NgayKT AS DateEvent,
            TV.HoTen AS Recipient,
            'KhenThuong' AS TypeEvent
        FROM KhenThuong KT
        JOIN ThanhVien TV ON KT.MaTV = TV.MaTV

        ORDER BY DateEvent DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string title = reader["Title"].ToString();
                    string content = reader["Content"].ToString();
                    DateTime date = Convert.ToDateTime(reader["DateEvent"]);
                    string recipient = reader["Recipient"].ToString();
                    string typeEvent = reader["TypeEvent"].ToString();

                    AddTimelineItem(date, title, content, recipient, typeEvent);
                }
            }
        }

        private void AddTimelineItem(DateTime date, string title, string content, string recipient, string typeEvent)
        {
            Panel postPanel = new Panel()
            {
                Width = flowTimeline.Width - 40,
                Height = 140,
                BackColor = Color.FromArgb(240, 248, 255),
                Margin = new Padding(5),
                Padding = new Padding(10)
            };

            Label lblTitle = new Label()
            {
                Text = title,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25
            };

            Label lblDate = new Label()
            {
                Text = date.ToString("dd/MM/yyyy HH:mm"),
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.Gray,
                Dock = DockStyle.Top
            };

            Label lblRecipient = new Label()
            {
                Text = "Người nhận: " + recipient,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.DarkBlue,
                Dock = DockStyle.Top,
                Height = 20
            };

            Label lblContent = new Label()
            {
                Text = content,
                Font = new Font("Segoe UI", 10),
                AutoSize = false,
                Dock = DockStyle.Fill
            };

            // Thêm thứ tự: content dưới cùng, recipient trên content, date trên recipient, title trên cùng
            postPanel.Controls.Add(lblContent);
            postPanel.Controls.Add(lblRecipient);
            postPanel.Controls.Add(lblDate);
            postPanel.Controls.Add(lblTitle);

            flowTimeline.Controls.Add(postPanel);
        }
        private void LoadDashboardData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 🟦 Hoạt động CLB mà thành viên đã tham gia (có điểm danh)
                    int hoatDongCount = GetCount(conn, @"
                        SELECT COUNT(DISTINCT dd.MaLH) 
                        FROM DiemDanhLichHop dd     
                        INNER JOIN LichHop lh ON dd.MaLH = lh.MaLH
                        WHERE dd.MaTV = @maTV AND dd.TrangThai = N'Có mặt'");
                    lblMemberCount.Text = hoatDongCount.ToString();

                    // 🟩 Dự án được giao (bảng PhanCong)
                    int duAnCount = GetCount(conn, @"
                        SELECT COUNT(*) 
                        FROM PhanCong 
                        WHERE MaTV = @maTV");
                    lblProjectCount.Text = duAnCount.ToString();

                    // 🟨 Điểm rèn luyện trung bình
                    int diemRL = GetCount(conn, @"
                        SELECT ISNULL(AVG(Diem), 0)
                        FROM DiemRenLuyen
                        WHERE MaTV = @maTV");
                    lblPostCount.Text = diemRL.ToString();

                    // 🟥 Khen thưởng
                    int khenThuongCount = GetCount(conn, @"
                        SELECT COUNT(*) 
                        FROM KhenThuong 
                        WHERE MaTV = @maTV");
                    lblEventCount.Text = khenThuongCount.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu bảng điều khiển: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetCount(SqlConnection conn, string query)
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@maTV", maTV);
                object result = cmd.ExecuteScalar();

                if (result == DBNull.Value || result == null)
                    return 0;

                if (int.TryParse(result.ToString(), out int count))
                    return count;

                if (float.TryParse(result.ToString(), out float avg))
                    return (int)Math.Round(avg);

                return 0;
            }
        }

        private void LoadUpcomingActivities()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT TOP 10
                            lh.TieuDe,
                            lh.NgayHop,
                            lh.DiaDiem,
                            lh.TrangThai,
                            CASE 
                                WHEN dd.TrangThai IS NULL THEN N'Chưa điểm danh'
                                ELSE dd.TrangThai
                            END AS TrangThaiDiemDanh
                        FROM LichHop lh
                        LEFT JOIN DiemDanhLichHop dd ON lh.MaLH = dd.MaLH AND dd.MaTV = @maTV
                        WHERE lh.NgayHop >= GETDATE()
                        ORDER BY lh.NgayHop ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTV", maTV);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Gán dữ liệu vào DataGridView
                        guna2DataGridView1.DataSource = dt;

                        // Định dạng cột
                        if (guna2DataGridView1.Columns.Count > 0)
                        {
                            guna2DataGridView1.Columns["TieuDe"].HeaderText = "Tiêu đề";
                            guna2DataGridView1.Columns["NgayHop"].HeaderText = "Ngày họp";
                            guna2DataGridView1.Columns["DiaDiem"].HeaderText = "Địa điểm";
                            guna2DataGridView1.Columns["TrangThai"].HeaderText = "Trạng thái";
                            guna2DataGridView1.Columns["TrangThaiDiemDanh"].HeaderText = "Điểm danh";

                            // Auto size columns
                            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

        #region Sidebar Animation

        private void btnham_Click(object sender, EventArgs e)
        {
            slidebarTransition.Start();
        }

        private void slidebarTransition_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                slidebar.Width -= 10;
                if (slidebar.Width <= SIDEBAR_MIN)
                {
                    sidebarExpand = false;
                    slidebarTransition.Stop();

                    // Ẩn text của các button khi sidebar thu nhỏ
                    HideButtonText();
                }
            }
            else
            {
                slidebar.Width += 10;
                if (slidebar.Width >= SIDEBAR_MAX)
                {
                    sidebarExpand = true;
                    slidebarTransition.Stop();

                    // Hiện lại text của các button
                    ShowButtonText();
                }
            }
        }

        private void HideButtonText()
        {
            btnMemberDashBoard.Text = "";
            btnHoso.Text = "";
            btnHoatdong.Text = "";
            btnThongbao.Text = "";
            btnTinNhan.Text = "";
            btnDuan.Text = "";
            btnLichhop.Text = "";
            btnDangXuat.Text = "";
        }

        private void ShowButtonText()
        {
            btnMemberDashBoard.Text = "   Dashboard";
            btnHoso.Text = "   Hồ sơ";
            btnHoatdong.Text = "   Hoạt động";
            btnThongbao.Text = "   Thông báo";
            btnTinNhan.Text = "   Tin nhắn";
            btnDuan.Text = "   Dự án";
            btnLichhop.Text = "   Lịch họp";
            btnDangXuat.Text = "   Đăng xuất";
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
                MessageBox.Show("Chức năng Hồ sơ đang được phát triển", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnHoatdong.Click += (s, e) => {
                HighlightButton(btnHoatdong);
                MessageBox.Show("Chức năng Hoạt động đang được phát triển", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnThongbao.Click += (s, e) => {
                HighlightButton(btnThongbao);
                MessageBox.Show("Chức năng Thông báo đang được phát triển", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnTinNhan.Click += (s, e) => {
                HighlightButton(btnTinNhan);
                MessageBox.Show("Chức năng Tin nhắn đang được phát triển", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnDuan.Click += (s, e) => {
                HighlightButton(btnDuan);
                MessageBox.Show("Chức năng Dự án đang được phát triển", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnLichhop.Click += (s, e) => {
                HighlightButton(btnLichhop);
                MessageBox.Show("Chức năng Lịch họp đang được phát triển", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnDangXuat.Click += BtnDangXuat_Click;
        }

        private void HighlightButton(Button selectedButton)
        {
            // Reset tất cả các button về màu mặc định
            Color defaultColor = Color.FromArgb(199, 217, 255);
            Color selectedColor = Color.FromArgb(150, 180, 255);

            btnMemberDashBoard.BackColor = defaultColor;
            btnHoso.BackColor = defaultColor;
            btnHoatdong.BackColor = defaultColor;
            btnThongbao.BackColor = defaultColor;
            btnTinNhan.BackColor = defaultColor;
            btnDuan.BackColor = defaultColor;
            btnLichhop.BackColor = defaultColor;

            // Highlight button được chọn
            selectedButton.BackColor = selectedColor;
        }

        private void ShowDashboard()
        {
            // Hiện lại panel stats và reload dữ liệu
            panelStats.Visible = true;
            flowTimeline.Visible = true;
            LoadDashboardData();
            LoadUpcomingActivities();
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
                // Đóng form hiện tại và mở form đăng nhập
                this.Hide();
                Login loginForm = new Login();
                loginForm.ShowDialog();
                this.Close();
            }
        }

        #endregion
    }
}