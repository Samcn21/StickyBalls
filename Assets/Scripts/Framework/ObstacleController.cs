using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour
{

    public enum GenerationMode
    {
        OneSide,
        SymmetryWidth,
        SymmetryHeight,
        FourSides
    }

    public Vector2[] obstaclePos;
    [SerializeField] GameObject obstacleType;

    public GenerationMode[] obstacleMode;

    
    void Start()
    {
        if (obstacleMode.Length == obstaclePos.Length)
        {
            Debug.Log("We can start generation");
            //TODO:
            //0. first step is to generate 1x1 block!!!
            //1. find x and y in a loop 
            //2. check if we in order to generation mode the obstacle will be in the board
            //3. find the generated tile in the board
            //4. copy found tile's position to initialized obstacle
            //4.5 generating obstacles around the central machine and sources must be impossible
            //5. check the mode and generate obstacle to related position
            //6. lock all the obstacle positions
        }
    }

}
