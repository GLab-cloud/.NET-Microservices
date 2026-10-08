using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>options.SupportNonNullableReferenceTypes());
builder.Services.AddValidation();
builder.AddGameStoreDb();
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Welcome to the Game Store API!");
app.MapGamesEndpoints();
app.MigrateDb();
app.Run();
