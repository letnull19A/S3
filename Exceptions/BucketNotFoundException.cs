using System;
using System.Runtime.Serialization;

namespace W2B.S3.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested bucket is not found
    /// </summary>
    [Serializable]
    public class BucketNotFoundException : Exception
    {
        public string BucketName { get; }
        
        /// <summary>
        /// Initializes a new instance of the BucketNotFoundException class
        /// </summary>
        public BucketNotFoundException(string bucketName)
        {
            BucketName = bucketName;
        }

        /// <summary>
        /// Initializes a new instance of the BucketNotFoundException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="bucketName"></param>
        public BucketNotFoundException(string message, string bucketName) 
            : base(message)
        {
            BucketName = bucketName;
        }

        /// <summary>
        /// Initializes a new instance of the BucketNotFoundException class with a specified error message 
        /// and a reference to the inner exception that is the cause of this exception
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="innerException">The inner exception</param>
        /// <param name="bucketName"></param>
        public BucketNotFoundException(string message, Exception innerException, string bucketName) 
            : base(message, innerException)
        {
            BucketName = bucketName;
        }

        /// <summary>
        /// Initializes a new instance of the BucketNotFoundException class with serialized data
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data</param>
        /// <param name="context">The StreamingContext that contains contextual information</param>
        protected BucketNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            BucketName = info.GetString(nameof(BucketName));
        }

        /// <summary>
        /// Sets the SerializationInfo with information about the exception
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data</param>
        /// <param name="context">The StreamingContext that contains contextual information</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(BucketName), BucketName);
        }

        /// <summary>
        /// Creates a BucketNotFoundException for cases where the physical bucket directory is missing
        /// </summary>
        public static BucketNotFoundException ForMissingDirectory(string bucketName, string directoryPath)
        {
            return new BucketNotFoundException(bucketName)
            {
                HResult = unchecked((int)0x80070003),
                Source = "W2B.S3.PhysicalStorage"
            };
        }
    }
}