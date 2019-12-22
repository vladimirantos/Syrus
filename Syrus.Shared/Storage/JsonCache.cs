using System.IO;

namespace Syrus.Shared.Storage
{
    public class JsonCache<T> : JsonStorage<T>
    {
        public JsonCache() : base()
        {
            Path = System.IO.Path.Combine(Constants.CacheDirectory, typeof(T).Name.ToLower() + ".json");
            if (!File.Exists(Path))
                File.Create(Path);
        }
    }
}
