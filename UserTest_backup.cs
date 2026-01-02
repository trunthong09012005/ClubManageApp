using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ClubManageApp
{
    public partial class ucUserTest : UserControl
    {
        #region Fields

        // ✅ SỬ DỤNG ConnectionHelper thay vì hard-code
        private string connectionString = ConnectionHelper.ConnectionString;
        private string currentUserRole = "Thành viên"; // Mặc định

        // Controls for pnlThanhVien
        private DataGridView dgvMembers;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Button btnExport;
        private ComboBox cboFilter;
        private Label lblTotalMembers;

        // Controls for pnlThongKe
        private Chart chartMembersByRole;
        private Chart chartMembersByStatus;
        private Chart chartMembersByDepartment;
        private Panel pnlStats;
        private Panel pnlStatsContainer;

        #endregion

        #region Constructor

        public ucUserTest()
        {
            InitializeComponent();
            this.Load += UcUserTest_Load;
        }

        public ucUserTest(string userRole) : this()
        {
            this.currentUserRole = userRole;
        }

        #endregion

        #region Event Handlers

        private void UcUserTest_Load(object sender, EventArgs e)
        {
            InitializeMemberPanel();
            InitializeStatisticsPanel();
            LoadMemberData();
            LoadStatistics();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearch_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string status = cboFilter.SelectedItem?.ToString() ?? "Tất cả";
            LoadMemberData(txtSearch.Text.Trim(), status);
        }

        private void CboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string status = cboFilter.SelectedItem?.ToString() ?? "Tất cả";
            LoadMemberData(txtSearch.Text.Trim(), status);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            FormAddEditMember form = new FormAddEditMember(connectionString, currentUserRole);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadMemberData();
                LoadStatistics();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvMembers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn thành viên cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maTV = Convert.ToInt32(dgvMembers.SelectedRows[0].Cells["Mã TV"].Value);
            FormAddEditMember form = new FormAddEditMember(connectionString, currentUserRole, maTV);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadMemberData();
                LoadStatistics();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvMembers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn thành viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa thành viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    int maTV = Convert.ToInt32(dgvMembers.SelectedRows[0].Cells["Mã TV"].Value);
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM ThanhVien WHERE MaTV = @MaTV";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaTV", maTV);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Xóa thành viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadMemberData();
                    LoadStatistics();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cboFilter.SelectedIndex = 0;
            LoadMemberData();
            LoadStatistics();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV Files (*.csv)|*.csv",
                    FileName = $"DanhSachThanhVien_{DateTime.Now:yyyyMMdd}.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    
                    // Headers
                    for (int i = 0; i < dgvMembers.Columns.Count; i++)
                    {
                        sb.Append(dgvMembers.Columns[i].HeaderText);
                        if (i < dgvMembers.Columns.Count - 1) sb.Append(",");
                    }
                    sb.AppendLine();

                    // Rows
                    foreach (DataGridViewRow row in dgvMembers.Rows)
                    {
                        for (int i = 0; i < dgvMembers.Columns.Count; i++)
                        {
                            sb.Append(row.Cells[i].Value?.ToString() ?? "");
                            if (i < dgvMembers.Columns.Count - 1) sb.Append(",");
                        }
                        sb.AppendLine();
                    }

                    System.IO.File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Xuất file thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvMembers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEdit_Click(sender, e);
            }
        }

        #endregion

        #region Member Panel Initialization

        private void InitializeMemberPanel()
        {
            pnlThanhVien.BackColor = Color.FromArgb(245, 247, 250);
            pnlThanhVien.Padding = new Padding(15);

            // Header Panel
            Panel pnlHeader = CreateHeaderPanel();
            
            // Search and Filter Panel
            Panel pnlSearchFilter = CreateSearchFilterPanel();
            
            // Action Buttons Panel
            Panel pnlActions = CreateActionsPanel();

            // DataGridView
            dgvMembers = CreateMembersDataGridView();

            pnlThanhVien.Controls.Add(dgvMembers);
            pnlThanhVien.Controls.Add(pnlActions);
            pnlThanhVien.Controls.Add(pnlSearchFilter);
            pnlThanhVien.Controls.Add(pnlHeader);
        }

        private Panel CreateHeaderPanel()
        {
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };

            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ THÀNH VIÊN",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(94, 148, 255),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            lblTotalMembers = new Label
            {
                Text = "Tổng số: 0",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 45)
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblTotalMembers);

            return pnlHeader;
        }

        private Panel CreateSearchFilterPanel()
        {
            Panel pnlSearchFilter = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };

            Label lblSearch = new Label
            {
                Text = "Tìm kiếm:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            txtSearch = new TextBox
            {
                Location = new Point(100, 18),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10)
            };
            txtSearch.KeyDown += TxtSearch_KeyDown;

            btnSearch = new Button
            {
                Text = "🔍 Tìm",
                Location = new Point(410, 15),
                Size = new Size(100, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(94, 148, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;

            Label lblFilter = new Label
            {
                Text = "Lọc:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(530, 20)
            };

            cboFilter = new ComboBox
            {
                Location = new Point(570, 18),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboFilter.Items.AddRange(new object[] { "Tất cả", "Hoạt động", "Tạm ngưng", "Nghỉ" });
            cboFilter.SelectedIndex = 0;
            cboFilter.SelectedIndexChanged += CboFilter_SelectedIndexChanged;

            pnlSearchFilter.Controls.AddRange(new Control[] { lblSearch, txtSearch, btnSearch, lblFilter, cboFilter });

            return pnlSearchFilter;
        }

        private Panel CreateActionsPanel()
        {
            Panel pnlActions = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };

            btnAdd = CreateActionButton("➕ Thêm mới", 20, Color.FromArgb(76, 175, 80));
            btnAdd.Click += BtnAdd_Click;

            btnEdit = CreateActionButton("✏️ Sửa", 140, Color.FromArgb(255, 152, 0));
            btnEdit.Click += BtnEdit_Click;

            btnDelete = CreateActionButton("🗑️ Xóa", 260, Color.FromArgb(244, 67, 54));
            btnDelete.Click += BtnDelete_Click;

            btnRefresh = CreateActionButton("🔄 Làm mới", 380, Color.FromArgb(33, 150, 243));
            btnRefresh.Click += BtnRefresh_Click;

            btnExport = CreateActionButton("📥 Xuất Excel", 500, Color.FromArgb(156, 39, 176));
            btnExport.Click += BtnExport_Click;

            pnlActions.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, btnRefresh, btnExport });

            return pnlActions;
        }

        private Button CreateActionButton(string text, int x, Color color)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, 10),
                Size = new Size(110, 40),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private DataGridView CreateMembersDataGridView()
        {
            DataGridView dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9),
                RowTemplate = { Height = 35 }
            };
            dgv.CellDoubleClick += DgvMembers_CellDoubleClick;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(94, 148, 255);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            return dgv;
        }

        #endregion

        #region Data Loading

        private void LoadMemberData(string searchText = "", string filterStatus = "Tất cả")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            MaTV AS [Mã TV],
                            HoTen AS [Họ tên],
                            NgaySinh AS [Ngày sinh],
                            GioiTinh AS [Giới tính],
                            Email,
                            SDT AS [SĐT],
                            Lop AS [Lớp],
                            Khoa,
                            VaiTro AS [Vai trò],
                            TrangThai AS [Trạng thái],
                            NgayThamGia AS [Ngày tham gia]
                        FROM ThanhVien
                        WHERE 1=1";

                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        query += " AND (HoTen LIKE @search OR Email LIKE @search OR SDT LIKE @search OR Lop LIKE @search)";
                    }

                    if (filterStatus != "Tất cả")
                    {
                        query += " AND TrangThai = @status";
                    }

                    query += " ORDER BY MaTV DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(searchText))
                        {
                            cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                        }
                        if (filterStatus != "Tất cả")
                        {
                            cmd.Parameters.AddWithValue("@status", filterStatus);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvMembers.DataSource = dt;
                        lblTotalMembers.Text = $"Tổng số: {dt.Rows.Count} thành viên";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Statistics Panel Initialization

        private void InitializeStatisticsPanel()
        {
            pnlThongKe.BackColor = Color.FromArgb(245, 247, 250);
            pnlThongKe.AutoScroll = true;
            pnlThongKe.Padding = new Padding(10);

            // Main container for all statistics
            pnlStatsContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(245, 247, 250)
            };

            int currentY = 10;

            // Header
            Label lblStatsTitle = new Label
            {
                Text = "THỐNG KÊ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(94, 148, 255),
                AutoSize = true,
                Location = new Point(10, currentY),
                Width = pnlThongKe.ClientSize.Width - 40
            };
            currentY += 50;

            // Stats Summary Panel
            pnlStats = new Panel
            {
                Location = new Point(10, currentY),
                Size = new Size(pnlThongKe.ClientSize.Width - 40, 120),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            currentY += 140;

            // Initialize Charts with proper sizing
            int chartWidth = pnlThongKe.ClientSize.Width - 40;
            
            chartMembersByRole = CreateChart("Thành viên theo Vai trò", 10, currentY, SeriesChartType.Pie, chartWidth);
            currentY += 320;

            chartMembersByStatus = CreateChart("Thành viên theo Trạng thái", 10, currentY, SeriesChartType.Doughnut, chartWidth);
            currentY += 320;

            chartMembersByDepartment = CreateChart("Thành viên theo Ban", 10, currentY, SeriesChartType.Column, chartWidth);

            pnlStatsContainer.Controls.Add(chartMembersByDepartment);
            pnlStatsContainer.Controls.Add(chartMembersByStatus);
            pnlStatsContainer.Controls.Add(chartMembersByRole);
            pnlStatsContainer.Controls.Add(pnlStats);
            pnlStatsContainer.Controls.Add(lblStatsTitle);

            pnlThongKe.Controls.Add(pnlStatsContainer);

            // Handle resize to adjust chart widths
            pnlThongKe.Resize += PnlThongKe_Resize;
        }

        private void PnlThongKe_Resize(object sender, EventArgs e)
        {
            if (pnlStats != null)
            {
                pnlStats.Width = pnlThongKe.ClientSize.Width - 40;
            }

            int chartWidth = pnlThongKe.ClientSize.Width - 40;
            if (chartMembersByRole != null)
            {
                chartMembersByRole.Width = chartWidth;
            }
            if (chartMembersByStatus != null)
            {
                chartMembersByStatus.Width = chartWidth;
            }
            if (chartMembersByDepartment != null)
            {
                chartMembersByDepartment.Width = chartWidth;
            }
        }

        private Chart CreateChart(string title, int x, int y, SeriesChartType chartType, int width = 350)
        {
            Chart chart = new Chart
            {
                Location = new Point(x, y),
                Size = new Size(width, 300),
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            ChartArea chartArea = new ChartArea
            {
                BackColor = Color.White
            };
            chart.ChartAreas.Add(chartArea);

            chart.Titles.Add(new Title
            {
                Text = title,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(94, 148, 255),
                Docking = Docking.Top,
                Alignment = ContentAlignment.MiddleCenter
            });

            Series series = new Series
            {
                ChartType = chartType,
                Font = new Font("Segoe UI", 9),
                IsValueShownAsLabel = true
            };

            // Style for Pie and Doughnut charts
            if (chartType == SeriesChartType.Pie || chartType == SeriesChartType.Doughnut)
            {
                series["PieLabelStyle"] = "Outside";
                series["PieLineColor"] = "Black";
            }

            chart.Series.Add(series);

            Legend legend = new Legend
            {
                Docking = Docking.Bottom,
                Font = new Font("Segoe UI", 9),
                Alignment = StringAlignment.Center
            };
            chart.Legends.Add(legend);

            return chart;
        }

        #endregion

        #region Statistics Loading

        private void LoadStatistics()
        {
            LoadStatsSummary();
            LoadMembersByRole();
            LoadMembersByStatus();
            LoadMembersByDepartment();
        }

        private void LoadStatsSummary()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    string query = @"
                        SELECT 
                            COUNT(*) AS Total,
                            SUM(CASE WHEN TrangThai = N'Hoạt động' THEN 1 ELSE 0 END) AS Active,
                            SUM(CASE WHEN GioiTinh = N'Nam' THEN 1 ELSE 0 END) AS Male,
                            SUM(CASE WHEN GioiTinh = N'Nữ' THEN 1 ELSE 0 END) AS Female
                        FROM ThanhVien";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pnlStats.Controls.Clear();

                            int total = reader.GetInt32(0);
                            int active = reader.GetInt32(1);
                            int male = reader.GetInt32(2);
                            int female = reader.GetInt32(3);

                            int boxWidth = (pnlStats.Width - 50) / 4;
                            int spacing = 10;

                            AddStatBox(pnlStats, "Tổng số", total.ToString(), Color.FromArgb(33, 150, 243), spacing);
                            AddStatBox(pnlStats, "Hoạt động", active.ToString(), Color.FromArgb(76, 175, 80), spacing + boxWidth + spacing);
                            AddStatBox(pnlStats, "Nam", male.ToString(), Color.FromArgb(255, 152, 0), spacing + (boxWidth + spacing) * 2);
                            AddStatBox(pnlStats, "Nữ", female.ToString(), Color.FromArgb(233, 30, 99), spacing + (boxWidth + spacing) * 3);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thống kê tổng quan: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddStatBox(Panel parent, string label, string value, Color color, int x)
        {
            int boxWidth = (parent.Width - 50) / 4;
            
            Panel box = new Panel
            {
                Location = new Point(x, 10),
                Size = new Size(boxWidth, 100),
                BackColor = color
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(boxWidth, 50),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 15)
            };

            Label lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(boxWidth, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 65)
            };

            box.Controls.Add(lblValue);
            box.Controls.Add(lblLabel);
            parent.Controls.Add(box);
        }

        private void LoadMembersByRole()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT VaiTro, COUNT(*) AS Count FROM ThanhVien WHERE VaiTro IS NOT NULL GROUP BY VaiTro";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        chartMembersByRole.Series[0].Points.Clear();
                        
                        while (reader.Read())
                        {
                            string role = reader.GetString(0);
                            int count = reader.GetInt32(1);
                            var point = chartMembersByRole.Series[0].Points.AddXY(role, count);
                            chartMembersByRole.Series[0].Points[point].LegendText = $"{role} ({count})";
                            chartMembersByRole.Series[0].Points[point].Label = count.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải biểu đồ vai trò: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMembersByStatus()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT TrangThai, COUNT(*) AS Count FROM ThanhVien GROUP BY TrangThai";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        chartMembersByStatus.Series[0].Points.Clear();
                        
                        while (reader.Read())
                        {
                            string status = reader.GetString(0);
                            int count = reader.GetInt32(1);
                            var point = chartMembersByStatus.Series[0].Points.AddXY(status, count);
                            
                            // Set colors based on status
                            if (status == "Hoạt động")
                                chartMembersByStatus.Series[0].Points[point].Color = Color.FromArgb(76, 175, 80);
                            else if (status == "Tạm ngưng")
                                chartMembersByStatus.Series[0].Points[point].Color = Color.FromArgb(255, 152, 0);
                            else
                                chartMembersByStatus.Series[0].Points[point].Color = Color.FromArgb(244, 67, 54);

                            chartMembersByStatus.Series[0].Points[point].LegendText = $"{status} ({count})";
                            chartMembersByStatus.Series[0].Points[point].Label = count.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải biểu đồ trạng thái: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMembersByDepartment()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT BCM.TenBan, COUNT(BCTV.MaTV) AS Count
                        FROM BanChuyenMon BCM
                        LEFT JOIN BanChuyenMon_ThanhVien BCTV ON BCM.MaBan = BCTV.MaBan
                        GROUP BY BCM.TenBan
                        ORDER BY Count DESC";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        chartMembersByDepartment.Series[0].Points.Clear();
                        
                        while (reader.Read())
                        {
                            string department = reader.GetString(0);
                            int count = reader.GetInt32(1);
                            var point = chartMembersByDepartment.Series[0].Points.AddXY(department, count);
                            chartMembersByDepartment.Series[0].Points[point].Label = count.ToString();
                        }

                        chartMembersByDepartment.ChartAreas[0].AxisX.Interval = 1;
                        chartMembersByDepartment.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
                        chartMembersByDepartment.ChartAreas[0].AxisY.Title = "Số lượng";
                        chartMembersByDepartment.ChartAreas[0].AxisX.Title = "Ban chuyên môn";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải biểu đồ ban: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
