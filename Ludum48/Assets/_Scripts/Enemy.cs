using System.Collections;
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
    public float ReloadCACTime = 1;
    public Transform Visuel;
    public Transform PointA;
    public Transform PointB;
    public GameObject spawnBullet;
    public GameObject Bullet;

    public AudioSource source;
    public AudioClip shot;
    public AudioClip attack;
    public AudioClip death;
    public AudioClip hit;
    public AudioClip Laugth;

    Vector3 target;
    bool targetA = false;
    GameObject Player;
    Coroutine Charge;
    bool charging = false;
    bool attacking = false;
    bool canAttack = true;
    bool isDead = false;

    public Animator animator;

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
            if (!attacking && !isDead)
            {
                
                if ((target - transform.position).magnitude < 0.1f)
                {
                    if (targetA)
                    {
                        target = PointB.position;
                        targetA = false;
                        Visuel.LookAt(new Vector3(target.x, transform.position.y, target.z));
                    }
                    else
                    {
                        target = PointA.position;
                        targetA = true;
                        Visuel.LookAt(new Vector3(target.x, transform.position.y, target.z));
                    }
                    
                }
                transform.Translate((target - transform.position).normalized * speed * Time.deltaTime);
            }
            else
            {
                Vector3 lookat = new Vector3(Player.transform.position.x, Visuel.position.y , Player.transform.position.z);
                Visuel.LookAt(lookat);
            }
        }
        else
        {
            Vector3 lookat = new Vector3(Player.transform.position.x, Visuel.position.y, Player.transform.position.z);
            Visuel.LookAt(lookat);
            RaycastHit hit;
            bool raycast = Physics.Raycast(transform.position, Player.transform.position - transform.position, out hit);
            if (raycast && (Player.transform.position - transform.position).magnitude < Range && hit.transform.tag == "Player")
            {
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
        source.PlayOneShot(shot);
        var inst = Instantiate(Bullet, spawnBullet.transform.position, Quaternion.identity);
        Vector3 dir = (Player.transform.position -transform.position).normalized;
        dir.y = 0;
        inst.GetComponent<Bullet>().Direction = dir;
    }

    public void Damage(float value)
    {
        Debug.Log("Took " + value + " Damage");
        Life -= value;
        if (animator != null && !isDead)
        {
            animator.SetTrigger("Hit");
            source.PlayOneShot(hit);
        }
        if (Life <= 0)
        {
            //attacking = true;
            if (animator != null)
            {
                source.PlayOneShot(death);
                animator.SetTrigger("isDead");
                isDead = true;
                StartCoroutine(Die());
            }
            else
            {
                Destroy(gameObject);
                transform.parent.GetComponent<isDead>().dead = true;
            }
            
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        transform.parent.GetComponent<isDead>().dead = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && canAttack && !RangedEnemy && !isDead)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        attacking = true;
        canAttack = false;
        if (animator != null)
            animator.SetTrigger("Attack");
        source.PlayOneShot(attack);
        Player.GetComponent<CharacterController>().Damage();
        if (Player.GetComponent<CharacterController>().Life == 0)
        {
            if (animator != null)
                animator.SetBool("Dance", true);
            source.PlayOneShot(Laugth);
        }
        else
        {
            yield return new WaitForSeconds(1);
            if (Life > 0)
                attacking = false;
            Visuel.LookAt(new Vector3(target.x, transform.position.y, target.z));
            StartCoroutine(ReloadAttack());
        }
    }

    IEnumerator ReloadAttack()
    {
        yield return new WaitForSeconds(ReloadCACTime);
        canAttack = true;
    }

}

