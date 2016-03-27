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
    private CheckpointControl checkpointControl;
    private ControllerTriggers triggers;

    private bool moving;
    public bool Moving { get { return moving; } }
    private float yMovement;

    private bool jetpack = false;
    private bool jumpButtonReleased, jetpackAllowed = false;
    private bool invokedJetpackEnd = false;

    private bool wallrun, cylinder, cylinderRight, justJumpedFromWall, fellFromWall = false;
    private GameObject currentWall;
    private bool wallrunAllowed = true;

    private bool sliding;
    private bool justStoppedSliding, justJumpedFromRail;
    private Vector3 afterSlidingVelocity;

    private float heightBeforeFall;
    private bool heightSet;

    private AudioSource footstepAudio;
    private AudioSource landingAudio;
    private AudioSource wallrunAudio;
    private AudioSource jetpackAudio;
    private AudioSource slidingAudio;
    private bool playJetpack;

    private float footStepAudioTime;
    private bool wasInAir;

    void Start()
    {
        AudioListener.volume = 0;

        invoker = GetComponent<Invoker>();
        firstPersonCamera = GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>();
        characterControl = GetComponent<CharacterController>();
        wallrunControl = GetComponent<WallrunControl>();
        wallrunControl.enabled = false;
        rayCastHelper = GetComponent<RayCastHelper>();
        checkpointControl = GetComponent<CheckpointControl>();
        triggers = GetComponent<ControllerTriggers>();

        AudioSource[] sources = GetComponents<AudioSource>();
        footstepAudio = sources[1];
        landingAudio = sources[2];
        wallrunAudio = sources[3];
        jetpackAudio = sources[4];
        slidingAudio = sources[6];
    }

    public void RespawnAt(Vector3 position)
    {
        characterControl.transform.position = position;
        heightBeforeFall = float.MinValue;
        heightSet = false;
    }

    void Update()
    {
        ApplyFootSounds();
        ApplyRespawn();
        transform.forward = firstPersonCamera.Camera.transform.forward;
        ApplyWallrun();
        ApplyRail();
        CheckJetpackAndJumpingAllowed();
        CheckAfterSlidingVelocity();

        if (!wallrun && !sliding)
            characterControl.Move(CalculateMovementVector() * Time.deltaTime * 60);

        ApplyHeadbob();
    }

    public void StopSliding()
    {
        afterSlidingVelocity = gameObject.GetComponent<SlideScript>().Velocity;
        Destroy(gameObject.GetComponent<SlideScript>());
        slidingAudio.Stop();
        sliding = false;
        justStoppedSliding = true;
        EnableJetpack();
        invoker.Invoke(.5f, () => justStoppedSliding = false);
        StartCoroutine("DecreaseSlidingVelocity");
    }

    private void ApplyRail()
    {
        RailInformation info = rayCastHelper.GetRail();
        if (!sliding && info != null && !justStoppedSliding)
        {
            sliding = true;
            gameObject.AddComponent<SlideScript>();
            gameObject.GetComponent<SlideScript>().RailInformation = info;
            slidingAudio.Play();
        }
        if(sliding && Input.GetButtonDown("Jump"))
        {
            justJumpedFromRail = true;
            StopSliding();
        }
    }

    private void ApplyFootSounds()
    {
        footStepAudioTime += Time.deltaTime;
        if (!sliding && Moving && footStepAudioTime > .5f && !IsJumping())
        {
            footstepAudio.Play();
            footStepAudioTime = 0;
        }

        if (IsJumping()) wasInAir = true;
        if (characterControl.isGrounded && wasInAir)
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

        if (sliding) offset = new Vector3(0, 0.9f, 0);
        firstPersonCamera.Camera.transform.position = transform.position + offset;
    }

    private void ApplyRespawn()
    {
        if (!IsJumping() || sliding)
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
        if (Input.GetKeyUp(KeyCode.Alpha1) || characterControl.transform.position.y <= heightBeforeFall - 20) checkpointControl.Revert();
    }

    // TODO: Beta etc., Camera position bei restart (selbst machen und config file); Danebenschießen: Zeit minus; Nicht benutze Assets alle löschen!!!
    private void ApplyWallrun()
    {
        if (wallrun && (Input.GetButtonDown("Jump") || triggers.LeftTriggerDown || !IsMovingForwardsOrSidewards()))
        {
            fellFromWall = !IsMovingForwardsOrSidewards();
            EndWallrun();
            justJumpedFromWall = true;
        }
        if (!wallrunAllowed || fellFromWall) return;
        justJumpedFromWall = false;

        WallInformation nearestWall;
        if (!cylinder)
        {
            nearestWall = rayCastHelper.GetNearestWall();
        }
        else
        {
            Vector3 direction = currentWall.transform.position - transform.position;
            nearestWall = rayCastHelper.GetWall(direction.normalized, cylinderRight);
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
        inputVec += afterSlidingVelocity;
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
        if (Input.GetButtonDown("Jump") || triggers.LeftTriggerDown)
        {
            if (IsJumping() && !justJumpedFromWall && !fellFromWall && !justJumpedFromRail) return;
            float factor = justJumpedFromWall ? 200 : 150f;
            if (justJumpedFromRail) factor = 400;
            if (justJumpedFromWall || justJumpedFromRail) yMovement = 0;
            yMovement += factor * 0.0008f;

            if (justJumpedFromWall) justJumpedFromWall = false;
            if (justJumpedFromRail) justJumpedFromRail = false;
        }
    }

    private void CheckJetpackAndJumpingAllowed()
    {
        if (IsJumping() && (Input.GetButtonUp("Jump") || triggers.LeftTriggerUp)) jumpButtonReleased = true;
        if (!IsJumping())
        {
            fellFromWall = false;
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

    private void CheckAfterSlidingVelocity()
    {
        if(!IsJumping())
        {
            afterSlidingVelocity = Vector3.zero;
            StopCoroutine("DecreaseSlidingVelocity");
        }
    }

    private void ApplyJetpack()
    {
        if (jumpButtonReleased && jetpackAllowed && IsJumping() && (Input.GetButton("Jump") || Input.GetAxis("JumpAxis") == 1) && !justJumpedFromWall)
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
                    if (IsJumping() && invokedJetpackEnd)
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
            Debug.Log("applied jetpack");
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
            dir += transform.right * .6f;  //TODO: If controller used: das hier nur mal .3f und sensitivity *= 3. Erst aber controller support wegmachen
        }
        if (horizontal < 0)
        {
            dir -= transform.right * .6f;
        }
        dir.y = 0;
        if(vertical > 0) dir.Normalize();
        dir *= speed * 8 * 0.016f;
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

    private bool IsMovingForwardsOrSidewards()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (FloatEquals(vertical, 0) && FloatEquals(horizontal, 0)) return false;
        return true;
    }

    private IEnumerator DecreaseSlidingVelocity()
    {
        while(afterSlidingVelocity.magnitude > .1f)
        {
            afterSlidingVelocity /= 1.5f;
            yield return new WaitForSeconds(.1f) ;
        }
        afterSlidingVelocity = Vector3.zero;
    }
}
