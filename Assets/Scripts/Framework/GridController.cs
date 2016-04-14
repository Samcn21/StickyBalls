﻿using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour
{
    
    [SerializeField] private Transform boardGenerationPoint;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int gridHeight = 9;
    [SerializeField] private int gridWidth = 16;

    public Tile[,] Grid;
	// Use this for initialization
	void Start ()
	{
	    Grid = new Tile[gridWidth, gridHeight];
        GenerateGrid(boardGenerationPoint.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GenerateGrid(Vector3 originPoint)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GameObject tileGameObject = Instantiate(tilePrefab, new Vector3(originPoint.x + x,originPoint.y, originPoint.z + y), Quaternion.Euler(90, 0, 0)) as GameObject;
                tileGameObject.name = x + "," + y;
                tileGameObject.transform.parent = transform;
                Tile tile = tileGameObject.GetComponent<Tile>();
                Grid[x, y] = tile;
            }
        }
    }
}
