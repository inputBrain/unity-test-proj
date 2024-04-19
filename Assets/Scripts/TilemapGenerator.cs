using System.Collections.Generic;
using System.Linq;
using Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap castleTilemap;
    public SpriteRenderer spriteRenderer;
    public Tile tile;
    public Tile castleTile;
    private readonly Dictionary<Color32, CountryJsonModel> CountryDict = new();
    private CountryTileData _countryTileData;
    public TextAsset countriesJson;
    
    
    private void Start()
    {
        _countryTileData = GetComponent<CountryTileData>();
        LoadCountryColorsFromJson();
   
        GenerateHexagons();
    }
    
    void LoadCountryColorsFromJson()
    {
        var countries = JsonConvert.DeserializeObject<List<CountryJsonModel>>(countriesJson.text);
        
        foreach (var country in countries)
        {
            CountryDict.Add(country.Color, country);
        }
    }

    private void GenerateHexagons()
    {
        var whiteColor = new Color(0.925490201f, 0.925490201f, 0.925490201f, 1);
        
        Bounds spriteBounds = spriteRenderer.sprite.bounds;
        Vector3 cellSize = tilemap.cellSize;
        
        var spriteWidthInCells = spriteBounds.size.x / (cellSize.x * 0.8659766f);
        var spriteHeightInCells = spriteBounds.size.y / (cellSize.x);
        

        int countCellsByX = Mathf.CeilToInt(spriteWidthInCells);
        var countCellsByY = Mathf.CeilToInt(spriteHeightInCells);
        
        var offsetX = countCellsByX / 2;
        var offsetY = countCellsByY / 2;
        
        Debug.Log(countCellsByX);
        Debug.Log(countCellsByY);
        
        for (int x = 0; x < countCellsByX; x++)
        {
            for (int y = 0; y < countCellsByY; y++)
            {
                var pos = new Vector3Int(y - offsetY, x - offsetX, 0);
                Vector3 tileWorldPos = tilemap.GetCellCenterWorld(pos);
                
                var isHexagonWhite = IsHexagonWhite(spriteBounds, tileWorldPos, whiteColor);
        
                if (isHexagonWhite)
                {
                    var tileInfo = new CountryModel
                    {
                        isOccupied = false,
                        isCapital = false,
                        Country = string.Empty,
                        Resources = ResourceModel.CreateEmpty()
                    };
                    
                    tilemap.SetTile(pos, tile);
                
                    const float spaceBetweenHexagons = 0.05f; // 5%
                    var scaledSizeX = (cellSize.x / 0.86f) * (1f - spaceBetweenHexagons);
                    var scaledSizeY = cellSize.y * (1f - spaceBetweenHexagons);
                    
                    tilemap.SetTransformMatrix(pos, Matrix4x4.Scale(new Vector3(scaledSizeX, scaledSizeY, 1)));
                    _countryTileData.TilesDict.Add(pos, tileInfo);
                }
            }
        }

        var countries = CountryDict.Select(x => x.Value).ToList();

        foreach (var country in countries)
        {
            Vector3Int pos1 = new Vector3Int(country.CapitalTilePosition.x, country.CapitalTilePosition.y, 0);
            tilemap.SetTileFlags(pos1, TileFlags.None);
            tilemap.SetColor(pos1, country.Color);
            castleTilemap.SetTile(pos1, castleTile);
            
            var tileInfo = new CountryModel
            {
                isOccupied = true,
                Country = country.Country,
                Resources = ResourceModel.CreateEmpty()
            };

            _countryTileData.TilesDict[pos1] = tileInfo;
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
        foreach (var tilePos in _countryTileData.TilesDict.Keys)
        {
            if (_countryTileData.TilesDict[tilePos].Country == countryName)
            {
                return tilePos;
            }
        }
        return Vector3.zero; 
    }
}
