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
        _models.Add(GetComponentInChildren<MeshRenderer>().gameObject);
    }
    
    private TileData _tileData;
    public void SetTileData(TileData tileData)
    {
        _tileData = tileData;
        _tileData.RaiseTileEvent += OnRaiseTile;
    }

    private void OnRaiseTile(TileData tileData, int previousHeight, int newHeight)
    {
        Debug.Log("Raising tile " + _tileData.tileType + " by " + (newHeight - previousHeight));
        for (int i = previousHeight; i < newHeight; i++)
        {
            var newModel = Instantiate(_models[0]);
            var originalPos = _models[0].transform.position;
            newModel.transform.position = new Vector3(originalPos.x, originalPos.y + tileHeight * (i + 1), originalPos.z);
            _models.Add(newModel);
            
        }
    }
}
