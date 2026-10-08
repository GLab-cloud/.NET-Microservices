using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;
public record GameDetailsDto(
    [property: Required] int Id,
    [property: Required][StringLength(50)] string Name,
    [property: Range(1,50)] int GenreId,
    [property: Range(typeof(decimal), "1", "100")] decimal Price,
    DateOnly ReleaseDate);