using System;
using System.Collections.Generic;
using Models;
using Models.Capital;
using UnityEngine;

public class HexagonTileStorage : MonoBehaviour
{
    public Dictionary<Vector3, HexagonTileModel> TilesData;

    public List<Vector3> tileCoordinateKeys = new();
    public List<HexagonTileModel> hexagonTileModelValues = new();

    
    public void SerializeTilesData()
    {
        TilesData = new Dictionary<Vector3, HexagonTileModel>();

        for (int i = 0; i != Math.Min(tileCoordinateKeys.Count, hexagonTileModelValues.Count); i++)
            TilesData.Add(tileCoordinateKeys[i], hexagonTileModelValues[i]);
    }

    
        
    
    // public Dictionary<Vector3, CapitalModel> Capitals;
    //
    // public List<Vector3> capitalKeys = new();
    // public List<CapitalModel> сapitalValues = new();
    //
    //
    // public void SerializeCountryData()
    // {
    //     Capitals = new Dictionary<Vector3, CapitalModel>();
    //
    //     for (int i = 0; i != Math.Min(capitalKeys.Count, сapitalValues.Count); i++)
    //         Capitals.Add(capitalKeys[i], сapitalValues[i]);
    // }
}