using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    public CanvasGroup canvasGroup;
    public float FadeDuration = 2f;

    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestartLevel();
    }

    void RestartLevel()
    {
        // don't restart level if we are on the main menu
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Win()
    {
        var musicManager = MusicManager.Instance;
        if (musicManager)
            musicManager.Play(SoundNames.Level_Completed);

        int targetBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (targetBuildIndex < SceneManager.sceneCountInBuildSettings)
            FadeToLevel(targetBuildIndex);
        else
            FadeToLevel(1);
    }

     public void FadeToLevel(int levelIndex)
    {
        canvasGroup.DOFade(1f, FadeDuration / 2).OnComplete(() =>
        {
            canvasGroup.DOFade(0f, FadeDuration / 2);
            SceneManager.LoadScene(levelIndex);
        });
    }
}
