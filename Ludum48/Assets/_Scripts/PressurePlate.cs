using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject Reward;

    bool used = false;

    private void Awake()
    {
        Reward.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!used)
            {
                Reward.SetActive(true);
                used = true;
            }
        }
    }
}
