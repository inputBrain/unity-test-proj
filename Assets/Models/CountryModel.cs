using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class CountryModel
    {
        [CanBeNull]
        public string Country;
        public Color32 Color;
        [SerializeField]
        public bool isCapital;
        public bool isOccupied;
        public IncomeManager Resources { get; set; }
    }
}