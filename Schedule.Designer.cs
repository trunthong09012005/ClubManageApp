namespace ClubManageApp
{
    partial class ucSchedule
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
            this.panelMeetings = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.btnDeleteMeeting = new System.Windows.Forms.Button();
            this.btnViewMeeting = new System.Windows.Forms.Button();
            this.btnEditMeeting = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.dgvMeetings = new System.Windows.Forms.DataGridView();
            this.panelParticipants = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvParticipants = new System.Windows.Forms.DataGridView();
            this.panelMeetings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeetings)).BeginInit();
            this.panelParticipants.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParticipants)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMeetings
            // 
            this.panelMeetings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMeetings.Controls.Add(this.label3);
            this.panelMeetings.Controls.Add(this.label2);
            this.panelMeetings.Controls.Add(this.monthCalendar1);
            this.panelMeetings.Controls.Add(this.btnDeleteMeeting);
            this.panelMeetings.Controls.Add(this.btnViewMeeting);
            this.panelMeetings.Controls.Add(this.btnEditMeeting);
            this.panelMeetings.Controls.Add(this.txtFilter);
            this.panelMeetings.Controls.Add(this.dgvMeetings);
            this.panelMeetings.Location = new System.Drawing.Point(20, 20);
            this.panelMeetings.Name = "panelMeetings";
            this.panelMeetings.Size = new System.Drawing.Size(1168, 565);
            this.panelMeetings.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(191, 25);
            this.label3.TabIndex = 9;
            this.label3.Text = "Danh sách cuộc họp";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 25);
            this.label2.TabIndex = 8;
            this.label2.Text = "Tìm kiếm";
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(835, 147);
            this.monthCalendar1.MaxSelectionCount = 1;
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 0;
            this.monthCalendar1.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateSelected);
            // 
            // btnDeleteMeeting
            // 
            this.btnDeleteMeeting.BackColor = System.Drawing.Color.Salmon;
            this.btnDeleteMeeting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteMeeting.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteMeeting.Location = new System.Drawing.Point(835, 489);
            this.btnDeleteMeeting.Name = "btnDeleteMeeting";
            this.btnDeleteMeeting.Size = new System.Drawing.Size(298, 50);
            this.btnDeleteMeeting.TabIndex = 4;
            this.btnDeleteMeeting.Text = "Xóa";
            this.btnDeleteMeeting.UseVisualStyleBackColor = false;
            this.btnDeleteMeeting.Click += new System.EventHandler(this.btnDeleteMeeting_Click);
            // 
            // btnViewMeeting
            // 
            this.btnViewMeeting.BackColor = System.Drawing.Color.SandyBrown;
            this.btnViewMeeting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewMeeting.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewMeeting.Location = new System.Drawing.Point(835, 433);
            this.btnViewMeeting.Name = "btnViewMeeting";
            this.btnViewMeeting.Size = new System.Drawing.Size(298, 50);
            this.btnViewMeeting.TabIndex = 5;
            this.btnViewMeeting.Text = "Xem";
            this.btnViewMeeting.UseVisualStyleBackColor = false;
            this.btnViewMeeting.Click += new System.EventHandler(this.btnViewMeeting_Click);
            // 
            // btnEditMeeting
            // 
            this.btnEditMeeting.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnEditMeeting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditMeeting.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditMeeting.Location = new System.Drawing.Point(835, 377);
            this.btnEditMeeting.Name = "btnEditMeeting";
            this.btnEditMeeting.Size = new System.Drawing.Size(298, 50);
            this.btnEditMeeting.TabIndex = 3;
            this.btnEditMeeting.Text = "Chỉnh sửa";
            this.btnEditMeeting.UseVisualStyleBackColor = false;
            this.btnEditMeeting.Click += new System.EventHandler(this.btnEditMeeting_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilter.Location = new System.Drawing.Point(154, 83);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(627, 30);
            this.txtFilter.TabIndex = 6;
            // 
            // dgvMeetings
            // 
            this.dgvMeetings.AllowUserToAddRows = false;
            this.dgvMeetings.AllowUserToDeleteRows = false;
            this.dgvMeetings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMeetings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMeetings.Location = new System.Drawing.Point(23, 147);
            this.dgvMeetings.Name = "dgvMeetings";
            this.dgvMeetings.ReadOnly = true;
            this.dgvMeetings.RowHeadersVisible = false;
            this.dgvMeetings.RowHeadersWidth = 51;
            this.dgvMeetings.RowTemplate.Height = 24;
            this.dgvMeetings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMeetings.Size = new System.Drawing.Size(758, 392);
            this.dgvMeetings.TabIndex = 1;
            this.dgvMeetings.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMeetings_CellContentClick);
            // 
            // panelParticipants
            // 
            this.panelParticipants.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelParticipants.Controls.Add(this.comboBox1);
            this.panelParticipants.Controls.Add(this.button1);
            this.panelParticipants.Controls.Add(this.label1);
            this.panelParticipants.Controls.Add(this.dgvParticipants);
            this.panelParticipants.Location = new System.Drawing.Point(20, 602);
            this.panelParticipants.Name = "panelParticipants";
            this.panelParticipants.Size = new System.Drawing.Size(809, 371);
            this.panelParticipants.TabIndex = 1;
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(273, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(309, 30);
            this.comboBox1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.CornflowerBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(609, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(172, 30);
            this.button1.TabIndex = 2;
            this.button1.Text = "Gửi tất cả";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Danh sách thành viên";
            // 
            // dgvParticipants
            // 
            this.dgvParticipants.AllowUserToAddRows = false;
            this.dgvParticipants.AllowUserToDeleteRows = false;
            this.dgvParticipants.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParticipants.Location = new System.Drawing.Point(23, 65);
            this.dgvParticipants.Name = "dgvParticipants";
            this.dgvParticipants.RowHeadersWidth = 51;
            this.dgvParticipants.RowTemplate.Height = 24;
            this.dgvParticipants.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvParticipants.Size = new System.Drawing.Size(758, 284);
            this.dgvParticipants.TabIndex = 0;
            this.dgvParticipants.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvParticipants_CellClick);
            // 
            // ucSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelParticipants);
            this.Controls.Add(this.panelMeetings);
            this.Name = "ucSchedule";
            this.Size = new System.Drawing.Size(1900, 1100);
            this.panelMeetings.ResumeLayout(false);
            this.panelMeetings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeetings)).EndInit();
            this.panelParticipants.ResumeLayout(false);
            this.panelParticipants.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParticipants)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMeetings;
        private System.Windows.Forms.DataGridView dgvMeetings;
        private System.Windows.Forms.Button btnEditMeeting;
        private System.Windows.Forms.Button btnDeleteMeeting;
        private System.Windows.Forms.Button btnViewMeeting;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Panel panelParticipants;
        private System.Windows.Forms.DataGridView dgvParticipants;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}
