namespace Domain.Entities;

public class JobPosting
{
    public Guid Id { get; private set; }
    public Guid CompanyId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Location { get; private set; }
    public DateTime PostedOnUtc { get; private set; }
    public DateTime? ClosesOnUtc { get; private set; }

    private JobPosting() { }

    public JobPosting(
        Guid id,
        Guid companyId,
        string title,
        string description,
        string location,
        DateTime postedOnUtc,
        DateTime? closesOnUtc)
    {
        Id = id;
        CompanyId = companyId;
        Title = title;
        Description = description;
        Location = location;
        PostedOnUtc = postedOnUtc;
        ClosesOnUtc = closesOnUtc;
    }
}
