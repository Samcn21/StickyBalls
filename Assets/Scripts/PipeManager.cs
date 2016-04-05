using UnityEngine;
using System.Collections;
[RequireComponent(typeof(MapManager))]
public class PipeManager : MonoBehaviour {
    [SerializeField]
    private GameObject voidPipePrefab;

    [SerializeField]
    private GameObject cornerPipePrefab;

    [SerializeField]
    private GameObject cornerPipePHPrefab;

    [SerializeField]
    private GameObject crossPipePrefab;

    [SerializeField]
    private GameObject crossPipePHPrefab;

    [SerializeField]
    private GameObject straightPipePrefab;

    [SerializeField]
    private GameObject straightPipePHPrefab;

    [SerializeField]
    private GameObject tPipePrefab;

    [SerializeField]
    private GameObject tPipePHPrefab;

    private GameObject _boardReference;

    private MapManager _mapManager;

    private Hashtable _builtPipe;

    private Hashtable _placeHPipe;
    void Awake()
    {
        _boardReference = GameObject.FindGameObjectWithTag("Board");
        _mapManager = GetComponent<MapManager>();
        _builtPipe = new Hashtable();
        _placeHPipe = new Hashtable();
    }
    //This function place a tiles of type at the position x,y with a rotation of rotation
    public void placePipeOfTypeAt(int playerIndex,PipeType t, int x, int y,Quaternion rotation)
    {
        if (!_builtPipe.ContainsKey(new Vector2(x, y)))
        {
            Vector3 position;
            GameObject g = new GameObject();
            position = _mapManager.getTileByCoord(x, y).transform.position;
            position.y += 1f;
            switch (t)
            {
                case PipeType.Corner:
                    g = (GameObject)Instantiate(cornerPipePrefab, position, rotation);
                    break;
                case PipeType.Straight:
                    g = (GameObject)Instantiate(straightPipePrefab, position, rotation);
                    break;
                case PipeType.Cross:
                    g = (GameObject)Instantiate(crossPipePrefab, position, rotation);
                    break;
                case PipeType.T:
                    g = (GameObject)Instantiate(tPipePrefab, position, rotation);
                    break;
            }
            g.name = "pipe" + (x + 1) + "_" + (y + 1);
            g.transform.parent = _boardReference.transform;
            PipeT pipe = new PipeT();
            pipe.pipeType = t;
            pipe.objRef = g;
            pipe.playerIndex = playerIndex;

            _builtPipe.Add(new Vector2(x, y), pipe);
        }
    }
    //This function place a placeholder of type at the position x,y with a rotation of rotation
    public void placePipePHOfTypeAt(int playerIndex, PipeType t, int x, int y)
    {
        if (_placeHPipe.ContainsKey(new Vector2(x, y))){
            foreach (Vector2 key in _placeHPipe.Keys)
                GameObject.Destroy(((PipeT)_placeHPipe[key]).objRef);
            _placeHPipe = new Hashtable();
        }
            Vector3 position;
            GameObject g = new GameObject();
            position = _mapManager.getTileByCoord(x, y).transform.position;
            position.y += 3f;
            switch (t)
            {
                case PipeType.Corner:
                    g = (GameObject)Instantiate(cornerPipePHPrefab, position, Quaternion.identity);
                    break;
                case PipeType.Straight:
                    g = (GameObject)Instantiate(straightPipePHPrefab, position, Quaternion.identity);
                    break;
                case PipeType.Cross:
                    g = (GameObject)Instantiate(crossPipePHPrefab, position, Quaternion.identity);
                    break;
                case PipeType.T:
                    g = (GameObject)Instantiate(tPipePHPrefab, position, Quaternion.identity);
                    break;
            }
            g.name = "pipePH" + (x + 1) + "_" + (y + 1);
            g.GetComponent<tileManager>().xPos = x;
            g.GetComponent<tileManager>().yPos = y;
            g.GetComponent<tileManager>().updatePipeHolePositions();
            Debug.Log(g.name);
            Debug.Log("X:" + x + " Y:"+y);
            g.transform.parent = _boardReference.transform;
            PipeT pipe = new PipeT();
            pipe.pipeType = t;
            pipe.objRef = g;
            pipe.playerIndex = playerIndex;

        _placeHPipe.Add(new Vector2(x, y), pipe);
        
    }
    //Inner class used to store data relative to each placed pipe
    private class PipeT {
        public PipeType pipeType;
        public GameObject objRef;
        public int playerIndex;

        private ArrayList possiblePositions;

        public void calculatePossiblePositions(int x, int y)
        {
            switch (pipeType)
            {
                case PipeType.Corner:
                    break;
                case PipeType.Cross:
                    break;
                case PipeType.Straight:
                    break;
                case PipeType.T:
                    break;
                
            }
        }
    }

    public GameObject getCrossPipePrefab()
    {
        return crossPipePrefab;
    }

    public GameObject getVoidPipePrefab()
    {
        return voidPipePrefab;
    }

    public GameObject getTPipePrefab()
    {
        return tPipePrefab;
    }

    public GameObject getCornerPipePrefab()
    {
        return cornerPipePrefab;
    }

    public GameObject getStraightPipePrefab()
    {
        return straightPipePrefab;
    }
    public GameObject getCrossPipePHPrefab()
    {
        return crossPipePHPrefab;
    }

    public GameObject getTPipePHPrefab()
    {
        return tPipePHPrefab;
    }

    public GameObject getCornerPipePHPrefab()
    {
        return cornerPipePHPrefab;
    }

    public GameObject getStraightPipePHPrefab()
    {
        return straightPipePHPrefab;
    }
}

public enum PipeType{
    Void,Corner,Straight,Cross,T
}
