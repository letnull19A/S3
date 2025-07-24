using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W2B.S3.Models;

public class BaseModel
{
    [Key]
    [Column("id")]
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();
}