using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HotelBooking.ViewModels;

public class RoomCreateViewModel
{
    public Room Room { get; set; }

    [ValidateNever]
    public List<Hotel> Hotels { get; set; }
}