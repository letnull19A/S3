using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using W2B.S3.Contexts;
using W2B.S3.Exceptions;
using W2B.S3.Interfaces;
using W2B.S3.Models;

namespace W2B.S3.Services;

public class S3Service : IS3Service
{
    private readonly S3DbContext _db;
    private readonly IWebHostEnvironment _env;
    private readonly ImageProcessor _imageProcessor;
    private readonly ILogger<S3Service> _logger;
    private readonly object _bucketLock = new();

    public S3Service(
        S3DbContext dbContext,
        IWebHostEnvironment env,
        ImageProcessor imageProcessor,
        ILogger<S3Service> logger)
    {
        _db = dbContext;
        _env = env;
        _imageProcessor = imageProcessor;
        _logger = logger;
        EnsureStorageDirectoryExists();
    }

    public async Task CreateBucketAsync(string name, long maxSize, string ownerKey)
    {
        ValidateBucketName(name);

        if (await _db.Buckets.AnyAsync(b => b.Name == name))
            throw new BucketOperationException($"Bucket '{name}' already exists");

        var bucket = new BucketModel
        {
            Name = name,
            MaxSizeBytes = maxSize,
            CurrentSizeBytes = 0,
            OwnerKey = ownerKey,
            CreatedAt = DateTime.UtcNow
        };

        await _db.Buckets.AddAsync(bucket);
        await _db.SaveChangesAsync();

        Directory.CreateDirectory(GetBucketPath(name));
        _logger.LogInformation("Bucket created: {BucketName} (Owner: {OwnerKey})", name, ownerKey);
    }

    public async Task DeleteBucketAsync(string name)
    {
        var bucket = await GetBucketAsync(name);
        var objects = await _db.Objects
            .Where(o => o.BucketName == name)
            .ToListAsync();

        // Delete physical files
        foreach (var obj in objects)
        {
            try
            {
                if (File.Exists(obj.StoragePath))
                    File.Delete(obj.StoragePath);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to delete object file: {Path}", obj.StoragePath);
            }
        }

        // Delete database records
        _db.Objects.RemoveRange(objects);
        _db.Buckets.Remove(bucket);
        await _db.SaveChangesAsync();

        // Delete bucket directory
        try
        {
            Directory.Delete(GetBucketPath(name), true);
        }
        catch (DirectoryNotFoundException)
        {
            _logger.LogWarning("Bucket directory not found during deletion: {BucketName}", name);
        }

        _logger.LogInformation("Bucket deleted: {BucketName}", name);
    }

