using HotelBooking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Controllers.Customer
{
    public class CustomerRoomController : Controller
    {
        private readonly ApplicationDbContext db;

        public CustomerRoomController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Index(string? type)
        {
            var roomShow = db.Rooms
                .Include(r => r.Hotel)
                .Where(r => r.IsActive);

            if (!string.IsNullOrEmpty(type))
            {
                roomShow = roomShow.Where(r => r.Type == type);
            }

            var rooms = roomShow.ToList();
            ViewBag.SelectedType = type;

            return View("~/Views/Customer/Rooms/Index.cshtml", rooms);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var room = db.Rooms
                .Include(r => r.Hotel)
                .FirstOrDefault(r => r.Id == id && r.IsActive);

            if (room == null)
            {
                return NotFound();
            }

            return View("~/Views/Customer/Rooms/Details.cshtml", room);
        }
    }
}