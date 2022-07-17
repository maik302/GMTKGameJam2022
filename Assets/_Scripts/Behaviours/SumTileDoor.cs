using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SumTileDoor : MonoBehaviour {

    private const int DEFAULT_SUM_TILES_MANAGER_ID = -1;

    [SerializeField]
    private TextMeshProUGUI _pointsText;

    private int _sumTilesManagerId;
    private Color _doorColor;

    public void UpdateText(string msg) {
        _pointsText.text = msg;
    }

    public void UpdateText(int points) {
        _pointsText.text = points.ToString();
    }

    public void ChangeTextColor(Color color) {
        _pointsText.color = color;
    }

    public void SetDoorColor(Color color) {
        _doorColor = new Color(color.r, color.g, color.b);
        var renderer = GetComponent<Renderer>();
        if (renderer != null) {
            renderer.material.color = _doorColor;
        }

        var childrenRenderers = transform.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in childrenRenderers) {
            if (childRenderer != null) {
                childRenderer.material.color = _doorColor;
            }
        }
    }

    public Color GetDoorColor() {
        return _doorColor;
    }

    public void SetSumTilesManagerId(int id) {
        _sumTilesManagerId = id;
    }

    public int GetSumTilesManagerId() {
        return _sumTilesManagerId;
    }

    public void ResetSumTilesManagerId() {
        SetSumTilesManagerId(DEFAULT_SUM_TILES_MANAGER_ID);
    }
}
