using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        
    }

    public void Play() {
        Debug.Log("I've pressed play!");
    }

    public void GoToTitleScreen() {
        Debug.Log("I've pressed go to title screen!");
    }
}
