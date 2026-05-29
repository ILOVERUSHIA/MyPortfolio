using System;

[Serializable] // Unityのインスペクターで編集できるようにする
public class Building
{
    public string buildingName;    // 設備名
    public double baseCost;        // 初期コスト
    public double currentCost;     // 現在の購入コスト
    public double baseSPS;         // 1台あたりの秒間獲得量
    public int count;              // 現在の所持数

    // 初期化処理（最初のコストを設定）
    public void Initialize()
    {
        if (currentCost == 0) currentCost = baseCost;
    }

    // 次の購入コストを計算（クッキークリッカー風に1.15倍ずつ値上がり）
    public void UpdateCost()
    {
        currentCost = Math.Round(baseCost * Math.Pow(1.15, count));
    }

    // この設備が合計で毎秒いくら稼いでいるか
    public double GetTotalSPS()
    {
        return baseSPS * count;
    }
}
