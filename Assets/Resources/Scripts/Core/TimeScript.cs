using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TimeScript : MonoBehaviour {

    public Canvas canvas;
    public Text timeText;
    public Text plusText;

    private float time;
    private Transform player;

    public void Reset()
    {
        time = 0;
    }

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    public void Plus()
    {
        FadeInAndOutScript fader = plusText.gameObject.AddComponent<FadeInAndOutScript>();
        fader.Canvas = canvas;
        fader.Text = plusText;
        time += 5;
    }

    void Update()
    {
        time += Time.deltaTime;
        SetTimeText();
    }

    private void SetTimeText()
    {
        timeText.text = TimeFormatter.Format(time);
    }
}
