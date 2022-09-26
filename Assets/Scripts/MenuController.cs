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
            Invoke(nameof(StartLoopingMusic), 69f);

            musicManager.Play(SoundNames.StartMenu_Start);
            StartCoroutine(musicManager.PlayDelayedBySound(SoundNames.StartMenu_Start, SoundNames.StartMenu_Loop));
        }
    }

    public void StartGame()
    {
        HideMenu();

        musicManager.Play(SoundNames.StartMenu_Play);

        scenesManager.FadeToLevel(1);
    }

    void StartLoopingMusic()
    {
        musicManager.Play(SoundNames.StartMenu_Loop);
    }

    public void ExitGame()
    {
        HideMenu();
        scenesManager.FadeToLevel(SceneManager.GetActiveScene().buildIndex);

        musicManager.Play(SoundNames.StartMenu_Quit);

        Invoke(nameof(QuitAplication), .5f);
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

    void QuitAplication()
    {
        Application.Quit();
    }
}
