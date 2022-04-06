using System;
using Wpf;
using Wpf.MVVM.ViewModels;

namespace WebSocket.Shared.DataAcess.Models
{
    public class MessageVM : BaseEntityVM
    {
        public MessageVM()
        {
            Date = DateTime.Now;
        }
        public bool IsOwner { get; set; }
        public bool IsSended { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public bool IsSequencialMessage { get; set; } = false;


        public string picture
        {
            get 
            {
                foreach(var window in App.Current.Windows)
                {
                    if (window.GetType().Equals(typeof(MainWindow)))
                    {
                        var dataContext = (MainViewModel)((MainWindow)window).DataContext;
                        return IsOwner ? dataContext.LoggedUserVM.PictureSource : dataContext.SelectedContact?.PictureSource ?? "";
                    }
                }
                return "";
            }
        }

    }



}
