using UnityEngine;
using System.Collections;

public class InfiniteMapScript : MonoBehaviour
{

    void Start()
    {
        //CreateMap();
        Test();

    }

    //private void CreateMap()
    //{
    //    GameObject 
    //    for(int i = 0; i < 10; i++)
    //    {
    //        int piece = Random.Range(1, 5);
    //        GameObject go = (GameObject) Instantiate(Resources.Load("Models/Infinite/piece" + piece));
    //    }
    //}

    private void Test()
    {
        GameObject two = (GameObject)Instantiate(Resources.Load("Models/Infinite/piece" + 1));
        GameObject one = (GameObject)Instantiate(Resources.Load("Models/Infinite/piece" + 4));

        Transform oneWorld = one.transform.Find("World");
        GameObject oneLast = oneWorld.GetChild(oneWorld.childCount - 1).gameObject;

        Debug.Log(oneLast.transform.position);

        two.transform.position += oneLast.transform.position;
        Destroy(oneLast);

        Initializer.Init(one);
        Initializer.Init(two);
    }
}
