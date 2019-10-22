using System;
using System.Collections.Generic;
using System.Text;

namespace Syrus.Shared
{
    public class SyrusException : Exception
    {
        public SyrusException(): base()
        {

        }

        public SyrusException(string message): base(message)
        {

        }

        public SyrusException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
