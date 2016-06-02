using UnityEngine;
using System.Collections;

public class RotateCameraScript : MonoBehaviour {

    private Vector3 pivot;

	void Start () {
        pivot = new Vector3(-86, 10.5f, 3.5f);
        transform.LookAt(pivot);
	}
	
	void Update () {
        transform.RotateAround(pivot, Vector3.up, Time.deltaTime * 3);
	}
}
