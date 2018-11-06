using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    // public 

    public static GameBoard Instance { get; private set;}

    protected void Awake()
    {
        Instance = this;
    }

    public bool UpdatePlayerCursor(int playerID, int x, int y)
    {
        throw new NotImplementedException();
    }

    public Tile GetTile(Vector2 position)
    {

    }

    public 
   
}
