using UnityEngine;
using System.Collections;

public class PipesSprite : AnimationController
{
    public GameData.SpriteSheet mySpriteSheet = GameData.SpriteSheet.Pipe;
    public int spriteSheetColumns       = 8;
    public int spriteSheetRows          = 8;

    public GameData.PipesStates currentAnim;
    private string stateName = "";
    //private Pipe Pipe;
    public Material pipesMat;
    public Material pipesNeutralMat;
    public Renderer rend;

    public float fpsEmptyPipes = 1;
    public float fpsFullPipes = 12;

    //ANIMATION FRAMES FOR PIPES
    private int neutralEmptyCorner      = 1;
    private int neutralEmptyT           = 2;
    private int neutralEmptyStraight    = 3;
    private int neutralEmptyCross       = 4;

    private int blueEmptyCorner         = 5;
    private int blueEmptyT              = 9;
    private int blueEmptyStraight       = 13;
    private int blueEmptyCross          = 17;

    private int cyanEmptyCorner         = 8;
    private int cyanEmptyT              = 12;
    private int cyanEmptyStraight       = 16;
    private int cyanEmptyCross          = 20;

    private int purpleEmptyCorner       = 7;
    private int purpleEmptyT            = 11;
    private int purpleEmptyStraight     = 15;
    private int purpleEmptyCross        = 19;

    private int yellowEmptyCorner       = 6;
    private int yellowEmptyT            = 10;
    private int yellowEmptyStraight     = 14;
    private int yellowEmptyCross        = 18;

    private int[] blueFullCorner        = new int[2] { 21, 22 };
    private int[] blueFullT             = new int[3] { 29, 30, 31 };
    private int[] blueFullStraight      = new int[3] { 41, 42, 43 };
    private int[] blueFullCross         = new int[3] { 53, 54, 55 };

    private int[] cyanFullCorner        = new int[2] { 27, 28 };
    private int[] cyanFullT             = new int[3] { 38, 39, 40 };
    private int[] cyanFullStraight      = new int[3] { 50, 51, 52 };
    private int[] cyanFullCross         = new int[3] { 62, 63, 64 };

    private int[] purpleFullCorner      = new int[2] { 25, 26 };
    private int[] purpleFullT           = new int[3] { 35, 36, 37 };
    private int[] purpleFullStraight    = new int[3] { 47, 48, 49 };
    private int[] purpleFullCross       = new int[3] { 59, 60, 61 };

    private int[] yellowFullCorner      = new int[2] { 23, 24 };
    private int[] yellowFullT           = new int[3] { 32, 33, 34 };
    private int[] yellowFullStraight    = new int[3] { 44, 45, 46 };
    private int[] yellowFullCross       = new int[3] { 56, 57, 58 };


	void Start () {
        Settings();
        rend = GetComponent<Renderer>();
        //if (Pipe != null)
        //{
            mySpriteSheet = GameData.SpriteSheet.Pipe;
            rend.material = pipesMat;
        //}
	}

    void Settings() 
    {
        rows = spriteSheetRows;
        columns = spriteSheetColumns;
    }
	
	void Update () {
        Settings();
        FPSController();
        PlayAnimation();
        ReadSpriteSheet(mySpriteSheet);
	}

