using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject Rewards;

    public List<Door> Doors = new List<Door>();
    public List<GameObject> Enemies = new List<GameObject>();


    private void Awake()
    {
        if (Rewards != null)
            Rewards.SetActive(false);

        if (Enemies.Count != 0)
        {
            foreach (GameObject g in Enemies)
            {
                g.SetActive(false);
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].GetComponent<isDead>().dead)
                Enemies.RemoveAt(i);
        }

        if (Enemies.Count == 0)
        {
            foreach (Door d in Doors)
            {
                d.Open();
            }
            if (Rewards != null)
            {
                if (Rewards.active == false)
                    Rewards.SetActive(true);
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (Door d in Doors)
            {
                d.Close();
            }
            foreach (GameObject g in Enemies)
            {
                g.SetActive(true);
            }
        }
    }
}
