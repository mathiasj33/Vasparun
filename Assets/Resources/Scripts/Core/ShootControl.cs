using UnityEngine;
using System.Collections;

public class ShootControl : MonoBehaviour
{
    public Camera cam;

    private Invoker invoker;
    private bool shootingAllowed = true;

    private ParticleSystem gunParticles;
    private Transform tipPosition;
    private Animator animator;

    void Start()
    {
        invoker = GameObject.Find("Player").GetComponent<Invoker>();

        gunParticles = GameObject.Find("LaserParticle").GetComponent<ParticleSystem>();
        tipPosition = GameObject.Find("Tip").transform;
        animator = GameObject.Find("Gun").GetComponent<Animator>();
    }

    void Update()
    {
        CheckShooting();
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
                    go.transform.parent.gameObject.GetComponent<ShootWallControl>().ShootAt();
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
        laser.SetPosition(0, tipPosition.position);
        return laser;
    }
}

