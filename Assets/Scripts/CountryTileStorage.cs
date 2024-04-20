using System;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class CountryTileStorage : MonoBehaviour
{
    public Dictionary<Vector3, CountryModel> TilesData;

    public List<Vector3> tileCoordinateKeys = new();
    public List<CountryModel> countryModelValues = new();


    public void SerializeTilesData()
    {
        TilesData = new Dictionary<Vector3, CountryModel>();

        for (int i = 0; i != Math.Min(tileCoordinateKeys.Count, countryModelValues.Count); i++)
            TilesData.Add(tileCoordinateKeys[i], countryModelValues[i]);
    }
}