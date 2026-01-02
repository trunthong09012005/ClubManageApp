using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;

namespace ClubManageApp
{
    public partial class Login : Form
    {
        private string connectionString = ConnectionHelper.ConnectionString;

        // Panel loading overlay
        private Panel loadingPanel;
        private Label loadingLabel;
        private Timer animationTimer;
        private int animationFrame = 0;
        private float birdX = -100;
        private float birdY = 100;
        private bool movingRight = true;
        private float cloudOffset = 0;

        // Animation timer
        private Timer successTimer;
        private int successScale = 0;
        private Panel successPanel;

        // Random cố định để animation mượt
        private Random starRandom = new Random(12345);
        private Random birdRandom = new Random(54321);

        public Login()
        {
            InitializeComponent();

            txttaikhoan.KeyDown += TxtBox_KeyDown;
            txtmatkhau.KeyDown += TxtBox_KeyDown;

            // Khởi tạo loading panel
            InitializeLoadingPanel();
            InitializeSuccessPanel();

            // Ghi nhớ tài khoản
            if (!string.IsNullOrEmpty(Properties.Settings.Default.SavedUser))
            {
                txttaikhoan.Text = Properties.Settings.Default.SavedUser;

                var savedPass = Properties.Settings.Default.SavedPass;
                if (!string.IsNullOrEmpty(savedPass) && !IsHex(savedPass))
                {
                    txtmatkhau.Text = savedPass;
                    cbghinho.Checked = true;
                }
                else
                {
                    txtmatkhau.Text = "";
                    cbghinho.Checked = false;
                }
            }

            // Fade in animation khi form load
            this.Opacity = 0;
            Timer fadeInTimer = new Timer { Interval = 20 };
            fadeInTimer.Tick += (s, e) =>
            {
                if (this.Opacity < 1)
                    this.Opacity += 0.05;
                else
                    fadeInTimer.Stop();
            };
            fadeInTimer.Start();
        }

