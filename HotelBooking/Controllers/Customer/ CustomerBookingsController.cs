using HotelBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.Controllers.Customer
{
    [Authorize(Roles = "Guest")]
    [Route("Bookings")]
    public class CustomerBookingsController : Controller
    {
        private readonly ApplicationDbContext db;

        public CustomerBookingsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var bookings = db.Bookings
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                .Where(b => b.UserId == userId.Value)
                .OrderByDescending(b => b.CreatedAt)
                .ToList();

            return View("~/Views/Customer/Bookings/Index.cshtml", bookings);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            var rooms = db.Rooms
                .Include(r => r.Hotel)
                .Where(r => r.IsActive)
                .ToList();

            ViewBag.Rooms = rooms;

            return View("~/Views/Customer/Bookings/Create.cshtml");
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Booking model)
        {
            var userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (model.CheckInDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(model.CheckInDate), "Check-in date cannot be in the past.");
            }

            if (model.CheckOutDate.Date <= model.CheckInDate.Date)
            {
                ModelState.AddModelError(nameof(model.CheckOutDate), "Check-out date must be after check-in date.");
            }

            var room = db.Rooms
                .Include(r => r.Hotel)
                .FirstOrDefault(r => r.Id == model.RoomId && r.IsActive);

            if (room == null)
            {
                ModelState.AddModelError(nameof(model.RoomId), "Selected room is not available.");
            }

            if (room != null)
            {
                bool roomBooked = db.Bookings.Any(b =>
                    b.RoomId == model.RoomId &&
                    b.Status != "Cancelled" &&
                    model.CheckInDate < b.CheckOutDate &&
                    model.CheckOutDate > b.CheckInDate);

                if (roomBooked)
                {
                    ModelState.AddModelError(string.Empty, "This room is already booked for the selected dates.");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Rooms = db.Rooms
                    .Include(r => r.Hotel)
                    .Where(r => r.IsActive)
                    .ToList();

                return View("~/Views/Customer/Bookings/Create.cshtml", model);
            }

            int numberOfNights = (model.CheckOutDate.Date - model.CheckInDate.Date).Days;

            model.UserId = userId.Value;
            model.Status = "Pending";
            model.CreatedAt = DateTime.UtcNow;
            model.TotalPrice = room.PricePerNight * numberOfNights;

            db.Bookings.Add(model);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var booking = db.Bookings
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                .FirstOrDefault(b => b.Id == id && b.UserId == userId.Value);

            if (booking == null)
                return NotFound();

            return View("~/Views/Customer/Bookings/Details.cshtml", booking);
        }

        [HttpGet("Cancel/{id}")]
        public IActionResult Cancel(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var booking = db.Bookings
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                .FirstOrDefault(b => b.Id == id && b.UserId == userId.Value);

            if (booking == null)
                return NotFound();

            return View("~/Views/Customer/Bookings/Cancel.cshtml", booking);
        }

        [HttpPost("Cancel/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult CancelConfirmed(int id)
        {
            var userId = GetUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var booking = db.Bookings
                .FirstOrDefault(b => b.Id == id && b.UserId == userId.Value);

            if (booking == null)
                return NotFound();

            if (booking.Status == "Cancelled" || booking.Status == "Completed")
            {
                return RedirectToAction("Index");
            }

            booking.Status = "Cancelled";

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        private int? GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int id))
                return id;

            return null;
        }
    }
}