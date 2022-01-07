using System;

namespace Isu
{
    public class ID
    {
        private static int start = 0;
        public static int Generate()
        {
            return start++;
        }
    }
}