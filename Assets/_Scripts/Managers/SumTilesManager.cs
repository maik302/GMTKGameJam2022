using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SumTilesManager : MonoBehaviour, IChallenge {
    private const int MAX_SCORABLE_POINTS = 100;

    [Header("Configuration")]
    [SerializeField]
    private int _pointsToScore = 10;

    [Header("Doors")]
    [SerializeField]
    private GameObject _enterDoor;
    [SerializeField]
    private GameObject _exitDoor;

    [Header("Points")]
    [SerializeField]
    private Color _defaultPointsColor;
    [SerializeField]
    private Color _notEnoughPointsColor;
    [SerializeField]
    private Color _enoughPointsColor;

    private int _scoredPoints;
    private int _remainingPoints;
    private List<GameObject> _scoredTilesPath;
    private bool _isScoringPoints;

    private ScoreReport _scoreReport;

    void OnEnable() {
        Messenger.AddListener(GameEvent.START_SUM, StartCountingPoints);
        Messenger<int, GameObject>.AddListener(GameEvent.ADD_TO_SUM, UpdatePointsCount);
        Messenger<GameObject>.AddListener(GameEvent.REMOVE_FROM_SUM, RemoveTilesFromScorePath);
        Messenger.AddListener(GameEvent.END_SUM, StopCountingPoints);
    }

    void StartCountingPoints() {
        _scoredPoints = 0;
        _remainingPoints = _pointsToScore;
        _scoreReport.SetIsSolved(false);
        _isScoringPoints = true;
        UpdatePointsCount(0, null);
        ResetScoredTilesPath();
        ResetDoorsTextFormat();
    }

    void ResetDoorsTextFormat() {
        var enterDoor = _enterDoor.GetComponent<SumTileDoor>();
        var exitDoor = _exitDoor.GetComponent<SumTileDoor>();

        if (enterDoor != null && exitDoor != null) {
            enterDoor.ChangeTextColor(new Color(_defaultPointsColor.r, _defaultPointsColor.g, _defaultPointsColor.b));
            exitDoor.ChangeTextColor(new Color(_defaultPointsColor.r, _defaultPointsColor.g, _defaultPointsColor.b));
        }
    }

    void ResetScoredTilesPath() {
        foreach (GameObject tile in _scoredTilesPath) {
            var tileFaceComponent = tile.GetComponent<TileFace>();
            if (tileFaceComponent != null) {
                tileFaceComponent.SetTileType(TileType.Base);
            }
        }
        _scoredTilesPath.Clear();
    }

    void OnDisable() {
        Messenger.RemoveListener(GameEvent.START_SUM, StartCountingPoints);
        Messenger<int, GameObject>.RemoveListener(GameEvent.ADD_TO_SUM, UpdatePointsCount);
        Messenger<GameObject>.RemoveListener(GameEvent.REMOVE_FROM_SUM, RemoveTilesFromScorePath);
    }

    void Awake() {
        _remainingPoints = _pointsToScore;
        _scoredTilesPath = new List<GameObject>();
        _scoreReport = new ScoreReport(isSolved: false, finalScoredPoints: 0, originalPointsToScore: _pointsToScore);
    }

    void Start() {
        var enterDoor = _enterDoor.GetComponent<SumTileDoor>();
        var exitDoor = _exitDoor.GetComponent<SumTileDoor>();

        if (enterDoor != null && exitDoor != null) {
            enterDoor.UpdateText("");
            exitDoor.UpdateText(_pointsToScore.ToString());
        }
    }

    public void UpdatePointsCount(int scoredPoints, GameObject scoredTile) {
        _scoredPoints += scoredPoints;
        _remainingPoints -= scoredPoints;

        var enterDoor = _enterDoor.GetComponent<SumTileDoor>();
        var exitDoor = _exitDoor.GetComponent<SumTileDoor>();

        if (enterDoor != null && exitDoor != null) {
            var enterDoorPointsToShow = _scoredPoints >= MAX_SCORABLE_POINTS ? MAX_SCORABLE_POINTS : _scoredPoints;
            var exitDoorPointsToShow = _remainingPoints <= 0 ? 0 : _remainingPoints;
            enterDoor.UpdateText(enterDoorPointsToShow);
            exitDoor.UpdateText(exitDoorPointsToShow);

            _scoreReport.SetFinalScoredPoints(enterDoorPointsToShow);
        }

        if (scoredTile != null) {
            var tileFaceComponent = scoredTile.GetComponent<TileFace>();
            if (tileFaceComponent != null) {
                tileFaceComponent.SetScoredPoints(scoredPoints);
            }

            _scoredTilesPath.Add(scoredTile);
        }
    }

    public void RemoveTilesFromScorePath(GameObject tileEntryPoint) {
        var tileEntryPointIndex = _scoredTilesPath.FindIndex(tile => tile.GetInstanceID() == tileEntryPoint.GetInstanceID());

        for (int i = tileEntryPointIndex + 1; i < _scoredTilesPath.Count; i++) {
            var tileFaceComponent = _scoredTilesPath[i].GetComponent<TileFace>();
            if (tileFaceComponent != null) {
                tileFaceComponent.SetTileType(TileType.Base);
                UpdatePointsCount(-tileFaceComponent.GetScoredPoints(), null);
            }
        }

        _scoredTilesPath.RemoveAll(tile => _scoredTilesPath.IndexOf(tile) >= tileEntryPointIndex + 1);
    }

    public ScoreReport GetScoreReport() {
        return _scoreReport;
    }

    void StopCountingPoints() {
        _scoreReport.SetIsSolved(_remainingPoints <= 0);

        var exitDoor = _exitDoor.GetComponent<SumTileDoor>();
        if (exitDoor != null && _isScoringPoints) {
            if (_remainingPoints > 0) {
                exitDoor.ChangeTextColor(new Color(_notEnoughPointsColor.r, _notEnoughPointsColor.g, _notEnoughPointsColor.b));
            } else {
                exitDoor.ChangeTextColor(new Color(_enoughPointsColor.r, _enoughPointsColor.g, _enoughPointsColor.b));
            }
        }

        _isScoringPoints = false;
    }
}
