using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointControl : MonoBehaviour
{
    public Checkpoint LastCheckpoint { get; private set; }

    private CheckpointManager checkpointManager;
    private RayCastHelper rayCastHelper;

    private RespawnControl respawnControl;

    void Start()
    {
        checkpointManager = GameObject.Find("Main").GetComponent<CheckpointManager>();
        rayCastHelper = GetComponent<RayCastHelper>();
        respawnControl = GameObject.Find("Player").GetComponent<RespawnControl>();
        InvokeRepeating("FindCheckpoint", 0, .2f);
    }

    public void Revert()
    {
        LastCheckpoint.Revert();
        respawnControl.RespawnAt(LastCheckpoint.Position);
    }

    public void AddToRevertSet(ShootWallControl control)
    {
        LastCheckpoint.AddToRevertSet(control);
    }

    private void FindCheckpoint()
    {
        GameObject go = rayCastHelper.GetUnderPlayer();
        if (go == null || go.tag == "Finish") return;

        Checkpoint checkpoint = checkpointManager.GetCheckpoint(go);
        if (checkpoint != null && checkpoint != LastCheckpoint && !checkpoint.AlreadyFinished)
        {
            LastCheckpoint = checkpoint;
            checkpoint.AlreadyFinished = true;
        }
    }
}
