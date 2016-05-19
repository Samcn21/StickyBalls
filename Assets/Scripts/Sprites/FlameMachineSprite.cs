using UnityEngine;
using System.Collections;

public class FlameMachineSprite : AnimationController
{
    public GameData.SpriteSheet mySpriteSheet = GameData.SpriteSheet.Character;
    public int spriteSheetColumns = 2;
    public int spriteSheetRows = 2;

    public float fpsFlameMachine = 8;
    private int[] flameMachine = new int[2] { 1, 2 };

    public void Settings()
    {
        rows = spriteSheetRows;
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
        fps = fpsFlameMachine;
        LoopingAnimation(flameMachine);
    }

}
