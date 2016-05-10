﻿using UnityEngine;
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
        foreach(Transform t in GameObject.Find("World").transform)
        {
            GameObject go = t.gameObject;
            Bounds bounds = go.GetComponent<MeshCollider>().bounds;
            if (IsCheckpointGameObject(go))
            {
                Checkpoint checkpoint = new Checkpoint(bounds.center + new Vector3(0, .5f, 0));
                checkpoints.Add(go, checkpoint);
                if (go.tag == "Untagged") go.tag = "NoWallrun";
            }
        }
    }

    private bool IsCheckpointGameObject(GameObject go)
    {
        Bounds bounds = go.GetComponent<MeshCollider>().bounds;
        return go.transform.rotation.eulerAngles.z == 0 && bounds.extents.y < .5f;
    }
}