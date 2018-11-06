using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType
    {
        Empty = -1,
        Organic = 0,
        Mechanic = 1,
    }

    public TileType tileType;
}
