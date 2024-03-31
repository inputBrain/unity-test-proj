using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GetTileBorderColor : MonoBehaviour
{
    public Tilemap tilemap;
    public SpriteRenderer imageRenderer;
    public List<Color32> recognitionColors;

    public float sizeX;
    public float sizeY;

    public float offsetX;
    public float offsetY;

    private List<Color32> GetColorsUnderTileBorders(Vector3 tilePosition, BoundsInt tileBounds)
    {
        List<Color32> colors = new List<Color32>();
        
        var cellSize = tilemap.layoutGrid.cellSize;
        float halfWidth = (tileBounds.size.x / sizeX) * cellSize.y;
        float halfHeight = (tileBounds.size.y / sizeY) * cellSize.x;

        // Calculate border positions for a hexagon
        Vector3[] borderPositions = new Vector3[6]; // Hexagon has 6 corners
        float angleIncrement = 60f; // Angle between corners in a hexagon
        for (int i = 0; i < 6; i++)
        {
            float angle = i * angleIncrement * Mathf.Deg2Rad;
            float xOffset = -halfWidth * Mathf.Cos(angle) - (halfWidth / offsetX);
            float yOffset = -halfHeight * Mathf.Sin(angle)- (halfHeight / offsetY);
            borderPositions[i] = tilePosition + new Vector3(xOffset, yOffset, 0);
        }
        
        foreach (var borderPosition in borderPositions)
        {
            Debug.DrawLine(borderPosition, Vector3.forward, Color.magenta, 3f);
            Vector2Int pixelCoord = WorldToPixelCoords(borderPosition);
            Color32 color = imageRenderer.sprite.texture.GetPixel(pixelCoord.x, pixelCoord.y);
            colors.Add(color);
        }

        recognitionColors = colors;

        return colors;
    }
    

    private Vector2Int WorldToPixelCoords(Vector3 worldPosition)
    {
        // Преобразование мировых координат в координаты пикселей на изображении
        Vector3 localPosition = imageRenderer.transform.InverseTransformPoint(worldPosition);
        
        var sprite = imageRenderer.sprite;
        
        Vector2 spriteCoord = new Vector2(
            localPosition.x / sprite.bounds.size.x + 0.5f,
            localPosition.y / sprite.bounds.size.y + 0.5f);

        // Перевод нормализованных координат в координаты пикселей на текстуре спрайта
        Vector2Int pixelCoord = new Vector2Int(
            Mathf.RoundToInt(spriteCoord.x * sprite.texture.width),
            Mathf.RoundToInt(spriteCoord.y * sprite.texture.height));

        return pixelCoord;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Получение мировых координат курсора
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Получение координат сетки тайлов из мировых координат курсора
            Vector3Int gridPosition = tilemap.WorldToCell(mouseWorldPosition);

            // Получение позиции и размеров тайла
            Vector3 tilePosition = tilemap.GetCellCenterWorld(gridPosition);
            var tileBounds = tilemap.cellBounds;

            // Получение цветов изображения под границами тайла
            List<Color32> colorsUnderBorders = GetColorsUnderTileBorders(tilePosition, tileBounds);

            // Вывод цветов в консоль
            foreach (Color32 color in colorsUnderBorders)
            {
                Debug.Log("Color under hex tile border: " + color);
            }
        }
    }
}