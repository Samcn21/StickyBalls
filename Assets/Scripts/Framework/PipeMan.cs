using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Contains information needed for placing pipes
public class PipeMan : MonoBehaviour {

    [SerializeField] private Material voidPipeMat;
    [SerializeField] private Material cornerPipeMat;
    [SerializeField] private Material straightPipeMat;
    [SerializeField] private Material crossPipeMat;
    [SerializeField] private Material tPipeMat;
    [SerializeField] private Material dynamiteMat;

    [SerializeField] private Material placeholder_cornerPipeMat;
    [SerializeField] private Material placeholder_straightPipeMat;
    [SerializeField] private Material placeholder_crossPipeMat;
    [SerializeField] private Material placeholder_tPipeMat;
 
    public GameObject pipePrefab;
    public GameObject dynamitePrefab;
    public GameObject placeholderPrefab;
    public Dictionary<PipeData.PipeType, Material> pipeTextures;
    public Dictionary<PipeData.PipeType, Material> placeholderPipeTextures;
    public Dictionary<PipeData.PipeType, List<Vector2>> pipeConnections;  
    // Use this for initialization
    void Start () {

        pipeTextures = new Dictionary<PipeData.PipeType, Material>()
            {
            {PipeData.PipeType.Void, voidPipeMat},
            {PipeData.PipeType.Corner, cornerPipeMat},
            {PipeData.PipeType.Cross, crossPipeMat},
            {PipeData.PipeType.T, tPipeMat},
            {PipeData.PipeType.Straight, straightPipeMat},
            {PipeData.PipeType.Dynamite, dynamiteMat }
        };

        placeholderPipeTextures = new Dictionary<PipeData.PipeType, Material>()
        {
            {PipeData.PipeType.Corner, placeholder_cornerPipeMat},
            {PipeData.PipeType.Cross, placeholder_crossPipeMat},
            {PipeData.PipeType.T, placeholder_tPipeMat},
            {PipeData.PipeType.Straight, placeholder_straightPipeMat},
            {PipeData.PipeType.Dynamite, dynamiteMat }
        };

        pipeConnections = new Dictionary<PipeData.PipeType, List<Vector2>>()
        {
            {PipeData.PipeType.Corner, new List<Vector2>() {new Vector2(0,1), new Vector2(1,0) }},
            {PipeData.PipeType.Cross, new List<Vector2>() {new Vector2(0,1), new Vector2(1,0), new Vector2(0, -1), new Vector2(-1, 0) }},
            {PipeData.PipeType.Straight, new List<Vector2>() {new Vector2(0,1), new Vector2(0,-1) }},
            {PipeData.PipeType.T, new List<Vector2>() {new Vector2(-1,0), new Vector2(1,0), new Vector2(0,-1) }},
            {PipeData.PipeType.Dynamite, new List<Vector2>() { } }
        };
    }

}
