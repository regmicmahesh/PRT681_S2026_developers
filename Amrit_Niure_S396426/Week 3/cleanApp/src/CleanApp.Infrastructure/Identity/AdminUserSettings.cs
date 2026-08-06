namespace CleanApp.Infrastructure.Identity;

public sealed class AdminUserSettings
{
    public const string SectionName = "AdminUser";

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
