namespace ClubManageApp
{
    partial class NotificationEditForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.lblRecipient = new System.Windows.Forms.Label();
            this.txtRecipient = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.lblNgayGui = new System.Windows.Forms.Label();
            this.dtpNgayGui = new System.Windows.Forms.DateTimePicker();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(12, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(49, 13);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Tiêu đề:";

            // txtTitle
            this.txtTitle.Location = new System.Drawing.Point(12, 28);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(660, 20);
            this.txtTitle.TabIndex = 1;

            // lblContent
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(12, 52);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(49, 13);
            this.lblContent.TabIndex = 2;
            this.lblContent.Text = "Nội dung:";

            // txtContent
            this.txtContent.Location = new System.Drawing.Point(12, 68);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContent.Size = new System.Drawing.Size(660, 140);
            this.txtContent.TabIndex = 3;

            // lblType
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(12, 216);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(35, 13);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Loại:";

            // cboType
            this.cboType.FormattingEnabled = true;
            this.cboType.Location = new System.Drawing.Point(12, 232);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(200, 21);
            this.cboType.TabIndex = 5;
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;

            // lblRecipient
            this.lblRecipient.AutoSize = true;
            this.lblRecipient.Location = new System.Drawing.Point(280, 216);
            this.lblRecipient.Name = "lblRecipient";
            this.lblRecipient.Size = new System.Drawing.Size(59, 13);
            this.lblRecipient.TabIndex = 6;
            this.lblRecipient.Text = "Đối tượng:";

            // txtRecipient
            this.txtRecipient.Location = new System.Drawing.Point(280, 232);
            this.txtRecipient.Name = "txtRecipient";
            this.txtRecipient.Size = new System.Drawing.Size(200, 20);
            this.txtRecipient.TabIndex = 7;

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(530, 216);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(62, 13);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Trạng thái:";

            // cboStatus
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Location = new System.Drawing.Point(530, 232);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(142, 21);
            this.cboStatus.TabIndex = 9;
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // lblNgayGui
            this.lblNgayGui.AutoSize = true;
            this.lblNgayGui.Location = new System.Drawing.Point(12, 265);
            this.lblNgayGui.Name = "lblNgayGui";
            this.lblNgayGui.Size = new System.Drawing.Size(60, 13);
            this.lblNgayGui.TabIndex = 10;
            this.lblNgayGui.Text = "Ngày gửi:";

            // dtpNgayGui
            this.dtpNgayGui.Location = new System.Drawing.Point(12, 282);
            this.dtpNgayGui.Name = "dtpNgayGui";
            this.dtpNgayGui.Size = new System.Drawing.Size(200, 20);
            this.dtpNgayGui.TabIndex = 11;
            this.dtpNgayGui.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayGui.CustomFormat = "dd/MM/yyyy HH:mm";

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(545, 324);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 30);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(612, 324);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 30);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // NotificationEditForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 366);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dtpNgayGui);
            this.Controls.Add(this.lblNgayGui);
            this.Controls.Add(this.cboStatus);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtRecipient);
            this.Controls.Add(this.lblRecipient);
            this.Controls.Add(this.cboType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NotificationEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chỉnh sửa thông báo";
            this.Load += new System.EventHandler(this.NotificationEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Label lblRecipient;
        private System.Windows.Forms.TextBox txtRecipient;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label lblNgayGui;
        private System.Windows.Forms.DateTimePicker dtpNgayGui;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
