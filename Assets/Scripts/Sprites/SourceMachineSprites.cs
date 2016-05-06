using UnityEngine;
using System.Collections;

public class SourceMachineSprites : AnimationController
{
    public GameData.SourceMachineStates sourceMachine;
    public int spriteSheetColumns = 2;
    public int spriteSheetRows = 2;
    [SerializeField] int currentFrameExpose = 1;

    //SPRITE FRAMES SOURCE MACHINE
    private int smCyan = 1;
    private int smYellow = 2;
    private int smPurple = 3;
    private int smBlue = 4;

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
        currentFrameExpose = currentFrame;

    }
}
