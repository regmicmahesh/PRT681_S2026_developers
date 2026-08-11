using System.ComponentModel.DataAnnotations;

namespace GameStore.DTOs;

public record GameDto(
    int Id,
    [Required][StringLength(50)]string Name,
    string Genre,
    double   Price,
    string Description,
    DateOnly ReleaseDate
    );