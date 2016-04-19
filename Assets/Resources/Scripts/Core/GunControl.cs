using UnityEngine;
using System.Collections;

public class GunControl : MonoBehaviour
{
    public Camera cam;
    private PlayerControl playerControl;

    public float swayX = 10f;
    public float swayY = 5f;
    public float maxSwayAmount = 20f;

    private float bobTime;
    private float bobFactor = .5f;
    private bool wasStanding;
    private bool wasMoving;

    private Invoker invoker;
    private bool shootingAllowed = true;

    private ParticleSystem gunParticles;
    private Transform start;
    private Animator animator;

    private Vector3 startRotation;
    private Vector3 startPosition;

    void Start()
    {
        invoker = GameObject.Find("Player").GetComponent<Invoker>();
        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();

        gunParticles = GameObject.Find("LaserParticle").GetComponent<ParticleSystem>();
        start = GameObject.Find("Tip").transform;
        animator = GetComponent<Animator>();

        startRotation = transform.localRotation.eulerAngles;
        startPosition = transform.localPosition;
    }

    void Update()
    {
        CheckShooting();
    }

    void LateUpdate()
    {
        Sway();
        CheckBobFactor();
        CheckBobTime();
        Bob();
    }

    private void CheckShooting()
    {
        if (shootingAllowed && (Input.GetButtonDown("Shoot")))
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;

            LineRenderer laser = CreateLaser();

            if (Physics.Raycast(ray, out hit) && Vector3.Distance(hit.point, gameObject.transform.position) < 50)
            {
                laser.SetPosition(1, hit.point);
                GameObject go = hit.collider.gameObject;
                if (go.tag == "HitWall")
                {
                    go.transform.parent.gameObject.GetComponent<ShootWallControl>().Shoot();
                    go.AddComponent<FadeOutScript>();
                    go.GetComponent<MeshCollider>().enabled = false;
                }
                else if(go.tag == "WarpPoint")
                {
                    Debug.Log("Warp");
                }
            }
            else
            {
                laser.SetPosition(1, ray.GetPoint(50));
            }
            PlayShootEffects(laser);
            shootingAllowed = false;
            invoker.Invoke(.3f, () => shootingAllowed = true);
        }
    }

    private void PlayShootEffects(LineRenderer laser)
    {
        gunParticles.Play();
        Globals.AudioSources.laser.Play();
        animator.SetTrigger("Shoot");
        StartCoroutine(FadeLaserOut(laser));
    }

    private IEnumerator FadeLaserOut(LineRenderer laser)
    {
        float time = 0;
        float alpha = 1;
        while (alpha > 0)
        {
            time += Time.deltaTime;
            alpha = 1 - time;

            laser.SetColors(new Color(255, 0, 0, alpha), new Color(255, 30, 0, alpha));
            yield return null;
        }
        Destroy(laser.gameObject);
    }

    private LineRenderer CreateLaser()
    {
        GameObject empty = new GameObject();
        empty.AddComponent<LineRenderer>();
        LineRenderer laser = empty.GetComponent<LineRenderer>();

        laser.sharedMaterial = (Material)Resources.Load("Models/Materials/laser");
        laser.SetColors(new Color(255, 0, 0, 1), new Color(255, 30, 0, 1));
        laser.SetWidth(0.2f, 0.01f);
        laser.SetPosition(0, start.position);
        return laser;
    }

    private void Sway()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Rotated")) return;

        float factorX = (Input.GetAxis("Mouse X")) * swayX;
        float factorY = -(Input.GetAxis("Mouse X")) * swayY;

        if (factorX > maxSwayAmount)
            factorX = maxSwayAmount;

        if (factorX < -maxSwayAmount)
            factorX = -maxSwayAmount;

        if (factorY > maxSwayAmount)
            factorY = maxSwayAmount;

        if (factorY < -maxSwayAmount)
            factorY = -maxSwayAmount;

        Vector3 newRotation = new Vector3(startRotation.x + factorX, startRotation.y + factorY, startRotation.z);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(newRotation), 0.16f);
    }

    private void CheckBobFactor()
    {
        if (playerControl.Moving)
        {
            if (bobFactor < 1)
            {
                bobFactor += Time.deltaTime;
            }
        }
        else
        {
            if (bobFactor > .5f)
            {
                bobFactor -= Time.deltaTime;
            }
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

        transform.localPosition = startPosition + offset; ;
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

