using UnityEngine;

public class InitializeScript : MonoBehaviour
{

    void Start()
    {
        InitWorldColliders();
        InitHitWalls();
        InitWarpPoints();
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
        Material mat = Instantiate((Material)Resources.Load("Models/Materials/scifiWall"));
        foreach (Transform t in GameObject.Find("HitWalls").transform)
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

    private void InitWarpPoints()
    {
        Material mat = Instantiate((Material)Resources.Load("Models/Materials/warpPoint"));
        foreach (Transform t in GameObject.Find("WarpPoints").transform)
        {
            GameObject go = t.gameObject;
            go.AddComponent<MeshCollider>();
            go.tag = "WarpPoint";
            go.GetComponent<MeshRenderer>().material = mat;
            go.AddComponent<EmissionStrengthScript>();
        }
    }
}
