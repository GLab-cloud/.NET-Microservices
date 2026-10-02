using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;
public record CreateGameDto(
    [property: Required] string Name,
    [property: Required] string Genre,
    [property: Required] decimal Price,
    [property: Required] DateOnly ReleaseDate);