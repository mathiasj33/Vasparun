using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed;

    private FirstPersonCamera firstPersonCamera;
    private CharacterController characterControl;
    private WallrunControl wallrunControl;
    private FindWallControl findWallControl;

    private float yMovement;
    private bool jetpack = false;
    private bool jetpackAllowed = false;
    private bool wallrun, justJumpedFromWall = false;

    private float timeSinceWallrun;
    private float jetpackTime, timeSinceJetpack;

    private AudioSource wallrunAudio;
    private AudioSource jetpackAudio;
    private bool playJetpack;

    void Start()
    {
        firstPersonCamera = GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>();
        characterControl = GetComponent<CharacterController>();
        wallrunControl = GetComponent<WallrunControl>();
        wallrunControl.enabled = false;
        findWallControl = GetComponent<FindWallControl>();

        wallrunAudio = GetComponents<AudioSource>()[1];
        jetpackAudio = GetComponents<AudioSource>()[2];
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.K)) Respawn();

        timeSinceWallrun += Time.deltaTime;
        timeSinceJetpack += Time.deltaTime;

        transform.forward = firstPersonCamera.Camera.transform.forward;
        ApplyWallrun();
        CheckJetpackAllowed();
        if (!wallrun)
            characterControl.Move(CalculateMovementVector());

        Vector2 headBob = firstPersonCamera.CalculateHeadBob();
        Vector3 offset = firstPersonCamera.Camera.transform.right * headBob.x;
        offset.y = 0.9f + headBob.y;

        firstPersonCamera.Camera.transform.position = transform.position + offset;
    }

    private void Respawn()
    {
        characterControl.transform.position = new Vector3(0, 2, 0);
    }

    private void ApplyWallrun()
    {
        if (wallrun && Input.GetButtonDown("Jump"))
        {
            EndWallrun();
            justJumpedFromWall = true;
        }
        if (timeSinceWallrun < .5f) return;
        justJumpedFromWall = false;

        WallInformation nearestWall = findWallControl.GetNearestWall();
        bool isWall = nearestWall != null;
        //if(isWall) Debug.DrawLine(nearestWall.HitPoint, nearestWall.HitPoint + new Vector3(0, .1f, 0), Color.green, 1000, false);
        if (IsJumping() && isWall && nearestWall.Distance <= 1f)
        {
            InitiateWallrun(nearestWall);
        }
        else if (wallrun && (!isWall || nearestWall.Distance > 1f)) Bug: unendlich jetpack wenn kurz loslassen. Feature: Anstatt Timer: Coroutines
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
        firstPersonCamera.Rotate(wallInfo.Right);

        wallrunAudio.Play();
        Debug.Log("INIT");
    }

    private void EndWallrun()
    {
        wallrunControl.enabled = false;
        characterControl.enabled = true;
        wallrun = false;
        timeSinceWallrun = 0;
        firstPersonCamera.RotateBack();
        Debug.Log("END");
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
            if (IsJumping() && !justJumpedFromWall) return;
            float factor = justJumpedFromWall ? 200 : 150;
            if (justJumpedFromWall) yMovement = 0;
            yMovement += factor * Time.deltaTime / 20;
        }
    }

    private void CheckJetpackAllowed()
    {
        if (IsJumping() && Input.GetButtonUp("Jump")) jetpackAllowed = true;
        if (!IsJumping())
        {
            jetpackAllowed = false;
            playJetpack = true;
        }
    }

    private void ApplyJetpack()
    {
        if (jetpackAllowed && IsJumping() && Input.GetButton("Jump") && !justJumpedFromWall)
        {
            if(playJetpack)
            {
                jetpackAudio.Play();
                playJetpack = false;
            }
            if (timeSinceJetpack > 1 && jetpackTime > .5f) {
                ResetJetpack();
            }

            jetpackTime += Time.deltaTime;
            if (jetpackTime > .5f)
            {
                if (timeSinceJetpack > 1) timeSinceJetpack = 0;
                jetpack = false;
                return;
            }
            yMovement += 20 * Time.deltaTime / 20;
            yMovement = Mathf.Min(yMovement, .05f);
            jetpack = true;
        }
        else
        {
            ResetJetpack();
        }
    }

    private void ResetJetpack()
    {
        jetpackTime = 0;
        jetpack = false;
    }

    private Vector3 GetInputVector()
    {
        Vector3 dir = Vector3.zero;
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        float scale = speed * Time.deltaTime * 8;

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
}
