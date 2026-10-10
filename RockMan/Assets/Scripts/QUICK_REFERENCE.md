# Rockman風ゲーム クイックリファレンス

## スクリプト説明

### PlayerController.cs
プレイヤーの移動、ジャンプ、スライディングを管理
- **公開メソッド**:
  - `GetFacingDirection()`: プレイヤーの向き（1=右, -1=左）を返す
  - `IsSliding()`: スライディング中かどうか
  - `IsGrounded()`: 地面に接触しているかどうか
  - `GetVelocity()`: 現在の速度を返す

### PlayerShooter.cs
弾の発射を管理（Z キーで発射）
- PlayerShooter が Bullet プリファブを参照して弾を生成

### Bullet.cs
弾の移動と衝突判定を管理
- 敵や壁に衝突すると消滅
- 5秒後に自動的に消滅

### Enemy.cs（オプション）
敵キャラクターの基本実装
- プレイヤーを検出すると追跡
- 弾に当たるとダメージを受けて消滅
- 3ヒットで倒れる（health: 3）

## 物理設定の重要ポイント

### プレイヤー（Rigidbody2D）
```
Gravity Scale: 2 ← 落下速度
Body Type: Dynamic
Mass: 1
Constraints: Freeze Rotation Z (回転を固定)
```

### 地面（Rigidbody2D）
```
Body Type: Static ← 動かない
```

### 弾（Rigidbody2D）
```
Gravity Scale: 0 ← 重力の影響を受けない
Body Type: Dynamic
```

## Layer設定

1. **Project Settings → Tags and Layers** を開く
2. **Layers** タブで以下を追加:
   - Ground
   - Player
   - Enemy
   - Bullet

3. 各GameObjectにレイヤーを割り当て:
   - Player GameObject → "Player" レイヤー
   - Ground GameObject → "Ground" レイヤー
   - Enemy GameObject → "Enemy" レイヤー
   - Bullet Prefab → "Bullet" レイヤー

## Collider2Dの使い分け

- **Is Trigger: OFF** → 物理的な衝突（プレイヤー、地面、壁）
- **Is Trigger: ON** → 判定用（敵判定、弾の敵判定）

## 敵を追加する方法

1. Hierarchy で右クリック → 2D Object → Sprite → Square
2. 名前を「Enemy」に変更
3. 以下を設定:
   - Scale: (0.8, 0.8, 1)
   - Color: 赤など
   - Tag: "Enemy"
   - Layer: "Enemy"

4. コンポーネント追加:
   - Rigidbody2D (Gravity Scale: 1)
   - Box Collider 2D (Is Trigger: OFF)
   - Enemy スクリプト

## パフォーマンス最適化

- Bullet の Lifetime を調整して画面外のオブジェクト削除
- OnPooling パターンを検討（大量の弾が必要な場合）
- Collider の数を必要最小限に

## 次のステップ（推奨順）

1. **基本動作テスト** - 移動、ジャンプ、弾発射が動作するか確認
2. **敵を追加** - Enemy スクリプトで敵を配置して撃つ練習
3. **アニメーション** - Sprite Animation で移動・スライディング・ジャンプアニメ
4. **効果音** - AudioSource で SEを追加
5. **ステージデザイン** - Platform を複数配置してレベルを作成
6. **UI・ゲームオーバー** - ライフ表示、敵全滅で次ステージへ

## デバッグのコツ

### Console で値を確認
```csharp
// PlayerController.cs の Update() に追加
Debug.Log($"Grounded: {isGrounded}, Velocity: {rb.velocity}, Sliding: {isSliding}");
```

### Gizmos で判定範囲を可視化
```csharp
private void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(transform.position + groundCheckOffset, 
        new Vector3(0.5f, groundCheckDistance * 2f, 0));
}
```
