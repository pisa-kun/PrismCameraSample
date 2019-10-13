/// Webカメラを表示するViewModel
/// Modelのカメラ映像をViewModelのプロパティで保持して
/// View側に通知を投げる

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PrismCameraSample.Views;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using PrismCameraSample.Models;
using System.Threading.Tasks;

namespace PrismCameraSample.ViewModels
{
    public class CameraScreenViewModel : BindableBase, IConfirmNavigationRequest, IJournalAware, IRegionMemberLifetime
    {
        /// <summary>
        /// Modelのインスタンスを格納
        /// </summary>
        Camera camera;

        bool isTask = true;

        /// <summary>
        /// 画面に表示するbitmap
        /// </summary>
        private WriteableBitmap bmp;

        public bool KeepAlive => false;

        private IRegionNavigationService RegionNavigationService { get; set; }

        /// <summary>
        /// 次画面に行くコマンド
        /// </summary>
        public DelegateCommand NextCommand { get; }

        public DelegateCommand StartCaptureCommand { get; }

        public DelegateCommand StopCaptureCommand { get; }

        /// <summary>
        /// Bindingするプロパティ
        /// </summary>
        public WriteableBitmap Bmp
        {
            get { return this.bmp; }
            set { SetProperty(ref this.bmp, value); }
        }

        /// <summary>
        /// コンストラクタでコマンドを初期化してセット
        /// </summary>
        public CameraScreenViewModel()
        {
            NextCommand = new DelegateCommand(() => NextButtonAction());
            StartCaptureCommand = new DelegateCommand(() => StartCapture());
            StopCaptureCommand = new DelegateCommand(() => StopCapture());
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
        
        /// <summary>
        /// カメラを起動する
        /// </summary>
        private async void StartCapture()
        {
            this.camera = new Camera();
            await ShowImage();
        }
      
        /// <summary>
        /// カメラを止める
        /// </summary>
        private void StopCapture() => this.isTask = false;

        /// <summary>
        /// ModelsのカメラからBitmapを取得する
        /// </summary>
        /// <returns></returns>
        private async Task ShowImage()
        {
            while(isTask)
            {
                try
                {
                    Bmp = this.camera.Capture(); // プロパティにカメラの映像をセット
                    if (Bmp == null) break;
                }
                catch
                {
                    MessageBox.Show("カメラが起動できませんでした");
                }
                await Task.Delay(30); //30フレームごとに送るように設定
            }
        }

        /// <summary>
        /// 次へのボタンを押したときの処理
        /// </summary>
        private void NextButtonAction()
        {
            var param = new NavigationParameters();
            param.Add("TargetData", true); // パラメータをkeyとvalueの組み合わせで追加

            StopCapture(); // カメラの映像を止める
          
            // 第二引数にパラメータを渡すと、viewが切り替わった先でパラメータを受け取る
            RegionNavigationService.RequestNavigate(nameof(SelectScreen), param);
        }
    }
}
