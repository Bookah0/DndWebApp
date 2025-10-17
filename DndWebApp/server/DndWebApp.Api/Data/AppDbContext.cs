using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // public DbSet<>  { get; set; }
}
