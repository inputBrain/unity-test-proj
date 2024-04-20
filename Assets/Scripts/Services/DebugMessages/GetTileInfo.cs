using UnityEngine;
using UnityEngine.Tilemaps;

namespace Services.DebugMessages
{
    public class GetTileInfo : MonoBehaviour
    {
        private Tilemap _tilemap;
        
        [SerializeField]
        private GameObject gridTilemap;
        
        private CountryTileStorage _countryTileStorage;
        
        
        private void Start()
        {
            _tilemap = GetComponent<Tilemap>();
            _countryTileStorage = gridTilemap.GetComponent<CountryTileStorage>();
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = Physics2D.GetRayIntersection(ray);
                var hitPosition = hit.point;
                var gridPos = _tilemap.WorldToCell(hitPosition);
            
                if (_countryTileStorage.TilesData.TryGetValue(gridPos, out var tileInfo))
                {
                    Debug.Log($"Country: {tileInfo.Country} Bronze: {tileInfo.Resources.CoinIncome}");
                }
                else
                {
                    Debug.Log("No tile found at position: " + gridPos);
                }
            }
            
            //
            // if (Input.GetMouseButtonDown(0))
            // {
            //     var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //     var hit = Physics2D.GetRayIntersection(ray);
            //     var hitPosition = hit.point;
            //     var gridPos = _tilemap.WorldToCell(hitPosition);
            //
            //     if (_countryTileData.CapitalsDict.TryGetValue(gridPos, out var countryName))
            //     {
            //         Debug.Log($"{countryName}");
            //     }
            //     else
            //     {
            //         Debug.Log("No tile found at position: " + gridPos);
            //     }
            // }
        }
    }
}
