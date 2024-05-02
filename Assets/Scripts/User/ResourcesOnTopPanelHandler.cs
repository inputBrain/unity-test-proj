using System.Linq;
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
                TotalResourceModel = new TotalResourceModel()
                {
                    Wood = 0,
                    Stone = 0,
                    Steel = 0,
                    Bronze = 0,
                    Silver = 0,
                    Gold = 0,
                    Platinum = 0,
                    Influence = 10,
                    DonatCrystal = 50000,
                }
            };
        }


        private void _updateResourceTexts()
        {
            if (_userModel != null && _userModel.TotalResourceModel != null)
            {
                wood.text = _userModel.TotalResourceModel.Wood.ToString();
                stone.text = _userModel.TotalResourceModel.Stone.ToString();
                steel.text = _userModel.TotalResourceModel.Steel.ToString();
                bronze.text = _userModel.TotalResourceModel.Bronze.ToString();
                silver.text = _userModel.TotalResourceModel.Silver.ToString();
                gold.text = _userModel.TotalResourceModel.Gold.ToString();
                platinum.text = _userModel.TotalResourceModel.Platinum.ToString();
                influence.text = _userModel.TotalResourceModel.Influence.ToString();
                donatCrystal.text = _userModel.TotalResourceModel.DonatCrystal.ToString();
            }
            else
            {
                Debug.LogWarning("ResourceModel is not set for UserModel.");
            }
        }


        public void UpdateResourceTexts(
            int woodIncome,
            int stoneIncome,
            int steelIncome,
            int bronzeIncome,
            int silverIncome,
            int goldIncome,
            int platinumIncome,
            int influenceIncome
        )
        {
            if (_userModel != null && _userModel.TotalResourceModel != null)
            {
                UpdateResource(ref wood, woodIncome);
                UpdateResource(ref stone, stoneIncome);
                UpdateResource(ref steel, steelIncome);
                UpdateResource(ref bronze, bronzeIncome);
                UpdateResource(ref silver, silverIncome);
                UpdateResource(ref gold, goldIncome);
                UpdateResource(ref platinum, platinumIncome);
                UpdateResource(ref influence, influenceIncome);
            }
            else
            {
                Debug.LogWarning("ResourceModel is not set for UserModel.");
            }
        }
        
        
        
        private static void UpdateResource(ref Text resourceText, int income)
        {
            int parsedResource;
            if (resourceText.text.Contains('+'))
            {
                var indexOfPlus = resourceText.text.IndexOf('+');
                var preparedResource = resourceText.text[..indexOfPlus];
                parsedResource = int.Parse(preparedResource);
            }
            else
            {
                parsedResource = int.Parse(resourceText.text);
            }

            var totalAmount = parsedResource + income;
            resourceText.text = $"{totalAmount}+{income}";

        }
        
        
        public void CaptureTerritory(int hexagonCellCount = 1)
        {
            int parsedResource;
            if (influence.text.Contains('+'))
            {
                var indexOfPlus = influence.text.IndexOf('+');
                var preparedResource = influence.text[..indexOfPlus];
                parsedResource = int.Parse(preparedResource);
            }
            else
            {
                parsedResource = int.Parse(influence.text);
            }

            // var totalAmount = parsedResource + income;
            // influence.text = $"{totalAmount}+{income}";
        }
    }
}