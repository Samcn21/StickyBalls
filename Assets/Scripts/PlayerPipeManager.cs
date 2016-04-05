using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerPipeManager : MonoBehaviour {
    [SerializeField]
    private Vector2 sourcePositionRef;

    public List<Vector2> freePositions;
    public List<Vector2> lastSourcePositions;
    public void setSource(Vector2 source)
    {
        sourcePositionRef = source;
        freePositions = new List<Vector2>();
        lastSourcePositions = new List<Vector2>();
        if (sourcePositionRef == new Vector2(1, 1))
        {
            freePositions.Add(new Vector2(3, 1));
            freePositions.Add(new Vector2(1, 3));
            lastSourcePositions.Add(new Vector2(2, 1));
            lastSourcePositions.Add(new Vector2(1,2));
        }
        else
            if (sourcePositionRef.x == 1)
        {
            freePositions.Add(new Vector2(3, sourcePositionRef.y));
            freePositions.Add(new Vector2(1, sourcePositionRef.y - 2));
            lastSourcePositions.Add(new Vector2(1, sourcePositionRef.y-1));
            lastSourcePositions.Add(new Vector2(2, sourcePositionRef.y));
        }
        else
            if (sourcePositionRef.y == 1)
        {
            freePositions.Add(new Vector2(sourcePositionRef.x - 2, 1));
            freePositions.Add(new Vector2(sourcePositionRef.x, 3));
            lastSourcePositions.Add(new Vector2(sourcePositionRef.x - 1, 1));
            lastSourcePositions.Add(new Vector2(sourcePositionRef.x, 2));
        }
        else
        {
            freePositions.Add(new Vector2(sourcePositionRef.x, sourcePositionRef.y - 2));
            freePositions.Add(new Vector2(sourcePositionRef.x - 2, sourcePositionRef.y));
            lastSourcePositions.Add(new Vector2(sourcePositionRef.x, sourcePositionRef.y - 1));
            lastSourcePositions.Add(new Vector2(sourcePositionRef.x - 1, sourcePositionRef.y));
        }

    }

    public void removeFreePosition(Vector2 pos)
    {
        freePositions.Remove(pos);
    }
}
