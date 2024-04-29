using JetBrains.Annotations;
using Models.Country.Сonstruction;
using Models.User;
using UnityEngine;

namespace Models.Country
{
    public class CountryUnitModel
    {
        public Color32 Color { get; set; }
        
        public string Name { get; set; }
        
        [CanBeNull]
        public TotalResourceModel TotalResourceModel { get; set; }
        
        [CanBeNull]
        public CapitalUnitModel CapitalUnitModel { get; set; }
        
        [CanBeNull]
        public ConstructionModel ConstructionModel { get; set; }
    }
}