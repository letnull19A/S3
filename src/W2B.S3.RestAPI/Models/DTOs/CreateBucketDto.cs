namespace W2B.S3.Models.DTOs;

public sealed class CreateBucketDto
{
    public string Name { get; set; } = string.Empty;
    public long MaxSizeBytes { get; set; }
}