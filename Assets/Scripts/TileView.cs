using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour
{
    public float tileWidth;
    public float tileHeight;
    public GameObject[] levels;

    private MeshFilter _meshFilter;
    private ParticleSystem _levelUpParticles;

    // private List<GameObject> _models;

    protected void Awake()
    {
    //     _models = new List<GameObject>();
    //     _models.Add(GetComponentInChildren<MeshRenderer>().gameObject);

        _meshFilter = GetComponentInChildren<MeshFilter>();
        _levelUpParticles = GetComponentInChildren<ParticleSystem>();
    }
    
    public TileData tileData;
    public void SetTileData(TileData tileData)
    {
        this.tileData = tileData;
        this.tileData.RaiseTileEvent += OnRaiseTile;
    }

    private void OnRaiseTile(TileData tileData, int previousHeight, int newHeight)
    {
        if (newHeight <= 0)
        {
            return;
        }

        // for (int i = previousHeight; i < newHeight; i++)
        // {
            // var newModel = Instantiate(_models[0]);
            // var originalPos = _models[0].transform.position;
            // newModel.transform.position = new Vector3(originalPos.x, originalPos.y + tileHeight * (i + 1), originalPos.z);
            // _models.Add(newModel);
        // }
        
        levels[previousHeight].SetActive(false);
        levels[newHeight].SetActive(true);
        _levelUpParticles?.Play();
    }
}
