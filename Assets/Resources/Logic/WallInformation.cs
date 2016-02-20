using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WallInformation
{
    public float Distance { get; set; }
    public Vector3 WallDirection { get; set; }
    public bool Right { get; set; }

    public Vector3 HitPoint { get; set; }

    public WallInformation(RaycastHit hit, bool right)
    {
        HitPoint = hit.point;
        Distance = hit.distance;
        WallDirection = right ? Vector3.Cross(Vector3.up, hit.normal) : Vector3.Cross(hit.normal, Vector3.up);
        WallDirection.Normalize();
        this.Right = right;
    }
}
