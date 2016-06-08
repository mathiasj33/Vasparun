using UnityEngine;
using System.Collections;

public class NewTileCheckerScript : MonoBehaviour {

    public bool NewTile { get; private set; }
    private RayCastHelper rayCastHelper;
    private bool tileAlreadyHandled;

	void Start () {
        rayCastHelper = GameObject.Find("Player").GetComponent<RayCastHelper>();
    }
	
	void LateUpdate () {
        CheckNewTile();
	}

    private void CheckNewTile()
    {
        NewTile = false;
        GameObject under = rayCastHelper.GetUnderPlayer();
        if (under != null && under.name == "A" && !tileAlreadyHandled) //TODO: nicht mit dem namen der tile machen sondern mit pieceXX vergleichen mit letztem piece
        {
            NewTile = true;
            tileAlreadyHandled = true;
        }
        else if (under == null || under.name != "A")
        {
            tileAlreadyHandled = false;
        }
    }
}
