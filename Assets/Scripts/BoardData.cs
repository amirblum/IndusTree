using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardData
{
    public struct BoardPos
    {
        public int x;
        public int y;
    }

    // Public interface
    public int BoardSize { get; private set; }
    public int NumOfPlayers { get; private set; }
    public static BoardData Instance { get; private set; }

    // Public events
    public event Action<TileData, BoardPos> OnPlacedTileEvent;

    // Private variables
    private TileData[,] _board;

    public BoardData()
    {
        Instance = this;
    }

    public void InitializeBoard(int boardSize, int numOfPlayers)
    {
        BoardSize = boardSize;
        NumOfPlayers = numOfPlayers;

        _board = new TileData[BoardSize, BoardSize];
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                var tile = new TileData();
                var pos = new BoardPos { x = i, y = j };
                PlaceTile(tile, pos);
            }
        }
    }

    public TileData GetTile(BoardPos position)
    {
        return _board[position.x, position.y];
    }

    public void PlaceTile(TileData tile, BoardPos position)
    {
        Debug.Assert(_board[position.x, position.y].tileType == TileData.TileType.Empty, 
            "Trying to place tile on non-empty spot!");

        _board[position.x, position.y] = tile;

        OnPlacedTileEvent?.Invoke(tile, position);
    }

}
