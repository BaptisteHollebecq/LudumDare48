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
    public Image SwordBackGround;
    public Image PlaceHolder;
    public Image Transition;
    public Image BackToMenu;
    public float timingShowSprite;
    public float timingFade;

    [Header("Sprites")]
    public Sprite ThreeLife;
    public Sprite TwoLife;
    public Sprite OneLife;
    public Sprite ZeroLife;
    public List<Sprite> SwordsAnimation = new List<Sprite>();

    CharacterController player;
    List<Image> trousseau = new List<Image>();

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<CharacterController>();

        Life.sprite = ThreeLife;
        Keys.enabled = false;
        BossKeys.enabled = false;
        Sword.enabled = false;
        SwordBackGround.enabled = false;
        PlaceHolder.enabled = false;
        BackToMenu.enabled = false;
    }

    public void SetSword()
    {
        if (player.zone.sword)
        {
            Sword.enabled = true;
            SwordBackGround.enabled = true;

            Sword.sprite = SwordsAnimation[player.zone.durability - 1];
            Debug.Log(player.zone.durability);

        }
        else
        {
            SwordBackGround.enabled = false;
            Sword.enabled = false;
        }
    }

    public void SetLife()
    {
        Life.transform.DOScale(new Vector3(5, 5, 5), .5f);
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
                i++;
            }
        }

        if (player.BigKeys != 0)
            BossKeys.enabled = true;
        else
            BossKeys.enabled = false;
    }

    public void ShowSprite(Sprite sp)
    {
        PlaceHolder.sprite = sp;
        PlaceHolder.enabled = true;
        PlaceHolder.DOFade(1, timingShowSprite);
    }

    public void HideSprite()
    {
        PlaceHolder.DOFade(0, timingShowSprite);
        PlaceHolder.sprite = null;
        PlaceHolder.enabled = false;
    }

    public void TransiOut()
    {
        Transition.DOFade(0, timingFade).OnComplete(() =>
        {
            Transition.enabled = false;
        });
    }

    public void TransiIn()
    {
        Transition.enabled = true;
        Transition.DOFade(1, timingFade);
    }

    public void BackToTheMenu(float timing)
    {
        BackToMenu.enabled = true;
        BackToMenu.DOFade(1, .5f).OnComplete(() =>
        {
            StartCoroutine(ResetMenu(timing));
        });
    }

    IEnumerator ResetMenu(float t)
    {
        yield return new WaitForSeconds(t);
        BackToMenu.DOFade(0, .5f).OnComplete(() =>
        {
            BackToMenu.enabled = false;
        });
    }

}
