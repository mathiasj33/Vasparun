using UnityEngine;
using System.Collections.Generic;

public class InfiniteMapScript : MonoBehaviour
{
    private RayCastHelper rayCastHelper;
    private NewTileCheckerScript newTileChecker;
    private PlayerControl playerControl;

    private Queue<int> lastUsedPieces = new Queue<int>();
    private GameObject last = null;
    private bool attach = false;

    void Start()
    {
        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        rayCastHelper = GameObject.Find("Player").GetComponent<RayCastHelper>();
        newTileChecker = GameObject.Find("Main").GetComponent<NewTileCheckerScript>();
        CreatePieces(5);
    }

    public void NewMap()
    {
        GenerateMap();
        Globals.DistanceOrigin = new Vector3(0, 0, 0);
        GameObject.Find("Main").GetComponent<ScoreScript>().Reset();
    }

    private void GenerateMap()
    {
        GetAllPieces().ForEach(go => Destroy(go));
        last = null;
        CreatePieces(5);
    }

    void Update()
    {
        CheckMoveToOrigin();
        CheckCreateNewPiece();
    }

    private void CheckMoveToOrigin()
    {
        if (playerControl.gameObject.transform.position.magnitude >= 300 && playerControl.IsGrounded)
        {
            Vector3 dir = playerControl.gameObject.transform.position - new Vector3(0, 0, 0);
            playerControl.gameObject.transform.position = new Vector3(0, 0, 0);
            GetAllPieces().ForEach(go => go.transform.position -= dir);

            Globals.DistanceOrigin -= dir;
        }
    }

    private void CheckCreateNewPiece()
    {
        if(newTileChecker.NewTile)
        {
            CreatePieces(1);
        }
    }

    private void CreatePieces(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int piece = Random.Range(1, 31);
            while (lastUsedPieces.Contains(piece)) piece = Random.Range(1, 31);

            lastUsedPieces.Enqueue(piece);
            if (lastUsedPieces.Count > 5) lastUsedPieces.Dequeue();

            GameObject go = (GameObject)Instantiate(Resources.Load("Models/Infinite/piece" + piece));
            if (last != null)
            {
                go.transform.position += last.transform.position;
                Destroy(last);
            }
            Initializer.Init(go, true);
            Initializer.GetCheckpointGameObjects(go.transform.Find("World").gameObject).ForEach(cgo =>
            {
                cgo.tag = "NoWallrun";
            });

            Transform world = go.transform.Find("World");
            last = world.GetChild(world.childCount - 1).gameObject;

            go.AddComponent<DestroyScript>();
        }
    }

    private List<GameObject> GetAllPieces()
    {
        List<GameObject> pieces = new List<GameObject>();
        GameObject[] scene = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject go in scene)
        {
            if (go.name.StartsWith("piece")) pieces.Add(go);
        }
        return pieces;
    }
}
