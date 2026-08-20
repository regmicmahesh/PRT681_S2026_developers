using Auth.Data;

namespace Auth.Auth;

// Shared shape returned by the auth/users endpoints so we never hand back the raw ApplicationUser
// entity (which carries PasswordHash, SecurityStamp, etc.).
public record UserResponse(string Id, string? Email, string Initials, bool EnableNotifications)
{
    public static UserResponse FromUser(ApplicationUser user) =>
        new(user.Id, user.Email, user.Initials, user.EnableNotifications);
}
