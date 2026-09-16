using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                var stats = new AdminDashboardViewModel
                {
                    TotalHotels = db.Hotels.Count(),
                    TotalRooms = db.Rooms.Count(),
                    ActiveRooms = db.Rooms.Count(r => r.IsActive),
                    TotalBookings = db.Bookings.Count(),
                    PendingBookings = db.Bookings.Count(b => b.Status == "Pending"),
                    ConfirmedBookings = db.Bookings.Count(b => b.Status == "Confirmed"),
                    TotalUsers = db.Users.Count(u => u.Role == "Guest")
                };

                return View("~/Views/Home/AdminDashboard.cshtml", stats);
            }

            return View("~/Views/Home/Index.cshtml");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}