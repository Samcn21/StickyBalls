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

    public GameData.AnimationStates currentAnim = GameData.AnimationStates.IdleFront;
    public GameData.AnimationStates previousAnim = GameData.AnimationStates.IdleFront;
    public bool hasMovementPermit = true;
    private int iCount;

    //ANIMATION FRAMES 
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


    void Update()
    {
        PlayAnimation();
        ReadSpriteSheet();
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
        }
    }

    private void ReadSpriteSheet() { 
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

