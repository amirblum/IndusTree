using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    [SerializeField] int _boardSize;
    [SerializeField] TileView _emptyTile;
    [SerializeField] TileView[] _playerTiles;
    private BoardData _boardData;
    private MeshFilter _boardModel;
    private float _boardUnitySize;
    private float _tileUnitySize;
    
    protected void Awake()
    {
        _boardModel = GetComponentInChildren<MeshFilter>();
        _boardUnitySize = _boardModel.mesh.bounds.size.x * _boardModel.transform.localScale.x;
        _tileUnitySize = _boardUnitySize / _boardSize;

        _boardData = new BoardData();
        _boardData.OnPlacedTileEvent += OnPlacedTile;
        _boardData.InitializeBoard(_boardSize, _playerTiles.Length);
    }

    private void OnPlacedTile(TileData tileData, BoardData.BoardPos pos)
    {
        var tile = Instantiate(GetTileFromData(tileData));

        tile.transform.position = new Vector3(
            _tileUnitySize / 2 + pos.x * _tileUnitySize,
            0,
            _tileUnitySize / 2 + pos.y * _tileUnitySize);
    }

    private TileView GetTileFromData(TileData tileData)
    {
        if (tileData.tileType == TileData.TileType.Empty)
        {
            return _emptyTile;
        }

        return _playerTiles[(int)tileData.tileType];
    }
}
