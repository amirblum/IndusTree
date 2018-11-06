﻿using System;
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
            return new BoardPos { x = this.x, y = this.y - 1 };
        }
        public BoardPos Left()
        {
            return new BoardPos { x = this.x - 1, y = this.y };
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
    private TileData _outOfBoundsTile = new TileData(TileData.TileType.OutOfBounds);

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
                _board[pos.x, pos.y] = tile;
                OnPlacedTileEvent?.Invoke(tile, pos);
            }
        }
    }
    public TileData GetTile(BoardPos position)
    {
        if (position.x < 0 || position.x >= BoardSize || position.y < 0 || position.y >= BoardSize)
        {
            return _outOfBoundsTile;
        }
        return _board[position.x, position.y];
    }

    public void PlaceTile(TileData newTile, BoardPos newTilePosition)
    {
        Debug.Assert(_board[newTilePosition.x, newTilePosition.y].tileType == TileData.TileType.Empty,
            "Trying to place tile on non-empty spot!");

        // Place the tile on the board.
        _board[newTilePosition.x, newTilePosition.y] = newTile;
        OnPlacedTileEvent?.Invoke(newTile, newTilePosition);

        // Check the quads.
        var quads = GetQuads(newTilePosition);

        foreach (var quad in quads)
        {
            if (QuadIsClosed(quad))
            {
                // Raise the quad
                foreach (var tile in quad)
                {
                    tile.amountToRaiseTile++;
                }
            }
        }

        var niner = GetNiner(newTilePosition);

        foreach (var tile in niner)
        {
            tile.RaiseTile();    
        }
    }

    private TileData[][] GetQuads(BoardPos newTilePos)
    {
        var quads = new TileData[4][];
        quads[0] = GetQuad(newTilePos);
        quads[0] = GetQuad(newTilePos.Left());
        quads[0] = GetQuad(newTilePos.Left().Down());
        quads[0] = GetQuad(newTilePos.Down());
        return quads;
    }

    private TileData[] GetQuad(BoardPos quadBottomLeft)
    {
        var quad = new TileData[4];
        quad[0] = GetTile(quadBottomLeft);
        quad[1] = GetTile(quadBottomLeft.Right());
        quad[2] = GetTile(quadBottomLeft.Up());
        quad[3] = GetTile(quadBottomLeft.Up().Right());
        return quad;
    }

    private bool QuadIsClosed(TileData[] quad)
    {
        int i = 0;
        int j = 0;

        // Bottom Left
        foreach (var tile in quad)
        {
            switch (tile.tileType)
            {
                case TileData.TileType.Organic:
                    i++;
                    break;
                case TileData.TileType.Mechanic:
                    j++;
                    break;
            }
        }

        return (i == 3 && j == 1) || (i == 1 && j == 3);
    }

    private TileData[] GetNiner(BoardPos centerPosition)
    {
        var niner = new TileData[9];

        niner[0] = GetTile(centerPosition.Down().Left());
        niner[1] = GetTile(centerPosition.Down());
        niner[2] = GetTile(centerPosition.Down().Right());
        niner[3] = GetTile(centerPosition.Left());
        niner[4] = GetTile(centerPosition);
        niner[5] = GetTile(centerPosition.Right());
        niner[6] = GetTile(centerPosition.Up().Left());
        niner[7] = GetTile(centerPosition.Up());
        niner[8] = GetTile(centerPosition.Up().Right());

        return niner;
    }
}
