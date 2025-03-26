# Grasshopper MCP サーバー (Rhinoceros 8用)

Rhinoceros 8/Grasshopper 2.0との連携を可能にするModel Context Protocol (MCP) サーバー実装です。AIモデルがパラメトリックデザインツールと自然言語で対話することを可能にします。

## 概要

このプロジェクトは、大規模言語モデル (LLM) をRhinoceros 8とGrasshopper 2.0機能に接続するMCPサーバーを実装し、以下を可能にします：

- 自然言語によるパラメトリックデザインの作成と操作
- Grasshopper定義とコンポーネントの管理
- 設計ソリューションの実行と結果の取得
- AIシステムと高度な計算デザインツールの連携

## アーキテクチャ

システムは主に2つのコンポーネントで構成されています：

1. **MCPサーバー** - Model Context Protocolを実装する.NET 7アプリケーション
2. **Grasshopperプラグイン** - Grasshopper内で実行され、MCPサーバーと通信するコンポーネント

```
[LLM] <--HTTP/WebSocket--> [MCPサーバー] <--API--> [Grasshopperプラグイン] <--> [Rhinoceros 8]
```

## 必要条件

- Rhinoceros 8
- .NET 7.0 SDK以上
- Visual Studio 2022またはC#拡張機能を持つVS Code

## バージョンについて

このリポジトリには2つのバージョンが含まれています：

- **main ブランチ**: .NET 6.0を使用したRhinoceros 7向けの実装
- **rhino8-net7 ブランチ**: .NET 7を使用したRhinoceros 8向けの実装

環境に合わせて適切なバージョンを選択してください。

## はじめ方

詳細なセットアップ手順については、docsディレクトリの「インストールガイド」を参照してください。

## 開発

プロジェクトの構築と拡張に関する情報については、docsディレクトリの「開発者ガイド」を参照してください。

## ライセンス

このプロジェクトはMITライセンスの下で提供されています。詳細についてはLICENSEファイルを参照してください。
