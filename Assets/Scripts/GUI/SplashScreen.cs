using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using GamepadInput;


public class SplashScreen : MonoBehaviour
{
    public string levelName = "MainMenu";
    public float waitForSec = 60f;
	
	void Update () 
    {
        waitForSec -= Time.deltaTime;

        if (waitForSec < 0 || GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any))
            SceneManager.LoadScene(levelName);
	}
}