    public async Task UploadObjectAsync(string bucketName, string key, Stream content, string contentType,
        string ownerKey)
    {
        var bucket = await GetBucketAsync(bucketName);
        var size = content.Length;

        lock (_bucketLock)
        {
            if (bucket.CurrentSizeBytes + size > bucket.MaxSizeBytes)
                throw new BucketOperationException($"Bucket '{bucketName}' size limit exceeded");

            bucket.CurrentSizeBytes += size;
            _db.SaveChanges();
        }

        var objectPath = Path.Combine(GetBucketPath(bucketName), key);
        Directory.CreateDirectory(Path.GetDirectoryName(objectPath)!);

        await using (var fileStream = File.Create(objectPath))
        {
            await content.CopyToAsync(fileStream);
        }

        var s3Object = new S3ObjectModel
        {
            Key = key,
            BucketName = bucketName,
            SizeBytes = size,
            ContentType = contentType,
            OwnerKey = ownerKey,
            StoragePath = objectPath,
            LastModifiedAt = DateTime.UtcNow
        };

        await _db.Objects.AddAsync(s3Object);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Object uploaded: {BucketName}/{Key} ({Size} bytes)",
            bucketName, key, size);
    }

    public async Task<Stream> GetObjectAsync(string bucketName, string key)
    {
        var obj = await GetObjectMetadataAsync(bucketName, key);

        if (!File.Exists(obj.StoragePath))
            throw new ObjectNotFoundException($"Physical file not found at {obj.StoragePath}");

        return File.OpenRead(obj.StoragePath);
    }

    public async Task<S3ObjectModel> GetObjectMetadataAsync(string bucketName, string key)
    {
        var obj = await _db.Objects
            .FirstOrDefaultAsync(o => o.BucketName == bucketName && o.Key == key);

        return obj ?? throw new ObjectNotFoundException($"Object '{key}' not found in bucket '{bucketName}'");
    }

    public async Task DeleteObjectAsync(string bucketName, string key)
    {
        var obj = await GetObjectMetadataAsync(bucketName, key);
        var bucket = await GetBucketAsync(bucketName);

        lock (_bucketLock)
        {
            bucket.CurrentSizeBytes -= obj.SizeBytes;
            _db.Objects.Remove(obj);
            _db.SaveChanges();
        }

        try
        {
            File.Delete(obj.StoragePath);
        }
        catch (FileNotFoundException)
        {
            _logger.LogWarning("File not found during deletion: {Path}", obj.StoragePath);
        }

        _logger.LogInformation("Object deleted: {BucketName}/{Key}", bucketName, key);
    }

    public async Task<IEnumerable<S3ObjectModel>> ListObjectsAsync(string bucketName)
    {
        await ValidateBucketExistsAsync(bucketName);
        return await _db.Objects
            .Where(o => o.BucketName == bucketName)
            .OrderBy(o => o.Key)
            .ToListAsync();
    }

    public async Task<Stream> GetProcessedImageAsync(
        string bucketName,
        string key,
        int quality = 75,
        bool progressive = true,
        int? width = null,
        int? height = null)
    {
        var obj = await GetObjectMetadataAsync(bucketName, key);

        if (!obj.ContentType.StartsWith("image/"))
            throw new ImageProcessingException($"Object {key} is not an image");

        return await _imageProcessor.ProcessAsync(
            obj.StoragePath,
            quality,
            progressive,
            width,
            height);
    }

    private string GetBucketPath(string bucketName) =>
        Path.Combine(_env.ContentRootPath, "storage", bucketName);

    private void EnsureStorageDirectoryExists()
    {
        var storagePath = Path.Combine(_env.ContentRootPath, "storage");
        if (!Directory.Exists(storagePath))
        {
            Directory.CreateDirectory(storagePath);
            _logger.LogInformation("Created storage directory at {Path}", storagePath);
        }
    }

    private async Task ValidateBucketExistsAsync(string bucketName)
    {
        if (!await _db.Buckets.AnyAsync(b => b.Name == bucketName))
            throw new BucketNotFoundException($"Bucket '{bucketName}' not found");
    }

    private static void ValidateBucketName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BucketOperationException("Bucket name cannot be empty");

        if (name.Length < 3 || name.Length > 63)
            throw new BucketOperationException("Bucket name must be between 3 and 63 characters");

        if (name.Any(c => !char.IsLetterOrDigit(c) && c != '-' && c != '.'))
            throw new BucketOperationException("Bucket name contains invalid characters");
    }

    public Task<bool> BucketExistsAsync(string name) =>
        _db.Buckets.AnyAsync(b => b.Name == name);

    public Task<bool> ObjectExistsAsync(string bucketName, string key) =>
        _db.Objects.AnyAsync(o => o.BucketName == bucketName && o.Key == key);

    public Task<IEnumerable<BucketModel>> ListBucketsAsync() =>
        Task.FromResult<IEnumerable<BucketModel>>(_db.Buckets.OrderBy(b => b.Name).ToList());

    public async Task<long> GetBucketUsageAsync(string bucketName)
    {
        var sum = await _db.Objects
            .Where(o => o.BucketName == bucketName)
            .SumAsync(o => (long?)o.SizeBytes);
    
        return sum.GetValueOrDefault();
    }

    public async Task<BucketModel> GetBucketAsync(string name) =>
        await _db.Buckets.FirstOrDefaultAsync(b => b.Name == name)
        ?? throw new BucketNotFoundException($"Bucket '{name}' not found");
}