using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PipeStatus : MonoBehaviour {
    public List<PlayerSource> playerSourcesRef { get; private set; }
    private Dictionary<GameData.Team, RecursivePipe> pipesPerPlayer;
    private Dictionary<GameData.Team, RecursivePipe> copyToDestroy;
    private List<RecursivePipe> neutralPipes;
    [SerializeField]
    private bool destroyAllThePipes;
    [SerializeField]
    private float delay;

    private bool destroyAllPipes = false;
    [SerializeField]
    private GameObject explosionEffect;

    private List<GameData.Team> teamsToDestroy;
    private GridController gridController;
	void Awake()
    {
        gridController = GetComponent<GridController>();
        playerSourcesRef = new List<PlayerSource>();
        GameObject[] objs=GameObject.FindGameObjectsWithTag("PlayerSource");
        foreach(GameObject obj in objs)
        {
            playerSourcesRef.Add(obj.GetComponent<PlayerSource>());
        }
        RecursivePipe voidList = new RecursivePipe();
        pipesPerPlayer = new Dictionary<GameData.Team, RecursivePipe>();
        pipesPerPlayer.Add(GameData.Team.Blue, voidList);
        voidList = new RecursivePipe();
        pipesPerPlayer.Add(GameData.Team.Cyan, voidList);
        voidList = new RecursivePipe();
        pipesPerPlayer.Add(GameData.Team.Purple, voidList);
        voidList = new RecursivePipe();
        pipesPerPlayer.Add(GameData.Team.Yellow, voidList);
        teamsToDestroy = new List<GameData.Team>();
        neutralPipes = new List<RecursivePipe>();
        copyToDestroy = new Dictionary<GameData.Team, RecursivePipe>();
    }

    public void AddPipeToTeam(GameData.Team team, Pipe pipe,Pipe father)
    {
        RecursivePipe tree = pipesPerPlayer[team];
        RecursivePipe r = tree.SearchAndAddChild(pipe,father);
      /*  if(tree.GetChildren().Count>1)
        {
            RecursivePipe rootParent = r.GetRootPipe();
            RecursivePipe start;
            if(rootParent.current.positionCoordinate.Equals(tree.firstChild.current.positionCoordinate))
            {
                start = tree.firstChild.nextBrother;
            }
            else
            {
                start = tree.firstChild;
            }
            foreach (GameData.Coordinate coord in pipe.connections)
            {
                start.SearchAndAddAsParent(r, coord);
            }
        }*/
        Debug.Log(tree.ToString());
        if (copyToDestroy.ContainsKey(team) && copyToDestroy[team] != null)
            copyToDestroy[team].ReconnectSubTree(pipe, father, neutralPipes);
        for (int i = neutralPipes.Count - 1; i >= 0; i--)
        {
            foreach (GameData.Coordinate coord in pipe.connections)
            {
                if (neutralPipes[i].CheckIfTreeIsConnected(coord))
                {
                    r.AddChild(neutralPipes[i]);
                    neutralPipes.RemoveAt(i);
                    break;
                }
            }
        }

        pipesPerPlayer[team] = tree;
    }

    public void AddFirstPipe(GameData.Team team, Pipe pipe)
    {
        RecursivePipe tree = pipesPerPlayer[team];
        RecursivePipe r=tree.AddFirstChild(pipe);
        if (copyToDestroy.ContainsKey(team) && copyToDestroy[team] != null)
            copyToDestroy[team].ReconnectSubTree(pipe,tree.firstChild.current,neutralPipes);
        for (int i = neutralPipes.Count - 1; i >= 0; i--)
        {
            foreach (GameData.Coordinate coord in pipe.connections)
            {
                if (neutralPipes[i].CheckIfTreeIsConnected(coord))
                {
                    r.AddChild(neutralPipes[i]);
                    neutralPipes.RemoveAt(i);
                    break;
                }
            }
        }
        pipesPerPlayer[team] = tree;
    }

    public void DestroyPipesOfPlayer(GameData.Team team)
    {
        destroyAllPipes = true;
        if (!destroyAllThePipes)
        {
            if (!teamsToDestroy.Contains(team))
                teamsToDestroy.Add(team);
        }
        else
        {
            if (!teamsToDestroy.Contains(GameData.Team.Blue))
                teamsToDestroy.Add(GameData.Team.Blue);
            if (!teamsToDestroy.Contains(GameData.Team.Cyan))
                teamsToDestroy.Add(GameData.Team.Cyan);
            if (!teamsToDestroy.Contains(GameData.Team.Purple))
                teamsToDestroy.Add(GameData.Team.Purple);
            if (!teamsToDestroy.Contains(GameData.Team.Yellow))
                teamsToDestroy.Add(GameData.Team.Yellow);
        }
    }

    public void DestroyPipeOfPlayer(GameData.Team team,Pipe pipe)
    {
        
        if(copyToDestroy.ContainsKey(team)&&copyToDestroy[team]!=null)
        copyToDestroy[team].DisconnectSubTree(pipe);
        foreach (RecursivePipe p in pipesPerPlayer[team].DestroyPipe(pipe,explosionEffect))
        {
            neutralPipes.Add(p);
        }
        Debug.Log("AFTER DESTRUCTION"+pipesPerPlayer[team].ToString());
    }

    void Update()
    {
        if(destroyAllPipes)
        {
            copyToDestroy = new Dictionary<GameData.Team, RecursivePipe>();
            copyToDestroy.Add(GameData.Team.Blue, new RecursivePipe(pipesPerPlayer[GameData.Team.Blue]));
            copyToDestroy.Add(GameData.Team.Cyan, new RecursivePipe(pipesPerPlayer[GameData.Team.Cyan]));
            copyToDestroy.Add(GameData.Team.Purple, new RecursivePipe(pipesPerPlayer[GameData.Team.Purple]));
            copyToDestroy.Add(GameData.Team.Yellow, new RecursivePipe(pipesPerPlayer[GameData.Team.Yellow]));
            StartCoroutine(DestroyLeavesWithDelay());
            destroyAllPipes = false;
        }
    }

    public void DestroyPipesFromFlameMachine(GameData.Coordinate flameMachineCoord, GameData.Team team)
    {
        copyToDestroy = new Dictionary<GameData.Team, RecursivePipe>();
        copyToDestroy.Add(GameData.Team.Blue, new RecursivePipe(pipesPerPlayer[GameData.Team.Blue]));
        copyToDestroy.Add(GameData.Team.Cyan, new RecursivePipe(pipesPerPlayer[GameData.Team.Cyan]));
        copyToDestroy.Add(GameData.Team.Purple, new RecursivePipe(pipesPerPlayer[GameData.Team.Purple]));
        copyToDestroy.Add(GameData.Team.Yellow, new RecursivePipe(pipesPerPlayer[GameData.Team.Yellow]));
        StartCoroutine(DestroyPipeFromFlameMachine(flameMachineCoord,team));
    }

    private IEnumerator DestroyLeavesWithDelay()
    {
        bool exit = false;
        while (leavesLeft()&&!exit)
        {
            foreach (GameData.Team team in teamsToDestroy)
            {
                List<RecursivePipe> leaves = copyToDestroy[team].DestroyLeaves();
                if (leaves.Count == 0)
                    exit = true;
                foreach (RecursivePipe leave in leaves)
                {
                    Instantiate(explosionEffect, leave.current.gameObject.transform.position, Quaternion.identity);
                    leave.current.DestroyPipe();
                }

            }
            yield return new WaitForSeconds(delay);
        }
        
    }

    private IEnumerator DestroyPipeFromFlameMachine(GameData.Coordinate flameMachineCoord,GameData.Team team)
    {
        List<RecursivePipe>  toDestroy = new List<RecursivePipe>();
        toDestroy.Add(GetPipeFromFlameMachineCoord(copyToDestroy[team], flameMachineCoord));

        while (toDestroy.Count>0)
        {
            Debug.Log(copyToDestroy[team]);
            List<RecursivePipe> temp=new List<RecursivePipe>();
            for(int i=0;i<toDestroy.Count;i++)
            {
                if (toDestroy[i] != null)
                {
                    temp.Add(toDestroy[i].father);
                    foreach (RecursivePipe child in toDestroy[i].GetChildren())
                        temp.Add(child);
                    RecursivePipe p = copyToDestroy[team].FindPipe(toDestroy[i]);
                    if (p != null)
                    {
                        foreach(RecursivePipe t in p.GetRelatives())
                        {
                            if (t.current == null)
                                continue;
                            bool found = false;
                            foreach (RecursivePipe t1 in temp)
                            {
                                if (t1==null || t1.current == null)
                                    continue;
                                if (t.current.positionCoordinate.Equals(t1.current.positionCoordinate))
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                                temp.Add(t);
                        }
                    }
                }
            }
            for (int i=toDestroy.Count-1;i>=0;i--) 
                {
                
                if (toDestroy[i]!=null&&toDestroy[i].current != null)
                {
                    Instantiate(explosionEffect, toDestroy[i].current.gameObject.transform.position, Quaternion.identity);
                    toDestroy[i].current.DestroyPipe();
                    toDestroy[i].Destroy();
                    toDestroy.RemoveAt(i);
                }
                else
                {
                    for (int k = temp.Count - 1; k >= 0; k--)
                        if(temp[k]!=null)
                        for(int j=temp[k].GetChildren().Count-1;j>=0;j--)
                            if (temp[k]!=null&&temp[k].GetChildren()[j]!=null&&temp[k].GetChildren()[j].current == null)
                            {
                                temp.RemoveAt(k);
                                break;
                            }

                }
            }
            toDestroy = temp;
            yield return new WaitForSeconds(delay);
        }
        copyToDestroy[team] = null;
    }

    private RecursivePipe GetPipeFromFlameMachineCoord(RecursivePipe pipe,GameData.Coordinate coord)
    {
        if (pipe.current != null)
        {
            foreach (GameData.Coordinate c in pipe.current.connections)
                if (coord.Equals(c))
                    return pipe;
        }
        foreach (RecursivePipe p in pipe.GetChildren())
            {
                RecursivePipe t = GetPipeFromFlameMachineCoord(p, coord);
                if (t != null)
                    return t;
            }
        
            return null;
    }

    private bool leavesLeft()
    {
        foreach(GameData.Team team in teamsToDestroy)
                if (copyToDestroy[team].GetChildren().Count > 0)
                    return true;
           

        return false;
    }

    public PlayerSource GetPlayerSourceFromTeam(GameData.Team team)
    {
        switch (team)
        {
            case GameData.Team.Blue:
                foreach (PlayerSource source in playerSourcesRef)
                    if (source.sourceLocation == GameData.PlayerSourceDirection.BottomRight)
                        return source;
                break;
            case GameData.Team.Cyan:
                foreach (PlayerSource source in playerSourcesRef)
                    if (source.sourceLocation == GameData.PlayerSourceDirection.TopLeft)
                        return source;
                break;
            case GameData.Team.Purple:
                foreach (PlayerSource source in playerSourcesRef)
                    if (source.sourceLocation == GameData.PlayerSourceDirection.BottomLeft)
                        return source;
                break;
            case GameData.Team.Yellow:
                foreach (PlayerSource source in playerSourcesRef)
                    if (source.sourceLocation == GameData.PlayerSourceDirection.TopRight)
                        return source;
                break;      
        }
        return null;
    }
    
    internal class RecursivePipe 
    {
        public RecursivePipe father { get; private set; }
        public Pipe current { get; private set; }
        public RecursivePipe firstChild { get; private set; }
        public RecursivePipe nextBrother { get; private set; }

        public RecursivePipe(RecursivePipe p)
        {
            father = null;
            firstChild = null;
            nextBrother = null;
            current = p.current;
            if (p.father != null)
            {
                father = new RecursivePipe(p.father.current);
                father.AddChild(this);
            }
            if (p.firstChild != null)
            {
                firstChild = new RecursivePipe(p.firstChild,father);
            }
            if (p.nextBrother != null)
            {
                nextBrother = new RecursivePipe(p.nextBrother,father);
            }
        }

        public RecursivePipe(RecursivePipe p, RecursivePipe father)
        {
            this.father = father;
            firstChild = null;
            nextBrother = null;
            current = p.current;
            if (p.firstChild != null)
            {
                firstChild = new RecursivePipe(p.firstChild,this);
            }
            if (p.nextBrother != null)
            {
                nextBrother = new RecursivePipe(p.nextBrother,father);
            }
        }

        public RecursivePipe(Pipe pipe)
        {
            current = pipe;
            father = null;
            firstChild = null;
            nextBrother = null;
        }

        public RecursivePipe()
        {
            current = null;
            father = null;
            firstChild = null;
            nextBrother = null;
        }

        public void Destroy()
        {
            DisconnectPipe();
        }

        public void AddChild(RecursivePipe pipe)
        {
            
            pipe.father = this;
            if (firstChild == null)
            {
                firstChild = pipe;
            }
            else
            {
                RecursivePipe last = firstChild.LastBrother();
                last.nextBrother = pipe;
            }
            
        }

        private RecursivePipe LastBrother()
        {
            if (nextBrother == null)
                return this;
            else
                return nextBrother.LastBrother();
        }

        public bool CheckIfTreeIsConnected(GameData.Coordinate coordinate)
        {
            if (current.positionCoordinate.Equals(coordinate))
                return true;
            if (nextBrother != null)
                if (nextBrother.CheckIfTreeIsConnected(coordinate))
                    return true;
            if (firstChild != null)
                return firstChild.CheckIfTreeIsConnected(coordinate);
            return false;
        }

        private void RemoveChild(RecursivePipe pipe)
        {
            if (firstChild != null)
            {
                if (firstChild.current.positionCoordinate.Equals(pipe.current.positionCoordinate))
                {
                    firstChild.father = null;
                    firstChild = firstChild.nextBrother;
                    return;
                }
                if(firstChild.nextBrother!=null)
                {
                    RecursivePipe p = firstChild.nextBrother;
                    if(p.current.positionCoordinate.Equals(pipe.current.positionCoordinate))
                    {
                        DisconnectPipe();
                        return;
                    }
                    do
                    {
                        if (p.nextBrother.current.positionCoordinate.Equals(pipe.current.positionCoordinate))
                        {
                            DisconnectPipe();
                            return;
                        }
                        p = p.nextBrother;
                    }while(p!=null);
                }
            }
        }

        public void DisconnectPipe()
        {
            if(father!=null)
            {
                if (father.firstChild.current.positionCoordinate.Equals(current.positionCoordinate))
                    father.firstChild = father.firstChild.nextBrother;
                else
                {
                    RecursivePipe p = father.firstChild;
                    while (p != null)
                    {
                        if(p.nextBrother.current.positionCoordinate.Equals(current.positionCoordinate))
                        {
                            p.nextBrother = p.nextBrother.nextBrother;
                            break;
                        }
                        p = p.nextBrother;
                    }
                }
                father = null;
            }
            if (firstChild != null)
            {
                RecursivePipe next = firstChild.nextBrother;
                firstChild.nextBrother = null;
                firstChild.father = null;
                while(next!=null)
                {
                    RecursivePipe n = next.nextBrother;
                    next.nextBrother = null;
                    next.father = null;
                    next = n;
                }
                firstChild = null;
            }
            
        }

        public RecursivePipe GetRootPipe()
        {
            if (father.current == null)
                return this;
            return father.GetRootPipe();
        }

        public bool SearchAndAddAsParent(RecursivePipe child, GameData.Coordinate fatherCoordinates)
        {
          //  Debug.Log(ToString());
            if (current.positionCoordinate.Equals(fatherCoordinates))
            {
                bool matches = false;
                foreach (GameData.Coordinate coord in current.connections)
                    if (coord.Equals(child.current.positionCoordinate))
                        matches = true;
                if (!matches)
                    return false;
                if (child.firstChild == null)
                    child.firstChild = this;
                else
                {
                    RecursivePipe p = child.firstChild.nextBrother;
                    if (p != null)
                    {
                        while (p.nextBrother != null)
                            p = p.nextBrother;
                        p.nextBrother = this;
                    }
                    else
                    {
                        child.firstChild.nextBrother = this;
                    }
                    return true;
                }
            }

            if (nextBrother != null)
                if (nextBrother.SearchAndAddAsParent(child, fatherCoordinates))
                    return true;
            if (firstChild != null)
                if (firstChild.SearchAndAddAsParent(child, fatherCoordinates))
                    return true;

            return false;
        }

        public RecursivePipe SearchAndAddChild(Pipe toAdd,Pipe father)
        {
            RecursivePipe ris=null;
            if (current!=null&&current.positionCoordinate.Equals(father.positionCoordinate))
            {
                 ris= new RecursivePipe(toAdd);
                AddChild(ris);
            }
            else
            {
                if (nextBrother != null)
                {
                    RecursivePipe t = nextBrother.SearchAndAddChild(toAdd, father);
                    if (t != null)
                    {
                        return t;
                    }
                }
                if(firstChild!=null)
                {
                    ris=firstChild.SearchAndAddChild(toAdd,father);
                }
            }
            return ris;
        }

        public void DisconnectSubTree(Pipe pipe)
        {
            if (current != null && current.positionCoordinate.Equals(pipe.positionCoordinate))
            {
                DisconnectPipe();
                return;
            }
            else
                foreach (RecursivePipe p in GetChildren())
                {
                    p.DisconnectSubTree(pipe);
                }
        }

        public RecursivePipe AddFirstChild(Pipe toAdd)
        {
            RecursivePipe child = new RecursivePipe(toAdd);
            child.father = this;
            if (firstChild != null)
            {
                child.nextBrother = firstChild;
                firstChild = child;
            }
            else
                firstChild = child;
            return child;
        }

        public void DestroyFromLeaves(GameObject explosionEffect)
        {
            if (firstChild==null)
            {
                if (father != null)
                    father.RemoveChild(this);
                if (current != null)
                {
                    current.DestroyPipe();
                    Instantiate(explosionEffect, current.gameObject.transform.position, Quaternion.identity);                    
                }

            }
            else
            {
                firstChild.DestroyFromLeaves(explosionEffect);
                if(nextBrother!=null)
                nextBrother.DestroyFromLeaves(explosionEffect);
            }
            if (father != null)
                father.RemoveChild(this);
            if (current != null)
            {
                current.DestroyPipe();
                Instantiate(explosionEffect, current.gameObject.transform.position, Quaternion.identity);    
            }
        }

        public List<RecursivePipe> DestroyLeaves()
        {
            List<RecursivePipe> l = new List<RecursivePipe>();
            if(firstChild!=null)
            {
                if (father != null)
                    father.RemoveChild(this);
                if(current!=null)
                {
                    l.Add(this);
                    return l;
                }
            }
            else
            {
                
                    foreach (RecursivePipe r in firstChild.DestroyLeaves())
                        l.Add(r);
                if (nextBrother != null)
                    foreach (RecursivePipe r in nextBrother.DestroyLeaves())
                        l.Add(r);
            }
            return l;
        }

        private void RemovePipeFromChildren(Pipe pipe)
        {
            RemoveChild(new RecursivePipe(pipe));
        }

        public List<RecursivePipe> GetChildren()
        {
            List<RecursivePipe> pipes = new List<RecursivePipe>();
            if (firstChild != null)
            {
                pipes.Add(firstChild);

                RecursivePipe p = firstChild.nextBrother;
                while (p != null)
                {
                    pipes.Add(p);
                    p = p.nextBrother;
                }
            }
            return pipes;
        }

        public List<RecursivePipe> DestroyPipe(Pipe pipe,GameObject explosionEffect)
        {
            if (current!=null&&current.positionCoordinate.Equals(pipe.positionCoordinate))
            {
                Debug.Log("entra");
                List<RecursivePipe> children = GetChildren();
                Instantiate(explosionEffect, current.gameObject.transform.position, Quaternion.identity);
                DisconnectPipe();
                pipe.DestroyPipe();
                return children;
            }
            else
                foreach (RecursivePipe p in GetChildren())
                {
                    List<RecursivePipe> neutral = p.DestroyPipe(pipe, explosionEffect);
                    if (neutral.Count > 0)
                        return neutral;
                }
                    
            return new List<RecursivePipe>();
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.2f);
        }

        public void ReconnectSubTree(Pipe pipe, Pipe father, List<RecursivePipe> neutralPipes)
        {
            if (current!=null && current.positionCoordinate.Equals(father.positionCoordinate))
            {
                RecursivePipe p = new RecursivePipe(pipe);
                AddChild(p);
                for (int i = neutralPipes.Count - 1; i >= 0; i--)
                {
                    foreach (GameData.Coordinate coord in pipe.connections)
                    {
                        if (neutralPipes[i].CheckIfTreeIsConnected(coord))
                        {
                            p.AddChild(neutralPipes[i]);
                            break;
                        }
                    }
                }
                return;
            }
            
                if (nextBrother != null)
                nextBrother.ReconnectSubTree(pipe, father, neutralPipes);
            if (firstChild != null)
                firstChild.ReconnectSubTree(pipe, father, neutralPipes);
        }

        public List<RecursivePipe> GetRelatives()
        {
            List<RecursivePipe> r = new List<RecursivePipe>();
            if (father != null)
                r.Add(father);
            if(firstChild!=null)
            {
                r.Add(firstChild);
                RecursivePipe t = firstChild.nextBrother;
                while(t!=null)
                {
                    r.Add(t);
                    t = t.nextBrother;
                }
            }
            return r;
        }

        public RecursivePipe FindPipe(RecursivePipe p)
        {
            if (current!=null&&current.positionCoordinate.Equals(p.current.positionCoordinate))
                return this;
            RecursivePipe r=null;
            if (nextBrother != null)
                r=nextBrother.FindPipe(p);
            if (r != null)
                return r;
            if (firstChild != null)
                return firstChild.FindPipe(p);
            return null;
        }
        
        override public string ToString()
        {
            string s = "";
            if(current!=null)
            s += current.PipeType;
            if (GetChildren().Count > 0) {
                s += "[";
                foreach (RecursivePipe p in GetChildren())
                    s += p.ToString()+", ";
                s += "]";
             }
            return s;
        }

    }
}
