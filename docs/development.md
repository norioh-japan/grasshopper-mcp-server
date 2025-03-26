# 開発者ガイド (Rhinoceros 8版)

## プロジェクト構成

```
GrasshopperMCP/
├── src/
│   ├── GrasshopperMCP.Server/          # MCPサーバー実装
│   │   ├── Program.cs                  # エントリポイント
│   │   ├── Startup.cs                  # サーバー設定
│   │   ├── MCPConfig.cs                # MCP設定クラス
│   │   ├── Controllers/                # APIコントローラー
│   │   ├── Models/                     # データモデル
│   │   ├── Services/                   # サービス層
│   │   └── Utils/                      # ユーティリティクラス
│   │
│   ├── GrasshopperMCP.Plugin/          # Grasshopperプラグイン
│   │   ├── GrasshopperMCPPlugin.cs     # プラグインメインクラス
│   │   ├── GrasshopperMCPComponent.cs  # Grasshopperコンポーネント
│   │   ├── Handlers/                   # リクエストハンドラー
│   │   └── Utils/                      # ユーティリティクラス
│   │
│   └── GrasshopperMCP.Common/          # 共通ライブラリ
│       ├── Models/                     # 共有データモデル
│       └── Interfaces/                 # 共通インターフェース
│
├── tests/                              # テストプロジェクト
│   ├── GrasshopperMCP.Tests.Unit/      # 単体テスト
│   └── GrasshopperMCP.Tests.Integration/# 統合テスト
│
├── docs/                               # ドキュメント
│   ├── api/                            # API ドキュメント
│   └── examples/                       # サンプル使用例
│
└── tools/                              # ビルドツールとスクリプト
```

## プロジェクトのビルド

### サーバープロジェクト

```bash
cd src/GrasshopperMCP.Server
dotnet build
```

### Grasshopperプラグイン

Rhinoceros 8のNuGetパッケージを参照することで、Grasshopper 2.0プラグインのビルドが簡単になりました。

```bash
cd src/GrasshopperMCP.Plugin
dotnet build
```

## MCP実装の詳細

### ツール (Tools)

MCPツールは、`IMcpTool`インターフェースを実装し、`[McpTool]`属性で装飾されたクラスとして実装されています。

例：

```csharp
[McpTool("add_component")]
public class AddComponentTool : IMcpTool
{
    private readonly IGrasshopperService _grasshopperService;
    
    public AddComponentTool(IGrasshopperService grasshopperService)
    {
        _grasshopperService = grasshopperService;
    }
    
    public async Task<IActionResult> ExecuteAsync(JObject parameters)
    {
        // 実装
    }
}
```

### リソース (Resources)

MCPリソースは、`IMcpResource`インターフェースを実装し、`[McpResource]`属性で装飾されたクラスとして実装されています。

例：

```csharp
[McpResource("definition")]
public class DefinitionResource : IMcpResource
{
    private readonly IDefinitionManager _definitionManager;
    
    public DefinitionResource(IDefinitionManager definitionManager)
    {
        _definitionManager = definitionManager;
    }
    
    public async Task<IActionResult> GetAsync(string resourcePath)
    {
        // 実装
    }
}
```

## .NET 7での変更点

.NET 7への移行に伴い、いくつかの重要な変更点があります：

1. **非同期APIの改善**: 非同期処理がより最適化され、パフォーマンスが向上しています
2. **並列処理**: 並列処理のAPIが拡張され、高負荷時の処理能力が向上しています
3. **Rhinoceros 8との互換性**: Rhinoceros 8の新機能にアクセスするためには.NET 7以上が必要です

## スレッド処理の考慮事項

RhinocerosはシングルスレッドモデルをAPIに使用しています。別のスレッドからRhino/Grasshopper APIを呼び出す場合は、`RhinoApp.InvokeOnUiThread`を使用して、呼び出しをメインUIスレッドにマーシャリングする必要があります：

```csharp
await Task.Run(() => {
    RhinoApp.InvokeOnUiThread(new Action(() =>
    {
        // Rhino API呼び出しをここに
    }));
});
```

## テスト

### 単体テスト

```bash
cd tests/GrasshopperMCP.Tests.Unit
dotnet test
```

### 統合テスト

```bash
cd tests/GrasshopperMCP.Tests.Integration
dotnet test
```

## 公開

### サーバーパッケージ

```bash
cd src/GrasshopperMCP.Server
dotnet pack -c Release
```

これにより、NuGet.orgに公開したり、ローカルにインストールしたりできるNuGetパッケージが生成されます。

### Grasshopperプラグイン

プラグインをRelease構成でビルドすると、.ghaファイルが生成されます。このファイルをユーザーのGrasshopperコンポーネントフォルダにインストールするよう配布できます。

## Rhinoceros 8固有の機能

Rhinoceros 8とGrasshopper 2.0には、以前のバージョンにない新機能が含まれています。これらの機能を活用するためのヒントを以下に示します：

### Grasshopper 2.0の新機能

1. **改善されたデータツリー処理**: Grasshopper 2.0ではデータツリーの処理が効率化されています。新しいAPIを活用してください。

2. **拡張されたジオメトリ機能**: Rhino 8の拡張されたジオメトリ機能にアクセスできます。

3. **パフォーマンスの向上**: Grasshopper 2.0ではパフォーマンスが向上しており、より高速な処理が可能です。

### Grasshopper 2.0のAPI変更点への対応

Grasshopper 2.0では一部のAPIが変更されている可能性があります。移行時に注意すべき点：

1. **非推奨APIの確認**: 古いAPIが非推奨になっている場合は、新しい推奨APIを使用してください。

2. **型の変更**: 一部の型や戻り値が変更されている可能性があるため、コンパイルエラーに注意してください。

3. **新機能の活用**: 新しく追加されたAPIを活用して、機能を拡張してください。

## 両バージョンの互換性について

このプロジェクトでは、Rhinoceros 7向け（.NET 6.0）とRhinoceros 8向け（.NET 7）の両方のバージョンを維持しています。コード共有や互換性維持のためのヒント：

1. **条件付きコンパイル**: 特定の環境でのみ必要なコードには条件付きコンパイルを使用できます：
   ```csharp
   #if NET7_0_OR_GREATER
       // .NET 7以上でのみ実行されるコード
   #else
       // .NET 6.0用のコード
   #endif
   ```

2. **共通コードの分離**: 両環境で共通のコードはGrasshopperMCP.Commonプロジェクトに配置し、環境固有のコードのみを条件付きで実装することで、メンテナンスを容易にします。

3. **バージョン間のブランチ管理**: mainブランチ（Rhino 7）とrhino8-net7ブランチ（Rhino 8）間で機能やバグ修正をマージする際は、環境固有の差異に注意してください。

## その他のリソース

- [Rhinoceros 8開発者ドキュメント](https://developer.rhino3d.com/)
- [Grasshopper SDK API リファレンス](https://developer.rhino3d.com/api/grasshopper/)
- [.NET 7ドキュメント](https://docs.microsoft.com/ja-jp/dotnet/core/whats-new/dotnet-7)
- [MCP（Model Context Protocol）仕様](https://modelcontextprotocol.io/)
