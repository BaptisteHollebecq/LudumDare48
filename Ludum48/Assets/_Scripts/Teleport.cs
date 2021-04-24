using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Interactable
{

    public GameObject TargetTeleport;

    public override void Interact()
    {
        CharacterController.Instance.transform.position = new Vector3(TargetTeleport.transform.position.x, 1, TargetTeleport.transform.position.z);
    }
}
