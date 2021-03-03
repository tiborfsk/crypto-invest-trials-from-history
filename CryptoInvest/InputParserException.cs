using System;
using System.Runtime.Serialization;

namespace CryptoInvest
{
    class InputParserException : Exception
    {
        public InputParserException()
        {
        }

        public InputParserException(string message) : base(message)
        {
        }

        public InputParserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InputParserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
