using System.Collections.Generic;
using Models;
using Models.Capital;
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
        var hexagonTileStorage = FindObjectOfType<HexagonTileStorage>();
        var tilemap = GameObject.FindWithTag("baseTilemap").GetComponent<Tilemap>();
        var castleTilemap = GameObject.FindWithTag("castleTilemap").GetComponent<Tilemap>();
        
        hexagonTileStorage.SerializeTilesData();
        
        if (middleware != null)
        {
            Debug.Log(middleware.SelectedCountry);

            foreach (var country in countryJson)
            {
                var position = new Vector3Int(country.Value.CapitalTilePosition.x, country.Value.CapitalTilePosition.y, 0);
                tilemap.SetTileFlags(position, TileFlags.None);
                tilemap.SetColor(position, country.Value.Color);
                
                castleTilemap.SetTile(position, castleTile);
            
                // var capitalModel = new CapitalModel
                // {
                //     Country = country.Value.Country,
                //     Capital = country.Value.Capital,
                //     Level = 1,
                //     Color = country.Value.Color,
                // };            
                var capitalModel = new HexagonTileModel()
                {
                    Country = country.Value.Country,
                    Color = country.Value.Color,
                };

                hexagonTileStorage.TilesData[position] = capitalModel;
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
