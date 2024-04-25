using UnityEngine;

namespace Models.Capital
{
    public class CapitalModel
    {
        public string Country { get; set; }
        
        public string Capital { get; set; }
        
        [field: Range(1, 5)]
        public int Level { get; set; }
        
        public Color32 Color { get; set; }
    }
}