using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Wpf.MVVM.Models
{
    public class Config
    {
        #region public config propi
        public string lastuserguid { get; set; }
        public bool isautologin { get; set; }
        #endregion
        private string FolderFullPath { get; }
        private string FileName { get; }
        private string FileFullPath => Path.Combine(FolderFullPath, FileName);
        public Config()
        {
            this.FolderFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"Salky");
            this.FileName = "config.json";
            Directory.CreateDirectory(FolderFullPath);
        }
        
    
        
        public void Save()
        {
            var thisJson = JsonSerializer.SerializeToUtf8Bytes(this);
            File.WriteAllBytes(FileFullPath, thisJson);
        }
        public static Config Load()
        {
            var config = new Config();
            if (File.Exists(config.FileFullPath))
            {
                var bytes = File.ReadAllBytes(config.FileFullPath);
                config = JsonSerializer.Deserialize<Config>(bytes) ?? config;
            }
            else
            {
                config.Save();
            }
            return config;
        }

    }
}
