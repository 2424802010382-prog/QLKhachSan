using System;
using System.Collections.Generic;

namespace QLKhachSan.Models
{
    public class BookingDetailViewModel
    {
        public int BookingId { get; set; }

        public int RoomId { get; set; }

        public string RoomName { get; set; }

        public string RoomType { get; set; }

        public string ImageUrl { get; set; }


        // =============================================
        // THÔNG TIN KHÁCH HÀNG
        // =============================================

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerCCCD { get; set; }


        // =============================================
        // THÔNG TIN LƯU TRÚ
        // =============================================

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public int GuestCount { get; set; }

        public int RoomCount { get; set; }

        public string SpecialRequest { get; set; }


        // =============================================
        // BOOKING
        // =============================================

        public string PaymentMethod { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }


        // =============================================
        // KHUYẾN MÃI
        // =============================================

        public string PromotionCode { get; set; }

        public string PromotionName { get; set; }

        public decimal DiscountAmount { get; set; }


        // =============================================
        // TIỀN
        // =============================================

        public decimal RoomAmount { get; set; }

        public decimal ServiceAmount { get; set; }


        // =============================================
        // THANH TOÁN
        // =============================================

        public string PaymentStatus { get; set; }

        public decimal PaymentAmount { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string TransactionCode { get; set; }

        public string PaymentNote { get; set; }


        // =============================================
        // DỊCH VỤ
        // =============================================

        public List<BookingServiceViewModel> Services
        {
            get;
            set;
        }


        public BookingDetailViewModel()
        {
            Services =
                new List<BookingServiceViewModel>();
        }
    }


    // =================================================
    // MỘT DỊCH VỤ TRONG BOOKING
    // =================================================

    public class BookingServiceViewModel
    {
        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public string Unit { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }


    // =================================================
    // DỮ LIỆU BỔ SUNG TRONG BOOKING
    // UserId / MaKhuyenMai / TienGiam có thể chưa có
    // trong Booking.cs do EDMX chưa cập nhật
    // =================================================

    public class BookingExtraData
    {
        public int? MaKhuyenMai { get; set; }

        public decimal TienGiam { get; set; }
    }
}