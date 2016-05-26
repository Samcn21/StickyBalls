using UnityEngine;
using System.Collections;
using GamepadInput;

public class CharacterSprite : AnimationController
{

    public GameData.SpriteSheet mySpriteSheet = GameData.SpriteSheet.Character;
    public int spriteSheetColumns = 8;
    public int spriteSheetRows = 8;

    public GameData.PlayerState currentAnim = GameData.PlayerState.IdleFront;
    public GameData.PlayerState previousAnim = GameData.PlayerState.IdleFront;

    public Material charNeutralMat;
    public Material charBlueMat;
    public Material charCyanMat;
    public Material charPurpleMat;
    public Material charYellowMat;
    public Material charNeutralSecondMat;
    public Material charBlueSecondMat;
    public Material charCyanSecondMat;
    public Material charPurpleSecondMat;
    public Material charYellowSecondMat;
    public Renderer rend;

    private InputController InputController;

    public float fpsIdle = 4;
    public float fpsMovement = 15;
    public float fpsMovementCarry = 15;
    public float fpsPipeGrab = 6;
    public float fpsPipePlace = 6;
    public float fpsDance = 3;

    public bool hasMovementPermit = true;


    //ANIMATION FRAMES FOR CHARACTERS
    private int idleFront = 1;
    private int idleBack = 25;
    private int idleRight = 9;
    private int idleLeft = 17;


    private int[] movementFront         = new int[3] { 1, 2, 3 };
    private int[] movementBack          = new int[3] { 25, 26, 27 };
    private int[] movementRight         = new int[3] { 9, 10, 11 };
    private int[] movementLeft          = new int[3] { 17, 18, 19 };

    private int[] movementFrontCarry    = new int[3] { 4, 5, 6 };
    private int[] movementBackCarry     = new int[3] { 28, 29, 30 };
    private int[] movementRightCarry    = new int[3] { 12, 13, 14 };
    private int[] movementLeftCarry     = new int[3] { 20, 21, 22 };

    private int pipeGrabFront           = 4;
    private int pipeGrabBack            = 28;
    private int pipeGrabRight           = 12;
    private int pipeGrabLeft            = 20;

    private int pipePlaceFront          = 4;
    private int pipePlaceBack           = 28;
    private int pipePlaceRight          = 12;
    private int pipePlaceLeft           = 20;

    private int[] dance = new int[10] { 2, 3, 13, 14, 15, 16, 21, 22, 23, 24};

    public GamePad.Index gamepadIndex;
    public int pressCounter;
    public int pressNeeded;
    public float actionTime;
    public bool extraMatPermit;

    void Start()
    {
        if (this.tag != "VirtualPlayer")
        { 
            InputController = transform.parent.GetComponent<InputController>();
            gamepadIndex = InputController.index;
        }

        pressCounter = 0;
        pressNeeded = 12;
        actionTime = 2.5f;
        extraMatPermit = false;
    }

    public void Settings()
    {
        rows = spriteSheetRows;
        columns = spriteSheetColumns;
        FindMaterial();
        ExtraMat();
    }

    void Update()
    {
        Settings();
        FPSController();
        PlayAnimation();
        ReadSpriteSheet(mySpriteSheet);
    }

    public void FindMaterial()
    {
        if (InputController != null)
        {
            switch (InputController.team)
            {
                case GameData.Team.Blue:
                    if (extraMatPermit)
                    {
                        rend.material = charBlueSecondMat;
                    }
                    else 
                    {
                        rend.material = charBlueMat;
                    }
                    break;

                case GameData.Team.Cyan:
                    if (extraMatPermit)
                    {
                        rend.material = charCyanSecondMat;
                    }
                    else 
                    {
                        rend.material = charCyanMat;
                    }
                    break;

                case GameData.Team.Purple:
                    if (extraMatPermit)
                    {
                        rend.material = charPurpleSecondMat;
                    }
                    else
                    {
                        rend.material = charPurpleMat;
                    }
                    break;

                case GameData.Team.Yellow:
                    if (extraMatPermit)
                    {
                        rend.material = charYellowSecondMat;
                    }
                    else
                    {
                        rend.material = charYellowMat;
                    }
                    break;

                case GameData.Team.Neutral:
                    if (extraMatPermit)
                    {
                        rend.material = charNeutralSecondMat;
                    }
                    else
                    {
                        rend.material = charNeutralMat;
                    }
                    break;
            }
        }
    }

