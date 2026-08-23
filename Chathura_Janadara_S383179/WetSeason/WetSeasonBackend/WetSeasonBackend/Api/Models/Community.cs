namespace WetSeasonBackend.Api.Models;

public class Community
{
    public int Id {get; set;}
    public string Name { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public int Population { get; set; }
    public string ContactEmail { get; set; } = string.Empty;
    public ICollection<Incident> Incidents { get; set; } = new List<Incident>();    
}