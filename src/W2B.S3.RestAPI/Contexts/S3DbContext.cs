using Microsoft.EntityFrameworkCore;
using W2B.S3.Models;

namespace W2B.S3.Contexts;

public sealed class S3DbContext : DbContext
{
    public DbSet<S3ObjectModel>? Objects { get; set; }
    public DbSet<S3BucketModel>? Buckets { get; set; }

    public S3DbContext(DbContextOptions<S3DbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}