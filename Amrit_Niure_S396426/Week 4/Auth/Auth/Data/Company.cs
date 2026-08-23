namespace Auth.Data;

public sealed class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;

    // The ApplicationUser.Id of whoever created this company - the ownership boundary
    // CompanyOwnerOrPermissionRequirement scopes job:create/update/delete against. Unlike the
    // standalone jobboard service, this can be a real FK since Identity lives in this same DB.
    public string OwnerId { get; set; } = string.Empty;
    public ApplicationUser Owner { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; }

    public ICollection<Job> Jobs { get; set; } = [];
}
