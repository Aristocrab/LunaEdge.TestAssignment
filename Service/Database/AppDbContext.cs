using Domain.Entities;
using Domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;

namespace Service.Database;

public class AppDbContext : DbContext
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<TodoTask> TodoTasks { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var baseEntities = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in baseEntities)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}