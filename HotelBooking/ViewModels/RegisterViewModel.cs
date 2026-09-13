namespace HotelBooking.ViewModels;

public class RegisterViewModel
{
    [Required, MaxLength(100)]
    [Display(Name = "Full Name")]
    public string Name { get; set; }

    [Required, EmailAddress, MaxLength(150)]
    [Display(Name = "Email Address")]
    public string Email { get; set; }

    [Required, MinLength(6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }
}