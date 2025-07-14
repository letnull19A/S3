using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W2B.S3.Models;

[Table("buckets")]
public sealed class BucketModel : BaseModel
{
    [Column("name")]
    [StringLength(63, MinimumLength = 3)]
    public string Name { get; set; } = null!;

    [Column("max_size_bytes")] public long MaxSizeBytes { get; set; }

    [Column("current_size_bytes")] public long CurrentSizeBytes { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("is_public")] public bool IsPublic { get; set; } = false;

    [Column("versioning_enabled")] public bool VersioningEnabled { get; set; } = false;

    [Column("last_modified_at")] public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<S3ObjectModel> Objects { get; set; } = new List<S3ObjectModel>();

    [ForeignKey("OwnerKey")] public UserModel Owner { get; set; } = null!;
}