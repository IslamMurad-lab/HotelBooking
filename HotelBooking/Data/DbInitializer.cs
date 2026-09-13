namespace HotelBooking.Data;

public static class DbInitializer
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

        // لو فيه Admin بالفعل، متعملش حاجة
        if (context.Users.Any(u => u.Role == "Admin"))
            return;

        var admin = new User
        {
            Name = "System Admin",
            Email = "admin@gmail.com",
            Role = "Admin"
        };

        admin.PasswordHash = passwordHasher.HashPassword(admin, "123456");

        context.Users.Add(admin);
        context.SaveChanges();
    }
}