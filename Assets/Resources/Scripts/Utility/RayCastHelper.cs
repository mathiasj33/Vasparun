﻿using UnityEngine;
using System.Collections;

public class RayCastHelper : MonoBehaviour
{

    public WallInformation GetNearestWall()
    {
        Vector3 camPos = transform.position + new Vector3(0, 0.9f, 0);

        RaycastHit rightHit;
        bool isRightHit = Physics.Raycast(camPos, transform.right, out rightHit);
        RaycastHit leftHit;
        bool isLeftHit = Physics.Raycast(camPos, -transform.right, out leftHit);

        if (!isRightHit && !isLeftHit) return null;
        if (isRightHit && !isLeftHit) return new WallInformation(rightHit, true);
        if (!isRightHit && isLeftHit) return new WallInformation(leftHit, false);
        if (isRightHit && isLeftHit)
        {
            return rightHit.distance < leftHit.distance ? new WallInformation(rightHit, true) : new WallInformation(leftHit, false);
        }
        return null;
    }

    public WallInformation GetWall(Vector3 direction, bool right)
    {
        Vector3 camPos = transform.position + new Vector3(0, 0.9f, 0);
        RaycastHit hit;
        bool isHit = Physics.Raycast(camPos, direction, out hit);

        if (!isHit) return null;
        return new WallInformation(hit, right);
    }

    public GameObject GetUnderPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    public GameObject GetSphereUnderPlayer()
    {
        RaycastHit hit;
        Vector3 characterMiddle = new Vector3(transform.position.x, transform.position.y + .9f, transform.position.z);
        if (Physics.SphereCast(characterMiddle, .5f, -Vector3.up, out hit))
        {
            return hit.collider.gameObject;
        }
        return null;
    }
}
