using UnityEngine;
using System.Collections;

public class ConveyorBeltSprite : AnimationController
{
    public GameData.SpriteSheet mySpriteSheet = GameData.SpriteSheet.ConveyorBelt;
    public int spriteSheetColumns   = 4;
    public int spriteSheetRows      = 4;

    public float fpsCB              = 8;

    private int[] straight          = new int[2] { 9, 10 };
    private int[] topRight          = new int[2] { 7, 8 };
    private int[] topLeft           = new int[2] { 5, 6 };
    private int[] bottomRight       = new int[2] { 1, 2 };
    private int[] bottomLeft        = new int[2] { 3, 4 };

    public GameData.ConveyorBeltStates currentAnim = GameData.ConveyorBeltStates.Straight;

    public void Settings()
    {
        rows    = spriteSheetRows;
        columns = spriteSheetColumns;
    }

    void Update()
    {
        Settings();
        FPSController();
        PlayAnimation();
        ReadSpriteSheet(mySpriteSheet);
    }

    void PlayAnimation()
    {
        fps = fpsCB;
        switch (currentAnim)
        {
            case GameData.ConveyorBeltStates.Straight:
                LoopingAnimation(straight);
                break;

            case GameData.ConveyorBeltStates.CornerBottomLeft:
                LoopingAnimation(bottomLeft);
                break;

            case GameData.ConveyorBeltStates.CornerBottomRight:
                LoopingAnimation(bottomRight);
                break;

            case GameData.ConveyorBeltStates.CornerTopLeft:
                LoopingAnimation(topLeft);
                break;

            case GameData.ConveyorBeltStates.CornerTopRight:
                LoopingAnimation(topRight);
                break;
        }
    }

}
