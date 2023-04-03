using System.ComponentModel;

namespace HomeApp.Frontend.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}