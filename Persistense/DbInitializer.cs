using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Persistense
{
    public class DbInitializer
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DbInitializer> _logger;
        public DbInitializer(AppDbContext context, ILogger<DbInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }
         public async Task InitializeAsync()
        {
            try
            {
                if ((await _context.Database.GetAppliedMigrationsAsync()).Any())
                {
                    _logger.LogInformation("Applying pending migrations...");
                    await _context.Database.MigrateAsync();
                    _logger.LogInformation("Database migrations applied successfully.");
                }
                else
                {
                    _logger.LogInformation("No pending migrations found. Database is up to date.");
                }
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }
    }
}
