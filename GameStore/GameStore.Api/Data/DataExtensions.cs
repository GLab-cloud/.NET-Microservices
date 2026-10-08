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
       
    }
    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetConnectionString("GameStore")??throw new InvalidOperationException("Connection string 'GameStore' is not found.");
        builder.Services.AddSqlite<GameStoreContext>(connString, optionsAction: option => option.UseSeeding ((context, _) =>
{
    if(!context.Set<Genre>().Any())
    {
        context.Set<Genre>().AddRange(
            new Genre { Id = 1, Name = "Action" },
            new Genre { Id = 2, Name = "Adventure" },
            new Genre { Id = 3, Name = "RPG" },
            new Genre { Id = 4, Name = "Strategy" }
        );
    }

    if(!context.Set<Game>().Any())
    {
        context.Set<Game>().AddRange(
            new Game { Id = 1, Name = "Stardew Valley", GenreId = 3, ReleaseDate = new DateOnly(2016, 2, 26), Price = 14.99m },
            new Game { Id = 2, Name = "Hades", GenreId = 1, ReleaseDate = new DateOnly(2020, 9, 17), Price = 24.99m },
            new Game { Id = 3, Name = "The Legend of Zelda: Breath of the Wild", GenreId = 2, ReleaseDate = new DateOnly(2017, 3, 3), Price = 59.99m },
            new Game { Id = 4, Name = "Civilization VI", GenreId = 4, ReleaseDate = new DateOnly(2016, 10, 21), Price = 59.99m }
        );
    }
    context.SaveChanges();
}));
    }
}