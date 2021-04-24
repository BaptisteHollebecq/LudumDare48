using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Persistent : MonoBehaviour
{
    public static event System.Action hasSpawned;
    private static Persistent _instance;
    public static Persistent Instance
    {
        get { return _instance; }
    }

    public CharacterController player = null;

    public bool playerSword;
    public bool playerHasWeapon;
    public int swordDurability;
    public int playerLife;
    public Vector3 LastCheckPoint;

    public bool isDead = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        
    }

    private void Start()
    {
        hasSpawned?.Invoke();
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        hasSpawned?.Invoke();
    }

    public void SetUpCharacter(CharacterController value)
    {
        player = value;
        Debug.Log("setup");
        player.Life = playerLife;
        player.zone.weapon = playerHasWeapon;
        player.zone.sword = playerSword;
        player.zone.durability = swordDurability;
        player.zone.SetUp();
        if (isDead)
        {
            player.Life = 1;
            player.transform.position = LastCheckPoint;
            isDead = false;
        }
        else
            LastCheckPoint = player.transform.position;
    }

    public void SaveCharacter()
    {
        playerLife = player.Life;
        playerHasWeapon = player.zone.weapon;
        playerSword = player.zone.sword;
        swordDurability = player.zone.durability;
    }
}
