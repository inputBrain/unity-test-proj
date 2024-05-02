using UnityEngine;
using User;

namespace Gameplay
{
    public class CaptureTerritory : Singleton<CaptureTerritory>
    {
        public RectTransform CapturePanel;
        public RectTransform ResourcesPanel;
        
        private ResourcesOnTopPanelHandler _resourcesOnTopPanelHandler;

        
        void Awake()
        {
            _resourcesOnTopPanelHandler = FindObjectOfType<ResourcesOnTopPanelHandler>();

            CapturePanel.gameObject.SetActive(false);
        }


        public void OnCaptureButtonClick()
        {
            ResourcesPanel.
        }
    }
}