using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Interactable
{

    public GameObject TargetTeleport;

    public override void Interact()
    {
        CharacterController.Instance.transform.position = new Vector3(TargetTeleport.transform.position.x, TargetTeleport.transform.position.y, TargetTeleport.transform.position.z);
    }
}
