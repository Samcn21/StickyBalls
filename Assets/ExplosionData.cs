using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionData : MonoBehaviour {
    public float flameMachineDelay;
    public float annhilationDelay;
    public GameObject bigExplosionEffect;
    public GameObject smallExplosionEffect;
    private HashSet<Vector2> visited;
    private GridController gridController;
    private List<PlayerSource> sources;

    [SerializeField]
    private AnnhilationMode annhilationMode;
    void Awake()
    {
       
        sources = new List<PlayerSource>();
        GameObject[] s = GameObject.FindGameObjectsWithTag("PlayerSource");
        foreach (GameObject g in s)
            sources.Add(g.GetComponent<PlayerSource>());
        gridController = GameController.Instance.GridController;
    }

    public void ExplodeFlameMachine(Pipe pipe)
    {
        visited = new HashSet<Vector2>();
        StartCoroutine(ExplosionReaction(pipe));
    }

    public void ExplodeAnnhilation(GameData.Team winningTeam)
    {
        visited = new HashSet<Vector2>();
        foreach(PlayerSource source in sources)
        {
            if (source.Team != winningTeam)
                StartCoroutine(ExplosionReaction(source, annhilationMode,new HashSet<Vector2>()));
        }
    }

  
    
    private List<Pipe> GetLeavesFromSource(Pipe source)
    {
        List<Pipe> result = new List<Pipe>();
        bool isLeaf = true;
        foreach (GameData.Coordinate coord in source.connections) {
            if (!visited.Contains(new Vector2(coord.x, coord.y)))
            {
                visited.Add(new Vector2(coord.x, coord.y));
                Pipe p = gridController.Grid[coord.x, coord.y].pipe;
                if (p != null)
                {
                    if (p.Team == source.Team)
                    {
                        bool isConnectedToThisPipe = false;
                        foreach (GameData.Coordinate coord1 in p.connections)
                        {
                            if (coord1.Equals(source.positionCoordinate))
                            {
                                isConnectedToThisPipe = true;
                                break;
                            }
                        }
                        if (isConnectedToThisPipe)
                        {
                            isLeaf = false;
                            foreach (Pipe leaf in GetLeavesFromSource(p))
                                result.Add(leaf);
                        }
                    }
                }
            }
        }
        if (isLeaf)
            result.Add(source);
        return result;
    }

    private IEnumerator ExplosionReaction(Pipe pipe)
    {
        List<GameData.Coordinate> toDestroy = new List<GameData.Coordinate>();
        toDestroy.Add(pipe.positionCoordinate);
        while (toDestroy.Count > 0) 
        {
            List<GameData.Coordinate> temp = new List<GameData.Coordinate>();
            foreach (GameData.Coordinate toDestroyCoord in toDestroy)
            {
                if (!visited.Contains(new Vector2(toDestroyCoord.x, toDestroyCoord.y)))
                {
                    Pipe pipeToDestroy = gridController.Grid[toDestroyCoord.x, toDestroyCoord.y].pipe;
                    foreach (GameData.Coordinate coord in pipeToDestroy.connections)
                        if (IsPipeDestroyable(gridController.Grid[coord.x, coord.y].pipe))
                        {
                            if (!visited.Contains(new Vector2(coord.x, coord.y)))
                                temp.Add(coord);
                        }
                        else
                            visited.Add(new Vector2(coord.x, coord.y));
                }
            }
            for(int i=toDestroy.Count-1;i>=0;i--)
            {
                if (!visited.Contains(new Vector2(toDestroy[i].x,toDestroy[i].y)))
                {
                    if (!IsPipeValidSource(gridController.Grid[toDestroy[i].x, toDestroy[i].y].pipe))
                    {
                        Instantiate(GameController.Instance.ExplosionData.smallExplosionEffect, gridController.Grid[toDestroy[i].x, toDestroy[i].y].gameObject.transform.position, Quaternion.identity);
                        gridController.Grid[toDestroy[i].x, toDestroy[i].y].pipe.DestroyPipe();
                        visited.Add(new Vector2(toDestroy[i].x, toDestroy[i].y));
                    }
                    else
                    {
                        Instantiate(GameController.Instance.ExplosionData.bigExplosionEffect, gridController.Grid[toDestroy[i].x, toDestroy[i].y].gameObject.transform.position, Quaternion.identity);
                        GameController.Instance.Lose(gridController.Grid[toDestroy[i].x, toDestroy[i].y].pipe.Team);
                        visited.Add(new Vector2(toDestroy[i].x, toDestroy[i].y));
                    }
                }
                toDestroy.RemoveAt(i);
            }
            foreach (GameData.Coordinate coord in temp)
            {
               
                toDestroy.Add(coord);
            }
            yield return new WaitForSeconds(flameMachineDelay);
        }

    }

    private IEnumerator ExplosionReaction(Pipe pipe, AnnhilationMode mode, HashSet<Vector2> visited)
    {
        List<GameData.Coordinate> toDestroy = new List<GameData.Coordinate>();
        if (mode == AnnhilationMode.FromSourceToLeaves)
            foreach (GameData.Coordinate coord in pipe.connections)
                toDestroy.Add(coord);
        else
        {
         
            foreach (GameData.Coordinate coord in pipe.connections) {
                Pipe tempPipe = gridController.Grid[coord.x, coord.y].pipe;
                if (tempPipe!=null && tempPipe.Team==pipe.Team)
                {
                    visited = new HashSet<Vector2>();
                    foreach (Pipe leaf in GetLeavesFromSource(tempPipe))
                    {
                        if(!leaf.isSource)
                        toDestroy.Add(leaf.positionCoordinate);
                    }
                }
           }
            
        }
        visited = new HashSet<Vector2>();
        if(toDestroy.Count==0)
        toDestroy.Add(pipe.positionCoordinate);
        while (toDestroy.Count > 0)
        {
            List<GameData.Coordinate> temp = new List<GameData.Coordinate>();
            foreach (GameData.Coordinate toDestroyCoord in toDestroy)
            {
                if (!visited.Contains(new Vector2(toDestroyCoord.x, toDestroyCoord.y)))
                {
                    Pipe pipeToDestroy = gridController.Grid[toDestroyCoord.x, toDestroyCoord.y].pipe;
                    if (pipeToDestroy != null)
                    {
                        foreach (GameData.Coordinate coord in pipeToDestroy.connections)
                            if (IsPipeDestroyable(gridController.Grid[coord.x, coord.y].pipe)&&(gridController.Grid[coord.x,coord.y].pipe.Team==pipe.Team|| gridController.Grid[coord.x, coord.y].pipe.Team == GameData.Team.Neutral))
                            {
                                if (!visited.Contains(new Vector2(coord.x, coord.y)))
                                    temp.Add(coord);
                            }
                            else
                                visited.Add(new Vector2(coord.x, coord.y));
                    }
                }
            }
            for (int i = toDestroy.Count - 1; i >= 0; i--)
            {
                if (!visited.Contains(new Vector2(toDestroy[i].x, toDestroy[i].y)))
                {
                    Pipe pTemp = gridController.Grid[toDestroy[i].x, toDestroy[i].y].pipe;
                    if (!IsPipeValidSource(pTemp))
                    {
                        if (IsPipeDestroyable(pTemp))
                        {
                            Instantiate(GameController.Instance.ExplosionData.smallExplosionEffect, gridController.Grid[toDestroy[i].x, toDestroy[i].y].gameObject.transform.position, Quaternion.identity);
                            pTemp.DestroyPipe();
                            visited.Add(new Vector2(toDestroy[i].x, toDestroy[i].y));
                        }
                    }
                    else
                    {
                        Instantiate(GameController.Instance.ExplosionData.bigExplosionEffect, gridController.Grid[toDestroy[i].x, toDestroy[i].y].gameObject.transform.position, Quaternion.identity);
                        GameController.Instance.Lose(gridController.Grid[toDestroy[i].x, toDestroy[i].y].pipe.Team);
                        visited.Add(new Vector2(toDestroy[i].x, toDestroy[i].y));
                    }
                }
                toDestroy.RemoveAt(i);
            }
            foreach (GameData.Coordinate coord in temp)
            {

                toDestroy.Add(coord);
            }
            yield return new WaitForSeconds(annhilationDelay);
        }

    }

    private bool IsPipeDestroyable(Pipe pipe)
    {
        return pipe != null && pipe.gameObject != null && !pipe.isFlameMachine;
    }

    private bool IsPipeValidSource(Pipe pipe)
    {
        return pipe != null && pipe.gameObject != null && pipe.isSource && !pipe.isFlameMachine;
    }

    public enum AnnhilationMode
    {
        FromSourceToLeaves, FromLeavesToSource
    }
}

