using QLKhachSan.Models;
using System;

using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using QLKhachSan.Filters;

namespace QLKhachSan.Controllers
{
    [AdminAuthorize]

    public class AdminController : Controller
    {
        private HotelDbContext db = new HotelDbContext();

        // DANH SÁCH ĐƠN ĐẶT PHÒNG
        public ActionResult Index(
            string keyword,
            string status,
            DateTime? fromDate,
            DateTime? toDate)
        {
            var query = db.Bookings
    .Include(b => b.Room)
    .AsQueryable();


            // TÌM THEO TÊN / SĐT / EMAIL
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(b =>
                    b.CustomerName.Contains(keyword) ||
                    b.CustomerPhone.Contains(keyword) ||
                    b.CustomerEmail.Contains(keyword)
                );
            }


            // LỌC THEO TRẠNG THÁI
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(b =>
                    b.Status == status
                );
            }


            // LỌC TỪ NGÀY
            if (fromDate.HasValue)
            {
                DateTime startDate =
                    fromDate.Value.Date;

                query = query.Where(b =>
                    b.CreatedAt >= startDate
                );
            }


            // LỌC ĐẾN NGÀY
            if (toDate.HasValue)
            {
                DateTime endDate =
                    toDate.Value.Date.AddDays(1);

                query = query.Where(b =>
                    b.CreatedAt < endDate
                );
            }


            // LẤY DANH SÁCH SAU KHI LỌC
            var bookings = query
                .OrderByDescending(b => b.CreatedAt)
                .ToList();
            ViewBag.Keyword = keyword;
            ViewBag.Status = status;

            ViewBag.FromDate =
                fromDate.HasValue
                ? fromDate.Value.ToString("yyyy-MM-dd")
                : "";

            ViewBag.ToDate =
                toDate.HasValue
                ? toDate.Value.ToString("yyyy-MM-dd")
                : "";


            // =========================
            // THỐNG KÊ ĐƠN
            // =========================

            ViewBag.TotalBookings =
                db.Bookings.Count();

            ViewBag.PendingBookings =
                db.Bookings.Count(b =>
                    b.Status == "Chờ xác nhận");

            ViewBag.ConfirmedBookings =
                db.Bookings.Count(b =>
                    b.Status == "Đã xác nhận");

            ViewBag.CancelledBookings =
                db.Bookings.Count(b =>
                    b.Status == "Đã hủy");


            // =========================
            // DOANH THU HÔM NAY
            // =========================

            DateTime today =
                DateTime.Today;

            DateTime tomorrow =
                today.AddDays(1);

            decimal todayRevenue =
                db.Bookings
                .Where(b =>
                    b.Status == "Đã xác nhận" &&
                    b.CreatedAt >= today &&
                    b.CreatedAt < tomorrow)
                .Select(b => (decimal?)b.TotalAmount)
                .Sum() ?? 0;

            ViewBag.TodayRevenue =
                todayRevenue;


            // =========================
            // DOANH THU THÁNG NÀY
            // =========================

            DateTime firstDayOfMonth =
                new DateTime(
                    today.Year,
                    today.Month,
                    1
                );

            DateTime firstDayNextMonth =
                firstDayOfMonth.AddMonths(1);

            decimal monthRevenue =
                db.Bookings
                .Where(b =>
                    b.Status == "Đã xác nhận" &&
                    b.CreatedAt >= firstDayOfMonth &&
                    b.CreatedAt < firstDayNextMonth)
                .Select(b => (decimal?)b.TotalAmount)
                .Sum() ?? 0;

            ViewBag.MonthRevenue =
                monthRevenue;

            // =========================
            // DOANH THU 7 NGÀY GẦN NHẤT
            // =========================

            var revenueLabels = new string[7];
            var revenueData = new decimal[7];

            for (int i = 6; i >= 0; i--)
            {
                DateTime date = DateTime.Today.AddDays(-i);
                DateTime nextDate = date.AddDays(1);

                int index = 6 - i;

                revenueLabels[index] =
                    date.ToString("dd/MM");

                revenueData[index] =
                    db.Bookings
                    .Where(b =>
                        b.Status == "Đã xác nhận" &&
                        b.CreatedAt >= date &&
                        b.CreatedAt < nextDate
                    )
                    .Select(b => (decimal?)b.TotalAmount)
                    .Sum() ?? 0;
            }

            ViewBag.RevenueLabels = revenueLabels;
            ViewBag.RevenueData = revenueData;
            return View(bookings);
        }

        // XÁC NHẬN ĐƠN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(int id)
        {
            var booking = db.Bookings.Find(id);

            if (booking == null)
            {
                return HttpNotFound();
            }

            booking.Status = "Đã xác nhận";

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // HỦY ĐƠN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(int id)
        {
            var booking = db.Bookings.Find(id);

            if (booking == null)
            {
                return HttpNotFound();
            }

            booking.Status = "Đã hủy";

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}