using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    public static BoardView Instance;
    [SerializeField] int _boardSize;
    [SerializeField] TileView _emptyTile;
    [SerializeField] TileView[] _playerTiles;
    private TileView[,] _placedTiles;
    private BoardData _boardData;
    private MeshFilter _boardModel;
    private float _boardUnitySize;
    private float _tileUnitySize;
    
    protected void Awake()
    {
        Instance = this;
        
        _boardModel = GetComponentInChildren<MeshFilter>();
        _boardUnitySize = _boardModel.mesh.bounds.size.x * _boardModel.transform.localScale.x;
        _tileUnitySize = _boardUnitySize / _boardSize;

        _placedTiles = new TileView[_boardSize, _boardSize];

        _boardData = new BoardData();
        _boardData.OnPlacedTileEvent += OnPlacedTile;
        _boardData.InitializeBoard(_boardSize, _playerTiles.Length);
    }

    public Vector3 GetUnityPos(BoardData.BoardPos pos)
    {
        return new Vector3(
            _tileUnitySize / 2 + pos.x * _tileUnitySize,
            0,
            _tileUnitySize / 2 + pos.y * _tileUnitySize);
    }

    private void OnPlacedTile(TileData tileData, BoardData.BoardPos pos)
    {
        // Out with the old!
        var currentTile = _placedTiles[pos.x, pos.y];
        Destroy(currentTile);

        // In with the new!
        var newTile = Instantiate(GetTileFromData(tileData));
        newTile.SetTileData(tileData);

        newTile.transform.position = GetUnityPos(pos);

        _placedTiles[pos.x, pos.y] = newTile;
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
