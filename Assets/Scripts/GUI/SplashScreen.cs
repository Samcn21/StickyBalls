using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public string levelName = "MainMenu";
    public float waitForSec = 3f;
	
	void Update () 
    {
        waitForSec -= Time.deltaTime;

        if (waitForSec < 0)
            SceneManager.LoadScene(levelName);
	}
}
