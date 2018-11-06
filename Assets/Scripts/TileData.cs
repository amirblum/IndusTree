using System;
using System.Collections;
using System.Collections.Generic;

public class TileData
{
    public enum TileType
    {
        Empty,
        Organic,
        Mechanic,
        OutOfBounds
    }

    public TileType tileType;
    public int tileLevel;
    public int amountToRaiseTile;

    public event Action<int, int> RaiseTileEvent;

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
        RaiseTileEvent?.Invoke(tileLevel, tileLevel + amountToRaiseTile);
        tileLevel += amountToRaiseTile;
        amountToRaiseTile = 0;
    }
}
