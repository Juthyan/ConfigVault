using ConfigVault.Domain;
using Microsoft.EntityFrameworkCore;


namespace ConfigVault.Infrastructure.Persistence;


public class ConfigVaultDbContext : DbContext
{
    public DbSet<ConfigVersion> ConfigVersions => Set<ConfigVersion>();


    public ConfigVaultDbContext(DbContextOptions<ConfigVaultDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConfigVersion>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Status)
                  .HasConversion<string>()
                  .IsRequired();
        });
    }
}