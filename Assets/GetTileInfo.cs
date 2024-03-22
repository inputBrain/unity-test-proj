using System;
using System.Collections.Generic;
using JetBrains.Annotations;
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
    private Dictionary<Color, string> CountryDict = new ();
        
    [Serializable]
    public class TileInfo
    {
        public string Country;
        public bool isCapital;
    }

    private void Start()
    {
        CountryDict.Add(new Color(0.101960786f, 0.545098066f, 0.443137258f, 1.000f), "USA");
        CalculateOccupiedCells();
    }

    void CalculateOccupiedCells()
    {
        Bounds spriteBounds = spriteRenderer.sprite.bounds;
        Vector3 cellSize = tilemap.cellSize;
        
        //cellSize.x *= globalCellSize;
        //cellSize.y *= globalCellSize;
        
        var spriteWidthInCells = spriteBounds.size.x / (cellSize.x * 0.8659766f);
        var spriteHeightInCells = spriteBounds.size.y / (cellSize.y * 0.8659766f);
        

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
                var color = TakeColorUnderTile(spriteBounds, tileWorldPos, spriteRenderer.sprite);
                
                var country = GetCountryByColor(color);

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
                    Debug.Log($"Create | X Y: {pos}, country: {country}, color: {color}");
                }
            }
        }
    }

    private Color TakeColorUnderTile(Bounds bounds, Vector3 position, Sprite map)
    {
        Vector2Int pixelCoord = new Vector2Int(
            (int)((position.x - bounds.min.x) / bounds.size.x * map.texture.width),
            (int)((position.y - bounds.min.y) / bounds.size.y * map.texture.height));
                
        return map.texture.GetPixel(pixelCoord.x, pixelCoord.y);
    }
    
    [CanBeNull]
    private string GetCountryByColor(Color color)
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
}
