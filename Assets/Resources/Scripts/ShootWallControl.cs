using UnityEngine;
using System.Collections;

public class ShootWallControl : MonoBehaviour {

    private int numShotsNeeded;
    private int numShot;

    void Start()
    {
        numShotsNeeded = gameObject.transform.childCount;
    }

    public void Shoot()
    {
        numShot++;
        if (numShot == numShotsNeeded) gameObject.AddComponent<DissolveScript>();
    }
}
