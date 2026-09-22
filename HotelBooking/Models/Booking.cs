namespace HotelBooking.Models;

public class Booking
{
    public int Id { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    [Required, MaxLength(30)]
    public string Status { get; set; } = "Pending"; 

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

 
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public int RoomId { get; set; }
    [ForeignKey(nameof(RoomId))]
    public Room? Room { get; set; }
}