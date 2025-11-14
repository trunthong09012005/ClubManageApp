namespace ClubManageApp
{
    partial class MemberDashboard
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.slidebarTransition = new System.Windows.Forms.Timer(this.components);
            this.panelStats = new Guna.UI2.WinForms.Guna2Panel();
            this.panelEvent = new Guna.UI2.WinForms.Guna2Panel();
            this.lblEventCount = new System.Windows.Forms.Label();
            this.lblKhenThuong = new System.Windows.Forms.Label();
            this.panelMember = new Guna.UI2.WinForms.Guna2Panel();
            this.lblHoatDong = new System.Windows.Forms.Label();
            this.lblMemberCount = new System.Windows.Forms.Label();
            this.panelPost = new Guna.UI2.WinForms.Guna2Panel();
            this.lblPostCount = new System.Windows.Forms.Label();
            this.lblDiemRL = new System.Windows.Forms.Label();
            this.panelProject = new Guna.UI2.WinForms.Guna2Panel();
            this.lblProjectCount = new System.Windows.Forms.Label();
            this.lblDuAn = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.flowTimeline = new System.Windows.Forms.FlowLayoutPanel();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.slidebar = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.btnMemberDashBoard = new System.Windows.Forms.Button();
            this.btnHoso = new System.Windows.Forms.Button();
            this.btnHoatdong = new System.Windows.Forms.Button();
            this.btnThongbao = new System.Windows.Forms.Button();
            this.btnTinNhan = new System.Windows.Forms.Button();
            this.btnDuan = new System.Windows.Forms.Button();
            this.btnLichhop = new System.Windows.Forms.Button();
            this.btnDangXuat = new System.Windows.Forms.Button();
            this.btnham = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2DataGridView1 = new Guna.UI2.WinForms.Guna2DataGridView();
            this.panelStats.SuspendLayout();
            this.panelEvent.SuspendLayout();
            this.panelMember.SuspendLayout();
            this.panelPost.SuspendLayout();
            this.panelProject.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.flowTimeline.SuspendLayout();
            this.slidebar.SuspendLayout();
            this.panel3.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnham)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2DataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // slidebarTransition
            // 
            this.slidebarTransition.Interval = 30;
            // 
            // panelStats
            // 
            this.panelStats.Controls.Add(this.panelEvent);
            this.panelStats.Controls.Add(this.panelMember);
            this.panelStats.Controls.Add(this.panelPost);
            this.panelStats.Controls.Add(this.panelProject);
            this.panelStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStats.Location = new System.Drawing.Point(206, 44);
            this.panelStats.Name = "panelStats";
            this.panelStats.Size = new System.Drawing.Size(1095, 119);
            this.panelStats.TabIndex = 7;
            // 
            // panelEvent
            // 
            this.panelEvent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(183)))), ((int)(((byte)(177)))));
            this.panelEvent.Controls.Add(this.lblEventCount);
            this.panelEvent.Controls.Add(this.lblKhenThuong);
            this.panelEvent.Location = new System.Drawing.Point(820, 6);
            this.panelEvent.Name = "panelEvent";
            this.panelEvent.Size = new System.Drawing.Size(264, 100);
            this.panelEvent.TabIndex = 6;
            // 
            // lblEventCount
            // 
            this.lblEventCount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEventCount.AutoSize = true;
            this.lblEventCount.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEventCount.Location = new System.Drawing.Point(113, 4);
            this.lblEventCount.Name = "lblEventCount";
            this.lblEventCount.Size = new System.Drawing.Size(39, 45);
            this.lblEventCount.TabIndex = 2;
            this.lblEventCount.Text = "0";
            // 
            // lblKhenThuong
            // 
            this.lblKhenThuong.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKhenThuong.AutoSize = true;
            this.lblKhenThuong.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKhenThuong.Location = new System.Drawing.Point(70, 54);
            this.lblKhenThuong.Name = "lblKhenThuong";
            this.lblKhenThuong.Size = new System.Drawing.Size(136, 28);
            this.lblKhenThuong.TabIndex = 1;
            this.lblKhenThuong.Text = "Khen thưởng";
            // 
            // panelMember
            // 
            this.panelMember.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(234)))), ((int)(((byte)(248)))));
            this.panelMember.Controls.Add(this.lblHoatDong);
            this.panelMember.Controls.Add(this.lblMemberCount);
            this.panelMember.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelMember.Location = new System.Drawing.Point(10, 6);
            this.panelMember.Name = "panelMember";
            this.panelMember.Size = new System.Drawing.Size(264, 100);
            this.panelMember.TabIndex = 3;
            // 
            // lblHoatDong
            // 
            this.lblHoatDong.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHoatDong.AutoSize = true;
            this.lblHoatDong.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoatDong.Location = new System.Drawing.Point(30, 54);
            this.lblHoatDong.Name = "lblHoatDong";
            this.lblHoatDong.Size = new System.Drawing.Size(212, 28);
            this.lblHoatDong.TabIndex = 1;
            this.lblHoatDong.Text = "Hoạt động trong CLB";
            // 
            // lblMemberCount
            // 
            this.lblMemberCount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMemberCount.AutoSize = true;
            this.lblMemberCount.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMemberCount.Location = new System.Drawing.Point(114, 1);
            this.lblMemberCount.Name = "lblMemberCount";
            this.lblMemberCount.Size = new System.Drawing.Size(39, 45);
            this.lblMemberCount.TabIndex = 0;
            this.lblMemberCount.Text = "0";
            // 
            // panelPost
            // 
            this.panelPost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(231)))), ((int)(((byte)(159)))));
            this.panelPost.Controls.Add(this.lblPostCount);
            this.panelPost.Controls.Add(this.lblDiemRL);
            this.panelPost.Location = new System.Drawing.Point(550, 7);
            this.panelPost.Name = "panelPost";
            this.panelPost.Size = new System.Drawing.Size(264, 100);
            this.panelPost.TabIndex = 5;
            // 
            // lblPostCount
            // 
            this.lblPostCount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPostCount.AutoSize = true;
            this.lblPostCount.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPostCount.Location = new System.Drawing.Point(113, 4);
            this.lblPostCount.Name = "lblPostCount";
            this.lblPostCount.Size = new System.Drawing.Size(39, 45);
            this.lblPostCount.TabIndex = 2;
            this.lblPostCount.Text = "0";
            // 
            // lblDiemRL
            // 
            this.lblDiemRL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDiemRL.AutoSize = true;
            this.lblDiemRL.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiemRL.Location = new System.Drawing.Point(60, 54);
            this.lblDiemRL.Name = "lblDiemRL";
            this.lblDiemRL.Size = new System.Drawing.Size(157, 28);
            this.lblDiemRL.TabIndex = 1;
            this.lblDiemRL.Text = "Điểm rèn luyện";
            // 
            // panelProject
            // 
            this.panelProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(239)))), ((int)(((byte)(223)))));
            this.panelProject.Controls.Add(this.lblProjectCount);
            this.panelProject.Controls.Add(this.lblDuAn);
            this.panelProject.Location = new System.Drawing.Point(280, 7);
            this.panelProject.Name = "panelProject";
            this.panelProject.Size = new System.Drawing.Size(264, 100);
            this.panelProject.TabIndex = 4;
            // 
            // lblProjectCount
            // 
            this.lblProjectCount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProjectCount.AutoSize = true;
            this.lblProjectCount.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectCount.Location = new System.Drawing.Point(111, 3);
            this.lblProjectCount.Name = "lblProjectCount";
            this.lblProjectCount.Size = new System.Drawing.Size(39, 45);
            this.lblProjectCount.TabIndex = 2;
            this.lblProjectCount.Text = "0";
            // 
            // lblDuAn
            // 
            this.lblDuAn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDuAn.AutoSize = true;
            this.lblDuAn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDuAn.Location = new System.Drawing.Point(47, 51);
            this.lblDuAn.Name = "lblDuAn";
            this.lblDuAn.Size = new System.Drawing.Size(171, 28);
            this.lblDuAn.TabIndex = 1;
            this.lblDuAn.Text = "Dự án được giao";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnDuan);
            this.panel4.Location = new System.Drawing.Point(3, 308);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(200, 55);
            this.panel4.TabIndex = 5;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnTinNhan);
            this.panel5.Location = new System.Drawing.Point(3, 247);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(200, 55);
            this.panel5.TabIndex = 6;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.btnThongbao);
            this.panel8.Location = new System.Drawing.Point(3, 186);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(200, 55);
            this.panel8.TabIndex = 9;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnHoatdong);
            this.panel7.Location = new System.Drawing.Point(3, 125);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(200, 55);
            this.panel7.TabIndex = 8;
            // 
            // timer1
            // 
            this.timer1.Interval = 30;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnMemberDashBoard);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 55);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnHoso);
            this.panel2.Location = new System.Drawing.Point(3, 64);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 55);
            this.panel2.TabIndex = 3;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnLichhop);
            this.panel6.Location = new System.Drawing.Point(3, 369);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(200, 55);
            this.panel6.TabIndex = 7;
            // 
            // flowTimeline
            // 
            this.flowTimeline.AutoScroll = true;
            this.flowTimeline.BackColor = System.Drawing.Color.White;
            this.flowTimeline.Controls.Add(this.guna2DataGridView1);
            this.flowTimeline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowTimeline.Location = new System.Drawing.Point(206, 44);
            this.flowTimeline.Name = "flowTimeline";
            this.flowTimeline.Size = new System.Drawing.Size(1095, 759);
            this.flowTimeline.TabIndex = 9;
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRole.ForeColor = System.Drawing.Color.Navy;
            this.lblRole.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblRole.Location = new System.Drawing.Point(1048, 23);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(59, 20);
            this.lblRole.TabIndex = 1;
            this.lblRole.Text = "Vai trò:";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.ForeColor = System.Drawing.Color.Navy;
            this.lblUsername.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblUsername.Location = new System.Drawing.Point(1047, 2);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(97, 20);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Người dùng:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(70, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "CLB KỸ NĂNG SỐNG X LSC";
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.Location = new System.Drawing.Point(2, 44);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(204, 57);
            this.guna2Panel2.TabIndex = 2;
            // 
            // slidebar
            // 
            this.slidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(217)))), ((int)(((byte)(255)))));
            this.slidebar.Controls.Add(this.panel1);
            this.slidebar.Controls.Add(this.panel2);
            this.slidebar.Controls.Add(this.panel7);
            this.slidebar.Controls.Add(this.panel8);
            this.slidebar.Controls.Add(this.panel5);
            this.slidebar.Controls.Add(this.panel4);
            this.slidebar.Controls.Add(this.panel6);
            this.slidebar.Controls.Add(this.panel3);
            this.slidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.slidebar.Location = new System.Drawing.Point(0, 44);
            this.slidebar.Name = "slidebar";
            this.slidebar.Size = new System.Drawing.Size(206, 759);
            this.slidebar.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnDangXuat);
            this.panel3.Location = new System.Drawing.Point(3, 430);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 55);
            this.panel3.TabIndex = 5;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.White;
            this.guna2Panel1.Controls.Add(this.lblRole);
            this.guna2Panel1.Controls.Add(this.lblUsername);
            this.guna2Panel1.Controls.Add(this.label1);
            this.guna2Panel1.Controls.Add(this.guna2Panel2);
            this.guna2Panel1.Controls.Add(this.btnham);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1301, 44);
            this.guna2Panel1.TabIndex = 5;
            // 
            // btnMemberDashBoard
            // 
            this.btnMemberDashBoard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(217)))), ((int)(((byte)(255)))));
            this.btnMemberDashBoard.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMemberDashBoard.Image = global::ClubManageApp.Properties.Resources.icons8_dashboard_30;
            this.btnMemberDashBoard.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMemberDashBoard.Location = new System.Drawing.Point(-5, -17);
            this.btnMemberDashBoard.Name = "btnMemberDashBoard";
            this.btnMemberDashBoard.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnMemberDashBoard.Size = new System.Drawing.Size(210, 108);
            this.btnMemberDashBoard.TabIndex = 1;
            this.btnMemberDashBoard.Text = "   Dashboard";
            this.btnMemberDashBoard.UseVisualStyleBackColor = false;
            // 
            // btnHoso
            // 
            this.btnHoso.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(217)))), ((int)(((byte)(255)))));
            this.btnHoso.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHoso.Image = global::ClubManageApp.Properties.Resources.icons8_account_24;
            this.btnHoso.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHoso.Location = new System.Drawing.Point(-5, -22);
            this.btnHoso.Name = "btnHoso";
            this.btnHoso.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnHoso.Size = new System.Drawing.Size(210, 108);
            this.btnHoso.TabIndex = 1;
            this.btnHoso.Text = "Hồ sơ";
            this.btnHoso.UseVisualStyleBackColor = false;
            // 
            // btnHoatdong
            // 
            this.btnHoatdong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(217)))), ((int)(((byte)(255)))));
            this.btnHoatdong.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHoatdong.Image = global::ClubManageApp.Properties.Resources.icons8_play_24;
            this.btnHoatdong.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHoatdong.Location = new System.Drawing.Point(-7, -30);
            this.btnHoatdong.Name = "btnHoatdong";
            this.btnHoatdong.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnHoatdong.Size = new System.Drawing.Size(212, 108);
            this.btnHoatdong.TabIndex = 1;
            this.btnHoatdong.Text = "   Hoạt động";
            this.btnHoatdong.UseVisualStyleBackColor = false;
            // 
            // btnThongbao
            // 
            this.btnThongbao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(217)))), ((int)(((byte)(255)))));
            this.btnThongbao.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThongbao.Image = global::ClubManageApp.Properties.Resources.icons8_announcement_30;
            this.btnThongbao.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnThongbao.Location = new System.Drawing.Point(-4, -29);
            this.btnThongbao.Name = "btnThongbao";
            this.btnThongbao.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnThongbao.Size = new System.Drawing.Size(213, 108);
            this.btnThongbao.TabIndex = 1;
            this.btnThongbao.Text = "Thông báo";
            this.btnThongbao.UseVisualStyleBackColor = false;
            // 
            // btnTinNhan
            // 
            this.btnTinNhan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(217)))), ((int)(((byte)(255)))));
            this.btnTinNhan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTinNhan.Image = global::ClubManageApp.Properties.Resources.icons8_money_box_24;
            this.btnTinNhan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTinNhan.Location = new System.Drawing.Point(-5, -29);
            this.btnTinNhan.Name = "btnTinNhan";
            this.btnTinNhan.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnTinNhan.Size = new System.Drawing.Size(213, 108);
            this.btnTinNhan.TabIndex = 1;
            this.btnTinNhan.Text = "Tin nhắn";
            this.btnTinNhan.UseVisualStyleBackColor = false;
            // 
            // btnDuan
            // 
            this.btnDuan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(217)))), ((int)(((byte)(255)))));
            this.btnDuan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDuan.Image = global::ClubManageApp.Properties.Resources.icons8_project_24;
            this.btnDuan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDuan.Location = new System.Drawing.Point(-4, -31);
            this.btnDuan.Name = "btnDuan";
            this.btnDuan.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnDuan.Size = new System.Drawing.Size(213, 108);
            this.btnDuan.TabIndex = 1;
            this.btnDuan.Text = "Dự án";
            this.btnDuan.UseVisualStyleBackColor = false;
            // 
            // btnLichhop
            // 
            this.btnLichhop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(217)))), ((int)(((byte)(255)))));
            this.btnLichhop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLichhop.Image = global::ClubManageApp.Properties.Resources.icons8_schedule_30;
            this.btnLichhop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLichhop.Location = new System.Drawing.Point(-7, -37);
            this.btnLichhop.Name = "btnLichhop";
            this.btnLichhop.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnLichhop.Size = new System.Drawing.Size(213, 108);
            this.btnLichhop.TabIndex = 1;
            this.btnLichhop.Text = "Lịch họp";
            this.btnLichhop.UseVisualStyleBackColor = false;
            // 
            // btnDangXuat
            // 
            this.btnDangXuat.BackColor = System.Drawing.Color.Red;
            this.btnDangXuat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDangXuat.Image = global::ClubManageApp.Properties.Resources.icons8_log_out_30;
            this.btnDangXuat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDangXuat.Location = new System.Drawing.Point(-6, -28);
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnDangXuat.Size = new System.Drawing.Size(213, 108);
            this.btnDangXuat.TabIndex = 2;
            this.btnDangXuat.Text = "Đăng xuất";
            this.btnDangXuat.UseVisualStyleBackColor = false;
            // 
            // btnham
            // 
            this.btnham.Image = global::ClubManageApp.Properties.Resources.menu;
            this.btnham.ImageRotate = 0F;
            this.btnham.Location = new System.Drawing.Point(0, 3);
            this.btnham.Name = "btnham";
            this.btnham.Size = new System.Drawing.Size(66, 37);
            this.btnham.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnham.TabIndex = 1;
            this.btnham.TabStop = false;
            // 
            // guna2DataGridView1
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.guna2DataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.guna2DataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.guna2DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.guna2DataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.guna2DataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.guna2DataGridView1.Location = new System.Drawing.Point(3, 3);
            this.guna2DataGridView1.Name = "guna2DataGridView1";
            this.guna2DataGridView1.RowHeadersVisible = false;
            this.guna2DataGridView1.RowHeadersWidth = 51;
            this.guna2DataGridView1.RowTemplate.Height = 24;
            this.guna2DataGridView1.Size = new System.Drawing.Size(240, 150);
            this.guna2DataGridView1.TabIndex = 0;
            this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.guna2DataGridView1.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.guna2DataGridView1.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.guna2DataGridView1.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.guna2DataGridView1.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.guna2DataGridView1.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2DataGridView1.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.guna2DataGridView1.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.guna2DataGridView1.ThemeStyle.HeaderStyle.Height = 4;
            this.guna2DataGridView1.ThemeStyle.ReadOnly = false;
            this.guna2DataGridView1.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.guna2DataGridView1.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.guna2DataGridView1.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2DataGridView1.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.guna2DataGridView1.ThemeStyle.RowsStyle.Height = 24;
            this.guna2DataGridView1.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.guna2DataGridView1.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // MemberDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 803);
            this.Controls.Add(this.panelStats);
            this.Controls.Add(this.flowTimeline);
            this.Controls.Add(this.slidebar);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MemberDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MemberDashboard";
            this.panelStats.ResumeLayout(false);
            this.panelEvent.ResumeLayout(false);
            this.panelEvent.PerformLayout();
            this.panelMember.ResumeLayout(false);
            this.panelMember.PerformLayout();
            this.panelPost.ResumeLayout(false);
            this.panelPost.PerformLayout();
            this.panelProject.ResumeLayout(false);
            this.panelProject.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.flowTimeline.ResumeLayout(false);
            this.slidebar.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnham)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2DataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer slidebarTransition;
        private System.Windows.Forms.Button btnDangXuat;
        private Guna.UI2.WinForms.Guna2Panel panelStats;
        private Guna.UI2.WinForms.Guna2Panel panelEvent;
        private System.Windows.Forms.Label lblEventCount;
        private System.Windows.Forms.Label lblKhenThuong;
        private Guna.UI2.WinForms.Guna2Panel panelMember;
        private System.Windows.Forms.Label lblHoatDong;
        private System.Windows.Forms.Label lblMemberCount;
        private Guna.UI2.WinForms.Guna2Panel panelPost;
        private System.Windows.Forms.Label lblPostCount;
        private System.Windows.Forms.Label lblDiemRL;
        private Guna.UI2.WinForms.Guna2Panel panelProject;
        private System.Windows.Forms.Label lblProjectCount;
        private System.Windows.Forms.Label lblDuAn;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnDuan;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnTinNhan;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button btnThongbao;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnHoatdong;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnMemberDashBoard;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnHoso;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnLichhop;
        private System.Windows.Forms.FlowLayoutPanel flowTimeline;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2PictureBox btnham;
        private System.Windows.Forms.FlowLayoutPanel slidebar;
        private System.Windows.Forms.Panel panel3;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2DataGridView guna2DataGridView1;
    }
}