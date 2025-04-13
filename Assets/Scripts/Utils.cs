using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utils
{
    public enum Scenes { MENU = 0, GAME = 1, TEST = 2 };

    static public void ChangeScene(Scenes nextScene)
    {
        SceneManager.LoadScene((int)nextScene, LoadSceneMode.Single);
    }

    static public void ExitGame()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
