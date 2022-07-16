using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumTilesManager : MonoBehaviour {
    private const int MAX_SCORABLE_POINTS = 100;

    [Header("Configuration")]
    [SerializeField]
    private int _pointsToScore = 10;

    [Header("Doors")]
    [SerializeField]
    private GameObject _enterDoor;
    [SerializeField]
    private GameObject _exitDoor;

    private int _scoredPoints;
    private int _remainingPoints;

    void OnEnable() {
        Messenger.AddListener(GameEvent.START_SUM, StartCountingPoints);
    }

    void StartCountingPoints() {
        Debug.Log("I've started counting points!");
        _scoredPoints = 0;
        _remainingPoints = _pointsToScore;
        UpdatePointsCount(0);
    }

    void OnDisable() {
        Messenger.RemoveListener(GameEvent.START_SUM, StartCountingPoints);
    }

    void Start() {
        var enterDoor = _enterDoor.GetComponent<SumTileDoor>();
        var exitDoor = _exitDoor.GetComponent<SumTileDoor>();

        if (enterDoor != null && exitDoor != null) {
            enterDoor.UpdateText("");
            exitDoor.UpdateText(_pointsToScore.ToString());
        }
    }

    public void UpdatePointsCount(int scoredPoints) {
        _scoredPoints += scoredPoints;
        _remainingPoints -= scoredPoints;

        var enterDoor = _enterDoor.GetComponent<SumTileDoor>();
        var exitDoor = _exitDoor.GetComponent<SumTileDoor>();

        if (enterDoor != null && exitDoor != null) {
            enterDoor.UpdateText(_scoredPoints >= MAX_SCORABLE_POINTS ? MAX_SCORABLE_POINTS : _scoredPoints);
            exitDoor.UpdateText(_remainingPoints <= 0 ? 0 : _remainingPoints);
        }
    }
}
