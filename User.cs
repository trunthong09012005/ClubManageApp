using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class ucUser : UserControl
    {
        private BindingList<Participant> users = new BindingList<Participant>();

        // store rectangles of drawn columns for hover detection
        private List<KeyValuePair<Rectangle, string>> classColumnRects = new List<KeyValuePair<Rectangle, string>>();
        private Label hoverLabel;

        public ucUser()
        {
            InitializeComponent();
            InitializeDemoData();
            WireGrid();

            // prepare hover label
            hoverLabel = new Label()
            {
                AutoSize = true,
                Visible = false,
                BackColor = Color.FromArgb(230, Color.White),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
            };

            if (chartByClass != null)
            {
                chartByClass.Controls.Add(hoverLabel);
                chartByClass.MouseMove += ChartByClass_MouseMove;
                chartByClass.MouseLeave += (s, e) => { hoverLabel.Visible = false; };
                chartByClass.SizeChanged += (s, e) => PopulateCharts();
            }

            if (chartGender != null)
            {
                chartGender.SizeChanged += (s, e) => PopulateCharts();
            }

            PopulateCharts();
        }

        private void InitializeDemoData()
        {
            // sample users - expanded to include more columns matching ThanhVien
            users.Add(new Participant { Id = 1, HoTen = "Nguyễn Vương Khang", NgaySinh = DateTime.Parse("2004-02-15"), GioiTinh = "Nam", Lop = "DHKTPM17A", Khoa = "Công nghệ thông tin", SDT = "0912345678", Email = "huytm@student.hcmute.edu.vn", DiaChi = "Quận 7, TP.HCM", VaiTro = "Chủ nhiệm", MaCV = 1, MaBan = null });
            users.Add(new Participant { Id = 2, HoTen = "Nguyễn Thị Lan", NgaySinh = DateTime.Parse("2005-05-10"), GioiTinh = "Nữ", Lop = "DHKTPM17A", Khoa = "Công nghệ thông tin", SDT = "0987654321", Email = "lannt@student.hcmute.edu.vn", DiaChi = "Quận 5, TP.HCM", VaiTro = "Phó chủ nhiệm", MaCV = 2, MaBan = null });
            users.Add(new Participant { Id = 3, HoTen = "Lê Quốc Bảo", NgaySinh = DateTime.Parse("2005-09-21"), GioiTinh = "Nam", Lop = "DHKTPM17B", Khoa = "Công nghệ thông tin", SDT = "0977112233", Email = "baolq@student.hcmute.edu.vn", DiaChi = "Quận 10, TP.HCM", VaiTro = "Trưởng ban", MaCV = 4, MaBan = 1 });
            users.Add(new Participant { Id = 4, HoTen = "Phạm Thị Mai", NgaySinh = DateTime.Parse("2005-03-18"), GioiTinh = "Nữ", Lop = "DHKTPM17B", Khoa = "Công nghệ thông tin", SDT = "0965432109", Email = "maipt@student.hcmute.edu.vn", DiaChi = "Quận 3, TP.HCM", VaiTro = "Trưởng ban", MaCV = 4, MaBan = 2 });
            users.Add(new Participant { Id = 5, HoTen = "Hoàng Văn Nam", NgaySinh = DateTime.Parse("2004-11-25"), GioiTinh = "Nam", Lop = "DHKTPM17C", Khoa = "Công nghệ thông tin", SDT = "0923456789", Email = "namhv@student.hcmute.edu.vn", DiaChi = "Quận 1, TP.HCM", VaiTro = "Trưởng ban", MaCV = 4, MaBan = 3 });
        }

        private void WireGrid()
        {
            dgvUsers.DataSource = users;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.Columns.Cast<DataGridViewColumn>().ToList().ForEach(c => c.Visible = true);

            // Show only useful columns that correspond to ThanhVien table
            var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Id", "HoTen", "NgaySinh", "GioiTinh", "Lop", "Khoa", "SDT", "Email", "DiaChi", "VaiTro"
            };

            foreach (DataGridViewColumn c in dgvUsers.Columns)
            { 
                if (allowed.Contains(c.Name))
                {
                    // Set friendly headers
                    switch (c.Name)
                    {
                        case "Id": c.HeaderText = "MaTV"; break;
                        case "HoTen": c.HeaderText = "Họ tên"; break;
                        case "NgaySinh": c.HeaderText = "Ngày sinh"; break;
                        case "GioiTinh": c.HeaderText = "Giới tính"; break;
                        case "Lop": c.HeaderText = "Lớp"; break;
                        case "Khoa": c.HeaderText = "Khoa"; break;
                        case "SDT": c.HeaderText = "SĐT"; break;
                        case "Email": c.HeaderText = "Email"; break;
                        case "DiaChi": c.HeaderText = "Địa chỉ"; break;
                        case "VaiTro": c.HeaderText = "Vai trò"; break;
                        default: c.HeaderText = c.Name; break;
                    }

                    c.Visible = true;
                }
                else
                {
                    c.Visible = false;
                }
            }
        }

        private void PopulateCharts()
        {
            // Safeguard: ensure chart panels exist
            if (chartGender == null || chartByClass == null)
                return;

            // Clear previous visuals
            chartGender.Controls.Clear();
            chartGender.BackgroundImage = null;
            chartByClass.Controls.Clear();
            // re-add hoverLabel into chartByClass controls
            chartByClass.Controls.Add(hoverLabel);
            classColumnRects.Clear();

            if (users == null || users.Count == 0)
            {
                var lblEmpty = new Label() { Dock = DockStyle.Fill, Text = "Không có dữ liệu", TextAlign = ContentAlignment.MiddleCenter };
                chartGender.Controls.Add(lblEmpty);
                chartByClass.Controls.Add(new Label() { Dock = DockStyle.Fill, Text = "Không có dữ liệu", TextAlign = ContentAlignment.MiddleCenter });
                return;
            }

            // ----- Gender Pie Chart -----
            int male = users.Count(u => string.Equals((u.GioiTinh ?? "").Trim(), "Nam", StringComparison.OrdinalIgnoreCase));
            int female = users.Count(u => string.Equals((u.GioiTinh ?? "").Trim(), "Nữ", StringComparison.OrdinalIgnoreCase));
            int other = users.Count() - male - female;

            Color maleColor = Color.FromArgb(94, 148, 255);
            Color femaleColor = Color.FromArgb(255, 105, 180);
            Color otherColor = Color.FromArgb(200, 200, 200);

            // create bitmap matching control size to avoid layout scaling
            int gw = Math.Max(1, chartGender.ClientSize.Width);
            int gh = Math.Max(1, chartGender.ClientSize.Height);
            var gbmp = new Bitmap(gw, gh);
            using (var g = Graphics.FromImage(gbmp))
            {
                g.Clear(chartGender.BackColor);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // compute pie rectangle (square) and reserved legend area on right if wide enough
                int pieSize = Math.Min(gw, gh) - 40;
                if (pieSize < 10) pieSize = Math.Min(gw, gh);
                var pieRect = new Rectangle(10, 10, Math.Max(0, pieSize), Math.Max(0, pieSize));

                float total = Math.Max(1, male + female + other);
                float start = 0f;

                if (male > 0)
                {
                    float sweep = male / total * 360f;
                    using (var b = new SolidBrush(maleColor)) g.FillPie(b, pieRect, start, sweep);
                    start += sweep;
                }
                if (female > 0)
                {
                    float sweep = female / total * 360f;
                    using (var b = new SolidBrush(femaleColor)) g.FillPie(b, pieRect, start, sweep);
                    start += sweep;
                }
                if (other > 0)
                {
                    float sweep = other / total * 360f;
                    using (var b = new SolidBrush(otherColor)) g.FillPie(b, pieRect, start, sweep);
                    start += sweep;
                }

                // legend to the right of pie if space, otherwise below
                using (var f = new Font("Segoe UI", 9))
                using (var tfont = new Font("Segoe UI", 10, FontStyle.Bold))
                {
                    int legendX = pieRect.Right + 12;
                    int legendY = pieRect.Top;
                    int box = 14;

                    if (legendX + 100 < gw) // enough width for right-side legend
                    {
                        if (male > 0) { g.FillRectangle(new SolidBrush(maleColor), legendX, legendY, box, box); g.DrawString($"Nam: {male}", f, Brushes.Black, legendX + box + 6, legendY - 2); legendY += 20; }
                        if (female > 0) { g.FillRectangle(new SolidBrush(femaleColor), legendX, legendY, box, box); g.DrawString($"Nữ: {female}", f, Brushes.Black, legendX + box + 6, legendY - 2); legendY += 20; }
                        if (other > 0) { g.FillRectangle(new SolidBrush(otherColor), legendX, legendY, box, box); g.DrawString($"Khác: {other}", f, Brushes.Black, legendX + box + 6, legendY - 2); }

                        g.DrawString("Thống kê giới tính", tfont, Brushes.Black, 10, pieRect.Bottom + 6);
                    }
                    else // draw legend below
                    {
                        int ly = pieRect.Bottom + 8;
                        int lx = 10;
                        if (male > 0) { g.FillRectangle(new SolidBrush(maleColor), lx, ly, box, box); g.DrawString($"Nam: {male}", f, Brushes.Black, lx + box + 6, ly - 2); lx += 120; }
                        if (female > 0) { g.FillRectangle(new SolidBrush(femaleColor), lx, ly, box, box); g.DrawString($"Nữ: {female}", f, Brushes.Black, lx + box + 6, ly - 2); lx += 120; }
                        if (other > 0) { g.FillRectangle(new SolidBrush(otherColor), lx, ly, box, box); g.DrawString($"Khác: {other}", f, Brushes.Black, lx + box + 6, ly - 2); }

                        g.DrawString("Thống kê giới tính", tfont, Brushes.Black, 10, ly + 24);
                    }
                }
            }

            chartGender.BackgroundImage = gbmp;
            chartGender.BackgroundImageLayout = ImageLayout.Stretch;

            // ----- Class (vertical column) Chart -----
            var byClass = users.GroupBy(u => string.IsNullOrWhiteSpace(u.Lop) ? "Chưa biết" : u.Lop)
                               .Select(g => new { Class = g.Key, Count = g.Count() })
                               .OrderByDescending(x => x.Count)
                               .Take(10)
                               .ToList();

            int cw = Math.Max(1, chartByClass.ClientSize.Width);
            int chPanel = Math.Max(1, chartByClass.ClientSize.Height);
            var cbmp = new Bitmap(cw, chPanel);
            using (var g = Graphics.FromImage(cbmp))
            {
                g.Clear(chartByClass.BackColor);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                int marginLeft = 60;
                int marginRight = 20;
                int marginTop = 30;
                int marginBottom = 80; // space for rotated labels (not drawn)

                int plotW = Math.Max(10, cw - marginLeft - marginRight);
                int plotH = Math.Max(10, chPanel - marginTop - marginBottom);

                int maxCount = byClass.Any() ? Math.Max(1, byClass.Max(x => x.Count)) : 1;
                int colCount = byClass.Count;

                if (colCount == 0)
                {
                    g.DrawString("Không có dữ liệu", new Font("Segoe UI", 10), Brushes.Black, 6, 6);
                }
                else
                {
                    int gap = Math.Max(8, plotW / (colCount * 8));
                    int colWidth = Math.Max(24, (plotW - gap * (colCount + 1)) / colCount);
                    int x = marginLeft + gap;

                    var colBrush = new SolidBrush(Color.FromArgb(94, 148, 255));
                    var axisPen = new Pen(Color.DarkGray, 1);
                    var labelFont = new Font("Segoe UI", 9);
                    var titleFont = new Font("Segoe UI", 10, FontStyle.Bold);

                    // draw axes
                    g.DrawLine(axisPen, marginLeft, marginTop + plotH, marginLeft + plotW, marginTop + plotH); // x-axis
                    g.DrawLine(axisPen, marginLeft, marginTop, marginLeft, marginTop + plotH); // y-axis

                    foreach (var item in byClass)
                    {
                        double ratio = item.Count / (double)maxCount;
                        int colHeight = (int)Math.Round(ratio * plotH);
                        var rect = new Rectangle(x, marginTop + (plotH - colHeight), colWidth, colHeight);

                        g.FillRectangle(colBrush, rect);
                        g.DrawRectangle(Pens.Black, rect);

                        // draw count above column
                        var countText = item.Count.ToString();
                        var size = g.MeasureString(countText, labelFont);
                        g.DrawString(countText, labelFont, Brushes.Black, rect.Left + (rect.Width - size.Width) / 2, rect.Top - size.Height - 4);

                        // store rect+label for hover (coordinates are in control space since bitmap equals control size)
                        classColumnRects.Add(new KeyValuePair<Rectangle, string>(rect, item.Class));

                        x += colWidth + gap;
                    }

                    // draw y-axis grid and ticks
                    int ticks = 5;
                    for (int i = 0; i <= ticks; i++)
                    {
                        double val = i / (double)ticks * maxCount;
                        int ytick = marginTop + plotH - (int)Math.Round(i / (double)ticks * plotH);
                        g.DrawLine(Pens.LightGray, marginLeft, ytick, marginLeft + plotW, ytick);
                        var txt = Math.Round(val).ToString();
                        var sz = g.MeasureString(txt, labelFont);
                        g.DrawString(txt, labelFont, Brushes.Black, marginLeft - sz.Width - 6, ytick - sz.Height / 2);
                    }

                    // Title
                    g.DrawString("Thống kê theo lớp (Top 10)", titleFont, Brushes.Black, marginLeft, chPanel - marginBottom + 8);

                    axisPen.Dispose();
                    colBrush.Dispose();
                    labelFont.Dispose();
                    titleFont.Dispose();
                }
            }

            chartByClass.BackgroundImage = cbmp;
            chartByClass.BackgroundImageLayout = ImageLayout.Stretch;

            // ensure hover label is on top
            hoverLabel.BringToFront();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            var q = (txtSearch.Text ?? string.Empty).Trim().ToLower();
            if (string.IsNullOrEmpty(q))
            {
                dgvUsers.DataSource = users;
            }
            else
            {
                var filtered = users.Where(u =>
                    (u.HoTen ?? string.Empty).ToLower().Contains(q) ||
                    (u.Lop ?? string.Empty).ToLower().Contains(q) ||
                    (u.Email ?? string.Empty).ToLower().Contains(q) ||
                    (u.SDT ?? string.Empty).ToLower().Contains(q) ||
                    (u.DiaChi ?? string.Empty).ToLower().Contains(q) ||
                    (u.Khoa ?? string.Empty).ToLower().Contains(q) ||
                    (u.VaiTro ?? string.Empty).ToLower().Contains(q)
                ).ToList();

                dgvUsers.DataSource = new BindingList<Participant>(filtered);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new UserEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                var nextId = (users.Count == 0) ? 1 : users.Max(x => x.Id) + 1;
                form.User.Id = nextId;
                users.Add(form.User);
                PopulateCharts();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) return;
            var selected = (Participant)dgvUsers.CurrentRow.DataBoundItem;
            var form = new UserEditForm(selected);
            if (form.ShowDialog() == DialogResult.OK)
            {
                // copy all editable properties back to selected
                selected.HoTen = form.User.HoTen;
                selected.NgaySinh = form.User.NgaySinh;
                selected.GioiTinh = form.User.GioiTinh;
                selected.Lop = form.User.Lop;
                selected.Khoa = form.User.Khoa;
                selected.SDT = form.User.SDT;
                selected.Email = form.User.Email;
                selected.DiaChi = form.User.DiaChi;
                selected.VaiTro = form.User.VaiTro;
                selected.MaCV = form.User.MaCV;
                selected.MaBan = form.User.MaBan;

                dgvUsers.Refresh();
                PopulateCharts();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) return;
            var selected = (Participant)dgvUsers.CurrentRow.DataBoundItem;
            var r = MessageBox.Show($"Xóa thành viên '{selected.HoTen}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (r == DialogResult.Yes)
            {
                users.Remove(selected);
                PopulateCharts();
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) return;
            var selected = (Participant)dgvUsers.CurrentRow.DataBoundItem;
            var info = new StringBuilder();
            info.AppendLine($"MaTV: {selected.Id}");
            info.AppendLine($"Họ tên: {selected.HoTen}");
            info.AppendLine($"Ngày sinh: {(selected.NgaySinh == default(DateTime) ? "" : selected.NgaySinh.ToString("dd/MM/yyyy"))}");
            info.AppendLine($"Giới tính: {selected.GioiTinh}");
            info.AppendLine($"Lớp: {selected.Lop}");
            info.AppendLine($"Khoa: {selected.Khoa}");
            info.AppendLine($"SĐT: {selected.SDT}");
            info.AppendLine($"Email: {selected.Email}");
            info.AppendLine($"Địa chỉ: {selected.DiaChi}");
            info.AppendLine($"Vai trò: {selected.VaiTro}");
            info.AppendLine($"MaCV: {selected.MaCV}");
            info.AppendLine($"MaBan: {(selected.MaBan.HasValue ? selected.MaBan.Value.ToString() : "")}");

            MessageBox.Show(info.ToString(), "Chi tiết thành viên");
        }

        private void ChartByClass_MouseMove(object sender, MouseEventArgs e)
        {
            if (classColumnRects == null || classColumnRects.Count == 0)
            {
                hoverLabel.Visible = false;
                return;
            }

            // find first rect under mouse
            var hit = classColumnRects.FirstOrDefault(kv => kv.Key.Contains(e.Location));
            if (!hit.Equals(default(KeyValuePair<Rectangle, string>)))
            {
                hoverLabel.Text = hit.Value;
                // place label above mouse, adjust to remain inside control
                int x = e.X + 12;
                int y = e.Y - hoverLabel.Height - 8;
                if (x + hoverLabel.Width > chartByClass.ClientSize.Width) x = chartByClass.ClientSize.Width - hoverLabel.Width - 6;
                if (y < 0) y = e.Y + 12;

                hoverLabel.Location = new Point(Math.Max(4, x), Math.Max(4, y));
                hoverLabel.Visible = true;
                hoverLabel.BringToFront();
            }
            else
            {
                hoverLabel.Visible = false;
            }
        }

        // small edit form for user (inside same namespace so Participant type is available)
        public class UserEditForm : Form
        {
            public Participant User { get; private set; }

            private TextBox txtHoTen;
            private DateTimePicker dtpNgaySinh;
            private ComboBox cboGioiTinh;
            private TextBox txtLop;
            private TextBox txtKhoa;
            private TextBox txtSDT;
            private TextBox txtEmail;
            private TextBox txtDiaChi;
            private TextBox txtVaiTro;
            private NumericUpDown nudMaCV;
            private NumericUpDown nudMaBan;
            private Button btnOk;
            private Button btnCancel;

            public UserEditForm()
            {
                InitializeForm();
                User = new Participant();
            }

            public UserEditForm(Participant p) : this()
            {
                User = new Participant
                {
                    Id = p.Id,
                    HoTen = p.HoTen,
                    NgaySinh = p.NgaySinh,
                    GioiTinh = p.GioiTinh,
                    Lop = p.Lop,
                    Khoa = p.Khoa,
                    SDT = p.SDT,
                    Email = p.Email,
                    DiaChi = p.DiaChi,
                    VaiTro = p.VaiTro,
                    MaCV = p.MaCV,
                    MaBan = p.MaBan
                };

                // populate controls
                txtHoTen.Text = User.HoTen;
                if (User.NgaySinh != default(DateTime)) dtpNgaySinh.Value = User.NgaySinh;
                cboGioiTinh.SelectedItem = User.GioiTinh;
                txtLop.Text = User.Lop;
                txtKhoa.Text = User.Khoa;
                txtSDT.Text = User.SDT;
                txtEmail.Text = User.Email;
                txtDiaChi.Text = User.DiaChi;
                txtVaiTro.Text = User.VaiTro;
                nudMaCV.Value = Math.Max(nudMaCV.Minimum, Math.Min(nudMaCV.Maximum, User.MaCV));
                nudMaBan.Value = User.MaBan.HasValue ? Math.Max(nudMaBan.Minimum, Math.Min(nudMaBan.Maximum, User.MaBan.Value)) : 0;
            }

            private void InitializeForm()
            {
                this.Text = "Thành viên";
                this.ClientSize = new Size(520, 420);
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.StartPosition = FormStartPosition.CenterParent;

                txtHoTen = new TextBox() { Left = 140, Top = 20, Width = 340 };
                dtpNgaySinh = new DateTimePicker() { Left = 140, Top = 60, Width = 200, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd" };
                cboGioiTinh = new ComboBox() { Left = 140, Top = 100, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
                cboGioiTinh.Items.AddRange(new[] { "Nam", "Nữ", "Khác" });
                txtLop = new TextBox() { Left = 140, Top = 140, Width = 200 };
                txtKhoa = new TextBox() { Left = 140, Top = 180, Width = 200 };
                txtSDT = new TextBox() { Left = 140, Top = 220, Width = 200 };
                txtEmail = new TextBox() { Left = 140, Top = 260, Width = 340 };
                txtDiaChi = new TextBox() { Left = 140, Top = 300, Width = 340 };
                txtVaiTro = new TextBox() { Left = 140, Top = 340, Width = 200 };
                nudMaCV = new NumericUpDown() { Left = 380, Top = 100, Width = 100, Minimum = 0, Maximum = 1000 };
                nudMaBan = new NumericUpDown() { Left = 380, Top = 140, Width = 100, Minimum = 0, Maximum = 1000 };

                btnOk = new Button() { Left = 320, Top = 370, Width = 80, Text = "OK" };
                btnCancel = new Button() { Left = 410, Top = 370, Width = 80, Text = "Cancel" };
                btnOk.Click += BtnOk_Click;
                btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

                // Labels
                this.Controls.AddRange(new Control[] {
                    new Label(){ Left = 20, Top = 20, Width = 100, Text = "Họ tên:" }, txtHoTen,
                    new Label(){ Left = 20, Top = 60, Width = 100, Text = "Ngày sinh:" }, dtpNgaySinh,
                    new Label(){ Left = 20, Top = 100, Width = 120, Text = "Giới tính:" }, cboGioiTinh,
                    new Label(){ Left = 320, Top = 100, Width = 60, Text = "MaCV:" }, nudMaCV,
                    new Label(){ Left = 20, Top = 140, Width = 100, Text = "Lớp:" }, txtLop,
                    new Label(){ Left = 320, Top = 140, Width = 60, Text = "MaBan:" }, nudMaBan,
                    new Label(){ Left = 20, Top = 180, Width = 100, Text = "Khoa:" }, txtKhoa,
                    new Label(){ Left = 20, Top = 220, Width = 100, Text = "SĐT:" }, txtSDT,
                    new Label(){ Left = 20, Top = 260, Width = 100, Text = "Email:" }, txtEmail,
                    new Label(){ Left = 20, Top = 300, Width = 100, Text = "Địa chỉ:" }, txtDiaChi,
                    new Label(){ Left = 20, Top = 340, Width = 100, Text = "Vai trò:" }, txtVaiTro,
                    btnOk, btnCancel
                });

                this.AcceptButton = btnOk;
                this.CancelButton = btnCancel;
            }

            private void BtnOk_Click(object sender, EventArgs e)
            {
                // Basic validation
                if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Họ tên không được để trống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("Email không được để trống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                User.HoTen = txtHoTen.Text.Trim();
                User.NgaySinh = dtpNgaySinh.Value.Date;
                User.GioiTinh = cboGioiTinh.SelectedItem?.ToString();
                User.Lop = txtLop.Text.Trim();
                User.Khoa = txtKhoa.Text.Trim();
                User.SDT = txtSDT.Text.Trim();
                User.Email = txtEmail.Text.Trim();
                User.DiaChi = txtDiaChi.Text.Trim();
                User.VaiTro = txtVaiTro.Text.Trim();
                User.MaCV = (int)nudMaCV.Value;
                User.MaBan = nudMaBan.Value == 0 ? (int?)null : (int)nudMaBan.Value;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ucUser_Load(object sender, EventArgs e)
        {

        }
    }
}
