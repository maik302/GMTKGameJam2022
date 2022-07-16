using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExitDoor : MonoBehaviour {

    [SerializeField, SerializeReference]
    private List<SumTilesManager> _challenges;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (_challenges.All(challenge => challenge.GetScoreReport().IsSolved())) {
            Debug.Log("The exit door is open!");
        }
    }
}
