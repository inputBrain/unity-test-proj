
using UnityEngine;

public class BuildUpgradeHandler : Singleton<BuildUpgradeHandler>
{
    public RectTransform upgradePanel;
    
    void OnMouseDown()
    {
        if (upgradePanel != null)
        {
            var screenWidth = Screen.width;
            var screenHeight = Screen.height;

            var panelWidth = upgradePanel.rect.width;
            var panelHeight = upgradePanel.rect.height;

            upgradePanel.anchoredPosition = new Vector2((screenWidth - panelWidth) / 2, panelHeight / 2);

            upgradePanel.gameObject.SetActive(true);
        }
    }
    
    
}
