using UnityEngine;

public class BuildUpgradeMenu : Singleton<BuildUpgradeMenu>
{
    public RectTransform upgradePanel;


    private void Awake()
    {
        upgradePanel.gameObject.SetActive(false);
    }
    
    public void IsEnabledPanel(bool isEnable)
    {
        upgradePanel.gameObject.SetActive(isEnable);
    }
}
