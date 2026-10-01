using GameStore.Api.Dtos;
const string GetGameEndpointName = "GetGameById";
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
// GET /games/1
app.MapGet("/games/{id}", (int id) => games.Find(game=>game.Id==id))
.WithName(GetGameEndpointName);
// POST /games
app.MapPost("/games", (CreateGameDto newGame) => {
    GameDto game = new (
        games.Count+1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);
    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);

});


// PUT /games/1
app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGame) => {
    var index = games.FindIndex(game => game.Id == id);
    if (index == -1)
    {
        //Console.WriteLine($"Game with ID {id} not found.");
        return Results.NotFound();
    }   
    games[index] = new GameDto(id, updatedGame.Name, updatedGame.Genre, updatedGame.Price, updatedGame.ReleaseDate);
    return Results.NoContent(); 
});
// DELETE /games/1
app.MapDelete("/games/{id}", (int id) => {
    var index = games.FindIndex(game => game.Id == id);
    if (index == -1)
    {
        return Results.NotFound();
    }
    games.RemoveAt(index);
    return Results.NoContent();
}); 
app.Run();
