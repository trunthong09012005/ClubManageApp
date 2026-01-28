namespace ClubManageApp
{
    partial class ActivityEditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTen = new System.Windows.Forms.Label();
            this.txtTenHD = new System.Windows.Forms.TextBox();
            this.lblNgay = new System.Windows.Forms.Label();
            this.dtpNgayToChuc = new System.Windows.Forms.DateTimePicker();
            this.lblGioBD = new System.Windows.Forms.Label();
            this.dtpGioBatDau = new System.Windows.Forms.DateTimePicker();
            this.lblGioKT = new System.Windows.Forms.Label();
            this.dtpGioKetThuc = new System.Windows.Forms.DateTimePicker();
            this.lblDiaDiem = new System.Windows.Forms.Label();
            this.txtDiaDiem = new System.Windows.Forms.TextBox();
            this.lblMoTa = new System.Windows.Forms.Label();
            this.txtMoTa = new System.Windows.Forms.TextBox();
            this.lblKinhPhi = new System.Windows.Forms.Label();
            this.nudKinhPhiDuKien = new System.Windows.Forms.NumericUpDown();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.nudSoLuongToiDa = new System.Windows.Forms.NumericUpDown();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.cboTrangThai = new System.Windows.Forms.ComboBox();
            this.lblNguoiPT = new System.Windows.Forms.Label();
            this.nudNguoiPhuTrach = new System.Windows.Forms.NumericUpDown();
            this.lblLoaiHD = new System.Windows.Forms.Label();
            this.cboLoaiHD = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudKinhPhiDuKien)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoLuongToiDa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNguoiPhuTrach)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTen
            // 
            this.lblTen.Location = new System.Drawing.Point(40, 20);
            this.lblTen.Name = "lblTen";
            this.lblTen.Size = new System.Drawing.Size(120, 23);
            this.lblTen.TabIndex = 0;
            this.lblTen.Text = "Tên hoạt động:";
            // 
            // txtTenHD
            // 
            this.txtTenHD.Location = new System.Drawing.Point(166, 20);
            this.txtTenHD.Name = "txtTenHD";
            this.txtTenHD.Size = new System.Drawing.Size(396, 26);
            this.txtTenHD.TabIndex = 1;
            // 
            // lblNgay
            // 
            this.lblNgay.Location = new System.Drawing.Point(40, 62);
            this.lblNgay.Name = "lblNgay";
            this.lblNgay.Size = new System.Drawing.Size(114, 23);
            this.lblNgay.TabIndex = 2;
            this.lblNgay.Text = "Ngày tổ chức:";
            // 
            // dtpNgayToChuc
            // 
            this.dtpNgayToChuc.Location = new System.Drawing.Point(166, 62);
            this.dtpNgayToChuc.Name = "dtpNgayToChuc";
            this.dtpNgayToChuc.Size = new System.Drawing.Size(396, 26);
            this.dtpNgayToChuc.TabIndex = 3;
            this.dtpNgayToChuc.ValueChanged += new System.EventHandler(this.dtpNgayToChuc_ValueChanged);
            // 
            // lblGioBD
            // 
            this.lblGioBD.Location = new System.Drawing.Point(40, 104);
            this.lblGioBD.Name = "lblGioBD";
            this.lblGioBD.Size = new System.Drawing.Size(100, 23);
            this.lblGioBD.TabIndex = 4;
            this.lblGioBD.Text = "Giờ bắt đầu:";
            // 
            // dtpGioBatDau
            // 
            this.dtpGioBatDau.CustomFormat = "HH:mm";
            this.dtpGioBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpGioBatDau.Location = new System.Drawing.Point(166, 101);
            this.dtpGioBatDau.Name = "dtpGioBatDau";
            this.dtpGioBatDau.ShowUpDown = true;
            this.dtpGioBatDau.Size = new System.Drawing.Size(180, 26);
            this.dtpGioBatDau.TabIndex = 5;
            // 
            // lblGioKT
            // 
            this.lblGioKT.Location = new System.Drawing.Point(40, 104);
            this.lblGioKT.Name = "lblGioKT";
            this.lblGioKT.Size = new System.Drawing.Size(100, 23);
            this.lblGioKT.TabIndex = 6;
            this.lblGioKT.Text = "Giờ kết thúc:";
            // 
            // dtpGioKetThuc
            // 
            this.dtpGioKetThuc.CustomFormat = "HH:mm";
            this.dtpGioKetThuc.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpGioKetThuc.Location = new System.Drawing.Point(382, 101);
            this.dtpGioKetThuc.Name = "dtpGioKetThuc";
            this.dtpGioKetThuc.ShowUpDown = true;
            this.dtpGioKetThuc.Size = new System.Drawing.Size(180, 26);
            this.dtpGioKetThuc.TabIndex = 7;
            // 
            // lblDiaDiem
            // 
            this.lblDiaDiem.Location = new System.Drawing.Point(40, 146);
            this.lblDiaDiem.Name = "lblDiaDiem";
            this.lblDiaDiem.Size = new System.Drawing.Size(81, 23);
            this.lblDiaDiem.TabIndex = 8;
            this.lblDiaDiem.Text = "Địa điểm:";
            // 
            // txtDiaDiem
            // 
            this.txtDiaDiem.Location = new System.Drawing.Point(166, 143);
            this.txtDiaDiem.Name = "txtDiaDiem";
            this.txtDiaDiem.Size = new System.Drawing.Size(396, 26);
            this.txtDiaDiem.TabIndex = 9;
            // 
            // lblMoTa
            // 
            this.lblMoTa.Location = new System.Drawing.Point(40, 188);
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.Size = new System.Drawing.Size(56, 23);
            this.lblMoTa.TabIndex = 10;
            this.lblMoTa.Text = "Mô tả:";
            // 
            // txtMoTa
            // 
            this.txtMoTa.Location = new System.Drawing.Point(44, 220);
            this.txtMoTa.Multiline = true;
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.Size = new System.Drawing.Size(518, 215);
            this.txtMoTa.TabIndex = 11;
            // 
            // lblKinhPhi
            // 
            this.lblKinhPhi.Location = new System.Drawing.Point(614, 24);
            this.lblKinhPhi.Name = "lblKinhPhi";
            this.lblKinhPhi.Size = new System.Drawing.Size(129, 23);
            this.lblKinhPhi.TabIndex = 12;
            this.lblKinhPhi.Text = "Kinh phí dự kiến:";
            this.lblKinhPhi.Click += new System.EventHandler(this.lblKinhPhi_Click);
            // 
            // nudKinhPhiDuKien
            // 
            this.nudKinhPhiDuKien.Location = new System.Drawing.Point(749, 24);
            this.nudKinhPhiDuKien.Maximum = new decimal(new int[] {
            2000000000,
            0,
            0,
            0});
            this.nudKinhPhiDuKien.Name = "nudKinhPhiDuKien";
            this.nudKinhPhiDuKien.Size = new System.Drawing.Size(396, 26);
            this.nudKinhPhiDuKien.TabIndex = 13;
            this.nudKinhPhiDuKien.ThousandsSeparator = true;
            this.nudKinhPhiDuKien.ValueChanged += new System.EventHandler(this.nudKinhPhiDuKien_ValueChanged);
            // 
            // lblSoLuong
            // 
            this.lblSoLuong.Location = new System.Drawing.Point(614, 68);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(129, 23);
            this.lblSoLuong.TabIndex = 14;
            this.lblSoLuong.Text = "Số lượng tối đa:";
            // 
            // nudSoLuongToiDa
            // 
            this.nudSoLuongToiDa.Location = new System.Drawing.Point(749, 63);
            this.nudSoLuongToiDa.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudSoLuongToiDa.Name = "nudSoLuongToiDa";
            this.nudSoLuongToiDa.Size = new System.Drawing.Size(396, 26);
            this.nudSoLuongToiDa.TabIndex = 15;
            this.nudSoLuongToiDa.ValueChanged += new System.EventHandler(this.nudSoLuongToiDa_ValueChanged);
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.Location = new System.Drawing.Point(614, 147);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(94, 23);
            this.lblTrangThai.TabIndex = 16;
            this.lblTrangThai.Text = "Trạng thái:";
            this.lblTrangThai.Click += new System.EventHandler(this.lblTrangThai_Click);
            // 
            // cboTrangThai
            // 
            this.cboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThai.Items.AddRange(new object[] {
            "Đang chuẩn bị",
            "Đang diễn ra",
            "Hoàn thành",
            "Hủy bỏ"});
            this.cboTrangThai.Location = new System.Drawing.Point(749, 147);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(200, 28);
            this.cboTrangThai.TabIndex = 17;
            // 
            // lblNguoiPT
            // 
            this.lblNguoiPT.Location = new System.Drawing.Point(614, 105);
            this.lblNguoiPT.Name = "lblNguoiPT";
            this.lblNguoiPT.Size = new System.Drawing.Size(129, 23);
            this.lblNguoiPT.TabIndex = 18;
            this.lblNguoiPT.Text = "Người phụ trách:";
            // 
            // nudNguoiPhuTrach
            // 
            this.nudNguoiPhuTrach.Location = new System.Drawing.Point(749, 105);
            this.nudNguoiPhuTrach.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudNguoiPhuTrach.Name = "nudNguoiPhuTrach";
            this.nudNguoiPhuTrach.Size = new System.Drawing.Size(396, 26);
            this.nudNguoiPhuTrach.TabIndex = 19;
            this.nudNguoiPhuTrach.ValueChanged += new System.EventHandler(this.nudNguoiPhuTrach_ValueChanged);
            // 
            // lblLoaiHD
            // 
            this.lblLoaiHD.Location = new System.Drawing.Point(614, 189);
            this.lblLoaiHD.Name = "lblLoaiHD";
            this.lblLoaiHD.Size = new System.Drawing.Size(129, 23);
            this.lblLoaiHD.TabIndex = 20;
            this.lblLoaiHD.Text = "Loại hoạt động:";
            // 
            // cboLoaiHD
            // 
            this.cboLoaiHD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiHD.Location = new System.Drawing.Point(749, 186);
            this.cboLoaiHD.Name = "cboLoaiHD";
            this.cboLoaiHD.Size = new System.Drawing.Size(300, 28);
            this.cboLoaiHD.TabIndex = 21;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(618, 368);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(107, 67);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(749, 368);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(106, 67);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ActivityEditForm
            // 
            this.ClientSize = new System.Drawing.Size(1157, 462);
            this.Controls.Add(this.lblTen);
            this.Controls.Add(this.txtTenHD);
            this.Controls.Add(this.lblNgay);
            this.Controls.Add(this.dtpNgayToChuc);
            this.Controls.Add(this.lblGioBD);
            this.Controls.Add(this.dtpGioBatDau);
            this.Controls.Add(this.lblGioKT);
            this.Controls.Add(this.dtpGioKetThuc);
            this.Controls.Add(this.lblDiaDiem);
            this.Controls.Add(this.txtDiaDiem);
            this.Controls.Add(this.lblMoTa);
            this.Controls.Add(this.txtMoTa);
            this.Controls.Add(this.lblKinhPhi);
            this.Controls.Add(this.nudKinhPhiDuKien);
            this.Controls.Add(this.lblSoLuong);
            this.Controls.Add(this.nudSoLuongToiDa);
            this.Controls.Add(this.lblTrangThai);
            this.Controls.Add(this.cboTrangThai);
            this.Controls.Add(this.lblNguoiPT);
            this.Controls.Add(this.nudNguoiPhuTrach);
            this.Controls.Add(this.lblLoaiHD);
            this.Controls.Add(this.cboLoaiHD);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ActivityEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thêm hoạt động";
            this.Load += new System.EventHandler(this.ActivityEditForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudKinhPhiDuKien)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSoLuongToiDa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNguoiPhuTrach)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTen;
        private System.Windows.Forms.TextBox txtTenHD;
        private System.Windows.Forms.Label lblNgay;
        private System.Windows.Forms.DateTimePicker dtpNgayToChuc;
        private System.Windows.Forms.Label lblGioBD;
        private System.Windows.Forms.DateTimePicker dtpGioBatDau;
        private System.Windows.Forms.Label lblGioKT;
        private System.Windows.Forms.DateTimePicker dtpGioKetThuc;
        private System.Windows.Forms.Label lblDiaDiem;
        private System.Windows.Forms.TextBox txtDiaDiem;
        private System.Windows.Forms.Label lblMoTa;
        private System.Windows.Forms.TextBox txtMoTa;
        private System.Windows.Forms.Label lblKinhPhi;
        private System.Windows.Forms.NumericUpDown nudKinhPhiDuKien;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.NumericUpDown nudSoLuongToiDa;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.ComboBox cboTrangThai;
        private System.Windows.Forms.Label lblNguoiPT;
        private System.Windows.Forms.NumericUpDown nudNguoiPhuTrach;
        private System.Windows.Forms.Label lblLoaiHD;
        private System.Windows.Forms.ComboBox cboLoaiHD;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
