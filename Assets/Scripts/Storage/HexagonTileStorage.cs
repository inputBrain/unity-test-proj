using System;
using System.Collections.Generic;
using Models.Country;
using UnityEngine;

namespace Storage
{
    public class HexagonTileStorage : MonoBehaviour
    {
        public Dictionary<Vector3, CountryUnitModel> TilesData;

        public List<Vector3> tileCoordinateKeys = new();
        
        public List<CountryUnitModel> hexagonTileModelValues = new();

    
        public void SerializeTilesData()
        {
            TilesData = new Dictionary<Vector3, CountryUnitModel>();

            for (int i = 0; i != Math.Min(tileCoordinateKeys.Count, hexagonTileModelValues.Count); i++)
                TilesData.Add(tileCoordinateKeys[i], hexagonTileModelValues[i]);
        }
    }
}