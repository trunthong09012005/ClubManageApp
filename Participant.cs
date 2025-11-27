using System;

namespace ClubManageApp
{
    // Move Participant model to its own file so other classes can reference it
    public class Participant
    {
        public int Id { get; set; }

        // Columns from ThanhVien table
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

        // Backwards-compatible alias
        public string Name { get => HoTen; set => HoTen = value; }
    }
}
