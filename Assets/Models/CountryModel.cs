using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Models
{
    [Serializable]
    public class CountryModel
    {
        [CanBeNull]
        public string Country;
        public bool? isCapital;
        public bool? isOccupied;
        public ResourceModel Resources { get; set; }
    }
}