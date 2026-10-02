using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;
public record GameDto(
    [property: Required] int Id,
    [property: Required] string Name,
    [property: Required] string Genre,
    //[property: Required] 
    decimal Price,
    // [property: Required] 
    DateOnly ReleaseDate);