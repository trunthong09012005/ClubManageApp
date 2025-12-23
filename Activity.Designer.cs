namespace ClubManageApp
{
    partial class Activity
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
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dgvActivities = new System.Windows.Forms.DataGridView();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.lblSort = new System.Windows.Forms.Label();
            this.cboSortBy = new System.Windows.Forms.ComboBox();
            this.btnResetFilter = new System.Windows.Forms.Button();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.btnAddActivity = new System.Windows.Forms.Button();
            this.btnEditActivity = new System.Windows.Forms.Button();
            this.btnDeleteActivity = new System.Windows.Forms.Button();
            this.btnViewDetails = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.lblCountFooter = new System.Windows.Forms.Label();
            this.lblTimeFooter = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvActivities)).BeginInit();
            this.pnlFilter.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Controls.Add(this.pnlGrid);
            this.pnlMain.Controls.Add(this.pnlFilter);
            this.pnlMain.Controls.Add(this.pnlToolbar);
            this.pnlMain.Controls.Add(this.pnlHeader);
            this.pnlMain.Controls.Add(this.pnlFooter);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(971, 570);
            this.pnlMain.TabIndex = 0;
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.dgvActivities);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 253);
            this.pnlGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlGrid.Size = new System.Drawing.Size(971, 283);
            this.pnlGrid.TabIndex = 0;
            // 
            // dgvActivities
            // 
            this.dgvActivities.BackgroundColor = System.Drawing.Color.White;
            this.dgvActivities.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvActivities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvActivities.Location = new System.Drawing.Point(13, 12);
            this.dgvActivities.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvActivities.Name = "dgvActivities";
            this.dgvActivities.RowHeadersVisible = false;
            this.dgvActivities.RowHeadersWidth = 62;
            this.dgvActivities.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvActivities.Size = new System.Drawing.Size(945, 259);
            this.dgvActivities.StandardTab = true;
            this.dgvActivities.TabIndex = 0;
            this.dgvActivities.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvActivities_CellContentClick_1);
            this.dgvActivities.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvActivities_CellDoubleClick);
            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pnlFilter.Controls.Add(this.lblSearch);
            this.pnlFilter.Controls.Add(this.txtSearch);
            this.pnlFilter.Controls.Add(this.btnSearch);
            this.pnlFilter.Controls.Add(this.lblStatus);
            this.pnlFilter.Controls.Add(this.cboStatus);
            this.pnlFilter.Controls.Add(this.lblSort);
            this.pnlFilter.Controls.Add(this.cboSortBy);
            this.pnlFilter.Controls.Add(this.btnResetFilter);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 163);
            this.pnlFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlFilter.Size = new System.Drawing.Size(971, 90);
            this.pnlFilter.TabIndex = 1;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSearch.Location = new System.Drawing.Point(13, 10);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(59, 15);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Tìm kiếm:";
            // 
            // txtSearch
            // 
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Location = new System.Drawing.Point(97, 11);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(267, 22);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(385, 6);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(97, 31);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "🔍 Tìm";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.Location = new System.Drawing.Point(508, 10);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(62, 15);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Trạng thái:";
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Items.AddRange(new object[] {
            "Tất cả",
            "Đang chuẩn bị",
            "Đang diễn ra",
            "Hoàn thành",
            "Hủy bỏ"});
            this.cboStatus.Location = new System.Drawing.Point(596, 10);
            this.cboStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(360, 24);
            this.cboStatus.TabIndex = 4;
            this.cboStatus.SelectedIndexChanged += new System.EventHandler(this.cboStatus_SelectedIndexChanged);
            // 
            // lblSort
            // 
            this.lblSort.AutoSize = true;
            this.lblSort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSort.Location = new System.Drawing.Point(18, 50);
            this.lblSort.Name = "lblSort";
            this.lblSort.Size = new System.Drawing.Size(51, 15);
            this.lblSort.TabIndex = 5;
            this.lblSort.Text = "Sắp xếp:";
            // 
            // cboSortBy
            // 
            this.cboSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSortBy.Items.AddRange(new object[] {
            "Ngày tổ chức (Mới nhất)",
            "Ngày tổ chức (Cũ nhất)",
            "Tên (A-Z)",
            "Tên (Z-A)",
            "Kinh phí (Cao nhất)",
            "Kinh phí (Thấp nhất)"});
            this.cboSortBy.Location = new System.Drawing.Point(97, 51);
            this.cboSortBy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboSortBy.Name = "cboSortBy";
            this.cboSortBy.Size = new System.Drawing.Size(267, 24);
            this.cboSortBy.TabIndex = 6;
            // 
            // btnResetFilter
            // 
            this.btnResetFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(158)))), ((int)(((byte)(158)))));
            this.btnResetFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnResetFilter.ForeColor = System.Drawing.Color.White;
            this.btnResetFilter.Location = new System.Drawing.Point(385, 47);
            this.btnResetFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnResetFilter.Name = "btnResetFilter";
            this.btnResetFilter.Size = new System.Drawing.Size(97, 31);
            this.btnResetFilter.TabIndex = 7;
            this.btnResetFilter.Text = "↺ Đặt lại";
            this.btnResetFilter.UseVisualStyleBackColor = false;
            this.btnResetFilter.Click += new System.EventHandler(this.btnResetFilter_Click);
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.pnlToolbar.Controls.Add(this.btnAddActivity);
            this.pnlToolbar.Controls.Add(this.btnEditActivity);
            this.pnlToolbar.Controls.Add(this.btnDeleteActivity);
            this.pnlToolbar.Controls.Add(this.btnViewDetails);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Location = new System.Drawing.Point(0, 92);
            this.pnlToolbar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlToolbar.Size = new System.Drawing.Size(971, 71);
            this.pnlToolbar.TabIndex = 2;
            // 
            // btnAddActivity
            // 
            this.btnAddActivity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnAddActivity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddActivity.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddActivity.ForeColor = System.Drawing.Color.White;
            this.btnAddActivity.Location = new System.Drawing.Point(13, 15);
            this.btnAddActivity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddActivity.Name = "btnAddActivity";
            this.btnAddActivity.Size = new System.Drawing.Size(185, 42);
            this.btnAddActivity.TabIndex = 0;
            this.btnAddActivity.Text = "+ Thêm hoạt động";
            this.btnAddActivity.UseVisualStyleBackColor = false;
            this.btnAddActivity.Click += new System.EventHandler(this.btnAddActivity_Click);
            // 
            // btnEditActivity
            // 
            this.btnEditActivity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnEditActivity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditActivity.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEditActivity.ForeColor = System.Drawing.Color.White;
            this.btnEditActivity.Location = new System.Drawing.Point(231, 15);
            this.btnEditActivity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEditActivity.Name = "btnEditActivity";
            this.btnEditActivity.Size = new System.Drawing.Size(132, 42);
            this.btnEditActivity.TabIndex = 1;
            this.btnEditActivity.Text = "✎ Chỉnh sửa";
            this.btnEditActivity.UseVisualStyleBackColor = false;
            this.btnEditActivity.Click += new System.EventHandler(this.btnEditActivity_Click);
            // 
            // btnDeleteActivity
            // 
            this.btnDeleteActivity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnDeleteActivity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteActivity.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeleteActivity.ForeColor = System.Drawing.Color.White;
            this.btnDeleteActivity.Location = new System.Drawing.Point(402, 15);
            this.btnDeleteActivity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDeleteActivity.Name = "btnDeleteActivity";
            this.btnDeleteActivity.Size = new System.Drawing.Size(106, 42);
            this.btnDeleteActivity.TabIndex = 2;
            this.btnDeleteActivity.Text = "🗑 Xóa";
            this.btnDeleteActivity.UseVisualStyleBackColor = false;
            this.btnDeleteActivity.Click += new System.EventHandler(this.btnDeleteActivity_Click);
            // 
            // btnViewDetails
            // 
            this.btnViewDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(39)))), ((int)(((byte)(176)))));
            this.btnViewDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewDetails.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnViewDetails.ForeColor = System.Drawing.Color.White;
            this.btnViewDetails.Location = new System.Drawing.Point(550, 15);
            this.btnViewDetails.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(140, 42);
            this.btnViewDetails.TabIndex = 3;
            this.btnViewDetails.Text = "👁 Chi tiết";
            this.btnViewDetails.UseVisualStyleBackColor = false;
            this.btnViewDetails.Click += new System.EventHandler(this.btnViewDetails_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(18, 16, 18, 16);
            this.pnlHeader.Size = new System.Drawing.Size(971, 92);
            this.pnlHeader.TabIndex = 3;
            this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeader_Paint);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 35);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(231, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản lý Hoạt động";
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            this.lblSubtitle.Location = new System.Drawing.Point(18, 16);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(341, 19);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Tạo, chỉnh sửa, xóa và theo dõi các hoạt động của CLB";
            this.lblSubtitle.Click += new System.EventHandler(this.lblSubtitle_Click);
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.pnlFooter.Controls.Add(this.lblCountFooter);
            this.pnlFooter.Controls.Add(this.lblTimeFooter);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 536);
            this.pnlFooter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(9, 8, 9, 8);
            this.pnlFooter.Size = new System.Drawing.Size(971, 34);
            this.pnlFooter.TabIndex = 4;
            // 
            // lblCountFooter
            // 
            this.lblCountFooter.AutoSize = true;
            this.lblCountFooter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblCountFooter.Location = new System.Drawing.Point(18, 8);
            this.lblCountFooter.Name = "lblCountFooter";
            this.lblCountFooter.Size = new System.Drawing.Size(130, 16);
            this.lblCountFooter.TabIndex = 0;
            this.lblCountFooter.Text = "Đang hiển thị: 0 dòng";
            // 
            // lblTimeFooter
            // 
            this.lblTimeFooter.AutoSize = true;
            this.lblTimeFooter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblTimeFooter.Location = new System.Drawing.Point(800, 8);
            this.lblTimeFooter.Name = "lblTimeFooter";
            this.lblTimeFooter.Size = new System.Drawing.Size(0, 16);
            this.lblTimeFooter.TabIndex = 1;
            // 
            // Activity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Activity";
            this.Size = new System.Drawing.Size(971, 570);
            this.Load += new System.EventHandler(this.Activity_Load);
            this.pnlMain.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvActivities)).EndInit();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.pnlToolbar.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.Button btnAddActivity;
        private System.Windows.Forms.Button btnEditActivity;
        private System.Windows.Forms.Button btnDeleteActivity;
        private System.Windows.Forms.Button btnViewDetails;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label lblSort;
        private System.Windows.Forms.ComboBox cboSortBy;
        private System.Windows.Forms.Button btnResetFilter;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dgvActivities;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblCountFooter;
        private System.Windows.Forms.Label lblTimeFooter;
    }
}
