using System.Collections.Generic;
using System.Linq;
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
    private Camera _camera;
    private readonly Dictionary<Color32, CountryJsonModel> _countryJson = new();

    private GameMiddleware _middleware;
    private HexagonTileStorage _hexagonTileStorage;
    private Tilemap _baseTilemap;
    
    public TextAsset countriesJson;
    public Tile castleTile;
    
    private ComponentShareService ComponentShareService => FindObjectOfType<ComponentShareService>();
    
    private void Start()
    {
        LoadCountryColorsFromJson();
        
        _middleware = ComponentShareService.GetComponentByType<GameMiddleware>();
        _hexagonTileStorage = ComponentShareService.GetComponentByType<HexagonTileStorage>();
        _baseTilemap = ComponentShareService.GetComponentByTypeAndTag<Tilemap>(Constants.BASE_TILEMAP);
        _camera = ComponentShareService.GetComponentByTypeAndTag<Camera>(Constants.MAIN_CAMERA);
        
        _hexagonTileStorage.SerializeTilesData();
         InitCameraPosition();
       

        if (_middleware != null)
        {
            Debug.Log(_middleware.SelectedCountry);
        
            foreach (var country in _countryJson)
            {
                var position = new Vector3Int(country.Value.CapitalTilePosition.x, country.Value.CapitalTilePosition.y, 0);
                _baseTilemap.SetTileFlags(position, TileFlags.None);
                _baseTilemap.SetColor(position, country.Value.Color);
                
                _baseTilemap.SetTile(new Vector3Int(position.x, position.y, 1), castleTile);
            
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
        
        
                _hexagonTileStorage.TilesData[position] = tileUnitModel;
            }
        }
    }
    private void InitCameraPosition()
    {
        var userCountry = _hexagonTileStorage.TilesData.FirstOrDefault(x => x.Value.Name == _middleware.SelectedCountry);

        var tileWorldPosition = _baseTilemap.GetCellCenterWorld(new Vector3Int((int)userCountry.Key.x, (int)userCountry.Key.y));

        var targetPosition = tileWorldPosition;
        targetPosition.z = _camera.transform.position.z; 

        _camera.transform.position = targetPosition;
    }

    void LoadCountryColorsFromJson()
    {
        var countries = JsonConvert.DeserializeObject<List<CountryJsonModel>>(countriesJson.text);
        
        foreach (var country in countries)
        {
            _countryJson.Add(country.Color, country);
        }
    }
}
