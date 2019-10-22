using System;
using System.Collections.Generic;
using System.Text;

namespace Syrus.Utils.Storage
{
    public interface IStorage
    {
        void Save();
        void Delete();
    }
}
