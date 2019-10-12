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

![pic1](https://github.com/pisa-kun/PrismCameraSample\Image\Readme\1.png)

2. ManageExtensionsの①[VisualStudioMarketplace]を選択し、②検索バーに"prism"を入力、③[Prism Template Pack]を選択してインストール

![pic2](https://github.com/pisa-kun/PrismCameraSample\Image\Readme\2.png)

3. ウィザードが立ち上がるのでインストールを実行

4. VisualStudioを再起動させて新規のプロジェクトを作成

5. 検索バーでprismを調べ、Prism Blank App(WPF)を選択してプロジェクトを作成する

![pic3](https://github.com/pisa-kun/PrismCameraSample\Image\Readme\3.png)

プロジェクト起動時はxamlなどにエラーがでますがPrismをNugetから落とせていないためです。

ビルド時にNugetからインポートしてくれるので、まずはデバッグ実行をしてみましょう。

#### アプリケーションの全容

- prismで画面遷移の実現
- 画面間でパラメータ受け渡し
- MVVM構築
- カメラ撮影

## prismで画面遷移の実現

## 画面間でパラメータ受け渡し

## MVVM構築

## カメラ撮影

## Demo

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