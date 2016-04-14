﻿using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayerPipeManager))]
public class PlayerManager : MonoBehaviour {
    private PipeManager _pipeManagerRef;
    private PlayerPipeManager _playerPipeManagerRef;
    private GameObject _voidPipeRef;
    private GameObject _crossPipeRef;
    private MapManager _mapManager;
    private GameObject _cornerPipeRef;
    private GameObject _straightPipeRef;
    private GameObject _tPipeRef;

    public int index;
    private Vector2 _closestPos;
     private float sightRadius;
    private PipeData.PipeType carryingPipe;
    
    void Awake()
    {
        carryingPipe = PipeData.PipeType.Void;
        _pipeManagerRef = GameObject.FindGameObjectWithTag("GameController").GetComponent<PipeManager>();
        _mapManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
        sightRadius = _mapManager.sightRadius;
        _playerPipeManagerRef = GetComponent<PlayerPipeManager>();
        _closestPos = new Vector2(-1, -1);
        initializePipes();
    }

    void Update()
    {
        if (_pipeManagerRef.getPHPipeOfPlayer(index).Count > 0)
        {
            if (_closestPos == new Vector2(-1, -1))
            {
                _closestPos = new Vector2();
                foreach (Vector2 pos in _pipeManagerRef.getPHPipeOfPlayer(index).Keys)
                {
                    PipeManager.PipeT g = (PipeManager.PipeT)_pipeManagerRef.getPHPipeOfPlayer(index)[pos];
                    g.objRef.SetActive(false);
                }
            }
            float d = -1;
            float d1;
            bool foundSomethingClose = false;
            foreach (Vector2 pos in _pipeManagerRef.getPHPipeOfPlayer(index).Keys)
                if (d == -1)
                {
                    d1 = Mathf.Abs(Vector3.Distance(_mapManager.getTileByCoord((int)pos.x, (int)pos.y).transform.position, transform.position));
                    if (d1 > sightRadius)
                        continue;
                    d = d1;
                    foundSomethingClose = true;
                    _closestPos = pos;
                    PipeManager.PipeT g = (PipeManager.PipeT)_pipeManagerRef.getPHPipeOfPlayer(index)[pos];
                    g.objRef.SetActive(true);
                }
                else
                if (Mathf.Abs(Vector3.Distance(_mapManager.getTileByCoord((int)pos.x, (int)pos.y).transform.position, transform.position)) < d)
                {
                    d1 = Mathf.Abs(Vector3.Distance(_mapManager.getTileByCoord((int)pos.x, (int)pos.y).transform.position, transform.position));
                    if (d1 > sightRadius)
                        continue;
                    d = d1;
                    PipeManager.PipeT g = (PipeManager.PipeT)_pipeManagerRef.getPHPipeOfPlayer(index)[_closestPos];
                    g.objRef.SetActive(false);
                    _closestPos = pos;
                    foundSomethingClose = true;
                    g = (PipeManager.PipeT)_pipeManagerRef.getPHPipeOfPlayer(index)[pos];
                    g.objRef.SetActive(true);
                }
            if (!foundSomethingClose)
                _closestPos = new Vector2(-1, -1);
        }
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
    //This function is called when the player is carrying a pipe of type cross
    //It is called when the player take the pipe from the factory and it instantiates for each free position a placeholder
    public void carryCrossPipe()
    {
        _voidPipeRef.SetActive(false);
        _crossPipeRef.SetActive(true);
        _cornerPipeRef.SetActive(false);
        _straightPipeRef.SetActive(false);
        _tPipeRef.SetActive(false);
        carryingPipe = PipeData.PipeType.Cross;
        foreach (Vector2 pos in _playerPipeManagerRef.freePositions)
            _pipeManagerRef.placePipePHOfTypeAt(index, PipeData.PipeType.Cross, (int)pos.x, (int)pos.y);
    }
    //This function is called when the player is carrying a pipe of type straight
    //It is called when the player take the pipe from the factory and it instantiates for each free position a placeholder
    public void carryStraightPipe()
    {
        _voidPipeRef.SetActive(false);
        _crossPipeRef.SetActive(false);
        _cornerPipeRef.SetActive(false);
        _straightPipeRef.SetActive(true);
        _tPipeRef.SetActive(false);
        carryingPipe = PipeData.PipeType.Straight;
        foreach (Vector2 pos in _playerPipeManagerRef.freePositions)
            _pipeManagerRef.placePipePHOfTypeAt(index, PipeData.PipeType.Straight, (int)pos.x, (int)pos.y);
    }
    //This function is called when the player is carrying a pipe of type t
    //It is called when the player take the pipe from the factory and it instantiates for each free position a placeholder
    public void carryTPipe()
    {
        _voidPipeRef.SetActive(false);
        _crossPipeRef.SetActive(false);
        _cornerPipeRef.SetActive(false);
        _straightPipeRef.SetActive(false);
        _tPipeRef.SetActive(true);
        carryingPipe = PipeData.PipeType.T;
        foreach (Vector2 pos in _playerPipeManagerRef.freePositions)
            _pipeManagerRef.placePipePHOfTypeAt(index, PipeData.PipeType.T, (int)pos.x, (int)pos.y);
    }
    //This function is called when the player is carrying a pipe of type corner
    //It is called when the player take the pipe from the factory and it instantiates for each free position a placeholder
    public void carryCornerPipe()
    {
        _voidPipeRef.SetActive(false);
        _crossPipeRef.SetActive(false);
        _cornerPipeRef.SetActive(true);
        _straightPipeRef.SetActive(false);
        _tPipeRef.SetActive(false);
        carryingPipe = PipeData.PipeType.Corner;
        foreach (Vector2 pos in _playerPipeManagerRef.freePositions)
            _pipeManagerRef.placePipePHOfTypeAt(index, PipeData.PipeType.Corner, (int)pos.x, (int)pos.y);
    }
    //This function is called when the player decided to waste the pipe that was carrying
    public void emptyPipe()
    {
        _voidPipeRef.SetActive(true);
        _crossPipeRef.SetActive(false);
        _cornerPipeRef.SetActive(false);
        _straightPipeRef.SetActive(false);
        _tPipeRef.SetActive(false);
        carryingPipe = PipeData.PipeType.Void;
        _pipeManagerRef.flushPHPipes(index);
    }

    public bool isCarryingEmptyPipe()
    {
        return carryingPipe == PipeData.PipeType.Void;
    }
}
