using System.Reflection;
using EliteSoftTask.Data.Database.Entities;
using EliteSoftTask.Data.Database.Entities.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EliteSoftTask.Data.Database;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}