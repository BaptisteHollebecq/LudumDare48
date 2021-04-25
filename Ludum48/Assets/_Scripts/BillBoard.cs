using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private void Update()
    {
        Vector3 dir = (transform.position - Camera.main.transform.position).normalized;

        transform.forward = dir;
        Debug.DrawRay(transform.position, dir, Color.red);

    }
}
