using PrismCameraSample.ViewModels;
using System.Windows.Controls;

namespace PrismCameraSample.Views
{
    /// <summary>
    /// SelectScreen.xaml の相互作用ロジック
    /// </summary>
    public partial class SelectScreen : UserControl
    {
        public SelectScreen()
        {
            InitializeComponent();
            this.DataContext = new SelectScreenViewModel();
        }
    }
}
