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
            this.btnFilter = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.btnViewMeeting = new System.Windows.Forms.Button();
            this.btnDeleteMeeting = new System.Windows.Forms.Button();
            this.btnEditMeeting = new System.Windows.Forms.Button();
            this.btnNewMeeting = new System.Windows.Forms.Button();
            this.dgvMeetings = new System.Windows.Forms.DataGridView();
            this.panelParticipants = new System.Windows.Forms.Panel();
            this.dgvParticipants = new System.Windows.Forms.DataGridView();
            this.panelMinutes = new System.Windows.Forms.Panel();
            this.btnViewMinute = new System.Windows.Forms.Button();
            this.dgvMinutes = new System.Windows.Forms.DataGridView();
            this.panelMeetings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeetings)).BeginInit();
            this.panelParticipants.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParticipants)).BeginInit();
            this.panelMinutes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMinutes)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMeetings
            // 
            this.panelMeetings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMeetings.Controls.Add(this.btnFilter);
            this.panelMeetings.Controls.Add(this.txtFilter);
            this.panelMeetings.Controls.Add(this.btnViewMeeting);
            this.panelMeetings.Controls.Add(this.btnDeleteMeeting);
            this.panelMeetings.Controls.Add(this.dgvMeetings);
            this.panelMeetings.Controls.Add(this.btnEditMeeting);
            this.panelMeetings.Controls.Add(this.btnNewMeeting);
            this.panelMeetings.Location = new System.Drawing.Point(68, 24);
            this.panelMeetings.Name = "panelMeetings";
            this.panelMeetings.Size = new System.Drawing.Size(680, 320);
            this.panelMeetings.TabIndex = 0;
            // 
            // btnFilter
            // 
            this.btnFilter.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Location = new System.Drawing.Point(562, 14);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(75, 27);
            this.btnFilter.TabIndex = 7;
            this.btnFilter.Text = "Lọc";
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilter.Location = new System.Drawing.Point(15, 14);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(524, 27);
            this.txtFilter.TabIndex = 6;
            // 
            // btnViewMeeting
            // 
            this.btnViewMeeting.BackColor = System.Drawing.Color.Orange;
            this.btnViewMeeting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewMeeting.Location = new System.Drawing.Point(267, 58);
            this.btnViewMeeting.Name = "btnViewMeeting";
            this.btnViewMeeting.Size = new System.Drawing.Size(120, 26);
            this.btnViewMeeting.TabIndex = 5;
            this.btnViewMeeting.Text = "Xem";
            this.btnViewMeeting.UseVisualStyleBackColor = false;
            this.btnViewMeeting.Click += new System.EventHandler(this.btnViewMeeting_Click);
            // 
            // btnDeleteMeeting
            // 
            this.btnDeleteMeeting.BackColor = System.Drawing.Color.Crimson;
            this.btnDeleteMeeting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteMeeting.Location = new System.Drawing.Point(393, 58);
            this.btnDeleteMeeting.Name = "btnDeleteMeeting";
            this.btnDeleteMeeting.Size = new System.Drawing.Size(120, 26);
            this.btnDeleteMeeting.TabIndex = 4;
            this.btnDeleteMeeting.Text = "Xóa";
            this.btnDeleteMeeting.UseVisualStyleBackColor = false;
            this.btnDeleteMeeting.Click += new System.EventHandler(this.btnDeleteMeeting_Click);
            // 
            // btnEditMeeting
            // 
            this.btnEditMeeting.BackColor = System.Drawing.Color.Gold;
            this.btnEditMeeting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditMeeting.Location = new System.Drawing.Point(141, 58);
            this.btnEditMeeting.Name = "btnEditMeeting";
            this.btnEditMeeting.Size = new System.Drawing.Size(120, 26);
            this.btnEditMeeting.TabIndex = 3;
            this.btnEditMeeting.Text = "Chỉnh sửa";
            this.btnEditMeeting.UseVisualStyleBackColor = false;
            this.btnEditMeeting.Click += new System.EventHandler(this.btnEditMeeting_Click);
            // 
            // btnNewMeeting
            // 
            this.btnNewMeeting.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnNewMeeting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewMeeting.Location = new System.Drawing.Point(15, 58);
            this.btnNewMeeting.Name = "btnNewMeeting";
            this.btnNewMeeting.Size = new System.Drawing.Size(120, 26);
            this.btnNewMeeting.TabIndex = 2;
            this.btnNewMeeting.Text = "Tạo mới";
            this.btnNewMeeting.UseVisualStyleBackColor = false;
            this.btnNewMeeting.Click += new System.EventHandler(this.btnNewMeeting_Click);
            // 
            // dgvMeetings
            // 
            this.dgvMeetings.AllowUserToAddRows = false;
            this.dgvMeetings.AllowUserToDeleteRows = false;
            this.dgvMeetings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMeetings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMeetings.Location = new System.Drawing.Point(15, 90);
            this.dgvMeetings.Name = "dgvMeetings";
            this.dgvMeetings.ReadOnly = true;
            this.dgvMeetings.RowHeadersVisible = false;
            this.dgvMeetings.RowHeadersWidth = 51;
            this.dgvMeetings.RowTemplate.Height = 24;
            this.dgvMeetings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMeetings.Size = new System.Drawing.Size(1123, 210);
            this.dgvMeetings.TabIndex = 1;
            // 
            // panelParticipants
            // 
            this.panelParticipants.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelParticipants.Controls.Add(this.dgvParticipants);
            this.panelParticipants.Location = new System.Drawing.Point(15, 350);
            this.panelParticipants.Name = "panelParticipants";
            this.panelParticipants.Size = new System.Drawing.Size(540, 340);
            this.panelParticipants.TabIndex = 1;
            // 
            // dgvParticipants
            // 
            this.dgvParticipants.AllowUserToAddRows = false;
            this.dgvParticipants.AllowUserToDeleteRows = false;
            this.dgvParticipants.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParticipants.Location = new System.Drawing.Point(12, 12);
            this.dgvParticipants.Name = "dgvParticipants";
            this.dgvParticipants.RowHeadersWidth = 51;
            this.dgvParticipants.RowTemplate.Height = 24;
            this.dgvParticipants.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvParticipants.Size = new System.Drawing.Size(512, 310);
            this.dgvParticipants.TabIndex = 0;
            this.dgvParticipants.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvParticipants_CellClick);
            // 
            // panelMinutes
            // 
            this.panelMinutes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMinutes.Controls.Add(this.btnViewMinute);
            this.panelMinutes.Controls.Add(this.dgvMinutes);
            this.panelMinutes.Location = new System.Drawing.Point(565, 350);
            this.panelMinutes.Name = "panelMinutes";
            this.panelMinutes.Size = new System.Drawing.Size(530, 340);
            this.panelMinutes.TabIndex = 2;
            // 
            // btnViewMinute
            // 
            this.btnViewMinute.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnViewMinute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewMinute.Location = new System.Drawing.Point(392, 300);
            this.btnViewMinute.Name = "btnViewMinute";
            this.btnViewMinute.Size = new System.Drawing.Size(120, 26);
            this.btnViewMinute.TabIndex = 1;
            this.btnViewMinute.Text = "Chi tiết";
            this.btnViewMinute.UseVisualStyleBackColor = false;
            this.btnViewMinute.Click += new System.EventHandler(this.btnViewMinute_Click);
            // 
            // dgvMinutes
            // 
            this.dgvMinutes.AllowUserToAddRows = false;
            this.dgvMinutes.AllowUserToDeleteRows = false;
            this.dgvMinutes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMinutes.Location = new System.Drawing.Point(12, 12);
            this.dgvMinutes.Name = "dgvMinutes";
            this.dgvMinutes.ReadOnly = true;
            this.dgvMinutes.RowHeadersWidth = 51;
            this.dgvMinutes.RowTemplate.Height = 24;
            this.dgvMinutes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMinutes.Size = new System.Drawing.Size(500, 280);
            this.dgvMinutes.TabIndex = 0;
            // 
            // ucSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMinutes);
            this.Controls.Add(this.panelParticipants);
            this.Controls.Add(this.panelMeetings);
            this.Name = "ucSchedule";
            this.Size = new System.Drawing.Size(1092, 712);
            this.panelMeetings.ResumeLayout(false);
            this.panelMeetings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeetings)).EndInit();
            this.panelParticipants.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvParticipants)).EndInit();
            this.panelMinutes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMinutes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMeetings;
        private System.Windows.Forms.DataGridView dgvMeetings;
        private System.Windows.Forms.Button btnNewMeeting;
        private System.Windows.Forms.Button btnEditMeeting;
        private System.Windows.Forms.Button btnDeleteMeeting;
        private System.Windows.Forms.Button btnViewMeeting;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Panel panelParticipants;
        private System.Windows.Forms.DataGridView dgvParticipants;
        private System.Windows.Forms.Panel panelMinutes;
        private System.Windows.Forms.DataGridView dgvMinutes;
        private System.Windows.Forms.Button btnViewMinute;
    }
}
