using UnityEngine;
using System.Collections;

public class ShootWallControl : MonoBehaviour
{
    private Shootable shootable;

    public bool Shot { get { return shootable.Shot; } }

    private CheckpointControl control;

    void Start()
    {
        shootable = new Shootable(gameObject.transform.childCount);
        control = GameObject.Find("Player").GetComponent<CheckpointControl>();
    }

    public void Shoot()
    {
        shootable.Shoot();
        control.AddToRevertSet(this);
        if (shootable.Shot)
        {
            gameObject.AddComponent<DissolveScript>();
            gameObject.GetComponent<MeshCollider>().enabled = false;
        }
    }

    public void Revert()
    {
        shootable.Revert();
        gameObject.GetComponent<MeshRenderer>().material.SetFloat("_AlphaCut", 0);
        
        foreach(Transform t in transform)
        {
            Material material = t.gameObject.GetComponent<MeshRenderer>().material;
            Color originalColor = material.color;
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
            t.gameObject.GetComponent<MeshCollider>().enabled = true;
        }

        gameObject.GetComponent<MeshCollider>().enabled = true;
    }
}
