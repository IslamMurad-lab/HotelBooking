namespace HotelBooking.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalHotels { get; set; }
    public int TotalRooms { get; set; }
    public int ActiveRooms { get; set; }
    public int TotalBookings { get; set; }
    public int PendingBookings { get; set; }
    public int ConfirmedBookings { get; set; }
    public int TotalUsers { get; set; }
}