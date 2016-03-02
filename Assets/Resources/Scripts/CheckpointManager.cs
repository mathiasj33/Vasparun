using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour {

    private IDictionary<GameObject, Checkpoint> checkpoints = new Dictionary<GameObject, Checkpoint>();

	void Start () {
        AddCheckpointObjects();
	}

    public Checkpoint? GetCheckpoint(GameObject go)
    {
        if (checkpoints.ContainsKey(go)) return checkpoints[go];
        return null;
    }

    private void AddCheckpointObjects()
    {
        foreach(Transform t in GameObject.Find("level1").transform)
        {
            GameObject go = t.gameObject;
            Bounds bounds = go.GetComponent<MeshCollider>().bounds;
            if (IsCheckpointGameObject(go))
            {
                Checkpoint checkpoint = new Checkpoint();
                checkpoint.Position = bounds.center + new Vector3(0, .5f, 0);
                checkpoints.Add(go, checkpoint);
            }
        }
    }

    private bool IsCheckpointGameObject(GameObject go)
    {
        Bounds bounds = go.GetComponent<MeshCollider>().bounds;
        return go.transform.rotation.eulerAngles.z == 0 && bounds.extents.y < .5f;
    }
}
