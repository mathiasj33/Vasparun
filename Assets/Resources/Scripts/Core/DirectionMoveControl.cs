using UnityEngine;

public class DirectionMoveControl : MonoBehaviour {

    private float speed = 1;
    public Vector3 Direction { get; set; }
    private bool running;
	
    public void StartWallrun(Vector3 dir)
    {
        speed = 1.4f;
        Direction = dir;
        running = true;
    }

    public void StartWarp(Vector3 dir)
    {
        speed = 5;
        Direction = dir;
        running = true;
    }

    public void Stop()
    {
        running = false;
    }

	void Update () {
        if (!running) return;
        transform.position += Direction * speed * Time.deltaTime * 7;
	}
}
