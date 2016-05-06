using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {
    protected int columns = 2;
    protected int rows = 2;
    private Vector2 framePosition;
    private Vector2 frameSize;
    private Vector2 frameOffset;
    private int iCount;
    protected int currentFrame = 1;

    void Start () {
	
	}
	
	void Update () {
	
	}

    protected void ReadSpriteSheet(GameData.SpriteSheet spriteSheet)
    {
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
}
