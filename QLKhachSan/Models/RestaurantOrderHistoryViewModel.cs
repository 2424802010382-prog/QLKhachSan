using System;
using System.Collections.Generic;

namespace QLKhachSan.Models
{
    public class RestaurantOrderHistoryItemViewModel
    {
        public string TenMonAn { get; set; }

        public int SoLuong { get; set; }

        public decimal DonGia { get; set; }

        public decimal ThanhTien { get; set; }
    }


    public class RestaurantOrderHistoryViewModel
    {
        public RestaurantOrderHistoryViewModel()
        {
            Items =
                new List<RestaurantOrderHistoryItemViewModel>();
        }


        public int MaDon { get; set; }

        public string HoTen { get; set; }

        public string SoDienThoai { get; set; }

        public string HinhThuc { get; set; }

        public string SoBan { get; set; }

        public string SoPhong { get; set; }

        public string GhiChu { get; set; }

        public decimal TongTien { get; set; }

        public string TrangThai { get; set; }

        public string PhuongThucThanhToan { get; set; }

        public string TrangThaiThanhToan { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<RestaurantOrderHistoryItemViewModel>
            Items
        { get; set; }
    }
}