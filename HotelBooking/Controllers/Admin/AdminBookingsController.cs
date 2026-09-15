using HotelBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminBookingsController : Controller
    {
        private readonly ApplicationDbContext db;

        public AdminBookingsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var bookings = db.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                .ToList();

            return View("~/Views/Admin/Bookings/Index.cshtml", bookings);
        }

        public IActionResult Details(int id)
        {
            var booking = db.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View("~/Views/Admin/Bookings/Details.cshtml", booking);
        }

        public IActionResult Edit(int id)
        {
            var booking = db.Bookings.Find(id);

            if (booking == null)
                return NotFound();

            return View("~/Views/Admin/Bookings/Edit.cshtml", booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Booking model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Bookings/Edit.cshtml", model);

            var booking = db.Bookings.Find(model.Id);

            if (booking == null)
                return NotFound();

            booking.Status = model.Status;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var booking = db.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View("~/Views/Admin/Bookings/Delete.cshtml", booking);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var booking = db.Bookings.Find(id);

            if (booking == null)
                return NotFound();

            db.Bookings.Remove(booking);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}