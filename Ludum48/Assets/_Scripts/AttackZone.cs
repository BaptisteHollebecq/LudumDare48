using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    public CharacterController player;
    public BoxCollider box;
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
    float damage;
    CharacterController parent;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
    }

    private void Start()
    {

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
        player.Hud.SetSword();
    }

    public void Alamano()
    {
        sword = false;

        SetUp();

        player.Hud.SetSword();
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
        player.Hud.SetSword();
        Targets.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (!Targets.Contains(other.gameObject))
                Targets.Add(other.gameObject);
        }
        else if (other.tag == "Torch")
        {
            if (sword)
            {

                Light l = other.transform.GetChild(0).GetComponent<Light>();
                if (l.enabled == false)
                    l.enabled = true;
                other.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Allumage");
            }
        }
        else if (other.tag == "Wall")
        {
            if (sword)
            {
                player.BoostLight();
                durability--;
                if (durability <= 0)
                {
                    sword = false;
                    weapon = false;
                }
                player.Hud.SetSword();
            }
        }
    }


}
