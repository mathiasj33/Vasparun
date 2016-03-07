using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WallInformation
{
    public bool Allowed { get; set; }
    public GameObject GameObject { get; set; }
    public float Distance { get; set; }
    public Vector3 WallDirection { get; set; }
    public bool Right { get; set; }
    public bool Cylinder { get; set; }

    public WallInformation(RaycastHit hit, bool right)
    {
        GameObject = hit.collider.gameObject;
        Allowed = GameObject.tag != "NoWallrun" && GameObject.tag != "HitWall";
        Distance = hit.distance;
        WallDirection = right ? Vector3.Cross(Vector3.up, hit.normal) : Vector3.Cross(hit.normal, Vector3.up);
        WallDirection.Normalize();
        this.Right = right;
        this.Cylinder = GameObject.name.StartsWith("Cylinder");
    }
}
