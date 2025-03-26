# バージョン情報

## リリースノート

### v1.0.0 (Rhinoceros 8用)
- Rhinoceros 8向け初期リリース
- .NET 7をターゲット
- Rhinoceros 8およびGrasshopper 2.0向けに最適化
- MCPプロトコルの完全実装
- Rhino 7版からの移植と拡張

### v1.0.0 (Rhinoceros 7用)
- 初期リリース
- .NET 6.0をターゲット
- Rhinoceros 7およびGrasshopper向けに最適化

## ブランチ情報

このリポジトリには複数のバージョンが含まれています：

- **main**: Rhinoceros 7向けの実装（.NET 6.0）
- **rhino8-net7**: Rhinoceros 8向けの実装（.NET 7） - 現在のブランチ

## 互換性情報

| ブランチ | .NET バージョン | Rhinoceros バージョン | Grasshopper バージョン |
|---------|----------------|---------------------|----------------------|
| main    | 6.0            | 7.x                 | 1.x                  |
| rhino8-net7 | 7.0        | 8.x                 | 2.x                  |

## 選択ガイド

- Rhinoceros 7を使用している場合はmainブランチを選択
- Rhinoceros 8を使用している場合はrhino8-net7ブランチを選択（現在のブランチ）

詳細なインストール手順については、各ブランチのdocs/installation.mdを参照してください。
