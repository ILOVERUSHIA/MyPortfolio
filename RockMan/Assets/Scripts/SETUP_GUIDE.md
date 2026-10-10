# Rockman風 2Dプラットフォーマーゲーム セットアップガイド

## 機能一覧
- ✅ 左右移動（A/D キーまたは矢印キー）
- ✅ ジャンプ（スペースキー）
- ✅ 弾発射（Z キー）
- ✅ スライディング（Shift キー + 移動キー）

## セットアップ手順

### 1. プレイヤーGameObjectの作成

1. SampleSceneを開く
2. Hierarchy で右クリック → 2D Object → Sprite → Square を選択
3. 名前を「Player」に変更
4. Inspectorで以下を設定：
   - **Transform**: Position (0, 0, 0)
   - **Sprite Renderer**: Sprite を任意の画像に設定（または色で区別）
   - **Tag**: 「Player」タグを新規作成して割り当て

### 2. Rigidbody2Dコンポーネント追加

PlayerGameObjectに以下を追加：
- **Add Component → Rigidbody2D**
  - Body Type: Dynamic
  - Gravity Scale: 2
  - Collision Detection: Continuous
  - Constraints: Freeze Rotation Z を有効化

### 3. Colliderの設定

PlayerGameObjectに以下を追加：
- **Add Component → Box Collider 2D**
  - Is Trigger: OFF

### 4. スクリプトコンポーネント追加

PlayerGameObjectに以下を追加：
- **PlayerController** スクリプト
- **PlayerShooter** スクリプト

#### PlayerController設定：
- Move Speed: 5
- Jump Force: 5
- Slide Speed: 8
- Slide Duration: 0.5
- Slide Cooldown: 0.3
- Ground Layer: 「Ground」レイヤーを設定
- Ground Check Distance: 0.1

#### PlayerShooter設定：
- Shoot Cooldown: 0.3
- Bullet Speed: 10
- Shoot Point: Player自身（またはショット発射位置）

### 5. 地面の作成

1. 新しいGameObject作成：**GameObject → 2D Object → Sprite → Square**
2. 名前を「Ground」に変更
3. 以下を設定：
   - **Transform**: Position (0, -5, 0), Scale (10, 1, 1)
   - **Sprite Renderer**: デフォルトスプライト表示
   - **Add Component → Box Collider 2D** (Is Trigger: OFF)
4. Layer を「Ground」に設定（新規作成）
   - Layer Manager で「Ground」レイヤーを作成し割り当て

### 6. 弾（Bullet）プリファブの作成

1. 新しいGameObject作成：**GameObject → 2D Object → Sprite → Circle**
2. 名前を「Bullet」に変更
3. 以下を設定：
   - **Transform**: Scale (0.3, 0.3, 1)
   - **Sprite Renderer**: 色を黄色など見分けやすい色に
   - **Add Component → Rigidbody2D**
     - Body Type: Dynamic
     - Gravity Scale: 0（重力の影響を受けない）
     - Collision Detection: Continuous
   - **Add Component → Circle Collider 2D**
     - Is Trigger: ON（敵などとの判定用）
   - **Bullet** スクリプト追加
     - Lifetime: 5
     - Speed: 10

4. 「Bullet」GameObjectを Assets/Prefabs フォルダにドラッグ＆ドロップしてプリファブ化
5. Hierarchy から削除

### 7. PlayerShooterの設定

PlayerGameObjectの PlayerShooter コンポーネントで：
- **Bullet Prefab**: Assets/Prefabs/Bullet プリファブを割り当て
- **Shoot Point**: Player自身（またはプレイヤーの子オブジェクトとして発射位置を指定）

### 8. シーンの階層構造（完成後）

```
SampleScene
├─ Player
│  ├─ Rigidbody2D
│  ├─ Box Collider 2D
│  ├─ PlayerController
│  └─ PlayerShooter
└─ Ground
   ├─ Box Collider 2D
   └─ Rigidbody2D (Body Type: Static)
```

## 操作方法

| 操作 | キー |
|------|------|
| 左移動 | A / ← |
| 右移動 | D / → |
| ジャンプ | Space |
| 弾発射 | Z |
| スライディング | Shift + 移動キー（地面にいる時のみ） |

## 調整用パラメータ

### PlayerController
- **moveSpeed**: プレイヤーの移動速度（推奨: 4-8）
- **jumpForce**: ジャンプの高さ（推奨: 4-8）
- **slideSpeed**: スライディング時の速度（推奨: 6-12）
- **slideDuration**: スライディング持続時間（秒）（推奨: 0.3-0.7）
- **slideCooldown**: スライディング再使用待機時間

### PlayerShooter
- **shootCooldown**: 連射速度（秒、小さいほど速い）
- **bulletSpeed**: 弾の速度

### Bullet
- **lifetime**: 弾の消滅までの時間（秒）
- **speed**: 弾の速度（PlayerShooterと同期させる）

## トラブルシューティング

**プレイヤーが落ち続ける場合**
- Ground LayerがPlayerControllerの「Ground Layer」に設定されているか確認
- Ground LayerがGroundGameObjectに割り当てられているか確認

**弾が発射されない場合**
- PlayerShooter の Bullet Prefab が割り当てられているか確認
- Bullet プリファブが Assets/Prefabs にあるか確認

**スライディングが動作しない場合**
- Ground LayerとPlayerControllerの設定を確認
- Shift キーと矢印キー/A/D キーを同時押ししているか確認
- isGrounded が true になっているか確認（Console で Debug.Log を追加して検証）

## 拡張機能の提案

以下の機能を追加することで、より完全なロックマン風ゲームになります：

1. **敵システム**: Enemy スクリプトを作成
2. **ダメージシステム**: Player に Health コンポーネント追加
3. **アニメーション**: PlayerController に状態に応じたアニメーション
4. **パーティクルエフェクト**: スライディング、ジャンプ、着地時のエフェクト
5. **効果音**: 移動、ジャンプ、弾発射、スライディングのサウンド
6. **UI**: ライフ表示、スコア表示
