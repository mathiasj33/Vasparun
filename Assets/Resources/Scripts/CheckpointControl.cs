using UnityEngine;
using System.Collections;

public class CheckpointControl : MonoBehaviour
{
    private CheckpointManager checkpointManager;
    private RayCastHelper rayCastHelper;
    private Checkpoint lastCheckpoint;

    public Checkpoint LastCheckpoint { get { return lastCheckpoint; } }

    private PlayerControl playerControl;

    void Start()
    {
        checkpointManager = GameObject.Find("Main").GetComponent<CheckpointManager>();
        rayCastHelper = GetComponent<RayCastHelper>();
        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        InvokeRepeating("FindCheckpoint", 0, .2f);
    }

    public void Revert()
    {
        lastCheckpoint.Revert();
        playerControl.RespawnAt(lastCheckpoint.Position);
    }

    public void AddToRevertSet(ShootWallControl control)
    {
        lastCheckpoint.AddToRevertSet(control);
    }

    private void FindCheckpoint()
    {
        GameObject go = rayCastHelper.GetUnderPlayer();
        if (go == null) return;

        Checkpoint checkpoint = checkpointManager.GetCheckpoint(go);
        if (checkpoint != null && checkpoint != lastCheckpoint)
        {
            if(lastCheckpoint != null) lastCheckpoint.DestroyAllWalls();
            lastCheckpoint = checkpoint;
        }
    }
}
