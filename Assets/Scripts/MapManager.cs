using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapManager : MonoBehaviour {
    public int nPlayers;

    [SerializeField]
    private bool spawnHammers;

    public float sightRadius;

    [SerializeField]
    private GameObject hammerPrefab;

    [SerializeField]
    private float playersSpeed;

    [SerializeField]
    private GameObject pipeSourcePrefab;

	[SerializeField]
	private GameObject centralMachinePrefab;

    [SerializeField]
    private GameObject pipeFactoryPrefab;

    [SerializeField]
    private List<Vector2> listOfFactoryPositions;

    [SerializeField]
    private List<Vector3> listOfFactoryRotations;

    [SerializeField]
    private GameObject tile;

    private GameObject _boardReference;


    [SerializeField]
    private int rows;

    [SerializeField]
    private int columns;
    // Use this for initialization

    public int r
    {
        get
        {
            return rows;
        }
    }
    public int c
    {
        get
        {
            return columns;
        }
    }

    private Hashtable positionsMapping;

    [SerializeField]
    private Camera boardCamera;

    [SerializeField]
    private GameObject playerPrefab;

    private Hashtable _playersRef;

    private Hashtable _sourcesRef;

    private InputsManager _inputsManagerRef;
    void Awake () {
        
        Vector3 position;
        _boardReference = GameObject.FindGameObjectWithTag("Board");
        _inputsManagerRef=GetComponent<InputsManager>();
        Vector3 boardPosition = _boardReference.transform.position;
        positionsMapping = new Hashtable();
        float tileScaleX = tile.GetComponent<MeshRenderer>().bounds.size.x;
        float tileScaleZ = tile.GetComponent<MeshRenderer>().bounds.size.z;
        for (int i = 0; i < columns; i++) {
            position = boardPosition;
            position.x = tileScaleX * i;
            for (int k = 0; k < rows; k++)
            {
                position.z = tileScaleZ * k;
                GameObject g = (GameObject)GameObject.Instantiate(tile,position,Quaternion.identity);
                g.transform.parent = _boardReference.transform;
                g.name = "tile" + (i+1) +"_"+ (k+1);
                positionsMapping.Add(new Vector2(i, k), g);
            }
        }
        var childCounts = _boardReference.transform.childCount;
        List<Transform> childs = new List<Transform>();
        var pos = Vector3.zero;
        while(_boardReference.transform.childCount>0)
        {
            Transform C = _boardReference.transform.GetChild(0);
            pos += C.position;
            childs.Add(C);
            C.parent = null;
        }
        pos /= childCounts;
        _boardReference.transform.position = pos;
        foreach(Transform child in childs)
            child.parent = _boardReference.transform;
        pos.y += 100;
        boardCamera.transform.position = pos;
        if (rows*2 < columns)
            boardCamera.orthographicSize = 5 * columns / 10;
        else
            boardCamera.orthographicSize = 5 * rows / 10;
        if (spawnHammers)
            instantiateNHammers();
        instantiateNSources();
		instantiateCentralMachine();
        instantiateNPlayers();
        instantiateFactories();
       
    }
	
	public GameObject getTileByCoord(int y,int x)
    {
        if (positionsMapping.ContainsKey(new Vector2(x, y)))
            return (GameObject)positionsMapping[new Vector2(x, y)];
        return null;
    }

    private void instantiateNHammers()
    {
        Vector3 thickness = new Vector3(0, hammerPrefab.GetComponentInChildren<MeshRenderer>().bounds.size.y/2 + tile.GetComponent<MeshRenderer>().bounds.size.y / 2, 0);
        switch (nPlayers)
        {
            case 1:
                Instantiate(hammerPrefab, getTileByCoord(5, 5).transform.position + thickness, Quaternion.identity);
                break;
            case 2:
                Instantiate(hammerPrefab, getTileByCoord(5, 5).transform.position + thickness, Quaternion.identity);
                Instantiate(hammerPrefab, getTileByCoord(rows-5,columns- 5).transform.position + thickness, Quaternion.identity);
                break;
            case 4:
                Instantiate(hammerPrefab, getTileByCoord(5, 5).transform.position + thickness, Quaternion.identity);
                Instantiate(hammerPrefab, getTileByCoord(rows - 5, columns - 5).transform.position + thickness, Quaternion.identity);
                Instantiate(hammerPrefab, getTileByCoord(5,columns- 5).transform.position + thickness, Quaternion.identity);
                Instantiate(hammerPrefab, getTileByCoord(rows - 5, 5).transform.position + thickness, Quaternion.identity);
                break;
        }
    }

    private void instantiateNPlayers()
    {
        _playersRef = new Hashtable();
        Vector3 thickness = new Vector3(0, pipeSourcePrefab.GetComponentInChildren<MeshRenderer>().bounds.size.y + tile.GetComponent<MeshRenderer>().bounds.size.y / 2, 0);
        GameObject g;
        switch (nPlayers)
        {
            case 1:
                 g= (GameObject)Instantiate(playerPrefab, getTileByCoord(0, 0).transform.position+ thickness, Quaternion.identity);
                g.GetComponent<PlayerManager>().index = 0;
                g.GetComponent<PlayerPipeManager>().setSource((Vector2) _sourcesRef[0]);
                Debug.Log((Vector2)_sourcesRef[0]);
                g =assignKeysToP1(g);
                _playersRef.Add(0,g);
                break;
            case 2:
                g = (GameObject)Instantiate(playerPrefab, getTileByCoord(0, 0).transform.position+ thickness, Quaternion.identity);
                g.GetComponent<PlayerManager>().index = 0;
                g.GetComponent<PlayerPipeManager>().setSource((Vector2)_sourcesRef[0]);

                g = assignKeysToP1(g);
                _playersRef.Add(0,g);
                g = (GameObject)Instantiate(playerPrefab, getTileByCoord(rows-1, columns-1).transform.position+ thickness, Quaternion.identity);
                g.GetComponent<PlayerManager>().index = 1;
                g.GetComponent<PlayerPipeManager>().setSource((Vector2)_sourcesRef[1]);
                g = assignKeysToP2(g);
                _playersRef.Add(1,g);
                break;
            case 4:
                g = (GameObject)Instantiate(playerPrefab, getTileByCoord(0, 0).transform.position+ thickness, Quaternion.identity);
                g.GetComponent<PlayerManager>().index = 0;
                g.GetComponent<PlayerPipeManager>().setSource((Vector2)_sourcesRef[0]);
                g = assignKeysToP1(g);
                _playersRef.Add(0, g);
                g = (GameObject)Instantiate(playerPrefab, getTileByCoord(rows - 1, columns - 1).transform.position+ thickness, Quaternion.identity);
                g.GetComponent<PlayerManager>().index = 3;
                g.GetComponent<PlayerPipeManager>().setSource((Vector2)_sourcesRef[3]);
                g = assignKeysToP4(g);
                _playersRef.Add(3, g);
                g = (GameObject)Instantiate(playerPrefab, getTileByCoord(rows - 1, 0).transform.position+ thickness, Quaternion.identity);
                g.GetComponent<PlayerManager>().index = 2;
                g.GetComponent<PlayerPipeManager>().setSource((Vector2)_sourcesRef[2]);
                g = assignKeysToP3(g);
                _playersRef.Add(2,g);
                g = (GameObject)Instantiate(playerPrefab, getTileByCoord(0, columns - 1).transform.position+ thickness, Quaternion.identity);
                g.GetComponent<PlayerManager>().index = 1;
                g.GetComponent<PlayerPipeManager>().setSource((Vector2)_sourcesRef[1]);
                g = assignKeysToP2(g);
                _playersRef.Add(1,g);
                break;
        }
    }
    private void instantiateFactories()
    {
        float thickness = pipeFactoryPrefab.GetComponentInChildren<MeshRenderer>().bounds.size.y/2 + tile.GetComponent<MeshRenderer>().bounds.size.y / 2;
        int i = 0;
        foreach (Vector2 pos in listOfFactoryPositions)
        {
            Vector3 p = getTileByCoord((int)pos.x, (int)pos.y).transform.position;
            p.y += thickness;
            Instantiate(pipeFactoryPrefab,p , Quaternion.Euler(listOfFactoryRotations[i]));
            i++;
        }
    }
	private void instantiateCentralMachine()
	{
		Vector3 thickness = new Vector3(0, centralMachinePrefab.GetComponentInChildren<MeshRenderer>().bounds.size.y + tile.GetComponent<MeshRenderer>().bounds.size.y / 2, 0);

		Instantiate(centralMachinePrefab, getTileByCoord(rows / 2 , columns / 2 ).transform.position + thickness, Quaternion.identity);
	}
    private void instantiateNSources()
    {
        Vector3 thickness =new Vector3(0, pipeSourcePrefab.GetComponentInChildren<MeshRenderer>().bounds.size.y/2+tile.GetComponent<MeshRenderer>().bounds.size.y/2,0);
        _sourcesRef = new Hashtable();
        switch (nPlayers)
        {
            case 1:
                Instantiate(pipeSourcePrefab, getTileByCoord(1, 1).transform.position+ thickness, Quaternion.Euler(0, 90, 0));
                _sourcesRef.Add(0, new Vector2(1, 1));
                break;
            case 2:
                Instantiate(pipeSourcePrefab, getTileByCoord(1, 1).transform.position+ thickness, Quaternion.Euler(0, 90, 0));
                _sourcesRef.Add(0, new Vector2(1, 1));
                Instantiate(pipeSourcePrefab, getTileByCoord(rows-2, columns-2).transform.position+ thickness, Quaternion.Euler(0, 270, 0));
                _sourcesRef.Add(0, new Vector2(rows-2, columns-2));
                break;
            case 4:
                Instantiate(pipeSourcePrefab, getTileByCoord(1, 1).transform.position+ thickness, Quaternion.Euler(0, 90, 0));
                _sourcesRef.Add(0, new Vector2(1, 1));
                Instantiate(pipeSourcePrefab, getTileByCoord(1, columns - 2).transform.position+ thickness, Quaternion.Euler(0, 180, 0));
                _sourcesRef.Add(0, new Vector2(1, columns-2));
                Instantiate(pipeSourcePrefab, getTileByCoord(rows - 2, 1).transform.position+ thickness, Quaternion.Euler(0, 0, 0));
                _sourcesRef.Add(0, new Vector2(rows - 2, 1));
                Instantiate(pipeSourcePrefab, getTileByCoord(rows - 2, columns - 2).transform.position+ thickness, Quaternion.Euler(0, 270, 0));
                _sourcesRef.Add(0, new Vector2(rows - 2, columns - 2));
                break;
        }
    }
    private GameObject assignKeysToP1(GameObject g)
    {
        PlayerInputManager p = g.GetComponent<PlayerInputManager>();
       p.speed = playersSpeed;
        p.releasePipeKey = _inputsManagerRef.playerInputs[0].releasePipe;
        p.horizontalAxis = _inputsManagerRef.playerInputs[0].hAxis;
        p.verticalAxis = _inputsManagerRef.playerInputs[0].vAxis;
        p.hitHammer = _inputsManagerRef.playerInputs[0].hitHammer;
        return g;
    }
    private GameObject assignKeysToP2(GameObject g)
    {
        PlayerInputManager p = g.GetComponent<PlayerInputManager>();
        p.speed = playersSpeed;
        p.releasePipeKey = _inputsManagerRef.playerInputs[1].releasePipe;
        p.horizontalAxis = _inputsManagerRef.playerInputs[1].hAxis;
        p.verticalAxis = _inputsManagerRef.playerInputs[1].vAxis;
        p.hitHammer = _inputsManagerRef.playerInputs[1].hitHammer;
        return g;
    }
    private GameObject assignKeysToP3(GameObject g)
    {
        PlayerInputManager p = g.GetComponent<PlayerInputManager>();
        p.speed = playersSpeed;
        p.releasePipeKey = _inputsManagerRef.playerInputs[2].releasePipe;
        p.horizontalAxis = _inputsManagerRef.playerInputs[2].hAxis;
        p.verticalAxis = _inputsManagerRef.playerInputs[2].vAxis;
        p.hitHammer = _inputsManagerRef.playerInputs[2].hitHammer;
        return g;
    }
    private GameObject assignKeysToP4(GameObject g)
    {
        PlayerInputManager p = g.GetComponent<PlayerInputManager>();
        p.speed = playersSpeed;
        p.releasePipeKey = _inputsManagerRef.playerInputs[3].releasePipe;
        p.horizontalAxis = _inputsManagerRef.playerInputs[3].hAxis;
        p.verticalAxis = _inputsManagerRef.playerInputs[3].vAxis;
        p.hitHammer = _inputsManagerRef.playerInputs[3].hitHammer;
        return g;
    }
}
