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
    private InputController InputController;
    private GameObject[] players;
    private AudioSource playerAS;

    //We need to get the volume settings from the menu in the end to apply to all sounds and music
    private float SFXVolume = 1;
    private float SFXVolumeSetting = 50;
    private float MusicVolume = 1;
    private float MusicVolumeSetting = 50;

    public float[] playerPitchRanges = new float[5] { 1, 0.7f, 0.9f, 1.1f, 1.3f };
    [SerializeField] private float pitchRange = 1;

    void Start() 
    {
        SFXVolume = SFXVolume * (SFXVolumeSetting / 100);
        MusicVolume = MusicVolume * (MusicVolumeSetting / 100);

        players = GameObject.FindGameObjectsWithTag("Player");
    }

    public void PlayOneShotPlayer(GameData.AudioClipState state, GamePad.Index playerNumber, bool needsPitch)
    {
        foreach (GameObject player in players)
        {
            if (player.GetComponent<InputController>().index == playerNumber) 
            {
                playerAS = player.GetComponent<AudioSource>();
            }
        }

        switch (state)
        {
            case GameData.AudioClipState.PickupPipe:
                playerAS.clip = pickupPipeClip;
                break;

            case GameData.AudioClipState.PlacePipe:
                playerAS.clip = placePipeClip;
                break;

            case GameData.AudioClipState.RotatePipe:
                playerAS.clip = rotatePipeClip;
                break;
        }

        if (needsPitch)
        {
            switch (playerNumber)
            {
                case GamePad.Index.One:
                    pitchRange = playerPitchRanges[1];
                    break;

                case GamePad.Index.Two:
                    pitchRange = playerPitchRanges[2];
                    break;

                case GamePad.Index.Three:
                    pitchRange = playerPitchRanges[3];
                    break;

                case GamePad.Index.Four:
                    pitchRange = playerPitchRanges[4];
                    break;
            }
        }
        else
        {
            pitchRange = playerPitchRanges[0];
        }
        playerAS.pitch = pitchRange;
        playerAS.volume = SFXVolume;
        playerAS.Play();
    }

}
