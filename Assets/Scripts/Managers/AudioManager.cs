using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public AudioClip MusicClip;
    public AudioClip pickupPipeClip;
    public AudioClip placePipeClip;
    public AudioClip bombExplosionClip;
    public AudioClip winSoundClip;
    public AudioClip rotatePipeClip;

    public AudioSource audioControllerSFX;                   
    public AudioSource audioControllerMusic;                 
    public static AudioManager instantiate = null;                      
    
    public float lowPitchRange = .95f;                       
    public float highPitchRange = 1.05f;                     


    void Awake()
    {
        if (instantiate == null)
            instantiate = this;
        else if (instantiate != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


    public void PlayOneShot(GameData.AudioClipState state)
    {
        switch (state)
        { 
            case GameData.AudioClipState.PickupPipe:
                audioControllerSFX.clip = pickupPipeClip;
                break;

            case GameData.AudioClipState.PlacePipe:
                audioControllerSFX.clip = placePipeClip;
                break;

            case GameData.AudioClipState.RotatePipe:
                audioControllerSFX.clip = rotatePipeClip;
                break;
       }

       audioControllerSFX.Play();
    }

}
