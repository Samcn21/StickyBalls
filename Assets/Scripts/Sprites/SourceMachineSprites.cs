using UnityEngine;
using System.Collections;

public class SourceMachineSprites : AnimationController
{
    public GameData.SourceMachineStates sourceMachine;
    public int spriteSheetColumns = 8;
    public int spriteSheetRows = 8;
    [SerializeField] int currentFrameShow = 1;


    //SPRITE FRAMES SOURCE MACHINE
    private int smCyan = 1;
    private int smYellow = 2;
    private int smPurple = 3;
    private int smBlue = 4;

    //State Machine
    private StateManager StateManager;
    private GameObject gameController;

    void Start()
    {
        rows = spriteSheetRows;
        columns = spriteSheetColumns;
        switch (sourceMachine)
        {
            case GameData.SourceMachineStates.SourceMachineBlue:
                currentFrame = smBlue;
                break;

            case GameData.SourceMachineStates.SourceMachineCyan:
                currentFrame = smCyan;
                break;

            case GameData.SourceMachineStates.SourceMachinePurple:
                currentFrame = smPurple;
                break;

            case GameData.SourceMachineStates.SourceMachineYellow:
                currentFrame = smYellow;
                break;
        }

        ReadSpriteSheet(GameData.SpriteSheet.SourceMachine);
        currentFrameShow = currentFrame;

    }
}
