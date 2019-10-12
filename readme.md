Prismのインストールとカメラアプリ作成過程
====

- Overview
   - prismとは？
   - prismインストール
   - アプリケーションの全容
   - MVVM

## Description
### 

#### prsimとは？

> Prism とは Windows クライアントアプリケーション開発で使える代表的な MVVM フレームワークの１つ

#### prismインストール

1. VisualStudio2019から適当なプロジェクトを開いて、画面上部の[拡張機能]-[拡張機能の管理]を選択

![pic1](https://github.com/pisa-kun/PrismCameraSample/blob/gh-pages/Image/Readme/1.png)

2. ManageExtensionsの①[VisualStudioMarketplace]を選択し、②検索バーに"prism"を入力、③[Prism Template Pack]を選択してインストール

![pic2](https://github.com/pisa-kun/PrismCameraSample/blob/gh-pages/Image/Readme/2.png)

3. ウィザードが立ち上がるのでインストールを実行

4. VisualStudioを再起動させて新規のプロジェクトを作成

5. 検索バーでprismを調べ、Prism Blank App(WPF)を選択してプロジェクトを作成する

![pic3](https://github.com/pisa-kun/PrismCameraSample/blob/gh-pages/Image/Readme/3.png)


プロジェクト起動時はxamlなどにエラーがでますがPrismをNugetから落とせていないためです。

ビルド時にNugetからインポートしてくれるので、まずはデバッグ実行をしてみましょう。

#### アプリケーションの全容

- prismで画面遷移の実現
- 画面間でパラメータ受け渡し
- MVVM構築
- カメラ撮影

## prismで画面遷移の実現

デフォルトで作られるMainWindow.xamlにはContentControlがあります。
アプリ起動時にこの画面が表示され、ContentRegionに他のUserControlを入れ替えることで画面遷移を実現します。

```c#
// MainWindow.xaml
<Window x:Class="PrismCameraSample.Views.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:prism="http://prismlibrary.com/"
        ResizeMode="NoResize"
        Title="{Binding Title}" Height="350" Width="525">
    <Grid>
        <!--xamlを入れ替えて表示する空間-->
        <ContentControl prism:RegionManager.RegionName="ContentRegion" />
    </Grid>
</Window>
```

これまたデフォルトで作られているApp.xaml.csには
- CreateShell
が最初から記述されています。

```c#
// App.xaml.cs
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
```

Oninitialized()メソッドの中でRequestNavigateメソッドを呼んでいます。
第一引数でRegionName、第二引数で第一引数の場所にUserControl(.xaml)を張り付けます。

RegisterTypes()メソッドでは、画面遷移させたいUserControlを登録させます。
今回はCameraScreenViewとSelectScreenのUserControlを画面遷移させたいので、RegisterForNavigationメソッドで登録します。

これでアプリが起動するときに、MainWindowの空間にSelectScreen.xamlが表示される形になります。

次にボタンを押したときの画面遷移についてです。
SelectScreen.xamlは次のようにします。

```C#
<UserControl x:Class="PrismCameraSample.Views.SelectScreen"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
             xmlns:local="clr-namespace:PrismCameraSample.Views"
             mc:Ignorable="d" 
             Background="NavajoWhite"
             d:DesignHeight="450" d:DesignWidth="800">
    <Grid>
        <StackPanel>
            <Button Content="Next" Command="{Binding NextCommand}" />
        </StackPanel>
    </Grid>
</UserControl>

```

ViewModelは以下のように

```C#
public class SelectScreenViewModel : BindableBase, IConfirmNavigationRequest, IJournalAware, IRegionMemberLifetime
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
        public SelectScreenViewModel()
        {
            NextCommand = new DelegateCommand(() => RegionNavigationService.RequestNavigate(nameof(CameraScreenView)));
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback) => continuationCallback(true);

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        /// <param name="navigationContext"></param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            RegionNavigationService = navigationContext.NavigationService;
        }

        public bool PersistInHistory() => true;
    }
```

ボタンを押したときの処理(NextCommand)には、コンストラクタでRegionNavigationService.RequestNavigate()を呼び、遷移したい画面(xaml)を渡してあげます。

これで
1. ShellにUserControlを張り付け
2. ボタンを押したときに画面を遷移させる

の2つができました。

## 画面間でパラメータ受け渡し

今回CameraScreenからSelectScreenに遷移したときに、SelectScreenの一部領域をHiddenからVisibleに変更したかったので、
1. CameraScreenViewModelの遷移ボタンを押下したときに、NavigationParameters.AddでkeyとValueを登録
2. RequestNavigate()の第二引数に1の値を指定
3. SelectScreenViewModelのOnNavigatedToでパラメータを受け取り、プロパティに反映させる

```C#
// CameraScreenViewModel.cs

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
```

```C#
// SelectScreenViewModel
    public class SelectScreenViewModel : BindableBase, IConfirmNavigationRequest, IJournalAware, IRegionMemberLifetime
    {
        public bool KeepAlive => false;

        private IRegionNavigationService RegionNavigationService { get; set; }

        /// <summary>
        /// バインディング用の変数
        /// 最初は表示しないのでHidden
        /// </summary>
        private Visibility viewboxSampleVisibility = Visibility.Hidden;

        /// <summary>
        /// SetPropertyで変数が変化したときにviewに通知
        /// </summary>
        public Visibility ViewboxSampleVisibility
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
                ViewboxSampleVisibility = (Visibility)navigationContext.Parameters["TargetData"];
            }
            RegionNavigationService = navigationContext.NavigationService;
        }

        public bool PersistInHistory() => true;
    }
```

## MVVM構築

Prismはプロジェクト作成時にViewsとViewModelを作ってくれます。
自分はViewのコードビハインドでViewModelとバインディングさせるほうが好きです。
```c#
public MainWindow()
{
   Initialize();
   this.DateContect = new MainWindowViewModel(); // ViewModelをバインド
}
```
あとはModelは随時作成して、
ViewModelのインスタンス生成時に必要なModelのインスタンスをフィールド変数に格納して、
Commandに処理を記述する流れです。

## カメラ撮影

OpenCvSharpをNugetからインポートしましょう。
[WPF + OpenCVSharp + Camera](http://kowaimononantenai.blogspot.com/2017/02/wpf-prismopencvsharpweb.html)

上記のコードをほぼ流用でカメラを起動できました。
画面遷移させるときにカメラをきちんと止めるかインスタンスを破棄しないと例外がおきるみたいです。

## Demo

- アプリ起動時
無事MainWindow.xamlのResionにSelectScreen.xamlを張り付け

![pic4](https://github.com/pisa-kun/PrismCameraSample/blob/gh-pages/Image/Readme/4.png)

- Nextボタンを押下して、撮影画面に遷移
ボタンが増えてるのがわかります
![pic5](https://github.com/pisa-kun/PrismCameraSample/blob/gh-pages/Image/Readme/5.png)

- Startを押してカメラを起動
ViewboxのImageにきちんとBitmapをバインディングできてます
![pic6](https://github.com/pisa-kun/PrismCameraSample/blob/gh-pages/Image/Readme/6png.png)

- Nextボタンを押下して、SelectScreenに遷移
きちんとパラメータをうけとり、非表示にしていた空間をVisiblityに変更できています
![pic7](https://github.com/pisa-kun/PrismCameraSample/blob/gh-pages/Image/Readme/7.png)


## Requirement

## Usage

## Support Site
[MVVM フレームワーク Prism の全体概観](https://qiita.com/toydev/items/cf1bb4b519e7e2453d46)

- Qiitaでprismの全体像について触れられています。

[WPF Prism episode: 3 ～ Re: ゼロから始める Prism 生活 ～](https://elf-mission.net/programming/wpf/episode03/)

- インストール周りを丁寧に解説されています。タスクバーの解説を4からされています。

[Prism-Sample](https://github.com/PrismLibrary/Prism-Samples-Wpf)

- 本家のサンプルのようです。

[ResionNavigationService](https://qiita.com/kwhrkzk/items/719354e86a15ccac7296)

- 仕事ではRequestNavigatorかなにかを使っているので、今回はこちらで。

[画面遷移時のパラメータ受け取り](https://elf-mission.net/programming/wpf/episode07/)

- 画面遷移するときのパラメータ受け取り

[WPF + OpenCVSharp + Camera](http://kowaimononantenai.blogspot.com/2017/02/wpf-prismopencvsharpweb.html)

## Author
pisa