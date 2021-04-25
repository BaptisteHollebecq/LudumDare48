using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LockedDoor : Interactable
{
    public int LittleKeyPrice = 1;
    public int BigKeyPrice = 1;

    public float AnimSpeed = .5f;

    public Animator animator;

    public AudioSource source;
    public AudioClip open;

    private void Awake()
    {

    }

    public override void Interact()
    {
        CharacterController player = GameObject.Find("Player").GetComponent<CharacterController>();
        if (player.keys >= LittleKeyPrice)
        {
            player.keys -= LittleKeyPrice;
            Open();
            canInteract = false;
        }
        else if (player.BigKeys >= BigKeyPrice)
        {
            player.BigKeys -= BigKeyPrice;
            Open();
            canInteract = false;
        }
    }

    public void Open()
    {
        animator.SetTrigger("OpenDoor");
        source.PlayOneShot(open);
    }
}
