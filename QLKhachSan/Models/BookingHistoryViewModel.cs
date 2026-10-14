using System;

namespace QLKhachSan.Models
{
    public class BookingHistoryViewModel
    {
        public int BookingId { get; set; }

        public int RoomId { get; set; }

        public string RoomName { get; set; }

        public string RoomType { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public int GuestCount { get; set; }

        public int RoomCount { get; set; }

        public decimal TotalAmount { get; set; }

        public string PaymentMethod { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}