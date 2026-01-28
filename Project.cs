using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ClubManageApp
{
    public partial class ucProject : UserControl
    {
        // ✅ SỬ DỤNG ConnectionHelper thay vì hard-code
        private string connectionString = ConnectionHelper.ConnectionString;
        
        // in-memory view of projects loaded from DB
        private List<ProjectItem> allProjects = new List<ProjectItem>();
        private BindingList<ProjectItem> viewProjects = new BindingList<ProjectItem>();
        private BindingSource bsProjects = new BindingSource();

        // Pagination fields
        private int currentPage =1;
        private int pageSize =20;
        private int totalRecords =0;
        private int totalPages =1;
        private string currentFilter = null;

        public ucProject()
        {
            InitializeComponent();
            InitializeLogic();
        }

        private void InitializeLogic()
        {
            // setup DataGridView
            dgvProjects.AutoGenerateColumns = false;
            dgvProjects.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProjects.MultiSelect = false;
            dgvProjects.AllowUserToAddRows = false;
            // use Fill mode and set column fill weights so Name column gets most space
            dgvProjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvProjects.Columns.Clear();
            dgvProjects.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Id",
                HeaderText = "Mã",
                Width = 60,
                FillWeight = 20
            });
            dgvProjects.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Name",
                HeaderText = "Tên dự án",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 300
            });
            dgvProjects.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "StartDateText",
                HeaderText = "Bắt đầu",
                Width = 120,
                FillWeight = 60
            });
            dgvProjects.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "EndDateText",
                HeaderText = "Kết thúc",
                Width = 120,
                FillWeight = 60
            });
            dgvProjects.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Priority",
                HeaderText = "Mức độ",
                Width = 120,
                FillWeight = 60
            });
            dgvProjects.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Progress",
                HeaderText = "Tiến độ (%)",
                Width = 100,
                FillWeight = 40
            });
            dgvProjects.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Status",
                HeaderText = "Trạng thái",
                Width = 120,
                FillWeight = 80
            });
            dgvProjects.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "CreatedText",
                HeaderText = "Ngày tạo",
                Width = 140,
                FillWeight = 80
            });
            // assigned members column
            dgvProjects.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "AssignedMembers",
                HeaderText = "Phân công",
                Width = 200,
                FillWeight = 120
            });

            bsProjects.DataSource = viewProjects;
            dgvProjects.DataSource = bsProjects;
            // allow column reorder and horizontal scrolling
            dgvProjects.AllowUserToOrderColumns = true;
            dgvProjects.ScrollBars = ScrollBars.Both;

            // Ensure background is white (remove gray area when grid doesn't fill control)
            dgvProjects.BackgroundColor = Color.White;
            dgvProjects.DefaultCellStyle.BackColor = Color.White;
            dgvProjects.RowsDefaultCellStyle.BackColor = Color.White;
            dgvProjects.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dgvProjects.GridColor = Color.LightGray;

            // wire events
            txtSearch.TextChanged += TxtSearch_TextChanged;
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            // detail button
            try
            {
                var detailBtn = this.Controls.Find("btnDetail", true).FirstOrDefault() as Button;
                if (detailBtn != null) detailBtn.Click += BtnDetail_Click;
            }
            catch { }

            // wire clear search button if present
            try
            {
                var clearBtn = this.Controls.Find("btnClearSearch", true).FirstOrDefault() as Button;
                if (clearBtn != null)
                {
                    clearBtn.Click += (s, e) => { txtSearch.Text = string.Empty; txtSearch.Focus(); };
                }

                // wire context menu items if present
                var cms = this.Controls.Find("cmsGrid", true).FirstOrDefault() as ContextMenuStrip;
                if (cms != null)
                {
                    var editItem = cms.Items.Cast<ToolStripItem>().FirstOrDefault(it => it.Name == "cmsEdit") as ToolStripItem;
                    var delItem = cms.Items.Cast<ToolStripItem>().FirstOrDefault(it => it.Name == "cmsDelete") as ToolStripItem;
                    if (editItem != null) editItem.Click += (s, e) => BtnEdit_Click(s, e);
                    if (delItem != null) delItem.Click += (s, e) => BtnDelete_Click(s, e);
                }
            }
            catch { }

            // wire pagination controls if present
            try
            {
                var btnPrev = this.Controls.Find("btnPreviousPage", true).FirstOrDefault() as Button;
                var btnNext = this.Controls.Find("btnNextPage", true).FirstOrDefault() as Button;
                var cb = this.Controls.Find("cboPageSize", true).FirstOrDefault() as ComboBox;
                if (btnPrev != null) btnPrev.Click += BtnPreviousPage_Click;
                if (btnNext != null) btnNext.Click += BtnNextPage_Click;
                if (cb != null) cb.SelectedIndexChanged += CboPageSize_SelectedIndexChanged;
            }
            catch { }

            dgvProjects.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnEdit_Click(s, e); };
            dgvProjects.MouseDown += DgvProjects_MouseDown;
            dgvProjects.SelectionChanged += DgvProjects_SelectionChanged;

            // custom paint for chart panel (use Progress)
            chartProjects.Paint += ChartProjects_Paint;

            // wire pagination controls if present
            try
            {
                var btnPrev = this.Controls.Find("btnPreviousPage", true).FirstOrDefault() as Button;
                var btnNext = this.Controls.Find("btnNextPage", true).FirstOrDefault() as Button;
                var cb = this.Controls.Find("cboPageSize", true).FirstOrDefault() as ComboBox;
                if (btnPrev != null) btnPrev.Click += BtnPreviousPage_Click;
                if (btnNext != null) btnNext.Click += BtnNextPage_Click;
                if (cb != null) cb.SelectedIndexChanged += CboPageSize_SelectedIndexChanged;
            }
            catch { }

            // load from database
            InitializePagination();
            RefreshView();
        }

        private void InitializePagination()
        {
            try
            {
                var cb = this.Controls.Find("cboPageSize", true).FirstOrDefault() as ComboBox;
                if (cb != null)
                {
                    cb.SelectedItem = pageSize.ToString();
                }
                UpdatePaginationControls();
            }
            catch { }
        }

        private void DgvProjects_SelectionChanged(object sender, EventArgs e)
        {
            bool has = dgvProjects.CurrentRow != null;
            try { btnEdit.Enabled = has; } catch { }
            try { btnDelete.Enabled = has; } catch { }
        }

        private void DgvProjects_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hit = dgvProjects.HitTest(e.X, e.Y);
                if (hit.RowIndex >= 0)
                {
                    dgvProjects.ClearSelection();
                    dgvProjects.Rows[hit.RowIndex].Selected = true;
                    dgvProjects.CurrentCell = dgvProjects.Rows[hit.RowIndex].Cells[0];
                }
            }
        }

        private void RefreshView(string filter = null)
        {
            // if filter changed reset to first page
            if (filter != currentFilter) currentPage =1;

            // load from DB with optional filter
            allProjects.Clear();
            string sql = @"SELECT MaDA, TenDuAn, MoTa, NgayBatDau, NgayKetThuc, MucDoUuTien, TienDo, TrangThai, NgayTao
                           FROM DuAn";
            if (!string.IsNullOrWhiteSpace(filter))
            {
                sql += " WHERE TenDuAn LIKE @q";
            }
            sql += " ORDER BY MaDA";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        cmd.Parameters.AddWithValue("@q", "%" + filter.Trim() + "%");
                    }

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var p = new ProjectItem
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("MaDA")),
                                Name = reader["TenDuAn"] as string ?? string.Empty,
                                Description = reader["MoTa"] as string ?? string.Empty,
                                StartDate = reader["NgayBatDau"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayBatDau"]),
                                EndDate = reader["NgayKetThuc"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayKetThuc"]),
                                Priority = reader["MucDoUuTien"] as string ?? string.Empty,
                                Progress = reader["TienDo"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TienDo"]),
                                Status = reader["TrangThai"] as string ?? string.Empty,
                                Created = reader["NgayTao"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayTao"])
                            };
                            allProjects.Add(p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Lỗi khi tải dự án: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // after loading projects, load assignments and update viewProjects
            // load assignments for projects
            try
            {
                var ids = allProjects.Select(p => p.Id).ToList();
                if (ids.Count > 0)
                {
                    // build parameterized IN clause
                    var parameters = new List<string>();
                    for (int i = 0; i < ids.Count; i++) parameters.Add("@id" + i);
                    string inClause = string.Join(",", parameters);
                    string sqlAssign = $"SELECT PC.MaDA, TV.HoTen FROM PhanCong PC INNER JOIN ThanhVien TV ON PC.MaTV = TV.MaTV WHERE PC.MaDA IN ({inClause})";
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand(sqlAssign, conn))
                    {
                        for (int i = 0; i < ids.Count; i++) cmd.Parameters.AddWithValue(parameters[i], ids[i]);
                        conn.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            var dict = new Dictionary<int, List<string>>();
                            while (rdr.Read())
                            {
                                int da = Convert.ToInt32(rdr["MaDA"]);
                                string name = rdr["HoTen"] as string ?? string.Empty;
                                if (!dict.ContainsKey(da)) dict[da] = new List<string>();
                                dict[da].Add(name);
                            }

                            // attach to projects
                            foreach (var p in allProjects)
                            {
                                if (dict.ContainsKey(p.Id)) p.AssignedMembers = string.Join(", ", dict[p.Id]);
                                else p.AssignedMembers = string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception) { /* ignore assignment load errors */ }

            // Apply filter/search to allProjects in-memory and then page
            currentFilter = filter;
            IEnumerable<ProjectItem> filtered = allProjects;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                var q = filter.Trim();
                filtered = filtered.Where(p => p.Name.IndexOf(q, StringComparison.OrdinalIgnoreCase) >=0 || (p.Description ?? string.Empty).IndexOf(q, StringComparison.OrdinalIgnoreCase) >=0);
            }
            
            var filteredList = filtered.ToList();
            totalRecords = filteredList.Count;
            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            if (totalPages ==0) totalPages =1;
            if (currentPage > totalPages) currentPage = totalPages;
            if (currentPage <1) currentPage =1;

            var pageItems = filteredList.Skip((currentPage -1) * pageSize).Take(pageSize).ToList();

            viewProjects.RaiseListChangedEvents = false;
            viewProjects.Clear();
            foreach (var it in pageItems) viewProjects.Add(it);
            viewProjects.RaiseListChangedEvents = true;
            viewProjects.ResetBindings();

            UpdatePaginationControls();

            try
            {
                var lbl = this.Controls.Find("lblCount", true).FirstOrDefault() as Label;
                if (lbl != null) lbl.Text = $"Số dự án: {viewProjects.Count}";
            }
            catch { }

            chartProjects.Invalidate();
        }

        private void UpdatePaginationControls()
        {
            try
            {
                var lbl = this.Controls.Find("lblPageInfo", true).FirstOrDefault() as Label;
                if (lbl != null)
                {
                    lbl.Text = $"Trang {currentPage} / {totalPages} (Tổng: {totalRecords} dự án)";
                }

                var btnPrev = this.Controls.Find("btnPreviousPage", true).FirstOrDefault() as Button;
                var btnNext = this.Controls.Find("btnNextPage", true).FirstOrDefault() as Button;
                if (btnPrev != null) btnPrev.Enabled = currentPage >1;
                if (btnNext != null) btnNext.Enabled = currentPage < totalPages;
            }
            catch { }
        }

        private void BtnPreviousPage_Click(object sender, EventArgs e)
        {
            if (currentPage >1)
            {
                currentPage--;
                RefreshView(currentFilter);
            }
        }

        private void BtnNextPage_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                RefreshView(currentFilter);
            }
        }

        private void CboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb == null) return;
            if (int.TryParse(cb.SelectedItem?.ToString(), out int newSize))
            {
                pageSize = newSize;
                currentPage =1;
                RefreshView(currentFilter);
            }
        }

        private void ChartProjects_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(chartProjects.BackColor);
            if (viewProjects.Count == 0) return;

            int w = chartProjects.ClientSize.Width;
            int h = chartProjects.ClientSize.Height;
            int margin = 20;
            int availableW = Math.Max(1, w - margin * 2);
            int availableH = Math.Max(1, h - margin * 2);
            int cols = viewProjects.Count;
            int colW = Math.Max(1, availableW / cols - 10);
            int x = margin;
            int maxVal = viewProjects.Max(p => p.Progress);
            if (maxVal == 0) maxVal = 100; // show relative to100

            using (var font = new Font("Segoe UI", 8))
            using (var pen = new Pen(Color.DimGray))
            {
                for (int i = 0; i < viewProjects.Count; i++)
                {
                    var p = viewProjects[i];
                    int colH = (int)Math.Round((p.Progress / (double)maxVal) * (availableH - 30));
                    var rect = new Rectangle(x, margin + (availableH - colH), colW, colH);

                    Color barColor = Color.SeaGreen;
                    if (string.Equals(p.Status, "Tạm dừng", StringComparison.OrdinalIgnoreCase)) barColor = Color.IndianRed;
                    else if (string.Equals(p.Status, "Hoàn thành", StringComparison.OrdinalIgnoreCase)) barColor = Color.SeaGreen;
                    else if (string.Equals(p.Status, "Hủy bỏ", StringComparison.OrdinalIgnoreCase)) barColor = Color.LightGray;

                    using (var brush = new SolidBrush(barColor))
                    {
                        g.FillRectangle(brush, rect);
                    }
                    g.DrawRectangle(pen, rect);

                    // label
                    var label = p.Name;
                    var size = g.MeasureString(label, font);
                    var lx = x + (colW - (int)size.Width) / 2;
                    g.DrawString(label, font, Brushes.Black, Math.Max(margin, lx), margin + availableH + 2);
                    // value
                    var vs = p.Progress.ToString();
                    var vsz = g.MeasureString(vs, font);
                    var vx = x + (colW - (int)vsz.Width) / 2;
                    g.DrawString(vs, font, Brushes.Black, vx, rect.Top - (int)vsz.Height - 2);

                    x += colW + 10;
                }
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshView(txtSearch.Text);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new ProjectDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // insert into DB
                    string sql = @"INSERT INTO DuAn (TenDuAn, MoTa, NgayBatDau, NgayKetThuc, MucDoUuTien, TienDo, TrangThai)
                                   VALUES (@Ten, @MoTa, @NgayBD, @NgayKT, @MucDo, @TienDo, @TrangThai)";
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@Ten", dlg.ProjectName);
                            cmd.Parameters.AddWithValue("@MoTa", (object)dlg.Description ?? DBNull.Value);
                            if (dlg.StartDate.HasValue) cmd.Parameters.AddWithValue("@NgayBD", dlg.StartDate.Value.Date); else cmd.Parameters.AddWithValue("@NgayBD", DBNull.Value);
                            if (dlg.EndDate.HasValue) cmd.Parameters.AddWithValue("@NgayKT", dlg.EndDate.Value.Date); else cmd.Parameters.AddWithValue("@NgayKT", DBNull.Value);
                            cmd.Parameters.AddWithValue("@MucDo", (object)dlg.Priority ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@TienDo", dlg.Progress);
                            // New project should have status 'Mới tạo' by default
                            cmd.Parameters.AddWithValue("@TrangThai", "Mới tạo");

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Lỗi khi thêm dự án: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    RefreshView(txtSearch.Text);
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProjects.CurrentRow == null) return;
            var item = dgvProjects.CurrentRow.DataBoundItem as ProjectItem;
            if (item == null) return;

            using (var dlg = new ProjectDialog(item))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    string sql = @"UPDATE DuAn SET TenDuAn=@Ten, MoTa=@MoTa, NgayBatDau=@NgayBD, NgayKetThuc=@NgayKT, MucDoUuTien=@MucDo, TienDo=@TienDo, TrangThai=@TrangThai WHERE MaDA=@Id";
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@Ten", dlg.ProjectName);
                            cmd.Parameters.AddWithValue("@MoTa", (object)dlg.Description ?? DBNull.Value);
                            if (dlg.StartDate.HasValue) cmd.Parameters.AddWithValue("@NgayBD", dlg.StartDate.Value.Date); else cmd.Parameters.AddWithValue("@NgayBD", DBNull.Value);
                            if (dlg.EndDate.HasValue) cmd.Parameters.AddWithValue("@NgayKT", dlg.EndDate.Value.Date); else cmd.Parameters.AddWithValue("@NgayKT", DBNull.Value);
                            cmd.Parameters.AddWithValue("@MucDo", (object)dlg.Priority ?? DBNull.Value);
                            // enforce progress based on status
                            string statusToSave = dlg.Status ?? item.Status;
                            int progressToSave = dlg.Progress;
                            if (!string.IsNullOrEmpty(statusToSave))
                            {
                                if (string.Equals(statusToSave, "Hoàn thành", StringComparison.OrdinalIgnoreCase)) progressToSave = 100;
                                else if (string.Equals(statusToSave, "Hủy bỏ", StringComparison.OrdinalIgnoreCase)) progressToSave = 0;
                            }
                            cmd.Parameters.AddWithValue("@TienDo", progressToSave);
                            cmd.Parameters.AddWithValue("@TrangThai", (object)statusToSave ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Id", item.Id);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Lỗi khi cập nhật dự án: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    RefreshView(txtSearch.Text);
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProjects.CurrentRow == null) return;
            var item = dgvProjects.CurrentRow.DataBoundItem as ProjectItem;
            if (item == null) return;

            var r = MessageBox.Show(this, $"Xác nhận xóa dự án '{item.Name}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                string sql = "DELETE FROM DuAn WHERE MaDA=@Id";
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", item.Id);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Lỗi khi xóa dự án: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                RefreshView(txtSearch.Text);
            }
        }

        private void BtnDetail_Click(object sender, EventArgs e)
        {
            if (dgvProjects.CurrentRow == null) return;
            var item = dgvProjects.CurrentRow.DataBoundItem as ProjectItem;
            if (item == null) return;

            using (var dlg = new ProjectDetailDialog(item, connectionString))
            {
                var res = dlg.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    // if assignment done, change DuAn.TrangThai to 'Đang thực hiện'
                    if (dlg.Assigned)
                    {
                        string sql = "UPDATE DuAn SET TrangThai=@TrangThai WHERE MaDA=@Id";
                        try
                        {
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            using (SqlCommand cmd = new SqlCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@TrangThai", "Đang thực hiện");
                                cmd.Parameters.AddWithValue("@Id", item.Id);
                                conn.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "Lỗi khi cập nhật trạng thái: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    RefreshView(txtSearch.Text);
                }
            }
        }

        // dialog for add/edit with DB fields
        private class ProjectDialog : Form
        {
            private TextBox txtName;
            private TextBox txtDescription;
            private DateTimePicker dtpStart;
            private DateTimePicker dtpEnd;
            private ComboBox cmbPriority;
            private NumericUpDown nudProgress;
            private ComboBox cmbStatus;
            private Button btnOk;
            private Button btnCancel;

            public string ProjectName => txtName.Text.Trim();
            public string Description => txtDescription.Text.Trim();
            public DateTime? StartDate => dtpStart.Checked ? (DateTime?)dtpStart.Value.Date : null;
            public DateTime? EndDate => dtpEnd.Checked ? (DateTime?)dtpEnd.Value.Date : null;
            public string Priority => cmbPriority.SelectedItem as string;
            public int Progress => (int)nudProgress.Value;
            public string Status => cmbStatus.SelectedItem as string;

            public ProjectDialog()
            {
                Initialize();
            }

            public ProjectDialog(ProjectItem p) : this()
            {
                if (p != null)
                {
                    txtName.Text = p.Name;
                    txtDescription.Text = p.Description;
                    if (p.StartDate.HasValue) { dtpStart.Value = p.StartDate.Value; dtpStart.Checked = true; } else dtpStart.Checked = false;
                    if (p.EndDate.HasValue) { dtpEnd.Value = p.EndDate.Value; dtpEnd.Checked = true; } else dtpEnd.Checked = false;
                    if (!string.IsNullOrEmpty(p.Priority) && cmbPriority.Items.Contains(p.Priority)) cmbPriority.SelectedItem = p.Priority;
                    nudProgress.Value = p.Progress;
                    if (!string.IsNullOrEmpty(p.Status) && cmbStatus.Items.Contains(p.Status)) cmbStatus.SelectedItem = p.Status;
                }
            }

            private void Initialize()
            {
                this.Text = "Dự án";
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.StartPosition = FormStartPosition.CenterParent;
                this.ClientSize = new Size(500, 360);
                this.MaximizeBox = false;
                this.MinimizeBox = false;

                var lblName = new Label { Text = "Tên dự án:", Location = new Point(10, 15), AutoSize = true };
                txtName = new TextBox { Location = new Point(120, 12), Width = 350 };

                var lblDesc = new Label { Text = "Mô tả:", Location = new Point(10, 50), AutoSize = true };
                txtDescription = new TextBox { Location = new Point(120, 46), Width = 350, Height = 80, Multiline = true, ScrollBars = ScrollBars.Vertical };

                var lblStart = new Label { Text = "Bắt đầu:", Location = new Point(10, 140), AutoSize = true };
                dtpStart = new DateTimePicker { Location = new Point(120, 136), Width = 200, Format = DateTimePickerFormat.Short, ShowCheckBox = true };

                var lblEnd = new Label { Text = "Kết thúc:", Location = new Point(10, 175), AutoSize = true };
                dtpEnd = new DateTimePicker { Location = new Point(120, 171), Width = 200, Format = DateTimePickerFormat.Short, ShowCheckBox = true };

                var lblPriority = new Label { Text = "Mức độ ưu tiên:", Location = new Point(10, 210), AutoSize = true };
                cmbPriority = new ComboBox { Location = new Point(120, 206), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbPriority.Items.AddRange(new object[] { "Thấp", "Trung bình", "Cao", "Khẩn cấp" });

                var lblProgress = new Label { Text = "Tiến độ (%):", Location = new Point(10, 245), AutoSize = true };
                nudProgress = new NumericUpDown { Location = new Point(120, 241), Width = 100, Minimum = 0, Maximum = 100 };

                var lblStatus = new Label { Text = "Trạng thái:", Location = new Point(10, 280), AutoSize = true };
                cmbStatus = new ComboBox { Location = new Point(120, 276), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbStatus.Items.AddRange(new object[] { "Mới tạo", "Đang thực hiện", "Tạm dừng", "Hoàn thành", "Hủy bỏ" });

                btnOk = new Button { Text = "Lưu", Location = new Point(310, 310), DialogResult = DialogResult.OK };
                btnCancel = new Button { Text = "Hủy", Location = new Point(395, 310), DialogResult = DialogResult.Cancel };

                this.Controls.AddRange(new Control[] { lblName, txtName, lblDesc, txtDescription, lblStart, dtpStart, lblEnd, dtpEnd, lblPriority, cmbPriority, lblProgress, nudProgress, lblStatus, cmbStatus, btnOk, btnCancel });

                this.AcceptButton = btnOk;
                this.CancelButton = btnCancel;

                btnOk.Click += BtnOk_Click;
            }

            private void BtnOk_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show(this, "Tên dự án không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                    return;
                }

                if (dtpStart.Checked && dtpEnd.Checked && dtpEnd.Value.Date < dtpStart.Value.Date)
                {
                    MessageBox.Show(this, "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                    return;
                }

                // ensure selections for comboboxes
                if (cmbPriority.SelectedItem == null) cmbPriority.SelectedIndex = 0;
                if (cmbStatus.SelectedItem == null) cmbStatus.SelectedIndex = 1; // mặc định 'Đang thực hiện'
            }
        }

        // Detail dialog with member selection and assign button
        private class ProjectDetailDialog : Form
        {
            private Label lblTitle;
            private TextBox txtDescription;
            private ComboBox cmbMembers;
            private ListBox lstAssigned;
            private List<int> assignedIds = new List<int>();
            private Button btnAssign;
            private Button btnClose;
            private ProjectItem project;
            private string connectionString;

            public bool Assigned { get; private set; } = false;

            public ProjectDetailDialog(ProjectItem p, string connStr)
            {
                project = p;
                connectionString = connStr;
                Initialize();
                LoadMembers();
            }

            private void Initialize()
            {
                this.Text = "Chi tiết dự án";
                this.ClientSize = new Size(600, 320);
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.StartPosition = FormStartPosition.CenterParent;

                lblTitle = new Label { Text = project.Name, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(10, 10), AutoSize = true };
                txtDescription = new TextBox { Location = new Point(10, 40), Width = 560, Height = 110, Multiline = true, ReadOnly = true, Text = project.Description, ScrollBars = ScrollBars.Vertical };

                // Assigned list
                var lblAssigned = new Label { Text = "Đã phân công:", Location = new Point(10, 160), AutoSize = true };
                lstAssigned = new ListBox { Location = new Point(10, 185), Width = 350, Height = 80 };

                var lblMember = new Label { Text = "Chọn thành viên:", Location = new Point(380, 160), AutoSize = true };
                cmbMembers = new ComboBox { Location = new Point(380, 185), Width = 190, DropDownStyle = ComboBoxStyle.DropDownList };

                btnAssign = new Button { Text = "Phân công", Location = new Point(380, 220), Width = 90 };
                var btnRemove = new Button { Text = "Loại bỏ", Location = new Point(240, 272), Width = 90 };
                btnClose = new Button { Text = "Đóng", Location = new Point(480, 272), Width = 90, DialogResult = DialogResult.Cancel };

                this.Controls.AddRange(new Control[] { lblTitle, txtDescription, lblAssigned, lstAssigned, lblMember, cmbMembers, btnAssign, btnRemove, btnClose });

                btnAssign.Click += BtnAssign_Click;
                btnRemove.Click += BtnRemove_Click;
            }

            private void LoadMembers()
            {
                // load members from ThanhVien table
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand("SELECT MaTV, HoTen FROM ThanhVien ORDER BY HoTen", conn))
                    {
                        conn.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                int id = Convert.ToInt32(rdr["MaTV"]);
                                string name = rdr["HoTen"] as string ?? string.Empty;
                                cmbMembers.Items.Add(new KeyValuePair<int, string>(id, name));
                            }
                        }
                    }

                    // load current assignments
                    LoadAssignments();

                    if (cmbMembers.Items.Count > 0) cmbMembers.SelectedIndex = 0;
                    // display member names
                    cmbMembers.DrawMode = DrawMode.Normal;
                    cmbMembers.Format += (s, e) =>
                    {
                        if (e.ListItem is KeyValuePair<int, string> kv) e.Value = kv.Value;
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Lỗi khi tải thành viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private void LoadAssignments()
            {
                lstAssigned.Items.Clear();
                assignedIds.Clear();
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand("SELECT PC.MaTV, TV.HoTen FROM PhanCong PC INNER JOIN ThanhVien TV ON PC.MaTV=TV.MaTV WHERE PC.MaDA=@MaDA", conn))
                    {
                        cmd.Parameters.AddWithValue("@MaDA", project.Id);
                        conn.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                int id = Convert.ToInt32(rdr["MaTV"]);
                                string name = rdr["HoTen"] as string ?? string.Empty;
                                lstAssigned.Items.Add(name);
                                assignedIds.Add(id);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ignore but show message
                    MessageBox.Show(this, "Lỗi khi tải phân công: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private void BtnAssign_Click(object sender, EventArgs e)
            {
                if (cmbMembers.SelectedItem == null) return;
                var kv = (KeyValuePair<int, string>)cmbMembers.SelectedItem;
                int maTV = kv.Key;

                // insert into PhanCong
                string sql = @"INSERT INTO PhanCong (MaTV, MaDA, NhiemVu, TrangThai, NgayHetHan)
VALUES (@MaTV, @MaDA, @NhiemVu, @TrangThai, @NgayHetHan)";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTV", maTV);
                        cmd.Parameters.AddWithValue("@MaDA", project.Id);
                        cmd.Parameters.AddWithValue("@NhiemVu", "Phụ trách");
                        cmd.Parameters.AddWithValue("@TrangThai", "Chưa hoàn thành");
                        cmd.Parameters.AddWithValue("@NgayHetHan", DBNull.Value);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show(this, "Phân công thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Assigned = true;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (SqlException sqlEx)
                {
                    // If unique constraint on (MaTV,MaDA) prevents duplicate assignment
                    MessageBox.Show(this, "Lỗi SQL khi phân công: " + sqlEx.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Lỗi khi phân công: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private void BtnRemove_Click(object sender, EventArgs e)
            {
                if (lstAssigned.SelectedIndex < 0) return;
                int idx = lstAssigned.SelectedIndex;
                int maTV = assignedIds[idx];

                var r = MessageBox.Show(this, "Xác nhận loại bỏ phân công cho thành viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r != DialogResult.Yes) return;

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM PhanCong WHERE MaTV=@MaTV AND MaDA=@MaDA", conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTV", maTV);
                        cmd.Parameters.AddWithValue("@MaDA", project.Id);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    // remove from UI lists
                    lstAssigned.Items.RemoveAt(idx);
                    assignedIds.RemoveAt(idx);

                    MessageBox.Show(this, "Loại bỏ phân công thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // if no assignments left, update DuAn.TrangThai -> 'Mới tạo'
                    if (assignedIds.Count == 0)
                    {
                        try
                        {
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            using (SqlCommand cmd = new SqlCommand("UPDATE DuAn SET TrangThai=@TrangThai WHERE MaDA=@MaDA", conn))
                            {
                                cmd.Parameters.AddWithValue("@TrangThai", "Mới tạo");
                                cmd.Parameters.AddWithValue("@MaDA", project.Id);
                                conn.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch { }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Lỗi khi loại bỏ phân công: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // project model matching DB
        private class ProjectItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Priority { get; set; }
            public int Progress { get; set; }
            public string Status { get; set; }
            public DateTime? Created { get; set; }
            public string AssignedMembers { get; set; }

            public string StartDateText => StartDate.HasValue ? StartDate.Value.ToString("dd/MM/yyyy") : string.Empty;
            public string EndDateText => EndDate.HasValue ? EndDate.Value.ToString("dd/MM/yyyy") : string.Empty;
            public string CreatedText => Created.HasValue ? Created.Value.ToString("dd/MM/yyyy HH:mm") : string.Empty;
        }

        private void chartProjects_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
//Can be update in the future to add more features
//Can be update in the future to add more features
//Can be update in the future to add more features
//Can be update in the future to add more features
//Can be update in the future to add more features
//Can be update in the future to add more features
//Can be update in the future to add more features
//Can be update in the future to add more features
//Can be update in the future to add more features
//Can be update in the future to add more features