using System.Collections.Generic;

namespace Syrus.Shared.Storage
{
    public interface IStorage<T>
    {

        /// <summary>
        /// Uloží kolekci prvků. Při uložení dojde k přepsání původních dat.
        /// </summary>
        void Save(IEnumerable<T> items);

        /// <summary>
        /// Uložení objektu. Při uložení dojde k přepsání původních dat.
        /// </summary>
        void Save(T item);
        void Delete();
        IEnumerable<T> GetAll();
        T Get();
    }
}
