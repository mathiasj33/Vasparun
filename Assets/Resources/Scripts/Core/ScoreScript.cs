using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ScoreScript : MonoBehaviour {  //TODO: score nur einmal pro tile etc.

    public Text pointsText;
    public Text distanceText;

    private NewTileCheckerScript newTileChecker;
    private Transform player;

    private float score = -24135.74f;
    private float lastTime;

	void Start () {
        player = GameObject.Find("Player").transform;
        newTileChecker = GameObject.Find("Main").GetComponent<NewTileCheckerScript>();
    }

    public void Reset()
    {
        score = 0;
    }
	
	void Update () {
        SetScoreText();
        SetDistanceText();
    }

    private void SetScoreText()
    {
       if(newTileChecker.NewTile)  //TODO: delta in ui anzeigen
        {
            float distance = Vector3.Distance(player.position, Globals.LastTileStart);
            float time = Time.time - lastTime;
            float deltaPoints = distance / (time * time) * 10;
            score += deltaPoints;

            Globals.LastTileStart = player.position;
            lastTime = Time.time;

            pointsText.text = "Score: " + Math.Round(score, 0);
        }
    }

    private void SetDistanceText()
    {
        float distance = Vector3.Distance(player.position, Globals.DistanceOrigin);
        distance /= 2;
        distance -= 0.48f;
        distanceText.text = Math.Round(distance, 1) + " meters";
    }
}
