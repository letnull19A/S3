using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W2B.S3.Models;

[Table("api_keys")]
public class ApiKey
{
    [Key]
    [Column("key")]
    [StringLength(64)]
    public string Key { get; set; } = null!;

    [Required]
    [Column("owner")]
    [StringLength(255)]
    public string Owner { get; set; } = null!;

    [Column("permissions")]
    [StringLength(255)]
    public string Permissions { get; set; } = "read,write";

    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("expires_at")] public DateTime? ExpiresAt { get; set; }

    [Column("is_active")] public bool IsActive { get; set; } = true;

    [Column("revoked_at")] public DateTime? RevokedAt { get; set; }

    [Column("last_used_at")] public DateTime? LastUsedAt { get; set; }

    [Column("description")]
    [StringLength(500)]
    public string? Description { get; set; }

    public ICollection<S3Object> CreatedObjects { get; set; } = new List<S3Object>();

    public ICollection<Bucket> CreatedBuckets { get; set; } = new List<Bucket>();

    public bool HasPermission(string permission)
        => Permissions.Split(',')
            .Select(p => p.Trim().ToLower())
            .Contains(permission.Trim().ToLower());

    public bool IsValid()
        => IsActive && (ExpiresAt == null || ExpiresAt > DateTime.UtcNow);

    public void MarkAsUsed()
    {
        LastUsedAt = DateTime.UtcNow;
    }
}