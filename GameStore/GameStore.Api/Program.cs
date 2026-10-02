using GameStore.Api.Endpoints;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/", () => "Welcome to the Game Store API!");
app.MapGamesEndpoints();
app.Run();
