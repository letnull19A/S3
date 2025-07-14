using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W2B.S3.Models;

[Table("s3_objects")]
public class S3Object
{
    [Key]
    [Column("bucket_name")]
    [StringLength(63)]
    public string BucketName { get; set; } = null!;

    [Key]
    [Column("key")]
    public string Key { get; set; } = null!;

    [Column("size_bytes")]
    public long SizeBytes { get; set; }

    [Column("content_type")]
    public string ContentType { get; set; } = "application/octet-stream";

    [Column("storage_path")]
    public string StoragePath { get; set; } = null!;

    [Required]
    [Column("owner_key")]
    [StringLength(64)]
    public string OwnerKey { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("last_modified_at")]
    public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;

    [Column("etag")]
    [StringLength(64)]
    public string ETag { get; set; } = null!;

    [Column("metadata_json")]
    public string? MetadataJson { get; set; }

    [Column("is_public")]
    public bool IsPublic { get; set; } = false;

    [Column("version_id")]
    public string? VersionId { get; set; }

    // Навигационное свойство к бакету
    [ForeignKey("BucketName")]
    public Bucket Bucket { get; set; } = null!;

    // Навигационное свойство к владельцу
    [ForeignKey("OwnerKey")]
    public ApiKey Owner { get; set; } = null!;

    /// <summary>
    /// Получает метаданные объекта в виде словаря
    /// </summary>
    public Dictionary<string, string> GetMetadata()
    {
        return string.IsNullOrEmpty(MetadataJson)
            ? new Dictionary<string, string>()
            : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(MetadataJson)!;
    }

    /// <summary>
    /// Устанавливает метаданные объекта
    /// </summary>
    public void SetMetadata(Dictionary<string, string> metadata)
    {
        MetadataJson = System.Text.Json.JsonSerializer.Serialize(metadata);
        LastModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Генерирует ETag для объекта
    /// </summary>
    public void GenerateETag()
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        var hash = md5.ComputeHash(File.ReadAllBytes(StoragePath));
        ETag = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }

    /// <summary>
    /// Проверяет, является ли объект изображением
    /// </summary>
    public bool IsImage()
    {
        return ContentType.StartsWith("image/");
    }
}