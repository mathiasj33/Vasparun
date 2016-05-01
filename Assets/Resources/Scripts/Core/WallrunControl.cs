using UnityEngine;

public class WallrunControl : MonoBehaviour {

    public float speed = 1;
    public Vector3 Direction { get; set; }
	
	void Update () {
        transform.position += Direction * speed * Time.deltaTime * 7;
	}
}
