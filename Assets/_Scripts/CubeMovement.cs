using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour {
    [SerializeField]
    private float _speed = 300f;

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
    }
}
