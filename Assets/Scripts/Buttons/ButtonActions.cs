using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    [SerializeField]
    private GameEvents buttonEvents;

    public void OnPlayGameButton()
    {
        buttonEvents.Emit(new PlayButtonEvent());
    }

    public void OnOpenOptionsButton()
    {
        buttonEvents.Emit(new OptionsButtonEvent());
    }

    public void OnResumeButton()
    {
        buttonEvents.Emit(new ResumeButtonEvent());
    }

    public void OnToMainMenuButton()
    {
        buttonEvents.Emit(new ToMainMenuButtonEvent());
    }

    public void OnExitButton()
    {
        buttonEvents.Emit(new ExitButtonEvent());
    }
}
