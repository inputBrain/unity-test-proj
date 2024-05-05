using System;
using System.Linq;
using Models.Country.Сonstruction;
using Storage;
using UnityEngine;

public class IncomeManager : Singleton<IncomeManager>
{
    public ConstructionModel Construction;
    private HexagonTileStorage _hexagonTileStorage;

    private ComponentShareService ComponentShareService => FindObjectOfType<ComponentShareService>();
    
    
    void Awake()
    {
        _hexagonTileStorage = ComponentShareService.GetComponentByType<HexagonTileStorage>();
    }


    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            var countries = _hexagonTileStorage.TilesData.Values.Where(x => x.ConstructionModel != null && x.TotalResourceModel != null);

            foreach (var country in countries)
            {
                switch (country.ConstructionModel?.ProductionType)
                {
                    case ProductionType.Sawmill:

                        country.TotalResourceModel!.Wood += 10 * country.ConstructionModel.Level;

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
            
    }
}