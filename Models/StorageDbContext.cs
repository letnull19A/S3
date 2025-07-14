using Microsoft.EntityFrameworkCore;
using W2B.S3.Models;

namespace W2B.S3.Models;

public class S3DbContext(
    DbContextOptions<S3DbContext> options
)
    : DbContext(options)
{
    public DbSet<ApiKey>? ApiKeys { get; set; }
    public DbSet<Bucket>? Buckets { get; set; }
    public DbSet<S3Object>? Objects { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bucket>(b =>
        {
            b.HasIndex(x => x.Name).IsUnique();
            b.Property(x => x.Name).HasMaxLength(63);
        });

        modelBuilder.Entity<S3Object>(o =>
        {
            o.HasKey(x => new { x.BucketName, x.Key });
            o.HasIndex(x => x.OwnerKey);
        });

        modelBuilder.Entity<ApiKey>(k =>
        {
            k.HasKey(x => x.Key);
            k.Property(x => x.Key).HasMaxLength(64);
        });
    }
}