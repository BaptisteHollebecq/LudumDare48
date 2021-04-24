using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LockedDoor : Interactable
{
    public int LittleKeyPrice = 1;
    public int BigKeyPrice = 1;
    public Transform Visuel;
    public Collider col;

    public float AnimSpeed = .5f;

    private void Awake()
    {

    }

    public override void Interact()
    {
        CharacterController player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        if (player.keys >= LittleKeyPrice)
        {
            player.keys -= LittleKeyPrice;
            Open();
            canInteract = false;
        }
        if (player.BigKeys >= BigKeyPrice)
        {
            player.BigKeys -= BigKeyPrice;
            Open();
            canInteract = false;
        }
    }

    public void Open()
    {
        Visuel.DOScaleY(0, AnimSpeed).OnComplete(() =>
        {
            col.enabled = false;
        });
    }
}
