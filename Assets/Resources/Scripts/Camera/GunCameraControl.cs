using UnityEngine;
using System.Collections;

public class GunCameraControl : MonoBehaviour {

    public Camera mainCamera;

	void Start () {
	
	}
	
	void LateUpdate () {
        transform.position = mainCamera.transform.position;
        transform.rotation = mainCamera.transform.rotation;
    }
}
