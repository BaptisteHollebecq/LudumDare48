using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSprit : MonoBehaviour
{
    public bool Show = true;
    
    public Sprite sprite;

    CharacterController player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Show)
                player.Hud.ShowSprite(sprite);
            else
                player.Hud.HideSprite();
            
        }
    }
}
