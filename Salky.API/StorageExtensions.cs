using Salky.App.Storage;
using Salky.WebSockets.Implementations;

namespace Salky.API
{
    public static class StorageExtensions
    {
        public static void UseLocalRouteStorage(this IServiceCollection services)
        {
            var factory = new LocalStorageFactory();
            var root = factory.CreateNew();
            WebSocketRouteBaseCustom.RootStorage = root;
            WebSocketRouteBaseCustom.StorageFactory = factory;
        }
    }
}
