using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadCubeFace : MonoBehaviour {

    [SerializeField]
    private int _faceValue;

    [SerializeField]
    private Texture _faceTexture;

    void Start() {
        var renderer = GetComponent<Renderer>();
        if (renderer != null) {
            renderer.material.mainTexture = _faceTexture;
        }
    }

    public int GetFaceValue() {
        return _faceValue;
    }

    public Texture GetFaceTexture() {
        return _faceTexture;
    }
}
