using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public bool Menu3D = false;
    public GameObject CanvasBackGround;
    public GameObject CanvasStart;
    public GameObject CanvasMenu;
    public GameObject CanvasCredit;

    [Header("IF 3D MENU " + "\u2713")]
    public Transform CamPosPlay;
    public Transform CamPosMenu;
    public Transform CamPosCredit;
    public Transform CamPosQuit;

    Animator startAnimator;
    Animator menuAnimator;
    Animator creditAnimator;

    Camera cam;
    bool start = false;

    private void Awake()
    {
        cam = Camera.main;

        startAnimator = CanvasStart.GetComponent<Animator>();
        menuAnimator = CanvasMenu.GetComponent<Animator>();
        creditAnimator = CanvasCredit.GetComponent<Animator>();

        if (Menu3D)
        {
            CanvasBackGround.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !start)
        {
            start = true;
            startAnimator.SetTrigger("CanvasStartFadeOut");
            menuAnimator.SetTrigger("CanvasMenuFadeIn");
        }
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
