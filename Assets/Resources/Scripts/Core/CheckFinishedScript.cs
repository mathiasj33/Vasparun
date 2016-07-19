using UnityEngine;
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
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            GameObject.Find("Main").GetComponent<CheckBesttimesScript>().ShowBesttimes();
            Destroy(this);
        }
    }
}
