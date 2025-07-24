using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using W2B.S3.Contexts;
using W2B.S3.Models;

namespace W2B.S3.Controllers;

[Route("api/bucket/{bucketId:guid}/object")]
[ApiController]
public sealed class S3ObjectController(
    S3DbContext context,
    IConfiguration configuration,
    ILogger<S3ObjectController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Upload(Guid bucketId, [FromForm] List<IFormFile>? files)
    {
        var bucket = context.Buckets?.FirstOrDefault(i => i.Id == bucketId);

        if (bucket == null)
            return BadRequest("bucket not found");

        if (files == null || files.Count == 0)
            return BadRequest("No files were uploaded.");

        var uploadDirectory = configuration.GetValue<string>("Storage");

        if (string.IsNullOrEmpty(uploadDirectory))
            throw new NullReferenceException(nameof(uploadDirectory));

        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), uploadDirectory);

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var uploadedFiles = new List<S3ObjectModel>();

        foreach (var file in files)
        {
            if (file.Length <= 0) continue;

            var currentFileName = Guid.NewGuid() + "__" + file.FileName;
            var filePath = Path.Combine(uploadDirectory, currentFileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            uploadedFiles.Add(new S3ObjectModel()
            {
                Id = Guid.NewGuid(),
                BucketId = bucketId,
                FileName = currentFileName
            });
        }

        context.Objects?.AddRangeAsync(uploadedFiles);
        await context.SaveChangesAsync();

        return Ok(new { files = uploadedFiles });
    }

    [HttpGet("{objectId:guid}")]
    public async Task<IActionResult> GetFile(Guid objectId)
    {
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), configuration.GetValue<string>("Storage"));

        var result = await context.Objects!.FirstOrDefaultAsync(p => p.Id == objectId);

        if (result == null)
            return NotFound("file not found");

        logger.LogDebug(result.FileName);

        var image = System.IO.File.OpenRead(Path.Combine(uploadPath, result.FileName));

        return File(image, "image/jpeg");
    }
}