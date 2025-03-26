# Grasshopper MCP サーバー

Rhinoceros/Grasshopperとの連携を可能にするModel Context Protocol (MCP) サーバー実装です。AIモデルがパラメトリックデザインツールと自然言語で対話することを可能にします。

## 概要

このプロジェクトは、大規模言語モデル (LLM) をRhinocerosとGrasshopper機能に接続するMCPサーバーを実装し、以下を可能にします：

- 自然言語によるパラメトリックデザインの作成と操作
- Grasshopper定義とコンポーネントの管理
- 設計ソリューションの実行と結果の取得
- AIシステムと高度な計算デザインツールの連携

## アーキテクチャ

システムは主に2つのコンポーネントで構成されています：

1. **MCPサーバー** - Model Context Protocolを実装する.NET Coreアプリケーション
2. **Grasshopperプラグイン** - Grasshopper内で実行され、MCPサーバーと通信するコンポーネント

```
[LLM] <--HTTP/WebSocket--> [MCPサーバー] <--API--> [Grasshopperプラグイン] <--> [Rhinoceros]
```

## 必要条件

- Rhinoceros 7以上
- .NET 6.0 SDK以上
- Visual Studio 2022またはC#拡張機能を持つVS Code

## はじめ方

詳細なセットアップ手順については、docsディレクトリの「インストールガイド」を参照してください。

## 開発

プロジェクトの構築と拡張に関する情報については、docsディレクトリの「開発者ガイド」を参照してください。

## ライセンス

このプロジェクトはMITライセンスの下で提供されています。詳細についてはLICENSEファイルを参照してください。
