using UnityEngine;

namespace Models.Country.Сonstruction
{
    public class ConstructionModel
    {
        [field: Range(1, 5)]
        public int Level { get; set; }
        
        public ProductionType ProductionType { get; set; }
    }
}