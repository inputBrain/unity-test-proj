using System;
using Resource;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class CaptureTerritory : Singleton<CaptureTerritory>
    {
        private ResourcesOnTopPanelHandler _resourcesOnTopPanelHandler;
        
        public RectTransform capturePanel;
        public Text captureCostText;
        
        
        void Awake()
        {
            _resourcesOnTopPanelHandler = FindObjectOfType<ResourcesOnTopPanelHandler>();

            capturePanel.gameObject.SetActive(false);
        }
        


        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                capturePanel.gameObject.SetActive(true);
            }
            
            // if (Input.GetMouseButtonDown(0) && capturePanel.gameObject.transform.position.)
            // {
            //     capturePanel.gameObject.SetActive(true);
            // }
        }
        
        

        public void OnCaptureButtonClick()
        {
            // _resourcesOnTopPanelHandler.UpdateInfluenceByCaptureTerritory(hexagonCellCount);
        }
    }
}