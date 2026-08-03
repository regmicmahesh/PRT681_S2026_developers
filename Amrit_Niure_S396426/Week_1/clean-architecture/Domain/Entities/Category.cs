using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class Category : BaseEntity<Guid>
{
    public string Name { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public string? Description { get; private set; }

    private Category() { } // Required for ORM

    public Category(Guid id, string name, string slug, string? description = null) : base(id)
    {
        UpdateDetails(name, slug, description);
    }

    public static Category Create(string name, string slug, string? description = null)
    {
        return new Category(Guid.NewGuid(), name, slug, description);
    }

    public void UpdateDetails(string name, string slug, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name is required.");

        if (string.IsNullOrWhiteSpace(slug))
            throw new DomainException("Category slug is required.");

        Name = name.Trim();
        Slug = slug.Trim().ToLowerInvariant();
        Description = description?.Trim();
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
