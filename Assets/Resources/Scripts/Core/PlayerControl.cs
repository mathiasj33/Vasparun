﻿using System;
using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed;

    private Invoker invoker;

    private FirstPersonCamera firstPersonCamera;
    private CharacterController characterControl;
    private WallrunControl wallrunControl;
    private RayCastHelper rayCastHelper;

    public bool Moving { get; private set; }
    private float yMovement;
    public bool IsGrounded { get { return characterControl.isGrounded; } }

    private bool jetpack = false;
    private bool jumpButtonReleased, jetpackAllowed;
    private bool invokedJetpackEnd = false;
    private bool playJetpack;

    private bool wallrun, cylinder, cylinderClockwise, justJumpedFromWall;
    private GameObject currentWall;
    private bool wallrunAllowed = true;

    void Start()
    {
        AudioListener.volume = 0;

        invoker = GetComponent<Invoker>();
        firstPersonCamera = GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>();
        characterControl = GetComponent<CharacterController>();
        wallrunControl = GetComponent<WallrunControl>();
        wallrunControl.enabled = false;
        rayCastHelper = GetComponent<RayCastHelper>();
    }

    void Update()
    {
        transform.forward = firstPersonCamera.Camera.transform.forward;
        ApplyWallrun();
        CheckJetpackAndJumpingAllowed();
        if (!wallrun) characterControl.Move(CalculateMovementVector() * Time.deltaTime * 60);
    }

    private void ApplyWallrun()
    {
        if (wallrun && (Input.GetButtonDown("Jump")))
        {
            EndWallrun();
            justJumpedFromWall = true;
        }
        if (!wallrunAllowed) return;

        justJumpedFromWall = false;
        WallInformation nearestWall;
        if (!cylinder) nearestWall = rayCastHelper.GetNearestWall();
        else
        {
            Vector3 direction = currentWall.transform.position - transform.position;
            nearestWall = rayCastHelper.GetWall(direction.normalized, cylinderClockwise);
        }
        bool isWall = nearestWall != null;
        if (isWall && !nearestWall.Allowed) return;
        if (isWall)
        {
            currentWall = nearestWall.GameObject;
            wallrunControl.Direction = nearestWall.WallDirection;
        }

        if (IsJumping() && isWall && nearestWall.Distance <= 2f)
        {
            InitiateWallrun(nearestWall);
        }
        else if (wallrun && !cylinder && (!isWall || nearestWall.Distance > 3f))
        {
            EndWallrun();
        }
    }

    private void InitiateWallrun(WallInformation wallInfo)
    {
        characterControl.enabled = false;
        wallrunControl.Direction = wallInfo.WallDirection;
        wallrunControl.enabled = true;
        wallrun = true;
        cylinder = wallInfo.Cylinder;
        cylinderClockwise = wallInfo.Right;
        firstPersonCamera.Rotate(wallInfo.Right);

        Globals.AudioSources.wallrun.Play();
    }

    private void EndWallrun()
    {
        wallrunControl.enabled = false;
        characterControl.enabled = true;
        wallrun = false;
        wallrunAllowed = false;
        cylinder = false;
        invoker.Invoke(.5f, () => wallrunAllowed = true);
        firstPersonCamera.RotateBack();
    }

    private Vector3 CalculateMovementVector()
    {
        ApplyGravity();
        ApplyJetpack();
        ApplyJump();
        Vector3 inputVec = GetInputVector();
        if (jetpack) inputVec *= 1.5f;
        return inputVec + new Vector3(0, yMovement, 0);
    }

    private void ApplyGravity()
    {
        if (IsJumping())
            yMovement -= 9.81f * Time.deltaTime / 20;
        else
            yMovement = -9.81f * 0.016f / 20;
    }

    private void ApplyJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (IsJumping() && !justJumpedFromWall) return;
            float factor = justJumpedFromWall ? 200 : 150f;
            if (justJumpedFromWall) yMovement = 0;
            yMovement += factor * 0.0008f;

            if (justJumpedFromWall) justJumpedFromWall = false;
        }
    }

    private void CheckJetpackAndJumpingAllowed()
    {
        if (IsJumping() && (Input.GetButtonUp("Jump"))) jumpButtonReleased = true;
        if (!IsJumping())
        {
            EnableJetpack();
        }
    }

    private void EnableJetpack()
    {
        jumpButtonReleased = false;
        jetpackAllowed = true;
        playJetpack = true;
        invokedJetpackEnd = false;
    }

    private void ApplyJetpack()
    {
        if (jumpButtonReleased && jetpackAllowed && IsJumping() && (Input.GetButton("Jump") ) && !justJumpedFromWall)
        {
            if (playJetpack)
            {
                Globals.AudioSources.jetpack.Play();
                playJetpack = false;
            }

            if (!invokedJetpackEnd)
            {
                invokedJetpackEnd = true;
                invoker.Invoke(.5f, () => StopJetpackIfStillInAir());
            }

            yMovement += Time.deltaTime;  //20 * Time.deltaTime / 20
            yMovement = Mathf.Min(yMovement, .05f);
            jetpack = true;
        }
        else if (!jetpackAllowed || !IsJumping())
        {
            jetpack = false;
        }
    }

    private void StopJetpackIfStillInAir()
    {
        if (IsJumping() && invokedJetpackEnd)
        {
            jetpackAllowed = false;
            jetpack = false;
        }
    }

    private Vector3 GetInputVector()
    {
        Vector3 dir = Vector3.zero;
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        float scale = speed * Time.deltaTime * 8;

        if (Util.FloatEquals(vertical, 0) && Util.FloatEquals(horizontal, 0)) Moving = false;
        else Moving = true;

        if (vertical > 0) dir += transform.forward;
        else if (vertical < 0) dir -= transform.forward * .3f;

        if (horizontal > 0) dir += transform.right * .6f;
        else if (horizontal < 0) dir -= transform.right * .6f;

        dir.y = 0;
        if(vertical > 0) dir.Normalize();
        dir *= speed * 8 * 0.016f;
        return dir;
    }

    public bool IsJumping()
    {
        return !characterControl.isGrounded && !wallrun;
    }
}
