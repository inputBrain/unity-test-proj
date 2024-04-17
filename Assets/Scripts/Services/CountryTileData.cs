using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Services
{
    public class CountryTileData : MonoBehaviour
    {
        public readonly Dictionary<Vector3, TileInfoModel> TilesDict = new ();
        public readonly Dictionary<Vector3Int, string> CapitalsDict = new ();
    }
}