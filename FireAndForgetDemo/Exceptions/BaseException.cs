using System;
using System.Runtime.Serialization;

namespace FireAndForgetDemo.Exceptions
{
    [Serializable]
    public class BaseException : ApplicationException
    {
        protected BaseException()
        {
        }

        protected BaseException(string message) : base(message)
        {
        }

        protected BaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}