using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour {

    private IDictionary<GameObject, Checkpoint> checkpoints = new Dictionary<GameObject, Checkpoint>();

	void Start () {
        AddCheckpointObjects();
	}

    public Checkpoint GetCheckpoint(GameObject go)
    {
        if (checkpoints.ContainsKey(go)) return checkpoints[go];
        return null;
    }

    private void AddCheckpointObjects()
    {
        Initializer.GetCheckpointGameObjects(GameObject.Find("World")).ForEach(go =>
        {
            if (go.tag == "Untagged") go.tag = "NoWallrun";

            Bounds bounds = go.GetComponent<MeshCollider>().bounds;
            Checkpoint checkpoint = new Checkpoint(bounds.center + new Vector3(0, .5f, 0));
            checkpoints.Add(go, checkpoint);
        });
    }
}
