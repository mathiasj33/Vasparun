﻿using UnityEngine;
using System.Collections;

public class CheckFinishedScript : MonoBehaviour {

    private RayCastHelper rayCastHelper;
    private PlayerControl playerControl;

	void Start () {
        rayCastHelper = GetComponent<RayCastHelper>();
        playerControl = GetComponent<PlayerControl>();
    }
	
	void Update () {
        GameObject go = rayCastHelper.GetUnderPlayer();
        if (go == null) return;
        if (go.tag == "Finish" && !playerControl.IsJumping())
        {
            GameObject.Find("Main").GetComponent<BesttimesScript>().ShowBesttimes();
            Destroy(this);
        }
    }
}
