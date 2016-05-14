using UnityEngine;

public class InitializeScript : MonoBehaviour
{
    void Start()
    {
        GameObject[] scene = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject go in scene)
        {
            if(go.name.StartsWith("level") || go.name.StartsWith("piece"))
            {
                Initializer.Init(go.gameObject);
                break;
            }
        }
    }
}
