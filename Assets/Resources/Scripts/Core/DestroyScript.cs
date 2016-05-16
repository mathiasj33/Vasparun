using UnityEngine;
using System.Collections;

public class DestroyScript : MonoBehaviour {

    private Transform player;

	void Start () {
        player = GameObject.Find("Player").transform;
	}
	
	void Update () {
        if(Vector3.Distance(transform.position, player.position) >= 400)
        {
            Destroy(gameObject);
        }
	}
}
