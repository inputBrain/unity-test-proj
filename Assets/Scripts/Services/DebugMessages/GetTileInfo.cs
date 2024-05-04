using System.Collections.Generic;
using Const;
using Storage;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Services.DebugMessages
{
    public class GetTileInfo : MonoBehaviour
    {
        private Tilemap _tilemap;

        private Camera _camera;

        private HexagonTileStorage _hexagonTileStorage;

        private BuildUpgradeMenu _buildUpgradeMenu;

        [SerializeField] public Vector3Int gridPos;

        public bool isGetInfo;

        private ComponentShareService ComponentShareService => FindObjectOfType<ComponentShareService>();
        
        private void Start()
        {
            _camera = ComponentShareService.GetComponentByTypeAndTag<Camera>(Constants.MAIN_CAMERA);
            _tilemap = ComponentShareService.GetComponentByTypeAndTag<Tilemap>(Constants.BASE_TILEMAP);
            _buildUpgradeMenu = ComponentShareService.GetComponentByType<BuildUpgradeMenu>();
            _hexagonTileStorage = ComponentShareService.GetComponentByType<HexagonTileStorage>();
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                var hit = Physics2D.GetRayIntersection(ray);
                var hitPosition = hit.point;

                gridPos = _tilemap.WorldToCell(hitPosition);

                if (isGetInfo)
                {
                    if (_hexagonTileStorage.TilesData.TryGetValue(gridPos, out var tileInfo))
                    {
                        Debug.Log($"Country: {tileInfo.Name} Wood: {tileInfo.TotalResourceModel!.Wood}");

                        if (string.IsNullOrWhiteSpace(tileInfo.Name) == false)
                        {
                            _buildUpgradeMenu.IsEnabledPanel(true);
                        }
                    }
                    else
                    {
                        Debug.Log("No tile found at position: " + gridPos);
                        _buildUpgradeMenu.IsEnabledPanel(false);
                    }
                }
            }
        }
    }
}