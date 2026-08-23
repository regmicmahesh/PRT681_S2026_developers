namespace WetSeasonBackend.Api.Models;

public class Users
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public required Role Role { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}