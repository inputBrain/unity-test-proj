using System.Collections.Generic;
using JetBrains.Annotations;
using Models;
using Newtonsoft.Json;
using Services;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public SpriteRenderer spriteRenderer;
    public Tile tile;
    private readonly Dictionary<Color32, string> CountryDict = new Dictionary<Color32, string>();
    private CountryTileData _countryTileData;
    public TextAsset countriesJson;
    
    
    private void Start()
    {
        _countryTileData = GetComponent<CountryTileData>();
        // CountryDict.Add(new Color32(26, 139, 113, 255), "USA");
        // CountryDict.Add(new Color32(189, 215, 61, 255), "CANADA");
        LoadCountryColorsFromJson();
   
        GenerateHexagons();
    }
    
    void LoadCountryColorsFromJson()
    {
        var countries = JsonConvert.DeserializeObject<List<CountryJsonModel>>(countriesJson.text);
        
        foreach (var country in countries)
        {
            CountryDict.Add(country.Color, country.Country);
        }
    }

    private void GenerateHexagons()
    {
        Bounds spriteBounds = spriteRenderer.sprite.bounds;
        Vector3 cellSize = tilemap.cellSize;
        
        var spriteWidthInCells = spriteBounds.size.x / (cellSize.x * cellSize.x);
        var spriteHeightInCells = spriteBounds.size.y / (cellSize.y * cellSize.x);
        

        int countCellsByX = Mathf.CeilToInt(spriteWidthInCells);
        var countCellsByY = Mathf.CeilToInt(spriteHeightInCells);
        
        var offsetX = countCellsByX / 2;
        var offsetY = countCellsByY / 2;
        
        for (int x = 0; x < countCellsByX; x++)
        {
            for (int y = 0; y < countCellsByY; y++)
            {
                var pos = new Vector3Int(y - offsetY, x - offsetX, 0);
                Vector3 tileWorldPos = tilemap.GetCellCenterWorld(pos);
                
                var country = TryGetCountry(spriteBounds, tileWorldPos);

                if (country != null)
                {
                    var tileInfo = new TileInfoModel
                    {
                        Country = country,
                        isCapital = false,
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
    }
    
    private string TryGetCountry(Bounds spriteBounds, Vector3 tileWorldPos)
    {
        var color = TakeColorUnderTile(spriteBounds, tileWorldPos, spriteRenderer.sprite);

        var emptyColor = new Color32(0, 0, 0, 0);
        if (color != emptyColor)
        { 
            var country = GetCountryByColor(color);
            if (country != null)
                return country;
        }
        return null;
    }
    

    private Color TakeColorUnderTile(Bounds bounds, Vector3 position, Sprite map)
    {
        var pixelCoord = WorldToPixelCoords(bounds, position, map);
        return map.texture.GetPixel(pixelCoord.x, pixelCoord.y);
    }
    

    private Vector2Int WorldToPixelCoords(Bounds bounds, Vector3 position, Sprite map)
    {
        return new Vector2Int(
            Mathf.Clamp(Mathf.RoundToInt((position.x - bounds.min.x) / bounds.size.x * map.texture.width), 0, map.texture.width - 1),
            Mathf.Clamp(Mathf.RoundToInt((position.y - bounds.min.y) / bounds.size.y * map.texture.height), 0, map.texture.height - 1)
        );
    }
    
    [CanBeNull]
    private string GetCountryByColor(Color32 color)
    {
         CountryDict.TryGetValue(color, out var country);
         return country;
    }
}
