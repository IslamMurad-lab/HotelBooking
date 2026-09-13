namespace HotelBooking.Models;

public class Room
{
    public int Id { get; set; }

    [Required, MaxLength(20)]
    public string RoomNumber { get; set; }

    [Required, MaxLength(50)]
    public string Type { get; set; } // Single, Double, Suite...

    [Column(TypeName = "decimal(10,2)")]
    public decimal PricePerNight { get; set; }

    public bool IsActive { get; set; } = true;

    // FK
    public int HotelId { get; set; }
    [ForeignKey(nameof(HotelId))]
    public Hotel? Hotel { get; set; }

    // Navigation
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}