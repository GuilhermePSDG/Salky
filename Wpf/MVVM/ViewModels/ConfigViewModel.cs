using WebSocket.Shared.DataAcess.Models;
using Wpf.Core.Models;

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
