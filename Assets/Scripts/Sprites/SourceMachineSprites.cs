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

    void Start()
    {
        GameData.Team team = GetComponentInParent<PlayerSource>().Team;
        rows = spriteSheetRows;
        columns = spriteSheetColumns;
        switch (team)
        {
            case GameData.Team.Blue:
                currentFrame = smBlue;
                break;

            case GameData.Team.Cyan:
                currentFrame = smCyan;
                break;

            case GameData.Team.Purple:
                currentFrame = smPurple;
                break;

            case GameData.Team.Yellow:
                currentFrame = smYellow;
                break;
        }

        ReadSpriteSheet(GameData.SpriteSheet.SourceMachine);
        currentFrameShow = currentFrame;

    }
}
