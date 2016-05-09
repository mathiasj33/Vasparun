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

    public void ShootAt()
    {
        shootable.ShootAt();
        control.AddToRevertSet(this);
        if (shootable.Shot)
        {
            ChangeEmissionColorScript redToGreen = gameObject.AddComponent<ChangeEmissionColorScript>();
            redToGreen.Begin = Color.red;
            redToGreen.End = Color.green;
            gameObject.GetComponent<MeshCollider>().enabled = false;
            Globals.AudioSources.wallDisappear.Play();
        }
    }

    public void Revert()
    {
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(1, 0, 0));
        shootable.Revert();
        
        foreach(Transform t in transform)
        {
            SetAlphaToOne(t);
            t.gameObject.GetComponent<MeshCollider>().enabled = true;
        }

        gameObject.GetComponent<MeshCollider>().enabled = true;
    }

    private void SetAlphaToOne(Transform t)
    {
        Material material = t.gameObject.GetComponent<MeshRenderer>().material;
        Color originalColor = material.color;
        material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }
}
