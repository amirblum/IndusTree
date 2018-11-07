using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardData
{
    [Serializable]
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
    public int WinCondition { get; set; }
    public static BoardData Instance { get; private set; }
    public int[] PlayerScores { get; set; }
    public int FilledTiles { get; set; }

    // Public events
    public event Action<TileData, BoardPos> OnPlacedTileEvent;
    public event Action OnScoreUpdatedEvent;
    public event Action<int> OnGameOverEvent;

    // Private variables
    private TileData[,] _board;
    private TileData _outOfBoundsTile = new TileData(TileData.TileType.OutOfBounds, new BoardPos {x = -1, y = -1});

    public BoardData()
    {
        Instance = this;
    }

    public void InitializeBoard(int boardSize, BoardPos[] startingTiles, int winCondition)
    {
        BoardSize = boardSize;
        NumOfPlayers = startingTiles.Length;
        WinCondition = winCondition;

        _board = new TileData[BoardSize, BoardSize];
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                var pos = new BoardPos { x = i, y = j };
                var tileData = new TileData(pos);

                for (int k = 0; k < startingTiles.Length; k++)
                {
                    if (i == (int)startingTiles[k].x && j == (int)startingTiles[k].y)
                    {
                        tileData.tileType = (TileData.TileType)k;
                    }
                }
                _board[i, j] = tileData;
                OnPlacedTileEvent?.Invoke(tileData, pos);
            }
        }

        PlayerScores = new int[] {1, 1};
    }
    public TileData GetTile(BoardPos position)
    {
        if (position.x < 0 || position.x >= BoardSize || position.y < 0 || position.y >= BoardSize)
        {
            return _outOfBoundsTile;
        }
        return _board[position.x, position.y];
    }

    private void UpdateScore(int player, int scoreToAdd)
    {
        if (player < 0 || player >= PlayerScores.Length)
        {
            return;
        }
        PlayerScores[player] += scoreToAdd;
        OnScoreUpdatedEvent?.Invoke();
    }

    public void PlaceTile(TileData.TileType newTileType, BoardPos newTilePosition)
    {
        Debug.Assert((_board[newTilePosition.x, newTilePosition.y].tileType == TileData.TileType.Empty ||
        newTileType == TileData.TileType.Destroyed),
            "Trying to place tile on non-empty spot!");

        // Place the tile on the board.
        var newTile = new TileData(newTileType, newTilePosition);
        _board[newTilePosition.x, newTilePosition.y] = newTile;
        OnPlacedTileEvent?.Invoke(newTile, newTilePosition);

        UpdateScore((int)newTileType, 1);

        if (newTileType != TileData.TileType.Organic && newTileType != TileData.TileType.Mechanic)
        {
            return;
        }

        // Check the quads.
        var quads = GetQuads(newTilePosition);
        bool closedAQuad = false;
        var destroyedQuads = new List<TileData[]>();

        foreach (var quad in quads)
        {
            if (QuadIsClosed(quad))
            {
                Debug.Log("Closing a quad");
                // Raise the quad
                foreach (var tile in quad)
                {
                    tile.amountToRaiseTile++;
                }
                closedAQuad = true;
            } 
            else if (QuadIsDestroyed(quad))
            {
                destroyedQuads.Add(quad);
            }
        }
        if (closedAQuad) 
        {
            var niner = GetNiner(newTilePosition);

            foreach (var tile in niner)
            {
                if (tile.amountToRaiseTile > 0)
                {
                    tile.RaiseTile();
                    UpdateScore((int)tile.tileType, 1);
                }
            }
        }
        foreach (var destroyedQuad in destroyedQuads)
        {            
            foreach (var tile in destroyedQuad)
            {
                UpdateScore((int)tile.tileType, tile.tileLevel);
                tile.DestroyTile();
                PlaceTile(TileData.TileType.Destroyed, tile.boardPos);
            }
        }

        // Check win condition.
        if (PlayerScores[0] >= WinCondition && PlayerScores[1] < WinCondition)
        {
            OnGameOverEvent?.Invoke(0);
        }
        else if (PlayerScores[1] >= WinCondition && PlayerScores[0] < WinCondition)
        {
            OnGameOverEvent?.Invoke(1);
        }
        else if (PlayerScores[0] >= WinCondition && PlayerScores[1] >= WinCondition)
        {
            Debug.Log("OMG you tied!");
        }


        // FilledTiles++;

        // if (FilledTiles >= BoardSize * BoardSize)
        // {
        //     Debug.Log("End game!");
        //     var winner = -1;
        //     if (PlayerScores[0] > PlayerScores[1])
        //     {
        //         winner = 0;
        //     }
        //     else if (PlayerScores[1] > PlayerScores[0])
        //     {
        //         winner = 1;
        //     }
        //     OnGameOverEvent?.Invoke(winner);
        // }
    }

    private TileData[][] GetQuads(BoardPos newTilePos)
    {
        var quads = new TileData[4][];
        quads[0] = GetQuad(newTilePos);
        quads[1] = GetQuad(newTilePos.Left());
        quads[2] = GetQuad(newTilePos.Left().Down());
        quads[3] = GetQuad(newTilePos.Down());
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

    private bool QuadIsDestroyed(TileData[] quad)
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

        return (i == 4 || j == 4);
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
