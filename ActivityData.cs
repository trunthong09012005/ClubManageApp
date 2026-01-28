using System;

namespace ClubManageApp
{
    // Shared model for activities used by forms and tests
    public class ActivityData
    {
        public int MaHD { get; set; }
        public string TenHD { get; set; }
        public DateTime NgayToChuc { get; set; }
        public string GioBatDau { get; set; }
        public string GioKetThuc { get; set; }
        public string DiaDiem { get; set; }
        public string MoTa { get; set; }
        public decimal KinhPhiDuKien { get; set; }
        public decimal KinhPhiThucTe { get; set; }
        public int SoLuongToiDa { get; set; }
        public string TrangThai { get; set; }
        public int MaLoaiHD { get; set; }
        public int NguoiPhuTrach { get; set; }
    }
}
