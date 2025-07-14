using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W2B.S3.Models;

[Table("api_keys")]
public sealed class ApiKeyModel : BaseModel
{
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

    public ICollection<S3ObjectModel> CreatedObjects { get; set; } = new List<S3ObjectModel>();

    public ICollection<BucketModel> CreatedBuckets { get; set; } = new List<BucketModel>();
}