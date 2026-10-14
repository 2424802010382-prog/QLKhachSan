using System.Collections.Generic;

namespace QLKhachSan.Models
{
    // =========================================================
    // DANH MỤC MÓN ĂN
    // =========================================================

    public class RestaurantCategoryViewModel
    {
        public int MaDanhMuc { get; set; }

        public string TenDanhMuc { get; set; }

        public string MoTa { get; set; }

        public int ThuTu { get; set; }
    }


    // =========================================================
    // MÓN ĂN
    // =========================================================

    public class RestaurantFoodViewModel
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
    }


    // =========================================================
    // MODEL CHÍNH CHO TRANG NHÀ HÀNG
    // =========================================================

    public class RestaurantIndexViewModel
    {
        public RestaurantIndexViewModel()
        {
            Categories =
                new List<RestaurantCategoryViewModel>();

            Foods =
                new List<RestaurantFoodViewModel>();
        }


        public List<RestaurantCategoryViewModel>
            Categories
        { get; set; }


        public List<RestaurantFoodViewModel>
            Foods
        { get; set; }
    }
}