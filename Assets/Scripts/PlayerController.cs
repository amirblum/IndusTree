using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerID;
    private TileData.TileType PlayerTileType;

    public BoardData.BoardPos _boardPosition;
    public PlayerInputStrings PlayerInputs;
    private TileData _currentTile;
    private BoardData.BoardPos InitBoardPos;

    

    [Serializable]
    public class PlayerInputStrings  {

        public KeyCode up;
        public KeyCode down;
        public KeyCode left;
        public KeyCode right;
        public KeyCode PlaceTile;

        
        //public PlayerInputStrings(string Sup, string Sdown, string, Sleft, string Sright)
        //{
        //    up = ""
        //    down = Sdown;
        //}

        }
    
    //add boundary condition
    public void PlayerMovement ( )
    {   

        if (Input.GetKeyDown(PlayerInputs.up) && _boardPosition.y < BoardData.Instance.BoardSize - 1 )
        {
            _boardPosition = _boardPosition.Up();
        }
        if (Input.GetKeyDown(PlayerInputs.down) && _boardPosition.y > 0)
        {
            _boardPosition = _boardPosition.Down();
        }
        if (Input.GetKeyDown(PlayerInputs.left) && _boardPosition.x > 0)
        {
            _boardPosition = _boardPosition.Left();
        }
        if (Input.GetKeyDown(PlayerInputs.right) && _boardPosition.x < BoardData.Instance.BoardSize - 1 )
        {
            _boardPosition = _boardPosition.Right();
        }
        transform.position = BoardView.Instance.GetUnityPos(_boardPosition);
        
    }
         

    public TileData GetTile ()
    {
        return BoardData.Instance.GetTile(_boardPosition);
    }

    public void SetTileToPlayerType ()
    {
        if (Input.GetKeyDown(PlayerInputs.PlaceTile))
            {
                if (GetTile().tileType == TileData.TileType.Empty)
                {
                    BoardData.Instance.PlaceTile(PlayerTileType, _boardPosition);
                }
                else
                {
                    //In case we add a time counter for placing a tile, do not "spend" the tile placement token (I.E lock placement)
                    Debug.Log("Tile Not Empty! Current tile type is :" + GetTile().tileType);
                }
            }
    }
    

    protected void Awake()
    {
        if (playerID == 0)
        {
            PlayerTileType = TileData.TileType.Organic;
        }
        if (playerID == 1)
        {
            PlayerTileType = TileData.TileType.Mechanic;
        }
    }

    protected void Start()
    {

        InitBoardPos = new BoardData.BoardPos();
        InitBoardPos.x = 0;
        InitBoardPos.y = 0;
        _boardPosition = InitBoardPos;
}

    protected void Update()
    {
        PlayerMovement();
        SetTileToPlayerType();
    }

}
