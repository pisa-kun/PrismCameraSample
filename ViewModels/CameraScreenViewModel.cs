/// Webカメラを表示するViewModel
/// Modelのカメラ映像をViewModelのプロパティで保持して
/// View側に通知を投げる

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismCameraSample.Views;
using System;
using System.Windows;

namespace PrismCameraSample.ViewModels
{
    public class CameraScreenViewModel : BindableBase, IConfirmNavigationRequest, IJournalAware, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        private IRegionNavigationService RegionNavigationService { get; set; }

        /// <summary>
        /// 次画面に行くコマンド
        /// </summary>
        public DelegateCommand NextCommand { get; }

        /// <summary>
        /// コンストラクタでコマンドを初期化してセット
        /// </summary>
        public CameraScreenViewModel()
        {
            NextCommand = new DelegateCommand(() =>
            {
                var param = new NavigationParameters();
                param.Add("TargetData", Visibility.Visible); // パラメータをkeyとvalueの組み合わせで追加

                // 第二引数にパラメータを渡すと、viewが切り替わった先でパラメータを受け取る
                RegionNavigationService.RequestNavigate(nameof(SelectScreen), param);
            });
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback) => continuationCallback(true);

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// 画面遷移された時の処理
        /// 自分の画面をShellに登録？
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            RegionNavigationService = navigationContext.NavigationService;
        }

        public bool PersistInHistory() => true;
    }
}
