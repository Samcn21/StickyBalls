using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {
    //Arrays of textures that corresponds to the several possible rotations of the different pipes
    [SerializeField]
    private Texture[] CrossPipeTextureRotations;
    [SerializeField]
    private Texture[] CornerPipeTextureRotations;
    [SerializeField]
    private Texture[] StraightPipeTextureRotations;
    [SerializeField]
    private Texture[] TPipeTextureRotations;

    //Field to store the information relative to the players
    private TeamPipeInfo[] teamsInfo;

    //X and Y padding towards the center. Needs to be positive
    private float XPadding;
    private float YPadding;

    public float textureScaler;
    
    void Awake()
    {
        teamsInfo = new TeamPipeInfo[4];
        XPadding = Screen.width * 5 / 100;
        YPadding = Screen.height * 5 / 100;
        teamsInfo[0] = new TeamPipeInfo(GameData.Team.Black, XPadding,YPadding);
        teamsInfo[1] = new TeamPipeInfo(GameData.Team.Red, XPadding, YPadding);
        teamsInfo[2] = new TeamPipeInfo(GameData.Team.Yellow, XPadding, YPadding);
        teamsInfo[3] = new TeamPipeInfo(GameData.Team.Blue, XPadding, YPadding);
        
    }

	void OnGUI()
    {
        //Foreach stored information, if you have to show the held pipe, get the correct texture and draw it on the screen
        foreach(TeamPipeInfo teamInfo in teamsInfo)
        {
            if (teamInfo.show)
            {   
                Texture t = getCorrectTextureBasedOnRotation(teamInfo.pipeInfo, teamInfo.rotationIndex);
                float textureWidth, textureHeight;
                textureWidth = t.width / textureScaler;
                textureHeight = t.height / textureScaler;
                float[] offsetXY = GetOffsetXY(textureWidth,textureHeight, teamInfo.teamColor);
                GUI.DrawTexture(new Rect(teamInfo.posX+ offsetXY[0], teamInfo.posY+offsetXY[1], textureWidth, textureHeight), t);
            }
        }
    }

    private float[] GetOffsetXY(float textureWidth,float textureHeight,GameData.Team color)
    {
        float[] offsetXY = new float[2];
        offsetXY[0] = 0;
        offsetXY[1] = 0;
        switch (color)
        {
            case GameData.Team.Black:
                offsetXY[0] -= textureWidth / 2;
                offsetXY[1] -= textureHeight / 2;
                break;
            case GameData.Team.Blue:
                offsetXY[0] += textureWidth / 2;
                offsetXY[1] += textureHeight / 2;
                break;
            case GameData.Team.Red:
                offsetXY[0] += textureWidth / 2;
                offsetXY[1] -= textureHeight / 2;
                break;
            case GameData.Team.Yellow:
                offsetXY[0] -= textureWidth/ 2;
                offsetXY[1] += textureHeight / 2;
                break;
        }
        return offsetXY;
    }

    /// <summary>
    /// Returns the correct texture(GUI does not allow the rotation of a texture) based on the pipe type and the index
    /// </summary>
    /// <param name="pipeType">The type of the pipe</param>
    /// <param name="index">The index of the rotation</param>
    /// <returns>The texture to display</returns>
    private Texture getCorrectTextureBasedOnRotation(PipeData.PipeType pipeType, int index)
    {
        switch (pipeType)
        {
            case PipeData.PipeType.Corner:
                return CornerPipeTextureRotations[index];
            case PipeData.PipeType.Cross:
                return CrossPipeTextureRotations[index];
            case PipeData.PipeType.Straight:
                return StraightPipeTextureRotations[index];
            case PipeData.PipeType.T:
                return TPipeTextureRotations[index];
            default:
                return null;
        }
    }

    /// <summary>
    /// Hides the pipe of the team
    /// </summary>
    /// <param name="color">The team owner of the pipe to hide</param>
    public void HidePipe(GameData.Team color)
    {
        foreach (TeamPipeInfo teamInfo in teamsInfo)
            if (teamInfo.teamColor == color)
            {
                teamInfo.show = false;
                break;
            }
    }
    /// <summary>
    /// Shows the pipe of the team with certain rotation
    /// </summary>
    /// <param name="color">The color of the team</param>
    /// <param name="pipeType">The type of the pipe to display</param>
    /// <param name="rot">The rotation of the pipe to display</param>
    public void ShowPipe(GameData.Team color, PipeData.PipeType pipeType, int rot)
    {
        foreach (TeamPipeInfo teamInfo in teamsInfo)
            if (teamInfo.teamColor == color)
            {
                teamInfo.show = true;
                teamInfo.pipeInfo = pipeType;
                teamInfo.rotationIndex = rot;
            }
    }

    /// <summary>
    /// Internal class To keep track of the informations of the players
    /// </summary>
    internal class TeamPipeInfo
    {
        public bool show;
        public GameData.Team teamColor;
        public PipeData.PipeType pipeInfo;
        public int rotationIndex;
    
        public float posX { private set; get; }
        public float posY { private set; get; }

        public TeamPipeInfo(GameData.Team color,float xPadding, float yPadding)
        {
            show = false;
            teamColor = color;
            pipeInfo = PipeData.PipeType.Void;
            rotationIndex = 0;
            switch (teamColor)
            {
                case GameData.Team.Black:
                    posX = xPadding;
                    posY = yPadding;
                    break;
                case GameData.Team.Blue:
                    posX = Screen.width-xPadding;
                    posY = Screen.height-yPadding;
                    break;
                case GameData.Team.Red:
                    posX = xPadding;
                    posY = Screen.height-yPadding;
                    break;
                case GameData.Team.Yellow:
                    posX = Screen.width-xPadding;
                    posY = yPadding;
                    break;
            }
        }

    }
}
