using UnityEngine;
using System.Collections;

public class FindWallControl : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
	
	}

    public WallInformation GetNearestWall()
    {
        Vector3 camPos = transform.position + new Vector3(0, 0.9f, 0);

        RaycastHit rightHit;
        bool isRightHit = Physics.Raycast(camPos, transform.right, out rightHit);
        RaycastHit leftHit;
        bool isLeftHit = Physics.Raycast(camPos, -transform.right, out leftHit);

        if (!isRightHit && !isLeftHit) return null;
        if (isRightHit && !isLeftHit) return new WallInformation(rightHit, true);
        if (!isRightHit && isLeftHit) return new WallInformation(leftHit, false);
        if(isRightHit && isLeftHit)
        {
            return rightHit.distance < leftHit.distance ? new WallInformation(rightHit, true) : new WallInformation(leftHit, false);
        }
        return null;
    }
}
