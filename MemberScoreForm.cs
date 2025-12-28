using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ClubManageApp
{
    public class MemberScoreForm : Form
    {
        private string connectionString;
        private int maTV;
        private string hoTen;

        // UI Components
        private Panel headerPanel;
        private Label lblTitle;
        private Label lblMemberInfo;
        private TabControl tabControl;
        private TabPage tabScore;
        private TabPage tabActivities;
        private DataGridView dgvScores;
        private DataGridView dgvActivities;
        private Panel summaryPanel;
        private Label lblTotalScore;
        private Label lblXepLoai;
        private Label lblSoHocKy;
        private Button btnSendReward;
        private Button btnClose;

        public MemberScoreForm(string connString, int memberMaTV, string memberName)
        {
            this.connectionString = connString;
            this.maTV = memberMaTV;
            this.hoTen = memberName;
            
            InitializeComponents();
            LoadData();
        }

        private void InitializeComponents()
        {
            this.Text = "Điểm rèn luyện và Hoạt động";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 247, 250);

            // ========== HEADER ==========
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = Color.FromArgb(52, 152, 219),
                Padding = new Padding(20)
            };

            lblTitle = new Label
            {
                Text = "📊 ĐIỂM RÈN LUYỆN & HOẠT ĐỘNG",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };

            lblMemberInfo = new Label
            {
                Text = $"👤 {hoTen} (Mã TV: {maTV})",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(236, 240, 241),
                Location = new Point(20, 50),
                AutoSize = true
            };

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(lblMemberInfo);
            this.Controls.Add(headerPanel);

            // ========== SUMMARY PANEL ==========
            summaryPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };

            lblTotalScore = new Label
            {
                Text = "Tổng điểm: N/A",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 204, 113),
                Location = new Point(20, 20),
                AutoSize = true
            };

            lblXepLoai = new Label
            {
                Text = "Xếp loại: N/A",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                Location = new Point(20, 50),
                AutoSize = true
            };

            lblSoHocKy = new Label
            {
                Text = "Số học kỳ: 0",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                Location = new Point(250, 25),
                AutoSize = true
            };

            btnSendReward = new Button
            {
                Text = "🎁 Gửi phần thưởng lên Timeline",
                Size = new Size(250, 50),
                Location = new Point(680, 20),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSendReward.FlatAppearance.BorderSize = 0;
            btnSendReward.Click += BtnSendReward_Click;

            summaryPanel.Controls.Add(lblTotalScore);
            summaryPanel.Controls.Add(lblXepLoai);
            summaryPanel.Controls.Add(lblSoHocKy);
            summaryPanel.Controls.Add(btnSendReward);
            this.Controls.Add(summaryPanel);

            // ========== TAB CONTROL ==========
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10)
            };

            // Tab 1: Điểm rèn luyện
            tabScore = new TabPage
            {
                Text = "📈 Điểm rèn luyện",
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            dgvScores = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10)
            };

            tabScore.Controls.Add(dgvScores);

            // Tab 2: Hoạt động tham gia
            tabActivities = new TabPage
            {
                Text = "🎯 Hoạt động tham gia",
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            dgvActivities = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10)
            };

            tabActivities.Controls.Add(dgvActivities);

            tabControl.TabPages.Add(tabScore);
            tabControl.TabPages.Add(tabActivities);
            this.Controls.Add(tabControl);

            // ========== CLOSE BUTTON ==========
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            btnClose = new Button
            {
                Text = "✖ Đóng",
                Dock = DockStyle.Right,
                Width = 120,
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            bottomPanel.Controls.Add(btnClose);
            this.Controls.Add(bottomPanel);
        }

        private void LoadData()
        {
            LoadScoreData();
            LoadActivityData();
        }

        private void LoadScoreData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Load điểm rèn luyện
                    string query = @"
                        SELECT 
                            HocKy,
                            NamHoc,
                            Diem,
                            XepLoai,
                            GhiChu,
                            NgayCapNhat
                        FROM DiemRenLuyen
                        WHERE MaTV = @MaTV
                        ORDER BY NamHoc DESC, HocKy DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTV", maTV);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvScores.DataSource = dt;

                        // Đổi tên cột
                        if (dgvScores.Columns.Contains("HocKy"))
                            dgvScores.Columns["HocKy"].HeaderText = "Học kỳ";
                        if (dgvScores.Columns.Contains("NamHoc"))
                            dgvScores.Columns["NamHoc"].HeaderText = "Năm học";
                        if (dgvScores.Columns.Contains("Diem"))
                            dgvScores.Columns["Diem"].HeaderText = "Điểm";
                        if (dgvScores.Columns.Contains("XepLoai"))
                            dgvScores.Columns["XepLoai"].HeaderText = "Xếp loại";
                        if (dgvScores.Columns.Contains("GhiChu"))
                            dgvScores.Columns["GhiChu"].HeaderText = "Ghi chú";
                        if (dgvScores.Columns.Contains("NgayCapNhat"))
                            dgvScores.Columns["NgayCapNhat"].HeaderText = "Ngày cập nhật";

                        // Tính tổng điểm và xếp loại
                        if (dt.Rows.Count > 0)
                        {
                            decimal totalScore = 0;
                            int count = 0;

                            foreach (DataRow row in dt.Rows)
                            {
                                if (row["Diem"] != DBNull.Value)
                                {
                                    totalScore += Convert.ToDecimal(row["Diem"]);
                                    count++;
                                }
                            }

                            decimal avgScore = count > 0 ? totalScore / count : 0;
                            lblTotalScore.Text = $"Điểm trung bình: {avgScore:F2}";
                            lblSoHocKy.Text = $"Số học kỳ: {count}";

                            // Xếp loại
                            string xepLoai = avgScore >= 90 ? "Xuất sắc" :
                                            avgScore >= 80 ? "Giỏi" :
                                            avgScore >= 65 ? "Khá" :
                                            avgScore >= 50 ? "Trung bình" : "Yếu";
                            lblXepLoai.Text = $"Xếp loại: {xepLoai}";

                            // Màu sắc theo xếp loại
                            Color scoreColor = avgScore >= 90 ? Color.FromArgb(46, 204, 113) :
                                             avgScore >= 80 ? Color.FromArgb(52, 152, 219) :
                                             avgScore >= 65 ? Color.FromArgb(241, 196, 15) :
                                             avgScore >= 50 ? Color.FromArgb(230, 126, 34) :
                                             Color.FromArgb(231, 76, 60);
                            lblTotalScore.ForeColor = scoreColor;
                        }
                        else
                        {
                            lblTotalScore.Text = "Chưa có điểm rèn luyện";
                            lblXepLoai.Text = "Xếp loại: N/A";
                            lblSoHocKy.Text = "Số học kỳ: 0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi tải điểm rèn luyện:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadActivityData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Load hoạt động tham gia
                    string query = @"
                        SELECT 
                            HD.TenHD AS [Tên hoạt động],
                            HD.NgayToChuc AS [Ngày tổ chức],
                            HD.DiaDiem AS [Địa điểm],
                            CASE WHEN TG.DiemDanh = 1 THEN N'✓ Có mặt' ELSE N'✗ Vắng' END AS [Điểm danh],
                            ISNULL(TG.DiemThuong, 0) AS [Điểm thưởng],
                            TG.GhiChu AS [Ghi chú],
                            HD.TrangThai AS [Trạng thái]
                        FROM ThamGia TG
                        INNER JOIN HoatDong HD ON TG.MaHD = HD.MaHD
                        WHERE TG.MaTV = @MaTV
                        ORDER BY HD.NgayToChuc DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTV", maTV);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvActivities.DataSource = dt;

                        // Tô màu điểm danh
                        dgvActivities.CellFormatting += (s, e) =>
                        {
                            if (dgvActivities.Columns[e.ColumnIndex].Name == "Điểm danh")
                            {
                                if (e.Value?.ToString().Contains("✓") == true)
                                {
                                    e.CellStyle.BackColor = Color.FromArgb(212, 239, 223);
                                    e.CellStyle.ForeColor = Color.FromArgb(22, 160, 133);
                                }
                                else if (e.Value?.ToString().Contains("✗") == true)
                                {
                                    e.CellStyle.BackColor = Color.FromArgb(248, 215, 218);
                                    e.CellStyle.ForeColor = Color.FromArgb(185, 28, 28);
                                }
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi tải hoạt động:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnSendReward_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy điểm trung bình từ label
                string scoreText = lblTotalScore.Text;
                string xepLoaiText = lblXepLoai.Text;

                if (scoreText == "Chưa có điểm rèn luyện")
                {
                    MessageBox.Show(
                        "Thành viên chưa có điểm rèn luyện để gửi phần thưởng!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Tạo nội dung khen thưởng
                string tieuDe = $"🎉 Khen thưởng thành viên xuất sắc - {hoTen}";
                string noiDung = $@"Chúc mừng {hoTen} (Mã TV: {maTV}) đã đạt thành tích xuất sắc!

📊 {scoreText}
🏆 {xepLoaiText}
📚 {lblSoHocKy.Text}

Chúc bạn tiếp tục phát huy và đạt nhiều thành tích cao hơn nữa!

Trân trọng,
Ban Quản lý CLB";

                // Thêm vào bảng Thông báo
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // ✅ SỬA: Đổi 'Khen thưởng' thành 'Sự kiện' (giá trị hợp lệ trong CHECK constraint)
                    string query = @"
                        INSERT INTO ThongBao (TieuDe, NoiDung, LoaiThongBao, NguoiDang, DoiTuong, TrangThai, NgayDang)
                        VALUES (@TieuDe, @NoiDung, N'Sự kiện', @NguoiDang, @DoiTuong, N'Đã gửi', GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TieuDe", tieuDe);
                        cmd.Parameters.AddWithValue("@NoiDung", noiDung);
                        cmd.Parameters.AddWithValue("@NguoiDang", maTV); // Có thể thay bằng MaTV của admin
                        cmd.Parameters.AddWithValue("@DoiTuong", maTV.ToString()); // Gửi cho thành viên này

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show(
                                "✅ Đã gửi phần thưởng lên timeline thành công!\n\n" +
                                "Thành viên sẽ nhận được thông báo khi đăng nhập.",
                                "Thành công",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi gửi phần thưởng:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
