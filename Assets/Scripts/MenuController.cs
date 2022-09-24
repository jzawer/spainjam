using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private ScenesManager scenesManager;
    public CanvasGroup canvas;

    private void Awake()
    {
        scenesManager = FindObjectOfType<ScenesManager>();
    }


    public void StartGame()
    {
        HideMenu();
        scenesManager.FadeToLevel(1);
    }

    public void StartCredits()
    {
        HideMenu();
        scenesManager.FadeToLevel(SceneManager.sceneCountInBuildSettings);
    }

    private void HideMenu()
    {
        canvas.DOFade(0, .5f);
    }
}
