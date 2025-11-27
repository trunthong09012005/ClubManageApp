using System;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class SendMailForm : Form
    {
        private Participant recipient;
        private TextBox txtSender;
        private TextBox txtPassword;
        private TextBox txtSmtp;
        private NumericUpDown nudPort;
        private CheckBox chkSsl;
        private TextBox txtSubject;
        private TextBox txtBody;
        private Button btnSend;
        private Button btnCancel;

        public SendMailForm(Participant p)
        {
            recipient = p;
            InitializeComponentManual();
        }

        // This method builds the UI in code so you can still edit by designer if you want later.
        private void InitializeComponentManual()
        {
            this.Text = "Gửi thông báo";
            this.ClientSize = new Size(540, 520);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;

            var tl = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), ColumnCount = 2 };
            tl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            tl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            tl.RowCount = 9;
            for (int i = 0; i < tl.RowCount; i++) tl.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            tl.RowStyles[7] = new RowStyle(SizeType.Percent, 100F);
            tl.RowStyles[8] = new RowStyle(SizeType.Absolute, 54);

            tl.Controls.Add(new Label { Text = "Người nhận:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
            tl.Controls.Add(new Label { Text = recipient?.HoTen + " <" + recipient?.Email + ">", Anchor = AnchorStyles.Left, AutoSize = true }, 1, 0);

            tl.Controls.Add(new Label { Text = "Email gửi:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 1);
            txtSender = new TextBox { Dock = DockStyle.Fill };
            tl.Controls.Add(txtSender, 1, 1);

            tl.Controls.Add(new Label { Text = "Mật khẩu:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 2);
            txtPassword = new TextBox { Dock = DockStyle.Fill, UseSystemPasswordChar = true };
            tl.Controls.Add(txtPassword, 1, 2);

            tl.Controls.Add(new Label { Text = "SMTP host:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 3);
            txtSmtp = new TextBox { Dock = DockStyle.Fill, Text = "smtp.gmail.com" };
            tl.Controls.Add(txtSmtp, 1, 3);

            tl.Controls.Add(new Label { Text = "Port:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 4);
            nudPort = new NumericUpDown { Minimum = 1, Maximum = 65535, Value = 587 };
            tl.Controls.Add(nudPort, 1, 4);

            tl.Controls.Add(new Label { Text = "SSL/TLS:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 5);
            chkSsl = new CheckBox { Checked = true, Anchor = AnchorStyles.Left };
            tl.Controls.Add(chkSsl, 1, 5);

            tl.Controls.Add(new Label { Text = "Tiêu đề:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 6);
            txtSubject = new TextBox { Dock = DockStyle.Fill, Text = "Thông báo từ CLB" };
            tl.Controls.Add(txtSubject, 1, 6);

            tl.Controls.Add(new Label { Text = "Nội dung:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 7);
            txtBody = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Fill, Text = $"Xin chào {recipient?.HoTen},\n\nXin vui lòng tham gia cuộc họp...\n" };
            tl.Controls.Add(txtBody, 1, 7);

            var btnPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            btnSend = new Button { Text = "Gửi", Width = 90 };
            btnCancel = new Button { Text = "Hủy", Width = 90 };
            btnSend.Click += BtnSend_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            btnPanel.Controls.Add(btnCancel);
            btnPanel.Controls.Add(btnSend);

            tl.Controls.Add(new Label { Text = "", AutoSize = true }, 0, 8);
            tl.Controls.Add(btnPanel, 1, 8);

            this.Controls.Add(tl);
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            try
            {
                var from = txtSender.Text.Trim();
                var password = txtPassword.Text;
                var smtpHost = txtSmtp.Text.Trim();
                var port = (int)nudPort.Value;
                var enableSsl = chkSsl.Checked;
                var subject = txtSubject.Text;
                var body = txtBody.Text;

                if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(recipient?.Email))
                {
                    MessageBox.Show("Vui lòng nhập email người gửi, mật khẩu và đảm bảo người nhận có email.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSend.Enabled = true;
                    return;
                }

                await Task.Run(() =>
                {
                    using (var msg = new MailMessage())
                    {
                        msg.From = new MailAddress(from);
                        msg.To.Add(new MailAddress(recipient.Email));
                        msg.Subject = subject;
                        msg.Body = body;
                        msg.IsBodyHtml = false;

                        using (var client = new SmtpClient(smtpHost, port))
                        {
                            client.EnableSsl = enableSsl;
                            client.Credentials = new NetworkCredential(from, password);
                            client.Send(msg);
                        }
                    }
                });

                MessageBox.Show("Gửi email thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi email thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSend.Enabled = true;
            }
        }
    }
}
