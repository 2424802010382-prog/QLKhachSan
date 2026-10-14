using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLKhachSan.Models;

namespace QLKhachSan.Controllers

{
    public class HomeController : Controller

    {
        private HotelDbContext db = new HotelDbContext();
        public ActionResult Index()
        {
            var rooms = db.Rooms
                          .Where(r => r.IsActive)
                          .ToList();

            return View(rooms);
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}