namespace QLKhachSan.Models
{
    public class RestaurantFoodAdminViewModel
    {
        public int MaMonAn { get; set; }

        public int MaDanhMuc { get; set; }

        public string TenDanhMuc { get; set; }

        public string TenMonAn { get; set; }

        public string MoTa { get; set; }

        public decimal Gia { get; set; }

        public string HinhAnh { get; set; }

        public string DonVi { get; set; }

        public int? ThoiGianChuanBi { get; set; }

        public bool NoiBat { get; set; }

        public bool HoatDong { get; set; }
    }
}