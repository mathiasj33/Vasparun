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
    private RayCastHelper findWallControl;
    private CheckpointControl checkpointControl;

    private bool moving;
    private float yMovement;

    private bool jetpack = false;
    private bool jumpButtonReleased, jetpackAllowed = false;
    private bool invokedJetpackEnd = false;

    private bool wallrun, cylinder, cylinderRight, justJumpedFromWall, fellFromWall = false;
    private GameObject currentWall;
    private bool wallrunAllowed = true;

    private float heightBeforeFall;
    private bool heightSet;

    private AudioSource footstepAudio;
    private AudioSource landingAudio;
    private AudioSource wallrunAudio;
    private AudioSource jetpackAudio;
    private bool playJetpack;

    private float footStepAudioTime;
    private bool wasInAir;

    void Start()
    {
        //AudioListener.volume = 0;

        invoker = GetComponent<Invoker>();
        firstPersonCamera = GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>();
        characterControl = GetComponent<CharacterController>();
        wallrunControl = GetComponent<WallrunControl>();
        wallrunControl.enabled = false;
        findWallControl = GetComponent<RayCastHelper>();
        checkpointControl = GetComponent<CheckpointControl>();

        AudioSource[] sources = GetComponents<AudioSource>();
        footstepAudio = sources[1];
        landingAudio = sources[2];
        wallrunAudio = sources[3];
        jetpackAudio = sources[4];
    }

    void Update()
    {
        ApplyFootSounds();
        ApplyRespawn();
        transform.forward = firstPersonCamera.Camera.transform.forward;
        ApplyWallrun();
        CheckJetpackAllowed();

        if (!wallrun)
            characterControl.Move(CalculateMovementVector());

        ApplyHeadbob();
    }

    private void ApplyFootSounds()
    {
        footStepAudioTime += Time.deltaTime;
        if (footStepAudioTime > .5f && !IsJumping())
        {
            footstepAudio.Play();
            footStepAudioTime = 0;
        }

        if (IsJumping()) wasInAir = true;
        if(characterControl.isGrounded && wasInAir)
        {
            landingAudio.Play();
            wasInAir = false;
        }
    }

    private void ApplyHeadbob()
    {
        Vector2 headBob = firstPersonCamera.CalculateHeadBob(moving);
        Vector3 offset = firstPersonCamera.Camera.transform.right * headBob.x;
        offset.y = 0.9f + headBob.y;

        firstPersonCamera.Camera.transform.position = transform.position + offset;
    }

    private void ApplyRespawn()
    {
        if (!IsJumping())
        {
            heightBeforeFall = float.MinValue;
            heightSet = false;
            return;
        }
        if (IsJumping() && !heightSet)
        {
            heightBeforeFall = characterControl.transform.position.y;
            heightSet = true;
        }
        if (Input.GetKeyUp(KeyCode.Alpha1) || characterControl.transform.position.y <= heightBeforeFall - 20) Respawn();
    }

    private void Respawn()
    {
        characterControl.transform.position = checkpointControl.LastCheckpoint.Position;
        heightBeforeFall = float.MinValue;
        heightSet = false;
    }

    //TODO: weapon sway und bob, Beta
    // TODO: Bug: Zu hoch springen? INDIEDB, BETA ETC., Design folgen, Camera position bei restart (selbst machen und config file), Weapon sway und bob
    private void ApplyWallrun()
    {
        if (wallrun && (Input.GetButtonDown("Jump") || Input.GetAxis("Vertical") <= 0))
        {
            fellFromWall = Input.GetAxis("Vertical") <= 0;
            EndWallrun();
            justJumpedFromWall = true;
        }
        if (!wallrunAllowed || fellFromWall) return;
        justJumpedFromWall = false;

        WallInformation nearestWall;
        if (!cylinder)
        {
            nearestWall = findWallControl.GetNearestWall();
        }
        else
        {
            Vector3 direction = currentWall.transform.position - transform.position;
            nearestWall = findWallControl.GetWall(direction.normalized, cylinderRight);
        }
        bool isWall = nearestWall != null;
        if (isWall)
        {
            currentWall = nearestWall.GameObject;
            wallrunControl.Direction = nearestWall.WallDirection;
        }

        if (IsJumping() && isWall && nearestWall.Distance <= 1.5f)
        {
            InitiateWallrun(nearestWall);
        }
        else if (wallrun && !cylinder && (!isWall || nearestWall.Distance > 2f))
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
        cylinderRight = wallInfo.Right;
        firstPersonCamera.Rotate(wallInfo.Right);

        wallrunAudio.Play();
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
            yMovement = -9.81f * Time.deltaTime / 20;
    }

    private void ApplyJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (IsJumping() && !justJumpedFromWall && !fellFromWall) return;
            float factor = justJumpedFromWall ? 200 : 150;
            if (justJumpedFromWall) yMovement = 0;
            yMovement += factor * Time.deltaTime / 20;

            if (justJumpedFromWall) justJumpedFromWall = false;
        }
    }

    private void CheckJetpackAllowed()
    {
        if (IsJumping() && Input.GetButtonUp("Jump")) jumpButtonReleased = true;
        if (!IsJumping())
        {
            jumpButtonReleased = false;
            jetpackAllowed = true;
            playJetpack = true;
            invokedJetpackEnd = false;
            fellFromWall = false;
        }
    }

    private void ApplyJetpack()
    {
        if (jumpButtonReleased && jetpackAllowed && IsJumping() && Input.GetButton("Jump") && !justJumpedFromWall)
        {
            if (playJetpack)
            {
                jetpackAudio.Play();
                playJetpack = false;
            }

            if (!invokedJetpackEnd)
            {
                invokedJetpackEnd = true;
                invoker.Invoke(.5f, () =>
                {
                    if(IsJumping() && invokedJetpackEnd)
                    {
                        jetpackAllowed = false;
                        jetpack = false;
                    }
                });
            }

            yMovement += 20 * Time.deltaTime / 20;
            yMovement = Mathf.Min(yMovement, .05f);
            jetpack = true;
            fellFromWall = false;
        }
        else if (!jetpackAllowed || !IsJumping())
        {
            jetpack = false;
        }
    }

    private Vector3 GetInputVector()
    {
        Vector3 dir = Vector3.zero;
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        float scale = speed * Time.deltaTime * 8;

        if (FloatEquals(vertical, 0) && FloatEquals(horizontal, 0)) moving = false;
        else moving = true;

        if (vertical > 0)
        {
            dir += transform.forward;
        }
        if (vertical < 0)
        {
            dir -= transform.forward * .3f;
        }
        if (horizontal > 0)
        {
            dir += transform.right * .6f;
        }
        if (horizontal < 0)
        {
            dir -= transform.right * .6f;
        }
        dir.y = 0;
        dir *= scale;
        return dir;
    }

    private bool IsJumping()
    {
        return !characterControl.isGrounded && !wallrun;
    }

    private bool FloatEquals(float f1, float f2)
    {
        return Mathf.Abs(f1 - f2) < 0.01f;
    }

    
}
