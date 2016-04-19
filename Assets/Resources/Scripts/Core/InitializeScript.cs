using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitializeScript : MonoBehaviour
{

    void Start()
    {
        InitWorldColliders();
        InitHitWalls();
    }

    private void InitWorldColliders()
    {
        foreach (Transform t in GameObject.Find("World").transform)
        {
            t.gameObject.AddComponent<MeshCollider>();
        }
    }

    private void InitHitWalls()
    {
        foreach (Transform t in GameObject.Find("HitWalls").transform)
        {
            GameObject go = t.gameObject;
            go.tag = "NoWallrun";
            go.GetComponent<MeshRenderer>().material = Instantiate((Material)Resources.Load("Models/Materials/scifiWall"));
            go.AddComponent<ShootWallControl>();
            go.AddComponent<MeshCollider>();

            foreach (Transform c in t)
            {
                GameObject child = c.gameObject;
                child.tag = "HitWall";
                child.AddComponent<MeshCollider>();
            }
        }
    }
}
