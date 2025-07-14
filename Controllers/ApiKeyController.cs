using Microsoft.AspNetCore.Mvc;
using W2B.S3.Models;
using W2B.S3.Services;

namespace W2B.S3.Controllers;

[ApiController]
[Route("api/keys")]
public class ApiKeyController(
    IApiKeyService keyService,
    ILogger<ApiKeyController> logger)
    : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApiKeyModel>> CreateKey([FromBody] CreateKeyRequest request)
    {
        try
        {
            var key = await keyService.CreateKeyAsync(
                request.Owner,
                request.Permissions,
                request.ExpiresAt);

            logger.LogInformation("Created new API key for {Owner}", request.Owner);
            return Ok(key);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create API key");
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApiKeyModel>>> ListKeys()
    {
        var keys = await keyService.GetActiveKeysAsync();
        return Ok(keys);
    }
    
    [HttpDelete("{key}")]
    public async Task<IActionResult> RevokeKey(string key)
    {
        try
        {
            await keyService.RevokeKeyAsync(key);
            logger.LogInformation("Revoked API key: {Key}", key);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpGet("{key}")]
    public async Task<ActionResult<ApiKeyModel>> GetKey(string key)
    {
        try
        {
            var apiKey = await keyService.GetKeyAsync(key);
            return Ok(apiKey);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

public class CreateKeyRequest
{
    /// <example>user@example.com</example>
    public required string Owner { get; set; }

    /// <example>read,write,delete</example>
    public string Permissions { get; set; } = "read,write";

    /// <example>2025-12-31T00:00:00Z</example>
    public DateTime? ExpiresAt { get; set; }
}