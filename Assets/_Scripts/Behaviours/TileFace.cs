using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFace : MonoBehaviour {
    [SerializeField]
    private Material _defaultTileMaterial;

    [SerializeField]
    private Material _sumTileMaterial;

    private TileType _tileType;
    private int _scoredPoints;

    void Start() {
        SetTileType(TileType.Base);
    }

    public void SetTileType(TileType type) {
        _tileType = type;
        var renderer = GetComponent<Renderer>();
        if (renderer != null) {
            switch (type) {
                case TileType.Base:
                    renderer.material = _defaultTileMaterial;
                    break;
                case TileType.Sum:
                    renderer.material = _sumTileMaterial;
                    break;
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
}

public enum TileType {
    Base,
    Sum,
}
