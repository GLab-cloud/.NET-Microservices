using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;
public record UpdateGameDto(
    [property: Required][StringLength(50)] string Name,
    [property: Required][StringLength(20)]  string Genre,
    [property: Range(typeof(decimal), "1", "100")] decimal Price,
    DateOnly ReleaseDate);    