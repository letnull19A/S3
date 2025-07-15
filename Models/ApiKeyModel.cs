using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W2B.S3.Models;

[Table("api_keys")]
public sealed class ApiKeyModel : BaseModel
{
    [Required] 
    [Column("owner_id")] 
    public Guid OwnerId { get; set; }

    [Required] 
    [Column("bucket_id")] 
    public Guid BucketId { get; set; }

    [Required] 
    [Column("status")] 
    public string Status { get; set; } = "active";

    [Column("permissions")] 
    public byte Permissions { get; set; }

    [Column("key")] 
    public string Key { get; set; } = string.Empty;
    
    public UserModel User { get; set; }
    public ICollection<BucketModel> Buckets { get; set; } = new List<BucketModel>();
}