using Microsoft.AspNetCore.Identity;

namespace CleanApp.Persistence.Identity;

/// <summary>
/// The EF Core Identity user entity. Lives in Persistence (not Domain) because Identity is
/// an infrastructure/persistence concern - the Domain only ever sees the opaque UserId it
/// gets tagged with, never this type.
/// </summary>
public sealed class ApplicationUser : IdentityUser<Guid>;
