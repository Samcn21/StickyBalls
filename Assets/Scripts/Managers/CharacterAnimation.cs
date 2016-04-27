using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour
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
    public float fps = 10.0f;
    [SerializeField] private animState currentAnim = animState.IdleFront;
    [SerializeField] private animState previousAnim = animState.IdleFront;

    private int i;

    [SerializeField] private float velocityX;
    [SerializeField] private float velocityZ;
    [SerializeField] private float velocityTotal;
    [SerializeField] private float animThreshold = 0.1f;

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

    private int animFrameCounter    =           0;

    //ANIMATION STATES
    private enum animState {
        IdleFront,
        MovementFront,
        IdleBack,
        MovementBack,
        IdleRight,
        MovementRight,
        IdleLeft,
        MovementLeft,
        PipeGrabFront,
        PipeGrabBack,
        PipeGrabRight,
        PipeGrabLeft,
        PipePlaceFront,
        PipePlaceBack,
        PipePlaceRight,
        PipePlaceLeft
    }



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FindAnimationState();
        PlayAnimation();
    }

    void FindAnimationState()
    {
        Rigidbody playerRigidbody = transform.parent.GetComponent<Rigidbody>();
        velocityX = playerRigidbody.velocity.x;
        velocityZ = playerRigidbody.velocity.z;
        velocityTotal = Mathf.Abs(velocityX + velocityZ);

        if (velocityTotal <= animThreshold)
        {
            if (previousAnim.ToString().Contains("Front")){
                previousAnim = currentAnim;
                currentAnim = animState.IdleFront;
            }
            else if (previousAnim.ToString().Contains("Right"))
            {
                previousAnim = currentAnim;
                currentAnim = animState.IdleRight;
            }
            else if (previousAnim.ToString().Contains("Left"))
            {
                previousAnim = currentAnim;
                currentAnim = animState.IdleLeft;
            }
            else if (previousAnim.ToString().Contains("Back"))
            {
                previousAnim = currentAnim;
                currentAnim = animState.IdleBack;
            }

        }
        else 
        {
            if (velocityX >= animThreshold && Mathf.Abs(velocityX) > Mathf.Abs(velocityZ))
            {
                previousAnim = currentAnim;
                currentAnim = animState.MovementRight;
            }
            else if (velocityZ >= animThreshold && Mathf.Abs(velocityX) < Mathf.Abs(velocityZ))
            {
                previousAnim = currentAnim;
                currentAnim = animState.MovementBack;
            }
            else if (velocityZ <= animThreshold && Mathf.Abs(velocityX) < Mathf.Abs(velocityZ))
            {
                previousAnim = currentAnim;
                currentAnim = animState.MovementFront;
            }
            else if (velocityX <= animThreshold && Mathf.Abs(velocityX) > Mathf.Abs(velocityZ))
            {
                previousAnim = currentAnim;
                currentAnim = animState.MovementLeft;
            }
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

        //LOOPING ANIMATIONS
        if (currentAnim == animState.IdleFront)
        {
            LoopingAnimation(idleFront);
        }
        if (currentAnim == animState.IdleBack)
        {
            LoopingAnimation(idleBack);
        }
        if (currentAnim == animState.IdleRight)
        {
            LoopingAnimation(idleRight);
        }
        if (currentAnim == animState.IdleLeft)
        {
            LoopingAnimation(idleLeft);
        }
        if (currentAnim == animState.MovementRight)
        {
            LoopingAnimation(movementRight);
        }
        if (currentAnim == animState.MovementLeft)
        {
            LoopingAnimation(movementLeft);
        }
        if (currentAnim == animState.MovementFront)
        {
            LoopingAnimation(movementFront);
        }
        if (currentAnim == animState.MovementBack)
        {
            LoopingAnimation(movementBack);
        }

        framePosition.y = 1;
        for (i = currentFrame; i > columns; i -= rows)
        {
            framePosition.y += 1;
        }
        framePosition.x = i - 1;

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
}

