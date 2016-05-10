using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Initializer
{
    public static void Init(GameObject level)
    {
        InitWorldColliders(level);
        InitHitWalls(level);
        InitWarpPoints(level);
    }

    private static void InitWorldColliders(GameObject level)
    {
        foreach (Transform t in level.transform.Find("World"))
        {
            t.gameObject.AddComponent<MeshCollider>();
        }
    }

    private static void InitHitWalls(GameObject level)
    {
        Material mat = GameObject.Instantiate((Material)Resources.Load("Models/Materials/scifiWall"));
        foreach (Transform t in level.transform.Find("HitWalls"))
        {
            GameObject go = t.gameObject;
            go.tag = "NoWallrun";
            go.GetComponent<MeshRenderer>().material = mat;
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

    private static void InitWarpPoints(GameObject level)
    {
        Material mat = GameObject.Instantiate((Material)Resources.Load("Models/Materials/warpPoint"));
        foreach (Transform t in level.transform.Find("WarpPoints"))
        {
            GameObject go = t.gameObject;
            go.AddComponent<MeshCollider>();
            go.tag = "WarpPoint";
            go.GetComponent<MeshRenderer>().material = mat;
            go.AddComponent<EmissionStrengthScript>();
        }
    }
}

