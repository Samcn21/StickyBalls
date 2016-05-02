using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour
{
    //SPRITE SHEET PROPERTIES
    public int columns = 8;
    public int rows = 8;
    private Vector2 framePosition;
    private Vector2 frameSize;
    private Vector2 frameOffset;

    //ANIMATION CONTROL VARIABLES
    public int currentFrame = 1;
    public float animTime = 0.0f;
    [SerializeField] private float fps = 10.0f;
    public float fpsIdle = 4;
    public float fpsMovement = 15;
    public float fpsPipeGrab = 6;
    public float fpsPipePlace = 6;
    public float fpsEmptyPipes = 1;
    public float fpsFullPipes = 12;


    //SPRITE SHEETS
    [SerializeField] GameData.SpriteSheet mySpriteSheet;
    public Material pipesMat;
    public Material charBlueMat;
    public Material charCyanMat;
    public Material charPurpleMat;
    public Material charYellowMat;
    public Material charNeutralMat;
    public Renderer rend;

    public GameData.AnimationStates currentAnim = GameData.AnimationStates.IdleFront;
    public GameData.AnimationStates previousAnim = GameData.AnimationStates.IdleFront;
    public bool hasMovementPermit = true;
    private string StateName = "";
    private int iCount;
    private Pipe Pipe;
    private InputController InputController;


    //ANIMATION FRAMES FOR CHARACTERS
    private int[] idleFront         =           new int[3] { 1, 2, 3 };
    private int[] idleBack          =           new int[3] { 25, 26, 27 };
    private int[] idleRight         =           new int[3] { 9, 10, 11 };
    private int[] idleLeft          =           new int[3] { 17, 18, 19 };
    private int[] movementFront     =           new int[5] { 4, 5, 6, 7, 8 };
    private int[] movementBack      =           new int[5] { 28, 29, 30, 31, 32 };
    private int[] movementRight     =           new int[5] { 12, 13, 14, 15, 16 };
    private int[] movementLeft      =           new int[5] { 20, 21, 22, 23, 24 };
    private int[] pipeGrabFront     =           new int[2] { 33, 34 };
    private int[] pipeGrabBack      =           new int[2] { 39, 40 };
    private int[] pipeGrabRight     =           new int[2] { 35, 36 };
    private int[] pipeGrabLeft      =           new int[2] { 37, 38 };
    private int[] pipePlaceFront    =           new int[2] { 41, 42 };
    private int[] pipePlaceBack     =           new int[2] { 47, 48 };
    private int[] pipePlaceRight    =           new int[2] { 43, 44 };
    private int[] pipePlaceLeft     =           new int[2] { 45, 46 };


    //ANIMATION FRAMES FOR PIPES
    private int neutralEmptyCorner          =                         1;
    private int neutralEmptyT               =                         2;
    private int neutralEmptyStraight        =                         3;
    private int neutralEmptyCross           =                         4;

    private int blueEmptyCorner             =                         5;
    private int blueEmptyT                  =                         9;
    private int blueEmptyStraight           =                        13;
    private int blueEmptyCross              =                        17;

    private int cyanEmptyCorner             =                         8;
    private int cyanEmptyT                  =                        12;
    private int cyanEmptyStraight           =                        16;
    private int cyanEmptyCross              =                        20;

    private int purpleEmptyCorner          =                         7;
    private int purpleEmptyT               =                        11;
    private int purpleEmptyStraight        =                        15;
    private int purpleEmptyCross           =                        19;

    private int yellowEmptyCorner           =                         6;
    private int yellowEmptyT                =                        10;
    private int yellowEmptyStraight         =                        14;
    private int yellowEmptyCross            =                        18;

    private int[] blueFullCorner            =   new int[2] {  21, 22 };
    private int[] blueFullT                 =   new int[3] { 29, 30, 31 };
    private int[] blueFullStraight          =   new int[3] { 41, 42, 43 };
    private int[] blueFullCross             =   new int[3] { 53, 54, 55 };

    private int[] cyanFullCorner            =   new int[2] { 27, 28 };
    private int[] cyanFullT                 =   new int[3] { 38, 39, 40 };
    private int[] cyanFullStraight          =   new int[3] { 50, 51, 52 };
    private int[] cyanFullCross             =   new int[3] { 62, 63, 64 };

    private int[] purpleFullCorner         =   new int[2] { 25, 26 };
    private int[] purpleFullT              =   new int[3] { 35, 36, 37 };
    private int[] purpleFullStraight       =   new int[3] { 47, 48, 49 };
    private int[] purpleFullCross          =   new int[3] { 59, 60, 61 };

    private int[] yellowFullCorner          =   new int[2] { 23, 24 };
    private int[] yellowFullT               =   new int[3] { 32, 33, 34 };
    private int[] yellowFullStraight        =   new int[3] { 44, 45, 46 };
    private int[] yellowFullCross           =   new int[3] { 56, 57, 58 };

    void Start() {
        Pipe = GetComponent<Pipe>();
        //InputController = transform.parent.GetComponent<InputController>();
        rend = GetComponent<Renderer>();

        if (Pipe != null){
            mySpriteSheet = GameData.SpriteSheet.Pipe;
            rend.material = pipesMat;
        }

        else if (InputController != null)
        {
            mySpriteSheet = GameData.SpriteSheet.Character;
            switch (InputController.team)
            { 
                case GameData.Team.Blue:
                    rend.material = charBlueMat;
                    break;

                case GameData.Team.Cyan:
                    rend.material = charCyanMat;
                    break;

                case GameData.Team.Purple:
                    rend.material = charPurpleMat;
                    break;

                case GameData.Team.Yellow:
                    rend.material = charYellowMat;
                    break;

                case GameData.Team.Neutral:
                    rend.material = charNeutralMat;
                    break;

            }
        }

    }
    void Update()
    {
        PlayAnimation();
        ReadSpriteSheet(mySpriteSheet);
    }

    public void FindWinnerPipes(GameData.Team color)
    {
        if (currentAnim.ToString().Contains(color.ToString()))
        {
            if (currentAnim.ToString().Contains("Corner")) 
            {
                StateName = "Pipe" + color + "Full" + "Corner";
                currentAnim = (GameData.AnimationStates)GameData.AnimationStates.Parse(typeof(GameData.AnimationStates), StateName, true);
            }

            else if (currentAnim.ToString().Contains("T"))
            {
                StateName = "Pipe" + color + "Full" + "T";
                currentAnim = (GameData.AnimationStates)GameData.AnimationStates.Parse(typeof(GameData.AnimationStates), StateName, true);
            }

            else if (currentAnim.ToString().Contains("Straight"))
            {
                StateName = "Pipe" + color + "Full" + "Straight";
                currentAnim = (GameData.AnimationStates)GameData.AnimationStates.Parse(typeof(GameData.AnimationStates), StateName, true);
            }

            else if (currentAnim.ToString().Contains("Cross"))
            {

                StateName = "Pipe" + color + "Full" + "Cross";
                currentAnim = (GameData.AnimationStates)GameData.AnimationStates.Parse(typeof(GameData.AnimationStates), StateName, true);
            }
        }
    }

    public void FindPipeStatus(PipeData.PipeType pipeType, GameData.Team color) 
    {
        switch(pipeType)
        {
            case PipeData.PipeType.Corner:
                switch (color) 
                { 
                    case GameData.Team.Blue:
                        currentAnim = GameData.AnimationStates.PipeBlueEmptyCorner;
                        break;

                    case GameData.Team.Cyan:
                        currentAnim = GameData.AnimationStates.PipeCyanEmptyCorner;
                        break;

                    case GameData.Team.Purple:
                        currentAnim = GameData.AnimationStates.PipePurpleEmptyCorner;
                        break;

                    case GameData.Team.Yellow:
                        currentAnim = GameData.AnimationStates.PipeYellowEmptyCorner;
                        break;
                }
                break;

            case PipeData.PipeType.Straight:
                switch (color)
                {
                    case GameData.Team.Blue:
                        currentAnim = GameData.AnimationStates.PipeBlueEmptyStraight;
                        break;

                    case GameData.Team.Cyan:
                        currentAnim = GameData.AnimationStates.PipeCyanEmptyStraight;
                        break;

                    case GameData.Team.Purple:
                        currentAnim = GameData.AnimationStates.PipePurpleEmptyStraight;
                        break;

                    case GameData.Team.Yellow:
                        currentAnim = GameData.AnimationStates.PipeYellowEmptyStraight;
                        break;
                }
                break;

            case PipeData.PipeType.Cross:
                switch (color)
                {
                    case GameData.Team.Blue:
                        currentAnim = GameData.AnimationStates.PipeBlueEmptyCross;
                        break;

                    case GameData.Team.Cyan:
                        currentAnim = GameData.AnimationStates.PipeCyanEmptyCross;
                        break;

                    case GameData.Team.Purple:
                        currentAnim = GameData.AnimationStates.PipePurpleEmptyCross;
                        break;

                    case GameData.Team.Yellow:
                        currentAnim = GameData.AnimationStates.PipeYellowEmptyCross;
                        break;
                }
                break;

            case PipeData.PipeType.T:
                switch (color)
                {
                    case GameData.Team.Blue:
                        currentAnim = GameData.AnimationStates.PipeBlueEmptyT;
                        break;

                    case GameData.Team.Cyan:
                        currentAnim = GameData.AnimationStates.PipeCyanEmptyT;
                        break;

                    case GameData.Team.Purple:
                        currentAnim = GameData.AnimationStates.PipePurpleEmptyT;
                        break;

                    case GameData.Team.Yellow:
                        currentAnim = GameData.AnimationStates.PipeYellowEmptyT;
                        break;
                }
                break;

        }
    }

    public void FindGrabPipeAnimation()
    {
        hasMovementPermit = false;
        previousAnim = currentAnim;
        if (previousAnim.ToString().Contains("Front"))
        {
            currentAnim = GameData.AnimationStates.PipeGrabFront;
        }
        else if (previousAnim.ToString().Contains("Right"))
        {
            currentAnim = GameData.AnimationStates.PipeGrabRight;
        }
        else if (previousAnim.ToString().Contains("Left"))
        {
            currentAnim = GameData.AnimationStates.PipeGrabLeft;
        }
        else if (previousAnim.ToString().Contains("Back"))
        {
            currentAnim = GameData.AnimationStates.PipeGrabBack;
        }
    }

    
    public void FindPlacePipeAnimation()
    {
        hasMovementPermit = false;
        previousAnim = currentAnim;
        if (previousAnim.ToString().Contains("Front"))
        {
            currentAnim = GameData.AnimationStates.PipePlaceFront;
        }
        else if (previousAnim.ToString().Contains("Right"))
        {
            currentAnim = GameData.AnimationStates.PipePlaceRight;
        }
        else if (previousAnim.ToString().Contains("Left"))
        {
            currentAnim = GameData.AnimationStates.PipePlaceLeft;
        }
        else if (previousAnim.ToString().Contains("Back"))
        {
            currentAnim = GameData.AnimationStates.PipePlaceBack;
        }
    }

    void PlayAnimation()
    {
        animTime -= Time.deltaTime;
        if (animTime <= 0)
        {
            currentFrame += 1;
            animTime += 1.0f / fps;
        }

        switch (currentAnim)
        {
            //character pipe grab animations
            case GameData.AnimationStates.PipeGrabFront:
                fps = fpsPipeGrab;
                CharacterOneShotAnimation(pipeGrabFront);
                break;

            case GameData.AnimationStates.PipeGrabBack:
                fps = fpsPipeGrab;
                CharacterOneShotAnimation(pipeGrabBack);
                break;

            case GameData.AnimationStates.PipeGrabLeft:
                fps = fpsPipeGrab;
                CharacterOneShotAnimation(pipeGrabLeft);
                break;

            case GameData.AnimationStates.PipeGrabRight:
                fps = fpsPipeGrab;
                CharacterOneShotAnimation(pipeGrabRight);
                break;

            //character pipe place animations
            case GameData.AnimationStates.PipePlaceFront:
                fps = fpsPipePlace;
                CharacterOneShotAnimation(pipePlaceFront);
                break;

            case GameData.AnimationStates.PipePlaceBack:
                fps = fpsPipePlace;
                CharacterOneShotAnimation(pipePlaceBack);
                break;

            case GameData.AnimationStates.PipePlaceLeft:
                fps = fpsPipePlace;
                CharacterOneShotAnimation(pipePlaceLeft);
                break;

            case GameData.AnimationStates.PipePlaceRight:
                fps = fpsPipePlace;
                CharacterOneShotAnimation(pipePlaceRight);
                break;

            //character idle animations
            case GameData.AnimationStates.IdleFront:
                fps = fpsIdle;
                LoopingAnimation(idleFront);
                break;

            case GameData.AnimationStates.IdleBack:
                fps = fpsIdle;
                LoopingAnimation(idleBack);
                break;

            case GameData.AnimationStates.IdleRight:
                fps = fpsIdle;
                LoopingAnimation(idleRight);
                break;

            case GameData.AnimationStates.IdleLeft:
                fps = fpsIdle;
                LoopingAnimation(idleLeft);
                break;

            //character movemnt animations
            case GameData.AnimationStates.MovementFront:
                fps = fpsMovement;
                LoopingAnimation(movementFront);
                break;

            case GameData.AnimationStates.MovementBack:
                fps = fpsMovement;
                LoopingAnimation(movementBack);
                break;

            case GameData.AnimationStates.MovementRight:
                fps = fpsMovement;
                LoopingAnimation(movementRight);
                break;

            case GameData.AnimationStates.MovementLeft:
                fps = fpsMovement;
                LoopingAnimation(movementLeft);
                break;

            //empty pipes animations
            //nutreal empty 
            case GameData.AnimationStates.PipeNeutralEmptyCorner:
                fps = fpsEmptyPipes;
                currentFrame = neutralEmptyCorner;
                break;

            case GameData.AnimationStates.PipeNeutralEmptyCross:
                fps = fpsEmptyPipes;
                currentFrame = neutralEmptyCross;
                break;

            case GameData.AnimationStates.PipeNeutralEmptyT:
                fps = fpsEmptyPipes;
                currentFrame = neutralEmptyT;
                break;

            case GameData.AnimationStates.PipeNeutralEmptyStraight:
                fps = fpsEmptyPipes;
                currentFrame = neutralEmptyStraight;
                break;

            //blue empty 
            case GameData.AnimationStates.PipeBlueEmptyCorner:
                fps = fpsEmptyPipes;
                currentFrame = blueEmptyCorner;
                break;

            case GameData.AnimationStates.PipeBlueEmptyCross:
                fps = fpsEmptyPipes;
                currentFrame = blueEmptyCross;
                break;

            case GameData.AnimationStates.PipeBlueEmptyT:
                fps = fpsEmptyPipes;
                currentFrame = blueEmptyT;
                break;

            case GameData.AnimationStates.PipeBlueEmptyStraight:
                fps = fpsEmptyPipes;
                currentFrame = blueEmptyStraight;
                break;

            //cyan empty 
            case GameData.AnimationStates.PipeCyanEmptyCorner:
                fps = fpsEmptyPipes;
                currentFrame = cyanEmptyCorner;
                break;

            case GameData.AnimationStates.PipeCyanEmptyCross:
                fps = fpsEmptyPipes;
                currentFrame = cyanEmptyCross;
                break;

            case GameData.AnimationStates.PipeCyanEmptyT:
                fps = fpsEmptyPipes;
                currentFrame = cyanEmptyT;
                break;

            case GameData.AnimationStates.PipeCyanEmptyStraight:
                fps = fpsEmptyPipes;
                currentFrame = cyanEmptyStraight;
                break;

            //purple empty
            case GameData.AnimationStates.PipePurpleEmptyCorner:
                fps = fpsEmptyPipes;
                currentFrame = purpleEmptyCorner;
                break;

            case GameData.AnimationStates.PipePurpleEmptyCross:
                fps = fpsEmptyPipes;
                currentFrame = purpleEmptyCross;
                break;

            case GameData.AnimationStates.PipePurpleEmptyT:
                fps = fpsEmptyPipes;
                currentFrame = purpleEmptyT;
                break;

            case GameData.AnimationStates.PipePurpleEmptyStraight:
                fps = fpsEmptyPipes;
                currentFrame = purpleEmptyStraight;
                break;

            //yellow empty
            case GameData.AnimationStates.PipeYellowEmptyCorner:
                fps = fpsEmptyPipes;
                currentFrame = yellowEmptyCorner;
                break;

            case GameData.AnimationStates.PipeYellowEmptyCross:
                fps = fpsEmptyPipes;
                currentFrame = yellowEmptyCross;
                break;

            case GameData.AnimationStates.PipeYellowEmptyT:
                fps = fpsEmptyPipes;
                currentFrame = yellowEmptyT;
                break;

            case GameData.AnimationStates.PipeYellowEmptyStraight:
                fps = fpsEmptyPipes;
                currentFrame = yellowEmptyStraight;
                break;


            //full pipes animations
            //full blue
            case GameData.AnimationStates.PipeBlueFullCorner:
                fps = fpsFullPipes;
                LoopingAnimation(blueFullCorner);
                break;

            case GameData.AnimationStates.PipeBlueFullCross:
                fps = fpsFullPipes;
                LoopingAnimation(blueFullCross);
                break;

            case GameData.AnimationStates.PipeBlueFullT:
                fps = fpsFullPipes;
                LoopingAnimation(blueFullT);
                break;

            case GameData.AnimationStates.PipeBlueFullStraight:
                fps = fpsFullPipes;
                LoopingAnimation(blueFullStraight);
                break;

            //full cyan
            case GameData.AnimationStates.PipeCyanFullCorner:
                fps = fpsFullPipes;
                LoopingAnimation(cyanFullCorner);
                break;

            case GameData.AnimationStates.PipeCyanFullCross:
                fps = fpsFullPipes;
                LoopingAnimation(cyanFullCross);
                break;

            case GameData.AnimationStates.PipeCyanFullT:
                fps = fpsFullPipes;
                LoopingAnimation(cyanFullT);
                break;

            case GameData.AnimationStates.PipeCyanFullStraight:
                fps = fpsFullPipes;
                LoopingAnimation(cyanFullStraight);
                break;

            //full purple
            case GameData.AnimationStates.PipePurpleFullCorner:
                fps = fpsFullPipes;
                LoopingAnimation(purpleFullCorner);
                break;

            case GameData.AnimationStates.PipePurpleFullCross:
                fps = fpsFullPipes;
                LoopingAnimation(purpleFullCross);
                break;

            case GameData.AnimationStates.PipePurpleFullT:
                fps = fpsFullPipes;
                LoopingAnimation(purpleFullT);
                break;

            case GameData.AnimationStates.PipePurpleFullStraight:
                fps = fpsFullPipes;
                LoopingAnimation(purpleFullStraight);
                break;

            //full yellow
            case GameData.AnimationStates.PipeYellowFullCorner:
                fps = fpsFullPipes;
                LoopingAnimation(yellowFullCorner);
                break;

            case GameData.AnimationStates.PipeYellowFullCross:
                fps = fpsFullPipes;
                LoopingAnimation(yellowFullCross);
                break;

            case GameData.AnimationStates.PipeYellowFullT:
                fps = fpsFullPipes;
                LoopingAnimation(yellowFullT);
                break;

            case GameData.AnimationStates.PipeYellowFullStraight:
                fps = fpsFullPipes;
                LoopingAnimation(yellowFullStraight);
                break;

        }
    }

    private void ReadSpriteSheet(GameData.SpriteSheet spriteSheet) { 
        framePosition.y = 1;
        for (iCount = currentFrame; iCount > columns; iCount -= rows)
        {
            framePosition.y += 1;
        }
        framePosition.x = iCount - 1;

        frameSize = new Vector2(1.0f / columns, 1.0f / rows);
        frameOffset = new Vector2(framePosition.x / columns, 1.0f - (framePosition.y / rows));

        GetComponent<Renderer>().material.SetTextureScale("_MainTex", frameSize);
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", frameOffset);
    }

    private void LoopingAnimation(int[] frames) {

        currentFrame = Mathf.Clamp(currentFrame, frames[0], frames[frames.Length - 1] + 1);
        if (currentFrame > frames[frames.Length - 1])
        {
            currentFrame = frames[0];
        }

    }

    private void CharacterOneShotAnimation(int[] frames)
    {

        currentFrame = Mathf.Clamp(currentFrame, frames[0], frames[frames.Length - 1] + 1);
        if (currentFrame > frames[frames.Length - 1])
        {
            currentAnim = previousAnim;
            hasMovementPermit = true;
        }

    }
}

