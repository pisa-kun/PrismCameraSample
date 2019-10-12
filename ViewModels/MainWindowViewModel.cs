using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismCameraSample.Views;

namespace PrismCameraSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}
