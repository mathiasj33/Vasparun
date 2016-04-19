using UnityEngine;
using System.Collections;

public class PlayFirstTime : MonoBehaviour {

	void Awake () {
        if (Time.time > 5) return;
        GetComponent<AudioSource>().Play();
	}
}
