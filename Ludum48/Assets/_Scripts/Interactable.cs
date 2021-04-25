using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool canInteract = true;

    private void Awake()
    {
        canInteract = true;    
    }

    public virtual void Interact()
    {
        canInteract = false;
    }
}
