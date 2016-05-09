using System;
using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed;

    private Invoker invoker;

    private FirstPersonCamera firstPersonCamera;
    private CharacterController characterControl;
    private DirectionMoveControl moveControl;
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

    private bool warping, afterWarp;
    private Vector3 warpTarget;
    private Vector3 afterWarpVector;
    private GameObject warpPoint;

    void Start()
    {
        AudioListener.volume = 0;

        invoker = GetComponent<Invoker>();
        firstPersonCamera = GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>();
        characterControl = GetComponent<CharacterController>();
        moveControl = GetComponent<DirectionMoveControl>();
        rayCastHelper = GetComponent<RayCastHelper>();
    }

    void Update()
    {
        transform.forward = firstPersonCamera.Camera.transform.forward;
        ApplyWallrun();
        ApplyWarping();
        CheckJetpackAndJumpingAllowed();
        if (!wallrun && !warping) characterControl.Move(CalculateMovementVector() * Time.deltaTime * 60);
    }

    public void InitiateWarp(GameObject target)
    {
        if(warping != false) warpPoint.GetComponent<EmissionStrengthScript>().DecreaseEmissionColor();
        if (wallrun) EndWallrun();

        warpPoint = target;
        warpTarget = new Vector3(target.transform.position.x, target.transform.position.y - 2f, target.transform.position.z);
        warping = true;

        Vector3 dir = warpTarget - transform.position;
        dir.Normalize();
        moveControl.StartWarp(dir);

        target.GetComponent<EmissionStrengthScript>().IncreaseEmissionColor();
    }

    private void ApplyWallrun()
    {
        if (wallrun && (Input.GetButtonDown("Jump")))
        {
            EndWallrun();
            justJumpedFromWall = true;
        }
        if (!wallrunAllowed || warping) return;

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
        if (isWall && !warping)
        {
            currentWall = nearestWall.GameObject;
            moveControl.Direction = nearestWall.WallDirection;
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
        moveControl.StartWallrun(wallInfo.WallDirection);
        wallrun = true;
        cylinder = wallInfo.Cylinder;
        cylinderClockwise = wallInfo.Right;
        firstPersonCamera.Rotate(wallInfo.Right);

        Globals.AudioSources.wallrun.Play();
    }

    private void EndWallrun()
    {
        moveControl.Stop();
        characterControl.enabled = true;
        wallrun = false;
        wallrunAllowed = false;
        cylinder = false;
        invoker.Invoke(.5f, () => wallrunAllowed = true);
        firstPersonCamera.RotateBack();
    }

    private void ApplyWarping()
    {
        if(warping)
        {
            if(Vector3.Distance(transform.position, warpTarget) <= .5f)
            {
                moveControl.Stop();
                afterWarpVector = moveControl.Direction;
                StartCoroutine("DecreaseAfterWarpVector");
                warpPoint.GetComponent<EmissionStrengthScript>().DecreaseEmissionColor();
                warping = false;
                afterWarp = true;
            }
        }
    }

    private IEnumerator DecreaseAfterWarpVector()
    {
        yield return new WaitForSeconds(.01f);  //needed for y Movement not being null and thus "isGrounded" returning the correct result
        while (afterWarpVector.magnitude > .1f)
        {
            if(!IsJumping())
            {
                break;
            }
            afterWarpVector /= 1.5f;
            yield return new WaitForSeconds(.1f);
        }
        afterWarpVector = Vector3.zero;
        afterWarp = false;
    }

    private Vector3 CalculateMovementVector()
    {
        ApplyGravity();
        ApplyJetpack();
        ApplyJump();
        Vector3 inputVec = GetInputVector();
        if (jetpack) inputVec *= 1.5f;
        if (afterWarp) inputVec += afterWarpVector;
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
        if (jumpButtonReleased && jetpackAllowed && IsJumping() && (Input.GetButton("Jump") ) && !justJumpedFromWall && !afterWarp)
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
