using System;
using System.Collections.Generic;
using System.Text;

namespace Syrus.Shared.Storage
{
    public interface IStorage
    {
        void Save();
        void Delete();
    }
}
