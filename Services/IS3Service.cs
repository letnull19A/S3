using W2B.S3.Models;

namespace W2B.S3.Services;

/// <summary>
/// Интерфейс сервиса для работы с S3-совместимым хранилищем
/// </summary>
public interface IS3Service
{
    // Бакеты
    /// <summary>
    /// Создает новый бакет
    /// </summary>
    /// <param name="name">Имя бакета</param>
    /// <param name="maxSize">Максимальный размер в байтах</param>
    /// <param name="ownerKey">API-ключ владельца</param>
    Task CreateBucketAsync(string name, long maxSize, string ownerKey);

    /// <summary>
    /// Удаляет бакет и все его содержимое
    /// </summary>
    /// <param name="name">Имя бакета</param>
    Task DeleteBucketAsync(string name);

    /// <summary>
    /// Получает информацию о бакете
    /// </summary>
    /// <param name="name">Имя бакета</param>
    Task<Bucket> GetBucketAsync(string name);

    /// <summary>
    /// Проверяет существование бакета
    /// </summary>
    Task<bool> BucketExistsAsync(string name);

    /// <summary>
    /// Получает список всех бакетов
    /// </summary>
    Task<IEnumerable<Bucket>> ListBucketsAsync();

    // Объекты
    /// <summary>
    /// Загружает объект в хранилище
    /// </summary>
    /// <param name="bucketName">Имя бакета</param>
    /// <param name="key">Ключ объекта</param>
    /// <param name="content">Поток с содержимым</param>
    /// <param name="contentType">MIME-тип содержимого</param>
    /// <param name="ownerKey">API-ключ владельца</param>
    Task UploadObjectAsync(string bucketName, string key, Stream content, string contentType, string ownerKey);

    /// <summary>
    /// Получает объект из хранилища
    /// </summary>
    /// <param name="bucketName">Имя бакета</param>
    /// <param name="key">Ключ объекта</param>
    Task<Stream> GetObjectAsync(string bucketName, string key);

    /// <summary>
    /// Получает метаданные объекта
    /// </summary>
    Task<S3Object> GetObjectMetadataAsync(string bucketName, string key);

    /// <summary>
    /// Удаляет объект из хранилища
    /// </summary>
    Task DeleteObjectAsync(string bucketName, string key);

    /// <summary>
    /// Проверяет существование объекта
    /// </summary>
    Task<bool> ObjectExistsAsync(string bucketName, string key);

    /// <summary>
    /// Получает список объектов в бакете
    /// </summary>
    Task<IEnumerable<S3Object>> ListObjectsAsync(string bucketName);

    /// <summary>
    /// Получает размер занятого пространства в бакете
    /// </summary>
    Task<long> GetBucketUsageAsync(string bucketName);
}