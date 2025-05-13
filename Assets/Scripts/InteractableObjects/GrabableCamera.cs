using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableCamera : MonoBehaviour
{
    [SerializeField]
    private GameEvents interactionEvents;

    [SerializeField]
    private int frequency;

    [SerializeField]
    private GameEvents cameraToPlayerEvents;

    [SerializeField]
    private GameEvents gameplayEvents;

    [SerializeField]
    private Animator cameraAnimator;

    private bool isCameraActive;

    private void Start()
    {
        isCameraActive = false;
    }

    private void OnEnable()
    {
        cameraToPlayerEvents.AddListener(CameraToPlayerEventsCallback);
        interactionEvents.AddListener(InteractionEventsCallback);
    }

    private void OnDisable()
    {
        cameraToPlayerEvents.RemoveListener(CameraToPlayerEventsCallback);
        interactionEvents.RemoveListener(InteractionEventsCallback);
    }

    private void InteractionEventsCallback(object data)
    {
        if (data is ActivateSignalEvent)
        {
            if ((data as ActivateSignalEvent).signal == frequency)
            {
                if (!isCameraActive) SetAnimation();
                isCameraActive = true;
            }
        }
    }

    private void CameraToPlayerEventsCallback(object data)
    {
        if (data is NotifyRemoteReplayHasStopEvent) StartSequence();
    }

    public bool IsCameraActive() { return isCameraActive; }

    private void SetAnimation()
    {
        cameraAnimator.SetBool("isOpened", true);
    }

    public void StartSequence()
    {
        gameplayEvents.Emit(new InitiateRecordingEvent());
    }

    public void Interact()
    {
        gameplayEvents.Emit(new TakeCameraEvent());
        Destroy(gameObject);
    }
}
