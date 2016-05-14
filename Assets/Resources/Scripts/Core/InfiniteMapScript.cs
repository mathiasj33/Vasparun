using UnityEngine;
using System.Collections.Generic;

public class InfiniteMapScript : MonoBehaviour
{

    private Queue<int> lastUsedPieces = new Queue<int>();

    void Start()
    {
        CreateMap();
    }

    private void CreateMap()
    {
        GameObject last = null;
        for (int i = 0; i < 10; i++)
        {
            int piece = Random.Range(1, 14);
            while (lastUsedPieces.Contains(piece)) piece = Random.Range(1, 14);
            GameObject go = (GameObject)Instantiate(Resources.Load("Models/Infinite/piece" + piece));
            if (last != null)
            {
                go.transform.position += last.transform.position;
                Destroy(last);
            }
            Initializer.Init(go);
            Transform world = go.transform.Find("World");
            last = world.GetChild(world.childCount - 1).gameObject;
            lastUsedPieces.Enqueue(piece);
            if (lastUsedPieces.Count > 5) lastUsedPieces.Dequeue();
        }
    }

    private void Test()
    {
        GameObject one = (GameObject)Instantiate(Resources.Load("Models/Infinite/piece" + 5));
        GameObject two = (GameObject)Instantiate(Resources.Load("Models/Infinite/piece" + 4));

        Transform oneWorld = one.transform.Find("World");
        GameObject oneLast = oneWorld.GetChild(oneWorld.childCount - 1).gameObject;

        Debug.Log(oneLast.transform.position);

        two.transform.position += oneLast.transform.position;
        Destroy(oneLast);

        Initializer.Init(one);
        Initializer.Init(two);
    }
}
