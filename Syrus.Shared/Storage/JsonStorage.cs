using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Syrus.Shared.Storage
{
    public class JsonStorage<T> : IStorage<T>
    {
        public string Path { get; private set; }

        public JsonStorage(string path) => Path = path;

        public void Save(IEnumerable<T> items)
        {
            string json = JsonConvert.SerializeObject(items, Formatting.Indented);
            File.WriteAllText(Path, json);
        }

        public void Save(T item)
        {
            string json = JsonConvert.SerializeObject(item, Formatting.Indented);
            File.WriteAllText(Path, json);
        }

        public void Delete() => File.Delete(Path);

        public IEnumerable<T> GetAll() => JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(Path));

        public T Get() => JsonConvert.DeserializeObject<T>(File.ReadAllText(Path));
    }
}
