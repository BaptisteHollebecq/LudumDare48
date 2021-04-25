using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    public Transform Visuel;
    public Collider col;

    public float AnimSpeed = .5f;

    private void Awake()
    {
        col.enabled = false;
        Visuel.localScale = new Vector3(1, 0, 1);
    }

    public void Close()
    {
        col.enabled = true;
        Visuel.DOScaleY(1, AnimSpeed);
    }

    public void Open()
    {

        Visuel.DOScaleY(0, AnimSpeed).OnComplete(() =>
        {
            col.enabled = false;
        });
    }

}
