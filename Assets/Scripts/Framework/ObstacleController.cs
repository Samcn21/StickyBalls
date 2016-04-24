using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour
{
    public enum GenerationMode
    {
        Single,
        FourSides,
        SymmetryWidth,
        SymmetryHeight
    }

    public int[] obstacleWidth;
    public int[] obstacleHight;
    public GameObject[] obstacleType;
    public GenerationMode[] obstacleMode;
    
    void Start()
    {

    }

}
