using System;
using System.Collections.Generic;

namespace QLKhachSan.Models
{
    public class TaoDatPhongRequest
    {
        public int RoomId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerCCCD { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public int GuestCount { get; set; }

        public int RoomCount { get; set; }

        public string SpecialRequest { get; set; }

        public string PaymentMethod { get; set; }

        public string PromotionCode { get; set; }

        public List<DichVuDatRequest> Services { get; set; }
    }


    public class DichVuDatRequest
    {
        public int MaDichVu { get; set; }

        public int SoLuong { get; set; }
    }
}