using System;
using System.Runtime.Serialization;

namespace W2B.S3.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при попытке доступа к несуществующему объекту
    /// </summary>
    [Serializable]
    public class ObjectNotFoundException : Exception
    {
        public string BucketName { get; }
        public string ObjectKey { get; }
        public string FullPath => $"{BucketName}/{ObjectKey}";

        public ObjectNotFoundException()
        {
        }

        public ObjectNotFoundException(string message) 
            : base(message)
        {
        }

        public ObjectNotFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        public ObjectNotFoundException(string bucketName, string objectKey)
            : base($"Object '{objectKey}' not found in bucket '{bucketName}'")
        {
            BucketName = bucketName;
            ObjectKey = objectKey;
        }

        public ObjectNotFoundException(string bucketName, string objectKey, Exception innerException)
            : base($"Object '{objectKey}' not found in bucket '{bucketName}'", innerException)
        {
            BucketName = bucketName;
            ObjectKey = objectKey;
        }

        protected ObjectNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            BucketName = info.GetString(nameof(BucketName)) ?? string.Empty;
            ObjectKey = info.GetString(nameof(ObjectKey)) ?? string.Empty;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(BucketName), BucketName);
            info.AddValue(nameof(ObjectKey), ObjectKey);
        }

        /// <summary>
        /// Создает исключение для случая, когда физический файл не найден
        /// </summary>
        public static ObjectNotFoundException ForPhysicalFile(string filePath, string bucketName, string objectKey)
        {
            return new ObjectNotFoundException(bucketName, objectKey)
            {
                HResult = unchecked((int)0x80070002), // HRESULT_FILENOTFOUND
                Source = "W2B.S3.Storage"
            };
        }
    }
}