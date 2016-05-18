using UnityEngine;
using System.Collections;

public class RespawnControl : MonoBehaviour
{
    public bool infiniteMode = false;

    private PlayerControl playerControl;
    private float heightBeforeFall;
    private bool heightSet;

    private CheckpointControl checkpointControl;
    private InfiniteMapScript infiniteMapScript;

    void Start()
    {
        playerControl = gameObject.GetComponent<PlayerControl>();
        checkpointControl = GetComponent<CheckpointControl>();
        infiniteMapScript = GameObject.Find("Main").GetComponent<InfiniteMapScript>();
    }

    public void RespawnAt(Vector3 position)
    {
        transform.position = position;
        heightBeforeFall = float.MinValue;
        heightSet = false;
    }

    void Update()
    {
        if (!playerControl.IsJumping())
        {
            heightBeforeFall = float.MinValue;
            heightSet = false;
            return;
        }
        if (playerControl.IsJumping() && !heightSet)
        {
            heightBeforeFall = transform.position.y;
            heightSet = true;
        }
        if (Input.GetKeyUp(KeyCode.Alpha1) || transform.position.y <= heightBeforeFall - 20)
        {
            if (infiniteMode)
            {
                infiniteMapScript.Recreate();
                RespawnAt(new Vector3(0, 0, 0));
            }
            else checkpointControl.Revert();
        }
    }
}
