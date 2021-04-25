using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Buttons : MonoBehaviour
{
    public float transition = .2f;


    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void OnPointerEnter()
    {
        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), transition);
        source.Play();
    }

    public void OnPointerExit()
    {
        transform.DOScale(new Vector3(1f, 1f, 1f), transition);
    }
}
