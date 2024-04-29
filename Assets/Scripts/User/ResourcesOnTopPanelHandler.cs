using Models.User;
using UnityEngine;
using UnityEngine.UI;

namespace User
{
    public class ResourcesOnTopPanelHandler : Singleton<ResourcesOnTopPanelHandler>
    {
        public RectTransform resourcesPanel;

        public Text wood;

        public Text stone;
    
        public Text steel;
    
        public Text bronze;
    
        public Text silver;
    
        public Text gold;
    
        public Text platinum;
    
        public Text influence;
    
        public Text donatCrystal;
        
        private UserModel _userModel;


        private void Awake()
        {
            _initUser();
            _updateResourceTexts();
        }
        
        
        private void _initUser()
        {
            _userModel = new UserModel
            {
                Id = 1,
                Username = "Alex11919291",
                ResourceModel = new ResourceModel()
                {
                    Wood = 100,
                    Stone = 100,
                    Steel = 100,
                    Bronze = 100,
                    Silver = 100,
                    Gold = 100,
                    Platinum = 100,
                    Influence = 100,
                    DonatCrystal = 50000,
                }
            };
        }


        private void _updateResourceTexts()
        {
            if (_userModel != null && _userModel.ResourceModel != null)
            {
                wood.text = _userModel.ResourceModel.Wood.ToString();
                stone.text = _userModel.ResourceModel.Stone.ToString();
                steel.text = _userModel.ResourceModel.Steel.ToString();
                bronze.text = _userModel.ResourceModel.Bronze.ToString();
                silver.text = _userModel.ResourceModel.Silver.ToString();
                gold.text = _userModel.ResourceModel.Gold.ToString();
                platinum.text = _userModel.ResourceModel.Platinum.ToString();
                influence.text = _userModel.ResourceModel.Influence.ToString();
                donatCrystal.text = _userModel.ResourceModel.DonatCrystal.ToString();
            }
            else
            {
                Debug.LogWarning("ResourceModel is not set for UserModel.");
            }
        }
    }
}