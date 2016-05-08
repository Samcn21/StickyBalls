using UnityEngine;
using System.Collections;
using GamepadInput;

public class AudioManager : MonoBehaviour
{

    public AudioClip MusicClip;
    public AudioClip pickupPipeClip;
    public AudioClip placePipeClip;
    public AudioClip bombExplosionClip;
    public AudioClip winSoundClip;
    public AudioClip rotatePipeClip;


    public AudioSource audioControllerSFX;
    public AudioSource audioControllerMusic;
    public static AudioManager instantiate = null;

    public float[] pitchRanges = new float[5] { 1, 0.7f, 0.9f, 1.1f, 1.3f };
    [SerializeField] private float pitchRange = 1;


    void Awake()
    {
        if (instantiate == null)
            instantiate = this;
        else if (instantiate != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }



    public void PlayOneShotPlayer(GameData.AudioClipState state, GamePad.Index playerNumber, bool needsPitch)
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

        if (needsPitch)
        {
            switch (playerNumber)
            {
                case GamePad.Index.One:
                    pitchRange = pitchRanges[1];
                    break;

                case GamePad.Index.Two:
                    pitchRange = pitchRanges[2];
                    break;

                case GamePad.Index.Three:
                    pitchRange = pitchRanges[3];
                    break;

                case GamePad.Index.Four:
                    pitchRange = pitchRanges[4];
                    break;
            }
        }
        else
        {
            pitchRange = pitchRanges[0];
        }
        audioControllerSFX.pitch = pitchRange;
        audioControllerSFX.volume = 0.4f;
        audioControllerSFX.Play();
    }

}
