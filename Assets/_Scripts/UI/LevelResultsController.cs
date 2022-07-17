using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelResultsController : MonoBehaviour {

    void Start() {
        Time.timeScale = 0f;
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToLevelSelect() {
        Debug.Log("I've pressed level select!");
    }

    public void GoToNextLevel() {
        Debug.Log("I've pressed next!");
    }
}
