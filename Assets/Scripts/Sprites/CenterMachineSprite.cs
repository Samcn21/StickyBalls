using UnityEngine;
using System.Collections;

public class CenterMachineSprite : AnimationController {
    public int spriteSheetColumns       = 4;
    public int spriteSheetRows          = 4;

    public GameData.CenterMachineStates currentAnim = GameData.CenterMachineStates.CenterMachineNeutral;

    //SPRITE FRAMES CENTER MACHINE
    private int cmNeutral       = 1;
    private int cmBlue          = 2;
    private int cmCyan          = 3;
    private int cmPurple        = 4;
    private int cmYellow        = 5;

    public float fpsCM          = 30;

    void Start() 
    {
        Settings();
    }

    void Settings()
    {
        rows = spriteSheetRows;
        columns = spriteSheetColumns;
        currentFrame = cmNeutral;
    }

    void Update()
    {
        Settings();
        FPSController();
        PlayAnimation();
        ReadSpriteSheet(GameData.SpriteSheet.CenterMachine);
    }

    void PlayAnimation() 
    {
        switch (currentAnim)
        {
            //character pipe grab animations
            case GameData.CenterMachineStates.CenterMachineBlue:
                fps = fpsCM;
                currentFrame = cmBlue;
                break;

            case GameData.CenterMachineStates.CenterMachineCyan:
                fps = fpsCM;
                currentFrame = cmCyan;
                break;

            case GameData.CenterMachineStates.CenterMachinePurple:
                fps = fpsCM;
                currentFrame = cmPurple;
                break;

            case GameData.CenterMachineStates.CenterMachineYellow:
                fps = fpsCM;
                currentFrame = cmYellow;
                break;
            default:
                fps = fpsCM;
                currentFrame = cmNeutral;
                break;
        }
    }

    public void FindCentralMachineStatus(GameData.Team color)
    {
        switch (color)
        {
            case GameData.Team.Blue:
                currentAnim = GameData.CenterMachineStates.CenterMachineBlue;
                break;

            case GameData.Team.Cyan:
                currentAnim = GameData.CenterMachineStates.CenterMachineCyan;
                break;

            case GameData.Team.Purple:
                currentAnim = GameData.CenterMachineStates.CenterMachinePurple;
                break;

            case GameData.Team.Yellow:
                currentAnim = GameData.CenterMachineStates.CenterMachineYellow;
                break;

            default:
                currentAnim = GameData.CenterMachineStates.CenterMachineNeutral;
                break;
        }
    }
}
