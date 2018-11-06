using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public enum TileType
    {
        OutOfBounds = -2,
        Empty = -1,
        Organic = 0,
        Mechanic = 1
    }

    public TileType tileType;
    public int tileLevel;
    public int amountToRaiseTile;

    public event Action<TileData, int, int> RaiseTileEvent;

    public TileData()
    {
        tileType = TileType.Empty;
    }

    public TileData(TileType tileType)
    {
        this.tileType = tileType;
    }

    public void RaiseTile()
    {
        if (amountToRaiseTile == 0)
        {
            return;
        }
        RaiseTileEvent?.Invoke(this, tileLevel, tileLevel + amountToRaiseTile);
        tileLevel += amountToRaiseTile;
        amountToRaiseTile = 0;
    }
}
