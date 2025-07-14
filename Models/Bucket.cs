using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W2B.S3.Models;

[Table("buckets")]
public class Bucket
{
    [Key]
    [Column("name")]
    [StringLength(63, MinimumLength = 3)]
    public string Name { get; set; } = null!;

    [Column("max_size_bytes")] public long MaxSizeBytes { get; set; }

    [Column("current_size_bytes")] public long CurrentSizeBytes { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [Column("owner_key")]
    [StringLength(64)]
    public string OwnerKey { get; set; } = null!;

    [Column("is_public")] public bool IsPublic { get; set; } = false;

    [Column("versioning_enabled")] public bool VersioningEnabled { get; set; } = false;

    [Column("last_modified_at")] public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;

    [Column("tags")] public string? TagsJson { get; set; }

    // Навигационное свойство для объектов в бакете
    public ICollection<S3Object> Objects { get; set; } = new List<S3Object>();

    // Навигационное свойство к API-ключу владельца
    [ForeignKey("OwnerKey")] public ApiKey Owner { get; set; } = null!;

    /// <summary>
    /// Проверяет, достаточно ли места для загрузки файла
    /// </summary>
    public bool CanStoreFile(long fileSize)
    {
        return CurrentSizeBytes + fileSize <= MaxSizeBytes;
    }

    /// <summary>
    /// Обновляет размер бакета после добавления файла
    /// </summary>
    public void AddFile(long fileSize)
    {
        CurrentSizeBytes += fileSize;
        LastModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Обновляет размер бакета после удаления файла
    /// </summary>
    public void RemoveFile(long fileSize)
    {
        CurrentSizeBytes -= fileSize;
        LastModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Получает теги бакета в виде словаря
    /// </summary>
    public Dictionary<string, string> GetTags()
    {
        return string.IsNullOrEmpty(TagsJson)
            ? new Dictionary<string, string>()
            : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(TagsJson)!;
    }

    /// <summary>
    /// Устанавливает теги бакета
    /// </summary>
    public void SetTags(Dictionary<string, string> tags)
    {
        TagsJson = System.Text.Json.JsonSerializer.Serialize(tags);
    }
}