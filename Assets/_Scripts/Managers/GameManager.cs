using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private GameObject _resultsScreenCanvas;

    void OnEnable() {
        Messenger.AddListener(GameEvent.LEVEL_COMPLETED, ShowLevelResults);
    }

    void OnDisable() {
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETED, ShowLevelResults);
    }

    void ShowLevelResults() {
        _resultsScreenCanvas.SetActive(true);
    }
}
