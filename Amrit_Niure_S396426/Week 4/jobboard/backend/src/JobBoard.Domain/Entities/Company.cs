using JobBoard.Domain.Common;
using JobBoard.Domain.ValueObjects;

namespace JobBoard.Domain.Entities;

public class Company : AggregateRoot
{
    public string Name { get; private set; }
    public Email ContactEmail { get; private set; }

    public Company(Guid id, string name, Email contactEmail) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        Name = name;
        ContactEmail = contactEmail ?? throw new ArgumentNullException(nameof(contactEmail));
    }
}
