using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    public List<Interactable> inRange = new List<Interactable>();
    public bool gotSomething = false;

    public void Interact()
    {
        foreach(Interactable i in inRange)
        {
            if (i.canInteract)
                i.Interact();
        }
    }

    private void Update()
    {
        if (inRange.Count != 0)
            gotSomething = true;
        else
            gotSomething = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interaction")
        {
            Interactable i = other.GetComponent<Interactable>();
            if (i.canInteract)
                inRange.Add(i);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interaction")
        {
            Interactable i = other.GetComponent<Interactable>();
            if (inRange.Contains(i))
                inRange.Remove(i);
        }
    }
}
