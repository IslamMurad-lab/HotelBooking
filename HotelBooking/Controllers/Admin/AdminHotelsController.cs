using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminHotelsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment env;

        public AdminHotelsController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
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
        public IActionResult Create(Hotel hotel, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Hotels/Create.cshtml", hotel);

            if (ImageFile != null && ImageFile.Length > 0)
            {
                hotel.ImagePath = SaveImage(ImageFile);
            }

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
        public IActionResult Edit(Hotel hotel, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Hotels/Edit.cshtml", hotel);

            var existingHotel = db.Hotels.AsNoTracking().FirstOrDefault(h => h.Id == hotel.Id);
            if (existingHotel == null)
                return NotFound();

            if (ImageFile != null && ImageFile.Length > 0)
            {
                hotel.ImagePath = SaveImage(ImageFile);
            }
            else
            {
                hotel.ImagePath = existingHotel.ImagePath; // احتفظ بالصورة القديمة لو مفيش صورة جديدة
            }

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
        [ActionName("Delete")]
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

        private string SaveImage(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Invalid image format.");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var uploadsFolder = Path.Combine(env.WebRootPath, "images", "hotels");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"/images/hotels/{fileName}";
        }
    }
}