namespace Salky.App.Storage
{
    public interface IStorage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns><see langword="null"/> <see langword="if"/> not found or <see langword="if"/> the item is <see langword="null"/></returns>
        public object? Get(string key);
        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void Add(string key, object value);
        public void AddOrUpdate(string key, object value);
        public bool ContainsKey(string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if is contains and removed, false otherwise</returns>
        public bool Remove(string key);
        /// <summary>
        /// Clear and set to <see langword="null"/> everything
        /// </summary>
        public void Clear();
    }
}
