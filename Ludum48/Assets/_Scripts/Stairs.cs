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
        GameObject.Find("Player").GetComponent<CharacterController>().Hud.HideSprite();
        GameObject.Find("Player").GetComponent<CharacterController>().Hud.TransiIn();
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(GameObject.Find("Player").GetComponent<CharacterController>().Hud.timingFade);
        SceneManager.LoadScene(SceneIndex);
        if (SceneIndex == 0)
            Destroy(GameObject.Find("Player"));
    }
}
