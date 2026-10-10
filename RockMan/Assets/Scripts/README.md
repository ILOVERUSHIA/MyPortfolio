# Rockman風 2Dプラットフォーマーゲーム 完全セットアップガイド

## 📝 実装済みの機能

✅ **プレイヤー操作**
- 左右移動（A/D または ←→キー）
- ジャンプ（スペースキー）
- スライディング（Shift + 移動キー）
- 弾発射（Zキー）

✅ **ゲームシステム**
- 物理演算（Rigidbody2D）による自然な動き
- グラウンド判定システム
- 敵AI（敵オブジェクト用）
- カメラフォロー機能

## 📁 作成済みスクリプト

```
Assets/Scripts/
├── PlayerController.cs       (プレイヤー操作・ジャンプ・スライディング)
├── PlayerShooter.cs          (弾発射)
├── Bullet.cs                 (弾の挙動)
├── Enemy.cs                  (敵キャラクター)
├── CameraFollower.cs         (カメラ追従)
├── SETUP_GUIDE.md           (詳細セットアップガイド)
└── QUICK_REFERENCE.md       (クイックリファレンス)

Assets/Prefabs/
└── (Bulletプリファブをここに配置)
```

## 🎮 セットアップ（5分で完了）

### ステップ 1: 入力システムの設定（既に実装）
- A/D キー → 移動
- Space → ジャンプ
- Shift → スライディング
- Z → 発射

### ステップ 2: プレイヤー作成

1. **Hierarchy** で右クリック → **2D Object → Sprite → Square**
2. 名前を `Player` に変更
3. **Inspector** で設定：
   ```
   Tag: "Player" （新規作成）
   Layer: "Player" （新規作成）
   Scale: (1, 1, 1)
   ```

4. **Rigidbody2D** コンポーネント追加：
   ```
   Body Type: Dynamic
   Gravity Scale: 2
   Collision Detection: Continuous
   Constraints: Freeze Rotation Z ✓
   ```

5. **Box Collider 2D** コンポーネント追加：
   ```
   Is Trigger: OFF
   ```

6. **PlayerController** スクリプト追加：
   ```
   Move Speed: 5
   Jump Force: 5
   Slide Speed: 8
   Slide Duration: 0.5
   Slide Cooldown: 0.3
   Ground Layer: "Ground" ← 後で設定
   Ground Check Distance: 0.1
   ```

7. **PlayerShooter** スクリプト追加：
   ```
   Shoot Cooldown: 0.3
   Bullet Speed: 10
   Shoot Point: Player (自身を設定)
   Bullet Prefab: (ステップ 5 で設定)
   ```

### ステップ 3: 地面（Ground）作成

1. **Hierarchy** で右クリック → **2D Object → Sprite → Square**
2. 名前を `Ground` に変更
3. **Inspector** で設定：
   ```
   Position: (0, -5, 0)
   Scale: (20, 1, 1)
   Tag: Untagged
   Layer: "Ground" （新規作成）
   ```

4. **Rigidbody2D** コンポーネント追加：
   ```
   Body Type: Static
   ```

5. **Box Collider 2D** コンポーネント追加：
   ```
   Is Trigger: OFF
   ```

### ステップ 4: Layer 作成

**Project Settings → Tags and Layers**

**Layers タブ** で以下を追加：
- Layer 8: Ground
- Layer 9: Player
- Layer 10: Enemy
- Layer 11: Bullet

各GameObjectに割り当て：
- Player → "Player" Layer
- Ground → "Ground" Layer

### ステップ 5: 弾（Bullet）プリファブ作成

1. **Hierarchy** で右クリック → **2D Object → Sprite → Circle**
2. 名前を `Bullet` に変更
3. **Inspector** で設定：
   ```
   Position: (0, 0, 0)
   Scale: (0.3, 0.3, 1)
   Tag: Untagged
   Layer: "Bullet"
   Color: 黄色（見分けやすく）
   ```

