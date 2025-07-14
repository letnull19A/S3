using Microsoft.EntityFrameworkCore;
using W2B.S3.Contexts;
using W2B.S3.Models;

namespace W2B.S3.Services;

public class ApiKeyService(S3DbContext db, ILogger<ApiKeyService> logger) : IApiKeyService
{
    private readonly object _lock = new();

    public Task<ApiKeyModel> CreateKeyAsync(string owner, string permissions, DateTime? expiresAt = null)
    {
        if (string.IsNullOrWhiteSpace(owner))
            throw new ArgumentException("Owner cannot be empty", nameof(owner));

        var key = new ApiKeyModel
        {
            Owner = owner.Trim(),
            Permissions = NormalizePermissions(permissions),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt,
            IsActive = true
        };

        lock (_lock)
        {
            db.ApiKeys.Add(key);
            db.SaveChanges();
        }

        logger.LogInformation("Created new API key for {Owner} (Key: {KeyLast4})",
            owner, key.Id.ToString()[^4..]);

        return Task.FromResult(key);
    }

    public async Task<bool> IsValidKeyAsync(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            return false;

        return await db.ApiKeys
            .AnyAsync(k => k.Id == apiKey &&
                           k.IsActive &&
                           (k.ExpiresAt == null || k.ExpiresAt > DateTime.UtcNow));
    }

    public async Task<ApiKeyModel> GetKeyAsync(string key)
    {
        var apiKey = await db.ApiKeys.FirstOrDefaultAsync(k => k.Id == key);
        return apiKey ?? throw new KeyNotFoundException($"API key {key[..4]}... not found");
    }

    public async Task<IEnumerable<ApiKeyModel>> GetActiveKeysAsync()
    {
        return await db.ApiKeys
            .Where(k => k.IsActive)
            .OrderBy(k => k.Owner)
            .ToListAsync();
    }

    public async Task RevokeKeyAsync(string key)
    {
        var apiKey = await GetKeyAsync(key);

        lock (_lock)
        {
            apiKey.IsActive = false;
            apiKey.RevokedAt = DateTime.UtcNow;
            db.SaveChanges();
        }

        logger.LogInformation("Revoked API key {KeyLast4} (Owner: {Owner})",
            key[^4..], apiKey.Owner);
    }

    public async Task<bool> HasPermissionAsync(string apiKey, string permission)
    {
        if (!await IsValidKeyAsync(apiKey))
            return false;

        var key = await GetKeyAsync(apiKey);
        var permissions = key.Permissions.Split(',');
        return permissions.Contains(permission.Trim(), StringComparer.OrdinalIgnoreCase);
    }

    private static string GenerateSecureKey()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("+", "")
            .Replace("/", "")
            .Replace("=", "")
            .Trim();
    }

    private static string NormalizePermissions(string permissions)
    {
        if (string.IsNullOrWhiteSpace(permissions))
            return "read";

        var normalized = permissions.Split(',')
            .Select(p => p.Trim().ToLower())
            .Where(p => !string.IsNullOrEmpty(p))
            .Distinct();

        return string.Join(',', normalized);
    }

    public async Task RotateKeyAsync(string oldKey)
    {
        var oldApiKey = await GetKeyAsync(oldKey);
        var newKey = await CreateKeyAsync(
            oldApiKey.Owner,
            oldApiKey.Permissions,
            oldApiKey.ExpiresAt);

        await RevokeKeyAsync(oldKey);

        logger.LogInformation("Rotated key for {Owner} (Old: {OldKeyLast4}, New: {NewKeyLast4})",
            oldApiKey.Owner, oldKey[^4..], newKey.Key[^4..]);
    }
}