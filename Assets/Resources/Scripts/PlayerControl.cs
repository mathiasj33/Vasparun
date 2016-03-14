using System;
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
        triggers = GetComponent<ControllerTriggers>();

        AudioSource[] sources = GetComponents<AudioSource>();
        footstepAudio = sources[1];
        landingAudio = sources[2];
        wallrunAudio = sources[3];
        jetpackAudio = sources[4];
        //TODO: irgendwo bug dass zu früh shootable detached wird, NoWallrun implementieren, camera drehung bug, wenn frames unlimited: landing bug, wände höher, jetpack indicator, Beta an andere und indiedb + twitter + website URL
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
        CheckJetpackAndJumpingAllowed();

        if (!wallrun)
            characterControl.Move(CalculateMovementVector() * Time.deltaTime * 60);

        ApplyHeadbob();
    }

    private void ApplyFootSounds()
    {
        footStepAudioTime += Time.deltaTime;
        if (Moving && footStepAudioTime > .5f && !IsJumping())
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
        if (Input.GetKeyUp(KeyCode.Alpha1) || characterControl.transform.position.y <= heightBeforeFall - 20) checkpointControl.Revert();
    }

    // TODO: Bug: Zu hoch springen? Manchmal runterfallen!!!! Camera bleibt gedreht? INDIEDB, BETA ETC, Camera position bei restart (selbst machen und config file); Danebenschießen: Zeit minus; Nicht benutze Assets alle löschen!!!
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
            nearestWall = findWallControl.GetNearestWall();
        }
        else
        {
            Vector3 direction = currentWall.transform.position - transform.position;
            nearestWall = findWallControl.GetWall(direction.normalized, cylinderRight);
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
        if (Input.GetButtonDown("Jump") || triggers.LeftTriggerDown)
        {
            if (IsJumping() && !justJumpedFromWall && !fellFromWall) return;
            float factor = justJumpedFromWall ? 200 : 150f;
            if (justJumpedFromWall) yMovement = 0;
            yMovement += factor * 0.0008f;

            if (justJumpedFromWall) justJumpedFromWall = false;
        }
    }

    private void CheckJetpackAndJumpingAllowed()
    {
        if (IsJumping() && (Input.GetButtonUp("Jump") || triggers.LeftTriggerUp)) jumpButtonReleased = true;
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
            dir += transform.right * .6f;  //TODO: If controller used: das hier nur mal .3f und sensitivity *= 3
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
}
