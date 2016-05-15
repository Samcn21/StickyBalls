﻿using UnityEngine;
using System.Collections;
using GamepadInput;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class PauseMenu : MonoBehaviour {
    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);

    public float mouseSensitivity = 50f;
    private bool isPaused = false;
    private float stickSensivity = 0.25f;
    private Vector2 mousePos;

    void Start () {
        isPaused = false;
        SetCursorPos(Screen.width / 2, Screen.height / 2);
        mousePos = new Vector2(Screen.width / 2, Screen.height / 2);
	}

	void Update () {

        if (isPaused)
        {
            GetComponentInChildren<Image>().enabled = true;

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
            GetComponentInChildren<Image>().enabled = false;
        }
	}

    public void DoPause(bool state)
    {
        if (state)
        {
            Time.timeScale = 0;
            isPaused = true;    
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }
}
