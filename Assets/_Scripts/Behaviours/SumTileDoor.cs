using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SumTileDoor : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _pointsText;

    public void UpdateText(string msg) {
        _pointsText.text = msg;
    }

    public void UpdateText(int points) {
        _pointsText.text = points.ToString();
    }
}
