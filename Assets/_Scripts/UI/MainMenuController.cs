using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {

    }

    public void Play() {
        SceneManager.LoadScene("LevelSelect");
    }

    public void GoToTitleScreen() {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToLevelSelect() {
        SceneManager.LoadScene("LevelSelect");
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
