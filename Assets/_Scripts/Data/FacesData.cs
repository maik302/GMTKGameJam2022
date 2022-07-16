using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FacesData {

    // X-axis
    public int RightFace;
    public int LeftFace;
    // Y-axis
    public int UpFace;
    public int DownFace;
    // Z-axis
    public int FrontFace;
    public int BackFace;

    public FacesData(int rightFace, int leftFace, int upFace, int downFace, int frontFace, int backFace) {
        RightFace = rightFace;
        LeftFace = leftFace;
        UpFace = upFace;
        DownFace = downFace;
        FrontFace = frontFace;
        BackFace = backFace;
    }
}
