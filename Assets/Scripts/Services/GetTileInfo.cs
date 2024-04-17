using UnityEngine;
using UnityEngine.Tilemaps;

namespace Services
{
    public class GetTileInfo : MonoBehaviour
    {
        private Tilemap _tilemap;
        
        [SerializeField]
        private GameObject gridTilemap;
        
        private CountryTileData _countryTileData;
        
        
        private void Start()
        {
            _tilemap = GetComponent<Tilemap>();
            _countryTileData = gridTilemap.GetComponent<CountryTileData>();
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = Physics2D.GetRayIntersection(ray);
                var hitPosition = hit.point;
                var gridPos = _tilemap.WorldToCell(hitPosition);
            
                if (_countryTileData.CapitalsDict.TryGetValue(gridPos, out var countryName))
                {
                    Debug.Log($"{countryName}");
                }
                else
                {
                    Debug.Log("No tile found at position: " + gridPos);
                }
            }
        }
    }
}
