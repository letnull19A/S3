using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W2B.S3.Models;

[Table("s3_objects")]
public sealed class S3ObjectModel : BaseModel
{

    [Required]
    [Column("bucket_id")]
    public Guid BucketId { get; set; }

    [Required]
    [Column("type")] 
    public string Type { get; set; } = "type/text";

    [Required]
    [Column("size_bytes")] 
    public long SizeBytes { get; set; } = 1;

    [Required]
    [Column("uploaded_at")] 
    public DateTime UploadedAt { get; set; } = new DateTime().Date;

    [Required]
    [Column("bin_file_name")]
    public string BinFileName { get; set; } = string.Empty;

    [Required]
    [Column("is_copy")]
    public bool IsCopy { get; set; } = false;

    [Required]
    [Column("origin_file_name")]
    public string OriginFileName { get; set; } = string.Empty;
        
    [Column("origin_object_id")]
    public Guid? OriginObjectId { get; set; }

    [ForeignKey("bucket_id")]
    public BucketModel Bucket { get; set; } = null!;
}