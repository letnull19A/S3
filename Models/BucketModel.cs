using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W2B.S3.Models;

[Table("buckets")]
public sealed class BucketModel : BaseModel
{
    [Required]
    [Column("max_size_bytes")] 
    public long MaxSizeBytes { get; set; } = -1;

    [Required]
    [Column("current_size_bytes")] 
    public long CurrentSizeBytes { get; set; } = 0;

    [Required] 
    [Column("system_path")] 
    public string SystemPath { get; set; } = string.Empty;

    public ICollection<S3ObjectModel> Objects { get; set; } = new List<S3ObjectModel>();
}