namespace CleanApp.Infrastructure.Email;

public sealed class EmailSettings
{
    public const string SectionName = "Email";

    public bool Enabled { get; set; }
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool UseSsl { get; set; } = true;
    public string FromAddress { get; set; } = "noreply@cleanapp.local";
    public string FromName { get; set; } = "CleanApp Todo";
    public string NotificationRecipient { get; set; } = "user@cleanapp.local";
}
