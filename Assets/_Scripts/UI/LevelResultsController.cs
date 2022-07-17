using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelResultsController : MonoBehaviour {

    [SerializeField]
    private Image _star1Image;
    [SerializeField]
    private Image _star2Image;
    [SerializeField]
    private Image _star3Image;

    [SerializeField]
    private Sprite _emptyStarSprite;
    [SerializeField]
    private Sprite _filledStarSprite;

    [SerializeField]
    private string _nextLevelName;

    void Start() {
        Time.timeScale = 0f;
        SetScoredStars();
    }

    void SetScoredStars() {
        var scoredStars = GameManager.Instance.GetLevelScoredStars();
        switch (scoredStars) {
            case 1:
                _star1Image.sprite = _filledStarSprite;
                _star2Image.sprite = _emptyStarSprite;
                _star3Image.sprite = _emptyStarSprite;
                break;
            case 2:
                _star1Image.sprite = _filledStarSprite;
                _star2Image.sprite = _filledStarSprite;
                _star3Image.sprite = _emptyStarSprite;
                break;
            case 3:
                _star1Image.sprite = _filledStarSprite;
                _star2Image.sprite = _filledStarSprite;
                _star3Image.sprite = _filledStarSprite;
                break;
        }
    }

    public void RestartLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToLevelSelect() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelSelect");
    }

    public void GoToNextLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(_nextLevelName);
    }
}
