using UnityEngine;
using UnityEngine.Serialization;

namespace Services
{
    public class ResourceIncome : MonoBehaviour
    {
        [FormerlySerializedAs("countryTileStorage")]
        [FormerlySerializedAs("CountryTileData")]
        [SerializeField]
        public HexagonTileStorage hexagonTileStorage;
        

        // private  void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.I))
        //     {
        //         for (int i = 0; i < 10; i++)
        //         {
        //              BronzeIncome();
        //         }
        //     }
        // }
        //
        //
        // void BronzeIncome()
        // {
        //     foreach (var country in countryTileStorage.TilesData.Values)
        //     {
        //         country.Resources.CoinIncome++;
        //     }
        // }
    }
}