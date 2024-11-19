using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Record> Records { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
