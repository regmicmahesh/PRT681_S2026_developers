using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public DateTime DOB { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    // Make this nullable if you want it to be optional
    public string? ProfilePictureUrl { get; set; }
}
