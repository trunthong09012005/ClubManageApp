using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ClubManageApp
{
    public partial class ucFinance : UserControl
    {
        private DataTable thuChiTable;
        private DataView thuChiView;


        // ✅ SỬ DỤNG ConnectionHelper thay vì hard-code
        private string connectionString = ConnectionHelper.ConnectionString;

        // Pagination fields
        private int currentPage =1;
        private int pageSize =20;
        private int totalRecords =0;
        private int totalPages =0;
        private string currentFilter = null;

        public ucFinance()
        {
            InitializeComponent();
            InitializeTable();
            HookEvents();
            InitializePagination();
            LoadDataFromDb();
        }

        private void InitializeTable()
        {
            thuChiTable = new DataTable();
            thuChiView = new DataView(thuChiTable);
            dgvThuChi.DataSource = thuChiView;

            // configure grid 
            dgvThuChi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThuChi.MultiSelect = false;
            dgvThuChi.ReadOnly = true;
            dgvThuChi.AutoGenerateColumns = true;

            // make columns auto-size to fill control width
            dgvThuChi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvThuChi.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvThuChi.RowHeadersWidth =24;

            // ensure background area is white (remove default gray area when rows don't fill control)
            dgvThuChi.BackgroundColor = System.Drawing.Color.White;
            dgvThuChi.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvThuChi.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            // keep alternating rows subtle if desired (optional) - set to same white to avoid gray bands
            dgvThuChi.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.White;

            // grid lines color
            dgvThuChi.GridColor = System.Drawing.Color.LightGray;
        }

        private void HookEvents()
        {
            txtSearch.TextChanged += (s, e) => ApplyFilterAndSearch();
            cmbFilter.SelectedIndexChanged += (s, e) => ApplyFilterAndSearch();
            btnCreate.Click += BtnCreate_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnSearch.Click += BtnSearch_Click;
            btnRefresh.Click += BtnRefresh_Click;
            this.Load += UcFinance_Load;
        }

        private void InitializePagination()
        {
            try
            {
                // set default page size if control exists
                if (cboPageSize != null)
                {
                    cboPageSize.SelectedItem = pageSize.ToString();
                }

                UpdatePaginationControls();
            }
            catch { }
        }

        private void UcFinance_Load(object sender, EventArgs e)
        {
            cmbFilter.Items.Clear();
            cmbFilter.Items.Add("Tất cả");
            cmbFilter.Items.Add("Thu");
            cmbFilter.Items.Add("Chi");
            cmbFilter.SelectedIndex =0;

            // ensure grid fills panel when control sized in designer/runtime
            dgvThuChi.Dock = DockStyle.Fill;
        }

        private void LoadDataFromDb()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("SELECT t.MaGD, t.LoaiGD, t.SoTien, t.NgayGD, t.NoiDung, t.NguoiThucHien, TV.HoTen AS NguoiThucHienName, t.MaHD, t.MaNguon, t.TrangThai, t.NgayTao FROM ThuChi t LEFT JOIN ThanhVien TV ON t.NguoiThucHien = TV.MaTV ORDER BY t.NgayGD DESC", conn))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    thuChiTable.Clear();
                    adapter.Fill(thuChiTable);
                }

                ApplyColumnSettings();

                ApplyFilterAndSearch();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Tài chính: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyColumnSettings()
        {
            try
            {
                // set nicer column headers if columns present
                if (dgvThuChi.Columns.Contains("MaGD")) dgvThuChi.Columns["MaGD"].HeaderText = "Mã GD";
                if (dgvThuChi.Columns.Contains("LoaiGD")) dgvThuChi.Columns["LoaiGD"].HeaderText = "Loại";
                if (dgvThuChi.Columns.Contains("SoTien")) dgvThuChi.Columns["SoTien"].HeaderText = "Số tiền";
                if (dgvThuChi.Columns.Contains("NgayGD")) dgvThuChi.Columns["NgayGD"].HeaderText = "Ngày";
                if (dgvThuChi.Columns.Contains("NoiDung")) dgvThuChi.Columns["NoiDung"].HeaderText = "Nội dung";

                // show person name instead of id when available
                if (dgvThuChi.Columns.Contains("NguoiThucHienName"))
                {
                    dgvThuChi.Columns["NguoiThucHienName"].HeaderText = "Người thực hiện";
                }

                // hide internal id columns not needed in UI
                if (dgvThuChi.Columns.Contains("MaNguon")) dgvThuChi.Columns["MaNguon"].Visible = false;
                if (dgvThuChi.Columns.Contains("MaHD")) dgvThuChi.Columns["MaHD"].Visible = false;

                // hide id and status columns (show name via NguoiThucHienName)
                if (dgvThuChi.Columns.Contains("NguoiThucHien")) dgvThuChi.Columns["NguoiThucHien"].Visible = false;
                if (dgvThuChi.Columns.Contains("TrangThai")) dgvThuChi.Columns["TrangThai"].Visible = false;

                // adjust specific column widths if present to make layout nicer
                if (dgvThuChi.Columns.Contains("MaGD")) dgvThuChi.Columns["MaGD"].FillWeight =40;
                if (dgvThuChi.Columns.Contains("LoaiGD")) dgvThuChi.Columns["LoaiGD"].FillWeight =60;
                if (dgvThuChi.Columns.Contains("SoTien")) dgvThuChi.Columns["SoTien"].FillWeight =120;
                if (dgvThuChi.Columns.Contains("NoiDung")) dgvThuChi.Columns["NoiDung"].FillWeight =200;
                if (dgvThuChi.Columns.Contains("NguoiThucHienName")) dgvThuChi.Columns["NguoiThucHienName"].FillWeight =140;
            }
            catch { }
        }

        private void ApplyFilterAndSearch()
        {
            try
            {
                var filters = new System.Collections.Generic.List<string>();

                var f = (cmbFilter.SelectedItem as string) ?? "Tất cả";
                if (f == "Thu" || f == "Chi")
                {
                    filters.Add("LoaiGD = '" + f.Replace("'", "''") + "'");
                }

                var q = txtSearch.Text.Trim();
                currentFilter = q;
                if (!string.IsNullOrEmpty(q))
                {
                    var s = q.Replace("'", "''");
                    filters.Add($"(NoiDung LIKE '%{s}%')");
                }

                // Create filtered view of full table (not paged)
                thuChiView = new DataView(thuChiTable);
                thuChiView.RowFilter = string.Join(" AND ", filters);

                // Update pagination counts based on filtered view
                totalRecords = thuChiView.Count;
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                if (totalPages ==0) totalPages =1;

                if (currentPage > totalPages) currentPage = totalPages;
                if (currentPage <1) currentPage =1;

                // Build a paged DataTable for the grid
                var pageTable = thuChiView.ToTable().Clone();
                int start = (currentPage -1) * pageSize;
                int end = Math.Min(start + pageSize, thuChiView.Count);

                for (int i = start; i < end; i++)
                {
                    pageTable.ImportRow(thuChiView[i].Row);
                }

                dgvThuChi.DataSource = pageTable;

                // Apply column settings again because DataSource changed
                ApplyColumnSettings();

                UpdatePaginationControls();
                UpdateChart();
            }
            catch (Exception ex)
            {
                // ignore filter errors
                System.Diagnostics.Debug.WriteLine("Filter error: " + ex.Message);
            }
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            using (var form = new TransactionForm(connectionString))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var conn = new SqlConnection(connectionString))
                        using (var cmd = new SqlCommand(@"INSERT INTO ThuChi (LoaiGD, SoTien, NgayGD, NoiDung, NguoiThucHien, MaNguon, MaHD, TrangThai) 
VALUES (@Loai, @SoTien, @Ngay, @NoiDung, @Nguoi, @Nguon, @MaHD, @Trang)", conn))
                        {
                            cmd.Parameters.AddWithValue("@Loai", form.Loai);
                            cmd.Parameters.AddWithValue("@SoTien", form.SoTien);
                            cmd.Parameters.AddWithValue("@Ngay", form.Ngay);
                            cmd.Parameters.AddWithValue("@NoiDung", (object)form.NoiDung ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Nguoi", form.NguoiThucHienId.HasValue ? (object)form.NguoiThucHienId.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@Nguon", DBNull.Value);
                            cmd.Parameters.AddWithValue("@MaHD", DBNull.Value);
                            cmd.Parameters.AddWithValue("@Trang", "Chờ duyệt");

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }

                        // reset to first page after creating new
                        currentPage =1;
                        LoadDataFromDb();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi tạo giao dịch: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvThuChi.SelectedRows.Count ==0) return;
            var rowView = dgvThuChi.SelectedRows[0].DataBoundItem as DataRowView;
            if (rowView == null) return;

            var id = Convert.ToInt32(rowView["MaGD"]);
            var loai = rowView["LoaiGD"]?.ToString();
            var sotien = Convert.ToDecimal(rowView["SoTien"]);
            var ngay = rowView["NgayGD"] != DBNull.Value ? Convert.ToDateTime(rowView["NgayGD"]) : DateTime.Today;
            var noidung = rowView["NoiDung"]?.ToString();
            int? nguoiId = null;
            if (rowView.Row.Table.Columns.Contains("NguoiThucHien") && rowView["NguoiThucHien"] != DBNull.Value)
                nguoiId = Convert.ToInt32(rowView["NguoiThucHien"]);

            using (var form = new TransactionForm(connectionString, loai, sotien, ngay, noidung, nguoiId))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var conn = new SqlConnection(connectionString))
                        using (var cmd = new SqlCommand(@"UPDATE ThuChi SET LoaiGD=@Loai, SoTien=@SoTien, NgayGD=@Ngay, NoiDung=@NoiDung, NguoiThucHien=@Nguoi WHERE MaGD=@MaGD", conn))
                        {
                            cmd.Parameters.AddWithValue("@Loai", form.Loai);
                            cmd.Parameters.AddWithValue("@SoTien", form.SoTien);
                            cmd.Parameters.AddWithValue("@Ngay", form.Ngay);
                            cmd.Parameters.AddWithValue("@NoiDung", (object)form.NoiDung ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Nguoi", form.NguoiThucHienId.HasValue ? (object)form.NguoiThucHienId.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@MaGD", id);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }

                        LoadDataFromDb();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi cập nhật giao dịch: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvThuChi.SelectedRows.Count ==0) return;
            var rowView = dgvThuChi.SelectedRows[0].DataBoundItem as DataRowView;
            if (rowView == null) return;

            var id = Convert.ToInt32(rowView["MaGD"]);
            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa giao dịch này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("DELETE FROM ThuChi WHERE MaGD=@MaGD", conn))
                {
                    cmd.Parameters.AddWithValue("@MaGD", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadDataFromDb();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa giao dịch: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            // perform same filter/search as typing
            currentPage =1; // reset to first page on search
            ApplyFilterAndSearch();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            // reset search and filter and reload data
            txtSearch.Clear();
            if (cmbFilter.Items.Contains("Tất cả")) cmbFilter.SelectedItem = "Tất cả";
            currentPage =1;
            LoadDataFromDb();
        }

        private void UpdateChart()
        {
            try
            {
                // clear existing controls in the chart panel
                chartSummary.Controls.Clear();

                // create first chart (pie) - summary Thu/Chi
                var chart = new Chart();
                chart.Dock = DockStyle.Fill;
                chart.BackColor = System.Drawing.Color.WhiteSmoke;

                var chartArea = new ChartArea("MainArea");
                chartArea.BackColor = System.Drawing.Color.Transparent;
                chart.ChartAreas.Add(chartArea);

                var legend = new Legend("Legend");
                legend.Docking = Docking.Bottom;
                chart.Legends.Add(legend);

                var series = new Series("ThuChiSeries");
                series.ChartType = SeriesChartType.Pie;
                series.ChartArea = "MainArea";
                series.IsValueShownAsLabel = true;
                series.LabelFormat = "N0";

                // compute totals for Thu and Chi
                var dt = thuChiView?.ToTable() ?? new DataTable();
                decimal sumChi =0m;
                decimal sumThu =0m;

                if (dt.Rows.Count >0)
                {
                    try
                    {
                        sumChi = dt.AsEnumerable()
                            .Where(r => string.Equals(r.Field<string>("LoaiGD"), "Chi", StringComparison.OrdinalIgnoreCase))
                            .Sum(r => Convert.ToDecimal(r.Field<object>("SoTien")));
                    }
                    catch { sumChi =0m; }

                    try
                    {
                        sumThu = dt.AsEnumerable()
                            .Where(r => string.Equals(r.Field<string>("LoaiGD"), "Thu", StringComparison.OrdinalIgnoreCase))
                            .Sum(r => Convert.ToDecimal(r.Field<object>("SoTien")));
                    }
                    catch { sumThu =0m; }
                }

                // add data points
                series.Points.Clear();
                series.Points.AddXY("Chi", sumChi);
                series.Points.AddXY("Thu", sumThu);

                // format labels and tooltips
                foreach (DataPoint p in series.Points)
                {
                    p.Label = string.Format("{0}: {1:N0}", p.AxisLabel, p.YValues[0]);
                    p.ToolTip = string.Format("{0}: {1:N0}", p.AxisLabel, p.YValues[0]);
                }

                chart.Series.Add(series);

                // create second chart (column) - monthly Thu/Chi for last6 months
                var chart2 = new Chart();
                chart2.Dock = DockStyle.Fill;
                chart2.BackColor = System.Drawing.Color.WhiteSmoke;

                var monthArea = new ChartArea("MonthArea");
                monthArea.BackColor = System.Drawing.Color.Transparent;
                monthArea.AxisX.Interval =1;
                monthArea.AxisX.LabelStyle.Angle = -45;
                chart2.ChartAreas.Add(monthArea);

                var legend2 = new Legend("Legend2");
                legend2.Docking = Docking.Top;
                chart2.Legends.Add(legend2);

                var seriesThuMonthly = new Series("Thu") { ChartType = SeriesChartType.Column, ChartArea = "MonthArea", IsValueShownAsLabel = true, LabelFormat = "N0" };
                var seriesChiMonthly = new Series("Chi") { ChartType = SeriesChartType.Column, ChartArea = "MonthArea", IsValueShownAsLabel = true, LabelFormat = "N0" };

                // prepare last6 months (oldest -> newest)
                var months = Enumerable.Range(0,6)
                    .Select(i => new DateTime(DateTime.Today.AddMonths(-5 + i).Year, DateTime.Today.AddMonths(-5 + i).Month,1))
                    .ToList();

                foreach (var m in months)
                {
                    decimal mThu =0m;
                    decimal mChi =0m;

                    try
                    {
                        var rows = dt.AsEnumerable()
                            .Where(r => r["NgayGD"] != DBNull.Value)
                            .Where(r =>
                            {
                                DateTime d;
                                try { d = Convert.ToDateTime(r["NgayGD"]); } catch { return false; }
                                return d.Year == m.Year && d.Month == m.Month;
                            });

                        mThu = rows
                            .Where(r => string.Equals(r.Field<string>("LoaiGD"), "Thu", StringComparison.OrdinalIgnoreCase))
                            .Sum(r => Convert.ToDecimal(r.Field<object>("SoTien")));

                        mChi = rows
                            .Where(r => string.Equals(r.Field<string>("LoaiGD"), "Chi", StringComparison.OrdinalIgnoreCase))
                            .Sum(r => Convert.ToDecimal(r.Field<object>("SoTien")));
                    }
                    catch { mThu =0m; mChi =0m; }

                    var label = m.ToString("MM/yy");
                    seriesThuMonthly.Points.AddXY(label, mThu);
                    seriesChiMonthly.Points.AddXY(label, mChi);
                }

                chart2.Series.Add(seriesThuMonthly);
                chart2.Series.Add(seriesChiMonthly);

                // arrange two charts side-by-side using a TableLayoutPanel
                var container = new TableLayoutPanel();
                container.Dock = DockStyle.Fill;
                container.ColumnCount =2;
                container.RowCount =1;
                container.ColumnStyles.Clear();
                container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50f));
                container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50f));
                container.RowStyles.Clear();
                container.RowStyles.Add(new RowStyle(SizeType.Percent,100f));

                container.Controls.Add(chart,0,0);
                container.Controls.Add(chart2,1,0);

                chartSummary.Controls.Add(container);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Chart update error: " + ex.Message);
            }
        }

        // Pagination UI update
        private void UpdatePaginationControls()
        {
            try
            {
                if (lblPageInfo != null)
                {
                    lblPageInfo.Text = $"Trang {currentPage} / {totalPages} (Tổng: {totalRecords} giao dịch)";
                }

                if (btnPreviousPage != null) btnPreviousPage.Enabled = currentPage >1;
                if (btnNextPage != null) btnNextPage.Enabled = currentPage < totalPages;

                if (btnPreviousPage != null)
                {
                    btnPreviousPage.BackColor = btnPreviousPage.Enabled
                        ? System.Drawing.Color.FromArgb(52,152,219)
                        : System.Drawing.Color.FromArgb(189,195,199);
                }
                if (btnNextPage != null)
                {
                    btnNextPage.BackColor = btnNextPage.Enabled
                        ? System.Drawing.Color.FromArgb(52,152,219)
                        : System.Drawing.Color.FromArgb(189,195,199);
                }
            }
            catch { }
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            if (currentPage >1)
            {
                currentPage--;
                ApplyFilterAndSearch();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                ApplyFilterAndSearch();
            }
        }

        private void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(cboPageSize.SelectedItem?.ToString(), out int newPageSize))
            {
                pageSize = newPageSize;
                currentPage =1; // reset to first page
                ApplyFilterAndSearch();
            }
        }

        // small modal form for creating/editing a ThuChi
        private class TransactionForm : Form
        {
            public string Loai { get; private set; }
            public decimal SoTien { get; private set; }
            public DateTime Ngay { get; private set; }
            public string NoiDung { get; private set; }
            public int? NguoiThucHienId { get; private set; }

            private ComboBox cmbLoai;
            private NumericUpDown nudSoTien;
            private DateTimePicker dtpNgay;
            private TextBox txtNoiDung;
            private ComboBox cmbNguoi;
            private Button btnOk;
            private Button btnCancel;

            public TransactionForm(string connStr, string loai = "Thu", decimal soTien =0, DateTime? ngay = null, string noiDung = "", int? nguoiId = null)
            {
                this.Text = "Giao dịch";
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.StartPosition = FormStartPosition.CenterParent;
                this.ClientSize = new System.Drawing.Size(520,260);
                this.MinimizeBox = false;
                this.MaximizeBox = false;

                var lblLoai = new Label() { Text = "Loại:", Left =12, Top =12, Width =80 };
                cmbLoai = new ComboBox() { Left =100, Top =10, Width =120, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbLoai.Items.AddRange(new string[] { "Thu", "Chi" });

                var lblSoTien = new Label() { Text = "Số tiền:", Left =12, Top =52, Width =80 };
                nudSoTien = new NumericUpDown() { Left =100, Top =50, Width =120, Maximum =1000000000, DecimalPlaces =0, ThousandsSeparator = true };

                var lblNgay = new Label() { Text = "Ngày:", Left =12, Top =92, Width =80 };
                dtpNgay = new DateTimePicker() { Left =100, Top =90, Width =200, Format = DateTimePickerFormat.Short };

                var lblNoiDung = new Label() { Text = "Nội dung:", Left =12, Top =132, Width =80 };
                txtNoiDung = new TextBox() { Left =100, Top =130, Width =380 };

                var lblNguoi = new Label() { Text = "Người thực hiện:", Left =12, Top =170, Width =100 };
                cmbNguoi = new ComboBox() { Left =120, Top =168, Width =280, DropDownStyle = ComboBoxStyle.DropDownList };

                btnOk = new Button() { Text = "OK", Left =340, Width =80, Top =200, DialogResult = DialogResult.OK };
                btnCancel = new Button() { Text = "Hủy", Left =430, Width =80, Top =200, DialogResult = DialogResult.Cancel };

                this.Controls.AddRange(new Control[] { lblLoai, cmbLoai, lblSoTien, nudSoTien, lblNgay, dtpNgay, lblNoiDung, txtNoiDung, lblNguoi, cmbNguoi, btnOk, btnCancel });

                // init values
                cmbLoai.SelectedItem = loai == "Chi" ? "Chi" : "Thu";
                nudSoTien.Value = soTien >=0 ? soTien :0;
                dtpNgay.Value = ngay ?? DateTime.Today;
                txtNoiDung.Text = noiDung ?? string.Empty;

                // ensure negative values cannot be entered
                nudSoTien.Minimum =0; // prevent negative numbers
                // also suppress typing the '-' character
                nudSoTien.KeyPress += NudSoTien_KeyPress;

                // load members into cmbNguoi
                try
                {
                    var members = new DataTable();
                    members.Columns.Add("MaTV", typeof(int));
                    members.Columns.Add("HoTen", typeof(string));

                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = new SqlCommand("SELECT MaTV, HoTen FROM ThanhVien ORDER BY HoTen", conn))
                    {
                        conn.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                members.Rows.Add(Convert.ToInt32(rdr["MaTV"]), rdr["HoTen"].ToString());
                            }
                            }
                     }

                    var topRow = members.NewRow();
                    topRow["MaTV"] = DBNull.Value;
                    topRow["HoTen"] = "(Không chọn)";
                    members.Rows.InsertAt(topRow,0);

                    cmbNguoi.DisplayMember = "HoTen";
                    cmbNguoi.ValueMember = "MaTV";
                    cmbNguoi.DataSource = members;

                    if (nguoiId.HasValue)
                    {
                        try { cmbNguoi.SelectedValue = nguoiId.Value; } catch { }
                    }
                }
                catch { }

                this.AcceptButton = btnOk;
                this.CancelButton = btnCancel;

                btnOk.Click += BtnOk_Click;
            }

            private void NudSoTien_KeyPress(object sender, KeyPressEventArgs e)
            {
                // Block '-' so user cannot type negative sign
                if (e.KeyChar == '-')
                {
                    e.Handled = true;
                }
            }

            private void BtnOk_Click(object sender, EventArgs e)
            {
                if (cmbLoai.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn loại (Thu/Chi).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                    return;
                }

                if (nudSoTien.Value <=0)
                {
                    MessageBox.Show("Số tiền phải lớn hơn0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                    return;
                }

                Loai = cmbLoai.SelectedItem.ToString();
                SoTien = nudSoTien.Value;
                Ngay = dtpNgay.Value.Date;
                NoiDung = txtNoiDung.Text.Trim();

                if (cmbNguoi.SelectedIndex <=0)
                    NguoiThucHienId = null;
                 else
                {
                    try { NguoiThucHienId = cmbNguoi.SelectedValue != null && cmbNguoi.SelectedValue != DBNull.Value ? (int?)Convert.ToInt32(cmbNguoi.SelectedValue) : null; }
                    catch { NguoiThucHienId = null; }
                }
            }
        }
        private void dgvThuChi_CellContentClick(object sender, DataGridViewCellEventArgs e)
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