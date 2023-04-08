using System.ComponentModel;

namespace HomeApp.Frontend.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public abstract class BaseModelViewModel<T> : BaseViewModel where T : class
    {

        public abstract void FromModel(T model);
    }
}