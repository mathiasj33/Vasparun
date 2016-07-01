using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScript : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void Increase()
    {
        StopCoroutine("ScaleDown");
        StartCoroutine("ScaleUp");
    }

    public void Decrease()
    {
        StopCoroutine("ScaleUp");
        StartCoroutine("ScaleDown");
    }

    private IEnumerator ScaleUp()
    {
        float scale = 1;
        for(int i = 0; i < 10; i++)
        {
            scale += 0.01f;
            rectTransform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
    }

    private IEnumerator ScaleDown()
    {
        float scale = rectTransform.localScale.x;
        while(scale > 1)
        {
            scale -= 0.01f;
            rectTransform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
        rectTransform.localScale = new Vector3(1, 1, 1);
    }
}