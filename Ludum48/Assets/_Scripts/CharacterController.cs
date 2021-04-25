﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    private static CharacterController _instance;
    public static CharacterController Instance
    {
        get { return _instance; }
    }

    public GameObject Visuel;
    public int Life = 3;
    public float MoveSpeed = 10;
    public float AttackDuration = .5f;
    public float AttackReloadTime = .05f;
    public float DashInvincibility = .1f;
    public float DashReloadTime = .2f;

    public GameObject attackZone;
    public GameObject InteractZone;
    public GameObject InteractSprite;
    [HideInInspector] public AttackZone zone;
    InteractionZone Izone;

    public Collider collision;
    public LayerMask Walls;

    [Header("LightProperties")]
    public Light playerLight;
    public float hitWallMultiplier = 3;
    public float timeResetLamp = .5f;
    [HideInInspector] public bool boosted = false;


    public int keys = 0;
    public int BigKeys = 0;

    float speed;
    float horizontalAxis;
    float verticalAxis;
    Vector3 InputDirection;
    Rigidbody _rb;
    bool attacking = false;
    bool canAttack = true;
    bool canDash = true;
    bool canBeHit = true;

    bool isDead = false;
    [HideInInspector] public Vector3 LastCheckPoint = new Vector3(0,1,0);

    [Header("Animation")]
    public Animator animator;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        _rb = GetComponent<Rigidbody>();
        zone = attackZone.GetComponent<AttackZone>();
        Izone = InteractZone.GetComponent<InteractionZone>();
        InteractSprite.SetActive(false);
        attackZone.SetActive(false);
    }

    private void OnLevelWasLoaded(int level)
    {
        transform.position = new Vector3(0, 1, 0);
        keys = 0;
        BigKeys = 0;
        if (isDead)
        {
            Life = 1;
            isDead = false;
        }

    }

    private void Update()
    {
        animator.SetBool("Running", false);

        if (Izone.gotSomething && InteractSprite.active == false)
            InteractSprite.SetActive(true);
        else if (!Izone.gotSomething && InteractSprite.active == true)
            InteractSprite.SetActive(false);


        speed = MoveSpeed;
        if (!attacking)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            camForward.y = 0;
            camForward = camForward.normalized;
            camRight.y = 0;
            camRight = camRight.normalized;

            InputDirection = camRight * Input.GetAxis("Horizontal") + camForward * Input.GetAxis("Vertical");
            if (InputDirection.x > .25f)
                InputDirection.x = 1;
            else if (InputDirection.x < -.25f)
                InputDirection.x = -1;
            else InputDirection.x = 0;

            if (InputDirection.z > .25f)
                InputDirection.z = 1;
            else if (InputDirection.z < -.25f)
                InputDirection.z = -1;
            else InputDirection.z = 0;

            InputDirection = InputDirection.normalized;

            animator.SetBool("Running", true);
        }
        else
            InputDirection = Vector3.zero;


        if (Input.GetButtonDown("Attack") && !attacking && canAttack)
            StartCoroutine(Attack());

        if (Input.GetButtonDown("Rolls") && canDash && InputDirection != Vector3.zero)
        {
            StopAllCoroutines();
            if (!Physics.Raycast(transform.position, transform.forward, 3, Walls))
                _rb.AddForce(InputDirection * 125, ForceMode.VelocityChange);
            StartCoroutine(Dash());
        }

        if (Input.GetButtonDown("Switch"))
        {
            if (zone.sword)
                zone.Alamano();
            else
                zone.Ekip(false);
        }

        if (Input.GetButtonDown("Interact"))
        {
            Izone.Interact();
            InteractSprite.SetActive(false);
            if (Izone.inRange.Count != 0)
                Izone.inRange.RemoveAt(0);
        }


        _rb.velocity = InputDirection * speed;
        if (InputDirection != Vector3.zero)
            Visuel.transform.forward = InputDirection.normalized;
    }

    IEnumerator Dash()
    {
        canDash = false;
        attacking = false;
        canAttack = true;
        collision.enabled = false;
        yield return new WaitForSeconds(DashInvincibility);
        collision.enabled = true;
        yield return new WaitForSeconds(DashReloadTime);
        canDash = true;
    }

    IEnumerator Attack()
    {
        attacking = true;
        canAttack = false;
        attackZone.SetActive(true);
        zone.SetUp();
        yield return new WaitForSeconds(AttackDuration * 0.1f);
        zone.Attack();
        yield return new WaitForSeconds(AttackDuration * 0.9f);
        attackZone.SetActive(false);
        attacking = false;
        StartCoroutine(AttackReload());
    }

    IEnumerator AttackReload()
    {
        yield return new WaitForSeconds(AttackReloadTime);
        canAttack = true;
    }

    public void Damage()
    {
        Life--;
        canBeHit = false;

        if (Life == 0)
        {
            collision.enabled = false;
            attacking = true;
            StartCoroutine(Respawn());
        }
        else
        {
            collision.enabled = false;
            StartCoroutine(Invincibility());
        }
    }

    IEnumerator Respawn()
    {
        //anim HUD
        isDead = true;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return new WaitForSeconds(1);
        attacking = false;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Enemy" && canBeHit)
        {
            Damage();
        }
    }

    public IEnumerator Invincibility()
    {
        yield return new WaitForSeconds(1);
        canBeHit = true;
        collision.enabled = true;
    }

    public void BoostLight()
    {
        if (!boosted)
        {
            boosted = true;

            playerLight.range *= hitWallMultiplier;
            StartCoroutine(ResetLight());
        }
    }
    IEnumerator ResetLight()
    {
        yield return new WaitForSeconds(timeResetLamp);
        playerLight.range /= hitWallMultiplier;
        boosted = false;
    }
}