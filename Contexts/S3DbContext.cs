using Microsoft.EntityFrameworkCore;
using W2B.S3.Models;

namespace W2B.S3.Contexts;

public sealed class S3DbContext : DbContext
{
    public DbSet<ApiKeyModel>? ApiKeys { get; set; }
    public DbSet<BucketModel>? Buckets { get; set; }
    public DbSet<S3ObjectModel>? Objects { get; set; }

    public S3DbContext(DbContextOptions<S3DbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BucketModel>(b =>
        {
            b.HasIndex(x => x.Name).IsUnique();
            b.Property(x => x.Name).HasMaxLength(63);
        });

        modelBuilder.Entity<S3ObjectModel>(o =>
        {
            o.HasKey(x => new { x.BucketName, x.Key });
            o.HasIndex(x => x.OwnerKey);
        });

        modelBuilder.Entity<ApiKeyModel>(k =>
        {
            k.HasKey(x => x.Key);
            k.Property(x => x.Key).HasMaxLength(64);
        });
    }
}