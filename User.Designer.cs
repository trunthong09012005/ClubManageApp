namespace ClubManageApp
{
    partial class ucUser
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
            this.panelLeft = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.panelRight = new System.Windows.Forms.Panel();
            this.chartByClass = new System.Windows.Forms.Panel();
            this.chartGender = new System.Windows.Forms.Panel();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.panelRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLeft.Controls.Add(this.txtSearch);
            this.panelLeft.Controls.Add(this.btnFilter);
            this.panelLeft.Controls.Add(this.btnAdd);
            this.panelLeft.Controls.Add(this.btnEdit);
            this.panelLeft.Controls.Add(this.btnDelete);
            this.panelLeft.Controls.Add(this.btnView);
            this.panelLeft.Controls.Add(this.dgvUsers);
            this.panelLeft.Location = new System.Drawing.Point(27, 33);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(1360, 896);
            this.panelLeft.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(12, 12);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(520, 22);
            this.txtSearch.TabIndex = 0;
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(540, 10);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(75, 26);
            this.btnFilter.TabIndex = 1;
            this.btnFilter.Text = "Lọc";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 44);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(90, 28);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Tạo mới";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(110, 44);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(90, 28);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "Chỉnh sửa";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(208, 44);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 28);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(306, 44);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(90, 28);
            this.btnView.TabIndex = 5;
            this.btnView.Text = "Xem";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(12, 80);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.RowHeadersWidth = 51;
            this.dgvUsers.RowTemplate.Height = 24;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(1332, 804);
            this.dgvUsers.TabIndex = 6;
            // 
            // panelRight
            // 
            this.panelRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRight.Controls.Add(this.chartByClass);
            this.panelRight.Controls.Add(this.chartGender);
            this.panelRight.Location = new System.Drawing.Point(1440, 33);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(346, 674);
            this.panelRight.TabIndex = 1;
            // 
            // chartByClass
            // 
            this.chartByClass.BackColor = System.Drawing.Color.WhiteSmoke;
            this.chartByClass.Location = new System.Drawing.Point(12, 340);
            this.chartByClass.Name = "chartByClass";
            this.chartByClass.Size = new System.Drawing.Size(320, 320);
            this.chartByClass.TabIndex = 1;
            // 
            // chartGender
            // 
            this.chartGender.BackColor = System.Drawing.Color.WhiteSmoke;
            this.chartGender.Location = new System.Drawing.Point(12, 12);
            this.chartGender.Name = "chartGender";
            this.chartGender.Size = new System.Drawing.Size(320, 320);
            this.chartGender.TabIndex = 0;
            // 
            // ucUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Name = "ucUser";
            this.Size = new System.Drawing.Size(1900, 1100);
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.panelRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel chartGender;
        private System.Windows.Forms.Panel chartByClass;
    }
}
