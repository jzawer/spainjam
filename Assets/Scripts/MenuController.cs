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
            musicManager.Play(SoundNames.Menu);
            StartCoroutine(musicManager.PlayDelayedBySound(SoundNames.Menu, SoundNames.MenuLoop));
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
                musicManager.Stop(SoundNames.Menu);
                musicManager.Stop(SoundNames.MenuLoop);
            }
        });
    }
}
