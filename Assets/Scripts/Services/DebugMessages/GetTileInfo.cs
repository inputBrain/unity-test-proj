using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Services.DebugMessages
{
    public class GetTileInfo : MonoBehaviour
    {
        private Tilemap _tilemap;
        
        [SerializeField]
        private GameObject gridTilemap;
        
        private HexagonTileStorage _hexagonTileStorage;

        private BuildUpgradeHandler _buildUpgradeHandler;
        
        
        private void Start()
        {
            _tilemap = GetComponent<Tilemap>();
            _buildUpgradeHandler = gridTilemap.GetComponent<BuildUpgradeHandler>();
            _hexagonTileStorage = gridTilemap.GetComponent<HexagonTileStorage>();
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = Physics2D.GetRayIntersection(ray);
                var hitPosition = hit.point;
                var gridPos = _tilemap.WorldToCell(hitPosition);
            
                if (_hexagonTileStorage.TilesData.TryGetValue(gridPos, out var tileInfo))
                {
                    Debug.Log($"Country: {tileInfo.Country}");
                    //отрисовка

                    if (!string.IsNullOrEmpty(tileInfo.Country))
                    {
                        _buildUpgradeHandler.IsEnabledPanel(true);
                    }
                    else
                    {
                        _buildUpgradeHandler.IsEnabledPanel(false);
                        var ww = new List<string>(){"1", "2", ""};
                    }
                }
                else
                {
                    Debug.Log("No tile found at position: " + gridPos);
                    //скрываем


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
