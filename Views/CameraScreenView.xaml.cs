using PrismCameraSample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrismCameraSample.Views
{
    /// <summary>
    /// CameraScreenView.xaml の相互作用ロジック
    /// </summary>
    public partial class CameraScreenView : UserControl
    {
        public CameraScreenView()
        {
            InitializeComponent();
            this.DataContext = new CameraScreenViewModel();
        }
    }
}
