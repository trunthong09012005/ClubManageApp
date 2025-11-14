using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class DashBoard : Form
    {
        // 🔗 Chuỗi kết nối SQL Server
        private string connectionString =
             @"Data Source=21AK22-COM;Initial Catalog=QL_CLB_LSC;User ID=sa;Password=912005;TrustServerCertificate=True";


        private string role;
        private string username;
        private int maTV;
        private bool slidebarExpanded = true;

        // 🟢 Constructor nhận thông tin đăng nhập
        public DashBoard(string role, string username, int maTV)
        {
            InitializeComponent();
            this.role = role;
            this.username = username;
            this.maTV = maTV;
            this.Load += DashBoard_Load;
        }


        // 🔵 Constructor mặc định (khi test form)
        public DashBoard()
        {
            InitializeComponent();
            this.Load += DashBoard_Load;
        }

        // ================================
        // 🎬 FORM LOAD
        // ================================
        private void DashBoard_Load(object sender, EventArgs e)
        {
            try
            {
                LoadStats();
                LoadTimeline();
                // Hiển thị username và role
                lblUsername.Text = "Người dùng: " + username;
                lblRole.Text = "Vai trò: " + role;

                LoadStats();
                LoadTimeline();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Dashboard:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================================
        // 📊 LOAD SỐ LIỆU THỐNG KÊ
        // ================================
        private void LoadStats()
        {
            // Số thành viên
            lblMemberCount.Text = GetCount("SELECT COUNT(*) FROM ThanhVien").ToString();

            // Số dự án
            lblProjectCount.Text = GetCount("SELECT COUNT(*) FROM DuAn").ToString();

            // Số thông báo (không cần TrangThai nữa)
            lblPostCount.Text = GetCount("SELECT COUNT(*) FROM ThongBao").ToString();

            // Số sự kiện (Hoạt động có ngày >= hiện tại)
            lblEventCount.Text = GetCount("SELECT COUNT(*) FROM HoatDong WHERE NgayToChuc >= GETDATE()").ToString();
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
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy dữ liệu thống kê:\n" + ex.Message,
                    "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        // ================================
        // 📰 LOAD BẢNG TIN / TIMELINE
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


        // ================================
        // 🎞️ SLIDEBAR ANIMATION
        // ================================
        private void slidebarTransition_Tick(object sender, EventArgs e)
        {
            if (slidebarExpanded)
            {
                slidebar.Width -= 10;
                if (slidebar.Width <= 57)
                {
                    slidebarExpanded = false;
                    slidebarTransition.Stop();
                }
            }
            else
            {
                slidebar.Width += 10;
                if (slidebar.Width >= 206)
                {
                    slidebarExpanded = true;
                    slidebarTransition.Stop();
                }
            }
        }

        private void btnham_Click(object sender, EventArgs e)
        {
            slidebarTransition.Start();
        }

        private void btnDangxuat_Click(object sender, EventArgs e)
        {
            slidebarTransition.Start();
            // Ẩn form đăng ký
            this.Hide();

            // Mở form đăng nhập
            Login loginForm = new Login();

            // Khi form đăng nhập đóng, đóng luôn form đăng ký (để không bị vòng lặp)
            loginForm.FormClosed += (s, args) => this.Close();

            loginForm.Show();
        }


    }
}
