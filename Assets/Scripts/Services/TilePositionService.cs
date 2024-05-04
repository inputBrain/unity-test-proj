using Storage;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Services
{
    public class TilePositionService : Singleton<TilePositionService>
    {
        private Tilemap _tilemap;
        
        [SerializeField]
        private GameObject gridTilemap;
        
        private HexagonTileStorage _hexagonTileStorage;
        
        
        // public Vector3Int GetTileWorldPosition()
        // {
        //     
        // }
    }
}