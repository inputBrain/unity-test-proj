using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GetTileInfo : MonoBehaviour
{
    public Tilemap tilemap;
    public Camera camera;
    public SpriteRenderer spriteRenderer;
    public Tile tile;
    public float globalCellSize;
    
    private Dictionary<Vector3, TileInfo> TilesDict = new ();

    private readonly Dictionary<Color32, string> CountryDict = new Dictionary<Color32, string>();
    
    [SerializeField]
    public TextAsset countriesJson;
  
    [Serializable]
    public class TileInfo
    {
        public string Country;
        public bool isCapital;
    }

    private void Start()
    {
        LoadCountryColorsFromJson();
   
        CalculateOccupiedCells();
    }

    void CalculateOccupiedCells()
    {
        Bounds spriteBounds = spriteRenderer.sprite.bounds;
        Vector3 cellSize = tilemap.cellSize;
        
        //cellSize.x *= globalCellSize;
        //cellSize.y *= globalCellSize;
        
        //TODO:
        // var spriteWidthInCells = spriteBounds.size.x / (cellSize.x * 0.8659766f);
        // var spriteHeightInCells = spriteBounds.size.y / (cellSize.y * 0.8659766f);
        var spriteWidthInCells = spriteBounds.size.x / (cellSize.x * 0.1262882f);
        var spriteHeightInCells = spriteBounds.size.y / (cellSize.y * 0.1458333f);
        

        int countCellsByX = Mathf.CeilToInt(spriteWidthInCells);
        var countCellsByY = Mathf.CeilToInt(spriteHeightInCells);

        Debug.Log("Количество занятых клеток по X: " + countCellsByX);
        Debug.Log("Количество занятых клеток по Y: " + countCellsByY);
        
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
                    var tileInfo = new TileInfo
                    {
                        Country = country,
                        isCapital = false,
                    };
                    
                    tilemap.SetTile(pos, tile);
                
                    float reductionScalePercentage = 0.05f; // 5%
                    float scaledSizeX = (cellSize.x / 0.86f) * (1f - reductionScalePercentage);
                    float scaledSizeY = cellSize.y * (1f - reductionScalePercentage);
                    
                    tilemap.SetTransformMatrix(pos, Matrix4x4.Scale(new Vector3(scaledSizeX, scaledSizeY, 1)));
                    TilesDict.Add(pos, tileInfo);
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

            var borderColors = TakeColorsUnderTileByBorders(tileWorldPos, spriteRenderer.sprite);
                
            if(borderColors.Count > 0)
            {
                Color32 mostFreqColor = GetMostFrequentColor(borderColors);

                   country = GetCountryByColor(mostFreqColor);
                return country;
            }
        }

        return null;
    }
    
    private Color GetMostFrequentColor(List<Color32> colors)
    {
        Dictionary<Color32, int> colorCounts = new Dictionary<Color32, int>();

        // Подсчитываем количество вхождений каждого цвета
        foreach (Color color in colors)
        {
            if (!colorCounts.TryAdd(color, 1))
            {
                colorCounts[color]++;
            }
        }

        // Находим максимальное количество вхождений
        int maxCount = colorCounts.Values.Max();

        // Получаем все цвета с максимальным количеством вхождений
        var mostFrequentColors = colorCounts.Where(kvp => kvp.Value == maxCount)
            .Select(kvp => kvp.Key).ToList();

        // Если есть несколько цветов с максимальным количеством вхождений, выбираем случайный из них
        Color mostFrequentColor;
        if (mostFrequentColors.Count > 1)
        {
            mostFrequentColor = mostFrequentColors[UnityEngine.Random.Range(0, mostFrequentColors.Count)];
        }
        else
        {
            mostFrequentColor = mostFrequentColors.FirstOrDefault();
        }

        return mostFrequentColor;
    }
    

    private Color TakeColorUnderTile(Bounds bounds, Vector3 position, Sprite map)
    {
        var pixelCoord = WorldToPixelCoords(bounds, position, map);
        return map.texture.GetPixel(pixelCoord.x, pixelCoord.y);
    }
    
    private List<Color32> TakeColorsUnderTileByBorders(Vector3 position, Sprite map)
    {
        List<Color32> colors = new List<Color32>();
        
        // Рассчитываем координаты пикселей для каждой из шести граней
        float halfWidth = position.x / 2f;
        float halfHeight = position.y / 2f;

        Vector3[] directions = new Vector3[]
        {
            new Vector3(-halfWidth, 0f, 0f),
            new Vector3(-halfWidth / 2f, halfHeight * Mathf.Sqrt(3) / 2f, 0f),
            new Vector3(halfWidth / 2f, halfHeight * Mathf.Sqrt(3) / 2f, 0f),
            new Vector3(halfWidth, 0f, 0f),
            new Vector3(halfWidth / 2f, -halfHeight * Mathf.Sqrt(3) / 2f, 0f),
            new Vector3(-halfWidth / 2f, -halfHeight * Mathf.Sqrt(3) / 2f, 0f)
        };

        for (int i = 0; i < 6; i++)
        {
            var color = map.texture.GetPixel((int)directions[i].x, (int)directions[i].y);
            if (color != new Color32(0, 0, 0, 0))
            {
                colors.Add(color);
            }
        }

        return colors;
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.GetRayIntersection(ray);
            var hitPosition = hit.point;
            var gridPos = tilemap.WorldToCell(hitPosition);
            
            if (TilesDict.TryGetValue(gridPos, out var tileInfo))
            {
                Debug.Log($"Tile position: {gridPos}, Country: {tileInfo.Country}, isCapital: {tileInfo.isCapital}");
            }
            else
            {
                Debug.Log("No tile found at position: " + gridPos);
            }
        }
    }
    void LoadCountryColorsFromJson()
    {
        List<CountryJsonModel> countries = JsonConvert.DeserializeObject<List<CountryJsonModel>>(countriesJson.text);
        
        foreach (var country in countries)
        {
            CountryDict.Add(country.Color, country.Country);
        }
    }
}
