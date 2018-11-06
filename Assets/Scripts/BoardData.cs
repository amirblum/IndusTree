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
        
        public BoardPos Up()
        {
            return new BoardPos { x = this.x, y = this.y + 1 };
        }
        public BoardPos Down()
        {
            return new BoardPos { x = this.x, y = this.y -1 };
        }
        public BoardPos Left()
        {
            return new BoardPos { x = this.x - 1 , y = this.y };
        }
        public BoardPos Right()
        {
            return new BoardPos { x = this.x + 1, y = this.y };
        }
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
                var pos = new BoardPos { x = i, y = j };
                PlaceTile(TileData.TileType.Empty, pos);
            }
        }
    }

    public TileData GetTile(BoardPos position)
    {
        return _board[position.x, position.y];
    }

    public void PlaceTile(TileData.TileType tileType, BoardPos position)
    {
        Debug.Assert(_board[position.x, position.y] == null ||
        _board[position.x, position.y].tileType == TileData.TileType.Empty,
            "Trying to place tile on non-empty spot!");

        var newTile = new TileData() { tileType = tileType };
        _board[position.x, position.y] = newTile;

        OnPlacedTileEvent?.Invoke(newTile, position);
    }

}
