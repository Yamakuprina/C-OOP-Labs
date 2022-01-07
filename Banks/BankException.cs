using System;

namespace Banks
{
    public class BankException : Exception
    {
        public BankException()
        {
        }

        public BankException(string message)
            : base(message)
        {
        }

        public BankException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}