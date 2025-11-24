using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class ucAccount : UserControl
    {

        private string connectionString = @"Data Source=21AK22-COM;Initial Catalog=QL_CLB_LSC;Persist Security Info=True;User ID=sa;Password=912005;Encrypt=True;TrustServerCertificate=True";
        public ucAccount()
        {
            InitializeComponent();
        }

        private T GetControl<T>(string name) where T : Control
        {
            var arr = this.Controls.Find(name, true);
            if (arr != null && arr.Length > 0) return arr[0] as T;
            return null;
        }

        private void ucAccount_Load(object sender, EventArgs e)
        {
            LoadMembers();
            PopulateTrangThai();
        }

        private void PopulateTrangThai()
        {
            var cboTrangThai = GetControl<ComboBox>("cboTrangThai");
            if (cboTrangThai == null) return;

            cboTrangThai.Items.Clear();
            // Ensure default allowed statuses are present so user can select them
            var defaultStatuses = new[] { "Hoạt động", "Khóa", "Chờ kích hoạt" };
            foreach (var s in defaultStatuses)
            {
                if (!cboTrangThai.Items.Contains(s)) cboTrangThai.Items.Add(s);
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("SELECT DISTINCT TrangThai FROM ThanhVien WHERE TrangThai IS NOT NULL", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var v = reader.IsDBNull(0) ? null : reader.GetString(0);
                            if (!string.IsNullOrEmpty(v) && !cboTrangThai.Items.Contains(v)) cboTrangThai.Items.Add(v);
                        }
                    }
                }
            }
            catch
            {
                // ignore DB read errors, defaults already present
            }

            // keep defaults to avoid CHECK constraint issues on selection; actual DB constraint still must allow these values
        }

        private void LoadMembers(string filter = null)
        {
            try
            {
                var dgv = GetControl<DataGridView>("dgvMembers");
                if (dgv != null) dgv.DataSource = null;

                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;

                    string sql = "SELECT MaTV, HoTen, SDT, DiaChi, VaiTro, TrangThai, Email FROM ThanhVien";

                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        sql += " WHERE HoTen LIKE @f OR Email LIKE @f OR SDT LIKE @f OR DiaChi LIKE @f OR VaiTro LIKE @f OR TrangThai LIKE @f";
                        cmd.Parameters.AddWithValue("@f", "%" + filter + "%");
                    }

                    cmd.CommandText = sql;
                    conn.Open();

                    var da = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    da.Fill(dt);

                    if (dgv != null)
                    {
                        dgv.DataSource = dt;
                        if (dgv.Columns.Contains("SDT")) dgv.Columns["SDT"].HeaderText = "SĐT";
                        if (dgv.Columns.Contains("DiaChi")) dgv.Columns["DiaChi"].HeaderText = "Địa chỉ";
                        if (dgv.Columns.Contains("VaiTro")) dgv.Columns["VaiTro"].HeaderText = "Vai trò";
                        if (dgv.Columns.Contains("TrangThai")) dgv.Columns["TrangThai"].HeaderText = "Trạng thái";
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách thành viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var txtHoTen = GetControl<TextBox>("txtHoTen");
                var txtEmail = GetControl<TextBox>("txtEmail");
                var txtSDT = GetControl<TextBox>("txtSDT");
                var txtDiaChi = GetControl<TextBox>("txtDiaChi");
                var cboVaiTro = GetControl<ComboBox>("cboVaiTro");
                var cboTrangThai = GetControl<ComboBox>("cboTrangThai");

                string hoTen = txtHoTen?.Text.Trim();
                string email = txtEmail?.Text.Trim();
                string sdt = txtSDT?.Text.Trim();
                string diaChi = txtDiaChi?.Text.Trim();
                string vaiTro = cboVaiTro?.SelectedItem?.ToString();
                string trangThaiText = cboTrangThai?.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(hoTen))
                {
                    MessageBox.Show("Tên không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Prevent setting role to Admin (case-insensitive)
                if (!string.IsNullOrEmpty(vaiTro) && string.Equals(vaiTro, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Không được phép đặt vai trò là 'Admin'", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("INSERT INTO ThanhVien(HoTen, SDT, DiaChi, VaiTro, TrangThai, Email) VALUES(@ten, @sdt, @diachi, @vaitro, @trangthai, @email)", conn))
                {
                    cmd.Parameters.AddWithValue("@ten", hoTen);
                    cmd.Parameters.AddWithValue("@sdt", string.IsNullOrEmpty(sdt) ? (object)DBNull.Value : sdt);
                    cmd.Parameters.AddWithValue("@diachi", string.IsNullOrEmpty(diaChi) ? (object)DBNull.Value : diaChi);
                    cmd.Parameters.AddWithValue("@vaitro", string.IsNullOrEmpty(vaiTro) ? (object)DBNull.Value : vaiTro);
                    cmd.Parameters.AddWithValue("@trangthai", string.IsNullOrEmpty(trangThaiText) ? (object)DBNull.Value : trangThaiText);
                    cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);

                    conn.Open();
                    try
                    {
                        int r = cmd.ExecuteNonQuery();
                        if (r > 0) MessageBox.Show("Thêm thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (SqlException sex)
                    {
                        MessageBox.Show("Lỗi khi thêm thành viên: " + sex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // refresh statuses and list
                PopulateTrangThai();
                ClearFields();
                LoadMembers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm thành viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var txtMaTV = GetControl<TextBox>("txtMaTV");
                var txtHoTen = GetControl<TextBox>("txtHoTen");
                var txtEmail = GetControl<TextBox>("txtEmail");
                var txtSDT = GetControl<TextBox>("txtSDT");
                var txtDiaChi = GetControl<TextBox>("txtDiaChi");
                var cboVaiTro = GetControl<ComboBox>("cboVaiTro");
                var cboTrangThai = GetControl<ComboBox>("cboTrangThai");

                if (txtMaTV == null || string.IsNullOrWhiteSpace(txtMaTV.Text))
                {
                    MessageBox.Show("Chọn một thành viên để sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = int.Parse(txtMaTV.Text);
                string hoTen = txtHoTen?.Text.Trim();
                string email = txtEmail?.Text.Trim();
                string sdt = txtSDT?.Text.Trim();
                string diaChi = txtDiaChi?.Text.Trim();
                string vaiTro = cboVaiTro?.SelectedItem?.ToString();
                string trangThaiText = cboTrangThai?.SelectedItem?.ToString();

                // Prevent setting role to Admin (case-insensitive)
                if (!string.IsNullOrEmpty(vaiTro) && string.Equals(vaiTro, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Không được phép đặt vai trò là 'Admin'", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("UPDATE ThanhVien SET HoTen=@ten, SDT=@sdt, DiaChi=@diachi, VaiTro=@vaitro, TrangThai=@trangthai, Email=@email WHERE MaTV=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@ten", hoTen);
                    cmd.Parameters.AddWithValue("@sdt", string.IsNullOrEmpty(sdt) ? (object)DBNull.Value : sdt);
                    cmd.Parameters.AddWithValue("@diachi", string.IsNullOrEmpty(diaChi) ? (object)DBNull.Value : diaChi);
                    cmd.Parameters.AddWithValue("@vaitro", string.IsNullOrEmpty(vaiTro) ? (object)DBNull.Value : vaiTro);
                    cmd.Parameters.AddWithValue("@trangthai", string.IsNullOrEmpty(trangThaiText) ? (object)DBNull.Value : trangThaiText);
                    cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);

                    conn.Open();
                    try
                    {
                        int r = cmd.ExecuteNonQuery();
                        if (r > 0) MessageBox.Show("Cập nhật thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (SqlException sex)
                    {
                        // more user-friendly message for constraint errors
                        MessageBox.Show("Lỗi khi cập nhật: " + sex.Message + "\nKiểm tra giá trị Trạng thái (TrangThai) theo ràng buộc cơ sở dữ liệu.", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // refresh statuses and list
                PopulateTrangThai();
                ClearFields();
                LoadMembers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var txtMaTV = GetControl<TextBox>("txtMaTV");
                if (txtMaTV == null || string.IsNullOrWhiteSpace(txtMaTV.Text))
                {
                    MessageBox.Show("Chọn một thành viên để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = int.Parse(txtMaTV.Text);
                var res = MessageBox.Show("Bạn có chắc muốn xóa thành viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res != DialogResult.Yes) return;

                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("DELETE FROM ThanhVien WHERE MaTV=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    int r = cmd.ExecuteNonQuery();
                    if (r > 0) MessageBox.Show("Xóa thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ClearFields();
                LoadMembers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadMembers();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var txtSearch = GetControl<TextBox>("txtSearch");
            LoadMembers(txtSearch?.Text.Trim());
        }

        private void dgvMembers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dgv = GetControl<DataGridView>("dgvMembers");
            if (dgv == null) return;
            var row = dgv.Rows[e.RowIndex];

            var txtMaTV = GetControl<TextBox>("txtMaTV");
            var txtHoTen = GetControl<TextBox>("txtHoTen");
            var txtEmail = GetControl<TextBox>("txtEmail");
            var txtSDT = GetControl<TextBox>("txtSDT");
            var txtDiaChi = GetControl<TextBox>("txtDiaChi");
            var cboVaiTro = GetControl<ComboBox>("cboVaiTro");
            var cboTrangThai = GetControl<ComboBox>("cboTrangThai");

            txtMaTV.Text = row.Cells["MaTV"].Value?.ToString();
            txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();
            txtEmail.Text = row.Cells["Email"].Value?.ToString();
            txtSDT.Text = row.Cells["SDT"].Value?.ToString();
            txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();

            var role = row.Cells["VaiTro"].Value?.ToString();
            if (cboVaiTro != null)
            {
                if (!string.IsNullOrEmpty(role) && cboVaiTro.Items.Contains(role)) cboVaiTro.SelectedItem = role;
                else cboVaiTro.SelectedIndex = -1;
            }

            var status = row.Cells["TrangThai"].Value?.ToString();
            if (cboTrangThai != null)
            {
                if (!string.IsNullOrEmpty(status))
                {
                    if (!cboTrangThai.Items.Contains(status)) cboTrangThai.Items.Add(status); // add any existing DB value not in list
                    cboTrangThai.SelectedItem = status;
                }
                else cboTrangThai.SelectedIndex = -1;
            }
        }

        private void ClearFields()
        {
            var txtMaTV = GetControl<TextBox>("txtMaTV");
            var txtHoTen = GetControl<TextBox>("txtHoTen");
            var txtEmail = GetControl<TextBox>("txtEmail");
            var txtSearch = GetControl<TextBox>("txtSearch");
            var txtSDT = GetControl<TextBox>("txtSDT");
            var txtDiaChi = GetControl<TextBox>("txtDiaChi");
            var cboVaiTro = GetControl<ComboBox>("cboVaiTro");
            var cboTrangThai = GetControl<ComboBox>("cboTrangThai");

            if (txtMaTV != null) txtMaTV.Text = string.Empty;
            if (txtHoTen != null) txtHoTen.Text = string.Empty;
            if (txtEmail != null) txtEmail.Text = string.Empty;
            if (txtSearch != null) txtSearch.Text = string.Empty;
            if (txtSDT != null) txtSDT.Text = string.Empty;
            if (txtDiaChi != null) txtDiaChi.Text = string.Empty;
            if (cboVaiTro != null) cboVaiTro.SelectedIndex = -1;
            if (cboTrangThai != null) cboTrangThai.SelectedIndex = -1;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvMembers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}


