using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadCubeFacesController : MonoBehaviour {
    [SerializeField]
    private Material _defaultCubeMaterial;

    [SerializeField]
    private Material _sumTileMaterial;

    void Start() {
        StopSumState();
    }

    public void StartSumState() {
        var childrenRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childrenRenderers) {
            renderer.material = _sumTileMaterial;
        }
    }

    public void StopSumState() {
        var childrenRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childrenRenderers) {
            renderer.material = _defaultCubeMaterial;
        }
    }
}
