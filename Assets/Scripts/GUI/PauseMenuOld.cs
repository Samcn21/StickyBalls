using UnityEngine;
using System.Collections;
using GamepadInput;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class PauseMenuOld : MonoBehaviour {
    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);

    public float mouseSensitivity = 50f;
    private bool isPaused = false;
    private float stickSensivity = 0.25f;
    private Vector2 mousePos;
    private ScreenFader ScreenFader;
    private GameObject[] buttons;
    private bool screenFader = false;


    void Start () {
        screenFader = true;
        isPaused = false;
        SetCursorPos(Screen.width / 2, Screen.height / 2);
        mousePos = new Vector2(Screen.width / 2, Screen.height / 2);
        ScreenFader = GameObject.FindGameObjectWithTag("FadeImg").GetComponent<ScreenFader>();
        if (ScreenFader == null)
        {
            Debug.LogError("The scene must have screen fader prefab. it's in Prefabs > GUI > pfbScreenFader");
        }
        buttons = GameObject.FindGameObjectsWithTag("PauseMenuButton");
	}

	void Update () {

        if (isPaused)
        {
            ScreenFader.FadeToBlack();
            foreach(GameObject btn in buttons)
            {
                btn.GetComponent<Image>().enabled = true;
            }

            if ((GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any)) != Vector2.zero)
            {
                GamepadState state = GamePad.GetState(GamePad.Index.Any);
                mousePos.x += (state.LeftStickAxis.x * stickSensivity) * mouseSensitivity;
                mousePos.y -= (state.LeftStickAxis.y * stickSensivity) * mouseSensitivity;

                SetCursorPos(Mathf.CeilToInt(mousePos.x), (Mathf.CeilToInt(mousePos.y)));
            }

            if (GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any))
            {
                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp | MouseOperations.MouseEventFlags.LeftDown);
            }
        }
        else 
        {
            ScreenFader.FadeToClear();
            foreach (GameObject btn in buttons)
            {
                btn.GetComponent<Image>().enabled = false;
            }
        }
	}

    public void DoPause(bool state)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (state)
        {
            foreach(GameObject player in players)
            {
                player.GetComponent<InputController>().enabled = false;
            }
            //Time.timeScale = 0;   //if I use timescale the screen fader doesn;t work so I disable input controller
            //in this case we could still have the music countining 
            isPaused = true;
            
        }
        else
        {
            foreach (GameObject player in players)
            {
                player.GetComponent<InputController>().enabled = true;
            }
            //Time.timeScale = 1;
            isPaused = false;
        }
    }
}
