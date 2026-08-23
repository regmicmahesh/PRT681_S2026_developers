namespace WetSeasonBackend.Api.Dtos;

public class IncidentListItemDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public int Severity { get; set; }
    public string Status { get; set; } = string.Empty;
    public string CommunityName { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string ReportedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}