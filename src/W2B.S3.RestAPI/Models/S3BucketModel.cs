using System.ComponentModel.DataAnnotations.Schema;

namespace W2B.S3.Models;

[Table("buckets")]
public sealed class S3BucketModel : BaseModel
{
    [Column("name")] 
    public string Name { get; set; } = string.Empty;

    [Column("max_volume_bytes")] 
    public long MaxVolumeBytes { get; set; } = 1024;

    [Column("status")]
    public string Status { get; set; } = "active";

    public List<S3ObjectModel> Objects { get; set; } = [];
}