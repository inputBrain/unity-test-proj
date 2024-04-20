using System.Linq;
using Models;
using Services;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PreLoadGame : MonoBehaviour
{
    
    private void Awake()
    {
        var middleware = FindObjectOfType<GameMiddleware>();
        var countryTileData = FindObjectOfType<CountryTileData>().GetComponent<CountryTileData>();
        var tilemap = GameObject.FindWithTag("baseTilemap").GetComponent<Tilemap>();
        
        if (middleware != null)
        {
            Debug.Log(middleware.SelectedCountry);

            foreach (var country in  countryTileData.TilesDict)
            {
                Vector3Int pos1 = new Vector3Int((int)country.Key.x, (int)country.Key.y, 0);
                tilemap.SetTileFlags(pos1, TileFlags.None);
                tilemap.SetColor(pos1, country.Value.Color);
            
                var tileInfo = new CountryModel
                {
                    isOccupied = true,
                    Country = country.Value.Country,
                    Color = country.Value.Color,
                    Resources = ResourceModel.CreateEmpty()
                };

                countryTileData.TilesDict[pos1] = tileInfo;
            }
        }
    }
}
