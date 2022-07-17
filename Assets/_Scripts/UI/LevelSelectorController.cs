using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectorController : MonoBehaviour {

    [SerializeField]
    private Sprite _emptyStarSprite;
    [SerializeField]
    private Sprite _filledStarSprite;

    [SerializeField]
    private GameObject _level1Stars;
    [SerializeField]
    private GameObject _level2Stars;
    [SerializeField]
    private GameObject _level3Stars;
    [SerializeField]
    private GameObject _level4Stars;
    [SerializeField]
    private GameObject _level5Stars;
    [SerializeField]
    private GameObject _level6Stars;

    // Start is called before the first frame update
    void Start() {
        AudioManager.Instance.Play("MusicMenu");
        SetUpLevels();
    }

    void SetUpLevels() {
        GameObject[] levels = new GameObject[] { _level1Stars, _level2Stars, _level3Stars, _level4Stars, _level5Stars, _level6Stars };
        for (int i = 0; i < levels.Length; i++) {
            SetUpLevel(i, levels[i]);
        }
    }

    void SetUpLevel(int level, GameObject levelStars) {
        var star1 = levelStars.transform.Find("Star1").GetComponent<Image>();
        var star2 = levelStars.transform.Find("Star2").GetComponent<Image>();
        var star3 = levelStars.transform.Find("Star3").GetComponent<Image>();

        var starsScored = PlayerPrefs.GetInt((DataPrefs.GenerateLevelKey(level)), -1);

        if (star1 != null && star2 != null && star3 != null && starsScored >= 0) {
            switch (starsScored) {
                case 1:
                    star1.sprite = _filledStarSprite;
                    star2.sprite = _emptyStarSprite;
                    star3.sprite = _emptyStarSprite;
                    break;
                case 2:
                    star1.sprite = _filledStarSprite;
                    star2.sprite = _filledStarSprite;
                    star3.sprite = _emptyStarSprite;
                    break;
                case 3:
                    star1.sprite = _filledStarSprite;
                    star2.sprite = _filledStarSprite;
                    star3.sprite = _filledStarSprite;
                    break;
                default:
                    star1.sprite = _emptyStarSprite;
                    star2.sprite = _emptyStarSprite;
                    star3.sprite = _emptyStarSprite;
                    break;
            }
        }
    }

    public void LoadLevel1() {
        var starsScored = PlayerPrefs.GetInt((DataPrefs.GenerateLevelKey(0)), -1);
        if (starsScored >= 0) {
            SceneManager.LoadScene("Level0");
        }
    }

    public void LoadLevel2() {
        var starsScored = PlayerPrefs.GetInt((DataPrefs.GenerateLevelKey(0)), -1);
        if (starsScored >= 0) {
            SceneManager.LoadScene("Level1");
        }
    }

    public void LoadLevel3() {
        var starsScored = PlayerPrefs.GetInt((DataPrefs.GenerateLevelKey(0)), -1);
        if (starsScored >= 0) {
            SceneManager.LoadScene("Level2");
        }
    }

    public void LoadLevel4() {
        var starsScored = PlayerPrefs.GetInt((DataPrefs.GenerateLevelKey(0)), -1);
        if (starsScored >= 0) {
            SceneManager.LoadScene("Level3");
        }
    }

    public void LoadLevel5() {
        var starsScored = PlayerPrefs.GetInt((DataPrefs.GenerateLevelKey(0)), -1);
        if (starsScored >= 0) {
            SceneManager.LoadScene("Level4");
        }
    }

    public void LoadLevel6() {
        var starsScored = PlayerPrefs.GetInt((DataPrefs.GenerateLevelKey(0)), -1);
        if (starsScored >= 0) {
            SceneManager.LoadScene("Level5");
        }
    }
}
