using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ScoreScript : MonoBehaviour {

    public Canvas canvas;
    public Text distanceText;
    public Text minusText;

    private float minus;

    private Transform player;

    public void Reset()
    {
        minus = 0;
    }

	void Start () {
        player = GameObject.Find("Player").transform;
    }

    public void Minus()
    {

        FadeInAndOutScript fader = minusText.gameObject.AddComponent<FadeInAndOutScript>();
        fader.Canvas = canvas;
        fader.Text = minusText;
        minus -= 50;
    }
	
	void Update () {
        SetDistanceText();
    }

    private void SetDistanceText()
    {
        float distance = Vector3.Distance(player.position, Globals.DistanceOrigin);
        distance /= 2;
        distance -= 0.48f;
        distance += minus;
        distanceText.text = Math.Round(distance, 1) + " meters";
    }
}
