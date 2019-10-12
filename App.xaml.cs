// アプリ起動時の処理を記述する(Shell.xaml)
// Shellの作成とShellにUserControlを張り付ける
// Shellに張り付けるUserControlを登録する

using PrismCameraSample.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using Prism.Regions;

namespace PrismCameraSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Window CreateShell() => Container.Resolve<MainWindow>();
        /// <summary>
        /// Initialize
        /// ContentRegionという名前の空間から第二引数のUserControlにジャンプ
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Container.Resolve<IRegionManager>().RequestNavigate("ContentRegion", nameof(SelectScreen));
        }

        protected override void ConfigureServiceLocator()
        {
            base.ConfigureServiceLocator();
        }

        /// <summary>
        /// コンテナにViewを登録する。
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow>(nameof(MainWindow));
            containerRegistry.RegisterForNavigation<CameraScreenView>(nameof(CameraScreenView));
            containerRegistry.RegisterForNavigation<SelectScreen>(nameof(SelectScreen));
        }


    }
}
