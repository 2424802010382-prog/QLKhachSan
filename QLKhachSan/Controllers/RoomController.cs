using QLKhachSan.Models;
using System.Linq;
using System.Web.Mvc;

namespace QLKhachSan.Controllers
{
    public class RoomController : Controller
    {
        // Kết nối database
        private readonly HotelDbContext db =
            new HotelDbContext();


        // =========================================================
        // TRANG CHI TIẾT PHÒNG
        // URL ví dụ:
        // /Room/Details/1
        // =========================================================

        public ActionResult Details(int id)
        {
            // Tìm đúng phòng theo RoomId
            var room =
                db.Rooms.FirstOrDefault(r =>
                    r.RoomId == id &&
                    r.IsActive
                );


            // Nếu không tìm thấy phòng
            if (room == null)
            {
                return HttpNotFound();
            }


            // Gửi thông tin phòng sang
            // Views/Room/Details.cshtml
            return View(room);
        }


        // =========================================================
        // GIẢI PHÓNG KẾT NỐI DATABASE
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