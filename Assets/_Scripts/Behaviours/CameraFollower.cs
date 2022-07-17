using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    [SerializeField]
    private Transform _leadObject;
    [SerializeField]
    private Vector3 _cameraOffset;

    void Update() {
        var cameraPosition = _leadObject.position + _cameraOffset;
        transform.position = cameraPosition;
    }
}
