using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HUD : MonoBehaviour
{
    [Header("PlaceHolders")]
    public Image Life;
    public Image Keys;
    public Image BossKeys;
    public Image Sword;

    [Header("Sprites")]
    public Sprite ThreeLife;
    public Sprite TwoLife;
    public Sprite OneLife;
    public Sprite ZeroLife;

    CharacterController player;
    List<Image> trousseau = new List<Image>();

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<CharacterController>();

        Life.sprite = ThreeLife;
        Keys.enabled = false;
        BossKeys.enabled = false;
    }

    public void SetLife()
    {
        Life.transform.DOScale(new Vector3(5f, 5, 5), .5f);
        if (player.Life == 3)
            Life.sprite = ThreeLife;
        if (player.Life == 2)
            Life.sprite = TwoLife;
        if (player.Life == 1)
            Life.sprite = OneLife;
        if (player.Life == 0)
            Life.sprite = ZeroLife;
        Life.transform.DOScale(new Vector3(1, 1, 1), 1f);
    }

    public void SetKeys()
    {
        if (player.keys == 0)
        {
            Keys.enabled = false;
            foreach (Image img in trousseau)
            {
                Destroy(img);
            }
            trousseau.Clear();
        }
        else
        {
            foreach (Image img in trousseau)
            {
                Destroy(img);
            }
            trousseau.Clear();
            Keys.enabled = true;
            int i = 1;
            while (i < player.keys)
            {
                var inst = Instantiate(Keys);
                inst.transform.localPosition = new Vector3(inst.transform.localPosition.x + 30, inst.transform.localPosition.y, inst.transform.localPosition.z);
            }
        }

        if (player.BigKeys != 0)
            BossKeys.enabled = true;
        else
            BossKeys.enabled = false;
    }
}
