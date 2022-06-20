namespace Salky.App
{
    public static class AppConfiguration
    {
        public static string AplicationUrl_Https { get; }
        public static string AplicationUrl_Http { get; }
        static AppConfiguration()
        {
            var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(";") ?? throw new NullReferenceException();
            AplicationUrl_Https = urls[0];
            AplicationUrl_Http = urls[1];
            return;
        }
    }
}
