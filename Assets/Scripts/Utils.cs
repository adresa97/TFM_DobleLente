using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utils
{
    public enum Scenes { MENU = 0, GAME = 1, INTRO = 2, CREDITS = 3, TEST = 4 };

    public enum ControlScheme { KEYBOARD = 0, GAMEPAD = 1 };

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
