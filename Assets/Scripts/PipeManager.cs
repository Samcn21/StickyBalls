using UnityEngine;
using System.Collections;
[RequireComponent(typeof(MapManager))]
public class PipeManager : MonoBehaviour {
    [SerializeField]
    private GameObject cornerPipePrefab;

    [SerializeField]
    private GameObject straightPipePrefab;

    [SerializeField]
    private GameObject crossPipePrefab;

    [SerializeField]
    private GameObject tPipePrefab;

    private GameObject _boardReference;

    private MapManager _mapManager; 

    void Awake()
    {
        _boardReference = GameObject.FindGameObjectWithTag("Board");
        _mapManager = GetComponent<MapManager>();
    }

    public void placePipeOfTypeAt(PipeType t, int x, int y)
    {
        Vector3 position;
        GameObject g=new GameObject();
        position = _mapManager.getTileByCoord(x, y).transform.position;
        position.y += 1f;
        switch (t)
        {
            case PipeType.Corner:
                g = (GameObject)Instantiate(cornerPipePrefab, position, Quaternion.identity);
                break;
            case PipeType.Straight:
                g = (GameObject)Instantiate(straightPipePrefab, position, Quaternion.identity);
                break;
            case PipeType.Cross:
                g = (GameObject)Instantiate(crossPipePrefab, position, Quaternion.identity);
                break;
            case PipeType.T:
                g = (GameObject)Instantiate(tPipePrefab, position, Quaternion.identity);
                break;
        }
        g.name = "pipe" + (x + 1) + "_" + (y + 1);
        g.transform.parent = _boardReference.transform;
    }
}

public enum PipeType{
    Corner,Straight,Cross,T
}
