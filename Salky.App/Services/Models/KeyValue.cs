namespace Salky.App.Services
{
    public struct KeyValue
    {
        public KeyValue(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
        public string key { get; set; }
        public string value { get; set; }
        public bool IsNotNullOrEmpty()
        {
            return !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value);
        }
    }
}
