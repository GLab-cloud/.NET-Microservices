using GameStore.Api.Data;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();

        var genres = new[]
        {
            new Genre { Id = 1, Name = "Action" },
            new Genre { Id = 2, Name = "Adventure" },
            new Genre { Id = 3, Name = "RPG" },
            new Genre { Id = 4, Name = "Strategy" }
        };
        var existingGenreIds = dbContext.Genres
            .Where(genre => genres.Select(seed => seed.Id).Contains(genre.Id))
            .Select(genre => genre.Id)
            .ToHashSet();
        dbContext.Genres.AddRange(genres.Where(genre => !existingGenreIds.Contains(genre.Id)));

        var games = new[]
        {
            new Game { Id = 1, Name = "Stardew Valley", GenreId = 3, ReleaseDate = new DateOnly(2016, 2, 26), Price = 14.99m },
            new Game { Id = 2, Name = "Hades", GenreId = 1, ReleaseDate = new DateOnly(2020, 9, 17), Price = 24.99m },
            new Game { Id = 3, Name = "The Legend of Zelda: Breath of the Wild", GenreId = 2, ReleaseDate = new DateOnly(2017, 3, 3), Price = 59.99m },
            new Game { Id = 4, Name = "Civilization VI", GenreId = 4, ReleaseDate = new DateOnly(2016, 10, 21), Price = 59.99m }
        };
        var existingGameIds = dbContext.Games
            .Where(game => games.Select(seed => seed.Id).Contains(game.Id))
            .Select(game => game.Id)
            .ToHashSet();
        dbContext.Games.AddRange(games.Where(game => !existingGameIds.Contains(game.Id)));
        dbContext.SaveChanges();
    }
}