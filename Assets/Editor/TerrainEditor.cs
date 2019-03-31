using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class TerrainEditor : Editor
{
    private static Vector3 _handlerPos = Vector3.zero;

    private static Vector3 _prevHandlerPos = Vector3.zero;
    private static Camera _cam;
    private static RaycastHit _hit;

    public static int SelectedTool
    {
        
        get { return EditorPrefs.GetInt("SelectedEditorTool", 0); }
        set
        {
            if (value == SelectedTool)
                return;
            
            EditorPrefs.SetInt("SelectedEditorTool", value);
            Tools.hidden = value != 0;
        }
    }

    static TerrainEditor()
    {
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    private static void DrawTools(Rect pos)
    {
        Handles.BeginGUI();

        GUILayout.BeginArea(new Rect(0, pos.height - 35, pos.width, 20), EditorStyles.toolbar);
        {
            string[] buttonLabels = new string[] {"Off", "Erase", "Paint"};
            SelectedTool = GUILayout.SelectionGrid(SelectedTool, buttonLabels, 3, EditorStyles.toolbarButton,
                GUILayout.Width(300));
        }
        GUILayout.EndArea();
        Handles.EndGUI();
    }

    private static void OnSceneGUI(SceneView view)
    {
        DrawTools(view.position);
        if (SelectedTool != 0)
        {
            if (Event.current == null) return;

            var mousePos = Event.current.mousePosition;
            var ray = HandleUtility.GUIPointToWorldRay(mousePos);

            if (Physics.Raycast(ray, out _hit))
            {
                _handlerPos = _hit.point;
                Handles.DrawWireCube(_handlerPos, new Vector3(50, 50, 50));
//                Debug.Log(_handlerPos);
//                if (Event.current.type == EventType.MouseDown &&
//                    Event.current.button == 0 &&
//                    Event.current.alt == false &&
//                    Event.current.shift == false &&
//                    Event.current.control == false)
//                {
//                    if ()
//                }
//                //            Tools.hidden = true;
////            _paintMaterial.SetVector("_Coordinate", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
////            var temp = RenderTexture.GetTemporary(_heightMap.width, _heightMap.height, 0,
////                RenderTextureFormat.ARGBFloat);
////            Graphics.Blit(_heightMap, temp);
////            Graphics.Blit(temp, _heightMap, _paintMaterial);
////            RenderTexture.ReleaseTemporary(temp);
//            }
//
//                if (_handlerPos != _prevHandlerPos)
//                {
//                    SceneView.RepaintAll();
//                    _prevHandlerPos = _handlerPos;
//                }
//            }
            }
        }
    }
}