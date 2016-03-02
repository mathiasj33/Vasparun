using UnityEngine;
using System.Collections;

public class GunControl : MonoBehaviour
{
    public Camera cam;

    private Invoker invoker;
    private bool shootingAllowed = true;

    private ParticleSystem particles;
    private Transform start;
    private AudioSource laserAudio;

    private Animator animator;

    void Start()
    {
        invoker = GetComponent<Invoker>();
        particles = GameObject.Find("LaserParticle").GetComponent<ParticleSystem>();
        start = GameObject.Find("Tip").transform;
        laserAudio = GetComponents<AudioSource>()[5];
        animator = GameObject.Find("Gun").GetComponent<Animator>();
    }

    void Update()
    {
        if (shootingAllowed && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = new Ray(start.position, cam.transform.forward);
            RaycastHit hit;

            LineRenderer laser = CreateLaser();
            laser.SetPosition(0, start.position);

            if (Physics.Raycast(ray, out hit))
            {
                laser.SetPosition(1, hit.point);
            }
            else
            {
                laser.SetPosition(1, ray.GetPoint(50));
            }
            particles.Play();
            laserAudio.Play();
            animator.SetTrigger("Shoot");
            StartCoroutine(FadeLaserOut(laser));

            shootingAllowed = false;
            invoker.Invoke(.4f, () => shootingAllowed = true);
        }
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

        laser.sharedMaterial = (Material)Resources.Load("Materials/laser");
        laser.SetColors(new Color(255, 0, 0, 1), new Color(255, 30, 0, 1));
        laser.SetWidth(0.2f, 0.01f);
        return laser;
    }
}
