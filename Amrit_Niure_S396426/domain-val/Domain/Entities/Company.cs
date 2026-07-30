namespace Domain.Entities;

public class Company
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Website { get; private set; }

    private Company() { }

    public Company(Guid id, string name, string description, string website)
    {
        Id = id;
        Name = name;
        Description = description;
        Website = website;
    }
}
