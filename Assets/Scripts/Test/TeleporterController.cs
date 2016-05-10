﻿using UnityEngine;
using System.Collections;

public class TeleporterController : MonoBehaviour
{
    public GameData.Direction teleporterDirection;
    public bool isTeleporter = true;
    public Material teleporter;
    public Material wall;

    private GameObject teleporterNorth;
    private GameObject teleporterSouth;
    private GameObject teleporterEast;
    private GameObject teleporterWest;
    private float teleportOffset;

    void Start()
    {
        teleportOffset = 3f;
        teleporterNorth = GameObject.Find("TeleporterNorth");
        teleporterSouth = GameObject.Find("TeleporterSouth");
        teleporterEast = GameObject.Find("TeleporterEast");
        teleporterWest = GameObject.Find("TeleporterWest");

        if (isTeleporter)
        {
            this.GetComponent<BoxCollider>().isTrigger = true;
            this.GetComponent<Renderer>().material = teleporter;
        }
        else
        {
            this.GetComponent<BoxCollider>().isTrigger = false;
            this.GetComponent<Renderer>().material = wall;
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (teleporterDirection)
            {
                case GameData.Direction.North:
                    if (teleporterSouth != null)
                    {
                        other.transform.position = new Vector3(teleporterSouth.transform.position.x, other.transform.position.y, teleporterSouth.transform.position.z + teleportOffset);
                    }
                    break;
                case GameData.Direction.South:
                    if (teleporterNorth != null)
                    {
                        other.transform.position = new Vector3(teleporterNorth.transform.position.x, other.transform.position.y, teleporterNorth.transform.position.z - teleportOffset);
                    }
                    break;
                case GameData.Direction.East:
                    if (teleporterWest != null)
                    {
                        other.transform.position = new Vector3(teleporterWest.transform.position.x + teleportOffset, other.transform.position.y, teleporterWest.transform.position.z);
                    }
                    break;
                case GameData.Direction.West:
                    if (teleporterEast != null)
                    {
                        other.transform.position = new Vector3(teleporterEast.transform.position.x - teleportOffset, other.transform.position.y, teleporterEast.transform.position.z);
                    }
                    break;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {

    }
}