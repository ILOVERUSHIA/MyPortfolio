using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalMoneyText; // 総財産テキスト
    [SerializeField] private TextMeshProUGUI spsText;        // 秒間獲得量テキスト
    [SerializeField] private List<TextMeshProUGUI> buildingButtonTexts; // 設備ボタンのテキスト群

    // 画面の文字を最新の状態に更新する
    public void UpdateUI(double currentMoney, double currentSPS, List<Building> buildings)
    {
        // 小数点を切り捨てて見やすく表示 (F0 = 小数点以下0桁)
        totalMoneyText.text = $"{currentMoney:F1} 円";
        spsText.text = $"毎秒: {currentSPS:F1} 円";

        // 各設備のボタンテキストを更新
        for (int i = 0; i < buildings.Count; i++)
        {
            if (i < buildingButtonTexts.Count)
            {
                Building b = buildings[i];
                buildingButtonTexts[i].text = $"{b.buildingName}\nコスト: {b.currentCost:F0}円 ({b.count}台)";
            }
        }
    }
}
