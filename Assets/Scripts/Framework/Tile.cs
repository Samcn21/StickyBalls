using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{

    public Pipe pipe { get; private set; }
    public bool locked;

    public void SetPipe(Pipe newPipe)
    {
        pipe = newPipe;
    }
}
