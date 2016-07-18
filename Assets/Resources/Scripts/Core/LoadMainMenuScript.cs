using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadMainMenuScript : MonoBehaviour {  //TODO: Username anfangen. sonderzeichen (ä) ausprobieren, Kein ',' und '$', username in settings für genau einen pc
    void Start()
    {
        SceneManager.LoadScene(1);
        //string url = "http://localhost:8080/";


    }

    IEnumerator WaitForRequest(WWW www)  //Online system: Sobald internet connection: Nach username fragen. Bool aktivieren.
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
}
