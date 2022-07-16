using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadCubeMovement : MonoBehaviour {

    private const int DEFAULT_SUM_TILES_MANAGER_ID = -1;

    [SerializeField]
    private float _speed = 300f;
    [SerializeField]
    private QuadCubeFacesController _facesController;

    [SerializeField]
    private bool _debugMode = false;

    private bool _canRaycast = false;
    private bool _canMoveInDirection;
    private bool _isMoving;
    private bool _isInSumState;
    private int _activeSumTilesManagerId;

    void Awake() {
        _canMoveInDirection = false;
        _isMoving = false;
        _isInSumState = false;
    }

    void Start() {
        _facesController.ShowLookahead();
    }

    void Update() {
        if (!_isMoving) {
            HandleMovement();
        }

        if (_canRaycast) {
            GetFloorTile();
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
            _facesController.HideLookahead();
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
        _facesController.ShowLookahead();
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

    void GetFloorTile() {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity)) {
            DebugMessage($"I hit: {hit.transform.gameObject.name}");
            // Entering the EnterDoor
            if (hit.transform.gameObject.name.Equals("SumTileEnter")) {
                _isInSumState = true;
                var sumTileDoorComponent = hit.transform.GetComponent<SumTileDoor>();
                if (sumTileDoorComponent != null) {
                    _facesController.StartSumState(sumTileDoorComponent.GetDoorColor());
                    _activeSumTilesManagerId = sumTileDoorComponent.GetSumTilesManagerId();
                    Messenger<int>.Broadcast(GameEvent.START_SUM, _activeSumTilesManagerId);

                }
            } 
            // Entering the ExitDoor
            else if (hit.transform.gameObject.name.Equals("SumTileExit")) {
                _isInSumState = false;
                _facesController.StopSumState();
                var sumTileDoorComponent = hit.transform.GetComponent<SumTileDoor>();
                if (sumTileDoorComponent != null) {
                    Messenger<int>.Broadcast(GameEvent.END_SUM, sumTileDoorComponent.GetSumTilesManagerId());
                    _activeSumTilesManagerId = DEFAULT_SUM_TILES_MANAGER_ID;
                }
            } 
            // Moving on the board tiles
            else if (_isInSumState && hit.transform.gameObject.name.StartsWith("Tile")) {
                var tileFaceComponent = hit.transform.GetComponent<TileFace>();
                if (tileFaceComponent != null) {
                    var downwardFace = _facesController.GetDownwardFace();
                    // Add points to the track/path
                    if (tileFaceComponent.GetTileType() == TileType.Base) {
                        Messenger<int, int, GameObject>.Broadcast(GameEvent.ADD_TO_SUM, _activeSumTilesManagerId, downwardFace.GetFaceValue(), hit.transform.gameObject);
                    }
                    // Remove points from the track
                    else if (tileFaceComponent.GetTileType() == TileType.Sum) {
                        // If the player is on a track from another SumTilesManager, reset it
                        var downTileSumTilesManagerId = tileFaceComponent.GetSumTilesManagerId();
                        if (downTileSumTilesManagerId != _activeSumTilesManagerId) {
                            Messenger<int>.Broadcast(GameEvent.RESET_SUM, downTileSumTilesManagerId);
                            Messenger<int, int, GameObject>.Broadcast(GameEvent.ADD_TO_SUM, _activeSumTilesManagerId, downwardFace.GetFaceValue(), hit.transform.gameObject);
                        }

                        Messenger<int, GameObject>.Broadcast(GameEvent.REMOVE_FROM_SUM, tileFaceComponent.GetSumTilesManagerId(), hit.transform.gameObject);
                    }
                }
            }
        }

        _canRaycast = false;
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
