using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CheckpointControl : MonoBehaviour //TODO: keine community maps, Website, screenshotsaturday, neural?
{
    private CheckpointManager checkpointManager;
    private RayCastHelper rayCastHelper;
    private Checkpoint beforeLastCheckpoint;
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
        if (go.tag == "Finish")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0);
        }

        Checkpoint checkpoint = checkpointManager.GetCheckpoint(go);
        if (checkpoint != null && checkpoint != lastCheckpoint)
        {
            if(beforeLastCheckpoint != null) beforeLastCheckpoint.DestroyAllWalls();
            if(lastCheckpoint != null) beforeLastCheckpoint = lastCheckpoint.Clone();
            lastCheckpoint = checkpoint;
        }
    }
}
