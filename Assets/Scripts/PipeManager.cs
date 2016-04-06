﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private List<Hashtable> _builtPipe;

    private List<Hashtable> _placeHPipe;
    void Awake()
    {
        _boardReference = GameObject.FindGameObjectWithTag("Board");
        _mapManager = GetComponent<MapManager>();
        _builtPipe = new List<Hashtable>();
        _placeHPipe = new List<Hashtable>();
        for(int i=0;i<_mapManager.nPlayers;i++)
        {
            _builtPipe.Add(new Hashtable());
            _placeHPipe.Add(new Hashtable());
        }
        
    }

    public void placePipeOfTypeAt(int playerIndex,PipeType t, int x, int y,Quaternion rotation)
    {

        if (!_builtPipe[playerIndex].ContainsKey(new Vector2(x, y)))
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

            _builtPipe[playerIndex].Add(new Vector2(x, y), pipe);
        }
    }

    public void placePipePHOfTypeAt(int playerIndex, PipeType t, int x, int y)
    {
        if (_placeHPipe[playerIndex].ContainsKey(new Vector2(x, y))){
            foreach (Vector2 key in _placeHPipe[playerIndex].Keys)
                GameObject.Destroy(((PipeT)_placeHPipe[playerIndex][key]).objRef);
            _placeHPipe[playerIndex] = new Hashtable();
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

        _placeHPipe[playerIndex].Add(new Vector2(x, y), pipe);
        
    }
    //Inner class used to store data relative to each placed pipe
    public class PipeT {
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

    public void flushPHPipes(int playerIndex)
    {
        _placeHPipe[playerIndex] = new Hashtable();
    }

    public Hashtable getPipeOfPlayer(int index)
    {
        return _builtPipe[index];
    }

    public Hashtable getPHPipeOfPlayer(int index)
    {
        return _placeHPipe[index];
    }
    public void removePipeFromPlayer(int index,Vector2 key)
    {
        _builtPipe[index].Remove(key);
    }
    public void removePipePHFromPlayer(int index, Vector2 key)
    {
        _placeHPipe[index].Remove(key);
    }
}

public enum PipeType{
    Void,Corner,Straight,Cross,T
}
