﻿using UnityEngine;
using System.Collections;
using GamepadInput;

public class PushMechanic : MonoBehaviour {
    [SerializeField]
    private float pushForce;
    [SerializeField]
    private bool stealMechanicActive;
    [SerializeField]
    private GameObject particleEffect;
    private Rigidbody body;
    private Player playerRef;
    private AudioManager AudioManager;
    private GamePad.Index gamepadIndex;
    private Vector3 lastPosition = Vector3.zero;
    public float speed { get; private set; }
    private bool bounced;
    void Awake()
    {
        bounced = false;
        body = GetComponent<Rigidbody>();
        playerRef = GetComponent<Player>();
        AudioManager = GameObject.FindObjectOfType<AudioManager>();
        gamepadIndex = GetComponent<InputController>().index;  
    }
   
    void FixedUpdate()
    {
        speed = (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            if (col.gameObject.GetComponent<InputController>().team == GetComponent<InputController>().team && GetComponent<InputController>().team != GameData.Team.Neutral)
                return;
            if (!bounced)
            {
                Rigidbody opponentBody = col.gameObject.GetComponent<Rigidbody>();
                Instantiate(particleEffect, Vector3.Lerp(transform.position, col.transform.position, 0.5f), Quaternion.identity);

                AudioManager.PlayOneShotPlayer(GameData.AudioClipState.PushOthers, gamepadIndex, false);
                if (speed < col.gameObject.GetComponent<PushMechanic>().speed)
                {
                    Vector3 direction = (transform.position - col.transform.position);
                    direction.y = 0;
                    body.AddForce(direction * pushForce, ForceMode.Impulse);
                }
                else
                {
                    Vector3 direction = (col.transform.position - transform.position);
                    direction.y = 0;
                    opponentBody.AddForce(direction * pushForce, ForceMode.Impulse);
                    if (stealMechanicActive)
                        stealCarryingPipe(col.gameObject);
                }
            }
        }
    }

    void OnCollisionStay(Collision col)
    {
        if(col.gameObject.tag== "Player")
        {
            Vector3 midPoint = Vector3.Lerp(transform.position, col.transform.position, 0.5f);
            if (col.gameObject.GetComponent<InputController>().team == GetComponent<InputController>().team)
            {
                col.gameObject.GetComponent<Rigidbody>().AddForce((col.transform.position - midPoint) * pushForce/2, ForceMode.Impulse);
                GetComponent<Rigidbody>().AddForce((transform.position - midPoint) * pushForce/2, ForceMode.Impulse);

            }
            else
            {
                col.gameObject.GetComponent<Rigidbody>().AddForce((col.transform.position - midPoint) * pushForce, ForceMode.Impulse);
                GetComponent<Rigidbody>().AddForce((transform.position - midPoint) * pushForce, ForceMode.Impulse);
            }
        }
    }

    private void stealCarryingPipe(GameObject opponent)
    {
        Player opponentPlayerRef = opponent.GetComponent<Player>();
        playerRef.PickupPipe(opponentPlayerRef.HeldPipeType, 0);
        opponentPlayerRef.PickupPipe(PipeData.PipeType.Void, 0);
    }
}
