using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoint

{   const string GetGenreEndpointName = "GetGenreById";

    public static void MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres").WithTags("Genres");
        
        // GET /genres
        group.MapGet("/", async (GameStoreContext DbContext) => await DbContext.Genres.Select(genre => new GenreDto(genre.Id, genre.Name)).AsNoTracking().ToListAsync());
        // GET /genres/1
        group.MapGet("/{id}", async (int id,GameStoreContext DbContext) => {
            var genre = await DbContext.Genres.FindAsync(id);
            return genre is not null ? Results.Ok(new GenreDto(genre.Id, genre.Name)) : Results.NotFound();
        }).WithName(GetGenreEndpointName);
        // POST /genres
        group.MapPost("/", async (GenreDto newGenre,GameStoreContext DbContext) => {
            var genre = new Genre(){
                Name=newGenre.Name
                };
            DbContext.Genres.Add(genre);
            await DbContext.SaveChangesAsync().ContinueWith(task => {
                if (task.IsFaulted)         
                {   // Handle the exception here
                    Console.WriteLine($"Error saving changes: {task.Exception?.Message}");
                }
                //continue with the rest of the code after saving changes logic
            });
            GenreDto genreDto=new GenreDto(genre.Id,genre.Name);
            return Results.CreatedAtRoute(GetGenreEndpointName, new { id = genreDto.Id }, genreDto);
        });
        // DELETE /genres/1
        group.MapDelete("/{id}", async (int id,GameStoreContext DbContext) => {
            var genre = await DbContext.Genres.FindAsync(id);
            if (genre is null)  {
                return Results.NotFound();
            }
            await DbContext.Genres.Where(g => g.Id == id).ExecuteDeleteAsync();
            await DbContext.SaveChangesAsync();
            return Results.NoContent();
        });
        //PUT /genres/1
        group.MapPut("/{id}", async (int id, GenreDto updatedGenre, GameStoreContext DbContext) => {
            var genre = await DbContext.Genres.FindAsync(id);
            if (genre is null)
            {
                return Results.NotFound();
            }
            genre.Name = updatedGenre.Name;
            await DbContext.SaveChangesAsync();
            return Results.Ok(new GenreDto(genre.Id, genre.Name));
        }); 
    }
}