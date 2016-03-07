using UnityEngine;
using System.Collections;

public class IncreaseTransparentQueueOrder : MonoBehaviour {
	void Start () {
        if (gameObject.GetComponent<ParticleSystemRenderer>() != null)
            gameObject.GetComponent<ParticleSystemRenderer>().material.renderQueue += 1;
	}
}