        private void InitializeLoadingPanel()
        {
            // Tạo overlay panel với DoubleBuffering để giảm giật lag
            loadingPanel = new Panel
            {
                Size = this.ClientSize,
                Location = new Point(0, 0),
                Visible = false
            };

            // QUAN TRỌNG: Bật DoubleBuffering để vẽ mượt
            loadingPanel.GetType().GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(loadingPanel, true, null);

            loadingPanel.Paint += LoadingPanel_Paint;
            this.Controls.Add(loadingPanel);
            loadingPanel.BringToFront();

            // Loading label với style đẹp hơn
            loadingLabel = new Label
            {
                Text = "Đang xác thực tài khoản...",
                Font = new Font("Segoe UI", 13, FontStyle.Regular),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(300, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            loadingLabel.Location = new Point(
                (loadingPanel.Width - loadingLabel.Width) / 2,
                loadingPanel.Height - 100
            );
            loadingPanel.Controls.Add(loadingLabel);

            // Timer với interval lớn hơn để mượt hơn (60 FPS thay vì ~33 FPS)
            animationTimer = new Timer { Interval = 16 }; // ~60 FPS
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            animationFrame++;

            // Di chuyển chim với tốc độ chậm hơn
            if (movingRight)
            {
                birdX += 2.5f; // Giảm từ 4 xuống 2.5
                if (birdX > loadingPanel.Width + 50)
                {
                    movingRight = false;
                    birdX = loadingPanel.Width + 50;
                    birdY = (float)(birdRandom.NextDouble() * 100 + 50);
                }
            }
            else
            {
                birdX -= 2.5f;
                if (birdX < -100)
                {
                    movingRight = true;
                    birdX = -100;
                    birdY = (float)(birdRandom.NextDouble() * 100 + 50);
                }
            }

            // Di chuyển mây chậm hơn
            cloudOffset = (cloudOffset + 0.3f) % 200; // Giảm từ 0.5f

            // Làm chữ nhấp nháy nhẹ
            int alpha = (int)(Math.Sin(animationFrame * 0.05) * 30 + 225); // Giảm tốc độ
            loadingLabel.ForeColor = Color.FromArgb(alpha, 255, 255, 255);

            loadingPanel.Invalidate();
        }

        private void LoadingPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Gradient background từ xanh dương đến tím
            using (LinearGradientBrush brush = new LinearGradientBrush(
                loadingPanel.ClientRectangle,
                Color.FromArgb(200, 41, 128, 185),
                Color.FromArgb(200, 142, 68, 173),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, loadingPanel.ClientRectangle);
            }

            // Vẽ các ngôi sao lấp lánh (dùng Random cố định)
            starRandom = new Random(12345); // Reset seed
            for (int i = 0; i < 30; i++)
            {
                float x = starRandom.Next(loadingPanel.Width);
                float y = starRandom.Next(loadingPanel.Height / 2);
                float twinkle = (float)Math.Sin((animationFrame + i * 10) * 0.05); // Chậm hơn
                int alpha = (int)(twinkle * 100 + 155);

                using (SolidBrush starBrush = new SolidBrush(Color.FromArgb(alpha, 255, 255, 255)))
                {
                    g.FillEllipse(starBrush, x, y, 3, 3);
                }
            }

            // Vẽ mây mờ mờ
            DrawCloud(g, cloudOffset, 80, 0.3f);
            DrawCloud(g, cloudOffset - 200, 150, 0.2f);
            DrawCloud(g, cloudOffset + 100, 200, 0.25f);

            // Vẽ chim bay
            DrawBird(g, birdX, birdY + (float)(Math.Sin(animationFrame * 0.15) * 10), movingRight);

            // Vẽ vòng tròn loading quay (chậm hơn)
            float centerX = loadingPanel.Width / 2;
            float centerY = loadingPanel.Height / 2 - 50;

            for (int i = 0; i < 8; i++)
            {
                float angle = (animationFrame * 0.08f + i * 45) * (float)Math.PI / 180; // Giảm tốc độ
                float dotX = centerX + (float)Math.Cos(angle) * 40;
                float dotY = centerY + (float)Math.Sin(angle) * 40;

                int alpha = (int)(255 - (i * 30));
                float size = 12 - i * 1.2f;

                using (SolidBrush dotBrush = new SolidBrush(Color.FromArgb(alpha, 255, 255, 255)))
                {
                    g.FillEllipse(dotBrush, dotX - size / 2, dotY - size / 2, size, size);
                }
            }
        }

        private void DrawCloud(Graphics g, float x, float y, float alpha)
        {
            using (SolidBrush cloudBrush = new SolidBrush(Color.FromArgb((int)(alpha * 100), 255, 255, 255)))
            {
                g.FillEllipse(cloudBrush, x, y, 60, 30);
                g.FillEllipse(cloudBrush, x + 20, y - 10, 50, 35);
                g.FillEllipse(cloudBrush, x + 40, y, 60, 30);
            }
        }

        private void DrawBird(Graphics g, float x, float y, bool facingRight)
        {
            // Tính toán góc vỗ cánh (chậm hơn)
            float wingAngle = (float)Math.Sin(animationFrame * 0.2) * 20; // Giảm từ 0.3

            using (Pen pen = new Pen(Color.White, 3))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;

                if (facingRight)
                {
                    PointF[] leftWing = {
                        new PointF(x, y),
                        new PointF(x - 15, y - 15 + wingAngle),
                        new PointF(x - 25, y - 10 + wingAngle)
                    };
                    g.DrawCurve(pen, leftWing, 0.5f);

                    PointF[] rightWing = {
                        new PointF(x, y),
                        new PointF(x - 15, y + 15 - wingAngle),
                        new PointF(x - 25, y + 10 - wingAngle)
                    };
                    g.DrawCurve(pen, rightWing, 0.5f);
                }
                else
                {
                    PointF[] leftWing = {
                        new PointF(x, y),
                        new PointF(x + 15, y - 15 + wingAngle),
                        new PointF(x + 25, y - 10 + wingAngle)
                    };
                    g.DrawCurve(pen, leftWing, 0.5f);

                    PointF[] rightWing = {
                        new PointF(x, y),
                        new PointF(x + 15, y + 15 - wingAngle),
                        new PointF(x + 25, y + 10 - wingAngle)
                    };
                    g.DrawCurve(pen, rightWing, 0.5f);
                }
            }

            using (SolidBrush bodyBrush = new SolidBrush(Color.White))
            {
                g.FillEllipse(bodyBrush, x - 5, y - 5, 10, 10);
            }
        }

        private void InitializeSuccessPanel()
        {
            successPanel = new Panel
            {
                Size = this.ClientSize,
                Location = new Point(0, 0),
                BackColor = Color.Transparent,
                Visible = false
            };

            // Bật DoubleBuffering cho success panel
            successPanel.GetType().GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(successPanel, true, null);

            successPanel.Paint += SuccessPanel_Paint;
            this.Controls.Add(successPanel);
            successPanel.BringToFront();

            // Timer mượt hơn
            successTimer = new Timer { Interval = 16 }; // ~60 FPS
            successTimer.Tick += SuccessTimer_Tick;
        }

