using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ClubManageApp
{
    /// <summary>
    /// 🎨 Các extension methods để tăng tính thẩm mỹ cho Dashboard
    /// </summary>
    public static class DashboardEnhancements
    {
        // ================================
        // 🎨 ROUNDED CORNERS
        // ================================
        public static void MakeRounded(this Panel panel, int radius = 20)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(panel.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(panel.Width - radius, panel.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, panel.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            panel.Region = new Region(path);
        }

        // ================================
        // 💫 SHADOW EFFECT
        // ================================
        public static void AddShadow(this Panel panel)
        {
            panel.Paint += (s, e) =>
            {
                using (GraphicsPath gp = new GraphicsPath())
                {
                    gp.AddRectangle(new Rectangle(3, 3, panel.Width - 6, panel.Height - 6));
                    using (PathGradientBrush pgb = new PathGradientBrush(gp))
                    {
                        pgb.CenterColor = Color.Transparent;
                        pgb.SurroundColors = new[] { Color.FromArgb(30, 0, 0, 0) };
                        e.Graphics.FillPath(pgb, gp);
                    }
                }
            };
        }

        // ================================
        // 🌈 GRADIENT BACKGROUND
        // ================================
        public static void AddGradient(this Panel panel, Color color1, Color color2)
        {
            panel.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    panel.ClientRectangle,
                    color1,
                    color2,
                    LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, panel.ClientRectangle);
                }
            };
        }

        // ================================
        // ✨ HOVER EFFECT
        // ================================
        public static void AddHoverEffect(this Button button, Color hoverColor)
        {
            Color originalColor = button.BackColor;

            button.MouseEnter += (s, e) =>
            {
                button.BackColor = hoverColor;
                button.Cursor = Cursors.Hand;
            };

            button.MouseLeave += (s, e) =>
            {
                button.BackColor = originalColor;
            };
        }

        public static void AddHoverEffect(this Panel panel)
        {
            panel.MouseEnter += (s, e) =>
            {
                panel.BackColor = ControlPaint.Light(panel.BackColor, 0.1f);
                panel.Cursor = Cursors.Hand;
            };

            panel.MouseLeave += (s, e) =>
            {
                panel.BackColor = ControlPaint.Light(panel.BackColor, -0.1f);
            };
        }

        // ================================
        // 📊 ANIMATED COUNTER
        // ================================
        public static void AnimateCounter(this Label label, int targetValue, int duration = 1000)
        {
            int startValue = 0;
            int.TryParse(label.Text, out startValue);

            Timer timer = new Timer();
            timer.Interval = 20;

            DateTime startTime = DateTime.Now;

            timer.Tick += (s, e) =>
            {
                double elapsed = (DateTime.Now - startTime).TotalMilliseconds;
                double progress = Math.Min(elapsed / duration, 1.0);

                // Easing function (ease-out)
                progress = 1 - Math.Pow(1 - progress, 3);

                int currentValue = (int)(startValue + (targetValue - startValue) * progress);
                label.Text = currentValue.ToString();

                if (progress >= 1.0)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            };

            timer.Start();
        }

        // ================================
        // 🎭 FADE IN ANIMATION
        // ================================
        public static void FadeIn(this Control control, int duration = 500)
        {
            control.Visible = true;
            Timer timer = new Timer();
            timer.Interval = 20;

            double opacity = 0;
            DateTime startTime = DateTime.Now;

            timer.Tick += (s, e) =>
            {
                double elapsed = (DateTime.Now - startTime).TotalMilliseconds;
                opacity = Math.Min(elapsed / duration, 1.0);

                // Simulate opacity by adjusting control color
                if (opacity >= 1.0)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            };

            timer.Start();
        }

        // ================================
        // 🎪 PULSE ANIMATION (for notifications)
        // ================================
        public static void Pulse(this Control control, int count = 3)
        {
            Size originalSize = control.Size;
            Point originalLocation = control.Location;

            Timer timer = new Timer();
            timer.Interval = 100;
            int pulseCount = 0;
            bool expanding = true;

            timer.Tick += (s, e) =>
            {
                if (expanding)
                {
                    control.Width += 5;
                    control.Height += 5;
                    control.Left -= 2;
                    control.Top -= 2;
                    expanding = false;
                }
                else
                {
                    control.Size = originalSize;
                    control.Location = originalLocation;
                    expanding = true;
                    pulseCount++;

                    if (pulseCount >= count)
                    {
                        timer.Stop();
                        timer.Dispose();
                    }
                }
            };

            timer.Start();
        }

        // ================================
        // 🔔 NOTIFICATION BADGE
        // ================================
        public static void AddNotificationBadge(this Button button, int count)
        {
            if (count <= 0) return;

            button.Paint += (s, e) =>
            {
                if (count > 0)
                {
                    // Draw badge circle
                    Rectangle badgeRect = new Rectangle(button.Width - 30, 10, 20, 20);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(231, 76, 60)), badgeRect);

                    // Draw count text
                    string countText = count > 99 ? "99+" : count.ToString();
                    Font badgeFont = new Font("Segoe UI", 8, FontStyle.Bold);
                    SizeF textSize = e.Graphics.MeasureString(countText, badgeFont);
                    PointF textPos = new PointF(
                        badgeRect.X + (badgeRect.Width - textSize.Width) / 2,
                        badgeRect.Y + (badgeRect.Height - textSize.Height) / 2
                    );
                    e.Graphics.DrawString(countText, badgeFont, Brushes.White, textPos);
                }
            };

            button.Invalidate();
        }

        // ================================
        // 🎨 CIRCULAR AVATAR
        // ================================
        public static void MakeCircular(this PictureBox pictureBox)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, pictureBox.Width, pictureBox.Height);
            pictureBox.Region = new Region(path);

            // Add border
            pictureBox.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.White, 3))
                {
                    e.Graphics.DrawEllipse(pen, 1, 1, pictureBox.Width - 3, pictureBox.Height - 3);
                }
            };
        }

        // ================================
        // 📈 PROGRESS BAR ANIMATION
        // ================================
        public static Panel CreateProgressBar(int percentage, Color color, int width = 200)
        {
            Panel container = new Panel
            {
                Width = width,
                Height = 8,
                BackColor = Color.FromArgb(220, 220, 220)
            };

            Panel progress = new Panel
            {
                Width = 0,
                Height = 8,
                BackColor = color,
                Dock = DockStyle.Left
            };

            container.Controls.Add(progress);
            container.MakeRounded(4);
            progress.MakeRounded(4);

            // Animate progress
            Timer timer = new Timer { Interval = 20 };
            int targetWidth = width * percentage / 100;

            timer.Tick += (s, e) =>
            {
                if (progress.Width < targetWidth)
                {
                    progress.Width += 5;
                }
                else
                {
                    timer.Stop();
                    timer.Dispose();
                }
            };

            timer.Start();

            return container;
        }

        // ================================
        // 🎯 TOOLTIP WITH STYLE
        // ================================
        public static void AddStyledTooltip(this Control control, string text)
        {
            ToolTip tooltip = new ToolTip
            {
                BackColor = Color.FromArgb(44, 62, 80),
                ForeColor = Color.White,
                IsBalloon = true,
                ToolTipIcon = ToolTipIcon.Info,
                InitialDelay = 500,
                ReshowDelay = 100
            };

            tooltip.SetToolTip(control, text);
        }
    }

    // ================================
    // 🎨 CUSTOM COLOR PALETTE
    // ================================
    public static class ColorPalette
    {
        // Material Design Colors
        public static readonly Color Primary = Color.FromArgb(52, 152, 219);      // Blue
        public static readonly Color Success = Color.FromArgb(46, 204, 113);      // Green
        public static readonly Color Warning = Color.FromArgb(241, 196, 15);      // Yellow
        public static readonly Color Danger = Color.FromArgb(231, 76, 60);        // Red
        public static readonly Color Info = Color.FromArgb(52, 73, 94);           // Dark Blue
        public static readonly Color Purple = Color.FromArgb(155, 89, 182);       // Purple
        public static readonly Color Orange = Color.FromArgb(230, 126, 34);       // Orange
        public static readonly Color Teal = Color.FromArgb(26, 188, 156);         // Teal

        // Background Colors
        public static readonly Color Background = Color.FromArgb(236, 240, 241);  // Light Gray
        public static readonly Color CardBackground = Color.White;
        public static readonly Color DarkBackground = Color.FromArgb(44, 62, 80);

        // Text Colors
        public static readonly Color TextPrimary = Color.FromArgb(44, 62, 80);
        public static readonly Color TextSecondary = Color.FromArgb(127, 140, 141);
        public static readonly Color TextLight = Color.White;
    }
}