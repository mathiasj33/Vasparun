using UnityEngine;
using System.Collections;

public class SlideOffControl : MonoBehaviour {

    private CharacterController characterControl;
    private PlayerControl playerControl;
    private RayCastHelper rayCastHelper;
    private int dir = 1;
    private bool moving;

	void Start () {
        characterControl = GetComponent<CharacterController>();
        playerControl = GetComponent<PlayerControl>();
        rayCastHelper = GetComponent<RayCastHelper>();
    }
	
	void Update () {
        GameObject go = rayCastHelper.GetSphereUnderPlayer();
        if (!moving && IsStandingOnWall(go)) {
            dir = Random.value > .5f ? 1 : -1;
            StartCoroutine("Move");
        }
	}

    private bool IsStandingOnWall(GameObject go)
    {
        return go != null && !playerControl.IsJumping() && Initializer.IsWallRunOnly(go);
    }

    private IEnumerator Move()
    {
        moving = true;
        for(int i = 0; i < 10; i++)
        {
            characterControl.Move(characterControl.transform.right * .1f * dir * Time.deltaTime * 60);
            yield return null;
        }
        moving = false;
    }
}