        private void SuccessPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float progress = successScale / 100f;
            int centerX = successPanel.Width / 2;
            int centerY = successPanel.Height / 2;

            if (progress > 0)
            {
                using (PathGradientBrush bgBrush = new PathGradientBrush(
                    new PointF[] {
                        new PointF(0, 0),
                        new PointF(successPanel.Width, 0),
                        new PointF(successPanel.Width, successPanel.Height),
                        new PointF(0, successPanel.Height)
                    }))
                {
                    bgBrush.CenterPoint = new PointF(centerX, centerY);
                    bgBrush.CenterColor = Color.FromArgb((int)(180 * progress), 76, 175, 80);
                    bgBrush.SurroundColors = new Color[] {
                        Color.FromArgb((int)(100 * progress), 46, 125, 50),
                        Color.FromArgb((int)(100 * progress), 46, 125, 50),
                        Color.FromArgb((int)(100 * progress), 46, 125, 50),
                        Color.FromArgb((int)(100 * progress), 46, 125, 50)
                    };
                    g.FillRectangle(bgBrush, successPanel.ClientRectangle);
                }
            }

            if (progress > 0.3f)
            {
                Random rand = new Random(42);
                int particleCount = 24;
                for (int i = 0; i < particleCount; i++)
                {
                    float angle = (i * 360f / particleCount) * (float)Math.PI / 180f;
                    float distance = 100 * (progress - 0.3f) * 2.5f;
                    float particleX = centerX + (float)Math.Cos(angle) * distance;
                    float particleY = centerY + (float)Math.Sin(angle) * distance;

                    float particleSize = 8 + (float)Math.Sin(progress * 10 + i) * 4;
                    int alpha = (int)(255 * (1 - (progress - 0.3f) * 1.4f));
                    if (alpha < 0) alpha = 0;

                    Color particleColor = i % 3 == 0 ? Color.FromArgb(alpha, 255, 235, 59) :
                                         i % 3 == 1 ? Color.FromArgb(alpha, 255, 255, 255) :
                                         Color.FromArgb(alpha, 129, 212, 250);

                    using (SolidBrush particleBrush = new SolidBrush(particleColor))
                    {
                        g.FillEllipse(particleBrush, particleX - particleSize / 2, particleY - particleSize / 2,
                                    particleSize, particleSize);
                    }
                }
            }

