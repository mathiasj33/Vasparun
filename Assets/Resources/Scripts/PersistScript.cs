using UnityEngine;
using System.Collections;

public class PersistScript : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
