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
        //DbContext configuration has a Scoped service lifetime because:
        // 1. it ensures that a new instance of the DbContext is created for each request, which is important for thread safety and to avoid potential issues with concurrent access to the database.
        // 2. DB connections are limited and typically expensive resources, and having a scoped lifetime allows for better resource management by disposing of the DbContext instance at the end of each request.
        // 3. Dbcontext are not thread-safe, and using a scoped lifetime ensures that each request gets its own instance of the DbContext, preventing potential issues with concurrent access to the same instance.
        // 4. Make it easier to manage transactions and unit of work patterns, as each request can have its own transaction scope without interfering with other requests and ensuure data consistency and integrity.
        // 5. Reusing a Db context instance can lead to increased memory usage and potential memory leaks, as the context may hold onto references to entities that are no longer needed. By using a scoped lifetime, the context is disposed of at the end of each request, freeing up memory and preventing potential memory leaks.
        builder.Services.AddScoped<GameStoreContext>(provider =>
        {
            var options = new DbContextOptionsBuilder<GameStoreContext>()
                .UseSqlite(connString)
                .Options;
            return new GameStoreContext(options);
        }); 
    }
}