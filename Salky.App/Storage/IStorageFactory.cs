namespace Salky.App.Storage
{
    public interface IStorageFactory
    {
        public IStorage CreateNew();
    }
}
