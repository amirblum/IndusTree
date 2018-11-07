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
        int newLevel = tileLevel + amountToRaiseTile;
        newLevel = Mathf.Min(newLevel, 2);
        RaiseTileEvent?.Invoke(this, tileLevel, newLevel);
        
        tileLevel = newLevel;
        amountToRaiseTile = 0;
    }

    internal void DestroyTile()
    {
        RaiseTileEvent?.Invoke(this, tileLevel, 0);
    }
}
