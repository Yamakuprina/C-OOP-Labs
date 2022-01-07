using System;

namespace Isu.Tools
{
    public class IsuException : Exception
    {
        public IsuException()
        {
            Console.WriteLine("Error happened!");
        }

        public IsuException(string message)
            : base(message)
        {
            Console.WriteLine(message);
        }

        public IsuException(string message, Exception innerException)
            : base(message, innerException)
        {
            Console.WriteLine(InnerException + message);
        }
    }
}