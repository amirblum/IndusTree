using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour
{
    public float tileWidth;
    public float tileHeight;

    private List<GameObject> _models;

    protected void Awake()
    {
        _models = new List<GameObject>();
        _models[0] = GetComponentInChildren<MeshFilter>().gameObject;
    }
    
    private TileData _tileData;
    public void SetTileData(TileData tileData)
    {
        _tileData = tileData;
        // _tileData.RaiseTileEvent += OnRaiseTile;
    }

    private void OnRaiseTile(int previousHeight, int newHeight)
    {
        // for 
        // _models
    }
}
