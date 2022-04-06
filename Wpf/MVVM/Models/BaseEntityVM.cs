using PropertyChanged;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WebSocket.Shared.DataAcess.Models
{
    [AddINotifyPropertyChangedInterface]
    public class BaseEntityVM : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
