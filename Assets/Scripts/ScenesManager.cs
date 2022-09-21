using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

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
        //if (SceneManager.GetActiveScene().buildIndex == 0)
        //    return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Win()
    {
        int targetBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (targetBuildIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(targetBuildIndex);
        else
            SceneManager.LoadScene(0);
    }
}
