using System;

namespace IsuExtra
{
    public class OgnpException : Exception
    {
        public OgnpException()
        {
        }

        public OgnpException(string message)
            : base(message)
        {
        }

        public OgnpException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}