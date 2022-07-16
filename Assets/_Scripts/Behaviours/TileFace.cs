using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFace : MonoBehaviour {
    [SerializeField]
    private Material _defaultTileMaterial;

    private const int DEFAULT_SUM_TILES_MANAGER_ID = -1;

    private TileType _tileType;
    private int _scoredPoints;
    private int _sumTilesManagerId;

    private void Awake() {
        ResetSumTilesManagerId();
    }

    void Start() {
        SetTileType(TileType.Base);
    }

    public void SetTileType(TileType type) {
        _tileType = type;
        if (type == TileType.Base) {
            var renderer = GetComponent<Renderer>();
            if (renderer != null) {
                renderer.material = _defaultTileMaterial;
            }
        }
    }

    public TileType GetTileType() {
        return _tileType;
    }

    public void SetScoredPoints(int scoredPoints) {
        _scoredPoints = scoredPoints;
    }

    public int GetScoredPoints() {
        return _scoredPoints;
    }

    public void SetTileColor(Color color) {
        var renderer = GetComponent<Renderer>();
        if (renderer != null) {
            renderer.material.color = new Color(color.r, color.g, color.b);
        }
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

public enum TileType {
    Base,
    Sum,
}
