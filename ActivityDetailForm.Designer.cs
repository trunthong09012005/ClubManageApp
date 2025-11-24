namespace ClubManageApp
{
    partial class ActivityDetailForm
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
            this.components = new System.ComponentModel.Container();

            this.Text = "Chi tiết hoạt động";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;

            this.lblTitle = new System.Windows.Forms.Label();
            this.txtDetails = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();

            this.lblTitle.Left = 20; this.lblTitle.Top = 20; this.lblTitle.Width = 720; this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold); this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(94, 148, 255);

            this.txtDetails.Left = 20; this.txtDetails.Top = 70; this.txtDetails.Width = 740; this.txtDetails.Height = 420; this.txtDetails.Multiline = true; this.txtDetails.ReadOnly = true; this.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            this.btnClose.Text = "Đóng"; this.btnClose.Left = 20; this.btnClose.Top = 510; this.btnClose.Width = 100; this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtDetails);
            this.Controls.Add(this.btnClose);
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtDetails;
        private System.Windows.Forms.Button btnClose;
    }
}