    void PlayAnimation() 
    {
        switch (currentAnim)
        {
            //empty pipes animations
            //nutreal empty 
            case GameData.PipesStates.PipeNeutralEmptyCorner:
                fps = fpsEmptyPipes;
                currentFrame = neutralEmptyCorner;
                break;

            case GameData.PipesStates.PipeNeutralEmptyCross:
                fps = fpsEmptyPipes;
                currentFrame = neutralEmptyCross;
                break;

            case GameData.PipesStates.PipeNeutralEmptyT:
                fps = fpsEmptyPipes;
                currentFrame = neutralEmptyT;
                break;

            case GameData.PipesStates.PipeNeutralEmptyStraight:
                fps = fpsEmptyPipes;
                currentFrame = neutralEmptyStraight;
                break;

            //blue empty 
            case GameData.PipesStates.PipeBlueEmptyCorner:
                fps = fpsEmptyPipes;
                currentFrame = blueEmptyCorner;
                break;

            case GameData.PipesStates.PipeBlueEmptyCross:
                fps = fpsEmptyPipes;
                currentFrame = blueEmptyCross;
                break;

            case GameData.PipesStates.PipeBlueEmptyT:
                fps = fpsEmptyPipes;
                currentFrame = blueEmptyT;
                break;

            case GameData.PipesStates.PipeBlueEmptyStraight:
                fps = fpsEmptyPipes;
                currentFrame = blueEmptyStraight;
                break;

            //cyan empty 
            case GameData.PipesStates.PipeCyanEmptyCorner:
                fps = fpsEmptyPipes;
                currentFrame = cyanEmptyCorner;
                break;

            case GameData.PipesStates.PipeCyanEmptyCross:
                fps = fpsEmptyPipes;
                currentFrame = cyanEmptyCross;
                break;

            case GameData.PipesStates.PipeCyanEmptyT:
                fps = fpsEmptyPipes;
                currentFrame = cyanEmptyT;
                break;

            case GameData.PipesStates.PipeCyanEmptyStraight:
                fps = fpsEmptyPipes;
                currentFrame = cyanEmptyStraight;
                break;

            //purple empty
            case GameData.PipesStates.PipePurpleEmptyCorner:
                fps = fpsEmptyPipes;
                currentFrame = purpleEmptyCorner;
                break;

            case GameData.PipesStates.PipePurpleEmptyCross:
                fps = fpsEmptyPipes;
                currentFrame = purpleEmptyCross;
                break;

            case GameData.PipesStates.PipePurpleEmptyT:
                fps = fpsEmptyPipes;
                currentFrame = purpleEmptyT;
                break;

            case GameData.PipesStates.PipePurpleEmptyStraight:
                fps = fpsEmptyPipes;
                currentFrame = purpleEmptyStraight;
                break;

            //yellow empty
            case GameData.PipesStates.PipeYellowEmptyCorner:
                fps = fpsEmptyPipes;
                currentFrame = yellowEmptyCorner;
                break;

            case GameData.PipesStates.PipeYellowEmptyCross:
                fps = fpsEmptyPipes;
                currentFrame = yellowEmptyCross;
                break;

            case GameData.PipesStates.PipeYellowEmptyT:
                fps = fpsEmptyPipes;
                currentFrame = yellowEmptyT;
                break;

            case GameData.PipesStates.PipeYellowEmptyStraight:
                fps = fpsEmptyPipes;
                currentFrame = yellowEmptyStraight;
                break;


            //full pipes animations
            //full blue
            case GameData.PipesStates.PipeBlueFullCorner:
                fps = fpsFullPipes;
                LoopingAnimation(blueFullCorner);
                break;

            case GameData.PipesStates.PipeBlueFullCross:
                fps = fpsFullPipes;
                LoopingAnimation(blueFullCross);
                break;

            case GameData.PipesStates.PipeBlueFullT:
                fps = fpsFullPipes;
                LoopingAnimation(blueFullT);
                break;

            case GameData.PipesStates.PipeBlueFullStraight:
                fps = fpsFullPipes;
                LoopingAnimation(blueFullStraight);
                break;

            //full cyan
            case GameData.PipesStates.PipeCyanFullCorner:
                fps = fpsFullPipes;
                LoopingAnimation(cyanFullCorner);
                break;

            case GameData.PipesStates.PipeCyanFullCross:
                fps = fpsFullPipes;
                LoopingAnimation(cyanFullCross);
                break;

            case GameData.PipesStates.PipeCyanFullT:
                fps = fpsFullPipes;
                LoopingAnimation(cyanFullT);
                break;

            case GameData.PipesStates.PipeCyanFullStraight:
                fps = fpsFullPipes;
                LoopingAnimation(cyanFullStraight);
                break;

            //full purple
            case GameData.PipesStates.PipePurpleFullCorner:
                fps = fpsFullPipes;
                LoopingAnimation(purpleFullCorner);
                break;

            case GameData.PipesStates.PipePurpleFullCross:
                fps = fpsFullPipes;
                LoopingAnimation(purpleFullCross);
                break;

            case GameData.PipesStates.PipePurpleFullT:
                fps = fpsFullPipes;
                LoopingAnimation(purpleFullT);
                break;

            case GameData.PipesStates.PipePurpleFullStraight:
                fps = fpsFullPipes;
                LoopingAnimation(purpleFullStraight);
                break;

            //full yellow
            case GameData.PipesStates.PipeYellowFullCorner:
                fps = fpsFullPipes;
                LoopingAnimation(yellowFullCorner);
                break;

            case GameData.PipesStates.PipeYellowFullCross:
                fps = fpsFullPipes;
                LoopingAnimation(yellowFullCross);
                break;

            case GameData.PipesStates.PipeYellowFullT:
                fps = fpsFullPipes;
                LoopingAnimation(yellowFullT);
                break;

            case GameData.PipesStates.PipeYellowFullStraight:
                fps = fpsFullPipes;
                LoopingAnimation(yellowFullStraight);
                break;

        }

    }

