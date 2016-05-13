using UnityEngine;
using System.Collections;

public class ControllerSprite : AnimationController
{
    public GameData.SpriteSheet mySpriteSheet = GameData.SpriteSheet.XboxController;
    public int spriteSheetColumns = 2;
    public int spriteSheetRows = 4;
    public GameData.ControllerStates currentAnim = GameData.ControllerStates.XboxController;

    public Material XboxControllerMat;
    public Material PsControllerMat;
    public Renderer rend;

    private int controller          = 1;
    private int leftScroll          = 2;
    private int aorX                = 3;
    private int borCircle           = 4;
    private int start               = 5;

    void Start()
    {
        Settings();
    }

    void Settings()
    {
        rows = spriteSheetRows;
        columns = spriteSheetColumns;
    }

    void Update()
    {
        FPSController();
        PlayAnimation();
        ReadSpriteSheet(mySpriteSheet);
    }

    private void PlayAnimation()
    {

        //xboxController
        switch (mySpriteSheet)
        {
            case GameData.SpriteSheet.XboxController:
                rend.material = XboxControllerMat;

                switch (currentAnim)
                {
                    case GameData.ControllerStates.XboxController:
                        currentFrame = controller;
                        break;
                    case GameData.ControllerStates.XboxLeftScroll:
                        currentFrame = leftScroll;
                        break;
                    case GameData.ControllerStates.XboxA:
                        currentFrame = aorX;
                        break;
                    case GameData.ControllerStates.XboxB:
                        currentFrame = borCircle;
                        break;
                    case GameData.ControllerStates.XboxStart:
                        currentFrame = start;
                        break;

                }

                break;

            case GameData.SpriteSheet.PsController:
                rend.material = PsControllerMat;

                switch (currentAnim)
                {
                    case GameData.ControllerStates.PsController:
                        currentFrame = controller;
                        break;

                    case GameData.ControllerStates.PsLeftScroll:
                        currentFrame = leftScroll;
                        break;

                    case GameData.ControllerStates.PsX:
                        currentFrame = aorX;
                        break;

                    case GameData.ControllerStates.PsCircle:
                        currentFrame = borCircle;
                        break;

                    case GameData.ControllerStates.PsStart:
                        currentFrame = start;
                        break;

                }

                break;
        }
    }

}
