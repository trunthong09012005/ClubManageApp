using System;

namespace ClubManageApp
{
    /// <summary>
    /// Data model for Participant (ThanhVien)
    /// </summary>
    public class ucParticipant
    {
        public int Id { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string Lop { get; set; }
        public string Khoa { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string VaiTro { get; set; }
        public int MaCV { get; set; }
        public int? MaBan { get; set; }

        public ucParticipant()
        {
            // Default values
            NgaySinh = DateTime.Now;
            MaCV = 0;
            MaBan = null;
        }
    }
}
