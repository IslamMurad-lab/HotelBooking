namespace HotelBooking.Models;

public class Hotel
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; }

    [Required, MaxLength(250)]
    public string Address { get; set; }

    [Required, MaxLength(100)]
    public string City { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}