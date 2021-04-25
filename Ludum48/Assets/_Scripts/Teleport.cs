using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Interactable
{
    public GameObject TargetTeleport;

    GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    public override void Interact()
    {
        player.transform.position = new Vector3(TargetTeleport.transform.position.x, TargetTeleport.transform.position.y, TargetTeleport.transform.position.z);
    }
}
