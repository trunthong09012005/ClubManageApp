using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class SignUp : Form
    {
        // Thông tin thành viên từ FormSignUp
        private int? maTV = null;
        private string hoTenThanhVien = "";
        private string emailThanhVien = "";

        // Panel cho animation thành công
        private Panel successPanel;
        private Timer successTimer;
        private int successScale = 0;

        // Constructor mặc định (khi đăng ký từ đầu)
        public SignUp()
        {
            InitializeComponent();
            InitializeSuccessPanel();
        }

        // Constructor nhận thông tin từ FormSignUp
        public SignUp(int maTV, string hoTen, string email)
        {
            InitializeComponent();
            InitializeSuccessPanel();
            this.maTV = maTV;
            this.hoTenThanhVien = hoTen;
            this.emailThanhVien = email;

            // Hiển thị thông tin thành viên
            ShowMemberInfo();
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

            // Bật DoubleBuffering
            successPanel.GetType().GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(successPanel, true, null);

            successPanel.Paint += SuccessPanel_Paint;
            this.Controls.Add(successPanel);
            successPanel.BringToFront();

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

            // Background gradient
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

            // Pháo hoa
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

            // Vòng tròn phát sáng
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

            // Vòng tròn chính
            int mainSize = (int)(160 * progress);
            using (SolidBrush mainBrush = new SolidBrush(Color.FromArgb((int)(255 * progress), 76, 175, 80)))
            {
                g.FillEllipse(mainBrush,
                    centerX - mainSize / 2,
                    centerY - mainSize / 2,
                    mainSize, mainSize);
            }

            // Viền trắng
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

            // Dấu tick
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

            // Text
            if (progress > 0.6f)
            {
                float textProgress = (progress - 0.6f) / 0.4f;
                string text = "Đăng ký thành công!";
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

                string subText = "Bạn có thể đăng nhập ngay!";
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
                successScale += 3;
                if (successScale > 100) successScale = 100;
                successPanel.Invalidate();
            }
            else
            {
                successTimer.Stop();
            }
        }

        private async Task ShowSuccessAnimation()
        {
            successScale = 0;
            successPanel.Visible = true;
            successPanel.BringToFront();
            successTimer.Start();

            await Task.Delay(2500); // Hiển thị 2.5 giây
        }

        private async Task FadeOut()
        {
            while (this.Opacity > 0)
            {
                this.Opacity -= 0.05;
                await Task.Delay(20);
            }
        }

        // Hiển thị thông tin thành viên đã đăng ký
        private void ShowMemberInfo()
        {
            if (maTV.HasValue)
            {
                Label lblInfo = new Label
                {
                    Text = $"Đang tạo tài khoản cho:\n{hoTenThanhVien}\n{emailThanhVien}\nMã TV: {maTV}",
                    AutoSize = true,
                    Location = new Point(15, 420),
                    ForeColor = Color.DarkGreen,
                    Font = new Font("Segoe UI", 8, FontStyle.Italic)
                };
                this.guna2Panel1.Controls.Add(lblInfo);
                lblInfo.BringToFront();
            }
        }

        private void InitializeComponent()
        {
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.txtNhapLai = new Guna.UI2.WinForms.Guna2TextBox();
            this.lbnhaplaimatkhau = new System.Windows.Forms.Label();
            this.lbdangnhap = new System.Windows.Forms.Label();
            this.pt2 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.btndangky = new Guna.UI2.WinForms.Guna2Button();
            this.txtmatkhau = new Guna.UI2.WinForms.Guna2TextBox();
            this.txttaikhoan = new Guna.UI2.WinForms.Guna2TextBox();
            this.ptlogo = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.lbtaikhoan = new System.Windows.Forms.Label();
            this.lbmatkhau = new System.Windows.Forms.Label();
            this.lbTenclb = new System.Windows.Forms.Label();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.lbdangky = new System.Windows.Forms.Label();
            this.ptLogo2 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pt2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptlogo)).BeginInit();
            this.guna2Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptLogo2)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(217)))), ((int)(((byte)(255)))));
            this.guna2Panel1.Controls.Add(this.txtNhapLai);
            this.guna2Panel1.Controls.Add(this.lbnhaplaimatkhau);
            this.guna2Panel1.Controls.Add(this.lbdangnhap);
            this.guna2Panel1.Controls.Add(this.pt2);
            this.guna2Panel1.Controls.Add(this.btndangky);
            this.guna2Panel1.Controls.Add(this.txtmatkhau);
            this.guna2Panel1.Controls.Add(this.txttaikhoan);
            this.guna2Panel1.Controls.Add(this.ptlogo);
            this.guna2Panel1.Controls.Add(this.lbtaikhoan);
            this.guna2Panel1.Controls.Add(this.lbmatkhau);
            this.guna2Panel1.Controls.Add(this.lbTenclb);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(300, 450);
            this.guna2Panel1.TabIndex = 1;
            // 
            // txtNhapLai
            // 
            this.txtNhapLai.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNhapLai.DefaultText = "";
            this.txtNhapLai.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtNhapLai.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtNhapLai.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNhapLai.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNhapLai.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNhapLai.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNhapLai.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNhapLai.Location = new System.Drawing.Point(89, 313);
            this.txtNhapLai.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNhapLai.Name = "txtNhapLai";
            this.txtNhapLai.PlaceholderText = "Nhập lại mật khẩu...";
            this.txtNhapLai.SelectedText = "";
            this.txtNhapLai.Size = new System.Drawing.Size(177, 31);
            this.txtNhapLai.TabIndex = 9;
            this.txtNhapLai.UseSystemPasswordChar = true;
            // 
            // lbnhaplaimatkhau
            // 
            this.lbnhaplaimatkhau.AutoSize = true;
            this.lbnhaplaimatkhau.ForeColor = System.Drawing.Color.Purple;
            this.lbnhaplaimatkhau.Location = new System.Drawing.Point(12, 319);
            this.lbnhaplaimatkhau.Name = "lbnhaplaimatkhau";
            this.lbnhaplaimatkhau.Size = new System.Drawing.Size(57, 16);
            this.lbnhaplaimatkhau.TabIndex = 8;
            this.lbnhaplaimatkhau.Text = "Nhập lại";
            // 
            // lbdangnhap
            // 
            this.lbdangnhap.AutoSize = true;
            this.lbdangnhap.ForeColor = System.Drawing.Color.DimGray;
            this.lbdangnhap.Location = new System.Drawing.Point(49, 396);
            this.lbdangnhap.Name = "lbdangnhap";
            this.lbdangnhap.Size = new System.Drawing.Size(207, 16);
            this.lbdangnhap.TabIndex = 1;
            this.lbdangnhap.Text = "Đã có tài khoản? Đăng nhập ngay";
            this.lbdangnhap.Click += new System.EventHandler(this.lbDangNhap_Click);
            // 
            // pt2
            // 
            this.pt2.Image = global::ClubManageApp.Properties.Resources.logo1;
            this.pt2.ImageRotate = 0F;
            this.pt2.Location = new System.Drawing.Point(-42, 425);
            this.pt2.Name = "pt2";
            this.pt2.Size = new System.Drawing.Size(174, 25);
            this.pt2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pt2.TabIndex = 1;
            this.pt2.TabStop = false;
            // 
            // btndangky
            // 
            this.btndangky.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btndangky.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btndangky.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btndangky.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btndangky.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btndangky.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btndangky.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btndangky.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btndangky.ForeColor = System.Drawing.Color.Purple;
            this.btndangky.Location = new System.Drawing.Point(89, 356);
            this.btndangky.Name = "btndangky";
            this.btndangky.Size = new System.Drawing.Size(125, 30);
            this.btndangky.TabIndex = 7;
            this.btndangky.Text = "Đăng ký";
            this.btndangky.Click += new System.EventHandler(this.btnDangKy_Click);
            // 
            // txtmatkhau
            // 
            this.txtmatkhau.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtmatkhau.DefaultText = "";
            this.txtmatkhau.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtmatkhau.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtmatkhau.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtmatkhau.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtmatkhau.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtmatkhau.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtmatkhau.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtmatkhau.Location = new System.Drawing.Point(89, 263);
            this.txtmatkhau.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtmatkhau.Name = "txtmatkhau";
            this.txtmatkhau.PlaceholderText = "Nhập mật khẩu...";
            this.txtmatkhau.SelectedText = "";
            this.txtmatkhau.Size = new System.Drawing.Size(177, 31);
            this.txtmatkhau.TabIndex = 6;
            this.txtmatkhau.UseSystemPasswordChar = true;
            // 
            // txttaikhoan
            // 
            this.txttaikhoan.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txttaikhoan.DefaultText = "";
            this.txttaikhoan.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txttaikhoan.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txttaikhoan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txttaikhoan.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txttaikhoan.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txttaikhoan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txttaikhoan.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txttaikhoan.Location = new System.Drawing.Point(89, 211);
            this.txttaikhoan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txttaikhoan.Name = "txttaikhoan";
            this.txttaikhoan.PlaceholderText = "Nhập tài khoản...";
            this.txttaikhoan.SelectedText = "";
            this.txttaikhoan.Size = new System.Drawing.Size(177, 31);
            this.txttaikhoan.TabIndex = 1;
            // 
            // ptlogo
            // 
            this.ptlogo.Image = global::ClubManageApp.Properties.Resources.Logo;
            this.ptlogo.ImageRotate = 0F;
            this.ptlogo.Location = new System.Drawing.Point(74, 18);
            this.ptlogo.Name = "ptlogo";
            this.ptlogo.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.ptlogo.Size = new System.Drawing.Size(140, 140);
            this.ptlogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptlogo.TabIndex = 1;
            this.ptlogo.TabStop = false;
            // 
            // lbtaikhoan
            // 
            this.lbtaikhoan.AutoSize = true;
            this.lbtaikhoan.ForeColor = System.Drawing.Color.Purple;
            this.lbtaikhoan.Location = new System.Drawing.Point(12, 219);
            this.lbtaikhoan.Name = "lbtaikhoan";
            this.lbtaikhoan.Size = new System.Drawing.Size(67, 16);
            this.lbtaikhoan.TabIndex = 5;
            this.lbtaikhoan.Text = "Tài khoản";
            // 
            // lbmatkhau
            // 
            this.lbmatkhau.AutoSize = true;
            this.lbmatkhau.ForeColor = System.Drawing.Color.Purple;
            this.lbmatkhau.Location = new System.Drawing.Point(13, 271);
            this.lbmatkhau.Name = "lbmatkhau";
            this.lbmatkhau.Size = new System.Drawing.Size(61, 16);
            this.lbmatkhau.TabIndex = 3;
            this.lbmatkhau.Text = "Mật khẩu";
            // 
            // lbTenclb
            // 
            this.lbTenclb.AutoSize = true;
            this.lbTenclb.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTenclb.ForeColor = System.Drawing.Color.Purple;
            this.lbTenclb.Location = new System.Drawing.Point(21, 161);
            this.lbTenclb.Name = "lbTenclb";
            this.lbTenclb.Size = new System.Drawing.Size(256, 31);
            this.lbTenclb.TabIndex = 2;
            this.lbTenclb.Text = "CÂU LẠC BỘ KỸ NĂNG";
            this.lbTenclb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.guna2Panel2.Controls.Add(this.lbdangky);
            this.guna2Panel2.Controls.Add(this.ptLogo2);
            this.guna2Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel2.Location = new System.Drawing.Point(300, 0);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(500, 450);
            this.guna2Panel2.TabIndex = 2;
            // 
            // lbdangky
            // 
            this.lbdangky.AutoSize = true;
            this.lbdangky.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbdangky.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbdangky.ForeColor = System.Drawing.Color.Purple;
            this.lbdangky.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbdangky.Location = new System.Drawing.Point(0, 0);
            this.lbdangky.Name = "lbdangky";
            this.lbdangky.Size = new System.Drawing.Size(304, 38);
            this.lbdangky.TabIndex = 0;
            this.lbdangky.Text = "ĐĂNG KÝ TÀI KHOẢN";
            // 
            // ptLogo2
            // 
            this.ptLogo2.Image = global::ClubManageApp.Properties.Resources._1;
            this.ptLogo2.ImageRotate = 0F;
            this.ptLogo2.Location = new System.Drawing.Point(-389, -119);
            this.ptLogo2.Name = "ptLogo2";
            this.ptLogo2.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.ptLogo2.Size = new System.Drawing.Size(1267, 619);
            this.ptLogo2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptLogo2.TabIndex = 2;
            this.ptLogo2.TabStop = false;
            this.ptLogo2.Click += new System.EventHandler(this.ptLogo2_Click);
            // 
            // SignUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SignUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SignUp";
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pt2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptlogo)).EndInit();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptLogo2)).EndInit();
            this.ResumeLayout(false);

        }

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Label lbdangnhap;
        private Guna.UI2.WinForms.Guna2PictureBox pt2;
        private Guna.UI2.WinForms.Guna2Button btndangky;
        private Guna.UI2.WinForms.Guna2TextBox txtmatkhau;
        private Guna.UI2.WinForms.Guna2TextBox txttaikhoan;
        private Guna.UI2.WinForms.Guna2CirclePictureBox ptlogo;
        private Label lbtaikhoan;
        private Label lbmatkhau;
        private Label lbTenclb;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Label lbdangky;
        private Guna.UI2.WinForms.Guna2CirclePictureBox ptLogo2;
        private Guna.UI2.WinForms.Guna2TextBox txtNhapLai;
        private Label lbnhaplaimatkhau;

        private string connectionString = @"Data Source=21AK22-COM;Initial Catalog=QL_CLB_LSC;Persist Security Info=True;User ID=sa;Password=912005;Encrypt=True;TrustServerCertificate=True";

        // 🔐 Hàm băm SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hash)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        // ✅ Sự kiện nút "Đăng ký"
        // ✅ Sự kiện nút "Đăng ký" - CẬP NHẬT VỚI ANIMATION ĐẸP
        private async void btnDangKy_Click(object sender, EventArgs e)
        {
            string username = txttaikhoan.Text.Trim();
            string password = txtmatkhau.Text.Trim();
            string confirm = txtNhapLai.Text.Trim();

            // Validation
            if (username == "" || password == "" || confirm == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirm)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra trùng tài khoản
                    string checkQuery = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDN = @user";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@user", username);

                    int exists = (int)checkCmd.ExecuteScalar();
                    if (exists > 0)
                    {
                        MessageBox.Show("Tên tài khoản đã tồn tại, vui lòng chọn tên khác!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Nếu có MaTV từ FormSignUp
                    if (maTV.HasValue)
                    {
                        // Kiểm tra MaTV đã có tài khoản chưa
                        string checkMaTVQuery = "SELECT COUNT(*) FROM TaiKhoan WHERE MaTV = @maTV";
                        SqlCommand checkMaTVCmd = new SqlCommand(checkMaTVQuery, conn);
                        checkMaTVCmd.Parameters.AddWithValue("@maTV", maTV.Value);

                        int maTVExists = (int)checkMaTVCmd.ExecuteScalar();
                        if (maTVExists > 0)
                        {
                            MessageBox.Show("Mã thành viên này đã có tài khoản!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Tạo tài khoản với MaTV có sẵn
                        string insertQuery = @"
                    INSERT INTO TaiKhoan (MaTV, TenDN, MatKhau, QuyenHan, TrangThai, NgayTao)
                    VALUES (@maTV, @user, @pass, N'Thành viên', N'Hoạt động', GETDATE())";

                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@maTV", maTV.Value);
                        insertCmd.Parameters.AddWithValue("@user", username);
                        insertCmd.Parameters.AddWithValue("@pass", HashPassword(password));
                        insertCmd.ExecuteNonQuery();

                        // 🎉 HIỂN THỊ ANIMATION THÀNH CÔNG
                        await ShowSuccessAnimation();

                        // Đợi 1 giây sau animation
                        await Task.Delay(1000);

                        // Fade out mượt mà
                        await FadeOut();

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        // Đăng ký không qua FormSignUp (tạo cả ThanhVien và TaiKhoan)
                        MessageBox.Show(
                            "Vui lòng nhập thông tin cá nhân trước!\n\n" +
                            "Bạn cần điền thông tin trong form 'Thông tin chi tiết' trước.",
                            "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng ký: " + ex.Message, "Lỗi hệ thống",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Nút trở lại trang đăng nhập
        private void lbDangNhap_Click(object sender, EventArgs e)
        {
            // Nếu đang trong quy trình tạo tài khoản từ FormSignUp
            if (maTV.HasValue)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn hủy tạo tài khoản? Thông tin cá nhân đã được lưu. Bạn sẽ trở về trang trước để có thể chỉnh sửa hoặc thử lại sau.",
                    "Xác nhận hủy",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Trả về cho FormSignUp biết là đã hủy (FormSignUp sẽ xử lý tiếp)
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
                // nếu chọn No thì vẫn ở lại form tạo tài khoản
            }
            else
            {
                // Trường hợp người dùng mở SignUp trực tiếp từ màn hình đăng nhập
                this.Hide();
                Login loginForm = new Login();
                // Khi Login đóng, đóng luôn form này để tránh cửa sổ ẩn còn tồn tại
                loginForm.FormClosed += (s, args) => this.Close();
                loginForm.Show();
            }
        }

        private void ptLogo2_Click(object sender, EventArgs e)
        {

        }
    }
}