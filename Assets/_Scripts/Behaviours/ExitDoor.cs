using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExitDoor : MonoBehaviour {

    [SerializeField]
    private Color _exitDoorOpenedColor;
    [SerializeField]
    private Color _exitDoorClosedColor;

    [SerializeField]
    private List<SumTilesManager> _challenges;

    private bool _areAllChallengesCompleted;

    private void Awake() {
        _areAllChallengesCompleted = false;
    }

    void Start() {
        CloseExitDoor();
    }

    void ChangeDoorColor(Color color) {
        var childrenRenderers = transform.GetComponentsInChildren<Renderer>();
        foreach (Renderer childrenRenderer in childrenRenderers) {
            var previousColor = childrenRenderer.material.color;
            childrenRenderer.material.color = new Color(color.r, color.g, color.b, previousColor.a);
        }
    }

    void Update() {
        _areAllChallengesCompleted = _challenges.All(challenge => challenge.GetScoreReport().IsSolved());

        if (_areAllChallengesCompleted) {
            OpenExitDoor();
        } else {
            CloseExitDoor();
        }
    }

    void OpenExitDoor() {
        ChangeDoorColor(_exitDoorOpenedColor);
    }

    void CloseExitDoor() {
        ChangeDoorColor(_exitDoorClosedColor);
    }

    public bool IsDoorOpen() {
        return _areAllChallengesCompleted;
    }

    public List<(int expectedPointsToScore, int scoredPoints)> GetChallengesScoreData() {
        List<(int, int)> scoresData = new List<(int, int)>();

        return _challenges.Select(challenge => challenge.GetScoreData()).ToList();
    }
}
