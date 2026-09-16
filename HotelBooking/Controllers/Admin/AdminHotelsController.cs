using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers.Admin
{
    public class AdminHotelsController : Controller
    {
        private readonly ApplicationDbContext db;

        public AdminHotelsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        
        public IActionResult Index()
        {
            var hotels = db.Hotels.ToList();

            return View("~/Views/Admin/Hotels/Index.cshtml", hotels);
        }

        public IActionResult Create()
        {
            return View("~/Views/Admin/Hotels/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hotel hotel)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Hotels/Create.cshtml", hotel);

            db.Hotels.Add(hotel);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var hotel = db.Hotels
                .Include(h => h.Rooms)
                .FirstOrDefault(h => h.Id == id);

            if (hotel == null)
                return NotFound();

            return View("~/Views/Admin/Hotels/Details.cshtml", hotel);
        }

        public IActionResult Edit(int id)
        {
            var hotel = db.Hotels.Find(id);

            if (hotel == null)
                return NotFound();

            return View("~/Views/Admin/Hotels/Edit.cshtml", hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Hotel hotel)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Hotels/Edit.cshtml", hotel);

            db.Hotels.Update(hotel);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var hotel = db.Hotels.Find(id);

            if (hotel == null)
                return NotFound();

            return View("~/Views/Admin/Hotels/Delete.cshtml", hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var hotel = db.Hotels.Find(id);

            if (hotel == null)
                return NotFound();

            db.Hotels.Remove(hotel);
            db.SaveChanges();

            return RedirectToAction("Index");
        }



    }
}
