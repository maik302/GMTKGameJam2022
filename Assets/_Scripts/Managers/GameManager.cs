using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {
    [Header("Level configurations")]
    [SerializeField]
    private int _levelId;
    [SerializeField]
    private GameObject _resultsScreenCanvas;
    [SerializeField]
    private string _bgmName;

    [Header("Level score configuration")]
    [SerializeField]
    private ExitDoor _exitDoor;

    public static GameManager Instance;

    void OnEnable() {
        Messenger.AddListener(GameEvent.LEVEL_COMPLETED, ShowLevelResults);
    }

    void OnDisable() {
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETED, ShowLevelResults);
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Start() {
        AudioManager.Instance.Play(_bgmName);
    }

    void ShowLevelResults() {
        PlayerPrefs.SetInt(DataPrefs.GenerateLevelKey(_levelId), GetLevelScoredStars());
        _resultsScreenCanvas.SetActive(true);
    }

    public int GetLevelScoredStars() {
        return (int) _exitDoor.GetChallengesScoreData().Average(scoreData => GetScoredStartsByChallenge(scoreData));
    }

    int GetScoredStartsByChallenge((int, int) challengeScoreData) {
        var (expectedPointsToScore, scoredPoints) = challengeScoreData;
        var scoredStars = 1;

        // 21 => Sum of all sides of a die : 1+2+3+4+5+6
        int highestBound = expectedPointsToScore + 42;
        int midBound = expectedPointsToScore + 21;

        if (scoredPoints >= highestBound) {
            scoredStars = 1;
        } else if (scoredPoints >= midBound) {
            scoredStars = 2;
        } else {
            scoredStars = 3;
        }
        
        return scoredStars;
    }
}
