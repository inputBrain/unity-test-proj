using UnityEngine;

public class BuildUpgradeHandler : Singleton<BuildUpgradeHandler>
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
