using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;  
public static class GamesEndpoint
{          
    const string GetGameEndpointName = "GetGameById";

    private static List<GameDto> games = new List<GameDto>
        {
            new GameDto(1, "Game 1", "Action", 59.99m, new DateOnly(2023, 1, 1)),
            new GameDto(2, "Game 2", "Adventure", 39.99m, new DateOnly(2023, 2, 1)),
            new GameDto(3, "Game 3", "RPG", 49.99m, new DateOnly(2023, 3, 1)),
            new GameDto(4, "Game 4", "Strategy", 29.99m, new DateOnly(2023, 4, 1))
        };
    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games").WithTags("Games");
        
        // GET /games
        group.MapGet("/", () => games);

        // GET /games/1
        group.MapGet("/{id}", (int id) => {
            var game = games.Find(game => game.Id == id);
            return game is not null ? Results.Ok(game) : Results.NotFound();
        })
        .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame) => {
            GameDto game = new (
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate);
            games.Add(game);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) => {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }
            games[index] = new GameDto(id, updatedGame.Name, updatedGame.Genre, updatedGame.Price, updatedGame.ReleaseDate);
            return Results.NoContent();
        });

        // DELETE /games/1
        group.MapDelete("/{id}", (int id) => {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }
            games.RemoveAt(index);
            return Results.NoContent();
        });
    }
}