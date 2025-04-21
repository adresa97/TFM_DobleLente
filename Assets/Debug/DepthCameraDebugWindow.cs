using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DepthCameraDebugWindow : EditorWindow
{
    private GameEvents depthCameraEvents;

    [MenuItem("Window/Depth Camera/Events Debug Window")]

    public static void ShowExample()
    {
        DepthCameraDebugWindow wnd = GetWindow<DepthCameraDebugWindow>();
        wnd.titleContent = new GUIContent("DepthCameraDebugWindow");
    }

    public void OnEnable()
    {
        depthCameraEvents = (GameEvents)Resources.FindObjectsOfTypeAll(typeof(GameEvents)).Where((obj) => obj.name == "FromPlayerManagerEvents").First();
    }

    public void OnGUI()
    {
        if (!Application.isPlaying)
        {
            EditorGUILayout.LabelField("Solo activo durante la ejecución");
            return;
        }

        EditorGUILayout.LabelField("Selecciona el canal de eventos");
        depthCameraEvents = (GameEvents)EditorGUILayout.ObjectField(depthCameraEvents, typeof(GameEvents), false);

        GUILayout.Space(10);

        EditorGUILayout.LabelField("Selecciona el evento a mandar");
        if (GUILayout.Button("Activar Camara"))
        {
            if (depthCameraEvents != null) depthCameraEvents.Emit(new StopRecordingEvent());
        }
        if (GUILayout.Button("Desactivar Camara"))
        {
            if (depthCameraEvents != null) depthCameraEvents.Emit(new ForceStopReplayEvent());
        }
        if (GUILayout.Button("Activar Preview"))
        {
            if (depthCameraEvents != null) depthCameraEvents.Emit(new InitiatePreviewEvent());
        }
        if (GUILayout.Button("Desactivar Preview"))
        {
            if (depthCameraEvents != null) depthCameraEvents.Emit(new CancelPreviewEvent());
        }
    }
}
