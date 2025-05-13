using System.Collections;
using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    [SerializeField]
    private GameEvents buttonEvents;

    [SerializeField]
    private AudioSource soundPlayer;

    public void OnPlayGameButton()
    {
        soundPlayer.Play();
        StartCoroutine(PlayButtonCoroutine());
    }

    private IEnumerator PlayButtonCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        buttonEvents.Emit(new PlayButtonEvent());
    }

    public void OnOpenOptionsButton()
    {
        soundPlayer.Play();
        StartCoroutine(OptionsButtonCoroutine());
    }

    private IEnumerator OptionsButtonCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        buttonEvents.Emit(new OptionsButtonEvent());
    }

    public void OnResumeButton()
    {
        soundPlayer.Play();
        StartCoroutine(ResumeButtonCoroutine());
    }

    private IEnumerator ResumeButtonCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        buttonEvents.Emit(new ResumeButtonEvent());
    }

    public void OnToMainMenuButton()
    {
        soundPlayer.Play();
        StartCoroutine(MainMenuButtonCoroutine());
    }

    private IEnumerator MainMenuButtonCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        buttonEvents.Emit(new ToMainMenuButtonEvent());
    }

    public void OnExitButton()
    {
        soundPlayer.Play();
        StartCoroutine(ExitButtonCoroutine());
    }

    private IEnumerator ExitButtonCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        buttonEvents.Emit(new ExitButtonEvent());
    }
}
