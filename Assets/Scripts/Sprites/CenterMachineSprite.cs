using UnityEngine;
using System.Collections;

public class CenterMachineSprite : AnimationController {
    public int spriteSheetColumns       = 4;
    public int spriteSheetRows          = 4;

    public GameData.CenterMachineStates currentAnim = GameData.CenterMachineStates.CenterMachineNeutral;

    //SPRITE FRAMES CENTER MACHINE
    private int[] cmNeutral       = new int[8] { 1, 2, 3 , 4, 5, 6, 7, 8};
    private int[] cmBlue          = new int[4] { 17, 18, 19, 20 };
    private int[] cmCyan          = new int[4] { 13, 14, 15, 16 };
    private int[] cmPurple        = new int[4] { 21, 22, 23, 24 };
    private int[] cmYellow        = new int[4] { 9 , 10 , 11, 12};

    public float fpsCMNeutral     = 12;
    public float fpsCMColor       = 8;

    void Start() 
    {
        Settings();
    }

    void Settings()
    {
        rows = spriteSheetRows;
        columns = spriteSheetColumns;

        fps = fpsCMNeutral;
        LoopingAnimation(cmNeutral);
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
                fps = fpsCMColor;
                LoopingAnimation(cmBlue);
                break;

            case GameData.CenterMachineStates.CenterMachineCyan:
                fps = fpsCMColor;
                LoopingAnimation(cmCyan);
                break;

            case GameData.CenterMachineStates.CenterMachinePurple:
                fps = fpsCMColor;
                LoopingAnimation(cmPurple);
                break;

            case GameData.CenterMachineStates.CenterMachineYellow:
                fps = fpsCMColor;
                LoopingAnimation(cmYellow);
                break;
            default:
                fps = fpsCMNeutral;
                LoopingAnimation(cmNeutral);
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
