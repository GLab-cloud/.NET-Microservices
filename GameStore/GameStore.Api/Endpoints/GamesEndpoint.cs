using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;  
public static class GamesEndpoint
{          
    const string GetGameEndpointName = "GetGameById";

    private static List<GameSummaryDto> games = new List<GameSummaryDto>
        {
            new GameSummaryDto(1, "Game 1", "Action", 59.99m, new DateOnly(2023, 1, 1)),
            new GameSummaryDto(2, "Game 2", "Adventure", 39.99m, new DateOnly(2023, 2, 1)),
            new GameSummaryDto(3, "Game 3", "RPG", 49.99m, new DateOnly(2023, 3, 1)),
            new GameSummaryDto(4, "Game 4", "Strategy", 29.99m, new DateOnly(2023, 4, 1))
        };
    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games").WithTags("Games");
        
        // GET /games
        group.MapGet("/", async (GameStoreContext DbContext) => await DbContext.Games.Include(g => g.Genre).Select(game => new GameSummaryDto(game.Id, game.Name, game.Genre!.Name, game.Price, game.ReleaseDate)).AsNoTracking().ToListAsync());

        // GET /games/1
        group.MapGet("/{id}", async (int id,GameStoreContext DbContext) => {
            var game = await DbContext.Games.FindAsync(id);
            return game is not null ? Results.Ok(new GameDetailsDto(game.Id, game.Name, game.GenreId, game.Price, game.ReleaseDate)) : Results.NotFound();
        })
        .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", async (CreateGameDto newGame,GameStoreContext DbContext) => {
            // if(string.IsNullOrEmpty(newGame.Name) || string.IsNullOrEmpty(newGame.Genre) || newGame.Price <= 0)
            // {
            //     return Results.BadRequest("Invalid game data.");
            // }
            // GameDto game = new (
            //     games.Count + 1,
            //     newGame.Name,
            //     newGame.Genre,
            //     newGame.Price,
            //     newGame.ReleaseDate);
            Game game = new (){
                Name=newGame.Name,
                GenreId=newGame.GenreId,
                Price=newGame.Price,
                ReleaseDate=newGame.ReleaseDate
                };
            DbContext.Games.Add(game);
            await DbContext.SaveChangesAsync();
            GameDetailsDto gameDetailsDto=new GameDetailsDto(game.Id,game.Name,game.GenreId,game.Price,game.ReleaseDate);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDetailsDto.Id }, gameDetailsDto );
        });

        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) => {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }
            games[index] = new GameSummaryDto(id, updatedGame.Name, updatedGame.Genre, updatedGame.Price, updatedGame.ReleaseDate);
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