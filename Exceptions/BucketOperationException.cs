using System;
using System.Runtime.Serialization;

namespace W2B.S3.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при ошибках операций с бакетами
    /// </summary>
    [Serializable]
    public class BucketOperationException : Exception
    {
        public string BucketName { get; }
        public OperationType Operation { get; }

        public BucketOperationException(string message)
            : base(message)
        {
        }

        public BucketOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BucketOperationException(string bucketName, OperationType operation, string message)
            : base($"Bucket operation failed: {operation} on '{bucketName}'. {message}")
        {
            BucketName = bucketName;
            Operation = operation;
        }

        public BucketOperationException(string bucketName, OperationType operation, string message,
            Exception innerException)
            : base($"Bucket operation failed: {operation} on '{bucketName}'. {message}", innerException)
        {
            BucketName = bucketName;
            Operation = operation;
        }

        protected BucketOperationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            BucketName = info.GetString(nameof(BucketName));
            Operation = (OperationType)info.GetValue(nameof(Operation), typeof(OperationType));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(BucketName), BucketName);
            info.AddValue(nameof(Operation), Operation);
        }

        /// <summary>
        /// Тип операции, вызвавшей исключение
        /// </summary>
        public enum OperationType
        {
            Create,
            Delete,
            Update,
            List,
            Configure,
            PermissionCheck
        }
    }
}