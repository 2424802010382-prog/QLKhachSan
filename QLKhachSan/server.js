using System.Web.Mvc;
using System.Collections.Generic;

namespace QLKhachSan.Controllers {
    public class HomeController : Controller
    {
        // Action trả về danh sách phòng dạng JSON cho giao diện
        [HttpGet]
        public JsonResult GetRooms()
        {
            var rooms = new List < object >
            {
                new { id = 1, name = "Standard Single", price = 500000, status = "Trống" },
                new { id = 2, name = "Deluxe Double", price = 1200000, status = "Trống" },
                new { id = 3, name = "VIP Suite", price = 2500000, status = "Trống" }
            };

            return Json(rooms, JsonRequestBehavior.AllowGet);
        }
    }
}