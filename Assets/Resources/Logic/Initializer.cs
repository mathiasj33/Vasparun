using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Initializer
{
    public static void Init(GameObject level, bool infiniteMode)
    {
        InitWorldColliders(level);
        InitHitWalls(level, infiniteMode);
        InitWarpPoints(level);
    }

    public static List<GameObject> GetCheckpointGameObjects(GameObject level)
    {
        List<GameObject> gos = new List<GameObject>();
        foreach (Transform t in level.transform)
        {
            GameObject go = t.gameObject;
            if (IsCheckpointGameObject(go))
            {
                gos.Add(go);
            }
        }
        return gos;
    }

    public static bool IsWallRunOnly(GameObject go)
    {
        return !IsCheckpointGameObject(go) && !go.name.StartsWith("Cylinder") && !go.name.StartsWith("Cube");
    }

    private static bool IsCheckpointGameObject(GameObject go)
    {
        Bounds bounds = go.GetComponent<MeshCollider>().bounds;
        return go.transform.rotation.eulerAngles.z == 0 && bounds.extents.y < .5f;
    }

    private static void InitWorldColliders(GameObject level)
    {
        foreach (Transform t in level.transform.Find("World"))
        {
            MeshCollider collider = t.gameObject.AddComponent<MeshCollider>();
        }
    }

    private static void InitHitWalls(GameObject level, bool infiniteMode)
    {
        Material mat = GameObject.Instantiate((Material)Resources.Load("Models/Materials/scifiWall"));
        foreach (Transform t in level.transform.Find("HitWalls"))
        {
            GameObject go = t.gameObject;
            go.tag = "NoWallrun";
            go.GetComponent<MeshRenderer>().material = mat;
            ShootWallControl control = go.AddComponent<ShootWallControl>();
            control.infiniteMode = infiniteMode;
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

