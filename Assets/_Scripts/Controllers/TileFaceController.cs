using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFaceController : MonoBehaviour {
    [SerializeField]
    private Material _defaultTileMaterial;

    [SerializeField]
    private Material _sumTileMaterial;

    void Start() {
        SetTileType(TileType.Base);
    }

    public void SetTileType(TileType type) {
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
}

public enum TileType {
    Base,
    Sum,
}
