using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using W2B.S3.Exceptions;

namespace W2B.S3.Services;

public class ImageProcessor
{
    private readonly string _cacheDirectory;
    private readonly ILogger<ImageProcessor> _logger;
    private readonly object _cacheLock = new();

    public ImageProcessor(IWebHostEnvironment env, ILogger<ImageProcessor> logger)
    {
        _cacheDirectory = Path.Combine(env.ContentRootPath, "image-cache");
        Directory.CreateDirectory(_cacheDirectory);
        _logger = logger;
    }

    public async Task<Stream> ProcessAsync(
        string imagePath,
        int quality = 75,
        bool progressive = true,
        int? width = null,
        int? height = null)
    {
        if (!File.Exists(imagePath))
            throw new FileNotFoundException("Source image not found", imagePath);

        ValidateParameters(quality, width, height);

        var cacheKey = GenerateCacheKey(imagePath, quality, progressive, width, height);
        var cachedImage = TryGetCachedImage(cacheKey);
        if (cachedImage != null)
        {
            _logger.LogDebug("Serving from cache: {CacheKey}", cacheKey);
            return cachedImage;
        }

        using var image = await Image.LoadAsync(imagePath);
        var outputStream = new MemoryStream();

        // Apply transformations
        if (width.HasValue || height.HasValue)
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(width ?? 0, height ?? 0),
                Mode = ResizeMode.Max
            }));
        }

        var encoder = new JpegEncoder
        {
            Quality = quality,
            Interleaved = progressive
        };

        await image.SaveAsync(outputStream, encoder);
        outputStream.Position = 0;

        CacheImage(cacheKey, outputStream);
        
        _logger.LogInformation("Processed image {Image} (Q:{Quality}, P:{Progressive}, Size:{Width}x{Height})", 
            Path.GetFileName(imagePath), quality, progressive, width, height);

        return outputStream;
    }

    private void ValidateParameters(int quality, int? width, int? height)
    {
        if (quality < 1 || quality > 100)
            throw new ImageProcessingException("Quality must be between 1 and 100");

        if (width < 0 || height < 0)
            throw new ImageProcessingException("Dimensions cannot be negative");
    }

    private string GenerateCacheKey(string imagePath, int quality, bool progressive, int? width, int? height)
    {
        var lastModified = File.GetLastWriteTimeUtc(imagePath).Ticks;
        var sizeKey = $"{width ?? 0}x{height ?? 0}";
        return $"{Path.GetFileNameWithoutExtension(imagePath)}-{lastModified}-q{quality}-{(progressive ? "p" : "np")}-{sizeKey}.jpg";
    }

    private Stream? TryGetCachedImage(string cacheKey)
    {
        var cachePath = Path.Combine(_cacheDirectory, cacheKey);

        lock (_cacheLock)
        {
            if (File.Exists(cachePath))
            {
                var memoryStream = new MemoryStream();
                using (var fileStream = File.OpenRead(cachePath))
                {
                    fileStream.CopyTo(memoryStream);
                }
                memoryStream.Position = 0;
                return memoryStream;
            }
        }

        return null;
    }

    private void CacheImage(string cacheKey, Stream imageStream)
    {
        var cachePath = Path.Combine(_cacheDirectory, cacheKey);

        lock (_cacheLock)
        {
            using var fileStream = File.Create(cachePath);
            imageStream.Position = 0;
            imageStream.CopyTo(fileStream);
        }
    }

    public async Task ClearCacheAsync()
    {
        await Task.Run(() =>
        {
            lock (_cacheLock)
            {
                foreach (var file in Directory.GetFiles(_cacheDirectory))
                {
                    File.Delete(file);
                }
            }
        });
        _logger.LogInformation("Image cache cleared");
    }
}