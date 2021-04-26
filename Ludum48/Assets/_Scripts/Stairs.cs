using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stairs : Interactable
{
    public int SceneIndex;


    public override void Interact()
    {
        base.Interact();
        GameObject.Find("Player").GetComponent<CharacterController>().Hud.TransiIn();
        GameObject.Find("Player").GetComponent<CharacterController>().Hud.HideSprite();
        SceneManager.LoadScene(SceneIndex);
        if (SceneIndex == 0)
            Destroy(GameObject.Find("Player"));

    }
}
