using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WetSeasonBackend.Api.Data;
using WetSeasonBackend.Api.Dtos;
using WetSeasonBackend.Api.Models;

namespace WetSeasonBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class IncidentController(AppDbContext db) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Incident>>> GetAllIncidents()
  {
    var incidents = await db.Incidents
      .OrderByDescending(i => i.CreatedAt)
      .Select(i => new IncidentListItemDto
      {
        Id = i.Id,
        Type = i.Type.ToString(),
        Severity = i.Severity,
        Status = i.Status.ToString(),
        CommunityName = i.Community.Name,
        Region = i.Community.Region,
        ReportedBy = i.ReportedBy,
        CreatedAt = i.CreatedAt,
      })
      .ToListAsync();
    return Ok(incidents);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<IEnumerable<Incident>>> GetIncidentById(int id)
  {
    var incident = await db.Incidents.FirstOrDefaultAsync(i => i.Id == id);
    return Ok(incident);
  }

  [HttpPost]
  public async Task<ActionResult<IncidentListItemDto>> CreateIncident(CreateIncidentRequestDto request)
  {
    var communityExists = await db.Communities.AnyAsync(c => c.Id == request.CommunityId);
    if (!communityExists)
    {
      return NotFound("$Community {request.CommunityId} does not exist");
    }

    var incident = new Incident
    {
      CommunityId = request.CommunityId,
      Type = request.Type,
      Severity = request.Severity,
      Description = request.Description,
      Status = IncidentStatus.Reported,
      ReportedBy = "Chathura",
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow
    };
    db.Incidents.Add(incident);
    await db.SaveChangesAsync();
    return CreatedAtAction("GetIncidentById", "Incident", new { id = incident.Id }, new IncidentListItemDto()
    {
      Id = incident.Id,
      Type = incident.Type.ToString(),
      Severity = incident.Severity,
      Status = incident.Status.ToString(),
      ReportedBy = incident.ReportedBy,
      CreatedAt = incident.CreatedAt,
    });
    return Ok();
  }

}