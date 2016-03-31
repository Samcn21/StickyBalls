using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapManager : MonoBehaviour {
    public int nPlayers;

    [SerializeField]
    private GameObject tile;

    private GameObject _boardReference;

    [SerializeField]
    private int columns;

    [SerializeField]
    private int rows;
    // Use this for initialization

    private Hashtable positionsMapping;

    [SerializeField]
    private Camera boardCamera;

    [SerializeField]
    private GameObject playerPrefab;
    void Awake () {
        Vector3 position;
        _boardReference = GameObject.FindGameObjectWithTag("Board");
        Vector3 boardPosition = _boardReference.transform.position;
        positionsMapping = new Hashtable();
        float tileScaleX = tile.GetComponent<MeshRenderer>().bounds.size.x;
        float tileScaleZ = tile.GetComponent<MeshRenderer>().bounds.size.z;
        for (int i = 0; i < rows; i++) {
            position = boardPosition;
            position.x = tileScaleX * i;
            for (int k = 0; k < columns; k++)
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

        instantiateNPlayers(nPlayers);
    }
	
	public GameObject getTileByCoord(int x,int y)
    {
        if (positionsMapping.ContainsKey(new Vector2(x, y)))
            return (GameObject)positionsMapping[new Vector2(x, y)];
        return null;
    }

    private void instantiateNPlayers(int n)
    {
        switch (n)
        {
            case 1:
                Instantiate(playerPrefab, getTileByCoord(0, 0).transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(playerPrefab, getTileByCoord(0, 0).transform.position, Quaternion.identity);
                Instantiate(playerPrefab, getTileByCoord(rows-1, columns-1).transform.position, Quaternion.identity);
                break;
            case 4:
                Instantiate(playerPrefab, getTileByCoord(0, 0).transform.position, Quaternion.identity);
                Instantiate(playerPrefab, getTileByCoord(rows - 1, columns - 1).transform.position, Quaternion.identity);
                Instantiate(playerPrefab, getTileByCoord(rows - 1, 0).transform.position, Quaternion.identity);
                Instantiate(playerPrefab, getTileByCoord(0, columns - 1).transform.position, Quaternion.identity);
                break;
        }
    }
}
