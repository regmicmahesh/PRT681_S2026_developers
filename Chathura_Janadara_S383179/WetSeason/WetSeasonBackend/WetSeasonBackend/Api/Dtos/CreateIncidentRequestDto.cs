using WetSeasonBackend.Api.Models;

namespace WetSeasonBackend.Api.Dtos;

public class CreateIncidentRequestDto
{
    public int CommunityId { get; set; }
    public IncidentType Type { get; set; }
    public int Severity { get; set; }
    public string Description { get; set; } = string.Empty;
}