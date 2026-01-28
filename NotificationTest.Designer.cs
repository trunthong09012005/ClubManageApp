namespace ClubManageApp
{
    partial class NotificationTest
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlThongBao = new System.Windows.Forms.Panel();
            this.pnlPagination = new System.Windows.Forms.Panel();
            this.cboPageSize = new System.Windows.Forms.ComboBox();
            this.btnPreviousPage = new System.Windows.Forms.Button();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.dgvThongBao = new System.Windows.Forms.DataGridView();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.cboFilterType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cboFilterStatus = new System.Windows.Forms.ComboBox();
            this.lblFilterStatus = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dtpNgayGui = new System.Windows.Forms.DateTimePicker();
            this.lblDate = new System.Windows.Forms.Label();
            this.txtDoiTuong = new System.Windows.Forms.TextBox();
            this.lblRecipient = new System.Windows.Forms.Label();
            this.txtTieuDe = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMaTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlLog = new System.Windows.Forms.Panel();
            this.dgvLog = new System.Windows.Forms.DataGridView();
            this.pnlLogHeader = new System.Windows.Forms.Panel();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.lblLog = new System.Windows.Forms.Label();
            this.pnlStats = new System.Windows.Forms.Panel();
            this.pnlStatsContent = new System.Windows.Forms.Panel();
            this.lblHuyBo = new System.Windows.Forms.Label();
            this.lblDuyet = new System.Windows.Forms.Label();
            this.lblNhap = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblTongThongBao = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblStatsTitle = new System.Windows.Forms.Label();
            this.pnlThongBao.SuspendLayout();
            this.pnlPagination.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongBao)).BeginInit();
            this.pnlFilter.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).BeginInit();
            this.pnlLogHeader.SuspendLayout();
            this.pnlStats.SuspendLayout();
            this.pnlStatsContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlThongBao
            // 
            this.pnlThongBao.BackColor = System.Drawing.Color.White;
            this.pnlThongBao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlThongBao.Controls.Add(this.pnlPagination);
            this.pnlThongBao.Controls.Add(this.dgvThongBao);
            this.pnlThongBao.Controls.Add(this.pnlFilter);
            this.pnlThongBao.Controls.Add(this.pnlHeader);
            this.pnlThongBao.Location = new System.Drawing.Point(39, 40);
            this.pnlThongBao.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlThongBao.Name = "pnlThongBao";
            this.pnlThongBao.Size = new System.Drawing.Size(1392, 1268);
            this.pnlThongBao.TabIndex = 0;
            // 
            // pnlPagination
            // 
            this.pnlPagination.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.pnlPagination.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPagination.Controls.Add(this.cboPageSize);
            this.pnlPagination.Controls.Add(this.btnPreviousPage);
            this.pnlPagination.Controls.Add(this.lblPageInfo);
            this.pnlPagination.Controls.Add(this.btnNextPage);
            this.pnlPagination.Location = new System.Drawing.Point(22, 1186);
            this.pnlPagination.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlPagination.Name = "pnlPagination";
            this.pnlPagination.Size = new System.Drawing.Size(1340, 60);
            this.pnlPagination.TabIndex = 3;
            // 
            // cboPageSize
            // 
            this.cboPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPageSize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboPageSize.FormattingEnabled = true;
            this.cboPageSize.Items.AddRange(new object[] {
            "5",
            "10",
            "20",
            "50"});
            this.cboPageSize.Location = new System.Drawing.Point(1185, 11);
            this.cboPageSize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboPageSize.Name = "cboPageSize";
            this.cboPageSize.Size = new System.Drawing.Size(133, 33);
            this.cboPageSize.TabIndex = 3;
            this.cboPageSize.SelectedIndexChanged += new System.EventHandler(this.cboPageSize_SelectedIndexChanged);
            // 
            // btnPreviousPage
            // 
            this.btnPreviousPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnPreviousPage.FlatAppearance.BorderSize = 0;
            this.btnPreviousPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviousPage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPreviousPage.ForeColor = System.Drawing.Color.White;
            this.btnPreviousPage.Location = new System.Drawing.Point(22, 8);
            this.btnPreviousPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPreviousPage.Name = "btnPreviousPage";
            this.btnPreviousPage.Size = new System.Drawing.Size(150, 43);
            this.btnPreviousPage.TabIndex = 0;
            this.btnPreviousPage.Text = "◀ Trước";
            this.btnPreviousPage.UseVisualStyleBackColor = false;
            this.btnPreviousPage.Click += new System.EventHandler(this.btnPreviousPage_Click);
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblPageInfo.Location = new System.Drawing.Point(180, 8);
            this.lblPageInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(690, 43);
            this.lblPageInfo.TabIndex = 1;
            this.lblPageInfo.Text = "Trang 1 / 1 (Tổng: 0 thông báo)";
            this.lblPageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNextPage
            // 
            this.btnNextPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnNextPage.FlatAppearance.BorderSize = 0;
            this.btnNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextPage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNextPage.ForeColor = System.Drawing.Color.White;
            this.btnNextPage.Location = new System.Drawing.Point(1020, 8);
            this.btnNextPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(150, 43);
            this.btnNextPage.TabIndex = 2;
            this.btnNextPage.Text = "Tiếp ▶";
            this.btnNextPage.UseVisualStyleBackColor = false;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // dgvThongBao
            // 
            this.dgvThongBao.AllowUserToAddRows = false;
            this.dgvThongBao.AllowUserToDeleteRows = false;
            this.dgvThongBao.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvThongBao.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dgvThongBao.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvThongBao.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThongBao.EnableHeadersVisualStyles = false;
            this.dgvThongBao.Location = new System.Drawing.Point(22, 492);
            this.dgvThongBao.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvThongBao.Name = "dgvThongBao";
            this.dgvThongBao.ReadOnly = true;
            this.dgvThongBao.RowHeadersVisible = false;
            this.dgvThongBao.RowHeadersWidth = 62;
            this.dgvThongBao.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThongBao.Size = new System.Drawing.Size(1341, 685);
            this.dgvThongBao.TabIndex = 2;
            this.dgvThongBao.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvThongBao_CellClick);
            this.dgvThongBao.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvThongBao_CellContentClick);
            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.pnlFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilter.Controls.Add(this.cboFilterType);
            this.pnlFilter.Controls.Add(this.lblType);
            this.pnlFilter.Controls.Add(this.cboFilterStatus);
            this.pnlFilter.Controls.Add(this.lblFilterStatus);
            this.pnlFilter.Controls.Add(this.btnClear);
            this.pnlFilter.Controls.Add(this.btnDelete);
            this.pnlFilter.Controls.Add(this.btnEdit);
            this.pnlFilter.Controls.Add(this.btnAdd);
            this.pnlFilter.Controls.Add(this.dtpNgayGui);
            this.pnlFilter.Controls.Add(this.lblDate);
            this.pnlFilter.Controls.Add(this.txtDoiTuong);
            this.pnlFilter.Controls.Add(this.lblRecipient);
            this.pnlFilter.Controls.Add(this.txtTieuDe);
            this.pnlFilter.Controls.Add(this.label3);
            this.pnlFilter.Controls.Add(this.txtMaTB);
            this.pnlFilter.Controls.Add(this.label1);
            this.pnlFilter.Location = new System.Drawing.Point(22, 108);
            this.pnlFilter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(1340, 360);
            this.pnlFilter.TabIndex = 1;
            // 
            // cboFilterType
            // 
            this.cboFilterType.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cboFilterType.FormattingEnabled = true;
            this.cboFilterType.Location = new System.Drawing.Point(603, 121);
            this.cboFilterType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboFilterType.Name = "cboFilterType";
            this.cboFilterType.Size = new System.Drawing.Size(228, 36);
            this.cboFilterType.TabIndex = 9;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblType.Location = new System.Drawing.Point(460, 125);
            this.lblType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(56, 28);
            this.lblType.TabIndex = 8;
            this.lblType.Text = "Loại:";
            // 
            // cboFilterStatus
            // 
            this.cboFilterStatus.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cboFilterStatus.FormattingEnabled = true;
            this.cboFilterStatus.Location = new System.Drawing.Point(1080, 121);
            this.cboFilterStatus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboFilterStatus.Name = "cboFilterStatus";
            this.cboFilterStatus.Size = new System.Drawing.Size(229, 36);
            this.cboFilterStatus.TabIndex = 7;
            // 
            // lblFilterStatus
            // 
            this.lblFilterStatus.AutoSize = true;
            this.lblFilterStatus.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblFilterStatus.Location = new System.Drawing.Point(874, 125);
            this.lblFilterStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFilterStatus.Name = "lblFilterStatus";
            this.lblFilterStatus.Size = new System.Drawing.Size(113, 28);
            this.lblFilterStatus.TabIndex = 6;
            this.lblFilterStatus.Text = "Trạng thái:";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(1100, 277);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(202, 54);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "🔄 Làm mới";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(888, 277);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(202, 54);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "🗑️ Xóa";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(676, 277);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(202, 54);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "✏️ Sửa";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(465, 277);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(202, 54);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "➕ Thêm";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dtpNgayGui
            // 
            this.dtpNgayGui.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpNgayGui.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.dtpNgayGui.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayGui.Location = new System.Drawing.Point(225, 124);
            this.dtpNgayGui.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtpNgayGui.Name = "dtpNgayGui";
            this.dtpNgayGui.Size = new System.Drawing.Size(193, 33);
            this.dtpNgayGui.TabIndex = 11;
            this.dtpNgayGui.ValueChanged += new System.EventHandler(this.dtpNgayGui_ValueChanged);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblDate.Location = new System.Drawing.Point(30, 128);
            this.lblDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(104, 28);
            this.lblDate.TabIndex = 10;
            this.lblDate.Text = "Ngày gửi:";
            // 
            // txtDoiTuong
            // 
            this.txtDoiTuong.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtDoiTuong.Location = new System.Drawing.Point(603, 51);
            this.txtDoiTuong.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDoiTuong.Name = "txtDoiTuong";
            this.txtDoiTuong.Size = new System.Drawing.Size(228, 33);
            this.txtDoiTuong.TabIndex = 5;
            // 
            // lblRecipient
            // 
            this.lblRecipient.AutoSize = true;
            this.lblRecipient.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblRecipient.Location = new System.Drawing.Point(460, 55);
            this.lblRecipient.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRecipient.Name = "lblRecipient";
            this.lblRecipient.Size = new System.Drawing.Size(114, 28);
            this.lblRecipient.TabIndex = 4;
            this.lblRecipient.Text = "Đối tượng:";
            // 
            // txtTieuDe
            // 
            this.txtTieuDe.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtTieuDe.Location = new System.Drawing.Point(225, 202);
            this.txtTieuDe.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTieuDe.Name = "txtTieuDe";
            this.txtTieuDe.Size = new System.Drawing.Size(606, 33);
            this.txtTieuDe.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(30, 206);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 28);
            this.label3.TabIndex = 2;
            this.label3.Text = "Tiêu đề:";
            // 
            // txtMaTB
            // 
            this.txtMaTB.Enabled = false;
            this.txtMaTB.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtMaTB.Location = new System.Drawing.Point(225, 46);
            this.txtMaTB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMaTB.Name = "txtMaTB";
            this.txtMaTB.ReadOnly = true;
            this.txtMaTB.Size = new System.Drawing.Size(193, 33);
            this.txtMaTB.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(30, 51);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã TB:";
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.btnRefresh);
            this.pnlHeader.Controls.Add(this.btnSearch);
            this.pnlHeader.Controls.Add(this.txtSearch);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Location = new System.Drawing.Point(22, 23);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1340, 76);
            this.pnlHeader.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(1200, 20);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(105, 43);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "🔄";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(1080, 20);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(105, 43);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "🔍";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtSearch.Location = new System.Drawing.Point(690, 23);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(373, 33);
            this.txtSearch.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(15, 18);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(325, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ THÔNG BÁO";
            // 
            // pnlLog
            // 
            this.pnlLog.BackColor = System.Drawing.Color.White;
            this.pnlLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLog.Controls.Add(this.dgvLog);
            this.pnlLog.Controls.Add(this.pnlLogHeader);
            this.pnlLog.Location = new System.Drawing.Point(1442, 838);
            this.pnlLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlLog.Name = "pnlLog";
            this.pnlLog.Size = new System.Drawing.Size(497, 470);
            this.pnlLog.TabIndex = 1;
            // 
            // dgvLog
            // 
            this.dgvLog.AllowUserToAddRows = false;
            this.dgvLog.AllowUserToDeleteRows = false;
            this.dgvLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLog.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            this.dgvLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLog.EnableHeadersVisualStyles = false;
            this.dgvLog.Location = new System.Drawing.Point(15, 92);
            this.dgvLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvLog.Name = "dgvLog";
            this.dgvLog.ReadOnly = true;
            this.dgvLog.RowHeadersVisible = false;
            this.dgvLog.RowHeadersWidth = 62;
            this.dgvLog.Size = new System.Drawing.Size(462, 357);
            this.dgvLog.TabIndex = 1;
            // 
            // pnlLogHeader
            // 
            this.pnlLogHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.pnlLogHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLogHeader.Controls.Add(this.btnClearLog);
            this.pnlLogHeader.Controls.Add(this.lblLog);
            this.pnlLogHeader.Location = new System.Drawing.Point(15, 15);
            this.pnlLogHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlLogHeader.Name = "pnlLogHeader";
            this.pnlLogHeader.Size = new System.Drawing.Size(461, 60);
            this.pnlLogHeader.TabIndex = 0;
            // 
            // btnClearLog
            // 
            this.btnClearLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnClearLog.FlatAppearance.BorderSize = 0;
            this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLog.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnClearLog.ForeColor = System.Drawing.Color.White;
            this.btnClearLog.Location = new System.Drawing.Point(338, 8);
            this.btnClearLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(105, 38);
            this.btnClearLog.TabIndex = 1;
            this.btnClearLog.Text = "Xóa Log";
            this.btnClearLog.UseVisualStyleBackColor = false;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblLog.ForeColor = System.Drawing.Color.White;
            this.lblLog.Location = new System.Drawing.Point(15, 15);
            this.lblLog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(188, 31);
            this.lblLog.TabIndex = 0;
            this.lblLog.Text = "LỊCH SỬ TÁC VỤ";
            // 
            // pnlStats
            // 
            this.pnlStats.BackColor = System.Drawing.Color.White;
            this.pnlStats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlStats.Controls.Add(this.pnlStatsContent);
            this.pnlStats.Location = new System.Drawing.Point(1442, 40);
            this.pnlStats.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlStats.Name = "pnlStats";
            this.pnlStats.Size = new System.Drawing.Size(497, 788);
            this.pnlStats.TabIndex = 1;
            // 
            // pnlStatsContent
            // 
            this.pnlStatsContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.pnlStatsContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlStatsContent.Controls.Add(this.lblHuyBo);
            this.pnlStatsContent.Controls.Add(this.lblDuyet);
            this.pnlStatsContent.Controls.Add(this.lblNhap);
            this.pnlStatsContent.Controls.Add(this.label12);
            this.pnlStatsContent.Controls.Add(this.lblTongThongBao);
            this.pnlStatsContent.Controls.Add(this.label10);
            this.pnlStatsContent.Controls.Add(this.lblStatsTitle);
            this.pnlStatsContent.Location = new System.Drawing.Point(15, 15);
            this.pnlStatsContent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlStatsContent.Name = "pnlStatsContent";
            this.pnlStatsContent.Size = new System.Drawing.Size(461, 747);
            this.pnlStatsContent.TabIndex = 0;
            // 
            // lblHuyBo
            // 
            this.lblHuyBo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.lblHuyBo.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblHuyBo.ForeColor = System.Drawing.Color.White;
            this.lblHuyBo.Location = new System.Drawing.Point(30, 645);
            this.lblHuyBo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHuyBo.Name = "lblHuyBo";
            this.lblHuyBo.Size = new System.Drawing.Size(387, 54);
            this.lblHuyBo.TabIndex = 6;
            this.lblHuyBo.Text = "Không gửi: 0";
            this.lblHuyBo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDuyet
            // 
            this.lblDuyet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.lblDuyet.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblDuyet.ForeColor = System.Drawing.Color.White;
            this.lblDuyet.Location = new System.Drawing.Point(30, 583);
            this.lblDuyet.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDuyet.Name = "lblDuyet";
            this.lblDuyet.Size = new System.Drawing.Size(387, 54);
            this.lblDuyet.TabIndex = 5;
            this.lblDuyet.Text = "Đã gửi: 0";
            this.lblDuyet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNhap
            // 
            this.lblNhap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.lblNhap.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblNhap.ForeColor = System.Drawing.Color.White;
            this.lblNhap.Location = new System.Drawing.Point(30, 522);
            this.lblNhap.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNhap.Name = "lblNhap";
            this.lblNhap.Size = new System.Drawing.Size(387, 54);
            this.lblNhap.TabIndex = 4;
            this.lblNhap.Text = "Nháp: 0";
            this.lblNhap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(30, 483);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(150, 28);
            this.label12.TabIndex = 3;
            this.label12.Text = "Trạng thái gửi:";
            // 
            // lblTongThongBao
            // 
            this.lblTongThongBao.Font = new System.Drawing.Font("Segoe UI", 16.25F, System.Drawing.FontStyle.Bold);
            this.lblTongThongBao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.lblTongThongBao.Location = new System.Drawing.Point(222, 126);
            this.lblTongThongBao.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTongThongBao.Name = "lblTongThongBao";
            this.lblTongThongBao.Size = new System.Drawing.Size(195, 57);
            this.lblTongThongBao.TabIndex = 2;
            this.lblTongThongBao.Text = "0";
            this.lblTongThongBao.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(30, 148);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(168, 28);
            this.label10.TabIndex = 1;
            this.label10.Text = "Tổng thông báo:";
            // 
            // lblStatsTitle
            // 
            this.lblStatsTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(188)))), ((int)(((byte)(156)))));
            this.lblStatsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatsTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblStatsTitle.ForeColor = System.Drawing.Color.White;
            this.lblStatsTitle.Location = new System.Drawing.Point(0, 0);
            this.lblStatsTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatsTitle.Name = "lblStatsTitle";
            this.lblStatsTitle.Size = new System.Drawing.Size(459, 62);
            this.lblStatsTitle.TabIndex = 0;
            this.lblStatsTitle.Text = "THỐNG KÊ THÔNG BÁO";
            this.lblStatsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NotificationTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlLog);
            this.Controls.Add(this.pnlStats);
            this.Controls.Add(this.pnlThongBao);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "NotificationTest";
            this.Size = new System.Drawing.Size(2220, 1354);
            this.Load += new System.EventHandler(this.NotificationTest_Load);
            this.pnlThongBao.ResumeLayout(false);
            this.pnlPagination.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongBao)).EndInit();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).EndInit();
            this.pnlLogHeader.ResumeLayout(false);
            this.pnlLogHeader.PerformLayout();
            this.pnlStats.ResumeLayout(false);
            this.pnlStatsContent.ResumeLayout(false);
            this.pnlStatsContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlThongBao;
        private System.Windows.Forms.Panel pnlLog;
        private System.Windows.Forms.Panel pnlStats;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMaTB;
        private System.Windows.Forms.TextBox txtTieuDe;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDoiTuong;
        private System.Windows.Forms.Label lblRecipient;
        private System.Windows.Forms.DateTimePicker dtpNgayGui;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dgvThongBao;
        private System.Windows.Forms.Panel pnlPagination;
        private System.Windows.Forms.Button btnPreviousPage;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.ComboBox cboPageSize;
        private System.Windows.Forms.ComboBox cboFilterType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cboFilterStatus;
        private System.Windows.Forms.Label lblFilterStatus;
        private System.Windows.Forms.Panel pnlStatsContent;
        private System.Windows.Forms.Label lblStatsTitle;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblTongThongBao;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblNhap;
        private System.Windows.Forms.Label lblDuyet;
        private System.Windows.Forms.Label lblHuyBo;
        private System.Windows.Forms.DataGridView dgvLog;
        private System.Windows.Forms.Panel pnlLogHeader;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Button btnClearLog;
    }
}
