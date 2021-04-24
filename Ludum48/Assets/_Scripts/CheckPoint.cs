using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : Interactable
{
    public override void Interact()
    {
        base.Interact();
        CharacterController player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        player.LastCheckPoint = player.transform.position;
    }
}
