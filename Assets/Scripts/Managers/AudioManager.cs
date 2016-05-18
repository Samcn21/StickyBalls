using UnityEngine;
using System.Collections;
using GamepadInput;

public class AudioManager : MonoBehaviour
{
    //Music Clip
    public AudioClip musicClip;
    public AudioClip menuMusicClip;

    //Player Clips
    public AudioClip pickupPipeClip;
    public AudioClip placePipeClip;
    public AudioClip pushClip;
    public AudioClip rotatePipeClip;
    public AudioClip walkingClip;

    //Level Clips
    public AudioClip bombExplosionClip;
    public AudioClip winSoundClip;
    public AudioClip pipeExplosion;
    public AudioClip sourceMachineExplosion;
    
    //GUI Clips
    public AudioClip menuNavClip;


    public AudioSource audioControllerSFX;
    public AudioSource audioControllerSFXsmExplosion;
    public AudioSource audioControllerMusic;
    private InputController InputController;
    private GameObject[] players;
    private AudioSource playerAS;

    //We need to get the volume settings from the menu in the end to apply to all sounds and music
    private float SFXVolume = 1;
    [Range (0,100)]
    public float SFXVolumeSetting = 50;
    private float MusicVolume = 1;
    [Range(0, 100)]
    public float MusicVolumeSetting = 30;

    public float[] playerPitchRanges = new float[5] { 1, 0.7f, 0.9f, 1.1f, 1.3f };
    [SerializeField] private float pitchRange = 1;

    void Start() 
    {
        SFXVolume = SFXVolume * (SFXVolumeSetting / 100);
        MusicVolume = MusicVolume * (MusicVolumeSetting / 100);

        PlayMusic(true);
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    public void PlayMusic(bool play) 
    {
        if (play)
        {
            audioControllerMusic.loop = true;
            audioControllerMusic.clip = musicClip;
            audioControllerMusic.volume = MusicVolume;
            audioControllerMusic.Play();
        }
        else 
        {
            audioControllerMusic.Stop();

        }
    }

    public void PlayMenuNav(bool status)
    {
        if (status)
        {
            pitchRange = playerPitchRanges[2];
        }
        else
        {
            pitchRange = playerPitchRanges[4];
        }
        audioControllerSFX.pitch = pitchRange;
        audioControllerSFX.clip = menuNavClip;
        audioControllerSFX.volume = SFXVolume;
        audioControllerSFX.Play();

    }
    public void PlayPipeExplosion(string explosionName, Vector3 pos)
    {
        GameObject go = GameObject.Find(explosionName);
        if (go.transform.position.x == pos.x || go.transform.position.y == pos.y)
        {
            if (pos.z % 2 == 0)
            {
                pitchRange = playerPitchRanges[4];
            }
            else
            {
                pitchRange = playerPitchRanges[1];
            }
            //TODO: if it is Source Machine don't play
            AudioSource pipeAS = go.GetComponent<AudioSource>();
            pipeAS.clip = pipeExplosion;
            pipeAS.volume = SFXVolume;
            pipeAS.pitch = pitchRange;
            pipeAS.Play();
        }
    }

    public void PlaySourceExplosion(GameData.PlayerSourceDirection smPos) 
    {
        switch (smPos)
        { 
            case GameData.PlayerSourceDirection.BottomLeft:
                pitchRange = playerPitchRanges[1];
                break;

            case GameData.PlayerSourceDirection.BottomRight:
                pitchRange = playerPitchRanges[2];
                break;

            case GameData.PlayerSourceDirection.TopLeft:
                pitchRange = playerPitchRanges[3];
                break;

            case GameData.PlayerSourceDirection.TopRight:
                pitchRange = playerPitchRanges[4];
                break;

        }

        audioControllerSFXsmExplosion.clip = sourceMachineExplosion;
        audioControllerSFXsmExplosion.volume = SFXVolume;
        audioControllerSFXsmExplosion.pitch = pitchRange;
        audioControllerSFXsmExplosion.Play();
    }

    public void StopPlayerAudio(GamePad.Index playerNumber)
    {
        foreach (GameObject player in players)
        {
            if (player.GetComponent<InputController>().index == playerNumber)
            {
                playerAS = player.GetComponent<AudioSource>();
            }
        }

        if (playerAS.isPlaying) 
        {
            playerAS.Stop();
        }
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

            case GameData.AudioClipState.PushOthers:
                playerAS.clip = pushClip;
                break;

            case GameData.AudioClipState.Walking:
                playerAS.clip = walkingClip;
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
