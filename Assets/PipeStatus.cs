using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipeStatus : MonoBehaviour {
    public List<PlayerSource> playerSourcesRef { get; private set; }
    private Dictionary<GameData.Team, RecursivePipe> pipesPerPlayer;
    private List<RecursivePipe> neutralPipes;
    [SerializeField]
    private bool destroyAllThePipes;

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
    }

    public void AddPipeToTeam(GameData.Team team, Pipe pipe,Pipe father)
    {
        RecursivePipe tree = pipesPerPlayer[team];
        tree.SearchAndAddChild(pipe,father);
        for (int i = neutralPipes.Count - 1; i >= 0; i--)
        {
            foreach (GameData.Coordinate coord in pipe.connections)
            {
                if (neutralPipes[i].CheckIfTreeIsConnected(coord))
                {
                    tree.AddChild(neutralPipes[i]);
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
        tree.AddFirstChild(pipe);
        for (int i = neutralPipes.Count - 1; i >= 0; i--)
        {
            foreach (GameData.Coordinate coord in pipe.connections)
            {
                if (neutralPipes[i].CheckIfTreeIsConnected(coord))
                {
                    tree.AddChild(neutralPipes[i]);
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
        Debug.Log(pipe.PipeType+" "+pipe.positionCoordinate.x+" ,"+pipe.positionCoordinate.y);
        foreach(RecursivePipe p in pipesPerPlayer[team].DestroyPipe(pipe,explosionEffect))
        {
            neutralPipes.Add(p);
        }

    }

    void Update()
    {
        if(destroyAllPipes)
        {
            foreach(GameData.Team team in teamsToDestroy)
                pipesPerPlayer[team].DestroyFromLeaves(explosionEffect);
            destroyAllPipes = false;
        }
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


        public void AddChild(RecursivePipe pipe)
        {
            if(!children.Contains(pipe))
                children.Add(pipe);
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

        public void SearchAndAddChild(Pipe toAdd,Pipe father)
        {
            if(current==father)
            {
                AddChild(new RecursivePipe(toAdd,this));
            }
            else
            {
                foreach (RecursivePipe r in children)
                    r.SearchAndAddChild(toAdd, father);
            }
        }

        public void AddFirstChild(Pipe toAdd)
        {
            AddChild(new RecursivePipe(toAdd,this));
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

        private void RemovePipeFromChildren(Pipe pipe)
        {
            foreach(RecursivePipe p in children)
                if(p.current.positionCoordinate.Equals(pipe))
                {
                    children.Remove(p);
                    break;
                }
        }

        public List<RecursivePipe> DestroyPipe(Pipe pipe,GameObject explosionEffect)
        {
            if (current!=null&&current.positionCoordinate.Equals(pipe.positionCoordinate))
            {
                Instantiate(explosionEffect, current.gameObject.transform.position, Quaternion.identity);
                pipe.DestroyPipe();
                if (father != null)
                    father.RemovePipeFromChildren(pipe);
                return children;
            }
            else
                foreach (RecursivePipe p in children)
                    return p.DestroyPipe(pipe,explosionEffect);
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
