using System;
using System.Runtime.Serialization;

namespace FireAndForgetDemo.Exceptions
{
    [Serializable]
    public class TaskException : ApplicationException
    {
        protected TaskException()
        {
        }

        protected TaskException(string message) : base(message)
        {
        }

        protected TaskException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TaskException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static TaskException TaskHasFailed(string taskName)
        {
            throw new TaskException($"Task '{taskName}' has failed");
        }
    }
}