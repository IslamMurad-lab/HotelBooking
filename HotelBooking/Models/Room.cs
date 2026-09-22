namespace HotelBooking.Models;

public class Room
{
    public int Id { get; set; }

    [Required, MaxLength(20)]
    public string RoomNumber { get; set; }

    [Required, MaxLength(50)]
    public string Type { get; set; } 

    [Column(TypeName = "decimal(10,2)")]
    public decimal PricePerNight { get; set; }

    public bool IsActive { get; set; } = true;

    public int HotelId { get; set; }
    [ForeignKey(nameof(HotelId))]
    public Hotel? Hotel { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}