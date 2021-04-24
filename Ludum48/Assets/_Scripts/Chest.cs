using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    CharacterController player;
    public enum item { Key, BigKey, Life, Sword}
    public item Gift;
    public bool traped = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }

    public override void Interact()
    {
        base.Interact();
        switch (Gift)
        {
            case item.Key:
                {
                    player.keys++;
                    break;
                }
            case item.BigKey:
                {
                    player.BigKeys++;
                    break;
                }
            case item.Life:
                {
                    player.Life++;
                    if (player.Life > 3)
                        player.Life = 3;
                    break;
                }
            case item.Sword:
                {
                    player.attackZone.GetComponent<AttackZone>().Ekip(true);
                    break;
                }
        }
    }
}
