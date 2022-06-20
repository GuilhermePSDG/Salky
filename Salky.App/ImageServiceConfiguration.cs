namespace Salky.App
{
    public static class ImageServiceConfiguration
    {
        public const string FolderName = "images/a";
        public static string CurrentDirectory => Directory.GetCurrentDirectory();
        public static string FullPath => Path.Combine(CurrentDirectory, FolderName);
        private static string App_Url => AppConfiguration.AplicationUrl_Https;
        public static string CreateExternalLink(string relativePath)
        {
            if (relativePath.Contains("http")) return relativePath;
            return $"{App_Url}/{relativePath}";
        }
    }
}
