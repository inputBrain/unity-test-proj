using System.Collections.Generic;
using Storage;
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

        private BuildUpgradeMenu _buildUpgradeMenu;
        
        
        private void Start()
        {
            _tilemap = GetComponent<Tilemap>();
            _buildUpgradeMenu = gridTilemap.GetComponent<BuildUpgradeMenu>();
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
                    Debug.Log($"Country: {tileInfo.Name} Wood: {tileInfo.ResourceModel!.Wood}");
                    //отрисовка

                    if (string.IsNullOrWhiteSpace(tileInfo.Name) == false)
                    {
                        _buildUpgradeMenu.IsEnabledPanel(true);
                    }
                }
                else
                {
                    Debug.Log("No tile found at position: " + gridPos);
                    //скрываем
                    _buildUpgradeMenu.IsEnabledPanel(false);


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
