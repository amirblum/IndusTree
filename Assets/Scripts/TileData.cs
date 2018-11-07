using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public enum TileType
    {
        OutOfBounds = -3,
        Destroyed = -2,
        Empty = -1,
        Organic = 0,
        Mechanic = 1,
    }

    public TileType tileType;
    public int tileLevel;
    public BoardData.BoardPos boardPos;
    public int amountToRaiseTile;

    public event Action<TileData, int, int> RaiseTileEvent;

    public TileData(BoardData.BoardPos boardPos)
    {
        this.boardPos = boardPos;
        tileType = TileType.Empty;
    }

    public TileData(TileType tileType)
    {
        this.tileType = tileType;
    }

    public TileData(TileType tileType, BoardData.BoardPos boardPos)
    {
        this.boardPos = boardPos;
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

    internal void DestroyTile()
    {
        RaiseTileEvent?.Invoke(this, tileLevel, 0);
    }
}
