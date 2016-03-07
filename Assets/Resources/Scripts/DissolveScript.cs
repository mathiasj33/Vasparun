using UnityEngine;
using System.Collections;

public class DissolveScript : MonoBehaviour {

    void Start()
    {
        StartCoroutine("Dissolve");
    }

    private IEnumerator Dissolve()
    {
        float alphaCut = 0;
        Material material = gameObject.GetComponent<MeshRenderer>().material;
        while (alphaCut < 1)
        {
            alphaCut += Time.deltaTime * 2;
            material.SetFloat("_AlphaCut", alphaCut);
            yield return null;
        }
        Destroy(gameObject);
    }
}
