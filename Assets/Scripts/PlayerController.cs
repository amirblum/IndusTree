using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerID;
    private BoardData.BoardPos _boardPosition;
    public GameObject CurrentTile;
    public PlayerInputStrings PlayerInputs;

    [Serializable]
    public class PlayerInputStrings  {

        public KeyCode up;
        public KeyCode down;
        public KeyCode left;
        public KeyCode right;

        
        //public PlayerInputStrings(string Sup, string Sdown, string, Sleft, string Sright)
        //{
        //    up = ""
        //    down = Sdown;
        //}

        }
    

    public void PlayerControl ( )
    {
        if (Input.GetKeyDown(PlayerInputs.up))
        {
            _boardPosition = _boardPosition.Up();
        }
        if (Input.GetKeyDown(PlayerInputs.down))
        {
            _boardPosition = _boardPosition.Down();
        }
        if (Input.GetKeyDown(PlayerInputs.left))
        {
            _boardPosition = _boardPosition.Left();
        }
        if (Input.GetKeyDown(PlayerInputs.right))
        {
            _boardPosition = _boardPosition.Right();
        }
        transform.position = BoardView.Instance.GetUnityPos(_boardPosition);
    }
         

    public void validateTile (TileData.TileType tile)
    {
        
    }

    private void Start()
    {
        
    }

}
