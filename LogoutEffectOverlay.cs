using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class LogoutEffectOverlay : Form
    {
        private Timer animationTimer;
        private float doorProgress = 0f;
        private bool isClosingDoor = false;
        private bool isOpening = true;
        private Panel leftDoor;
        private Panel rightDoor;
        private Button btnLogout;
        private Button btnCancel;
        private Label lblMessage;

        public bool LogoutConfirmed { get; private set; } = false;

        public LogoutEffectOverlay()
        {
            InitializeForm();
            InitializeDoors();
            InitializeControls();
            InitializeTimer();
        }


        private void InitializeForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = Color.Black;
            this.Opacity = 0.85;  // Độ trong suốt 85%
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.DoubleBuffered = true;
        }

        private void InitializeDoors()
        {
            // Left door - cánh trái
            leftDoor = new Panel
            {
                BackColor = Color.FromArgb(20, 20, 20),
                Dock = DockStyle.None
            };
            this.Controls.Add(leftDoor);

            // Right door - cánh phải
            rightDoor = new Panel
            {
                BackColor = Color.FromArgb(20, 20, 20),
                Dock = DockStyle.None
            };
            this.Controls.Add(rightDoor);
        }

        private void InitializeControls()
        {
            // Message label
            lblMessage = new Label
            {
                Text = "BẠN CÓ CHẮC CHẮN MUỐN ĐĂNG XUẤT?",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                BackColor = Color.Transparent,
                Visible = false
            };
            this.Controls.Add(lblMessage);

            // Logout button - nút đăng xuất
            btnLogout = new Button
            {
                Text = "ĐĂNG XUẤT",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(220, 53, 69),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Size = new Size(180, 60),
                Visible = false
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.BorderColor = Color.White;
            btnLogout.Click += BtnLogout_Click;
            btnLogout.MouseEnter += (s, e) => {
                btnLogout.BackColor = Color.FromArgb(200, 35, 51);
                btnLogout.FlatAppearance.BorderSize = 2;
            };
            btnLogout.MouseLeave += (s, e) => {
                btnLogout.BackColor = Color.FromArgb(220, 53, 69);
                btnLogout.FlatAppearance.BorderSize = 0;
            };
            this.Controls.Add(btnLogout);

            // Cancel button - nút hủy
            btnCancel = new Button
            {
                Text = "HỦY",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(108, 117, 125),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Size = new Size(180, 60),
                Visible = false
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatAppearance.BorderColor = Color.White;
            btnCancel.Click += BtnCancel_Click;
            btnCancel.MouseEnter += (s, e) => {
                btnCancel.BackColor = Color.FromArgb(90, 98, 104);
                btnCancel.FlatAppearance.BorderSize = 2;
            };
            btnCancel.MouseLeave += (s, e) => {
                btnCancel.BackColor = Color.FromArgb(108, 117, 125);
                btnCancel.FlatAppearance.BorderSize = 0;
            };
            this.Controls.Add(btnCancel);

            // Đưa controls lên trên cùng
            lblMessage.BringToFront();
            btnLogout.BringToFront();
            btnCancel.BringToFront();
        }

        private void InitializeTimer()
        {
            animationTimer = new Timer { Interval = 15 };
            animationTimer.Tick += AnimationTimer_Tick;
        }

        public void ShowOverlay(Form parentForm)
        {
            this.Size = parentForm.Size;
            this.Location = parentForm.Location;

            // Đặt vị trí ban đầu cho 2 cánh cửa (đóng kín)
            leftDoor.SetBounds(0, 0, this.Width / 2, this.Height);
            rightDoor.SetBounds(this.Width / 2, 0, this.Width / 2, this.Height);

            PositionControls();
            animationTimer.Start();
        }

        private void PositionControls()
        {
            int centerX = this.Width / 2;
            int centerY = this.Height / 2;

            lblMessage.Size = new Size(500, 50);
            lblMessage.Location = new Point(centerX - 250, centerY - 100);

            btnLogout.Location = new Point(centerX - 190, centerY);
            btnCancel.Location = new Point(centerX + 10, centerY);
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (isOpening)
            {
                // Mở cửa ra
                doorProgress += 0.06f;

                if (doorProgress >= 1f)
                {
                    doorProgress = 1f;
                    isOpening = false;
                    animationTimer.Stop();

                    // Hiện nút khi cửa mở xong
                    lblMessage.Visible = true;
                    btnLogout.Visible = true;
                    btnCancel.Visible = true;
                }

                UpdateDoorPositions();
            }
            else if (isClosingDoor)
            {
                // Đóng cửa lại (phủ kín màn hình)
                doorProgress -= 0.08f;

                if (doorProgress <= 0f)
                {
                    doorProgress = 0f;
                    animationTimer.Stop();

                    if (LogoutConfirmed)
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        this.DialogResult = DialogResult.Cancel;
                    }
                    this.Close();
                }

                UpdateDoorPositions();
            }
        }

        private void UpdateDoorPositions()
        {
            int doorOffset = (int)(this.Width / 2 * doorProgress);

            // Cánh trái di chuyển sang trái
            leftDoor.SetBounds(-doorOffset, 0, this.Width / 2, this.Height);

            // Cánh phải di chuyển sang phải
            rightDoor.SetBounds(this.Width / 2 + doorOffset, 0, this.Width / 2, this.Height);
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            LogoutConfirmed = true;

            // Ẩn các nút và label
            btnLogout.Visible = false;
            btnCancel.Visible = false;
            lblMessage.Visible = false;

            // Bắt đầu đóng cửa (phủ kín màn hình)
            isClosingDoor = true;
            animationTimer.Start();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            LogoutConfirmed = false;

            // Ẩn các nút và label
            btnLogout.Visible = false;
            btnCancel.Visible = false;
            lblMessage.Visible = false;

            // Đóng cửa lại
            isClosingDoor = true;
            animationTimer.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            animationTimer?.Stop();
            animationTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}