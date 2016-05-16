using UnityEngine;
using System.Collections;

public class ControllerSprite : AnimationController
{
    private GameData.SpriteSheet mySpriteSheet = GameData.SpriteSheet.XboxController;
    public int spriteSheetColumns = 4;
    public int spriteSheetRows = 4;
    public GameData.ControllerStates currentAnim = GameData.ControllerStates.XboxController;

    public Material Controller;
    public Renderer rend;
    public float fpsBlinking        = 0.5f;

    private int xboxController      = 1;
    private int xboxLeftScroll    = 2;
    private int[] xboxA             = new int[2] { 3, 4 };
    private int[] xboxB             = new int[2] { 5, 6 };
    private int[] xboxStart         = new int[2] { 7, 8 };
    private int psController        = 9;
    private int psLeftScroll      = 10;
    private int[] psX               = new int[2] { 11, 12 };
    private int[] psCircle          = new int[2] { 13, 14 };
    private int[] psStart           = new int[2] { 15, 16 };

    void Start()
    {
        Settings();
    }

    void Settings()
    {
        currentFrame = 1;
        rows = spriteSheetRows;
        columns = spriteSheetColumns;
        fps = fpsBlinking;
    }

    void Update()
    {
        FPSController();
        PlayAnimation();
        ReadSpriteSheet(mySpriteSheet);
    }

    private void PlayAnimation()
    {
        switch (currentAnim)
        {
            case GameData.ControllerStates.XboxController:
                currentFrame = xboxController;
                break;

            case GameData.ControllerStates.XboxLeftScroll:
                currentFrame = xboxLeftScroll;
                break;

            case GameData.ControllerStates.XboxA:
                LoopingAnimation(xboxA);
                break;

            case GameData.ControllerStates.XboxB:
                LoopingAnimation(xboxB);
                break;

            case GameData.ControllerStates.XboxStart:
                LoopingAnimation(xboxStart);
                break;
        }

        switch (currentAnim) 
        {
            case GameData.ControllerStates.PsController:
                currentFrame = psController;
                break;

            case GameData.ControllerStates.PsLeftScroll:
                currentFrame = psLeftScroll;
                break;

            case GameData.ControllerStates.PsX:
                LoopingAnimation(psX);
                break;

            case GameData.ControllerStates.PsCircle:
                LoopingAnimation(psCircle);
                break;

            case GameData.ControllerStates.PsStart:
                LoopingAnimation(psStart);
                break;
        }
    }

}
