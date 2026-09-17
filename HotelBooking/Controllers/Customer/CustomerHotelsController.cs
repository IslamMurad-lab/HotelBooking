using HotelBooking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Controllers.Customer
{
    [Route("Hotels")]
    public class CustomerHotelsController : Controller
    {
        private readonly ApplicationDbContext db;

        public CustomerHotelsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet("")]
        public IActionResult Index(string? city)
        {
            var hotelsQuery = db.Hotels.AsQueryable();

            if (!string.IsNullOrWhiteSpace(city))
            {
                hotelsQuery = hotelsQuery.Where(h => h.City.Contains(city));
            }

            ViewBag.SelectedCity = city;
            ViewBag.Cities = db.Hotels.Select(h => h.City).Distinct().ToList();

            var hotels = hotelsQuery.ToList();

            return View("~/Views/Customer/Hotels/Index.cshtml", hotels);
        }

        [HttpGet("Details/{id}")]
        public IActionResult Details(int id, string? type)
        {
            var hotel = db.Hotels
                .Include(h => h.Rooms)
                .FirstOrDefault(h => h.Id == id);

            if (hotel == null)
                return NotFound();

            var rooms = hotel.Rooms.Where(r => r.IsActive).AsEnumerable();

            if (!string.IsNullOrWhiteSpace(type))
            {
                rooms = rooms.Where(r => r.Type == type);
            }

            ViewBag.SelectedType = type;
            ViewBag.Types = hotel.Rooms.Where(r => r.IsActive).Select(r => r.Type).Distinct().ToList();
            ViewBag.FilteredRooms = rooms.ToList();

            return View("~/Views/Customer/Hotels/Details.cshtml", hotel);
        }
    }
}