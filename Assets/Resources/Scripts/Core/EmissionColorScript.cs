using UnityEngine;
using System.Collections;

public class EmissionStrengthScript : MonoBehaviour {

    private Material Mat { get { return gameObject.GetComponent<MeshRenderer>().material; } }
    private Color startColor;
    private Color endColor;

    private float scale;
    private bool increase;
    private bool decrease;
	
	void Start()
    {
        startColor = Mat.GetColor("_EmissionColor");
        endColor = startColor * 8;
    }

    void Update()
    {
        if(increase)
        {
            scale += Time.deltaTime * 1.5f;
            Mat.SetColor("_EmissionColor", Color.Lerp(startColor, endColor, scale));
            if(scale > 1)
            {
                scale = 1;
                increase = false;
            }
        }
        else if(decrease)
        {
            scale -= Time.deltaTime * 1.5f;
            Mat.SetColor("_EmissionColor", Color.Lerp(startColor, endColor, scale));
            if (scale < 0)
            {
                scale = 0;
                decrease = false;
            }
        }
    }

    public void IncreaseEmissionColor()
    {
        increase = true;
        decrease = false;
    }

    public void DecreaseEmissionColor()
    {
        decrease = true;
        increase = false;
    }
}
