using Models.Country;
using Storage;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public SpriteRenderer spriteRenderer;
    public Tile tile;
 
    private HexagonTileStorage _hexagonTileStorage;
    private ComponentShareService ComponentShareService => FindObjectOfType<ComponentShareService>();


    private void Start()
    {
        _hexagonTileStorage = ComponentShareService.GetComponentByType<HexagonTileStorage>();

        GenerateHexagons();
    }


    private void GenerateHexagons()
    {
        var whiteColor = new Color(0.925490201f, 0.925490201f, 0.925490201f, 1);
        
        var spriteBounds = spriteRenderer.sprite.bounds;
        var cellSize = tilemap.cellSize;
        
        var spriteWidthInCells = spriteBounds.size.x / (cellSize.x * 0.8659766f);
        var spriteHeightInCells = spriteBounds.size.y / (cellSize.x);
        

        var countCellsByX = Mathf.CeilToInt(spriteWidthInCells);
        var countCellsByY = Mathf.CeilToInt(spriteHeightInCells);
        
        var offsetX = countCellsByX / 2;
        var offsetY = countCellsByY / 2;
        
        Debug.Log(countCellsByX);
        Debug.Log(countCellsByY);
        
        for (var x = 0; x < countCellsByX; x++)
        {
            for (var y = 0; y < countCellsByY; y++)
            {
                var pos = new Vector3Int(y - offsetY, x - offsetX, 0);
                Vector3 tileWorldPos = tilemap.GetCellCenterWorld(pos);
                
                var isHexagonWhite = IsHexagonWhite(spriteBounds, tileWorldPos, whiteColor);
        
                if (isHexagonWhite)
                {
                    var tileInfo = new CountryUnitModel()
                    {
                        Name = ""
                    };
                    
                    tilemap.SetTile(pos, tile);
                
                    const float spaceBetweenHexagons = 0.05f; // 5%
                    var scaledSizeX = (cellSize.x / 0.86f) * (1f - spaceBetweenHexagons);
                    var scaledSizeY = cellSize.y * (1f - spaceBetweenHexagons);
                    
                    tilemap.SetTransformMatrix(pos, Matrix4x4.Scale(new Vector3(scaledSizeX, scaledSizeY, 1)));
                    
                    _hexagonTileStorage.tileCoordinateKeys.Add(pos);
                    _hexagonTileStorage.hexagonTileModelValues.Add(tileInfo);
                }
            }
        }
    } 
    

    private bool IsHexagonWhite(Bounds spriteBounds, Vector3 tileWorldPos, Color whiteColor)
    {
        var pixelCoord = WorldToPixelCoords(spriteBounds, tileWorldPos, spriteRenderer.sprite);
        var colorTile = spriteRenderer.sprite.texture.GetPixel(pixelCoord.x, pixelCoord.y);
        
        if (colorTile != Color.clear)
        {
            return colorTile == whiteColor;

        }
        return false;
    }


    private Vector2Int WorldToPixelCoords(Bounds bounds, Vector3 position, Sprite map)
    {
        return new Vector2Int(
            Mathf.Clamp(Mathf.RoundToInt((position.x - bounds.min.x) / bounds.size.x * map.texture.width), 0, map.texture.width - 1),
            Mathf.Clamp(Mathf.RoundToInt((position.y - bounds.min.y) / bounds.size.y * map.texture.height), 0, map.texture.height - 1)
        );
    }
    
    
    public Vector3 FindCountryTile(string countryName)
    {
        foreach (var tilePos in _hexagonTileStorage.TilesData.Keys)
        {
            if (_hexagonTileStorage.TilesData[tilePos].Name == countryName)
            {
                return tilePos;
            }
        }
        return Vector3.zero; 
    }
}
