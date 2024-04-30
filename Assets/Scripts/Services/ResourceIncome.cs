using System.Collections.Generic;
using System.Linq;
using Models.API.BuildIncome;
using Newtonsoft.Json;
using UnityEngine;
using User;

namespace Services
{
    public class ResourceIncome : Singleton<ResourceIncome>
    {
        public TextAsset buildIncomeJson;
        private IncomeApiModel _castleIncome { get; set; } = new();
        private ResourcesOnTopPanelHandler _resourcesOnTopPanelHandler;


        void Awake()
        {
            _resourcesOnTopPanelHandler = FindObjectOfType<ResourcesOnTopPanelHandler>();
            _loadCountryCost();
            
            InvokeRepeating(nameof(AddResources), 0f, 1f);
        }

        private void AddResources()
        {
            var woodIncome = _castleIncome.Levels[0].level_1.IncomePerMinute.GetValueOrDefault("wood");
            var stoneIncome = _castleIncome.Levels[0].level_1.IncomePerMinute.GetValueOrDefault("stone");
            var steelIncome = _castleIncome.Levels[0].level_1.IncomePerMinute.GetValueOrDefault("steel");
            var bronzeIncome = _castleIncome.Levels[0].level_1.IncomePerMinute.GetValueOrDefault("bronze");
            var silverIncome = _castleIncome.Levels[0].level_1.IncomePerMinute.GetValueOrDefault("Silver");
            var goldIncome = _castleIncome.Levels[0].level_1.IncomePerMinute.GetValueOrDefault("gold");
            var platinumIncome = _castleIncome.Levels[0].level_1.IncomePerMinute.GetValueOrDefault("platinum");
            var influenceIncome = _castleIncome.Levels[0].level_1.IncomePerMinute.GetValueOrDefault("influence");

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