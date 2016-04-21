using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipeConnection : MonoBehaviour
{
    public Pipe ConnectedPipeFront;
    public Pipe ConnectedPipeBack;

    public Dictionary<GameData.Direction, Pipe> connection = new Dictionary<GameData.Direction, Pipe>();

    public void AddPipe(Pipe pipe, bool isFront)
    {
        if (isFront)
        {
            ConnectedPipeFront = pipe;
        }
        else
        {
            ConnectedPipeBack = pipe;
        }
    }

    void Update()
    {
        if (ConnectedPipeBack == null && ConnectedPipeFront == null)
        {
            Destroy(this);
        }
    }
}
