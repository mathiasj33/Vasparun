using UnityEngine;
using System.Collections;

public class CheckpointControl : MonoBehaviour
{

    private CheckpointManager manager;
    private RayCastHelper rayCastHelper;
    private Checkpoint lastCheckpoint;

    public Checkpoint LastCheckpoint { get { return lastCheckpoint; } }

    void Start()
    {
        manager = GameObject.Find("Main").GetComponent<CheckpointManager>();
        rayCastHelper = GetComponent<RayCastHelper>();
        InvokeRepeating("FindCheckpoint", 0, .2f);
    }

    private void FindCheckpoint()
    {
        GameObject go = rayCastHelper.GetUnderPlayer();
        if (go == null) return;

        Checkpoint? checkpoint = manager.GetCheckpoint(go);
        if (checkpoint.HasValue) lastCheckpoint = checkpoint.Value;
    }
}
