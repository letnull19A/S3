using Microsoft.AspNetCore.Mvc;
using W2B.S3.Interfaces;
using W2B.S3.Services;

namespace W2B.S3.Controllers;

[ApiController]
[Route("api/s3")]
public class S3Controller(IS3Service s3, ImageProcessor imageProcessor) : ControllerBase
{
    [HttpGet("buckets/{bucket}/objects/{key}")]
    public async Task<IActionResult> GetObject(
        string bucket,
        string key,
        [FromQuery] int? quality = null,
        [FromQuery] bool progressive = false)
    {
        var obj = await s3.GetObjectMetadataAsync(bucket, key);

        if (!obj.Type.StartsWith("image/"))
        {
            var stream = await s3.GetObjectAsync(bucket, key);
            return File(stream, obj.Type);
        }

        var processed = await imageProcessor.ProcessAsync(
            obj.StoragePath,
            quality ?? 75,
            progressive);

        Response.Headers.CacheControl = "public,max-age=31536000";
        return File(processed, "image/jpeg");
    }
}