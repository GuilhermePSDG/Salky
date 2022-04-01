using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Core.Models
{
    [AddINotifyPropertyChangedInterface]
    public class BaseEntityVM : INotifyPropertyChanged
    {
        public string GuiId { get; set; } = Guid.NewGuid().ToString().Replace("-", "");
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    
    }
}
