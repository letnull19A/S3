using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;

namespace W2B.S3.Models;

public sealed class UserModel : BaseModel
{
    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;
}