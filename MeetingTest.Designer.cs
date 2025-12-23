namespace ClubManageApp
{
    partial class ucMeeting
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlGhiChu = new System.Windows.Forms.Panel();
            this.lstEvents = new System.Windows.Forms.ListBox();
            this.lblDanhSachSuKien = new System.Windows.Forms.Label();
            this.txtMoTa = new System.Windows.Forms.TextBox();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.lblThoiGian = new System.Windows.Forms.Label();
            this.lblNgay = new System.Windows.Forms.Label();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblThongTinSuKien = new System.Windows.Forms.Label();
            this.pnlChucNang = new System.Windows.Forms.Panel();
            this.lblTimKiem = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.btnGuiEmail = new System.Windows.Forms.Button();
            this.btnXuatFile = new System.Windows.Forms.Button();
            this.btnThemSuKien = new System.Windows.Forms.Button();
            this.btnXoaSuKien = new System.Windows.Forms.Button();
            this.btnSuaSuKien = new System.Windows.Forms.Button();
            this.pnlLich = new System.Windows.Forms.Panel();
            this.pnlMonthNavigation = new System.Windows.Forms.Panel();
            this.btnThangTruoc = new System.Windows.Forms.Button();
            this.lblThangNam = new System.Windows.Forms.Label();
            this.btnThangSau = new System.Windows.Forms.Button();
            this.pnlDaysOfWeek = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlGhiChu.SuspendLayout();
            this.pnlChucNang.SuspendLayout();
            this.pnlLich.SuspendLayout();
            this.pnlMonthNavigation.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlGhiChu
            // 
            this.pnlGhiChu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGhiChu.BackColor = System.Drawing.Color.White;
            this.pnlGhiChu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGhiChu.Controls.Add(this.lstEvents);
            this.pnlGhiChu.Controls.Add(this.lblDanhSachSuKien);
            this.pnlGhiChu.Controls.Add(this.txtMoTa);
            this.pnlGhiChu.Controls.Add(this.lblMoTa);
            this.pnlGhiChu.Controls.Add(this.lblThoiGian);
            this.pnlGhiChu.Controls.Add(this.lblNgay);
            this.pnlGhiChu.Controls.Add(this.lblTieuDe);
            this.pnlGhiChu.Controls.Add(this.lblThongTinSuKien);
            this.pnlGhiChu.Location = new System.Drawing.Point(39, 40);
            this.pnlGhiChu.Name = "pnlGhiChu";
            this.pnlGhiChu.Size = new System.Drawing.Size(411, 576);
            this.pnlGhiChu.TabIndex = 4;
            // 
            // lstEvents
            // 
            this.lstEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstEvents.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lstEvents.FormattingEnabled = true;
            this.lstEvents.ItemHeight = 17;
            this.lstEvents.Location = new System.Drawing.Point(19, 145);
            this.lstEvents.Name = "lstEvents";
            this.lstEvents.Size = new System.Drawing.Size(370, 89);
            this.lstEvents.TabIndex = 7;
            this.lstEvents.SelectedIndexChanged += new System.EventHandler(this.lstEvents_SelectedIndexChanged);
            // 
            // lblDanhSachSuKien
            // 
            this.lblDanhSachSuKien.AutoSize = true;
            this.lblDanhSachSuKien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDanhSachSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblDanhSachSuKien.Location = new System.Drawing.Point(15, 110);
            this.lblDanhSachSuKien.Name = "lblDanhSachSuKien";
            this.lblDanhSachSuKien.Size = new System.Drawing.Size(131, 19);
            this.lblDanhSachSuKien.TabIndex = 6;
            this.lblDanhSachSuKien.Text = "Danh sách sự kiện:";
            // 
            // txtMoTa
            // 
            this.txtMoTa.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMoTa.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtMoTa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMoTa.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMoTa.Location = new System.Drawing.Point(19, 365);
            this.txtMoTa.Multiline = true;
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.ReadOnly = true;
            this.txtMoTa.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMoTa.Size = new System.Drawing.Size(370, 190);
            this.txtMoTa.TabIndex = 5;
            this.txtMoTa.Text = "Chọn một ngày trong lịch để xem thông tin sự kiện...";
            // 
            // lblMoTa
            // 
            this.lblMoTa.AutoSize = true;
            this.lblMoTa.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblMoTa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblMoTa.Location = new System.Drawing.Point(15, 335);
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.Size = new System.Drawing.Size(49, 19);
            this.lblMoTa.TabIndex = 4;
            this.lblMoTa.Text = "Mô tả:";
            // 
            // lblThoiGian
            // 
            this.lblThoiGian.AutoSize = true;
            this.lblThoiGian.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblThoiGian.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblThoiGian.Location = new System.Drawing.Point(15, 295);
            this.lblThoiGian.Name = "lblThoiGian";
            this.lblThoiGian.Size = new System.Drawing.Size(68, 19);
            this.lblThoiGian.TabIndex = 3;
            this.lblThoiGian.Text = "Thời gian:";
            // 
            // lblNgay
            // 
            this.lblNgay.AutoSize = true;
            this.lblNgay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNgay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblNgay.Location = new System.Drawing.Point(15, 260);
            this.lblNgay.Name = "lblNgay";
            this.lblNgay.Size = new System.Drawing.Size(44, 19);
            this.lblNgay.TabIndex = 2;
            this.lblNgay.Text = "Ngày:";
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTieuDe.Location = new System.Drawing.Point(15, 60);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(64, 20);
            this.lblTieuDe.TabIndex = 1;
            this.lblTieuDe.Text = "Tiêu đề:";
            // 
            // lblThongTinSuKien
            // 
            this.lblThongTinSuKien.AutoSize = true;
            this.lblThongTinSuKien.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblThongTinSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblThongTinSuKien.Location = new System.Drawing.Point(15, 15);
            this.lblThongTinSuKien.Name = "lblThongTinSuKien";
            this.lblThongTinSuKien.Size = new System.Drawing.Size(168, 25);
            this.lblThongTinSuKien.TabIndex = 0;
            this.lblThongTinSuKien.Text = "Thông tin sự kiện";
            // 
            // pnlChucNang
            // 
            this.pnlChucNang.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlChucNang.BackColor = System.Drawing.Color.White;
            this.pnlChucNang.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlChucNang.Controls.Add(this.lblTimKiem);
            this.pnlChucNang.Controls.Add(this.txtTimKiem);
            this.pnlChucNang.Controls.Add(this.btnTimKiem);
            this.pnlChucNang.Controls.Add(this.btnGuiEmail);
            this.pnlChucNang.Controls.Add(this.btnXuatFile);
            this.pnlChucNang.Controls.Add(this.btnThemSuKien);
            this.pnlChucNang.Controls.Add(this.btnXoaSuKien);
            this.pnlChucNang.Controls.Add(this.btnSuaSuKien);
            this.pnlChucNang.Location = new System.Drawing.Point(39, 652);
            this.pnlChucNang.Name = "pnlChucNang";
            this.pnlChucNang.Size = new System.Drawing.Size(411, 386);
            this.pnlChucNang.TabIndex = 5;
            // 
            // lblTimKiem
            // 
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTimKiem.Location = new System.Drawing.Point(15, 240);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Size = new System.Drawing.Size(115, 19);
            this.lblTimKiem.TabIndex = 6;
            this.lblTimKiem.Text = "Tìm kiếm nhanh";
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTimKiem.ForeColor = System.Drawing.Color.Gray;
            this.txtTimKiem.Location = new System.Drawing.Point(19, 275);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(370, 25);
            this.txtTimKiem.TabIndex = 5;
            this.txtTimKiem.Text = "Nhập tên sự kiện...";
            this.txtTimKiem.Enter += new System.EventHandler(this.txtTimKiem_Enter);
            this.txtTimKiem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTimKiem_KeyPress);
            this.txtTimKiem.Leave += new System.EventHandler(this.txtTimKiem_Leave);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTimKiem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnTimKiem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimKiem.FlatAppearance.BorderSize = 0;
            this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnTimKiem.ForeColor = System.Drawing.Color.White;
            this.btnTimKiem.Location = new System.Drawing.Point(19, 320);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(175, 45);
            this.btnTimKiem.TabIndex = 4;
            this.btnTimKiem.Text = "🔍 Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = false;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // btnGuiEmail
            // 
            this.btnGuiEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGuiEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.btnGuiEmail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuiEmail.FlatAppearance.BorderSize = 0;
            this.btnGuiEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuiEmail.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnGuiEmail.ForeColor = System.Drawing.Color.White;
            this.btnGuiEmail.Location = new System.Drawing.Point(214, 320);
            this.btnGuiEmail.Name = "btnGuiEmail";
            this.btnGuiEmail.Size = new System.Drawing.Size(175, 45);
            this.btnGuiEmail.TabIndex = 7;
            this.btnGuiEmail.Text = "📧 Gửi email";
            this.btnGuiEmail.UseVisualStyleBackColor = false;
            this.btnGuiEmail.Click += new System.EventHandler(this.btnGuiEmail_Click);
            // 
            // btnXuatFile
            // 
            this.btnXuatFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXuatFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnXuatFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXuatFile.FlatAppearance.BorderSize = 0;
            this.btnXuatFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXuatFile.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnXuatFile.ForeColor = System.Drawing.Color.White;
            this.btnXuatFile.Location = new System.Drawing.Point(19, 320);
            this.btnXuatFile.Name = "btnXuatFile";
            this.btnXuatFile.Size = new System.Drawing.Size(370, 45);
            this.btnXuatFile.TabIndex = 3;
            this.btnXuatFile.Text = "📄 Xuất file Excel";
            this.btnXuatFile.UseVisualStyleBackColor = false;
            this.btnXuatFile.Click += new System.EventHandler(this.btnXuatFile_Click);
            // 
            // btnThemSuKien
            // 
            this.btnThemSuKien.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThemSuKien.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnThemSuKien.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemSuKien.FlatAppearance.BorderSize = 0;
            this.btnThemSuKien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemSuKien.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnThemSuKien.ForeColor = System.Drawing.Color.White;
            this.btnThemSuKien.Location = new System.Drawing.Point(19, 20);
            this.btnThemSuKien.Name = "btnThemSuKien";
            this.btnThemSuKien.Size = new System.Drawing.Size(370, 50);
            this.btnThemSuKien.TabIndex = 0;
            this.btnThemSuKien.Text = "➕ Thêm sự kiện mới";
            this.btnThemSuKien.UseVisualStyleBackColor = false;
            this.btnThemSuKien.Click += new System.EventHandler(this.btnThemSuKien_Click);
            // 
            // btnXoaSuKien
            // 
            this.btnXoaSuKien.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXoaSuKien.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnXoaSuKien.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoaSuKien.Enabled = false;
            this.btnXoaSuKien.FlatAppearance.BorderSize = 0;
            this.btnXoaSuKien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoaSuKien.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnXoaSuKien.ForeColor = System.Drawing.Color.White;
            this.btnXoaSuKien.Location = new System.Drawing.Point(19, 160);
            this.btnXoaSuKien.Name = "btnXoaSuKien";
            this.btnXoaSuKien.Size = new System.Drawing.Size(370, 50);
            this.btnXoaSuKien.TabIndex = 2;
            this.btnXoaSuKien.Text = "🗑️ Xóa sự kiện";
            this.btnXoaSuKien.UseVisualStyleBackColor = false;
            this.btnXoaSuKien.Click += new System.EventHandler(this.btnXoaSuKien_Click);
            // 
            // btnSuaSuKien
            // 
            this.btnSuaSuKien.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSuaSuKien.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSuaSuKien.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSuaSuKien.Enabled = false;
            this.btnSuaSuKien.FlatAppearance.BorderSize = 0;
            this.btnSuaSuKien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSuaSuKien.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSuaSuKien.ForeColor = System.Drawing.Color.White;
            this.btnSuaSuKien.Location = new System.Drawing.Point(19, 90);
            this.btnSuaSuKien.Name = "btnSuaSuKien";
            this.btnSuaSuKien.Size = new System.Drawing.Size(370, 50);
            this.btnSuaSuKien.TabIndex = 1;
            this.btnSuaSuKien.Text = "✏️ Sửa sự kiện";
            this.btnSuaSuKien.UseVisualStyleBackColor = false;
            this.btnSuaSuKien.Click += new System.EventHandler(this.btnSuaSuKien_Click);
            // 
            // pnlLich
            // 
            this.pnlLich.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLich.BackColor = System.Drawing.Color.SlateGray;
            this.pnlLich.Controls.Add(this.pnlMonthNavigation);
            this.pnlLich.Controls.Add(this.pnlDaysOfWeek);
            this.pnlLich.Controls.Add(this.flowLayoutPanel1);
            this.pnlLich.Location = new System.Drawing.Point(484, 40);
            this.pnlLich.Name = "pnlLich";
            this.pnlLich.Size = new System.Drawing.Size(1356, 998);
            this.pnlLich.TabIndex = 6;
            // 
            // pnlMonthNavigation
            // 
            this.pnlMonthNavigation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMonthNavigation.BackColor = System.Drawing.Color.White;
            this.pnlMonthNavigation.Controls.Add(this.btnThangTruoc);
            this.pnlMonthNavigation.Controls.Add(this.lblThangNam);
            this.pnlMonthNavigation.Controls.Add(this.btnThangSau);
            this.pnlMonthNavigation.Location = new System.Drawing.Point(24, 920);
            this.pnlMonthNavigation.Name = "pnlMonthNavigation";
            this.pnlMonthNavigation.Size = new System.Drawing.Size(1305, 60);
            this.pnlMonthNavigation.TabIndex = 1;
            // 
            // btnThangTruoc
            // 
            this.btnThangTruoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnThangTruoc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThangTruoc.FlatAppearance.BorderSize = 0;
            this.btnThangTruoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThangTruoc.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnThangTruoc.ForeColor = System.Drawing.Color.White;
            this.btnThangTruoc.Location = new System.Drawing.Point(20, 10);
            this.btnThangTruoc.Name = "btnThangTruoc";
            this.btnThangTruoc.Size = new System.Drawing.Size(150, 40);
            this.btnThangTruoc.TabIndex = 0;
            this.btnThangTruoc.Text = "◀   Trước";
            this.btnThangTruoc.UseVisualStyleBackColor = false;
            this.btnThangTruoc.Click += new System.EventHandler(this.btnThangTruoc_Click);
            // 
            // lblThangNam
            // 
            this.lblThangNam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblThangNam.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblThangNam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblThangNam.Location = new System.Drawing.Point(180, 10);
            this.lblThangNam.Name = "lblThangNam";
            this.lblThangNam.Size = new System.Drawing.Size(945, 40);
            this.lblThangNam.TabIndex = 1;
            this.lblThangNam.Text = "Tháng 12 - 2024";
            this.lblThangNam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnThangSau
            // 
            this.btnThangSau.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThangSau.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnThangSau.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThangSau.FlatAppearance.BorderSize = 0;
            this.btnThangSau.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThangSau.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnThangSau.ForeColor = System.Drawing.Color.White;
            this.btnThangSau.Location = new System.Drawing.Point(1135, 10);
            this.btnThangSau.Name = "btnThangSau";
            this.btnThangSau.Size = new System.Drawing.Size(150, 40);
            this.btnThangSau.TabIndex = 2;
            this.btnThangSau.Text = "Sau   ▶";
            this.btnThangSau.UseVisualStyleBackColor = false;
            this.btnThangSau.Click += new System.EventHandler(this.btnThangSau_Click);
            // 
            // pnlDaysOfWeek
            // 
            this.pnlDaysOfWeek.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDaysOfWeek.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.pnlDaysOfWeek.Location = new System.Drawing.Point(24, 20);
            this.pnlDaysOfWeek.Name = "pnlDaysOfWeek";
            this.pnlDaysOfWeek.Size = new System.Drawing.Size(1305, 50);
            this.pnlDaysOfWeek.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(24, 80);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1305, 830);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // ucMeeting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlLich);
            this.Controls.Add(this.pnlChucNang);
            this.Controls.Add(this.pnlGhiChu);
            this.Name = "ucMeeting";
            this.Size = new System.Drawing.Size(1987, 1073);
            this.pnlGhiChu.ResumeLayout(false);
            this.pnlGhiChu.PerformLayout();
            this.pnlChucNang.ResumeLayout(false);
            this.pnlChucNang.PerformLayout();
            this.pnlLich.ResumeLayout(false);
            this.pnlMonthNavigation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlGhiChu;
        private System.Windows.Forms.Label lblThongTinSuKien;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblNgay;
        private System.Windows.Forms.Label lblThoiGian;
        private System.Windows.Forms.Label lblMoTa;
        private System.Windows.Forms.TextBox txtMoTa;
        private System.Windows.Forms.ListBox lstEvents;
        private System.Windows.Forms.Label lblDanhSachSuKien;
        private System.Windows.Forms.Panel pnlChucNang;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.Button btnGuiEmail;
        private System.Windows.Forms.Button btnXuatFile;
        private System.Windows.Forms.Button btnSuaSuKien;
        private System.Windows.Forms.Button btnXoaSuKien;
        private System.Windows.Forms.Button btnThemSuKien;
        private System.Windows.Forms.Panel pnlLich;
        private System.Windows.Forms.Panel pnlMonthNavigation;
        private System.Windows.Forms.Button btnThangTruoc;
        private System.Windows.Forms.Label lblThangNam;
        private System.Windows.Forms.Button btnThangSau;
        private System.Windows.Forms.Panel pnlDaysOfWeek;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
