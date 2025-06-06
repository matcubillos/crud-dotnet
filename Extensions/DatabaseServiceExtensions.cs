using WebApplication1.Persistense;

namespace WebApplication1.Extensions
{
    public static class DatabaseServiceExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services)
        {
            services.AddScoped<DbInitializer>();
            return services;
        }
        public static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
            await dbInitializer.InitializeAsync();
            return app;
        }
    }
}
