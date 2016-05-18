using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class DistanceScript : MonoBehaviour {

    public Text text;

    private Transform player;

	void Start () {
        player = GameObject.Find("Player").transform;
	}
	
	void Update () {
        float distance = Vector3.Distance(player.position, Globals.DistanceOrigin);
        distance /= 2;
        distance -= 0.48f;
        text.text = Math.Round(distance, 1) + " meters";
    }
}
