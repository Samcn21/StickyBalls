using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionData : MonoBehaviour {
    public float flameMachineDelay;
    public float annhilationDelay;
    public GameObject bigExplosionEffect;
    public GameObject smallExplosionEffect;
    private Dictionary<Vector2,bool> visited;
    private GridController gridController;

    void Awake()
    {
        gridController = GameController.Instance.GridController;
    }

    public void ExplodeFlameMachine(Pipe pipe)
    {
        visited = new Dictionary<Vector2, bool>();
        StartCoroutine(ExplosionReaction(pipe,flameMachineDelay));
    }

    private IEnumerator ExplosionReaction(Pipe pipe,float delay)
    {
        List<GameData.Coordinate> toDestroy = new List<GameData.Coordinate>();
        toDestroy.Add(pipe.positionCoordinate);
        while (toDestroy.Count > 0) 
        {
            List<GameData.Coordinate> temp = new List<GameData.Coordinate>();
            foreach (GameData.Coordinate toDestroyCoord in toDestroy)
            {
                if (!visited.ContainsKey(new Vector2(toDestroyCoord.x, toDestroyCoord.y)))
                {
                    Pipe pipeToDestroy = gridController.Grid[toDestroyCoord.x, toDestroyCoord.y].pipe;
                    foreach (GameData.Coordinate coord in pipeToDestroy.connections)
                        if (IsPipeDestroyable(gridController.Grid[coord.x, coord.y].pipe))
                        {
                            if (!visited.ContainsKey(new Vector2(coord.x, coord.y)))
                                temp.Add(coord);
                        }
                }
            }
            for(int i=toDestroy.Count-1;i>=0;i--)
            {
                if (!visited.ContainsKey(new Vector2(toDestroy[i].x,toDestroy[i].y)))
                {
                    Instantiate(GameController.Instance.ExplosionData.smallExplosionEffect, gridController.Grid[toDestroy[i].x, toDestroy[i].y].gameObject.transform.position, Quaternion.identity);
                    gridController.Grid[toDestroy[i].x, toDestroy[i].y].pipe.DestroyPipe();
                    visited.Add(new Vector2(toDestroy[i].x, toDestroy[i].y), true);
                }
                toDestroy.RemoveAt(i);
            }
            foreach (GameData.Coordinate coord in temp)
            {
               
                toDestroy.Add(coord);
            }
            yield return new WaitForSeconds(delay);
        }

    }

    private bool IsPipeDestroyable(Pipe pipe)
    {
        return pipe != null && pipe.gameObject != null && !pipe.isSource && !pipe.isFlameMachine;
    }
}
