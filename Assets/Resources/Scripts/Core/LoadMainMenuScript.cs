using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadMainMenuScript : MonoBehaviour {
    void Start()
    {
        SceneManager.LoadScene(1);
    }
}
