using QLKhachSan.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace QLKhachSan.Controllers
{
    public class BookingController : Controller
    {
        private readonly HotelDbContext db =
            new HotelDbContext();


        // =========================================================
        // LẤY DANH SÁCH DỊCH VỤ
        // =========================================================

        [HttpGet]
        public JsonResult GetServices()
        {
            try
            {
                var services = db.DichVus
                    .Where(d => d.HoatDong)
                    .OrderBy(d => d.TenDichVu)
                    .Select(d => new
                    {
                        id = d.MaDichVu,
                        name = d.TenDichVu,
                        description = d.MoTa,
                        price = d.Gia,
                        unit = d.DonVi
                    })
                    .ToList();


                return Json(
                    services,
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false,
                        message =
                            "Không tải được dịch vụ: "
                            + ex.Message
                    },
                    JsonRequestBehavior.AllowGet
                );
            }
        }


        // =========================================================
        // KIỂM TRA MÃ KHUYẾN MÃI
        // =========================================================

        [HttpGet]
        public JsonResult CheckPromotion(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return Json(
                    new
                    {
                        success = false,
                        message =
                            "Vui lòng nhập mã khuyến mãi."
                    },
                    JsonRequestBehavior.AllowGet
                );
            }


            string ma =
                code.Trim().ToUpper();


            DateTime today =
                DateTime.Today;


            var promotion =
                db.KhuyenMais.FirstOrDefault(k =>
                    k.MaCode == ma &&
                    k.HoatDong &&
                    k.NgayBatDau <= today &&
                    k.NgayKetThuc >= today
                );


            if (promotion == null)
            {
                return Json(
                    new
                    {
                        success = false,
                        message =
                            "Mã khuyến mãi không tồn tại hoặc đã hết hạn."
                    },
                    JsonRequestBehavior.AllowGet
                );
            }


            if (
                promotion.SoLuotSuDungToiDa.HasValue &&
                promotion.SoLuotDaSuDung >=
                promotion.SoLuotSuDungToiDa.Value
            )
            {
                return Json(
                    new
                    {
                        success = false,
                        message =
                            "Mã khuyến mãi đã hết lượt sử dụng."
                    },
                    JsonRequestBehavior.AllowGet
                );
            }


            // Quan trọng:
            // các tên field bên dưới phải khớp Index.cshtml

            return Json(
                new
                {
                    success = true,

                    id =
                        promotion.MaKhuyenMai,

                    code =
                        promotion.MaCode,

                    name =
                        promotion.TenKhuyenMai,

                    type =
                        promotion.LoaiGiamGia,

                    value =
                        promotion.GiaTriGiam,

                    minAmount =
                        promotion.DonToiThieu,

                    maxDiscount =
                        promotion.GiamToiDa,

                    message =
                        "Áp dụng mã thành công: "
                        + promotion.TenKhuyenMai
                },
                JsonRequestBehavior.AllowGet
            );
        }


        // =========================================================
        // TẠO ĐẶT PHÒNG
        // =========================================================

        [HttpPost]
        public JsonResult Create(
            TaoDatPhongRequest request)
        {
            using (
                var transaction =
                    db.Database.BeginTransaction()
            )
            {
                try
                {
                    // =============================================
                    // KIỂM TRA REQUEST
                    // =============================================

                    if (request == null)
                    {
                        return Json(new
                        {
                            success = false,
                            message =
                                "Dữ liệu đặt phòng không hợp lệ."
                        });
                    }


                    // =============================================
                    // TÌM PHÒNG
                    // =============================================

                    var room =
                        db.Rooms.Find(
                            request.RoomId
                        );


                    if (
                        room == null ||
                        !room.IsActive
                    )
                    {
                        return Json(new
                        {
                            success = false,
                            message =
                                "Phòng không tồn tại hoặc đã ngừng hoạt động."
                        });
                    }


                    // =============================================
                    // KIỂM TRA THÔNG TIN KHÁCH
                    // =============================================

                    if (
                        string.IsNullOrWhiteSpace(
                            request.CustomerName
                        ) ||

                        string.IsNullOrWhiteSpace(
                            request.CustomerPhone
                        ) ||

                        string.IsNullOrWhiteSpace(
                            request.CustomerEmail
                        )
                    )
                    {
                        return Json(new
                        {
                            success = false,
                            message =
                                "Vui lòng nhập đầy đủ họ tên, số điện thoại và email."
                        });
                    }
                    // =============================================
                    // KIỂM TRA ĐỊNH DẠNG SỐ ĐIỆN THOẠI
                    // =============================================

                    string customerPhone =
                        request.CustomerPhone.Trim();


                    if (
                        !System.Text.RegularExpressions.Regex.IsMatch(
                            customerPhone,
                            @"^\d{10,11}$"
                        )
                    )
                    {
                        return Json(new
                        {
                            success = false,

                            message =
                                "Số điện thoại phải gồm từ 10 đến 11 chữ số."
                        });
                    }
                    // =============================================
                    // KIỂM TRA ĐỊNH DẠNG EMAIL
                    // =============================================

                    string customerEmail =
                        request.CustomerEmail.Trim();


                    if (
                        !System.Text.RegularExpressions.Regex.IsMatch(
                            customerEmail,
                            @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
                        )
                    )
                    {
                        return Json(new
                        {
                            success = false,

                            message =
                                "Email không đúng định dạng."
                        });
                    }
                    // =============================================
                    // KIỂM TRA CCCD
                    // CCCD KHÔNG BẮT BUỘC
                    // NHƯNG NẾU NHẬP THÌ PHẢI ĐỦ 12 SỐ
                    // =============================================

                    if (
                        !string.IsNullOrWhiteSpace(
                            request.CustomerCCCD
                        )
                    )
                    {
                        string customerCCCD =
                            request.CustomerCCCD.Trim();


                        if (
                            !System.Text.RegularExpressions.Regex.IsMatch(
                                customerCCCD,
                                @"^\d{12}$"
                            )
                        )
                        {
                            return Json(new
                            {
                                success = false,

                                message =
                                    "CCCD phải gồm đúng 12 chữ số."
                            });
                        }
                    }

                    // =============================================
                    // KIỂM TRA PHƯƠNG THỨC THANH TOÁN
                    // =============================================

                    string paymentMethod =
                        string.IsNullOrWhiteSpace(
                            request.PaymentMethod
                        )
                            ? ""
                            : request.PaymentMethod.Trim();


                    if (
                        paymentMethod !=
                            "Thanh toán tại khách sạn" &&

                        paymentMethod !=
                            "Chuyển khoản"
                    )
                    {
                        return Json(new
                        {
                            success = false,

                            message =
                                "Phương thức thanh toán không hợp lệ."
                        });
                    }
                    // =============================================
                    // KIỂM TRA NGÀY
                    // =============================================

                    if (
                        request.CheckInDate.Date <
                        DateTime.Today
                    )
                    {
                        return Json(new
                        {
                            success = false,
                            message =
                                "Ngày nhận phòng không được nhỏ hơn hôm nay."
                        });
                    }


                    if (
                        request.CheckOutDate.Date <=
                        request.CheckInDate.Date
                    )
                    {
                        return Json(new
                        {
                            success = false,
                            message =
                                "Ngày trả phòng phải sau ngày nhận phòng."
                        });
                    }


                    // =============================================
                    // KIỂM TRA SỐ PHÒNG
                    // =============================================

                    if (request.RoomCount < 1)
                    {
                        return Json(new
                        {
                            success = false,
                            message =
                                "Số phòng không hợp lệ."
                        });
                    }


                    int bookedRooms =
                        db.Bookings

                        .Where(b =>

                            b.RoomId ==
                            request.RoomId &&

                            b.Status !=
                            "Đã hủy" &&

                            request.CheckInDate <
                            b.CheckOutDate &&

                            request.CheckOutDate >
                            b.CheckInDate

                        )

                        .Select(
                            b => (int?)b.RoomCount
                        )

                        .Sum() ?? 0;


                    int availableRooms =
                        room.TotalRooms -
                        bookedRooms;


                    if (availableRooms < 0)
                    {
                        availableRooms = 0;
                    }


                    if (
                        request.RoomCount >
                        availableRooms
                    )
                    {
                        return Json(new
                        {
                            success = false,

                            message =
                                "Không đủ phòng trống. Hiện chỉ còn "
                                + availableRooms
                                + " phòng."
                        });
                    }


                    // =============================================
                    // KIỂM TRA SỐ KHÁCH
                    // =============================================

                    if (
                        request.GuestCount < 1 ||

                        request.GuestCount >
                        room.Capacity *
                        request.RoomCount
                    )
                    {
                        return Json(new
                        {
                            success = false,

                            message =
                                "Số khách vượt quá sức chứa của số phòng đã chọn."
                        });
                    }


                    // =============================================
                    // TÍNH TIỀN PHÒNG
                    // =============================================

                    int nights =
                        (
                            request.CheckOutDate.Date -
                            request.CheckInDate.Date
                        ).Days;


                    decimal roomAmount =
                        room.Price *
                        nights *
                        request.RoomCount;


                    // =============================================
                    // XỬ LÝ DỊCH VỤ
                    // =============================================

                    var serviceRequests =
                        (
                            request.Services ??
                            new List<DichVuDatRequest>()
                        )

                        .Where(s =>
                            s != null &&
                            s.MaDichVu > 0 &&
                            s.SoLuong > 0
                        )

                        .GroupBy(
                            s => s.MaDichVu
                        )

                        .Select(g =>
                            new DichVuDatRequest
                            {
                                MaDichVu =
                                    g.Key,

                                SoLuong =
                                    g.Sum(
                                        x => x.SoLuong
                                    )
                            }
                        )

                        .ToList();


                    // Không cho nhập số lượng quá lớn
                    if (
                        serviceRequests.Any(
                            s => s.SoLuong > 20
                        )
                    )
                    {
                        return Json(new
                        {
                            success = false,

                            message =
                                "Số lượng dịch vụ không hợp lệ."
                        });
                    }


                    var serviceIds =
                        serviceRequests

                        .Select(
                            s => s.MaDichVu
                        )

                        .ToList();


                    var services =
                        serviceIds.Count == 0

                        ? new List<DichVu>()

                        : db.DichVus

                            .Where(d =>
                                serviceIds.Contains(
                                    d.MaDichVu
                                ) &&
                                d.HoatDong
                            )

                            .ToList();


                    if (
                        services.Count !=
                        serviceIds
                            .Distinct()
                            .Count()
                    )
                    {
                        return Json(new
                        {
                            success = false,

                            message =
                                "Có dịch vụ không tồn tại hoặc đã ngừng hoạt động."
                        });
                    }


                    decimal serviceAmount = 0;


                    foreach (
                        var serviceRequest
                        in serviceRequests
                    )
                    {
                        var service =
                            services.First(
                                d =>
                                d.MaDichVu ==
                                serviceRequest.MaDichVu
                            );


                        serviceAmount +=
                            service.Gia *
                            serviceRequest.SoLuong;
                    }


                    // =============================================
                    // XỬ LÝ KHUYẾN MÃI
                    // =============================================

                    KhuyenMai promotion = null;

                    decimal discountAmount = 0;


                    if (
                        !string.IsNullOrWhiteSpace(
                            request.PromotionCode
                        )
                    )
                    {
                        string promotionCode =
                            request
                                .PromotionCode
                                .Trim()
                                .ToUpper();


                        DateTime today =
                            DateTime.Today;


                        promotion =
                            db.KhuyenMais
                            .FirstOrDefault(k =>

                                k.MaCode ==
                                promotionCode &&

                                k.HoatDong &&

                                k.NgayBatDau <=
                                today &&

                                k.NgayKetThuc >=
                                today
                            );


                        if (promotion == null)
                        {
                            return Json(new
                            {
                                success = false,

                                message =
                                    "Mã khuyến mãi không tồn tại hoặc đã hết hạn."
                            });
                        }


                        // Khuyến mãi đang tính trên TIỀN PHÒNG
                        // để khớp với Index.cshtml

                        if (
                            roomAmount <
                            promotion.DonToiThieu
                        )
                        {
                            return Json(new
                            {
                                success = false,

                                message =
                                    "Tiền phòng chưa đạt mức tối thiểu "
                                    +
                                    promotion
                                        .DonToiThieu
                                        .ToString("N0")
                                    +
                                    "đ để sử dụng mã này."
                            });
                        }


                        if (
                            promotion
                                .SoLuotSuDungToiDa
                                .HasValue &&

                            promotion
                                .SoLuotDaSuDung >=

                            promotion
                                .SoLuotSuDungToiDa
                                .Value
                        )
                        {
                            return Json(new
                            {
                                success = false,

                                message =
                                    "Mã khuyến mãi đã hết lượt sử dụng."
                            });
                        }


                        if (
                            promotion.LoaiGiamGia ==
                            "PhanTram"
                        )
                        {
                            discountAmount =
                                roomAmount *
                                promotion.GiaTriGiam /
                                100m;


                            if (
                                promotion
                                    .GiamToiDa
                                    .HasValue
                            )
                            {
                                discountAmount =
                                    Math.Min(
                                        discountAmount,

                                        promotion
                                            .GiamToiDa
                                            .Value
                                    );
                            }
                        }

                        else if (
                            promotion.LoaiGiamGia ==
                            "SoTien"
                        )
                        {
                            discountAmount =
                                promotion.GiaTriGiam;
                        }


                        // Không cho tiền giảm lớn hơn tiền phòng
                        discountAmount =
                            Math.Min(
                                discountAmount,
                                roomAmount
                            );
                    }


                    // =============================================
                    // TỔNG THANH TOÁN
                    // =============================================

                    decimal totalAmount =
                        roomAmount +
                        serviceAmount -
                        discountAmount;


                    if (totalAmount < 0)
                    {
                        totalAmount = 0;
                    }


                    // =============================================
                    // TẠO BOOKING
                    // =============================================

                    var booking =
                        new Booking
                        {
                            RoomId =
                                request.RoomId,

                            CustomerName =
                                request
                                    .CustomerName
                                    .Trim(),

                            CustomerPhone =
                                request
                                    .CustomerPhone
                                    .Trim(),

                            CustomerEmail =
                                request
                                    .CustomerEmail
                                    .Trim(),

                            CustomerCCCD =
                                string.IsNullOrWhiteSpace(
                                    request.CustomerCCCD
                                )
                                ? null
                                : request
                                    .CustomerCCCD
                                    .Trim(),

                            CheckInDate =
                                request.CheckInDate,

                            CheckOutDate =
                                request.CheckOutDate,

                            GuestCount =
                                request.GuestCount,

                            RoomCount =
                                request.RoomCount,

                            SpecialRequest =
                                request.SpecialRequest,

                            PaymentMethod =
    paymentMethod,

                            TotalAmount =
                                totalAmount,

                            Status =
                                "Chờ xác nhận",

                            CreatedAt =
                                DateTime.Now
                        };


                    db.Bookings.Add(booking);

                    db.SaveChanges();


                    // =============================================
                    // LƯU CÁC DỊCH VỤ ĐÃ CHỌN
                    // =============================================

                    foreach (
                        var serviceRequest
                        in serviceRequests
                    )
                    {
                        var service =
                            services.First(
                                d =>
                                d.MaDichVu ==
                                serviceRequest.MaDichVu
                            );


                        var bookingService =
                            new DatPhongDichVu
                            {
                                BookingId =
                                    booking.BookingId,

                                MaDichVu =
                                    service.MaDichVu,

                                SoLuong =
                                    serviceRequest
                                        .SoLuong,

                                DonGia =
                                    service.Gia,

                                ThanhTien =
                                    service.Gia *
                                    serviceRequest
                                        .SoLuong
                            };


                        db.DatPhongDichVus.Add(
                            bookingService
                        );
                    }


                    // =============================================
                    // TẠO BẢN GHI THANH TOÁN
                    // =============================================

                    var payment =
                        new ThanhToan
                        {
                            BookingId =
                                booking.BookingId,

                            PhuongThucThanhToan =
    paymentMethod,

                            SoTien =
                                totalAmount,

                            NgayThanhToan =
                                DateTime.Now,

                            TrangThai =
                                "Chờ thanh toán",

                            MaGiaoDich =
                                null,

                            GhiChu =
                                "Đơn đặt phòng #"
                                + booking.BookingId
                        };


                    db.ThanhToans.Add(
                        payment
                    );


                    // =============================================
                    // TĂNG SỐ LƯỢT DÙNG KHUYẾN MÃI
                    // =============================================

                    if (promotion != null)
                    {
                        promotion
                            .SoLuotDaSuDung += 1;
                    }


                    db.SaveChanges();


                    // =============================================
                    // LƯU UserId + MaKhuyenMai + TienGiam
                    //
                    // Dùng SQL trực tiếp để vẫn chạy được
                    // nếu Booking.cs của EDMX chưa sinh lại
                    // 3 thuộc tính này.
                    // =============================================

                    int? userId = null;


                    if (
                        Session["UserId"] != null
                    )
                    {
                        int parsedUserId;


                        if (
                            int.TryParse(
                                Session["UserId"]
                                    .ToString(),

                                out parsedUserId
                            )
                        )
                        {
                            userId =
                                parsedUserId;
                        }
                    }


                    object userIdValue =
                        userId.HasValue

                        ? (object)userId.Value

                        : DBNull.Value;


                    object promotionIdValue =
                        promotion != null

                        ? (object)
                          promotion.MaKhuyenMai

                        : DBNull.Value;


                    db.Database.ExecuteSqlCommand(
                        @"
                        UPDATE dbo.Bookings

                        SET
                            UserId = @p0,
                            MaKhuyenMai = @p1,
                            TienGiam = @p2

                        WHERE BookingId = @p3
                        ",

                        userIdValue,

                        promotionIdValue,

                        discountAmount,

                        booking.BookingId
                    );


                    transaction.Commit();


                    // =============================================
                    // TRẢ VỀ INDEX
                    // =============================================

                    return Json(new
                    {
                        success = true,

                        bookingId =
                            booking.BookingId,

                        roomAmount =
                            roomAmount,

                        serviceAmount =
                            serviceAmount,

                        discountAmount =
                            discountAmount,

                        totalAmount =
                            totalAmount,

                        message =
                            "Đặt phòng thành công."
                    });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();


                    return Json(new
                    {
                        success = false,

                        message =
                            "Có lỗi khi đặt phòng: "
                            + ex.Message
                    });
                }
            }
        }


        // =========================================================
        // KIỂM TRA SỐ PHÒNG CÒN TRỐNG
        // =========================================================

        [HttpGet]
        public JsonResult CheckAvailability(
            int roomId,
            string checkIn,
            string checkOut)
        {
            DateTime checkInDate;
            DateTime checkOutDate;


            bool validCheckIn =
                DateTime.TryParseExact(
                    checkIn,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out checkInDate
                );


            bool validCheckOut =
                DateTime.TryParseExact(
                    checkOut,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out checkOutDate
                );


            if (
                !validCheckIn ||
                !validCheckOut
            )
            {
                return Json(
                    new
                    {
                        success = false,
                        message =
                            "Ngày không hợp lệ."
                    },
                    JsonRequestBehavior.AllowGet
                );
            }


            if (
                checkOutDate <=
                checkInDate
            )
            {
                return Json(
                    new
                    {
                        success = false,

                        message =
                            "Ngày trả phải sau ngày nhận."
                    },
                    JsonRequestBehavior.AllowGet
                );
            }


            var room =
                db.Rooms.Find(roomId);


            if (
                room == null ||
                !room.IsActive
            )
            {
                return Json(
                    new
                    {
                        success = false,
                        message =
                            "Không tìm thấy phòng."
                    },
                    JsonRequestBehavior.AllowGet
                );
            }


            int bookedRooms =
                db.Bookings

                .Where(b =>

                    b.RoomId ==
                    roomId &&

                    b.Status !=
                    "Đã hủy" &&

                    checkInDate <
                    b.CheckOutDate &&

                    checkOutDate >
                    b.CheckInDate

                )

                .Select(
                    b => (int?)b.RoomCount
                )

                .Sum() ?? 0;


            int availableRooms =
                room.TotalRooms -
                bookedRooms;


            if (availableRooms < 0)
            {
                availableRooms = 0;
            }


            return Json(
                new
                {
                    success = true,

                    availableRooms =
                        availableRooms,

                    totalRooms =
                        room.TotalRooms
                },

                JsonRequestBehavior.AllowGet
            );
        }


        // =========================================================
        // GIẢI PHÓNG DB CONTEXT
        // =========================================================

        protected override void Dispose(
            bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}