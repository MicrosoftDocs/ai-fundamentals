# Microsoft Learn AI Fundamentals Labs

このリポジトリのサンプルコードは、Microsoft Learn モジュールのハンズオン演習で使用します。


## セットアップ

演習問題は、Visual Studio Online で完了するように設計されています。ラボを完了するには、次のものが必要です。

- Microsoft Azure のサブスクリプション。まだお持ちでない場合は、<a href ='https://azure.microsoft.com' target='_blank'>https://azure.microsoft.com</a>で無料トライアルにサインアップすることができます。
- このレポをベースにしたVisual Studio Online環境。これは Visual Studio Code のホストされたインスタンスを提供し、ラボの演習用のノートブックを実行することができます。

Visual Studio Onlineの環境を設定するには:

1. 新しいブラウザタブで[https://online.visualstudio.com/environments/new](https://online.visualstudio.com/environments/new?name=ai-fundamentals&repo=MicrosoftDocs/ai-fundamentals)を開き、プロンプトが表示されたら、Azureサブスクリプションに関連付けられたMicrosoftアカウントを使用してサインインします。
2. Visual Studio Online の課金プランをまだお持ちでない場合は、プランを作成します。次に、次の設定で環境を作成します。:
    - **Environment Name**: *A name for your environment - for example, **ai-fundamentals**.*
    - **Git Repository**: MicrosoftDocs/ai-fundamentals
    - **Instance Type**: Standard (Linux) 4 cores, 8GB RAM
    - **Suspend idle environment after**: 30 minutes
3. 環境が作成されるのを待ちます。これには1分程度の時間がかかります。
4. 環境がセットアップされるまで、もう1分ほど待ってください。何も起きていないように見えるかもしれませんが、バックグラウンドでは演習に必要なコンポーネントをインストールしています。以下のようなことが起こるのがわかるでしょう。
    - 左側のペインにノートブック(.ipynb)ファイルのリストが表示されます。
    - 数分後、**z REFRESH NOW!**という名前のファイルがファイルリストの一番下に表示されます。これはすべてがインストールされたことを示しています。

5. **z REFRESH NOW!**ファイルが表示された後、***Webページをリフレッシュ***して、必要な拡張機能がすべて読み込まれていることを確認し、配色を明るい背景に設定します。これで開始の準備ができました。
6. Webページをリフレッシュした後、**z REFRESH NOW**ファイルを削除することができます。左下の **&#9881;** アイコンをクリックして、新しい**カラーテーマ**を選択するだけで、お好みに合わせて配色を変更することができます。

## Contributing

現時点では、このリポジトリへの投稿は受け付けていません。演習で問題が発生した場合は、[report it](https://docs.microsoft.com/learn/support/troubleshooting#report-feedback)をお願いします。
