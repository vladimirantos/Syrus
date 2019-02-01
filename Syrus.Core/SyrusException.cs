using System;

namespace Syrus.Core
{
    public class SyrusException : SystemException
    {
        public SyrusException(string message) : base(message)
        {

        }

        public SyrusException(string message, Exception innerException): base(message, innerException)
        {

        }
    }
}
