using UnityEngine;
using System.Collections;

public class FadeOutScript : MonoBehaviour {

     void Start () {
        StartCoroutine("FadeOut");
    }

    private IEnumerator FadeOut()
    {
        float alpha = 1;
        Color originalColor = gameObject.GetComponent<MeshRenderer>().material.color;
        while (alpha >= 0)
        {
            alpha -= Time.deltaTime * 3;
            Color color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            gameObject.GetComponent<MeshRenderer>().material.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }
}
