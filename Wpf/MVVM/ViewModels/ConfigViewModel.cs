using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Core.Models;
using Wpf.MVVM.Models;

namespace Wpf.MVVM.ViewModels
{
    
    public class ConfigViewModel : BaseEntityVM
    {
       
        public string UserName {    get; set; }
        public string KeyFullPath { get; set; }
        public Command UserNameChanged { get; set; }
        public Command GerarChave { get; set; }
        public ConfigViewModel()
        {
            GerarChave = new Command(NewKeys);
        }
        private void userNameChanged(object? discart)
        {
         
        }
        private void NewKeys(object? discart)
        {
           
        }

    }
}
