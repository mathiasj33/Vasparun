using UnityEngine;
using System.Collections;

public class MoveDownScript : MonoBehaviour {

    private Vector3 deltaPerFrame = new Vector3(0, -1f, 0);
    private int distance = 30;

    void Start () {
        StartCoroutine("MoveDown");
	}
	
	private IEnumerator MoveDown()
    {
        Vector3 pos = transform.position;
        for(int i = 0; i < distance; i++)
        {
            transform.position += deltaPerFrame;
            yield return null;
        }
        Destroy(this);
    }
}
