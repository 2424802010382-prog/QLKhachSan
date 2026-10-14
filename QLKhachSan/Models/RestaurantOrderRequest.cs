using System.Collections.Generic;

namespace QLKhachSan.Models
{
    public class RestaurantOrderItemRequest
    {
        public int MaMonAn { get; set; }

        public int SoLuong { get; set; }
    }


    public class RestaurantOrderRequest
    {
        public string HoTen { get; set; }

        public string SoDienThoai { get; set; }

        public string HinhThuc { get; set; }

        public string SoBan { get; set; }

        public string SoPhong { get; set; }

        public string GhiChu { get; set; }

        public string PhuongThucThanhToan { get; set; }

        public List<RestaurantOrderItemRequest>
            Items
        { get; set; }
    }
}