# Microsoft Learn AI Fundamentals Labs

このリポジトリのサンプルコードは、Microsoft Learn [AI Fundamentals](https://docs.microsoft.com/learn/certifications/azure-ai-fundamentals)資格に関連する各ラーニングパスのハンズオン演習で使用します。


## セットアップ

演習問題は、Azure Machine Learning ワークスペースのNotebook で完了するように設計されています。ラボを完了するには、次のものが必要です。

- Microsoft Azure のサブスクリプション。まだお持ちでない場合は、<a href ='https://azure.microsoft.com' target='_blank'>https://azure.microsoft.com</a>で無料トライアルにサインアップすることができます。
- このレポをベースにした環境Azure Machine Learning ワークスペース Notebook。この環境の中で、直接にラボの演習用のノートブックを実行することができます。

Azure Machine Learning ワークスペース の環境を設定するには：

1. Azure Machine Learning ワークスペースの作成
ご自分の Azure サブスクリプションに既に Azure Machine Learning ワークスペースがある場合は、新しいブラウザー タブで Azure Machine Learning Studio  に移動し、Microsoft アカウントを使用して Azure Machine Learning Studio にサインインします。 それ以外の場合は、次の手順に従って新しいワークスペースを作成します。
ご利用の Azure サブスクリプションに関連付けられている Microsoft アカウントを使用して、Azure portal  にサインインします。
[+ リソースの作成] を選択し、Machine Learning を検索し、次の設定を使用して新しい Machine Learning リソースを作成します。
[ワークスペース名]: 任意の一意の名前を入力します
[サブスクリプション]: お使いの Azure サブスクリプション
[リソース グループ]: 一意の名前で新しいリソース グループを作成します
[場所]: 利用可能な場所を選択します
ワークスペース リソースが作成されるまで待ちます (数分かかる場合があります)。 次に、ポータルでそれに移動し、ワークスペースの [概要] ページで Azure Machine Learning Studio を起動 (または新しいブラウザー タブを開いて https://ml.azure.com  に移動) し、ご自分の Microsoft アカウントを使用して Azure Machine Learning Studio にサインインします。
Azure Machine Learning Studio で、左上にある [☰] アイコンを切り替えると、インターフェイス内にさまざまなページが表示されます。 これらのページを使用して、ワークスペース内のリソースを管理できます。

2. コンピューティング インスタンスを作成する
この演習で使用しているノートブックを実行するには、ご自分の Azure Machine Learning ワークスペースにコンピューティング インスタンスが必要です。 既にある場合は、それを使用して次のステップに進んでください。ない場合は、次の手順に従って作成します。
Azure Machine Learning Studio  で、[コンピューティング] ページ ([管理] の下) を表示します。
[Compute Instances](コンピューティング インスタンス) タブで、次の設定を使用して新しいコンピューティング インスタンスを作成します。
[コンピューティング名]: "一意の名前を入力します"
[仮想マシンのタイプ]:CPU
[仮想マシンのサイズ]: Standard_DS11_v2
コンピューティング インスタンスが開始するのを待ちます (これには 1 分ほどかかる場合があります)。
演習用ファイルをダウンロードする
このモジュール (およびその他の関連モジュール) で使用するファイルは、GitHub リポジトリに公開されており、このリポジトリをお使いの Python 環境にクローンする必要があります。 前のモジュールで ai-fundamentals リポジトリをまだクローンしていない場合は、次の手順を使用して、Azure Machine Learning ワークスペースにクローンします。

Azure Machine Learning Studio  で、[ノートブック] ページ ([作成者] の下) を表示します。 このページには、ノートブックの実行に使用できるノートブック エディターが含まれています。
[My files](マイ ファイル) の下で 🗋 ボタンを使用して、次の設定で新しいファイルを作成します。

[ファイルの場所]: Users/"自分のユーザー名"
[ファイル名]:Get-Files
[ファイルの種類]: ノートブック
[Overwrite if already exists](既に存在する場合は上書きする): オン
新しいノートブックが作成されたら、前に作成したコンピューティング インスタンスが [コンピューティング] ボックスで選択されていること、およびその状態が [実行中] であることを確認します。 次に、ノートブックに作成された四角形のセルに、次のコードを貼り付けます。

<a name="gitclone"></a>
```
!git clone https://github.com/rytokuga/ai-fundamentals
```

セルの横にある [▷] ボタンを使用して、それに含めたコードを実行します。 これにより、GitHub から演習ファイルがクローンされます。

コードの実行が終了し、ファイルのチェックアウトが完了したら、[My files](マイ ファイル) の下の ↻ ボタンを使用してフォルダー ビューを更新し、ai-fundamentals という名前のフォルダーが作成されていることを確認します。 このフォルダーには、演習で使用するノートブックとその他のファイルが含まれています。

Get-Files.ipynb ノートブック タブを閉じます。

3. 演習を完了する
Python 環境を設定し、ai-fundamentals リポジトリをクローンしたら、Computer Vision を使用した画像分析を調べる準備が整います。

ai-fundamentals フォルダーにある Image Analysis with Computer Vision.ipynb ノートブックを開きます。 Azure Machine Learning Studio でノートブック エディターを使用している場合は、[≪] ボタンを使用してエクスプローラー ペインを折りたたむとスペースが広がるので、ノートブック タブに集中できます。
ノートブックの情報を確認し、それに含まれているコード セルを順番に実行します。
クリーンアップ
Azure Machine Learning Studio でコンピューティング インスタンスを使用した場合は、Azure クレジットを不必要に使用することがないように、それを停止する必要があります。

Azure Machine Learning Studio で、[コンピューティング] ページ ([管理] の下) を表示します。
[Compute Instances](コンピューティング インスタンス) タブで、ご自分のコンピューティング インスタンスを選択してから、[停止] ボタンを使用してそれを停止します。

環境が作成されるのを待ちます。これには1分程度の時間がかかります。
環境がセットアップされるまで、もう1分ほど待ってください。何も起きていないように見えるかもしれませんが、バックグラウンドでは演習に必要なコンポーネントをインストールしています。以下のようなことが起こるのがわかるでしょう。
    - 左側のペインにノートブック(.ipynb)ファイルのリストが表示されます。
    - 数分後、**REFRESH NOW!**という名前のファイルがファイルリストの一番下に表示されます。これはすべてがインストールされたことを示しています。

**REFRESH NOW!**ファイルが表示された後、***Webページをリフレッシュ***して、必要な拡張機能がすべて読み込まれていることを確認し、配色を明るい背景に設定します。これで開始の準備ができました。


## MS Learnで演習セットアップを行う場合
MS Learnの各モジュールの[演習部分](https://docs.microsoft.com/ja-jp/learn/modules/analyze-images-computer-vision/3-analyze-images)には、同様のセットアップ手順があります。
ただし、使用すべきGithub repoが異なりますので、Git cloneを行う部分は**代わりに**[このコマンドを使用して現在のRepoをクローンしてください](#gitclone))。


## Contributing

現時点では、このリポジトリへの投稿は受け付けていません。演習で問題が発生した場合は、[report it](https://docs.microsoft.com/learn/support/troubleshooting#report-feedback)をお願いします。
