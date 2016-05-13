using UnityEngine;
using System.Collections;

public class CharacterSprite : AnimationController {

    public GameData.SpriteSheet mySpriteSheet = GameData.SpriteSheet.Character;
    public int spriteSheetColumns   = 8;
    public int spriteSheetRows      = 8;
    [SerializeField]
    int currentFrameShow = 1;

    public GameData.PlayerState currentAnim = GameData.PlayerState.IdleFront;
    public GameData.PlayerState previousAnim = GameData.PlayerState.IdleFront;

    public Material charNeutralMat;
    public Material charBlueMat;
    public Material charCyanMat;
    public Material charPurpleMat;
    public Material charYellowMat;
    public Renderer rend;

    private InputController InputController;

    public float fpsIdle                = 4;
    public float fpsMovement            = 15;
    public float fpsPipeGrab            = 6;
    public float fpsPipePlace           = 6;

    public bool hasMovementPermit = true;


    //ANIMATION FRAMES FOR CHARACTERS
    private int[] idleFront             = new int[3] { 1, 2, 3 };
    private int[] idleBack              = new int[3] { 25, 26, 27 };
    private int[] idleRight             = new int[3] { 9, 10, 11 };
    private int[] idleLeft              = new int[3] { 17, 18, 19 };
    private int[] movementFront         = new int[5] { 4, 5, 6, 7, 8 };
    private int[] movementBack          = new int[5] { 28, 29, 30, 31, 32 };
    private int[] movementRight         = new int[5] { 12, 13, 14, 15, 16 };
    private int[] movementLeft          = new int[5] { 20, 21, 22, 23, 24 };
    private int[] pipeGrabFront         = new int[2] { 33, 34 };
    private int[] pipeGrabBack          = new int[2] { 39, 40 };
    private int[] pipeGrabRight         = new int[2] { 35, 36 };
    private int[] pipeGrabLeft          = new int[2] { 37, 38 };
    private int[] pipePlaceFront        = new int[2] { 41, 42 };
    private int[] pipePlaceBack         = new int[2] { 47, 48 };
    private int[] pipePlaceRight        = new int[2] { 43, 44 };
    private int[] pipePlaceLeft         = new int[2] { 45, 46 };

    void Start() 
    {
        InputController = transform.parent.GetComponent<InputController>();
    }

    public void Settings()
    {
        rows = spriteSheetRows;
        columns = spriteSheetColumns;
        FindMaterial();
    }

    void Update()
    {
        Settings();
        FPSController();
        PlayAnimation();
        ReadSpriteSheet(mySpriteSheet);
        currentFrameShow = currentFrame;
    }

    public void FindMaterial() 
    {
        if (InputController != null)
        {
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
    public void FindGrabPipeAnimation()
    {
        hasMovementPermit = false;
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
        hasMovementPermit = false;
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
                CharacterOneShotAnimation(pipeGrabFront);
                break;

            case GameData.PlayerState.PipeGrabBack:
                fps = fpsPipeGrab;
                CharacterOneShotAnimation(pipeGrabBack);
                break;

            case GameData.PlayerState.PipeGrabLeft:
                fps = fpsPipeGrab;
                CharacterOneShotAnimation(pipeGrabLeft);
                break;

            case GameData.PlayerState.PipeGrabRight:
                fps = fpsPipeGrab;
                CharacterOneShotAnimation(pipeGrabRight);
                break;

            //character pipe place animations
            case GameData.PlayerState.PipePlaceFront:
                fps = fpsPipePlace;
                CharacterOneShotAnimation(pipePlaceFront);
                break;

            case GameData.PlayerState.PipePlaceBack:
                fps = fpsPipePlace;
                CharacterOneShotAnimation(pipePlaceBack);
                break;

            case GameData.PlayerState.PipePlaceLeft:
                fps = fpsPipePlace;
                CharacterOneShotAnimation(pipePlaceLeft);
                break;

            case GameData.PlayerState.PipePlaceRight:
                fps = fpsPipePlace;
                CharacterOneShotAnimation(pipePlaceRight);
                break;

            //character idle animations
            case GameData.PlayerState.IdleFront:
                fps = fpsIdle;
                LoopingAnimation(idleFront);
                break;

            case GameData.PlayerState.IdleBack:
                fps = fpsIdle;
                LoopingAnimation(idleBack);
                break;

            case GameData.PlayerState.IdleRight:
                fps = fpsIdle;
                LoopingAnimation(idleRight);
                break;

            case GameData.PlayerState.IdleLeft:
                fps = fpsIdle;
                LoopingAnimation(idleLeft);
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
