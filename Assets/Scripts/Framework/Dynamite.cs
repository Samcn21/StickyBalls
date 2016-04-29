using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dynamite : MonoBehaviour
{
    private PipeMan pipeMan;
    private AudioSource audioSource;

    [SerializeField]
    private GameObject explosionPrefab;
    public void Initialize(GameData.Coordinate coord, int rotationAngle)
    {
        pipeMan = GameController.Instance.PipeMan;
        GetComponent<MeshRenderer>().material = pipeMan.pipeTextures[PipeData.PipeType.Dynamite];
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        if (!audioSource.isPlaying)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }



}