using System.Collections.Generic;
using Const;
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
    
    private ComponentShareService ComponentShareService => FindObjectOfType<ComponentShareService>();
    
    private void Awake()
    {
        LoadCountryColorsFromJson();
        
        var middleware = ComponentShareService.GetComponentByType<GameMiddleware>();
        var hexagonTileStorage = ComponentShareService.GetComponentByType<HexagonTileStorage>();
        var baseTilemap = ComponentShareService.GetComponentByTypeAndTag<Tilemap>(Constants.BASE_TILEMAP);
        var castleTilemap =ComponentShareService.GetComponentByTypeAndTag<Tilemap>(Constants.CASTLE_TILEMAP);
        
        hexagonTileStorage.SerializeTilesData();
        
        if (middleware != null)
        {
            Debug.Log(middleware.SelectedCountry);
        
            foreach (var country in countryJson)
            {
                var position = new Vector3Int(country.Value.CapitalTilePosition.x, country.Value.CapitalTilePosition.y, 0);
                baseTilemap.SetTileFlags(position, TileFlags.None);
                baseTilemap.SetColor(position, country.Value.Color);
                
                castleTilemap.SetTile(position, castleTile);
            
                var tileUnitModel = new CountryUnitModel()
                {
                    Color = country.Value.Color,
                    Name = country.Value.Country,
                    TotalResourceModel = new TotalResourceModel(),
                    CapitalUnitModel = new CapitalUnitModel()
                    {
                        isCapital = true,
                        CapitalName = country.Value.Capital
                    },
                    ConstructionModel = new ConstructionModel()
                    {
                        Level = 1,
                        ProductionType = ProductionType.Castle
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
