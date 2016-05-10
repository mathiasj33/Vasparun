using UnityEngine;

public class InitializeScript : MonoBehaviour
{
    void Start()
    {
        GameObject[] scene = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject go in scene)
        {
            if(go.name.StartsWith("level"))
            {
                Initializer.Init(go.gameObject);
                break;
            }
        }
    }
}
