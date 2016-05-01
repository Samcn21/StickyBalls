using UnityEngine;
using System.Collections;

public class SpriteManager : MonoBehaviour {
    //SPRITE SHEET PROPERTIES
    public int columns = 2;
    public int rows = 2;
    public GameData.AnimationStates sourceMachine;
    private Vector2 framePosition;
    private Vector2 frameSize;
    private Vector2 frameOffset;
    private int iCount;
    [SerializeField] int currentFrame = 1;

    //SPRITE FRAMES 
    int smCyan = 1;
    int smYellow = 2;
    int smPurple = 3;
    int smBlue = 4;

	void Start () {
        switch (sourceMachine)
        {
            case GameData.AnimationStates.SourceMachineBlue:
                currentFrame = smBlue;
                break;

            case GameData.AnimationStates.SourceMachineCyan:
                currentFrame = smCyan;
                break;

            case GameData.AnimationStates.SourceMachinePurple:
                currentFrame = smPurple;
                break;
            case GameData.AnimationStates.SourceMachineYellow:
                currentFrame = smYellow;
                break;
        }

        framePosition.y = 1;
        for (iCount = currentFrame; iCount > columns; iCount -= rows)
        {
            framePosition.y += 1;
        }
        framePosition.x = iCount - 1;


        frameSize = new Vector2(1.0f / columns, 1.0f / rows);
        frameOffset = new Vector2(framePosition.x / columns, 1.0f - (framePosition.y / rows));

        GetComponent<Renderer>().material.SetTextureScale("_MainTex", frameSize);
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", frameOffset);	
	}
}
