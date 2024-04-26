using JetBrains.Annotations;
using Models.Сonstruction;
using UnityEngine;

namespace Models.Country
{
    public class CountryUnitModel
    {
        public Color32 Color { get; set; }
        
        public string Name { get; set; }
        
        [CanBeNull]
        public ResourceModel ResourceModel { get; set; }
        
        [CanBeNull]
        public CapitalUnitModel CapitalUnitModel { get; set; }
        
        [CanBeNull]
        public ConstructionModel ConstructionModel { get; set; }
    }
}