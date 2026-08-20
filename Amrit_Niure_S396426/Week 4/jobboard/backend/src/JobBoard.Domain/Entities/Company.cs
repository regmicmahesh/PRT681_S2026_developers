using JobBoard.Domain.Common;
using JobBoard.Domain.ValueObjects;

namespace JobBoard.Domain.Entities;

public class Company : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public Email ContactEmail { get; private set; } = null!;

    // The Auth service's user id (JWT `sub`) of whoever created this company. This is the
    // ownership boundary the job-board's authorization layer scopes job:create/update/delete
    // against - see JobBoard.Api/Authorization/CompanyOwnerOrPermissionAuthorizationHandler.
    public string OwnerId { get; private set; } = null!;

    // Reserved for EF Core materialization.
    private Company()
    {
    }

    public Company(Guid id, string name, Email contactEmail, string ownerId) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        if (string.IsNullOrWhiteSpace(ownerId))
            throw new ArgumentException("OwnerId is required.", nameof(ownerId));

        Name = name;
        ContactEmail = contactEmail ?? throw new ArgumentNullException(nameof(contactEmail));
        OwnerId = ownerId;
    }
}
