using Salky.App.Storage;
using Salky.WebSockets.Exensions;

namespace Salky.API
{
    public class WebSocketRouteBaseCustom : WebSocketRouteBase
    {
        public static IStorage RootStorage;
        public static IStorageFactory StorageFactory;
        public IStorage UserStorage
        {
            get
            {
                return RootStorage.GetOrCreate<IStorage>(this.User.UserId, () => StorageFactory.CreateNew());
            }
        }
        public override Task OnDisconnectAsync()
        {
            RootStorage.Remove(this.User.UserId);
            return base.OnDisconnectAsync();
        }
    }
}
