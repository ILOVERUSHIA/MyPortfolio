using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public double totalMoney = 0;   // 総財産
    public double spc = 1;          // 1クリックの獲得量 (Click)
    public double totalSPS = 0;     // 総・秒間自動獲得量 (Second)

    public List<Building> buildings = new List<Building>(); // 設備リスト
    [SerializeField] private UIManager uiManager;           // UI管理への参照

    void Start()
    {
        // 各設備の初期設定
        foreach (var building in buildings)
        {
            building.Initialize();
        }
        UpdateGameStats();
    }

    void Update()
    {
        // 【時間経過での自動増加】フレーム毎に財産を増やす
        if (totalSPS > 0)
        {
            totalMoney += totalSPS * Time.deltaTime;
            uiManager.UpdateUI(totalMoney, totalSPS, buildings);
        }
    }

    // 【手動増加】メインボタンがクリックされたとき
    public void OnMainButtonClick()
    {
        totalMoney += spc;
        uiManager.UpdateUI(totalMoney, totalSPS, buildings);
    }

    // 設備購入ボタンがクリックされたとき
    public void TryBuyBuilding(int index)
    {
        if (index < 0 || index >= buildings.Count) return;

        Building building = buildings[index];

        // 財産が足りているかチェック
        if (totalMoney >= building.currentCost)
        {
            totalMoney -= building.currentCost; // コストを支払う
            building.count++;                   // 所持数を増やす
            building.UpdateCost();              // 次のコストを計算
            UpdateGameStats();                  // ★ここでSPCやSPSを再計算する
        }
    }

    // ステータスの再計算とUI更新
    private void UpdateGameStats()
    {
        // --- ★ココを変更：クリック獲得量(spc)の計算 ---
        // 初期値の1に、「カーソル(インデックス0)」の所持数をそのまま足す
        if (buildings.Count > 0)
        {
            spc = 1 + buildings[0].count;
        }
        else
        {
            spc = 1;
        }
        // ---------------------------------------------

        // 自動獲得量(SPS)の再計算
        totalSPS = 0;
        foreach (var building in buildings)
        {
            totalSPS += building.GetTotalSPS();
        }
        uiManager.UpdateUI(totalMoney, totalSPS, buildings);
    }
}
