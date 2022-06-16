namespace Salky.App.Services.Models
{
    public static class ImageServiceConfig
    {
        public const string FolderName = "images/a";
        public static string CurrentDirectory => Directory.GetCurrentDirectory();
        public static string FullPath => Path.Combine(CurrentDirectory, FolderName);

    }
}