4. **Rigidbody2D** 追加：
   ```
   Body Type: Dynamic
   Gravity Scale: 0
   Collision Detection: Continuous
   ```

5. **Circle Collider 2D** 追加：
   ```
   Is Trigger: ON
   ```

6. **Bullet** スクリプト追加：
   ```
   Lifetime: 5
   Speed: 10
   ```

7. **Assets/Prefabs** フォルダにドラッグ＆ドロップでプリファブ化
8. Hierarchy から削除

### ステップ 6: PlayerShooter に Bullet プリファブ割り当て

Player の Inspector → PlayerShooter コンポーネント
```
Bullet Prefab: Assets/Prefabs/Bullet (プリファブをドラッグ)
```

### ステップ 7: PlayerController に Ground Layer 割り当て

Player の Inspector → PlayerController コンポーネント
```
Ground Layer: Ground (チェックボックスで Ground を選択)
```

### ステップ 8: カメラ設定（推奨）

Main Camera に CameraFollower スクリプト追加：
```
Player: Player GameObject をドラッグ
Smooth Speed: 0.125
Offset: (0, 1, -10)
Min X: -10
Max X: 10
Min Y: -5
Max Y: 5
```

## 🧪 テスト手順

1. **Play ボタンを押す**
2. **以下をテスト**:
   - A/D キーでプレイヤーが左右に動く ✓
   - Space キーでジャンプする ✓
   - Z キーで黄色い弾が発射される ✓
   - Shift + 移動キーでスライディング ✓
   - 地面の上でだけジャンプできる ✓

## 🎯 敵の追加方法（オプション）

1. **Hierarchy** で右クリック → **2D Object → Sprite → Square**
2. 名前を `Enemy` に変更
3. **Inspector**:
   ```
   Position: (5, 0, 0)
   Scale: (0.8, 0.8, 1)
   Color: 赤
   Tag: Untagged
   Layer: "Enemy"
   ```

4. **Rigidbody2D** 追加:
   ```
   Body Type: Dynamic
   Gravity Scale: 1
   Collision Detection: Continuous
   ```

5. **Box Collider 2D** 追加:
   ```
   Is Trigger: OFF
   ```

6. **Enemy** スクリプト追加:
   ```
   Health: 3
   Speed: 2
   Detection Range: 10
   ```

敵は **プレイヤーを検出すると追跡** し、弾に当たると消滅します。

## 📊 パラメータ調整ガイド

### 移動・ジャンプが遅い場合
```
PlayerController:
- Move Speed を 6-8 に上げる
- Jump Force を 6-8 に上げる
```

### スライディングがすぐ終わる場合
```
PlayerController:
- Slide Duration を 0.7-1.0 に上げる
```

### 弾が遅い場合
```
PlayerShooter & Bullet:
- Bullet Speed を 12-15 に上げる
```

### 地面をすり抜ける場合
```
1. PlayerController の Ground Check Distance を確認
2. Player の Box Collider 2D のサイズを確認
3. Ground Layer が正しく設定されているか確認
```

## 🐛 トラブルシューティング

| 問題 | 原因 | 解決方法 |
|------|------|---------|
| ジャンプできない | Ground Layer 未設定 | PlayerController に Ground Layer を設定 |
| 弾が発射されない | Bullet Prefab 未割り当て | PlayerShooter に Bullet プリファブを設定 |
| 敵が動かない | Enemy スクリプト未追加 | Enemy.cs をアタッチ |
| カメラが動かない | Player が見つからない | Main Camera に Player タグを確認させる |

## 🚀 次のステップ

1. **プラットフォーム追加** - レベルデザイン
2. **アニメーション** - 移動、ジャンプ、スライディングアニメ
3. **効果音・BGM** - AudioSource で音を追加
4. **敵バリエーション** - 異なるAIの敵を作成
5. **UI** - ライフゲージ、スコア表示
6. **ボス戦** - 特殊な敵を作成

---

**注:** すべてのスクリプトは完全に実装済みです。上記の手順に従うだけで、ロックマン風のゲームが実行できます！
