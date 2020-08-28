using PropertyChanged;
using System.ComponentModel;

namespace PunterHomeDomain
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
