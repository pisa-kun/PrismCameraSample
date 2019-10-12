using System.Windows;
using PrismCameraSample.ViewModels;

namespace PrismCameraSample.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 画面生成時
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(); // VとVMを紐づけ
        }
    }
}
