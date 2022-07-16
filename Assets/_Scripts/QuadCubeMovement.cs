using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadCubeMovement : MonoBehaviour {
    [SerializeField]
    private float _speed = 300f;
    [SerializeField]
    private FacesData _faces;
    [SerializeField]
    private QuadCubeFacesController _facesController;

    [SerializeField]
    private bool _debugMode = false;

    private bool _canRaycast = false;
    private bool _canMoveInDirection;
    private bool _isMoving;
    private bool _isInSumState;

    void Awake() {
        _canMoveInDirection = false;
        _isMoving = false;
        _isInSumState = false;
    }

    void Update() {
        if (!_isMoving) {
            HandleMovement();
        }
    }

    void HandleMovement() {
        var verticalAxisValue = Input.GetAxisRaw("Vertical");
        var horizontalAxisValue = Input.GetAxisRaw("Horizontal");
        Vector3 moveDirection = Vector3.zero;

        if (verticalAxisValue > 0) {
            moveDirection = Vector3.forward;
        } else if (verticalAxisValue < 0) {
            moveDirection = Vector3.back;
        } else if (horizontalAxisValue > 0) {
            moveDirection = Vector3.right;
        } else if (horizontalAxisValue < 0) {
            moveDirection = Vector3.left;
        }

        _canMoveInDirection = CanMoveInDirection(moveDirection);

        if (_canMoveInDirection) {
            StartCoroutine(Roll(moveDirection));
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

        _canRaycast = true;
        _isMoving = false;
    }

    bool CanMoveInDirection(Vector3 direction) {
        RaycastHit hit;

        DebugDrawRay(transform.position, direction, 1f);
        if (Physics.Raycast(transform.position, direction, out hit, 1f)) {
            DebugMessage($"Collider.Tag: {hit.collider.tag}");
            return hit.collider.CompareTag("Tile") || hit.collider.CompareTag("Exit");
        }

        return false;
    }

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
            if (hit.transform.gameObject.name.Equals("SumTileEnter")) {
                _isInSumState = true;
                _facesController.StartSumState();
            } else if (hit.transform.gameObject.name.Equals("SumTileExit")) {
                _isInSumState = false;
                _facesController.StopSumState();
            }

            if (_isInSumState && hit.transform.gameObject.name.StartsWith("Tile")) {
                var tileFaceController = hit.transform.GetComponent<TileFaceController>();
                if (tileFaceController != null) {
                    tileFaceController.SetTileType(TileType.Sum);
                }
            }
        }

        _canRaycast = false;
    }

    Vector3 GetDownwardFace() {
        // Check what face is parallel to the down direction (in local space)
        // X-axis
        if ((int) Vector3.Cross(Vector3.down, transform.right).magnitude == 0) {
            // Check the actual face that is facing down
            if (Vector3.Dot(Vector3.down, transform.right) > 0f) {
                return transform.right;
            } else {
                return -transform.right;
            }
        }
        // Y-axis
        else if ((int) Vector3.Cross(Vector3.down, transform.up).magnitude == 0) {
            if (Vector3.Dot(Vector3.down, transform.up) > 0f) {
                return transform.up;
            } else {
                return -transform.up;
            }
        }
        // Z-axis
        else if ((int) Vector3.Cross(Vector3.down, transform.forward).magnitude == 0) {
            if (Vector3.Dot(Vector3.down, transform.forward) > 0f) {
                return transform.forward;
            } else {
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

    void DebugDrawRay(Vector3 origin, Vector3 direction, float maxDistance = 2.0f) {
        if (_debugMode) {
            Debug.DrawRay(origin, direction * maxDistance, Color.red);
        }
    }
}
