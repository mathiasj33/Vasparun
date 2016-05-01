using UnityEngine;
using System.Collections;

public class GunMovementControl : MonoBehaviour {

    private PlayerControl playerControl;

    public float swayX = 10f;
    public float swayY = 5f;
    public float maxSwayAmount = 20f;

    private float bobTime;
    private float bobFactor = .5f;
    private bool wasStanding;
    private bool wasMoving;

    private Animator animator;

    private Vector3 startRotation;
    private Vector3 startPosition;

    void Start () {
        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        animator = GetComponent<Animator>();

        startRotation = transform.localRotation.eulerAngles;
        startPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        Sway();
        CheckBobFactor();
        CheckBobTime();
        Bob();
    }

    private void Sway()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Rotated")) return;

        float factorX = (Input.GetAxis("Mouse X")) * swayX;
        float factorY = -(Input.GetAxis("Mouse X")) * swayY;

        factorX = Mathf.Clamp(factorX, -maxSwayAmount, maxSwayAmount);
        factorY = Mathf.Clamp(factorY, -maxSwayAmount, maxSwayAmount);

        Vector3 newRotation = new Vector3(startRotation.x + factorX, startRotation.y + factorY, startRotation.z);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(newRotation), 0.16f);
    }

    private void CheckBobFactor()
    {
        if (playerControl.Moving && bobFactor < 1)
        {
            bobFactor += Time.deltaTime;
        }
        else if (!playerControl.Moving && bobFactor > .5f)
        {
            bobFactor -= Time.deltaTime;
        }
    }

    private void CheckBobTime()
    {
        if (!playerControl.Moving) wasStanding = true;
        else wasMoving = true;

        if (playerControl.Moving && wasStanding)
        {
            wasStanding = false;
            bobTime /= 2;
        }
        if (!playerControl.Moving && wasMoving)
        {
            wasMoving = false;
            bobTime *= 2;
        }
    }

    private void Bob()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Rotated")) return;

        bobTime += Time.deltaTime;

        Vector2 bob = CalculateBob();
        Vector3 offset = transform.forward * bob.x;
        offset.y = bob.y;

        transform.localPosition = startPosition + offset;
    }

    private Vector2 CalculateBob()
    {
        Vector2 bob = new Vector2();

        float xPeriod = 1.75f;
        float yPeriod = 1;

        if (playerControl.Moving)
        {
            xPeriod *= 2;
            yPeriod *= 2;
        }

        bob.x = (float)Mathf.Sin(bobTime * xPeriod) * .02f * bobFactor;
        bob.y = (float)Mathf.Sin((bobTime + 0.3f) * yPeriod) * .03f * bobFactor;

        return bob;
    }
}
