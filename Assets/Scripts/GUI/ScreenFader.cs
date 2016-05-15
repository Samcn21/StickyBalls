using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
	public Image FadeImg;
	public float fadeSpeed = 2.0f;
	public bool sceneStarting = true;
	
	
	void Start()
	{
		FadeImg.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
	}
	
	void Update()
	{
		if (sceneStarting) StartScene();
	}
	
	
	public void FadeToClear()
	{
		// Lerp the colour of the image between itself and transparent.
		FadeImg.color = Color.Lerp(FadeImg.color, Color.clear, fadeSpeed * Time.deltaTime);
	}


    public void FadeToBlack()
	{
		FadeImg.color = Color.Lerp(FadeImg.color, Color.black, fadeSpeed * Time.deltaTime);
	}

    public IEnumerator fadeToBlack(float timeToFade)
    {
        float t = 0f;
        while (t < timeToFade)
        {
            FadeImg.color = Color.Lerp(FadeImg.color, Color.black,t/ timeToFade);
            t += Time.deltaTime;

            yield return null; //new WaitForSeconds(0.0f);
        }
        //Application.LoadLevel(4);
    
    }

    void StartScene()
	{
		FadeToClear();
		
		if (FadeImg.color.a <= 0.05f)
		{
			FadeImg.color = Color.clear;
			FadeImg.enabled = false;
			
			sceneStarting = false;
		}
	}
    public void enableImage() {
        FadeImg.enabled = true;
    }


	
	public void EndScene()
	{
		FadeImg.enabled = true;
		FadeToBlack();
		// If the screen is almost black...
		if (FadeImg.color.a >= 0.95f)
			Application.LoadLevel(Application.loadedLevel);
	}
}   