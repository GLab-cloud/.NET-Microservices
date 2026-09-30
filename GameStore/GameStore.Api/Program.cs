using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/", () => "Hello World!");
List<GameDto> games = new List<GameDto>
{
    new GameDto(1, "Game 1", "Action", 59.99m, new DateOnly(2023, 1, 1)),
    new GameDto(2, "Game 2", "Adventure", 39.99m, new DateOnly(2023, 2, 1)),
    new GameDto(3, "Game 3", "RPG", 49.99m, new DateOnly(2023, 3, 1)),
    new GameDto(4, "Game 4", "Strategy", 29.99m, new DateOnly(2023, 4, 1))
};
// GET /games
app.MapGet("/games", () => games);

app.Run();
