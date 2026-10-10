# Unity自動セットアップガイド

このドキュメントでは、Unity Editor内から1クリックでゲームシーンを自動構築する方法を説明します。

## 🚀 セットアップ実行方法

### ステップ 1: Unity Editorを開く
- RockMan プロジェクトを Unity Editor で開きます

### ステップ 2: SampleScene を開く
- `Assets/Scenes/SampleScene.unity` をダブルクリックして開きます

### ステップ 3: 自動セットアップを実行
Unity のメニューバーから **`RockMan → Setup Game Scene`** をクリック

**メニュー場所:**
```
Menu Bar → RockMan → Setup Game Scene
```

### ステップ 4: 確認ダイアログで「Yes」をクリック
確認ダイアログが表示されます：
```
Setup RockMan Scene
This will create all necessary GameObjects for the RockMan game.
Continue?
[Yes] [No]
```

「Yes」をクリックすると以下が自動実行されます：

✅ **自動実行内容**
- `Player` GameObject 作成（コンポーネント装備済み）
- `Ground` GameObject 作成（コンポーネント装備済み）
- `Bullet` プリファブ作成（Assets/Prefabs/Bullet.prefab）
- 各種 Layer 作成（Ground, Player, Enemy, Bullet）
- Main Camera に CameraFollower を追加
- すべてのスクリプトをアタッチ
- 推奨パラメータを設定

### ステップ 5: 完了メッセージ確認
```
Success
Scene setup complete!

Next steps:
1. Assign sprites to Player and Ground (optional)
2. Press Play to test the game!
```

「OK」をクリックして完了！

## 🎮 セットアップ後のシーン構造

自動セットアップ実行後、以下の構造が作成されます：

```
SampleScene
├─ Main Camera (CameraFollower 装備)
└─ Hierarchy に自動作成：
   ├─ Player
   │  ├─ Sprite Renderer (Light Blue)
   │  ├─ Rigidbody2D (Dynamic, Gravity: 2)
   │  ├─ Box Collider 2D
   │  ├─ PlayerController ✓設定済み
   │  └─ PlayerShooter ✓設定済み (Bullet プリファブ割り当て済み)
   │
   └─ Ground
      ├─ Sprite Renderer (Brown)
      ├─ Rigidbody2D (Static)
      └─ Box Collider 2D

Assets/Prefabs/
└─ Bullet.prefab
   ├─ Sprite Renderer (Yellow)
   ├─ Rigidbody2D (Dynamic, Gravity: 0)
   ├─ Circle Collider 2D (Trigger)
   └─ Bullet スクリプト ✓設定済み
```

## 📋 設定済み項目チェックリスト

### Player GameObject
- ✅ Tag: "Player"
- ✅ Layer: "Player"
- ✅ PlayerController コンポーネント
  - moveSpeed: 5
  - jumpForce: 5
  - slideSpeed: 8
  - slideDuration: 0.5
  - slideCooldown: 0.3
  - groundLayer: Ground
- ✅ PlayerShooter コンポーネント
  - shootCooldown: 0.3
  - bulletSpeed: 10
  - bulletPrefab: Bullet プリファブ（自動割り当て）
  - shootPoint: Player

### Ground GameObject
- ✅ Position: (0, -5, 0)
- ✅ Scale: (20, 1, 1)
- ✅ Layer: "Ground"
- ✅ Rigidbody2D: Static
- ✅ Box Collider 2D

### Bullet Prefab
- ✅ Layer: "Bullet"
- ✅ Scale: (0.3, 0.3, 1)
- ✅ Bullet スクリプト
  - lifetime: 5
  - speed: 10

### Layers
自動作成済み：
- ✅ Ground
- ✅ Player
- ✅ Enemy
- ✅ Bullet

### Main Camera
- ✅ CameraFollower スクリプト追加
- ✅ Player 参照設定完了

## ⚡ すぐにプレイテスト

セットアップ完了後、すぐにゲームをテストできます：

```
1. 「Play」ボタンをクリック
2. 以下を試す：
   - A/D キー: 移動
   - Space: ジャンプ
   - Shift + 移動: スライディング
   - Z: 弾発射
```

完全に動作するはずです！

## 🔄 再セットアップが必要な場合

何か問題が発生した場合：

1. Hierarchy の `Player` と `Ground` を削除
2. Assets/Prefabs の `Bullet.prefab` を削除
3. 再度 **`RockMan → Setup Game Scene`** を実行

## 📝 手動調整

セットアップ後、以下を手動で調整できます：

### プレイヤー外観の変更
Player の Sprite Renderer で Sprite を割り当てて見た目を変更

### 地面外観の変更
Ground の Sprite Renderer で Sprite を割り当てて見た目を変更

### パラメータ微調整
- Player の PlayerController コンポーネント内で
  - moveSpeed, jumpForce, slideSpeed などを調整
- Player の PlayerShooter コンポーネント内で
  - shootCooldown, bulletSpeed などを調整

## ✨ 次のステップ

セットアップ完了後、以下を追加することでゲームを拡張できます：

1. **敵の配置** - Enemy.cs を使って敵を配置
2. **プラットフォーム追加** - より複雑なレベルデザイン
3. **アニメーション** - 移動、ジャンプ、スライディングのアニメーション
4. **効果音** - 移動、ジャンプ、弾発射の音
5. **UI/ゲームオーバー** - ライフ表示、敵全滅で次ステージへ

---

**自動セットアップが完了したら、README.md と QUICK_REFERENCE.md を参照してさらにゲームを拡張してください！**
