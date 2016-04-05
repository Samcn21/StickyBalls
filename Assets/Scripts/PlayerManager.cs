using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayerPipeManager))]
public class PlayerManager : MonoBehaviour {
    private PipeManager _pipeManagerRef;
    private PlayerPipeManager _playerPipeManagerRef;
    private GameObject _voidPipeRef;
    private GameObject _crossPipeRef;
    private GameObject _cornerPipeRef;
    private GameObject _straightPipeRef;
    private GameObject _tPipeRef;

    public int index;

    private PipeType carryingPipe;
    
    void Awake()
    {
        carryingPipe = PipeType.Void;
        _pipeManagerRef = GameObject.FindGameObjectWithTag("GameController").GetComponent<PipeManager>();
        _playerPipeManagerRef = GetComponent<PlayerPipeManager>();
        initializePipes();
    }
    private void initializePipes()
    {
        Vector3 offset = new Vector3(0, 20, 0);
        _voidPipeRef=(GameObject)Instantiate(_pipeManagerRef.getVoidPipePrefab(), transform.position + offset, Quaternion.identity);
        _crossPipeRef = (GameObject)Instantiate(_pipeManagerRef.getCrossPipePrefab(), transform.position + offset, Quaternion.identity);
        _cornerPipeRef = (GameObject)Instantiate(_pipeManagerRef.getCornerPipePrefab(), transform.position + offset, Quaternion.identity);
        _straightPipeRef = (GameObject)Instantiate(_pipeManagerRef.getStraightPipePrefab(), transform.position + offset, Quaternion.identity);
        _tPipeRef = (GameObject)Instantiate(_pipeManagerRef.getTPipePrefab(), transform.position + offset, Quaternion.identity);
        _crossPipeRef.SetActive(false);
        _cornerPipeRef.SetActive(false);
        _straightPipeRef.SetActive(false);
        _tPipeRef.SetActive(false);
        _crossPipeRef.transform.parent = transform;
        _cornerPipeRef.transform.parent = transform;
        _straightPipeRef.transform.parent = transform;
        _tPipeRef.transform.parent = transform;
        _voidPipeRef.transform.parent = transform;
    }

    public void carryCrossPipe()
    {
        _voidPipeRef.SetActive(false);
        _crossPipeRef.SetActive(true);
        _cornerPipeRef.SetActive(false);
        _straightPipeRef.SetActive(false);
        _tPipeRef.SetActive(false);
        carryingPipe = PipeType.Cross;
        foreach (Vector2 pos in _playerPipeManagerRef.freePositions)
            _pipeManagerRef.placePipePHOfTypeAt(index, PipeType.Cross, (int)pos.x, (int)pos.y);
    }

    public void carryStraightPipe()
    {
        _voidPipeRef.SetActive(false);
        _crossPipeRef.SetActive(false);
        _cornerPipeRef.SetActive(false);
        _straightPipeRef.SetActive(true);
        _tPipeRef.SetActive(false);
        carryingPipe = PipeType.Straight;
        foreach (Vector2 pos in _playerPipeManagerRef.freePositions)
            _pipeManagerRef.placePipePHOfTypeAt(index, PipeType.Straight, (int)pos.x, (int)pos.y);
    }

    public void carryTPipe()
    {
        _voidPipeRef.SetActive(false);
        _crossPipeRef.SetActive(false);
        _cornerPipeRef.SetActive(false);
        _straightPipeRef.SetActive(false);
        _tPipeRef.SetActive(true);
        carryingPipe = PipeType.T;
        foreach (Vector2 pos in _playerPipeManagerRef.freePositions)
            _pipeManagerRef.placePipePHOfTypeAt(index, PipeType.T, (int)pos.x, (int)pos.y);
    }

    public void carryCornerPipe()
    {
        _voidPipeRef.SetActive(false);
        _crossPipeRef.SetActive(false);
        _cornerPipeRef.SetActive(true);
        _straightPipeRef.SetActive(false);
        _tPipeRef.SetActive(false);
        carryingPipe = PipeType.Corner;
        foreach (Vector2 pos in _playerPipeManagerRef.freePositions)
            _pipeManagerRef.placePipePHOfTypeAt(index, PipeType.Corner, (int)pos.x, (int)pos.y);
    }

    public void emptyPipe()
    {
        _voidPipeRef.SetActive(true);
        _crossPipeRef.SetActive(false);
        _cornerPipeRef.SetActive(false);
        _straightPipeRef.SetActive(false);
        _tPipeRef.SetActive(false);
        carryingPipe = PipeType.Void;
    }
}
