using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour {
    [SerializeField]
    private float _speed = 300f;
    [SerializeField]
    private FacesData _faces;

    [SerializeField]
    private bool _debugMode = false;

    private bool _isMoving;

    void Awake() {
        _isMoving = false;
    }

    void Update() {
        if (!_isMoving) {
            HandleMovement();
        }
    }

    void HandleMovement() {
        var verticalAxisValue = Input.GetAxisRaw("Vertical");
        var horizontalAxisValue = Input.GetAxisRaw("Horizontal");

        if (verticalAxisValue > 0) {
            StartCoroutine(Roll(Vector3.forward));
        } else if (verticalAxisValue < 0) {
            StartCoroutine(Roll(Vector3.back));
        } else if (horizontalAxisValue > 0) {
            StartCoroutine(Roll(Vector3.right));
        } else if (horizontalAxisValue < 0) {
            StartCoroutine(Roll(Vector3.left));
        }
    }

    /// <summary>
    /// Move the cube by rotating it by the axis of its desired movement.
    /// This method is based on:
    /// - https://www.youtube.com/watch?v=06rs3U2bpy8
    /// - https://answers.unity.com/questions/1437146/how-to-rotate-and-move-a-cube.html
    /// </summary>
    /// <param name="direction">Direction to move *roll* the cueb to</param>
    /// <returns></returns>
    IEnumerator Roll(Vector3 direction) {
        _isMoving = true;

        float remainingAngle = 90;
        Vector3 rotationCenter = transform.position + direction / 2 + Vector3.down / 2;
        // Rotation axis will be perpendicular to the y-axis and the direction of the movement
        // Cross product produce a vector that is perpendicular to both arguments
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        while (remainingAngle > 0) {
            float rotationAngle = Mathf.Min(Time.deltaTime * _speed, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;

            yield return null;
        }

        _isMoving = false;
        _canRaycast = true;

        GetDownwardFace();
    }

    private bool _canRaycast = false;
    void FixedUpdate() {
        if (_canRaycast) {
            GetFloorTile();
        }

        DebugDrawCubeAxis();
    }

    void GetFloorTile() {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity)) {
            DebugMessage($"I hit: {hit.transform.gameObject.name}");
        }

        _canRaycast = false;
    }

    Vector3 GetDownwardFace() {
        // Check what face is parallel to the down direction (in local space)
        // X-axis
        if ((int) Vector3.Cross(Vector3.down, transform.right).magnitude == 0) {
            // Check the actual face that is facing down
            if (Vector3.Dot(Vector3.down, transform.right) > 0f) {
                DebugMessage($"transform.right is facing the floor");
                return transform.right;
            } else {
                DebugMessage($"-transform.right is facing the floor");
                return -transform.right;
            }
        }
        // Y-axis
        else if ((int) Vector3.Cross(Vector3.down, transform.up).magnitude == 0) {
            if (Vector3.Dot(Vector3.down, transform.up) > 0f) {
                DebugMessage($"transform.up is facing the floor");
                return transform.up;
            } else {
                DebugMessage($"-transform.up is facing the floor");
                return -transform.up;
            }
        }
        // Z-axis
        else if ((int) Vector3.Cross(Vector3.down, transform.forward).magnitude == 0) {
            if (Vector3.Dot(Vector3.down, transform.forward) > 0f) {
                DebugMessage($"transform.forward is facing the floor");
                return transform.forward;
            } else {
                DebugMessage($"-transform.forward is facing the floor");
                return -transform.forward;
            }
        }

        return Vector3.zero;
    }

    void DebugMessage(string msg) {
        if (_debugMode) {
            Debug.Log(msg);
        }
    }

    void DebugDrawCubeAxis() {
        if (_debugMode) {
            Debug.DrawRay(transform.position, transform.right * 2.0f, Color.red);
            Debug.DrawRay(transform.position, transform.up * 2.0f, Color.green);
            Debug.DrawRay(transform.position, transform.forward * 2.0f, Color.blue);
        }
    }
}
