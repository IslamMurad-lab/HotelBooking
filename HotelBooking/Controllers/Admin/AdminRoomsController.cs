using HotelBooking.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers.Admin
{
    public class AdminRoomsController : Controller
    {
        private readonly ApplicationDbContext db;

        public AdminRoomsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        
        public IActionResult Index()
        {
            var rooms = db.Rooms
                .Include(r => r.Hotel)
                .ToList();
            return View("~/Views/Admin/Rooms/Index.cshtml", rooms);
        }

        public IActionResult Create()
        {
            var model = new RoomCreateViewModel
            {
                Hotels = db.Hotels.ToList()
            };

            return View("~/Views/Admin/Rooms/Create.cshtml", model);
        }

        [HttpPost]
        public IActionResult Create(RoomCreateViewModel model)
        {
            db.Rooms.Add(model.Room);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var room = db.Rooms.Find(id);

            if (room == null)
                return NotFound();

            var model = new RoomCreateViewModel
            {
                Room = room,
                Hotels = db.Hotels.ToList()
            };

            return View("~/Views/Admin/Rooms/Edit.cshtml", model);
        }

        [HttpPost]
        public IActionResult Edit(RoomCreateViewModel model)
        {
            db.Rooms.Update(model.Room);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var room = db.Rooms
                .Include(r => r.Hotel)
                .FirstOrDefault(r => r.Id == id);

            if (room == null)
                return NotFound();

            return View("~/Views/Admin/Rooms/Details.cshtml", room);
        }

        public IActionResult Delete(int id)
        {
            var room = db.Rooms.Find(id);

            if (room == null)
                return NotFound();

            return View("~/Views/Admin/Rooms/Delete.cshtml", room);
        }

        [HttpPost]
        public IActionResult Delete(Room room)
        {
            db.Rooms.Remove(room);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
