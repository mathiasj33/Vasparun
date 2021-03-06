﻿using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class FirstPersonCamera : MonoBehaviour
{
    public GameObject Camera { get { return gameObject; } }

    private PlayerControl playerControl;

    private float rotationY = 0.0f;
    private float rotationX = 0.0f;
    private float rotationZ = 0.0f;

    private bool rotateRight;
    private float wallrunRotateSpeed = 1.5f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rotationY = transform.localRotation.eulerAngles.y;

        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
    }

    void Update()
    {
        rotationY += Input.GetAxis("Mouse X") * (Globals.Sensitivity / 10);
        rotationX += Input.GetAxis("Mouse Y") * (Globals.Sensitivity / 10);
        rotationX = Mathf.Clamp(rotationX, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(rotationY, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationX, Vector3.left);
        transform.localRotation *= Quaternion.AngleAxis(rotationZ, Vector3.forward);

        Vector2 headBob = CalculateHeadBob(playerControl.Moving);
        Vector3 offset = transform.right * headBob.x;
        offset.y = 0.9f + headBob.y;
        transform.position = playerControl.gameObject.transform.position + offset;
    }

    private Vector2 CalculateHeadBob(bool moving)
    {
        Vector2 bob = new Vector2();
        float xPeriod = moving ? 7 : 3.5f;
        float yPeriod = moving ? 4 : 2;

        bob.x = (float)Mathf.Sin(Time.time * xPeriod) * .15f;
        bob.y = (float)Mathf.Sin((Time.time + 0.3f) * yPeriod) * .1f;

        if (!moving) bob /= 2;
        return bob;
    }

    public void Rotate(bool right)
    {
        rotateRight = right;
        StopCoroutine("RotateCameraBack");
        StartCoroutine("RotateCamera");
    }

    public void RotateBack()
    {
        StopCoroutine("RotateCamera");
        StartCoroutine("RotateCameraBack");
    }

    private IEnumerator RotateCamera()
    {
        int sign = rotateRight ? 1 : -1;
        while (rotationZ < 13 && rotationZ > -13)
        {
            rotationZ += Time.deltaTime * 60 * sign * wallrunRotateSpeed;
            yield return null;
        }
    }

    private IEnumerator RotateCameraBack()
    {
        int sign = rotateRight ? -1 : 1;
        if (sign == -1)
        {
            while (rotationZ > 0)
            {
                rotationZ += Time.deltaTime * 60 * sign * wallrunRotateSpeed;
                yield return null;
            }
        }
        else if (sign == 1)
        {
            while (rotationZ < 0)
            {
                rotationZ += Time.deltaTime * 60 * sign * wallrunRotateSpeed;
                yield return null;
            }

        }
        rotationZ = 0;
    }
}
