using UnityEngine;
using System.Collections;

public class RespawnControl : MonoBehaviour
{

    private PlayerControl playerControl;
    private float heightBeforeFall;
    private bool heightSet;

    private CheckpointControl checkpointControl;

    void Start()
    {
        playerControl = gameObject.GetComponent<PlayerControl>();
        checkpointControl = GetComponent<CheckpointControl>();
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
        if (Input.GetKeyUp(KeyCode.Alpha1) || transform.position.y <= heightBeforeFall - 20) checkpointControl.Revert();
    }
}
