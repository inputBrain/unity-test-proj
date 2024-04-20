using System.Collections.Generic;
using System.Linq;
using Models;
using Newtonsoft.Json;
using Services;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PreLoadGame : MonoBehaviour
{
    
    private readonly Dictionary<Color32, CountryJsonModel> countryJson = new();
    public TextAsset countriesJson;
    public Tile castleTile;
    
    private void Awake()
    {
        LoadCountryColorsFromJson();
        
        var middleware = FindObjectOfType<GameMiddleware>();
        var countryTileData = FindObjectOfType<CountryTileStorage>();
        var tilemap = GameObject.FindWithTag("baseTilemap").GetComponent<Tilemap>();
        var castleTilemap = GameObject.FindWithTag("castleTilemap").GetComponent<Tilemap>();
        
        countryTileData.SerializeTilesData();
        
        foreach (var country in countryTileData.TilesData)
        {
            country.Value.Resources = ResourceModel.CreateEmpty();
        }
        if (middleware != null)
        {
            Debug.Log(middleware.SelectedCountry);

            foreach (var country in countryJson)
            {
                Vector3Int pos1 = new Vector3Int(country.Value.CapitalTilePosition.x, country.Value.CapitalTilePosition.y, 0);
                tilemap.SetTileFlags(pos1, TileFlags.None);
                tilemap.SetColor(pos1, country.Value.Color);
                
                castleTilemap.SetTile(pos1, castleTile);
            
                var tileInfo = new CountryModel
                {
                    isOccupied = true,
                    isCapital = true,
                    Country = country.Value.Country,
                    Color = country.Value.Color,
                    Resources = ResourceModel.CreateBeginResourcesForCastle()
                };

                countryTileData.TilesData[pos1] = tileInfo;
            }
        }

      
    }
    
    void LoadCountryColorsFromJson()
    {
        var countries = JsonConvert.DeserializeObject<List<CountryJsonModel>>(countriesJson.text);
        
        foreach (var country in countries)
        {
            countryJson.Add(country.Color, country);
        }
    }
}
