using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlameMachineSpawner : MonoBehaviour {

   public List<Vector2> coordinates;

    [SerializeField]
    private GameObject FlameMachinePrefab;

    void Start()
    {
        foreach(Vector2 pos in coordinates)
        {
            GameData.Coordinate c = new GameData.Coordinate((int)pos.x, (int)pos.y);
            GameObject g = Instantiate(FlameMachinePrefab);
            g.GetComponent<FlameMachine>().Instantiate(c);
        }
    }
}
