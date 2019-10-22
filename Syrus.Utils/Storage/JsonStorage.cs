namespace Syrus.Utils.Storage
{
    public class JsonStorage<T> : IStorage
    {
        public string Path { get; set; }

        public JsonStorage() { }

        public JsonStorage(string path) => Path = path;

        public T Load()
        {

        }

        public void Delete()
        {
        }

        public void Save()
        {
        }

        private void Deserialize(string data)
        {

        }
    }
}
