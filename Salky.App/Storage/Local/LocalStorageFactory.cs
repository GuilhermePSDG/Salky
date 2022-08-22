using Salky.App.Storage;

namespace Salky.WebSockets.Implementations
{
    public class LocalStorageFactory : IStorageFactory
    {
        public IStorage CreateNew()
        {
            return new LocalStorage();
        }
    }
}
