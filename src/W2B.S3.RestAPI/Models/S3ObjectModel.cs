using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W2B.S3.Models;

[Table("objects")]
public sealed class S3ObjectModel : BaseModel
{
    [Required]
    [Column("bucket_id")]
    public Guid BucketId { get; set; }
    
    [Required]
    [Column("file_name")]
    public string FileName { get; set; } = string.Empty;
}