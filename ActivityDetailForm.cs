using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ClubManageApp
{
    public partial class ActivityDetailForm : Form
    {
        public ActivityDetailForm(ActivityData activity)
        {
            InitializeComponent();

            if (activity == null) return;

            lblTitle.Text = activity.TenHD;

            var sb = new StringBuilder();
            sb.AppendLine($"Ngày tổ chức: {(activity.NgayToChuc == DateTime.MinValue ? "Chưa xác định" : activity.NgayToChuc.ToString("dd/MM/yyyy"))}");
            sb.AppendLine($"Giờ bắt đầu: {activity.GioBatDau}");
            sb.AppendLine($"Giờ kết thúc: {activity.GioKetThuc}");
            sb.AppendLine($"Địa điểm: {activity.DiaDiem}");
            sb.AppendLine($"Trạng thái: {activity.TrangThai}");
            sb.AppendLine($"Kinh phí dự kiến: {activity.KinhPhiDuKien:N0}");
            sb.AppendLine($"Kinh phí thực tế: {activity.KinhPhiThucTe:N0}");
            sb.AppendLine($"Số lượng tối đa: {activity.SoLuongToiDa}");
            sb.AppendLine();
            sb.AppendLine("Mô tả:");
            sb.AppendLine(activity.MoTa);

            txtDetails.Text = sb.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
