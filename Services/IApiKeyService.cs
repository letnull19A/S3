using W2B.S3.Models;

namespace W2B.S3.Services;

/// <summary>
/// Сервис для управления API-ключами доступа
/// </summary>
public interface IApiKeyService
{
    /// <summary>
    /// Создает новый API-ключ
    /// </summary>
    /// <param name="owner">Владелец ключа</param>
    /// <param name="permissions">Права доступа (через запятую)</param>
    /// <param name="expiresAt">Дата истечения срока действия</param>
    /// <returns>Созданный API-ключ</returns>
    Task<ApiKeyModel> CreateKeyAsync(string owner, string permissions, DateTime? expiresAt = null);

    /// <summary>
    /// Проверяет валидность API-ключа
    /// </summary>
    /// <param name="apiKey">Ключ для проверки</param>
    /// <returns>True если ключ действителен</returns>
    Task<bool> IsValidKeyAsync(string apiKey);

    /// <summary>
    /// Получает информацию о ключе
    /// </summary>
    /// <param name="key">Значение ключа</param>
    /// <returns>Объект ApiKey</returns>
    /// <exception cref="KeyNotFoundException">Если ключ не найден</exception>
    Task<ApiKeyModel> GetKeyAsync(string key);

    /// <summary>
    /// Получает список всех активных ключей
    /// </summary>
    Task<IEnumerable<ApiKeyModel>> GetActiveKeysAsync();

    /// <summary>
    /// Отзывает (деактивирует) API-ключ
    /// </summary>
    /// <param name="key">Ключ для отзыва</param>
    Task RevokeKeyAsync(string key);

    /// <summary>
    /// Проверяет наличие конкретного разрешения у ключа
    /// </summary>
    /// <param name="apiKey">API-ключ</param>
    /// <param name="permission">Требуемое разрешение</param>
    Task<bool> HasPermissionAsync(string apiKey, string permission);
}