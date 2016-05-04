using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        for (int i = neutralPipes.Count - 1; i >= 0; i--)
        {
            foreach (GameData.Coordinate coord in pipe.connections)
            {
                if (neutralPipes[i].CheckIfTreeIsConnected(coord))
                {
                    r.AddChild(neutralPipes[i]);
                    neutralPipes.Remove(neutralPipes[i]);
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
        for (int i = neutralPipes.Count - 1; i >= 0; i--)
        {
            foreach (GameData.Coordinate coord in pipe.connections)
            {
                if (neutralPipes[i].CheckIfTreeIsConnected(coord))
                {
                    r.AddChild(neutralPipes[i]);
                    neutralPipes.Remove(neutralPipes[i]);
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
        Debug.Log(pipesPerPlayer[team].ToString());
        if(copyToDestroy.ContainsKey(team)&&copyToDestroy[team]!=null)
        copyToDestroy[team].DisconnectSubTree(pipe);
        foreach (RecursivePipe p in pipesPerPlayer[team].DestroyPipe(pipe,explosionEffect))
        {
            neutralPipes.Add(p);
        }
        Debug.Log(pipesPerPlayer[team].ToString());
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
            Debug.Log(copyToDestroy[team].ToString());
            List<RecursivePipe> temp=new List<RecursivePipe>();
            if (toDestroy.Count == 0)
                break;
            for(int i=0;i<toDestroy.Count;i++)
            {
                if (toDestroy[i] != null)
                {
                    temp.Add(toDestroy[i].father);
                    foreach (RecursivePipe child in toDestroy[i].children)
                        temp.Add(child);
                }
            }
            if (toDestroy.Count == 0)
                break;
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
                        for(int j=temp[k].children.Count-1;j>=0;j--)
                            if (temp[k]!=null&&temp[k].children[j]!=null&&temp[k].children[j].current == null)
                            {
                                temp.RemoveAt(k);
                                break;
                            }

                }
            }
            toDestroy = temp;
            yield return new WaitForSeconds(delay);
        }
    }

    private RecursivePipe GetPipeFromFlameMachineCoord(RecursivePipe pipe,GameData.Coordinate coord)
    {
        if (pipe.current != null)
        {
            foreach (GameData.Coordinate c in pipe.current.connections)
                if (coord.Equals(c))
                    return pipe;
        }
        foreach (RecursivePipe p in pipe.children)
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
                if (copyToDestroy[team].children.Count > 0)
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
        public List<RecursivePipe> children { get; private set; }

        public RecursivePipe(RecursivePipe p)
        {
                current = p.current;
                father = new RecursivePipe();
                children = new List<RecursivePipe>();
                if (p.father != null)
                {
                    father.current = p.father.current;          
                }
                father.AddChild(this);
                foreach (RecursivePipe child in p.children)
                    AddChild(new RecursivePipe(child));
            
        }

        public RecursivePipe(Pipe pipe, RecursivePipe father)
        {
            current = pipe;
            this.father = father;
            children = new List<RecursivePipe>();
        }

        public RecursivePipe(Pipe pipe)
        {
            current = pipe;
            father = null;
           children = new List<RecursivePipe>();
        }

        public RecursivePipe()
        {
            current = null;
            father = null;
            children = new List<RecursivePipe>();
        }

        public void Destroy()
        {
            foreach (RecursivePipe child in children)
                child.father = null;
            children = new List<RecursivePipe>();
            if (father != null)
                father.RemovePipeFromChildren(current);
        }

        public void AddChild(RecursivePipe pipe)
        {
            if (!children.Contains(pipe))
            {
                pipe.father = this;
                children.Add(pipe);
            }
        }

        public bool CheckIfTreeIsConnected(GameData.Coordinate coordinate)
        {
            if (current.positionCoordinate.Equals(coordinate))
                return true;
            foreach (RecursivePipe p in children)
                if (p.CheckIfTreeIsConnected(coordinate))
                    return true;
            return false;
        }

        private void RemoveChild(RecursivePipe pipe)
        {
            if (children.Contains(pipe))
                children.Remove(pipe);
        }

        public RecursivePipe SearchAndAddChild(Pipe toAdd,Pipe father)
        {
            RecursivePipe ris=null;
            if (current==father)
            {
                 ris= new RecursivePipe(toAdd, this);
                AddChild(ris);
            }
            else
            {
                foreach (RecursivePipe r in children) {
                    RecursivePipe t = r.SearchAndAddChild(toAdd, father);
                    if (t != null)
                    {
                        ris = t;
                        break;
                    }
                        
                }
            }
            return ris;
        }

        public void DisconnectSubTree(Pipe pipe)
        {
            if (current != null && current.positionCoordinate.Equals(pipe))
            {


                foreach (RecursivePipe child in children)
                {
                    child.father = null;
                }
                children = new List<RecursivePipe>();   
                
            }
                
            else
                foreach (RecursivePipe child in children)
                    child.DisconnectSubTree(pipe);
        }

        public RecursivePipe AddFirstChild(Pipe toAdd)
        {
            RecursivePipe r = new RecursivePipe(toAdd, this);
            AddChild(r);
            return r;
        }

        public void DestroyFromLeaves(GameObject explosionEffect)
        {
            if (children.Count == 0)
            {
                if(father!=null)
                father.children.Remove(this);
                if (current != null)
                {
                    current.DestroyPipe();
                    Instantiate(explosionEffect, current.gameObject.transform.position, Quaternion.identity);                    
                }

            }
            else
            {
                for(int i=children.Count-1;i>=0;i--)
                    children[i].DestroyFromLeaves(explosionEffect);
            }
            if (father != null)
                father.children.Remove(this);
            if (current != null)
            {
                current.DestroyPipe();
                Instantiate(explosionEffect, current.gameObject.transform.position, Quaternion.identity);    
            }
        }

        public List<RecursivePipe> DestroyLeaves()
        {
            List<RecursivePipe> l = new List<RecursivePipe>();
            if(children.Count==0)
            {
                if (father != null)
                    father.children.Remove(this);
                if(current!=null)
                {
                    l.Add(this);
                    return l;
                }
            }
            else
            {
                for (int i = children.Count - 1; i >= 0; i--)
                    foreach (RecursivePipe r in children[i].DestroyLeaves())
                        l.Add(r);
            }
            return l;
        }

        private void RemovePipeFromChildren(Pipe pipe)
        {
            foreach(RecursivePipe p in children)
                if(p.current.positionCoordinate.Equals(pipe.positionCoordinate))
                {
                    children.Remove(p);
                    break;
                }
        }

        public List<RecursivePipe> DestroyPipe(Pipe pipe,GameObject explosionEffect)
        {
            if (current!=null&&current.positionCoordinate.Equals(pipe.positionCoordinate))
            {
                Debug.Log("entra");
                Instantiate(explosionEffect, current.gameObject.transform.position, Quaternion.identity); 
                if (father != null)
                    father.RemovePipeFromChildren(pipe);
                pipe.DestroyPipe();
                return children;
            }
            else
                foreach (RecursivePipe p in children)
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
        
        override public string ToString()
        {
            string s = "";
            if(current!=null)
            s += current.PipeType;
            if (children.Count > 0) {
                s += "[";
                foreach (RecursivePipe p in children)
                    s += p.ToString()+", ";
                s += "]";
             }
            return s;
        }
    }
}
