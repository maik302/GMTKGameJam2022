using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadCubeFacesController : MonoBehaviour {
    [SerializeField]
    private Color _defaultCubeColor;

    [SerializeField]
    private Color _sumTileColor;

    [Space]
    [Header("Faces")]
    [SerializeField]
    private GameObject _rightFace;
    [SerializeField]
    private GameObject _leftFace;
    [SerializeField]
    private GameObject _upFace;
    [SerializeField]
    private GameObject _downFace;
    [SerializeField]
    private GameObject _frontFace;
    [SerializeField]
    private GameObject _backFace;

    [Space]
    [Header("Faces lookahead")]
    [SerializeField]
    private GameObject _parentLookaheadContainer;
    [SerializeField]
    private GameObject _rightLookahead;
    [SerializeField]
    private GameObject _leftLookahead;
    [SerializeField]
    private GameObject _frontLookahead;
    [SerializeField]
    private GameObject _backLookahead;

    private Dictionary<Vector3, GameObject> _faces;

    private void Awake() {
        _faces = new Dictionary<Vector3, GameObject>();
    }

    void Start() {
        InitFacesDictionary();
        StopSumState();
    }

    void InitFacesDictionary() {
        _faces.Add(Vector3.right, _rightFace);
        _faces.Add(Vector3.left, _leftFace);
        _faces.Add(Vector3.up, _upFace);
        _faces.Add(Vector3.down, _downFace);
        _faces.Add(Vector3.forward, _frontFace);
        _faces.Add(Vector3.back, _backFace);
    }

    public void StartSumState() {
        var childrenRenderers = transform.GetComponentsInChildren<Renderer>();
        foreach (Renderer childrenRenderer in childrenRenderers) {
            childrenRenderer.material.color = _sumTileColor;
        }
    }

    public void StopSumState() {
        var childrenRenderers = transform.GetComponentsInChildren<Renderer>();
        foreach (Renderer childrenRenderer in childrenRenderers) {
            childrenRenderer.material.color = _defaultCubeColor;
        }
    }

    public void ShowLookahead() {
        var upwardDirection = Vector3.up;

        // Check what face is parallel to the down direction (in local space)
        // X-axis
        if ((int) Vector3.Cross(Vector3.up, transform.right).magnitude == 0) {
            // Check the actual face that is facing down
            if (Vector3.Dot(Vector3.up, transform.right) > 0f) {
                upwardDirection = Vector3.right;
            } else {
                upwardDirection = Vector3.left;
            }
        }
        // Y-axis
        else if ((int) Vector3.Cross(Vector3.up, transform.up).magnitude == 0) {
            if (Vector3.Dot(Vector3.up, transform.up) > 0f) {
                upwardDirection = Vector3.up;
            } else {
                upwardDirection = Vector3.down;
            }
        }
        // Z-axis
        else if ((int) Vector3.Cross(Vector3.up, transform.forward).magnitude == 0) {
            if (Vector3.Dot(Vector3.up, transform.forward) > 0f) {
                upwardDirection = Vector3.forward;
            } else {
                upwardDirection = Vector3.back;
            }
        }

        var upwardFace = _faces[upwardDirection];
        SetFacesLookahead(upwardFace.transform.forward);
    }

    void SetFacesLookahead(Vector3 upwardFaceDirection) {
        List<GameObject> GetAllFaces() {
            List<GameObject> perpendicularFaces = new List<GameObject>();
            perpendicularFaces.Add(_rightFace);
            perpendicularFaces.Add(_leftFace);
            perpendicularFaces.Add(_upFace);
            perpendicularFaces.Add(_downFace);
            perpendicularFaces.Add(_frontFace);
            perpendicularFaces.Add(_backFace);

            return perpendicularFaces;
        }

        List<GameObject> perpendicularFaces = new List<GameObject>();

        foreach (GameObject face in GetAllFaces()) {
            if ((int) Vector3.Dot(upwardFaceDirection, -face.transform.forward) == 0) {
                perpendicularFaces.Add(face);
            }
        }

        foreach (GameObject perpendicularFace in perpendicularFaces) {
            var perpendicularFaceQuadFaceComponent = perpendicularFace.GetComponent<QuadCubeFace>();

            if (perpendicularFaceQuadFaceComponent != null) {
                if ((int) Vector3.Dot(Vector3.right, -perpendicularFace.transform.forward) > 0) {
                    _rightLookahead.GetComponent<Renderer>().material.mainTexture = perpendicularFaceQuadFaceComponent.GetFaceTexture();
                } else if ((int) Vector3.Dot(Vector3.left, -perpendicularFace.transform.forward) > 0) {
                    _leftLookahead.GetComponent<Renderer>().material.mainTexture = perpendicularFaceQuadFaceComponent.GetFaceTexture();
                } else if ((int) Vector3.Dot(Vector3.forward, -perpendicularFace.transform.forward) > 0) {
                    _frontLookahead.GetComponent<Renderer>().material.mainTexture = perpendicularFaceQuadFaceComponent.GetFaceTexture();
                } else if ((int) Vector3.Dot(Vector3.back, -perpendicularFace.transform.forward) > 0) {
                    _backLookahead.GetComponent<Renderer>().material.mainTexture = perpendicularFaceQuadFaceComponent.GetFaceTexture();
                }
            }
        }

        _parentLookaheadContainer.transform.rotation = Quaternion.Euler(Vector3.zero);
        _parentLookaheadContainer.SetActive(true);
    }

    public void HideLookahead() {
        _parentLookaheadContainer.SetActive(false);
    }

    public QuadCubeFace GetDownwardFace() {
        var downwardDirection = Vector3.down;

        // Check what face is parallel to the down direction (in local space)
        // X-axis
        if ((int) Vector3.Cross(Vector3.down, transform.right).magnitude == 0) {
            // Check the actual face that is facing down
            if (Vector3.Dot(Vector3.down, transform.right) > 0f) {
                downwardDirection = Vector3.right;
            } else {
                downwardDirection = Vector3.left;
            }
        }
        // Y-axis
        else if ((int) Vector3.Cross(Vector3.down, transform.up).magnitude == 0) {
            if (Vector3.Dot(Vector3.down, transform.up) > 0f) {
                downwardDirection = Vector3.up;
            } else {
                downwardDirection = Vector3.down;
            }
        }
        // Z-axis
        else if ((int) Vector3.Cross(Vector3.down, transform.forward).magnitude == 0) {
            if (Vector3.Dot(Vector3.down, transform.forward) > 0f) {
                downwardDirection = Vector3.forward;
            } else {
                downwardDirection = Vector3.back;
            }
        }

        var downwardFace = _faces.ContainsKey(downwardDirection) ? _faces[downwardDirection].GetComponent<QuadCubeFace>() : null;
        return downwardFace;
    }
}
