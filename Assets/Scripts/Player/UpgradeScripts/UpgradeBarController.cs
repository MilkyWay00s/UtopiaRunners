using UnityEngine;
using UnityEngine.UI;

public class UpgradeBarController : MonoBehaviour
{
    public UpgradeData upgradeData;    
    public Image[] barSegments;
    public Color filledColor = Color.green;
    public Color emptyColor = Color.gray;

    private int currentLevel => upgradeData.currentLevel;
    private int maxLevel => upgradeData.maxLevel;

    private void Start()
    {
        UpdateBarVisual();
    }

    public void UpdateBarVisual()
    {
        int currentLevel = upgradeData.currentLevel;

        for (int i = 0; i < barSegments.Length; i++)
        {
            barSegments[i].color = (i < currentLevel) ? filledColor : emptyColor;
        }
    }
}
