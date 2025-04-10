# インストールガイド (Rhinoceros 7版)

## 前提条件

- Rhinoceros 7以上
- .NET 6.0ランタイム以上
- Grasshopper（Rhinoに同梱）

## バージョンについて

このガイドはRhinoceros 7環境向けです。Rhinoceros 8を使用している場合は、`rhino8-net7`ブランチのガイドをご参照ください。

## サーバーのインストール

### オプション1: .NETツールとしてインストール

```bash
dotnet tool install --global GrasshopperMcp.Server
```

### オプション2: ソースからビルド

1. リポジトリのクローン
   ```bash
   git clone https://github.com/norioh-japan/grasshopper-mcp-server.git
   ```

2. サーバープロジェクトに移動
   ```bash
   cd grasshopper-mcp-server/src/GrasshopperMCP.Server
   ```

3. プロジェクトのビルド
   ```bash
   dotnet build -c Release
   ```

4. サーバーの実行
   ```bash
   dotnet run -c Release
   ```

## Grasshopperプラグインのインストール

1. GrasshopperMCP.Pluginプロジェクトをビルド
2. 生成された.ghaファイルをGrasshopperのコンポーネントフォルダにコピー
   - 通常は `%APPDATA%\\Grasshopper\\Libraries` にあります
3. 既に実行中の場合はRhinocerosを再起動

## 設定

`config.json`ファイルを以下の構造で作成します：

```json
{
  "GrasshopperMcp": {
    "Server": {
      "Host": "localhost",
      "Port": 5000,
      "EnableSecurity": true,
      "ApiKeys": ["your-secret-api-key"],
      "AllowedOrigins": ["https://your-llm-service.com"]
    },
    "Grasshopper": {
      "AutoStartRhino": false,
      "RhinoPath": "C:\\Program Files\\Rhino 7\\System\\Rhino.exe",
      "MaxDefinitions": 10,
      "MaxExecutionTimeSeconds": 300
    },
    "Logging": {
      "LogLevel": "Information",
      "LogPath": "logs/grasshopper-mcp.log"
    }
  }
}
```

## サーバーの起動

```bash
grasshopper-mcp-server --config config.json
```

Rhinoが既に起動している場合：

```bash
grasshopper-mcp-server --connect --config config.json
```

## 動作確認

1. Rhinocerosを開く
2. Grasshopperを開く
3. 「Extra」→「Utility」タブから「MCP Server」コンポーネントを探す
4. キャンバスに配置
5. Server URLパラメータを設定に合わせて設定
6. Enableパラメータをtrueに設定して接続を有効化
7. Status出力で接続状態を確認

## Rhinoceros 7版と8版の併用について

Rhinoceros 7とRhinoceros 8の両方を使用する環境で、適切なバージョンを選択するには：

1. **Rhinoceros 7用** (このバージョン): 
   - mainブランチのコードを使用し、.NET 6.0環境でビルド
   - Rhinoceros 7専用のインストールフォルダにインストール

2. **Rhinoceros 8用**:
   - rhino8-net7ブランチのコードを使用し、.NET 7環境でビルド
   - Rhinoceros 8専用のインストールフォルダにインストール

それぞれの環境に適したバージョンを使用することで、互換性の問題を回避できます。

## トラブルシューティング

### サーバーが起動しない場合
- ポートが既に使用されていないか確認
- .NET 6.0以上がインストールされているか確認
- ファイアウォールの設定を確認

### プラグインが認識されない場合
- Grasshopperのコンポーネントフォルダに正しくインストールされているか確認
- Rhinoを再起動
- Rhinoのバージョンが7であることを確認

### 接続エラーが発生する場合
- サーバーURLが正しいか確認
- サーバーが起動しているか確認
- APIキーが設定ファイルと一致しているか確認

## バージョン管理の注意点

複数のRhinocerosバージョンで作業する場合、以下の点に注意してください：

1. **インストールパスの分離**: Rhinoceros 7用とRhinoceros 8用のプラグインは別々のフォルダにインストールしてください

2. **環境変数の管理**: 複数の.NETバージョンを管理するために、環境変数を適切に設定してください

3. **設定ファイルの分離**: それぞれのバージョン用に別々の設定ファイルを用意してください
   - Rhinoceros 7用: `config-rhino7.json`
   - Rhinoceros 8用: `config-rhino8.json`

4. **プロジェクト名の区別**: ビルド成果物に区別しやすい名前を付けることを推奨します
   - Rhinoceros 7用: `GrasshopperMCP.Plugin.Rhino7.gha`
   - Rhinoceros 8用: `GrasshopperMCP.Plugin.Rhino8.gha`

これらの注意点を守ることで、複数のRhinocerosバージョンを混乱なく併用できます。
