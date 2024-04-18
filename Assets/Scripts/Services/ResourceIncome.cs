using UnityEngine;

namespace Services
{
    public class ResourceIncome : MonoBehaviour
    {
        [SerializeField]
        public CountryTileData CountryTileData;
        

        private  void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                for (int i = 0; i < 10; i++)
                {
                     BronzeIncome();
                }
            }
        }
        
        
        void BronzeIncome()
        {
            foreach (var country in CountryTileData.TilesDict.Values)
            {
                country.Resources.Bronze++;
            }
        }
    }
}