using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>options.SupportNonNullableReferenceTypes());
builder.Services.AddValidation();
var connString = "Data Source=GameStore.db";
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

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Welcome to the Game Store API!");
app.MapGamesEndpoints();
app.MigrateDb();
app.Run();