    public void FindMaterialVirtualPlayer(GameData.Team color)
    {
        switch (color)
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

    public void FindDanceAnimation()
    {
        currentAnim = GameData.PlayerState.Dance;
    }

    public void FindGrabPipeAnimation()
    {
        previousAnim = currentAnim;
        if (previousAnim.ToString().Contains("Front"))
        {
            currentAnim = GameData.PlayerState.PipeGrabFront;
        }
        else if (previousAnim.ToString().Contains("Right"))
        {
            currentAnim = GameData.PlayerState.PipeGrabRight;
        }
        else if (previousAnim.ToString().Contains("Left"))
        {
            currentAnim = GameData.PlayerState.PipeGrabLeft;
        }
        else if (previousAnim.ToString().Contains("Back"))
        {
            currentAnim = GameData.PlayerState.PipeGrabBack;
        }
    }


    public void FindPlacePipeAnimation()
    {
        previousAnim = currentAnim;
        if (previousAnim.ToString().Contains("Front"))
        {
            currentAnim = GameData.PlayerState.PipePlaceFront;
        }
        else if (previousAnim.ToString().Contains("Right"))
        {
            currentAnim = GameData.PlayerState.PipePlaceRight;
        }
        else if (previousAnim.ToString().Contains("Left"))
        {
            currentAnim = GameData.PlayerState.PipePlaceLeft;
        }
        else if (previousAnim.ToString().Contains("Back"))
        {
            currentAnim = GameData.PlayerState.PipePlaceBack;
        }
    }

    private void PlayAnimation()
    {
        switch (currentAnim)
        {
            //character pipe grab animations
            case GameData.PlayerState.PipeGrabFront:
                fps = fpsPipeGrab;
                currentFrame = pipeGrabFront;
                break;

            case GameData.PlayerState.PipeGrabBack:
                fps = fpsPipeGrab;
                currentFrame = pipeGrabBack;
                break;

            case GameData.PlayerState.PipeGrabLeft:
                fps = fpsPipeGrab;
                currentFrame = pipeGrabLeft;
                break;

            case GameData.PlayerState.PipeGrabRight:
                fps = fpsPipeGrab;
                currentFrame = pipeGrabRight;
                break;

            //character pipe place animations
            case GameData.PlayerState.PipePlaceFront:
                fps = fpsPipePlace;
                currentFrame = pipePlaceFront;
                break;

            case GameData.PlayerState.PipePlaceBack:
                fps = fpsPipePlace;
                currentFrame = pipePlaceBack;
                break;

            case GameData.PlayerState.PipePlaceLeft:
                fps = fpsPipePlace;
                currentFrame = pipePlaceLeft;
                break;

            case GameData.PlayerState.PipePlaceRight:
                fps = fpsPipePlace;
                currentFrame = pipePlaceRight;
                break;

            //character idle animations
            case GameData.PlayerState.IdleFront:
                fps = fpsIdle;
                currentFrame = idleFront;
                break;

            case GameData.PlayerState.IdleBack:
                fps = fpsIdle;
                currentFrame = idleBack;
                break;

            case GameData.PlayerState.IdleRight:
                fps = fpsIdle;
                currentFrame = idleRight;
                break;

            case GameData.PlayerState.IdleLeft:
                fps = fpsIdle;
                currentFrame = idleLeft;
                break;

            //character movemnt animations
            case GameData.PlayerState.MovementFront:
                fps = fpsMovement;
                LoopingAnimation(movementFront);
                break;

            case GameData.PlayerState.MovementBack:
                fps = fpsMovement;
                LoopingAnimation(movementBack);
                break;

            case GameData.PlayerState.MovementRight:
                fps = fpsMovement;
                LoopingAnimation(movementRight);
                break;

            case GameData.PlayerState.MovementLeft:
                fps = fpsMovement;
                LoopingAnimation(movementLeft);
                break;

            //character carrying pipe animations
            case GameData.PlayerState.MovementFrontCarryPipe:
                fps = fpsMovementCarry;
                LoopingAnimation(movementFrontCarry);
                break;

            case GameData.PlayerState.MovementBackCarryPipe:
                fps = fpsMovementCarry;
                LoopingAnimation(movementBackCarry);
                break;

            case GameData.PlayerState.MovementRightCarryPipe:
                fps = fpsMovementCarry;
                LoopingAnimation(movementRightCarry);
                break;

            case GameData.PlayerState.MovementLeftCarryPipe:
                fps = fpsMovementCarry;
                LoopingAnimation(movementLeftCarry);
                break;

            case GameData.PlayerState.Dance:
                fps = fpsDance;
                LoopingAnimation(dance);
                break;
        }
    }
    private void ExtraMat()
    {
        if (pressCounter > 0)
        {
            actionTime -= Time.deltaTime;
        }

        if (GamePad.GetButtonDown(GamePad.Button.Back, gamepadIndex))
        {
            pressCounter++;
            if (actionTime <= 0)
            {
                pressCounter = 0;
                actionTime = 2.5f;
            }

            if (pressCounter >= pressNeeded && actionTime >= 0)
            {
                pressCounter = 0;
                actionTime = 2.5f;

                if (extraMatPermit)
                {
                    extraMatPermit = false;
                }
                else
                {
                    extraMatPermit = true;
                }
            }
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
