/// 最初に表示される画面
/// 画面が移り変わったときにパラメータをチェックしてVisibilityに反映

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismCameraSample.Views;
using System;
using System.Windows;

namespace PrismCameraSample.ViewModels
{
    public class SelectScreenViewModel : BindableBase, IConfirmNavigationRequest, IJournalAware, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        private IRegionNavigationService RegionNavigationService { get; set; }

        /// <summary>
        /// バインディング用の変数
        /// 最初は表示しないのでfalse
        /// </summary>
        private bool viewboxSampleVisibility = false;

        /// <summary>
        /// SetPropertyで変数が変化したときにviewに通知
        /// </summary>
        public bool ViewboxSampleVisibility
        {
            get { return this.viewboxSampleVisibility; }
            set { SetProperty(ref this.viewboxSampleVisibility, value); }
        }

        /// <summary>
        /// 次画面に行くコマンド
        /// </summary>
        public DelegateCommand NextCommand { get; }

        /// <summary>
        /// コンストラクタでコマンドを初期化してセット
        /// </summary>
        public SelectScreenViewModel()
        {
            NextCommand = new DelegateCommand(() => RegionNavigationService.RequestNavigate(nameof(CameraScreenView)));
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback) => continuationCallback(true);

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// 画面遷移して表示するときの処理
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters["TargetData"] != null)
            {
                // Camera画面から受け取ったパラメータを照合してバインドさせる
                ViewboxSampleVisibility = (bool)navigationContext.Parameters["TargetData"];
            }
            RegionNavigationService = navigationContext.NavigationService;
        }

        public bool PersistInHistory() => true;
    }
}
