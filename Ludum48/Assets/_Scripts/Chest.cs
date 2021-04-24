using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    CharacterController player;
    public enum item { Key, BigKey, Life, Sword }
    public item Gift;
    public bool traped = false;
    public List<GameObject> Enemies = new List<GameObject>();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        if (traped)
        {
            foreach (GameObject g in Enemies)
            {
                g.SetActive(false);
            }
        }
    }

    public override void Interact()
    {
        base.Interact();
        if (!traped)
        {
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
        else
        {
            foreach (GameObject g in Enemies)
            {
                g.SetActive(true);
            }
        }
    }
}
