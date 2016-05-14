using UnityEngine;
using System.Collections;
using GamepadInput;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    private bool isPaused = false;
    
    void Start () {
        isPaused = false;
	}
	
	void Update () {
        if (isPaused) 
        {
            GetComponentInChildren<Image>().enabled = true;
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
