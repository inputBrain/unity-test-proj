using System.Collections.Generic;
using Models;
using Models.Country;
using Models.Country.Сonstruction;
using Models.User;
using Newtonsoft.Json;
using Services;
using Storage;
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
        var hexagonTileStorage = gameObject.GetComponent<HexagonTileStorage>();
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
            
                var tileUnitModel = new CountryUnitModel()
                {
                    Color = country.Value.Color,
                    Name = country.Value.Country,
                    ResourceModel = new ResourceModel(),
                    CapitalUnitModel = new CapitalUnitModel()
                    {
                        isCapital = true,
                        CapitalName = country.Value.Capital
                    },
                    ConstructionModel = new ConstructionModel()
                    {
                        Level = 1,
                        ProductionType = ProductionType.Influence
                    }
                };            


                hexagonTileStorage.TilesData[position] = tileUnitModel;
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
