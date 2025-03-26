# 開発者ガイド

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

Grasshopperプラグインのビルドには、RhinoとGrasshopper SDKの参照が必要です。これらは通常、Rhinoのインストールディレクトリにあります：

- RhinoCommon.dll: `C:\Program Files\Rhino 7\System\RhinoCommon.dll`
- Grasshopper.dll: `C:\Program Files\Rhino 7\Plug-ins\Grasshopper\Grasshopper.dll`

これらの参照をプロジェクトに追加し、Visual StudioまたはMSBuildを使用してビルドします。

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