            for (int i = 3; i >= 0; i--)
            {
                int glowSize = (int)(200 * progress) + i * 30;
                int glowAlpha = (int)(40 * progress) - i * 8;
                if (glowAlpha > 0)
                {
                    using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(glowAlpha, 255, 255, 255)))
                    {
                        g.FillEllipse(glowBrush,
                            centerX - glowSize / 2,
                            centerY - glowSize / 2,
                            glowSize, glowSize);
                    }
                }
            }

            int mainSize = (int)(160 * progress);
            using (SolidBrush mainBrush = new SolidBrush(Color.FromArgb((int)(255 * progress), 76, 175, 80)))
            {
                g.FillEllipse(mainBrush,
                    centerX - mainSize / 2,
                    centerY - mainSize / 2,
                    mainSize, mainSize);
            }

            if (progress > 0.5f)
            {
                int borderSize = (int)(170 * progress);
                using (Pen borderPen = new Pen(Color.FromArgb((int)(255 * (progress - 0.5f) * 2), 255, 255, 255), 4))
                {
                    g.DrawEllipse(borderPen,
                        centerX - borderSize / 2,
                        centerY - borderSize / 2,
                        borderSize, borderSize);
                }
            }

            if (progress > 0.4f)
            {
                float checkProgress = (progress - 0.4f) / 0.6f;
                if (checkProgress > 1f) checkProgress = 1f;

                using (Pen checkPen = new Pen(Color.White, 12))
                {
                    checkPen.StartCap = LineCap.Round;
                    checkPen.EndCap = LineCap.Round;

                    PointF p1 = new PointF(centerX - 35, centerY);
                    PointF p2 = new PointF(centerX - 10, centerY + 25);

                    if (checkProgress <= 0.4f)
                    {
                        float t = checkProgress / 0.4f;
                        PointF currentP2 = new PointF(
                            p1.X + (p2.X - p1.X) * t,
                            p1.Y + (p2.Y - p1.Y) * t
                        );
                        g.DrawLine(checkPen, p1, currentP2);
                    }
                    else
                    {
                        g.DrawLine(checkPen, p1, p2);

                        PointF p3 = new PointF(centerX + 40, centerY - 30);
                        float t = (checkProgress - 0.4f) / 0.6f;
                        PointF currentP3 = new PointF(
                            p2.X + (p3.X - p2.X) * t,
                            p2.Y + (p3.Y - p2.Y) * t
                        );
                        g.DrawLine(checkPen, p2, currentP3);
                    }
                }
            }

            if (progress > 0.6f)
            {
                float textProgress = (progress - 0.6f) / 0.4f;
                string text = "Đăng nhập thành công!";
                using (Font font = new Font("Segoe UI", 24, FontStyle.Bold))
                {
                    SizeF textSize = g.MeasureString(text, font);
                    float textX = centerX - textSize.Width / 2;
                    float textY = centerY + 120;

                    using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb((int)(100 * textProgress), 0, 0, 0)))
                    {
                        g.DrawString(text, font, shadowBrush, textX + 2, textY + 2);
                    }

                    using (SolidBrush textBrush = new SolidBrush(Color.FromArgb((int)(255 * textProgress), 255, 255, 255)))
                    {
                        g.DrawString(text, font, textBrush, textX, textY);
                    }
                }

                string subText = "Đang chuyển hướng...";
                using (Font subFont = new Font("Segoe UI", 12, FontStyle.Regular))
                {
                    SizeF subSize = g.MeasureString(subText, subFont);
                    float subX = centerX - subSize.Width / 2;
                    float subY = centerY + 160;

                    using (SolidBrush subBrush = new SolidBrush(Color.FromArgb((int)(200 * textProgress), 255, 255, 255)))
                    {
                        g.DrawString(subText, subFont, subBrush, subX, subY);
                    }
                }
            }
        }

        private void SuccessTimer_Tick(object sender, EventArgs e)
        {
            if (successScale < 100)
            {
                successScale += 3; // Giảm từ 4 để mượt hơn
                if (successScale > 100) successScale = 100;
                successPanel.Invalidate();
            }
            else
            {
                successTimer.Stop();
            }
        }

        private void ShowLoading()
        {
            loadingPanel.Visible = true;
            loadingPanel.BringToFront();
            animationFrame = 0;
            birdX = -100;
            birdY = 100;
            movingRight = true;
            cloudOffset = 0;
            animationTimer.Start();

            btndangnhap.Enabled = false;
        }

        private void HideLoading()
        {
            animationTimer.Stop();
            loadingPanel.Visible = false;
            btndangnhap.Enabled = true;
        }

        private async Task ShowSuccessAnimation()
        {
            HideLoading();

            successScale = 0;
            successPanel.Visible = true;
            successPanel.BringToFront();
            successTimer.Start();

            // Tăng thời gian chờ từ 2s lên 2.5s
            await Task.Delay(2500);
        }

        private void TxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (sender == txttaikhoan)
                    txtmatkhau.Focus();
                else
                    btndangnhap.PerformClick();

                e.SuppressKeyPress = true;
            }
        }

        private async void Btndangnhap_Click(object sender, EventArgs e)
        {
            string username = txttaikhoan.Text.Trim();
            string password = txtmatkhau.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShakeForm();
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ShowLoading();

            // Tăng delay từ 800ms lên 2000ms (2 giây)
            await Task.Delay(2000);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string query = @"
                        SELECT MatKhau, QuyenHan, TrangThai, MaTV
                        FROM TaiKhoan
                        WHERE TenDN = @user";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", username);

                        using (SqlDataReader reader = (SqlDataReader)await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.Read())
                            {
                                HideLoading();
                                ShakeForm();
                                MessageBox.Show("Tên đăng nhập không tồn tại hoặc sai!", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            string dbHash = reader["MatKhau"]?.ToString().Trim() ?? "";
                            string role = reader["QuyenHan"]?.ToString().Trim() ?? "";
                            string status = reader["TrangThai"]?.ToString().Trim() ?? "";
                            int maTV = Convert.ToInt32(reader["MaTV"]);

                            string hashedPassword = HashPassword(password);

                            if (!string.Equals(dbHash, hashedPassword, StringComparison.OrdinalIgnoreCase))
                            {
                                HideLoading();
                                ShakeForm();
                                if (dbHash.Length < 64)
                                {
                                    MessageBox.Show("Sai mật khẩu. (Debug: mật khẩu lưu trên DB có độ dài khác 64 — kiểm tra cột `MatKhau`)", "Lỗi",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                return;
                            }

                            // ✅ KIỂM TRA TRẠNG THÁI TÀI KHOẢN
                            if (status.Equals("Khóa", StringComparison.OrdinalIgnoreCase))
                            {
                                HideLoading();
                                MessageBox.Show(
                                    "❌ TÀI KHOẢN ĐÃ BỊ KHÓA!\n\n" +
                                    "Tài khoản của bạn đã bị khóa bởi quản trị viên.\n" +
                                    "Vui lòng liên hệ Ban quản lý để được hỗ trợ.",
                                    "Tài khoản bị khóa",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                return;
                            }

                            if (status.Equals("Chờ kích hoạt", StringComparison.OrdinalIgnoreCase))
                            {
                                HideLoading();
                                MessageBox.Show(
                                    "⏳ TÀI KHOẢN ĐANG CHỜ KÍCH HOẠT!\n\n" +
                                    "Tài khoản của bạn đang chờ được kích hoạt bởi quản trị viên.\n" +
                                    "Vui lòng liên hệ Ban quản lý để được hỗ trợ.",
                                    "Tài khoản chờ kích hoạt",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                                return;
                            }

                            if (!status.Equals("Hoạt động", StringComparison.OrdinalIgnoreCase))
                            {
                                HideLoading();
                                MessageBox.Show(
                                    $"❌ KHÔNG THỂ ĐĂNG NHẬP!\n\n" +
                                    $"Trạng thái tài khoản: {status}\n" +
                                    $"Chỉ tài khoản có trạng thái 'Hoạt động' mới có thể đăng nhập.\n\n" +
                                    $"Vui lòng liên hệ Ban quản lý để được hỗ trợ.",
                                    "Lỗi trạng thái tài khoản",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                return;
                            }

                            if (cbghinho.Checked)
                            {
                                Properties.Settings.Default.SavedUser = username;
                                Properties.Settings.Default.SavedPass = password;
                                Properties.Settings.Default.Save();
                            }
                            else
                            {
                                Properties.Settings.Default.SavedUser = "";
                                Properties.Settings.Default.SavedPass = "";
                                Properties.Settings.Default.Save();
                            }

                            await ShowSuccessAnimation();
                            await FadeOut();

                            this.Hide();

                            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                            {
                                DashBoard main = new DashBoard(role, username, maTV);
                                main.FormClosed += (s, args) => Application.Exit();
                                main.Show();
                            }
                            else if (role.Equals("Thành viên", StringComparison.OrdinalIgnoreCase) ||
                                     role.Equals("Quản trị viên", StringComparison.OrdinalIgnoreCase))
                            {
                                MemberDashboard memberForm = new MemberDashboard(role, username, maTV);
                                memberForm.FormClosed += (s, args) => Application.Exit();
                                memberForm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Quyền hạn không hợp lệ!", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Show();
                                this.Opacity = 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HideLoading();
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task FadeOut()
        {
            while (this.Opacity > 0)
            {
                this.Opacity -= 0.05;
                await Task.Delay(20);
            }
        }

        private void ShakeForm()
        {
            var original = this.Location;
            var shake = new Timer { Interval = 50 };
            int shakeCount = 0;

            shake.Tick += (s, e) =>
            {
                switch (shakeCount % 4)
                {
                    case 0: this.Location = new Point(original.X + 10, original.Y); break;
                    case 1: this.Location = new Point(original.X - 10, original.Y); break;
                    case 2: this.Location = new Point(original.X + 10, original.Y); break;
                    case 3: this.Location = original; break;
                }
                shakeCount++;
                if (shakeCount >= 8)
                {
                    shake.Stop();
                    this.Location = original;
                }
            };
            shake.Start();
        }

        private static string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password ?? "");
                var hash = sha.ComputeHash(bytes);
                var sb = new StringBuilder();
                foreach (var b in hash)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        private bool IsHex(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length != 64)
                return false;
            foreach (char c in text)
            {
                bool ok = (c >= '0' && c <= '9') ||
                          (c >= 'a' && c <= 'f') ||
                          (c >= 'A' && c <= 'F');
                if (!ok) return false;
            }
            return true;
        }

        private void Lbdangky_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormSignUp formDangKy = new FormSignUp();
            formDangKy.FormClosed += (s, args) => this.Show();
            formDangKy.Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }

        private void ptLogo2_Click(object sender, EventArgs e)
        {

        }
    }
}