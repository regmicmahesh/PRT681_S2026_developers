namespace WetSeasonBackend.Api.Models;

public enum IncidentStatus
{
    Reported,
    Triaged,
    Responding,
    Resolved
}

public enum IncidentType
{
    Flooding,
    CycloneDamage,
    RoadClosure,
    Evacuation,
    InfrastructureDamage
}