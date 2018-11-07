using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoardView : MonoBehaviour
{
    public static BoardView Instance;
    [SerializeField] int _boardSize;
    [SerializeField] int _winCondition;
    [SerializeField] BoardData.BoardPos[] _startingTiles;
    [SerializeField] TileView _emptyTile;
    [SerializeField] TileView _destroyedTile;
    [SerializeField] TileView[] _playerTiles;
    [Header("UI")]
    [SerializeField] GameObject[] _playerWinMessages;
    [SerializeField] Text[] _playerScoresText;

    [Header("Sound effects")]
    [SerializeField] AudioClip[] _p1PlacementSounds;
    [SerializeField] AudioClip[] _p2PlacementSounds;

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
        _boardData.OnScoreUpdatedEvent += UpdateScoreTexts;
        _boardData.OnGameOverEvent += OnGameOver;
        _boardData.InitializeBoard(_boardSize, _startingTiles, _winCondition);

        UpdateScoreTexts();
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("Main");
        }
    }

    private void OnGameOver(int winner)
    {
        _playerWinMessages[winner].SetActive(true);
        AudioManager.Instance.PlayWinner(winner);
    }

    private void UpdateScoreTexts()
    {
        SetScoreText(0, _boardData.PlayerScores[0]);
        SetScoreText(1, _boardData.PlayerScores[1]);
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
        if (currentTile != null)
        {
            Destroy(currentTile.gameObject);
        }

        // In with the new!
        TileView newTile = Instantiate(GetTileFromData(tileData));
        newTile.SetTileData(tileData);
        tileData.RaiseTileEvent += OnRaisedTile;

        newTile.transform.position = GetUnityPos(pos);

        _placedTiles[pos.x, pos.y] = newTile;

        if (AudioManager.Instance == null)
        {
            return;
        }

        var tileType = tileData.tileType;
        if (tileType == TileData.TileType.Organic || tileType == TileData.TileType.Mechanic)
        {
            var sounds = tileData.tileType == TileData.TileType.Organic ? _p1PlacementSounds : _p2PlacementSounds;
            AudioManager.Instance.PlaySfx(sounds[Random.Range(0, sounds.Length)]);
        }
    }

    private void OnRaisedTile(TileData tileData, int previousHeight, int newHeight)
    {
        if (tileData.tileType == TileData.TileType.Empty || tileData.tileType == TileData.TileType.OutOfBounds)
        {
            return;
        }

        int player = (int)tileData.tileType;
    }

    private void SetScoreText(int player, int score)
    {
        _playerScoresText[player].text = score.ToString();
    }

    private TileView GetTileFromData(TileData tileData)
    {
        if (tileData.tileType == TileData.TileType.Empty)
        {
            return _emptyTile;
        }
        
        if (tileData.tileType == TileData.TileType.Destroyed)
        {
            return _destroyedTile;
        }

        return _playerTiles[(int)tileData.tileType];
    }
}
