using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LockedBigDoor : Interactable
{
    //public int LittleKeyPrice = 1;
    public int BigKeyPrice = 1;

    public float AnimSpeed = .5f;

    public Animator animatorRight;
    public Animator animatorLeft;

    public AudioSource source;
    public AudioClip open;

    private void Awake()
    {

    }

    public override void Interact()
    {
        CharacterController player = GameObject.Find("Player").GetComponent<CharacterController>();
        /*if (player.keys >= LittleKeyPrice)
        {
            player.keys -= LittleKeyPrice;
            Open();
            canInteract = false;
        }*/
        if (player.BigKeys >= BigKeyPrice)
        {
            player.BigKeys -= BigKeyPrice;
            Open();
            canInteract = false;
        }
    }

    public void Open()
    {
        animatorRight.SetTrigger("OpenDoor");
        animatorLeft.SetTrigger("OpenDoor");
        source.PlayOneShot(open);
    }
}
