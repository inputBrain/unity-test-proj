using System.Collections.Generic;
using System.Linq;
using Models.API.BuildIncome;
using Newtonsoft.Json;
using UnityEngine;

namespace Resource
{
    public class ResourceIncome : Singleton<ResourceIncome>
    {
        private ComponentShareService ComponentShareService => FindObjectOfType<ComponentShareService>();

        public TextAsset buildIncomeJson;
        private IncomeApiModel _castleIncome { get; set; } = new();
        private ResourcesOnTopPanelHandler _resourcesOnTopPanelHandler;


        void Awake()
        {
            _resourcesOnTopPanelHandler = ComponentShareService.GetComponentByType<ResourcesOnTopPanelHandler>();
            _loadCountryCost();
            
            InvokeRepeating(nameof(AddResources), 0f, 1f);
        }

        private void AddResources()
        {
            var woodIncome = _castleIncome.Levels[0].IncomePerMinute.GetValueOrDefault("wood");
            var stoneIncome = _castleIncome.Levels[0].IncomePerMinute.GetValueOrDefault("stone");
            var steelIncome = _castleIncome.Levels[0].IncomePerMinute.GetValueOrDefault("steel");
            var bronzeIncome = _castleIncome.Levels[0].IncomePerMinute.GetValueOrDefault("bronze");
            var silverIncome = _castleIncome.Levels[0].IncomePerMinute.GetValueOrDefault("Silver");
            var goldIncome = _castleIncome.Levels[0].IncomePerMinute.GetValueOrDefault("gold");
            var platinumIncome = _castleIncome.Levels[0].IncomePerMinute.GetValueOrDefault("platinum");
            var influenceIncome = _castleIncome.Levels[0].IncomePerMinute.GetValueOrDefault("influence");

            _resourcesOnTopPanelHandler.UpdateResourceTexts(
                woodIncome,
                stoneIncome,
                steelIncome,
                bronzeIncome,
                silverIncome,
                goldIncome,
                platinumIncome,
                influenceIncome
            );
        }

        

        private void _loadCountryCost()
        {
            var builds = JsonConvert.DeserializeObject<List<IncomeApiModel>>(buildIncomeJson.text);

            _castleIncome = builds.FirstOrDefault(x => x.Type == "castle");
        }
    }
}