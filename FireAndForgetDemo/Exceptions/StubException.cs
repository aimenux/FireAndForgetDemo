using System;
using System.Runtime.Serialization;

namespace FireAndForgetDemo.Exceptions
{
    [Serializable]
    public class StubException : BaseException
    {
        protected StubException()
        {
        }

        protected StubException(string message) : base(message)
        {
        }

        protected StubException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StubException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static StubException UnfoundDocuments()
        {
            throw new StubException("Unfound documents");
        }
    }
}