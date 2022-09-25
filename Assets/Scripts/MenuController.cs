using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private ScenesManager scenesManager;
    private MusicManager musicManager;
    public CanvasGroup canvas;

    private void Start()
    {
        scenesManager = FindObjectOfType<ScenesManager>();
        musicManager = MusicManager.Instance;

        if (musicManager != null)
        {
            musicManager.Play(SoundNames.StartMenu_Start);
            StartCoroutine(musicManager.PlayDelayedBySound(SoundNames.StartMenu_Start, SoundNames.StartMenu_Loop));
        }
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
        canvas.DOFade(0, .5f).OnComplete(() =>
        {
            if (musicManager != null)
            {
                musicManager.Stop(SoundNames.StartMenu_Start);
                musicManager.Stop(SoundNames.StartMenu_Loop);
            }
        });
    }
}
