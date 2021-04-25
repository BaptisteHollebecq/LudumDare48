using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public bool Menu3D = false;
    public GameObject CanvasBackGround;
    public GameObject CanvasStart;
    public GameObject CanvasMenu;
    public GameObject CanvasCredit;

    public List<Buttons> buttons = new List<Buttons>();

    [Header("IF 3D MENU " + "\u2713")]
    public Transform CamPosPlay;
    public Transform CamPosMenu;
    public Transform CamPosCredit;
    public Transform CamPosQuit;

    [Header("Sounds Settings")]
    public AudioClip OveredClip;
    public AudioClip PressedClip;

    Animator startAnimator;
    Animator menuAnimator;
    Animator creditAnimator;

    Camera cam;
    AudioSource source;
    bool start = false;
    bool canSwicth = true;
    public bool isOnMenu = false;
    public bool isOnCredit = false;

    int index = 0;

    private void Awake()
    {
        cam = Camera.main;
        source = GetComponent<AudioSource>();

        startAnimator = CanvasStart.GetComponent<Animator>();
        menuAnimator = CanvasMenu.GetComponent<Animator>();
        creditAnimator = CanvasCredit.GetComponent<Animator>();

        if (Menu3D)
        {
            CanvasBackGround.SetActive(false);
        }
        buttons[0].OnPointerEnter();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (!start)
            {
                start = true;
                isOnMenu = true;
                startAnimator.SetTrigger("CanvasStartFadeOut");
                menuAnimator.SetTrigger("CanvasMenuFadeIn");
            }
            else if (isOnMenu)
            {
                if (index == 0)
                    PlayButton();
                if (index == 1)
                    CreditsButton();
                if (index == 2)
                    QuitButton();
                source.PlayOneShot(PressedClip);
            }
            else if (isOnCredit)
            {
                BackButton();
                source.PlayOneShot(PressedClip);
            }
        }
        if (Input.GetButtonDown("Rolls") && isOnCredit)
        {
            BackButton();
            source.PlayOneShot(PressedClip);
        }
        if (Input.GetAxis("Vertical") == 1 && isOnMenu && canSwicth)
        {
            buttons[index].OnPointerExit();
            index--;
            if (index < 0)
                index = buttons.Count - 1;
            buttons[index].OnPointerEnter();
            StartCoroutine(ResetSwitch());
        }
        if (Input.GetAxis("Vertical") == -1 && isOnMenu && canSwicth)
        {
            buttons[index].OnPointerExit();
            index++;
            if (index == buttons.Count)
                index = 0;
            buttons[index].OnPointerEnter();
            StartCoroutine(ResetSwitch());
        }
    }

    IEnumerator ResetSwitch()
    {
        canSwicth = false;
        yield return new WaitForSeconds(.2f);
        canSwicth = true;
    }

    public void PlayButton()
    {
        if (Menu3D)
        {
            menuAnimator.SetTrigger("CanvasMenuFadeOut");
            cam.transform.DOKill();
            cam.transform.DOMove(CamPosPlay.position, 0.5f);
            cam.transform.DORotate(CamPosPlay.rotation.eulerAngles, 0.5f).OnComplete(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            });
        }
        else
        {
            menuAnimator.SetTrigger("CanvasMenuFadeOut");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void CreditsButton()
    {   
        menuAnimator.SetTrigger("CanvasMenuFadeOut");
        creditAnimator.SetTrigger("CanvasCreditFadeIn");
        isOnCredit = true;
        isOnMenu = false;
        if (Menu3D)
        {
            cam.transform.DOKill();
            cam.transform.DOMove(CamPosCredit.position, 0.5f);
            cam.transform.DORotate(CamPosCredit.rotation.eulerAngles, 0.5f);
        }
    }

    public void BackButton()
    {
        menuAnimator.SetTrigger("CanvasMenuFadeIn");
        creditAnimator.SetTrigger("CanvasCreditFadeOut");
        isOnCredit = false;
        isOnMenu = true;
        if (Menu3D)
        {
            cam.transform.DOKill();
            cam.transform.DOMove(CamPosMenu.position, 0.5f);
            cam.transform.DORotate(CamPosMenu.rotation.eulerAngles, 0.5f);
        }
    }

    public void QuitButton()
    {
        if (Menu3D)
        {
            menuAnimator.SetTrigger("CanvasMenuFadeOut");
            cam.transform.DOKill();
            cam.transform.DOMove(CamPosQuit.position, 0.5f);
            cam.transform.DORotate(CamPosQuit.rotation.eulerAngles, 0.5f).OnComplete(() =>
            {
                Application.Quit();
            });
        }
        else
        {
            menuAnimator.SetTrigger("CanvasMenuFadeOut");
            Application.Quit();
        }
    }
}