    public void FindWinnerPipes(GameData.Team color)
    {
        if (currentAnim.ToString().Contains(color.ToString()))
        {
            if (currentAnim.ToString().Contains("Corner"))
            {
                stateName = "Pipe" + color + "Full" + "Corner";
                currentAnim = (GameData.PipesStates)GameData.PipesStates.Parse(typeof(GameData.PipesStates), stateName, true);
            }

            else if (currentAnim.ToString().Contains("T"))
            {
                stateName = "Pipe" + color + "Full" + "T";
                currentAnim = (GameData.PipesStates)GameData.PipesStates.Parse(typeof(GameData.PipesStates), stateName, true);
            }

            else if (currentAnim.ToString().Contains("Straight"))
            {
                stateName = "Pipe" + color + "Full" + "Straight";
                currentAnim = (GameData.PipesStates)GameData.PipesStates.Parse(typeof(GameData.PipesStates), stateName, true);
            }

            else if (currentAnim.ToString().Contains("Cross"))
            {

                stateName = "Pipe" + color + "Full" + "Cross";
                currentAnim = (GameData.PipesStates)GameData.PipesStates.Parse(typeof(GameData.PipesStates), stateName, true);
            }
        }
    }

    public void FindPipeStatus(PipeData.PipeType pipeType, GameData.Team color)
    {
        switch (pipeType)
        {
            case PipeData.PipeType.Corner:
                switch (color)
                {

                    case GameData.Team.Blue:
                        currentAnim = GameData.PipesStates.PipeBlueEmptyCorner;
                        break;

                    case GameData.Team.Cyan:
                        currentAnim = GameData.PipesStates.PipeCyanEmptyCorner;
                        break;

                    case GameData.Team.Purple:
                        currentAnim = GameData.PipesStates.PipePurpleEmptyCorner;
                        break;

                    case GameData.Team.Yellow:
                        currentAnim = GameData.PipesStates.PipeYellowEmptyCorner;
                        break;

                    default:
                        currentAnim = GameData.PipesStates.PipeNeutralEmptyCorner;
                        break;
                }
                break;

            case PipeData.PipeType.Straight:
                switch (color)
                {
                    case GameData.Team.Blue:
                        currentAnim = GameData.PipesStates.PipeBlueEmptyStraight;
                        break;

                    case GameData.Team.Cyan:
                        currentAnim = GameData.PipesStates.PipeCyanEmptyStraight;
                        break;

                    case GameData.Team.Purple:
                        currentAnim = GameData.PipesStates.PipePurpleEmptyStraight;
                        break;

                    case GameData.Team.Yellow:
                        currentAnim = GameData.PipesStates.PipeYellowEmptyStraight;
                        break;

                    default:
                        currentAnim = GameData.PipesStates.PipeNeutralEmptyStraight;
                        break;
                }
                break;

            case PipeData.PipeType.Cross:
                switch (color)
                {
                    case GameData.Team.Blue:
                        currentAnim = GameData.PipesStates.PipeBlueEmptyCross;
                        break;

                    case GameData.Team.Cyan:
                        currentAnim = GameData.PipesStates.PipeCyanEmptyCross;
                        break;

                    case GameData.Team.Purple:
                        currentAnim = GameData.PipesStates.PipePurpleEmptyCross;
                        break;

                    case GameData.Team.Yellow:
                        currentAnim = GameData.PipesStates.PipeYellowEmptyCross;
                        break;

                    default:
                        currentAnim = GameData.PipesStates.PipeNeutralEmptyCross;
                        break;
                }
                break;

            case PipeData.PipeType.T:
                switch (color)
                {
                    case GameData.Team.Blue:
                        currentAnim = GameData.PipesStates.PipeBlueEmptyT;
                        break;

                    case GameData.Team.Cyan:
                        currentAnim = GameData.PipesStates.PipeCyanEmptyT;
                        break;

                    case GameData.Team.Purple:
                        currentAnim = GameData.PipesStates.PipePurpleEmptyT;
                        break;

                    case GameData.Team.Yellow:
                        currentAnim = GameData.PipesStates.PipeYellowEmptyT;
                        break;

                    default:
                        currentAnim = GameData.PipesStates.PipeNeutralEmptyT;
                        break;
                }
                break;

        }
    }
}
