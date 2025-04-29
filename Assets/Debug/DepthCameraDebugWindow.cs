using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DepthCameraDebugWindow : EditorWindow
{
    private GameEvents depthCameraEvents;
    private int lastGridSelection, newGridSelection;
    private string[] worldOptions;

    [MenuItem("Window/Depth Camera/Depth Camera Debug Window")]

    public static void ShowExample()
    {
        DepthCameraDebugWindow wnd = GetWindow<DepthCameraDebugWindow>();
        wnd.titleContent = new GUIContent("DepthCameraDebugWindow");
    }

    public void OnEnable()
    {
        worldOptions = new string[] {"Mundo Real", "Mundo Oculto"};
        newGridSelection = (int)Shader.GetGlobalFloat("_IsDepthCameraPreview");
        lastGridSelection = newGridSelection;
        depthCameraEvents = (GameEvents)Resources.FindObjectsOfTypeAll(typeof(GameEvents)).Where((obj) => obj.name == "FromPlayerManagerEvents").First();
    }

    public void OnGUI()
    {
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 14,
            fontStyle = FontStyle.Bold
        };

        GUILayout.Space(10);

        if (!Application.isPlaying)
        {
            EditorGUILayout.LabelField("Control de visualizador en el editor", titleStyle);

            GUILayout.Space(10);

            EditorGUILayout.LabelField("Selecciona el mundo a mostrar");
            newGridSelection = GUILayout.SelectionGrid(lastGridSelection, worldOptions, 2);
            if (newGridSelection != lastGridSelection)
            {
                Shader.SetGlobalFloat("_IsDepthCameraPreview", newGridSelection);
                lastGridSelection = newGridSelection;
            }
        }
        else
        {
            EditorGUILayout.LabelField("Enviar eventos de la cámara en ejecución", titleStyle);

            GUILayout.Space(10);

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

            if (newGridSelection != 0)
            {
                newGridSelection = 0;
                lastGridSelection = 0;
            }
        }
    }
}
