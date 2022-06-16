using Salky.App.Services.Models;

namespace Salky.App.Services
{
    public class ImageService
    {
        /// <summary>
        /// <para>Base64 Required Format Example : data:image/png;base64,BASE64.. </para>
        /// Save the image in default folder
        /// </summary>
        /// <param name="Base64"></param>
        /// <returns><see cref="string"/></returns>
        public string SaveBase64Image(string Base64)
        {
            string fileName = this.GenerateRandomFileName();
            var extension = ExtractInfo(ref Base64);
            var physicalPath = Path.Combine(ImageServiceConfig.FullPath, fileName + $".{extension}");
            var buff = Convert.FromBase64String(Base64);
            File.WriteAllBytes(physicalPath, buff);
            var relativePath = Path.Combine(ImageServiceConfig.FolderName, fileName + $".{extension}");
            return relativePath.Replace(@"\","/");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns><see langword="true"/> if removed otherwise <see langword="false"/></returns>
        public bool TryDeleteImage(string relativePath)
        {
            try
            {
                var fullPath = Path.Combine(ImageServiceConfig.CurrentDirectory, relativePath);
                File.Delete(fullPath);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// <para>  <see langword="set"/> the <paramref name="Base64"/> as pure Base64 String</para>
        /// <para>Parameter Format Example : data:image/png;base64,BASE64 </para>
        /// </summary>
        /// <param name="Base64"></param>
        /// <returns>The Image Extension, EX: jpg,png</returns>
        private string ExtractInfo(ref string Base64)
        {
            var splited = Base64.Split(',');
            Base64 = splited[1];
            var indexBarra = splited[0].IndexOf("/");
            var indexPontoVir = splited[0].IndexOf(";");
            var fileExtension = splited[0].Substring(indexBarra + 1, indexPontoVir - indexBarra - 1);
            return fileExtension;
        }
        private string GenerateRandomFileName()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

    }
}
