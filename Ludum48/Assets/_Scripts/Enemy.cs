﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float Life;
    public float MaxLife = 10;
    public float speed = 2;
    public bool RangedEnemy = false;
    public float Range = 5;
    public float ChargingTime = 5;
    public float ReloadTime = 1;
    public Transform PointA;
    public Transform PointB;
    public GameObject Bullet;

    Vector3 target;
    bool targetA = false;
    GameObject Player;
    Coroutine Charge;
    bool charging = false;

    private void Awake()
    {
        Player = GameObject.Find("Player");
        Life = MaxLife;
    }

    private void Start()
    {
        if (!RangedEnemy)
        {
            transform.position = PointA.position;
            target = PointB.position;
        }
    }

    private void Update()
    {
        if (!RangedEnemy)
        {
            transform.Translate((target - transform.position).normalized * speed * Time.deltaTime);
            if ((target - transform.position).magnitude < 0.01f)
            {
                if (targetA)
                {
                    target = PointB.position;
                    targetA = false;
                }
                else
                {
                    target = PointA.position;
                    targetA = true;
                }
            }
        }
        else
        {
            RaycastHit hit;
            bool raycast = Physics.Raycast(transform.position, Player.transform.position - transform.position, out hit);
            if (raycast && (Player.transform.position - transform.position).magnitude < Range && hit.transform.tag == "Player")
            {
                transform.LookAt(Player.transform.position);
                if (!charging)
                    Charge = StartCoroutine(ShotCharging());
            }
            else
            {
                if (charging)
                {
                    charging = false;
                    StopCoroutine(Charge);
                }
            }
        }
    }

    IEnumerator ShotCharging()
    {
        charging = true;
        yield return new WaitForSeconds(ChargingTime);
        Shot();
        yield return new WaitForSeconds(ReloadTime);
        charging = false;
    }

    public void Shot()
    {

        var inst = Instantiate(Bullet, transform.GetChild(0).position, Quaternion.identity);
        inst.GetComponent<Bullet>().Direction = (Player.transform.position - transform.position).normalized;
    }

    public void Damage(float value)
    {
        Debug.Log("Took " + value + " Damage");
        Life -= value;
        //PTITE ANIM CHOUPIX DE DEGAT
        if (Life <= 0)
        {
            Destroy(gameObject); // PTITE ANIM CHOUPIX DE MORT
            transform.parent.GetComponent<isDead>().dead = true;
        }
    }
}
