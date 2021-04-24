using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    public bool sword = true;
    [Header("AttackWithSword")]
    public float SwordDamage = 10;
    public int SwordDurability = 15;
    public Vector3 SwordZoneSize;
    public Vector3 SwordZonePos;
    [Header("AttackWithHand")]
    public float HandDamage = 5;
    public Vector3 HandZoneSize;
    public Vector3 HandZonePos;

    List<GameObject> Targets = new List<GameObject>();

    [HideInInspector] public bool weapon;
    [HideInInspector] public int durability;
    BoxCollider box;
    float damage;
    CharacterController parent;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        parent = transform.parent.parent.GetComponent<CharacterController>();

        SetUp();
    }

    public void Ekip(bool newWeapon)
    {
        if (weapon)
        {
            sword = true;
        }
        if (newWeapon)
        {
            sword = true;
            durability = SwordDurability;
            weapon = true;
            SetUp();
        }
    }

    public void Alamano()
    {
        sword = false;
        SetUp();
    }

    public void SetUp()
    {
        if (sword)
        {
            box.center = SwordZonePos;
            box.size = SwordZoneSize;
            damage = SwordDamage;
        }
        else
        {
            box.center = HandZonePos;
            box.size = HandZoneSize;
            damage = HandDamage;
        }
    }

    public void Attack()
    {
        foreach (GameObject g in Targets)
        {
            g.GetComponent<Enemy>().Damage(damage);
        }

        if (Targets.Count != 0 && sword)
            durability--;
        if (durability <= 0)
        {
            sword = false;
            weapon = false;
        }

        Targets.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Targets.Add(other.gameObject);
        }
        if (other.tag == "Wall")
        {
            parent.BoostLight();
        }
        if (other.tag == "Torch")
        {
            Light l = other.transform.GetChild(0).GetComponent<Light>();
            if (l.enabled == false)
                l.enabled = true;
        }
    }


}
