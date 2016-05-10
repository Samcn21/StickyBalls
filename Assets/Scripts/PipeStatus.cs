using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PipeStatus : MonoBehaviour {
    public bool DestroySinglePipeActive;
    public float TimerToDestroyPipe;

    public List<PlayerSource> playerSourcesRef { get; private set; }
    //This dictionary will hold the pipe tree for each player
    private Dictionary<GameData.Team, RecursivePipe> pipesPerPlayer;
    //When the destruction(complete) of the pipe starts, this dictionary will be used instead
    private Dictionary<GameData.Team, RecursivePipe> copyToDestroy;
    //This list will contain the Tree form of the neutral pipes
    private List<RecursivePipe> neutralPipes;
    [SerializeField]
    private bool eliminatePlayerWithPlayerSource;
    [SerializeField]
    private bool destroyAllThePipes;
    [SerializeField]
    private float delay;
    public GameObject bigExplosionEffect;
    [SerializeField]
    private GameObject explosionEffect;
    //This list contains the pipes that will be destroyed
    private List<GameData.Team> teamsToDestroy;
    //Reference to the grid controller
    private GridController gridController;
    //List that will be reset every time that the tree will be explored. Will contain the already explored pipes
    private static List<RecursivePipe> exploredPipes;

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
        pipesPerPlayer[GameData.Team.Blue] = voidList;
        voidList = new RecursivePipe();
        pipesPerPlayer[GameData.Team.Cyan] = voidList;
        voidList = new RecursivePipe();
        pipesPerPlayer[GameData.Team.Purple] = voidList;
        voidList = new RecursivePipe();
        pipesPerPlayer[GameData.Team.Yellow] = voidList;
        teamsToDestroy = new List<GameData.Team>();
        neutralPipes = new List<RecursivePipe>();
        copyToDestroy = new Dictionary<GameData.Team, RecursivePipe>();
    }
    /// <summary>
    /// This function will be called to add a Pipe to a team
    /// </summary>
    /// <param name="team">The team owner of the pipe</param>
    /// <param name="pipe">The pipe to add to the team</param>
    /// <param name="father">The father(the pipe where the pipe is attached) of the pipe</param>
    public void AddPipeToTeam(GameData.Team team, Pipe pipe,Pipe father)
    {
        RecursivePipe tree = pipesPerPlayer[team];
        exploredPipes = new List<RecursivePipe>();
        RecursivePipe r = tree.SearchAndAddChild(pipe,father);
       if(tree.GetChildren().Count>1)
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
                exploredPipes = new List<RecursivePipe>();
                start.SearchAndAddAsParent(r, coord);
            }
        }
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

    /// <summary>
    /// This function will be called to place the first pipe of the tree of pipes.
    /// Note that compared to the previous funciton, the father does not exist because the first pipe will be connected to the 
    /// root of the tree
    /// </summary>
    /// <param name="team">The team owner of the pipe</param>
    /// <param name="pipe">The pipe to add</param>
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

    /// <summary>
    /// This function is called to destroy the pipes of a certain player. It is used by the button
    /// </summary>
    /// <param name="team">The team owner of the pipe to be destroyed</param>
    public void DestroyPipesOfPlayer(GameData.Team team)
    {
       
        if (!destroyAllThePipes)
        {
            if (!teamsToDestroy.Contains(team))
                teamsToDestroy.Add(team);       

            if (!pipesPerPlayer.ContainsKey(team))
                copyToDestroy.Add(team, new RecursivePipe());
            else
                copyToDestroy.Add(team, new RecursivePipe(pipesPerPlayer[team]));      
        StartCoroutine(DestroyLeavesWithDelay());
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

    /// <summary>
    /// This function is called to destroy a single pipe that belongs to a player
    /// </summary>
    /// <param name="team">The team owner of the pipe</param>
    /// <param name="pipe">The pipe to be destroyed</param>
    /// <param name="instantiateEffect">If it has to instantiate the effect</param>
    public void DestroyPipeOfPlayer(GameData.Team team,Pipe pipe,bool instantiateEffect)
    {
        
        if(copyToDestroy.ContainsKey(team)&&copyToDestroy[team]!=null)
        copyToDestroy[team].DisconnectSubTree(pipe);
        foreach (RecursivePipe p in pipesPerPlayer[team].DestroyPipe(pipe,explosionEffect,instantiateEffect))
        {
            neutralPipes.Add(p);
        }
    }



    /// <summary>
    /// This function si called to start the destruction of the pipe connected to the flame machine
    /// </summary>
    /// <param name="flameMachineCoord">The coordinates of the flame machine</param>
    /// <param name="team">The team whose pipe is going to be destroyed</param>
    public void DestroyPipesFromFlameMachine(GameData.Coordinate flameMachineCoord, GameData.Team team)
    {
        copyToDestroy = new Dictionary<GameData.Team, RecursivePipe>();
        copyToDestroy.Add(team, new RecursivePipe(pipesPerPlayer[team]));
        StartCoroutine(DestroyPipeFromFlameMachine(flameMachineCoord,team));
    }

    /// <summary>
    /// The coroutine that destroys the pipe starting from the leaves. Used by the button mechanic
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyLeavesWithDelay()
    {
        bool exit;
        do
        {

            exit = true;
            foreach (GameData.Team team in teamsToDestroy)
            {
                List<RecursivePipe> leaves = copyToDestroy[team].DestroyLeaves();
                if (leaves.Count != 0)
                    exit = false;
                foreach (RecursivePipe leave in leaves)
                {
                    Instantiate(explosionEffect, leave.current.gameObject.transform.position, Quaternion.identity);
                    leave.current.DestroyPipe();
                    RecursivePipe p = pipesPerPlayer[team].FindPipe(leave);
                    if (p != null)
                        p.DisconnectPipe();
                }

            }
            yield return new WaitForSeconds(delay);
        } while (leavesLeft() && !exit);
        foreach (GameData.Team team in teamsToDestroy)
            GameController.Instance.Lose(team);
    }

    public void Annhilation(GameData.Team winningTeam)
    {
        List<GameData.Team> teams = new List<GameData.Team>();
        if (GameController.Instance.Gamemode_IsCoop)
        {
            teams.Add(GameData.Team.Purple);
            teams.Add(GameData.Team.Cyan);
        }
        else
        {
            teams.Add(GameData.Team.Purple);
            teams.Add(GameData.Team.Yellow);
            teams.Add(GameData.Team.Cyan);
            teams.Add(GameData.Team.Blue);
        }
        foreach (GameData.Team team in teams)
        {
            if (team == winningTeam)
                continue;
            teamsToDestroy.Add(team);
            if (!pipesPerPlayer.ContainsKey(team))
                copyToDestroy.Add(team, new RecursivePipe());
            else
                copyToDestroy.Add(team, new RecursivePipe(pipesPerPlayer[team]));
        }
        StartCoroutine(DestroyLeavesWithDelay());
    }

    /// <summary>
    /// Function that destroys the pipe starting from the flame machine coordinates. Used by the flame machine mechanic
    /// </summary>
    /// <param name="flameMachineCoord">The coordinates of the flame machine</param>
    /// <param name="team">The color of the team</param>
    /// <returns></returns>
    private IEnumerator DestroyPipeFromFlameMachine(GameData.Coordinate flameMachineCoord,GameData.Team team)
    {
        List<RecursivePipe>  toDestroy = new List<RecursivePipe>();
        toDestroy.Add(GetPipeFromFlameMachineCoord(copyToDestroy[team], flameMachineCoord));
        bool blowUpEntirePipe = false;
        while (toDestroy.Count>0)
        {
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
                    RecursivePipe p = pipesPerPlayer[team].FindPipe(toDestroy[i]);
                    if (p==null||p.father==null||p.father.current==null)
                        blowUpEntirePipe = true;
                    if (p != null)
                        p.DisconnectPipe();
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
       if(blowUpEntirePipe)
            GameController.Instance.Lose(team);
        copyToDestroy[team] = null;
    }

    /// <summary>
    /// Function that will be called to get the tree type pipe that is connected to the flame machine.
    /// </summary>
    /// <param name="pipe">The pipe to be analyzed</param>
    /// <param name="coord">The coordinates of the flame machine</param>
    /// <returns>The tree type pipe connected to the machine</returns>
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

    /// <summary>
    /// Checks if a tree has leaves left. Used by the button mechanic
    /// </summary>
    /// <returns>True or false</returns>
    private bool leavesLeft()
    {
        foreach(GameData.Team team in teamsToDestroy)
                if (copyToDestroy[team].GetChildren().Count > 0)
                    return true;
           

        return false;
    }

    /// <summary>
    /// Function that returns a player source based on the team color
    /// </summary>
    /// <param name="team">The color of the team</param>
    /// <returns>The player source</returns>
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
    
    /// <summary>
    /// This class represent the tree type of the pipe. 
    /// The tree is built in the following way:
    ///     - One reference to the father;
    ///     - One reference to the first child;
    ///     - One reference to the brother;
    ///     - The actual value encapsuled by this node
    /// </summary>
    internal class RecursivePipe 
    {
        public RecursivePipe father { get; private set; }
        public Pipe current { get; private set; }
        public RecursivePipe firstChild { get; private set; }
        public RecursivePipe nextBrother { get; private set; }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="p">The tree type pipe to copy from</param>
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

        /// <summary>
        /// Constructor that builds a tree type pipe starting from the pipe and the father.
        /// Used by the copy constructor.
        /// </summary>
        /// <param name="p">The pipe to be copied</param>
        /// <param name="father">The father to be copied</param>
        private RecursivePipe(RecursivePipe p, RecursivePipe father)
        {
            this.father = father;
            firstChild = null;
            nextBrother = null;
            current = p.current;
            if (p.firstChild != null)
            {
                if(FindPipe(p.firstChild)==null)
                    firstChild = new RecursivePipe(p.firstChild,this);
            }
            if (p.nextBrother != null)
            {
                if (FindPipe(p.nextBrother) == null)
                    nextBrother = new RecursivePipe(p.nextBrother,father);
            }
        }

        /// <summary>
        /// Constructor that stores the passed pipe in the node
        /// </summary>
        /// <param name="pipe">The pipe to be stored</param>
        public RecursivePipe(Pipe pipe)
        {
            current = pipe;
            father = null;
            firstChild = null;
            nextBrother = null;
        }

        /// <summary>
        /// Simple constructor initializer.
        /// </summary>
        public RecursivePipe()
        {
            current = null;
            father = null;
            firstChild = null;
            nextBrother = null;
        }

        /// <summary>
        /// Destroys a pipe from a tree type pipe. Aka disconnect the pipe from the rest of the tree.
        /// </summary>
        public void Destroy()
        {
            DisconnectPipe();
        }

        /// <summary>
        /// Add a tree type pipe as child of the current tree type pipe.
        /// Takes care of the proper initialization of the references (father, nextbrother and firstchild)
        /// </summary>
        /// <param name="pipe">The child to be added</param>
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

        /// <summary>
        /// Function that returns the last brother of the current tree type pipe
        /// </summary>
        /// <returns>The last brother</returns>
        private RecursivePipe LastBrother()
        {
            if (nextBrother == null)
                return this;
            else
                return nextBrother.LastBrother();
        }

        /// <summary>
        /// Function that checks if the current tree type pipe is actually connected to the passed coordinate
        /// </summary>
        /// <param name="coordinate">The coordinate to check if the tree is connected</param>
        /// <returns>True or false</returns>
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

        /// <summary>
        /// Function called to remove a child from the current tree type pipe
        /// </summary>
        /// <param name="pipe">The pipe to remove from the children</param>
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

        /// <summary>
        /// Function that actually disconnects a pipe from the top layers(father), the bottom layer(firstchild) and the same layer(nextbrother)
        /// </summary>
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

        /// <summary>
        /// This function will NOT return the REAL root. This is because the real root contains a reference to 2 roots, one for each player source.
        /// This function will return the first tree type pipe that comes out from a player source starting from the current pipe
        /// </summary>
        /// <returns>The root</returns>
        public RecursivePipe GetRootPipe()
        {
            if (father.current == null)
                return this;
            return father.GetRootPipe();
        }

        /// <summary>
        /// This function is called when the player tries to add a pipe that involves multiple pipes coming out from different sources.
        /// It will EVENTUALLY set the child node as child of the EVENTUALLY found father based on the coordinates
        /// </summary>
        /// <param name="child">The child node to be added</param>
        /// <param name="fatherCoordinates">The eventual coordinates of the father</param>
        /// <returns>True if success, false otherwise</returns>
        public bool SearchAndAddAsParent(RecursivePipe child, GameData.Coordinate fatherCoordinates)
        {
            foreach (RecursivePipe p in PipeStatus.exploredPipes)
                if (p.current.positionCoordinate.Equals(current.positionCoordinate))
                    return false;
            exploredPipes.Add(this);
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

            if (father.current!=null&&nextBrother != null)
                if (nextBrother.SearchAndAddAsParent(child, fatherCoordinates))
                    return true;
            if (firstChild != null)
                if (firstChild.SearchAndAddAsParent(child, fatherCoordinates))
                    return true;

            return false;
        }


        /// <summary>
        /// This funciton is called every time that a player adds a pipe.
        /// It adds a pipe to a tree
        /// </summary>
        /// <param name="toAdd">The pipe to add</param>
        /// <param name="father">The father of the pipe to be added</param>
        /// <returns>The tree type pipe that will store the added pipe</returns>
        public RecursivePipe SearchAndAddChild(Pipe toAdd,Pipe father)
        {
            foreach (RecursivePipe p in PipeStatus.exploredPipes)
                if (p.current!=null&&p.current.positionCoordinate.Equals(current.positionCoordinate))
                    return null;
            exploredPipes.Add(this);
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

        /// <summary>
        /// This function will look for a passed pipe and eventually it will disconnect it from the tree.
        /// This means that all the subtree connected to that pipe will be disconnected from the main pipe
        /// </summary>
        /// <param name="pipe">The pipe to be disconnected</param>
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

        /// <summary>
        /// Function used to add a pipe as first child of the current tree type pipe
        /// </summary>
        /// <param name="toAdd">The pipe to add as first child</param>
        /// <returns>The tree type pipe that will store the pipe</returns>
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

        /// <summary>
        /// Function that will be called to destroy the leaves of the current tree
        /// </summary>
        /// <returns>Returns the list of tree that will be destroyed in the upper layer</returns>
        public List<RecursivePipe> DestroyLeaves()
        {
            List<RecursivePipe> l = new List<RecursivePipe>();
            if(firstChild==null)
            {
                if (current != null)
                {
                
                    l.Add(this);
                    if (nextBrother != null)
                        foreach (RecursivePipe r in nextBrother.DestroyLeaves())
                            l.Add(r);
                    DisconnectPipe();
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

        /// <summary>
        /// Function that removes a pipe from the children
        /// </summary>
        /// <param name="pipe">The pipe to be removed</param>
        private void RemovePipeFromChildren(Pipe pipe)
        {
            RemoveChild(new RecursivePipe(pipe));
        }

        /// <summary>
        /// Function that returns a list containing the children of the current node
        /// </summary>
        /// <returns>A list of tree type pipes that are the children</returns>
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

        /// <summary>
        /// Function called to destroy the pipe that is passed and generates a list of tree type pipe.
        /// Each one of these will contain a tree of neutral pipes
        /// </summary>
        /// <param name="pipe">The pipe to be destroyed</param>
        /// <param name="explosionEffect">The explosion effect to be instantiated</param>
        /// <returns>A list of tree type pipes containing the neutral pipes</returns>
        public List<RecursivePipe> DestroyPipe(Pipe pipe,GameObject explosionEffect,bool explosionActive)
        {
            if (current!=null&&current.positionCoordinate.Equals(pipe.positionCoordinate))
            {
                List<RecursivePipe> children = GetChildren();
                if(explosionActive)
                Instantiate(explosionEffect, current.gameObject.transform.position, Quaternion.identity);
                DisconnectPipe();
                pipe.DestroyPipe();
                return children;
            }
            else
                foreach (RecursivePipe p in GetChildren())
                {
                    List<RecursivePipe> neutral = p.DestroyPipe(pipe, explosionEffect,explosionActive);
                    if (neutral.Count > 0)
                        return neutral;
                }
                    
            return new List<RecursivePipe>();
        }

        /// <summary>
        /// Function called to reconnect a sub tree, given a pipe(the one that has recently been added), the father of the previous pipe
        /// and the known list of neutral pipes
        /// </summary>
        /// <param name="pipe">The pipe that has recently been added</param>
        /// <param name="father">The father of the previous pipe</param>
        /// <param name="neutralPipes">A list of the neutral known pipes</param>
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

        /// <summary>
        /// Function that returns the directly connected tree type pipes. The father and the children.
        /// </summary>
        /// <returns>Returns the father and the children of the current node</returns>
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

        /// <summary>
        /// Function that attempts to find a tree type pipe in the current tree type pipe
        /// </summary>
        /// <param name="p">The tree type pipe to be found</param>
        /// <returns>Returns the match if it finds it</returns>
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
        
        public bool HasValidChildren()
        {
            if (firstChild == null)
                return false;
            if (firstChild.current != null)
                return true;
            RecursivePipe p = firstChild;
            while (p.nextBrother != null)
            {
                if (p.current != null)
                    return true;
                p = p.nextBrother;
            }
                return false;
        }

        /// <summary>
        /// String representation of the node
        /// </summary>
        /// <returns>A representative string of the node</returns>
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
